using AccuPay.Core.Entities;
using AccuPay.Core.Interfaces.Excel;
using AccuPay.Core.Repositories;
using AccuPay.Core.ValueObjects;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace AccuPay.Core.Services.Imports.Overtimes
{
    public class OvertimeImportParser
    {
        private readonly EmployeeRepository _employeeRepository;
        private readonly IExcelParser<OvertimeRowRecord> _parser;
        private readonly OvertimeRepository _overtimeRepository;
        private const string WORKSHEETNAME = "Sheet1";

        public string XlsxExtension => _parser.XlsxExtension;

        public OvertimeImportParser(
            EmployeeRepository employeeRepository,
            OvertimeRepository overtimeRepository,
            IExcelParser<OvertimeRowRecord> parser)
        {
            _employeeRepository = employeeRepository;
            _overtimeRepository = overtimeRepository;
            _parser = parser;
        }

        public Task<OvertimeImportParserOutput> Parse(Stream importFile, int organizationId)
        {
            var parsedRecords = _parser.Read(importFile, WORKSHEETNAME);
            if (!parsedRecords.Any())
            {
                var emptyList = new List<OvertimeImportModel>();

                return Task.Run(() => new OvertimeImportParserOutput(emptyList, emptyList));
            }

            return Validate(parsedRecords, organizationId);
        }

        private async Task<OvertimeImportParserOutput> Validate(IList<OvertimeRowRecord> parsedRecords, int organizationId)
        {
            string[] employeeNumberList = parsedRecords.GroupBy(t => t.EmployeeNo).Select(t => t.FirstOrDefault().EmployeeNo).ToArray();

            var employees = (await _employeeRepository
                .GetByMultipleEmployeeNumberAsync(employeeNumberList, organizationId)
                ).ToList();

            var list = new List<OvertimeImportModel>();

            bool isEqualToLowerCase(string dataText, string parsedText) => string.IsNullOrWhiteSpace(parsedText) ? false : dataText.ToLower() == parsedText.ToLower();

            var employeeIds = employees.Select(e => e.RowID.Value).ToList();
            var minStartDate = parsedRecords.Where(p => p.StartDate.HasValue).Min(p => p.StartDate.Value);
            var minEndDate = parsedRecords.Where(p => p.StartDate.HasValue).Any() ? parsedRecords.Where(p => p.StartDate.HasValue).Max(p => p.StartDate.Value) : DateTime.UtcNow;
            DateTime[] effectiveDates = { minStartDate, minEndDate };
            minEndDate = effectiveDates.Max();
            var timePeriod = new TimePeriod(minStartDate, minEndDate);

            var overtimes = await _overtimeRepository.GetByEmployeeIdsBetweenDatesAsync(organizationId, employeeIds, timePeriod);

            foreach (var parsedRecord in parsedRecords)
            {
                var employee = employees
                    .Where(ee => isEqualToLowerCase(ee.EmployeeNo, parsedRecord.EmployeeNo))
                    .FirstOrDefault();

                var effectiveDate = parsedRecord.StartDate == null ? minEndDate : parsedRecord.StartDate.Value;

                var overtime = overtimes
                    .Where(a => isEqualToLowerCase(a.Employee.EmployeeNo, parsedRecord.EmployeeNo))
                    .Where(a => a.OTStartDate >= parsedRecord.StartDate)
                    .Where(a => a.OTStartDate <= effectiveDate)
                    .FirstOrDefault();

                list.Add(CreateOvertimeImportModel(parsedRecord, employee, overtime));
            }

            bool isInvalidToSave(OvertimeImportModel x) => x.InvalidToSave;

            var validRecords = list.Where(l => !isInvalidToSave(l)).ToList();
            var invalidRecords = list.Where(isInvalidToSave).ToList();

            foreach (var invalidRecord in invalidRecords)
            {
                invalidRecord.DescribeError();
            }

            return new OvertimeImportParserOutput(
                validRecords: validRecords,
                invalidRecords: invalidRecords);
        }

        private OvertimeImportModel CreateOvertimeImportModel(OvertimeRowRecord parsedRecord, Employee employee, Overtime overtime)
        {
            return new OvertimeImportModel(parsedRecord, employee, overtime);
        }
    }

    public class OvertimeImportParserOutput
    {
        public IReadOnlyCollection<OvertimeImportModel> ValidRecords { get; }

        public IReadOnlyCollection<OvertimeImportModel> InvalidRecords { get; }

        public OvertimeImportParserOutput(
            IReadOnlyCollection<OvertimeImportModel> validRecords,
            IReadOnlyCollection<OvertimeImportModel> invalidRecords)
        {
            ValidRecords = validRecords;
            InvalidRecords = invalidRecords;
        }
    }
}
