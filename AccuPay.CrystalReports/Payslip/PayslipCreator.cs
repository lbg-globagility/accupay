using Accupay.DB;
using AccuPay.Data.Entities;
using AccuPay.Data.Repositories;
using AccuPay.Data.Services;
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
using PdfSharp.Pdf;
using PdfSharp.Pdf.IO;
using PdfSharp.Pdf.Security;
using System.Data;
using System.IO;
using System.Linq;

namespace AccuPay.CrystalReports.Payslip
{
    public class PayslipCreator
    {
        private const string customDateFormat = "M/d/yyyy";

        private const string GoldwingsEmployeeIdColumn = "RowID";

        private const string CinemaEmployeeIdColumn = "EmployeeRowID";

        private const string DefaultEmployeeIdColumn = "RowID";

        private readonly AddressRepository _addressRepository;

        private readonly OrganizationRepository _organizationRepository;

        private readonly PayPeriodRepository _payPeriodRepository;

        private SystemOwnerService _systemOwnerService;

        private ReportClass _reportDocument;

        private string _pdfFile;

        private DataTable _payslipDatatable;

        private readonly string _currentSystemOwner;

        public PayslipCreator(AddressRepository addressRepository, OrganizationRepository organizationRepository, PayPeriodRepository payPeriodRepository, SystemOwnerService systemOwnerService)
        {
            _addressRepository = addressRepository;

            _organizationRepository = organizationRepository;

            _payPeriodRepository = payPeriodRepository;

            _systemOwnerService = systemOwnerService;

            _currentSystemOwner = _systemOwnerService.GetCurrentSystemOwner();
        }

        public ReportClass GetReportDocument()
        {
            return _reportDocument;
        }

        public string GetPDF()
        {
            return _pdfFile;
        }

        public bool CheckIfEmployeeExists(int employeeId)
        {
            var payslipList = _payslipDatatable.AsEnumerable();

            if (_currentSystemOwner == SystemOwnerService.Goldwings)
            {
                return payslipList.Where(x => x.Field<int>(GoldwingsEmployeeIdColumn) == employeeId).Any();
            }
            else if (_currentSystemOwner == SystemOwnerService.Goldwings)
            {
                return payslipList.Where(x => x.Field<int>(CinemaEmployeeIdColumn) == employeeId).Any();
            }
            else
            {
                return payslipList.Where(x => x.Field<int>(DefaultEmployeeIdColumn) == employeeId).Any();
            }
        }

        public DataRow GetFirstEmployee()
        {
            return _payslipDatatable.Rows.Count > 0 ? _payslipDatatable.Rows[0] : null;
        }

        public PayslipCreator CreateReportDocument(int organizationId, int payPeriodId, sbyte isActual, int[] employeeIds = null)
        {
            // Use dapper or entity framework for procedure calls

            var organization = _organizationRepository.GetById(organizationId);
            var payPeriod = _payPeriodRepository.GetById(payPeriodId);

            var organizationName = organization.Name;

            if (_currentSystemOwner == SystemOwnerService.Goldwings)
            {
                _payslipDatatable = CreateGoldWingsDataSource(organization, payPeriod, isActual, employeeIds);
                _reportDocument = CreateGoldWingsReport(organization, payPeriod);
            }
            else if (_currentSystemOwner == SystemOwnerService.Cinema2000)
            {
                _payslipDatatable = CreateCinemaDataSource(organization, payPeriodId, employeeIds);
                _reportDocument = CreateCinemaReport(organization, payPeriodId);
            }
            else
            {
                _payslipDatatable = CreateDefaultDataSource(organization, payPeriod, isActual, employeeIds);
                _reportDocument = CreateDefaultReport(organization, payPeriod);
            }

            _reportDocument.SetDataSource(_payslipDatatable);

            return this;
        }

        private ReportClass CreateGoldWingsReport(Organization organization, PayPeriod payPeriod)
        {
            OfficialPaySlipFormat rptdoc = new OfficialPaySlipFormat();

            var txtOrganizName = (TextObject)rptdoc.ReportDefinition.Sections[2].ReportObjects["txtOrganizName"];
            txtOrganizName.Text = organization.Name.ToUpper();

            var txtPayPeriod = (TextObject)rptdoc.ReportDefinition.Sections[2].ReportObjects["txtPayPeriod"];
            txtPayPeriod.Text = $"{payPeriod.PayFromDate.ToString(customDateFormat)} to {payPeriod.PayToDate.ToString(customDateFormat)}";

            return rptdoc;
        }

        private DataTable CreateGoldWingsDataSource(Organization organization, PayPeriod payPeriod, sbyte isActual, int[] employeeIds)
        {
            var procedureCall = "CALL paystub_payslip(" + organization.RowID + "," + payPeriod.RowID + "," + isActual + ");";

            return CreateDataSource(procedureCall, GoldwingsEmployeeIdColumn, employeeIds);
        }

