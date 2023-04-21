using AccuPay.Core.Entities;
using AccuPay.Core.Interfaces;
using AccuPay.CrystalReports.Payslip;
using CrystalDecisions.CrystalReports.Engine;
using PdfSharp.Pdf;
using PdfSharp.Pdf.IO;
using PdfSharp.Pdf.Security;
using System;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace AccuPay.CrystalReports
{
    public class PayslipBuilder : BaseReportBuilder, IPdfGenerator, IPayslipBuilder
    {
        private const string customDateFormat = "M/d/yyyy";

        private const string GoldwingsEmployeeIdColumn = "RowID";

        private const string CinemaEmployeeIdColumn = "EmployeeRowID";

        private const string DefaultEmployeeIdColumn = "RowID";

        private readonly IOrganizationRepository _organizationRepository;

        private readonly IPayPeriodRepository _payPeriodRepository;

        private readonly ISystemOwnerService _systemOwnerService;

        private readonly IPayslipDataService _dataService;

        private DataTable _payslipDatatable;

        private readonly string _currentSystemOwner;

        public PayslipBuilder(
            IOrganizationRepository organizationRepository,
            IPayPeriodRepository payPeriodRepository,
            IPayslipDataService dataService,
            ISystemOwnerService systemOwnerService)
        {
            _organizationRepository = organizationRepository;

            _payPeriodRepository = payPeriodRepository;

            _systemOwnerService = systemOwnerService;
            this._dataService = dataService;
            _currentSystemOwner = _systemOwnerService.GetCurrentSystemOwner();

            _payslipDatatable = new DataTable();
        }

        public bool CheckIfEmployeeExists(int employeeId)
        {
            var payslipList = _payslipDatatable.AsEnumerable();

            if (_currentSystemOwner == SystemOwner.Goldwings)
            {
                return payslipList.Where(x => x.Field<int>(GoldwingsEmployeeIdColumn) == employeeId).Any();
            }
            else if (_currentSystemOwner == SystemOwner.Cinema2000)
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

        public async Task<PayslipBuilder> CreateReportDocumentAsync(int payPeriodId, bool isActual = false, int[] employeeIds = null)
        {
            // Use dapper or entity framework for procedure calls
            var payPeriod = _payPeriodRepository.GetById(payPeriodId);

            if (payPeriod == null || payPeriod.OrganizationID == null)
                throw new Exception("Pay Period or OrganizationID cannot be null");

            var organization = await _organizationRepository.GetByIdWithAddressAsync(payPeriod.OrganizationID.Value);

            var currentSystemOwnerEntity = await _systemOwnerService.GetCurrentSystemOwnerEntityAsync();

            if (_currentSystemOwner == SystemOwner.Goldwings)
            {
                _payslipDatatable = CreateGoldWingsDataSource(
                    organizationId: organization.RowID.Value,
                    payPeriodId: payPeriod.RowID.Value,
                    isActual,
                    employeeIds);
                _reportDocument = CreateGoldWingsReport(organization, payPeriod);
            }
            else if (_currentSystemOwner == SystemOwner.Cinema2000)
            {
                _payslipDatatable = CreateCinemaDataSource(
                    organizationId: organization.RowID.Value,
                    payPeriodId: payPeriod.RowID.Value,
                    employeeIds);
                _reportDocument = CreateCinemaReport(organization, payPeriodId);
            }
            else if (currentSystemOwnerEntity.IsMorningSun)
            {
                _payslipDatatable = CreateMorningSunDataSource(
                    organizationId: organization.RowID.Value,
                    payPeriodId: payPeriod.RowID.Value,
                    isActual,
                    employeeIds);
                _reportDocument = CreateMorningSunReport(organization, payPeriod);
            }
            else
            {
                _payslipDatatable = CreateDefaultDataSource(
                    organizationId: organization.RowID.Value,
                    payPeriodId: payPeriod.RowID.Value,
                    isActual,
                    employeeIds);
                _reportDocument = CreateDefaultReport(organization, payPeriod);
            }

            _reportDocument.SetDataSource(_payslipDatatable);

            return this;
        }

        private ReportClass CreateMorningSunReport(Organization organization, PayPeriod payPeriod)
        {
            var rptdoc = new MorningSunPayslipFormat();

            var txtOrganizName = (TextObject)rptdoc.Section2.ReportObjects["txtOrganizName"];
            txtOrganizName.Text = organization.Name.ToUpper();

            var txtPayPeriod = (TextObject)rptdoc.Section2.ReportObjects["txtPayPeriod"];
            txtPayPeriod.Text = $"{payPeriod.PayFromDate.ToString(customDateFormat)} to {payPeriod.PayToDate.ToString(customDateFormat)}";

            return rptdoc;
        }

        private DataTable CreateMorningSunDataSource(int organizationId, int payPeriodId, bool isActual, int[] employeeIds)
        {
            var result = _dataService.GetMorningSunData(organizationId, payPeriodId, isActual);

            return FilterData(result, DefaultEmployeeIdColumn, employeeIds);
        }

        private ReportClass CreateGoldWingsReport(Organization organization, PayPeriod payPeriod)
        {
            var rptdoc = new OfficialPaySlipFormat();

            var txtOrganizName = (TextObject)rptdoc.ReportDefinition.Sections[2].ReportObjects["txtOrganizName"];
            txtOrganizName.Text = organization.Name.ToUpper();

            var txtPayPeriod = (TextObject)rptdoc.ReportDefinition.Sections[2].ReportObjects["txtPayPeriod"];
            txtPayPeriod.Text = $"{payPeriod.PayFromDate.ToString(customDateFormat)} to {payPeriod.PayToDate.ToString(customDateFormat)}";

            return rptdoc;
        }

        private DataTable CreateGoldWingsDataSource(int organizationId, int payPeriodId, bool isActual, int[] employeeIds)
        {
            var result = _dataService.GetGoldWingsData(organizationId, payPeriodId, isActual);

            return FilterData(result, GoldwingsEmployeeIdColumn, employeeIds);
        }

        private ReportClass CreateCinemaReport(Organization organization, int payPeriodId)
        {
            var rptdoc = new TwoEmpIn1PaySlip();

            var nextPayPeriod = _payPeriodRepository.GetNextPayPeriod(
                payPeriodId: payPeriodId,
                organizationId: organization.RowID.Value);

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
            txtOrgAddress.Text = organization?.Address?.FullAddress ?? "";

            return rptdoc;
        }

        private DataTable CreateCinemaDataSource(int organizationId, int payPeriodId, int[] employeeIds = null)
        {
            var result = _dataService.GetCinema2000Data(organizationId, payPeriodId);

            return FilterData(result, CinemaEmployeeIdColumn, employeeIds);
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

        private DataTable CreateDefaultDataSource(int organizationId, int payPeriodId, bool isActual, int[] employeeIds = null)
        {
            var result = _dataService.GetDefaultData(organizationId, payPeriodId, isActual);

            return FilterData(result, DefaultEmployeeIdColumn, employeeIds);
        }

        private DataTable FilterData(DataTable table, string employeeIdColumn, int[] employeeIds = null)
        {
            if (employeeIds != null && employeeIds.Count() > 0)
                return table.AsEnumerable().Where(r => employeeIds.Contains(r.Field<int>(employeeIdColumn))).CopyToDataTable();
            else
                return table;
        }

        public BaseReportBuilder GeneratePDF(string saveFolderPath, string fileName)
        {
            string pdfFullPath = Path.Combine(saveFolderPath, fileName);

            return GeneratePDF(pdfFullPath);
        }

        public BaseReportBuilder GeneratePDF(string pdfFullPath)
        {
            ExportPDF(pdfFullPath);

            return this;
        }

        public PayslipBuilder AddPdfPassword(string password)
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
