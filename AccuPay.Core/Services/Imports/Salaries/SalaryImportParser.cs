using AccuPay.Core.Entities;
using AccuPay.Core.Interfaces;
using AccuPay.Core.Interfaces.Excel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace AccuPay.Core.Services.Imports.Salaries
{
    public class SalaryImportParser
    {
        private readonly ISalaryRepository _salaryRepository;
        private readonly IExcelParser<SalaryRowRecord> _parser;
        private readonly IEmployeeRepository _employeeRepository;
        private const string WORKSHEETNAME = "Employee Salary";

        public string XlsxExtension => _parser.XlsxExtension;

        public SalaryImportParser(ISalaryRepository salaryRepository, IEmployeeRepository employeeRepository, IExcelParser<SalaryRowRecord> excelParser)
        {
            _salaryRepository = salaryRepository;
            _parser = excelParser;
            _employeeRepository = employeeRepository;
        }

        public Task<SalaryImportParserOutput> Parse(Stream importFile, int organizationId)
        {
            var parsedRecords = _parser.Read(importFile, WORKSHEETNAME);
            if (!parsedRecords.Any())
            {
                var emptyList = new List<SalaryImportModel>();

                return Task.Run(() => new SalaryImportParserOutput(emptyList, emptyList));
            }

            return Validate(parsedRecords, organizationId);
        }

        private async Task<SalaryImportParserOutput> Validate(IList<SalaryRowRecord> parsedRecords, int organizationId)
        {
            string[] employeeNumberList = parsedRecords.Select(s => s.EmployeeNo).ToArray();

            var employees = (await _employeeRepository
                            .GetByMultipleEmployeeNumberAsync(employeeNumberList, organizationId)
                            ).ToList();

            var salaries = (await _salaryRepository.GetAllAsync(organizationId)).ToList();

            bool isEqualToLowerCase(string dataText, string parsedText) => string.IsNullOrWhiteSpace(parsedText) ? false : dataText.ToLower() == parsedText.ToLower();

            bool noEffectiveFrom(DateTime effectiveFrom, DateTime? parsedEffectiveFrom) => !parsedEffectiveFrom.HasValue ? false : effectiveFrom == parsedEffectiveFrom.Value;

            var list = new List<SalaryImportModel>();

            foreach (var parsedSalary in parsedRecords)
            {
                var employee = employees
                    .Where(ee => isEqualToLowerCase(ee.EmployeeNo, parsedSalary.EmployeeNo))
                    .FirstOrDefault();

                var employeeId = employee == null ? 0 : employee.RowID.Value;

                var overlappedSalary = salaries
                    .Where(s => s.EmployeeID == employeeId)
                    .Where(s => noEffectiveFrom(s.EffectiveFrom, parsedSalary.EffectiveFrom))
                    .FirstOrDefault();

                list.Add(CreateSalaryImportModel(employee, overlappedSalary, parsedSalary));
            }

            bool isInvalid(SalaryImportModel x) => x.InvalidToSave;

            var validRecords = list.Where(t => !isInvalid(t)).ToList();
            var invalidRecords = list.Where(isInvalid).ToList();

            foreach (var rejectedRecord in invalidRecords)
            {
                rejectedRecord.DescribeErrors();
            }

            return new SalaryImportParserOutput(validRecords: validRecords,
                                                invalidRecords: invalidRecords);
        }

        private SalaryImportModel CreateSalaryImportModel(Employee employee, Salary overlappedSalary, SalaryRowRecord parsedSalary)
        {
            return new SalaryImportModel(employee, overlappedSalary, parsedSalary);
        }

        public class SalaryImportParserOutput
        {
            public IReadOnlyCollection<SalaryImportModel> ValidRecords { get; }

            public IReadOnlyCollection<SalaryImportModel> InvalidRecords { get; }

            public SalaryImportParserOutput(IReadOnlyCollection<SalaryImportModel> validRecords,
                                            IReadOnlyCollection<SalaryImportModel> invalidRecords)
            {
                ValidRecords = validRecords;
                InvalidRecords = invalidRecords;
            }
        }
    }
}
