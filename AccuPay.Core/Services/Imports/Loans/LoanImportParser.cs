using AccuPay.Core.Entities;
using AccuPay.Core.Interfaces;
using AccuPay.Core.Interfaces.Excel;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace AccuPay.Core.Services.Imports.Loans
{
    public class LoanImportParser : ILoanImportParser
    {
        private readonly IEmployeeRepository _employeeRepository;
        private readonly IExcelParser<LoanRowRecord> _parser;
        private readonly IProductRepository _productRepository;
        private const string WORKSHEETNAME = "Default";

        public string XlsxExtension => _parser.XlsxExtension;

        public LoanImportParser(
            ILoanRepository loanRepository,
            IEmployeeRepository employeeRepository,
            IProductRepository productRepository,
            IExcelParser<LoanRowRecord> excelParser)
        {
            _employeeRepository = employeeRepository;
            _parser = excelParser;
            _productRepository = productRepository;
        }

        public Task<LoanImportParserOutput> Parse(Stream importFile, int organizationId)
        {
            var parsedRecords = _parser.Read(importFile, WORKSHEETNAME);
            if (!parsedRecords.Any())
            {
                var emptyList = new List<LoanImportModel>();

                return Task.Run(() => new LoanImportParserOutput(emptyList, emptyList));
            }

            return Validate(parsedRecords, organizationId);
        }

        private async Task<LoanImportParserOutput> Validate(IList<LoanRowRecord> parsedRecords, int organizationId)
        {
            string[] employeeNumberList = parsedRecords.Select(s => s.EmployeeNo).ToArray();

            var employees = (await _employeeRepository
                            .GetByMultipleEmployeeNumberAsync(employeeNumberList, organizationId)
                            ).ToList();

            var loanTypes = (await _productRepository.GetLoanTypesAsync(organizationId)
                            ).ToList();

            var list = new List<LoanImportModel>();

            bool isEqualToLowerCase(string dataText, string parsedText) => string.IsNullOrWhiteSpace(parsedText) ? false : dataText.ToLower() == parsedText.ToLower();

            foreach (var loan in parsedRecords)
            {
                var employee = employees
                    .Where(ee => isEqualToLowerCase(ee.EmployeeNo, loan.EmployeeNo))
                    .FirstOrDefault();

                var loanType = loanTypes
                    .Where(lt => isEqualToLowerCase(lt.PartNo, loan.LoanName))
                    .FirstOrDefault();

                list.Add(CreateLoanImportModel(loan, employee, loanType));
            }

            bool isInvalidToSave(LoanImportModel x) => x.InvalidToSave;

            var validRecords = list.Where(l => !isInvalidToSave(l)).ToList();
            var invalidRecords = list.Where(isInvalidToSave).ToList();

            foreach (var invalidRecord in invalidRecords)
            {
                invalidRecord.DescribeError();
            }

            return new LoanImportParserOutput(validRecords: validRecords,
                                              invalidRecords: invalidRecords);
        }

        private LoanImportModel CreateLoanImportModel(LoanRowRecord loan, Employee employee, Product loanType)
        {
            return new LoanImportModel(loan, employee, loanType);
        }
    }

    public class LoanImportParserOutput
    {
        public IReadOnlyCollection<LoanImportModel> ValidRecords { get; }

        public IReadOnlyCollection<LoanImportModel> InvalidRecords { get; }

        public LoanImportParserOutput(IReadOnlyCollection<LoanImportModel> validRecords,
                                        IReadOnlyCollection<LoanImportModel> invalidRecords)
        {
            ValidRecords = validRecords;
            InvalidRecords = invalidRecords;
        }
    }
}
