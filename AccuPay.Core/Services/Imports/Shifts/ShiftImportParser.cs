using AccuPay.Core.Entities;
using AccuPay.Core.Helpers;
using AccuPay.Core.Interfaces;
using AccuPay.Core.Interfaces.Excel;
using AccuPay.Core.Services.Policies;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace AccuPay.Core.Services.Imports
{
    public class ShiftImportParser : IShiftImportParser
    {
        private readonly IEmployeeRepository _employeeRepository;
        private readonly IExcelParser<ShiftRowRecord> _parser;
        private ShiftBasedAutomaticOvertimePolicy _shiftBasedAutoOvertimePolicy;
        private bool _isShiftBasedAutoOvertimePolicyEnabled;
        private const string WorkSheetName = "ShiftSchedule";

        public ShiftImportParser(IEmployeeRepository employeeRepository, IExcelParser<ShiftRowRecord> parser)
        {
            _employeeRepository = employeeRepository;

            _parser = parser;
        }

        public string XlsxExtension => _parser.XlsxExtension;

        /// <summary>
        /// Parses a stream into a list of models. This only supports .xlsx files.
        /// Please check first if the file is an .xlsx type. If it is not, please throw an InvalidFormatException.
        /// </summary>
        /// <param name="importFile"></param>
        /// <param name="organizationId"></param>
        /// <returns></returns>
        public Task<ShiftImportParserOutput> Parse(Stream importFile, int organizationId)
        {
            var parsedRecords = _parser.Read(importFile, WorkSheetName);
            if (!parsedRecords.Any())
            {
                var emptyList = new List<ShiftImportModel>();

                return Task.Run(() => new ShiftImportParserOutput(emptyList, emptyList));
            }

            return Validate(parsedRecords, organizationId);
        }

        public Task<ShiftImportParserOutput> Parse(string filePath, int organizationId)
        {
            var parsedRecords = _parser.Read(filePath, WorkSheetName);
            return Validate(parsedRecords, organizationId);
        }

        private async Task<ShiftImportParserOutput> Validate(IList<ShiftRowRecord> parsedRecords, int organizationId)
        {
            string[] employeeNumberList = parsedRecords.Select(s => s.EmployeeNo).ToArray();

            var employees = (await _employeeRepository
                            .GetByMultipleEmployeeNumberAsync(employeeNumberList, organizationId)
                            ).ToList();

            var list = new List<ShiftImportModel>();

            foreach (var shift in parsedRecords)
            {
                if (shift.StartDate == DateTime.MinValue) continue;

                var employee = employees
                            .Where(ee => ee.EmployeeNo == shift.EmployeeNo)
                            .FirstOrDefault();

                var endDate = shift.EndDate.HasValue ? shift.EndDate.Value : shift.StartDate;
                var dates = CalendarHelper.EachDay(shift.StartDate, endDate);

                list.AddRange(CreateShiftImportModel(shift, dates, employee));
            }

            bool isValid(ShiftImportModel x) => x.IsValidToSave;

            var validRecords = list.Where(isValid).ToList();
            var invalidRecords = list.Where(ssm => !isValid(ssm)).ToList();

            return new ShiftImportParserOutput(validRecords: validRecords,
                                                invalidRecords: invalidRecords);
        }

        private List<ShiftImportModel> CreateShiftImportModel(
            ShiftRowRecord shift,
            IEnumerable<DateTime> dates,
            Employee employee)
        {
            var list = new List<ShiftImportModel>();

            foreach (var date in dates)
            {
                list.Add(new ShiftImportModel(employee, _shiftBasedAutoOvertimePolicy)
                {
                    Date = date,
                    BreakTime = shift.BreakStartTime,
                    BreakLength = shift.BreakLength,
                    IsRestDay = shift.IsRestDay,
                    StartTime = shift.StartTime,
                    EndTime = shift.EndTime
                });
            }

            return list;
        }

        public void SetShiftBasedAutoOvertimePolicy(ShiftBasedAutomaticOvertimePolicy shiftBasedAutoOvertimePolicy)
        {
            _shiftBasedAutoOvertimePolicy = shiftBasedAutoOvertimePolicy;
            _isShiftBasedAutoOvertimePolicyEnabled = _shiftBasedAutoOvertimePolicy != null ? _shiftBasedAutoOvertimePolicy.Enabled : false;
        }

        public class ShiftImportParserOutput
        {
            public IReadOnlyCollection<ShiftImportModel> ValidRecords { get; }

            public IReadOnlyCollection<ShiftImportModel> InvalidRecords { get; }

            public ShiftImportParserOutput(IReadOnlyCollection<ShiftImportModel> validRecords,
                                            IReadOnlyCollection<ShiftImportModel> invalidRecords)
            {
                ValidRecords = validRecords;
                InvalidRecords = invalidRecords;
            }
        }
    }
}
