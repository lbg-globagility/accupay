using AccuPay.Data.Entities;
using AccuPay.Data.Helpers;
using AccuPay.Data.Repositories;
using AccuPay.Infrastructure.Services.Excel;
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
        private readonly ExcelParser<ShiftScheduleRowRecord> _parser;

        public ShiftImportParser(EmployeeRepository employeeRepository)
        {
            _employeeRepository = employeeRepository;

            var workSheetName = "ShiftSchedule";
            _parser = new ExcelParser<ShiftScheduleRowRecord>(workSheetName);
        }

        public string XlsxExtension => ExcelParser<ShiftScheduleRowRecord>.XlsxExtension;

        /// <summary>
        /// Parses a stream into a list of models. This only supports .xlsx files.
        /// Please check first if the file is an .xlsx type. If it is not, please throw an InvalidFormatException.
        /// </summary>
        /// <param name="importFile"></param>
        /// <param name="organizationId"></param>
        /// <returns></returns>
        public Task<ShiftImportParserOutput> Parse(Stream importFile, int organizationId)
        {
            var parsedRecords = _parser.Read(importFile);
            return Validate(parsedRecords, organizationId);
        }

        public Task<ShiftImportParserOutput> Parse(string filePath, int organizationId)
        {
            var parsedRecords = _parser.Read(filePath);
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
                var employee = employees
                            .Where(ee => ee.EmployeeNo == shiftSched.EmployeeNo)
                            .FirstOrDefault();

                var endDate = shiftSched.EndDate.HasValue ? shiftSched.EndDate.Value : shiftSched.StartDate;
                var dates = CalendarHelper.EachDay(shiftSched.StartDate, endDate);

                list.AddRange(CreateShiftImportModel(shiftSched, dates, employee));
            }

            bool isValid(ShiftImportModel x) => x.IsValidToSave && x.IsExistingEmployee;

            var validRecords = list.Where(isValid).ToList();
            var invalidRecords = list.Where(ssm => !isValid(ssm)).ToList();

            foreach (var rejectedRecord in invalidRecords)
            {
                List<string> reasons = new List<string>();

                if (!rejectedRecord.IsValidToSave)
                    reasons.Add("no shift");

                if (!rejectedRecord.IsExistingEmployee)
                    reasons.Add("employee doesn't exists");

                rejectedRecord.Remarks = string.Join("; ", reasons.ToArray());
            }

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
            public IReadOnlyCollection<ShiftImportModel> ValidRecords { get; set; }

            public IReadOnlyCollection<ShiftImportModel> InvalidRecords { get; set; }

            public ShiftImportParserOutput(IReadOnlyCollection<ShiftImportModel> validRecords,
                                            IReadOnlyCollection<ShiftImportModel> invalidRecords)
            {
                ValidRecords = validRecords;
                InvalidRecords = invalidRecords;
            }
        }
    }
}