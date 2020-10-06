using AccuPay.Data.Entities;
using AccuPay.Data.Helpers;
using AccuPay.Data.Interfaces.Excel;
using AccuPay.Data.Repositories;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace AccuPay.Data.Services.Imports
{
    public class ShiftImportParser
    {
        private readonly EmployeeRepository _employeeRepository;
        private readonly IExcelParser<ShiftScheduleRowRecord> _parser;

        private const string WorkSheetName = "ShiftSchedule";

        public ShiftImportParser(EmployeeRepository employeeRepository, IExcelParser<ShiftScheduleRowRecord> parser)
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

        private async Task<ShiftImportParserOutput> Validate(IList<ShiftScheduleRowRecord> parsedRecords, int organizationId)
        {
            string[] employeeNumberList = parsedRecords.Select(s => s.EmployeeNo).ToArray();

            var employees = (await _employeeRepository
                                        .GetByMultipleEmployeeNumberAsync(employeeNumberList, organizationId)
                                        ).ToList();

            var list = new List<ShiftImportModel>();

            foreach (var shiftSched in parsedRecords)
            {
                if (shiftSched.StartDate == DateTime.MinValue) continue;

                var employee = employees
                            .Where(ee => ee.EmployeeNo == shiftSched.EmployeeNo)
                            .FirstOrDefault();

                var endDate = shiftSched.EndDate.HasValue ? shiftSched.EndDate.Value : shiftSched.StartDate;
                var dates = CalendarHelper.EachDay(shiftSched.StartDate, endDate);

                list.AddRange(CreateShiftImportModel(shiftSched, dates, employee));
            }

            bool isValid(ShiftImportModel x) => x.IsValidToSave;

            var validRecords = list.Where(isValid).ToList();
            var invalidRecords = list.Where(ssm => !isValid(ssm)).ToList();

            return new ShiftImportParserOutput(validRecords: validRecords,
                                                invalidRecords: invalidRecords);
        }

        private List<ShiftImportModel> CreateShiftImportModel(ShiftScheduleRowRecord shiftSched,
                                                        IEnumerable<DateTime> dates,
                                                        Employee employee)
        {
            var list = new List<ShiftImportModel>();

            foreach (var date in dates)
            {
                list.Add(new ShiftImportModel(employee)
                {
                    Date = date,
                    BreakTime = shiftSched.BreakStartTime,
                    BreakLength = shiftSched.BreakLength,
                    IsRestDay = shiftSched.IsRestDay,
                    StartTime = shiftSched.StartTime,
                    EndTime = shiftSched.EndTime
                });
            }

            return list;
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