        private ReportClass CreateCinemaReport(Organization organization, int payPeriodId)
        {
            Address address = organization?.PrimaryAddressId == null ? null : _addressRepository.GetById(organization.PrimaryAddressId.Value);

            var rptdoc = new TwoEmpIn1PaySlip();

            var nextPayPeriod = _payPeriodRepository.GetNextPayPeriod(payPeriodId);

            if (nextPayPeriod != null)
            {
                var txtPayperiod = (TextObject)rptdoc.ReportDefinition.Sections[2].ReportObjects["payperiod"];
                txtPayperiod.Text = $"Payroll period {nextPayPeriod.PayFromDate.ToShortDateString()}  to {nextPayPeriod.PayToDate.ToShortDateString()}";
            }

            var txtOrgContact = (TextObject)rptdoc.ReportDefinition.Sections[2].ReportObjects["OrgContact"];
            txtOrgContact.Text = string.Empty;

            var txtOrgName = (TextObject)rptdoc.ReportDefinition.Sections[2].ReportObjects["OrgName"];
            txtOrgName.Text = organization.Name.ToUpper();

            var txtOrgAddress = (TextObject)rptdoc.ReportDefinition.Sections[2].ReportObjects["OrgAddress"];
            txtOrgAddress.Text = address?.FullAddress ?? "";

            return rptdoc;
        }

        private DataTable CreateCinemaDataSource(Organization organization, int payPeriodId, int[] employeeIds = null)
        {
            var procedureCall = "CALL RPT_payslip(" + organization.RowID + "," + payPeriodId + ", TRUE, NULL);";

            return CreateDataSource(procedureCall, CinemaEmployeeIdColumn, employeeIds);
        }

        private ReportClass CreateDefaultReport(Organization organization, PayPeriod payperiod)
        {
            DefaultPayslipFormat rptdoc = new DefaultPayslipFormat();

            var txtOrganizName = (TextObject)rptdoc.Section2.ReportObjects["txtOrganizName"];
            txtOrganizName.Text = organization.Name.ToUpper();

            var txtPayPeriod = (TextObject)rptdoc.Section2.ReportObjects["txtPayPeriod"];
            txtPayPeriod.Text = $"{payperiod.PayFromDate.ToString(customDateFormat)} to {payperiod.PayToDate.ToString(customDateFormat)}";

            return rptdoc;
        }

        private DataTable CreateDefaultDataSource(Organization organization, PayPeriod payperiod, sbyte isActual, int[] employeeIds = null)
        {
            var procedureCall = "CALL PrintDefaultPayslip(" + organization.RowID + "," + payperiod.RowID + "," + isActual + ");";

            return CreateDataSource(procedureCall, DefaultEmployeeIdColumn, employeeIds);
        }

        private DataTable CreateDataSource(string procedureCall, string employeeIdColumn, int[] employeeIds = null)
        {
            SQLQueryToDatatable query = new SQLQueryToDatatable(procedureCall);

            if (employeeIds != null && employeeIds.Count() > 0)
                return query.ResultTable.AsEnumerable().Where(r => employeeIds.Contains(r.Field<int>(employeeIdColumn))).CopyToDataTable();
            else
                return query.ResultTable;
        }

        public PayslipCreator GeneratePDF(string saveFolderPath, string fileName)
        {
            string pdfFullPath = Path.Combine(saveFolderPath, fileName);

            ExportOptions CrExportOptions;
            DiskFileDestinationOptions CrDiskFileDestinationOptions = new DiskFileDestinationOptions();
            PdfRtfWordFormatOptions CrFormatTypeOptions = new PdfRtfWordFormatOptions();
            CrDiskFileDestinationOptions.DiskFileName = pdfFullPath;
            CrExportOptions = _reportDocument.ExportOptions;

            CrExportOptions.ExportDestinationType = ExportDestinationType.DiskFile;
            CrExportOptions.ExportFormatType = ExportFormatType.PortableDocFormat;
            CrExportOptions.DestinationOptions = CrDiskFileDestinationOptions;
            CrExportOptions.FormatOptions = CrFormatTypeOptions;

            _reportDocument.Export();

            _pdfFile = pdfFullPath;

            return this;
        }

        public PayslipCreator AddPdfPassword(string password)
        {
            PdfDocument document = PdfReader.Open(_pdfFile);
            PdfSecuritySettings securitySettings = document.SecuritySettings;
            securitySettings.UserPassword = password;
            securitySettings.OwnerPassword = password;
            securitySettings.PermitAccessibilityExtractContent = false;
            securitySettings.PermitAnnotations = false;
            securitySettings.PermitAssembleDocument = false;
            securitySettings.PermitExtractContent = false;
            securitySettings.PermitFormsFill = true;
            securitySettings.PermitFullQualityPrint = false;
            securitySettings.PermitModifyDocument = false;
            securitySettings.PermitPrint = true;
            document.Save(_pdfFile);

            return this;
        }
    }
}