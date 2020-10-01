using AccuPay.Data.Entities;
using AccuPay.Data.Interfaces.Excel;
using AccuPay.Data.Repositories;
using AccuPay.Data.ValueObjects;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace AccuPay.Data.Services.Imports.OfficialBusiness
{
    public class OfficialBusinessImportParser
    {
        private readonly EmployeeRepository _employeeRepository;
        private readonly IExcelParser<OfficialBusinessRowRecord> _parser;
        private readonly OfficialBusinessRepository _officialBusinessRepository;
        private const string WORKSHEETNAME = "Sheet1";

        public string XlsxExtension => _parser.XlsxExtension;

        public OfficialBusinessImportParser(EmployeeRepository employeeRepository,
                                    OfficialBusinessRepository officialBusinessRepository,
                                    IExcelParser<OfficialBusinessRowRecord> parser)
        {
            _employeeRepository = employeeRepository;
            _officialBusinessRepository = officialBusinessRepository;
            _parser = parser;
        }

        public Task<OfficialBusinessImportParserOutput> Parse(Stream importFile, int organizationId)
        {
            var parsedRecords = _parser.Read(importFile, WORKSHEETNAME);
            return Validate(parsedRecords, organizationId);
        }

        private async Task<OfficialBusinessImportParserOutput> Validate(IList<OfficialBusinessRowRecord> parsedRecords, int organizationId)
        {
            string[] employeeNumberList = parsedRecords.GroupBy(t => t.EmployeeNo).Select(t => t.FirstOrDefault().EmployeeNo).ToArray();

            var employees = (await _employeeRepository
                            .GetByMultipleEmployeeNumberAsync(employeeNumberList, organizationId)
                            ).ToList();

            var list = new List<OfficialBusinessImportModel>();

            bool isEqualToLowerCase(string dataText, string parsedText) => string.IsNullOrWhiteSpace(parsedText) ? false : dataText.ToLower() == parsedText.ToLower();

            var employeeIds = employees.Select(e => e.RowID.Value).ToList();
            var minStartDate = parsedRecords.Where(p => p.StartDate.HasValue).Min(p => p.StartDate.Value);
            var minEndDate = parsedRecords.Where(p => p.StartDate.HasValue).Any() ? parsedRecords.Where(p => p.StartDate.HasValue).Max(p => p.StartDate.Value) : DateTime.UtcNow;
            DateTime[] effectiveDates = { minStartDate, minEndDate };
            minEndDate = effectiveDates.Max();
            var timePeriod = new TimePeriod(minStartDate, minEndDate);

            var officialBusinesses = await _officialBusinessRepository.GetByEmployeeIdsBetweenDatesAsync(organizationId, employeeIds, timePeriod);

            foreach (var parsedRecord in parsedRecords)
            {
                var employee = employees
                    .Where(ee => isEqualToLowerCase(ee.EmployeeNo, parsedRecord.EmployeeNo))
                    .FirstOrDefault();

                var effectiveDate = parsedRecord.StartDate == null ? minEndDate : parsedRecord.StartDate.Value;

                var officialBusiness = officialBusinesses
                    .Where(a => isEqualToLowerCase(a.Employee.EmployeeNo, parsedRecord.EmployeeNo))
                    .Where(a => a.StartDate >= parsedRecord.StartDate)
                    .Where(a => a.StartDate <= effectiveDate)
                    .FirstOrDefault();

                list.Add(CreateOfficialBusinessImportModel(parsedRecord, employee, officialBusiness));
            }

            bool isInvalidToSave(OfficialBusinessImportModel x) => x.InvalidToSave;

            var validRecords = list.Where(l => !isInvalidToSave(l)).ToList();
            var invalidRecords = list.Where(isInvalidToSave).ToList();

            foreach (var invalidRecord in invalidRecords)
            {
                invalidRecord.DescribeError();
            }

            return new OfficialBusinessImportParserOutput(validRecords: validRecords,
                                                   invalidRecords: invalidRecords);
        }

        private OfficialBusinessImportModel CreateOfficialBusinessImportModel(OfficialBusinessRowRecord parsedRecord, Employee employee, Entities.OfficialBusiness officialBusiness)
        {
            return new OfficialBusinessImportModel(parsedRecord, employee, officialBusiness);
        }
    }

    public class OfficialBusinessImportParserOutput
    {
        public IReadOnlyCollection<OfficialBusinessImportModel> ValidRecords { get; }

        public IReadOnlyCollection<OfficialBusinessImportModel> InvalidRecords { get; }

        public OfficialBusinessImportParserOutput(IReadOnlyCollection<OfficialBusinessImportModel> validRecords,
                                           IReadOnlyCollection<OfficialBusinessImportModel> invalidRecords)
        {
            ValidRecords = validRecords;
            InvalidRecords = invalidRecords;
        }
    }
}