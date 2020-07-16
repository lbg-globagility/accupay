using AccuPay.Data.Entities;
using AccuPay.Data.Interfaces.Excel;
using AccuPay.Data.Repositories;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace AccuPay.Data.Services.Imports.Employees
{
    public class EmployeeImportParser
    {
        private readonly EmployeeRepository _employeeRepository;
        private readonly IExcelParser<EmployeeRowRecord> _parser;
        private const string WORKSHEETNAME = "Employees";

        public EmployeeImportParser(EmployeeRepository employeeRepository, IExcelParser<EmployeeRowRecord> excelParser)
        {
            _employeeRepository = employeeRepository;

            _parser = excelParser;
        }

        public string XlsxExtension => _parser.XlsxExtension;

        /// <summary>
        /// Parses a stream into a list of models. This only supports .xlsx files.
        /// Please check first if the file is an .xlsx type. If it is not, please throw an InvalidFormatException.
        /// </summary>
        /// <param name="importFile"></param>
        /// <param name="organizationId"></param>
        /// <returns></returns>
        public Task<EmployeeImportParserOutput> Parse(Stream importFile, int organizationId)
        {
            var parsedRecords = _parser.Read(importFile, WORKSHEETNAME);
            return Validate(parsedRecords, organizationId);
        }

        public Task<EmployeeImportParserOutput> Parse(string filePath, int organizationId)
        {
            var parsedRecords = _parser.Read(filePath, WORKSHEETNAME);
            return Validate(parsedRecords, organizationId);
        }

        private async Task<EmployeeImportParserOutput> Validate(IList<EmployeeRowRecord> parsedRecords, int organizationId)
        {
            string[] employeeNumberList = parsedRecords.Select(s => s.EmployeeNo).ToArray();

            var employees = (await _employeeRepository
                            .GetByMultipleEmployeeNumberAsync(employeeNumberList, organizationId)
                            ).ToList();

            var list = new List<EmployeeImportModel>();

            foreach (var parsedEmployee in parsedRecords)
            {
                var employee = employees
                            .Where(ee => ee.EmployeeNo == parsedEmployee.EmployeeNo)
                            .FirstOrDefault();

                list.Add(CreateEmployeeImportModel(employee, parsedEmployee));
            }

            bool isInvalid(EmployeeImportModel x) => x.InvalidToSave;

            var validRecords = list.Where(t => !isInvalid(t)).ToList();
            var invalidRecords = list.Where(isInvalid).ToList();

            foreach (var rejectedRecord in invalidRecords)
            {
                rejectedRecord.DescribeErrors();
            }

            return new EmployeeImportParserOutput(validRecords: validRecords,
                                                  invalidRecords: invalidRecords);
        }

        private EmployeeImportModel CreateEmployeeImportModel(Employee employee, EmployeeRowRecord parsedEmployee)
        {
            return new EmployeeImportModel(employee, parsedEmployee);
        }

        public class EmployeeImportParserOutput
        {
            public IReadOnlyCollection<EmployeeImportModel> ValidRecords { get; }

            public IReadOnlyCollection<EmployeeImportModel> InvalidRecords { get; }

            public EmployeeImportParserOutput(IReadOnlyCollection<EmployeeImportModel> validRecords,
                                              IReadOnlyCollection<EmployeeImportModel> invalidRecords)
            {
                ValidRecords = validRecords;
                InvalidRecords = invalidRecords;
            }
        }
    }
}