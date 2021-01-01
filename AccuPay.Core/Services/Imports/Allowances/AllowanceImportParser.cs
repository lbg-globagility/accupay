using AccuPay.Core.Entities;
using AccuPay.Core.Interfaces.Excel;
using AccuPay.Core.Repositories;
using AccuPay.Core.ValueObjects;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace AccuPay.Core.Services.Imports.Allowances
{
    public class AllowanceImportParser
    {
        private readonly EmployeeRepository _employeeRepository;
        private readonly AllowanceRepository _allowanceRepository;
        private readonly IExcelParser<AllowanceRowRecord> _parser;
        private readonly AllowanceTypeRepository _allowanceTypeRepository;
        private const string WORKSHEETNAME = "Default";

        public string XlsxExtension => _parser.XlsxExtension;

        public AllowanceImportParser(EmployeeRepository employeeRepository,
                                     AllowanceRepository allowanceRepository,
                                     IExcelParser<AllowanceRowRecord> parser,
                                     AllowanceTypeRepository allowanceTypeRepository)
        {
            _employeeRepository = employeeRepository;
            _allowanceRepository = allowanceRepository;
            _parser = parser;
            _allowanceTypeRepository = allowanceTypeRepository;
        }

        public Task<AllowanceImportParserOutput> Parse(Stream importFile, int organizationId)
        {
            var parsedRecords = _parser.Read(importFile, WORKSHEETNAME);
            if (!parsedRecords.Any())
            {
                var emptyList = new List<AllowanceImportModel>();

                return Task.Run(() => new AllowanceImportParserOutput(emptyList, emptyList));
            }

            return Validate(parsedRecords, organizationId);
        }

        private async Task<AllowanceImportParserOutput> Validate(IList<AllowanceRowRecord> parsedRecords, int organizationId)
        {
            string[] employeeNumberList = parsedRecords.Select(s => s.EmployeeNo).ToArray();

            var employees = (await _employeeRepository
                            .GetByMultipleEmployeeNumberAsync(employeeNumberList, organizationId)
                            ).ToList();

            var allowanceTypes = (await _allowanceTypeRepository.GetAllAsync()).ToList();

            var list = new List<AllowanceImportModel>();

            bool isEqualToLowerCase(string dataText, string parsedText) => string.IsNullOrWhiteSpace(parsedText) ? false : dataText.ToLower() == parsedText.ToLower();

            var employeeIds = employees.Select(e => e.RowID.Value).ToList();
            var allowanceTypeNames = parsedRecords.GroupBy(p => p.AllowanceName).Select(p => p.FirstOrDefault().AllowanceName).ToList();
            var minStartDate = parsedRecords.Where(p => p.EffectiveStartDate.HasValue).Min(p => p.EffectiveStartDate.Value);
            var minEndDate = parsedRecords.Where(p => p.EffectiveEndDate.HasValue).Any() ? parsedRecords.Where(p => p.EffectiveEndDate.HasValue).Max(p => p.EffectiveEndDate.Value) : DateTime.UtcNow;
            DateTime[] effectiveDates = { minStartDate, minEndDate };
            minEndDate = effectiveDates.Max();
            var timePeriod = new TimePeriod(minStartDate, minEndDate);

            var allowances = await _allowanceRepository.GetByEmployeeIdsBetweenDatesByAllowanceTypesAsync(employeeIds, allowanceTypeNames, timePeriod);

            foreach (var parsedRecord in parsedRecords)
            {
                var employee = employees
                    .Where(ee => isEqualToLowerCase(ee.EmployeeNo, parsedRecord.EmployeeNo))
                    .FirstOrDefault();

                var allowanceType = allowanceTypes
                    .Where(at => isEqualToLowerCase(at.Name, parsedRecord.AllowanceName) || isEqualToLowerCase(at.DisplayString, parsedRecord.AllowanceName))
                    .FirstOrDefault();

                var effectiveDate = parsedRecord.EffectiveEndDate == null ? minEndDate : parsedRecord.EffectiveEndDate.Value;

                var allowance = allowances
                    .Where(a => isEqualToLowerCase(a.Employee.EmployeeNo, parsedRecord.EmployeeNo))
                    .Where(a => isEqualToLowerCase(a.Type, parsedRecord.AllowanceName))
                    .Where(a => a.EffectiveStartDate >= parsedRecord.EffectiveStartDate)
                    .Where(a => a.EffectiveEndDate <= effectiveDate)
                    .FirstOrDefault();

                list.Add(CreateAllowanceImportModel(parsedRecord, employee, allowance, allowanceType));
            }

            bool isInvalidToSave(AllowanceImportModel x) => x.InvalidToSave;

            var validRecords = list.Where(l => !isInvalidToSave(l)).ToList();
            var invalidRecords = list.Where(isInvalidToSave).ToList();

            foreach (var invalidRecord in invalidRecords)
            {
                invalidRecord.DescribeError();
            }

            return new AllowanceImportParserOutput(validRecords: validRecords,
                                                   invalidRecords: invalidRecords);
        }

        private AllowanceImportModel CreateAllowanceImportModel(AllowanceRowRecord parsedRecord, Employee employee, Allowance allowance, AllowanceType allowanceType)
        {
            return new AllowanceImportModel(parsedRecord, employee, allowance, allowanceType);
        }
    }

    public class AllowanceImportParserOutput
    {
        public IReadOnlyCollection<AllowanceImportModel> ValidRecords { get; }

        public IReadOnlyCollection<AllowanceImportModel> InvalidRecords { get; }

        public AllowanceImportParserOutput(IReadOnlyCollection<AllowanceImportModel> validRecords,
                                           IReadOnlyCollection<AllowanceImportModel> invalidRecords)
        {
            ValidRecords = validRecords;
            InvalidRecords = invalidRecords;
        }
    }
}