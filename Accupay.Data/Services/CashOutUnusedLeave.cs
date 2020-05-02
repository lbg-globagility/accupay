using AccuPay.Data.Entities;
using AccuPay.Data.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace AccuPay.Data.Services
{
    /// <summary>
    /// Not working yet
    /// </summary>
    public class CashOutUnusedLeave
    {
        private const decimal _defaultWorkHours = 8.0M;

        // Private _adjUnusedVacationLeave As String = String.Join(Space(1), "Unused", LeaveType.Vacation.ToString(), "Leaves")
        private string _adjUnusedVacationLeave = "Unused Leaves";

        private string _adjUnusedSickLeave = $"Unused {LeaveType.Sick.ToString()} Leaves";

        private bool _isVLOnly, _isSLOnly, _asAdjustment;

        private int _adjUnusedVacationLeaveId, _currentPeriodId, _vacationLeaveId;
        private readonly int _payPeriodFromId;
        private readonly int _payPeriodToId;
        private readonly int _organizationId;
        private readonly int _userId;
        private DataTable _leaveLedger;
        private CategoryRepository _categoryRepository;
        private const string strAdjType = "Adjustment Type";

        private const string strLeaveType = "Leave Type";

        private EmployeeRepository _employeeRepository;

        private ListOfValueRepository _listOfValRepository;

        private PayPeriodRepository _payPeriodRepository;

        public CashOutUnusedLeave(int PayPeriodFromId, int PayPeriodToId, int CurrentPeriodID, int organizationId, int userId)
        {
            _payPeriodFromId = PayPeriodFromId;
            _payPeriodToId = PayPeriodToId;
            _currentPeriodId = CurrentPeriodID;
            _organizationId = organizationId;
            _userId = userId;

            _categoryRepository = new CategoryRepository();

            _employeeRepository = new EmployeeRepository();

            _listOfValRepository = new ListOfValueRepository();

            _payPeriodRepository = new PayPeriodRepository();

            var listOfValues = _listOfValRepository.GetLeaveConvertiblePolicies();

            var _leaveType = listOfValues.Where(l => l.LIC == "LeaveType").FirstOrDefault();

            _isVLOnly = _leaveType.DisplayValue == LeaveType.Vacation.ToString();

            _isSLOnly = _leaveType.DisplayValue == LeaveType.Sick.ToString();

            var _amountTreatment = listOfValues.Where(l => l.LIC == "AmountTreatment").FirstOrDefault();

            _asAdjustment = _amountTreatment.DisplayValue == AmountTreatment.Adjustment.ToString();

            // Dim policy = _settings.GetSublist("LeaveConvertiblePolicy")

            // _isVLOnly = policy.GetValue("LeaveType") = LeaveType.Vacation.ToString()

            // _isSLOnly = policy.GetValue("LeaveType") = LeaveType.Sick.ToString()

            // _asAdjustment = policy.GetValue("AmountTreatment") = AmountTreatment.Adjustment.ToString()

            LoadAdjUnusedVacationLeaveId();

            LoadVacationLeaveId();
        }

        private async void LoadVacationLeaveId()
        {
            _vacationLeaveId = await GetVacationLeaveId();
        }

        private async void LoadAdjUnusedVacationLeaveId()
        {
            _adjUnusedVacationLeaveId = await GetAdjUnusedVacationLeaveId();
        }

        private DataTable GetLatestLeaveLedger()
        {
            return null;
            //var query1 = $"CALL RPT_LeaveConvertibles(@orgId, @leaveTypeId, @payPeriodFromId, @payPeriodToId, NULL);";

            //int? leaveTypeId = 0;

            //var @params = new Dictionary<string, object>()
            //{
            //    {"@orgId", _organizationId},
            //    {"@leaveTypeId", (_isVLOnly ? _vacationLeaveId : DBNull.Value)
            //    },
            //    {"@payPeriodFromId",_payPeriodFromId},
            //    {"@payPeriodToId",_payPeriodToId}
            //};

            //var query = new SqlToDataTable(query1, @params);

            //return query.Read();
        }

        private async Task<int> GetVacationLeaveId()
        {
            // use product repository for this
            int value;
            var strVacationLeave = $"{LeaveType.Vacation.ToString()} Leave";

            var leaveCategory = await _categoryRepository.GetByNameAsync(_organizationId, strVacationLeave);
            var leaveCategoryId = leaveCategory.RowID;

            using (var context = new PayrollContext())
            {
                var product = context.Products.
                            Where(p => p.PartNo.ToLower() == strVacationLeave.ToLower()).
                            Where(p => p.OrganizationID == _organizationId).
                            Where(p => p.CategoryID == leaveCategoryId).
                            Where(p => p.Category == strLeaveType).
                            FirstOrDefault();

                value = product.RowID.Value;
            }

            return value;
        }

        private async Task<int> GetAdjUnusedVacationLeaveId()
        {
            // use product repository for this
            int value;

            var adjustmentCategory = await _categoryRepository.GetByNameAsync(_organizationId, strAdjType);
            var adjustmentCategoryId = adjustmentCategory.RowID;

            using (var context = new PayrollContext())
            {
                var product = context.Products.Where(p => p.PartNo.ToLower() == _adjUnusedVacationLeave.ToLower()).
                                    Where(p => p.OrganizationID == _organizationId).
                                    Where(p => p.CategoryID == adjustmentCategoryId).
                                    Where(p => p.Category == strAdjType).
                                    FirstOrDefault();

                if (product == null)
                    value = CreateProduct(context, _adjUnusedVacationLeave, strAdjType, adjustmentCategoryId);
                else
                    value = product.RowID.Value;
            }

            return value;
        }

        private int CreateProduct(PayrollContext context, string ProductName, string CategoryName, int CategoryRowId)
        {
            Product product = new Product()
            {
                Name = ProductName,
                OrganizationID = _organizationId,
                Description = string.Empty,
                PartNo = ProductName,
                Created = DateTime.Now,
                LastUpd = DateTime.Now,
                CreatedBy = _userId,
                LastUpdBy = _userId,
                Category = CategoryName,
                CategoryID = CategoryRowId,
                Status = "Active",
                Fixed = false
            };

            context.Products.Add(product);

            context.SaveChanges();

            return product.RowID.Value;
        }

        public void Execute()
        {
            _leaveLedger = GetLatestLeaveLedger();

            PayPeriod payperiod = new PayPeriod();

            if (_isVLOnly & _asAdjustment)
            {
                bool success = false;

                payperiod = _payPeriodRepository.GetById(_currentPeriodId);

                using (var context = new PayrollContext())
                {
                    var paystubs = (from p in context.Paystubs.Include(p => p.ActualAdjustments)
                                    where p.PayPeriodID.Value == _currentPeriodId & p.OrganizationID == _organizationId
                                    select p).ToList();

                    foreach (DataRow row in _leaveLedger.Rows)
                    {
                        var employeePrimKey = Convert.ToInt32(row["EmployeeID"]);
                        var llRowId = Convert.ToInt32(row["RowID"]);

                        LeaveLedger ll = new LeaveLedger() { RowID = llRowId, EmployeeID = employeePrimKey };

                        var unusedLeaveAmount = Convert.ToDecimal(row["Balance"]) * Convert.ToDecimal(row["HourlyRate"]);

                        var leaveBalance = Convert.ToDecimal(row["Balance"]);
                        var leaveDayBalance = leaveBalance / _defaultWorkHours;

                        var paystub = paystubs.Where(p => p.EmployeeID.Value == employeePrimKey).FirstOrDefault();

                        if (paystub != null)
                        {
                            var adjustmentsExceptUnusedLeave = paystub.ActualAdjustments.
                                                                Where(a => a.ProductID != _adjUnusedVacationLeaveId);

                            paystub.ActualAdjustments.Clear();

                            ActualAdjustment aa = new ActualAdjustment()
                            {
                                Created = DateTime.Now,
                                Comment = string.Concat(leaveDayBalance, leaveDayBalance > 1 ? " days" : " day"),
                                CreatedBy = _userId,
                                IsActual = true,
                                LastUpd = DateTime.Now,
                                LastUpdBy = _userId,
                                OrganizationID = _organizationId,
                                Amount = unusedLeaveAmount,
                                PaystubID = paystub.RowID,
                                ProductID = _adjUnusedVacationLeaveId
                            };

                            paystub.ActualAdjustments.Add(aa);

                            foreach (var adj in adjustmentsExceptUnusedLeave)
                                paystub.ActualAdjustments.Add(adj);

                            CreateLeaveTransaction(context, LeaveTransactionType.Debit, ll, payperiod, leaveBalance);
                        }
                        else
                            continue;
                    }

                    context.SaveChanges();
                    success = true;
                }

                if (success)
                {
                    string strCutOff;
                    using (var context = new PayrollContext())
                    {
                        if (payperiod == null)
                            payperiod = _payPeriodRepository.GetById(_currentPeriodId);

                        strCutOff = string.Join(" to ", payperiod.PayFromDate.ToShortDateString(), payperiod.PayToDate.ToShortDateString());
                    }
                    // Do this. Was not migrated from VB to C#
                    //Interaction.MsgBox(string.Concat("Unused leaves were successfully computed.", Constants.vbNewLine, "Please generate the ", strCutOff, " payroll."), MsgBoxStyle.Information);
                }
            }
            else
            {
            }
        }

        private async void ZeroOutEmployeeLeaveBalance(int employeeRowId)
        {
            var employee = await _employeeRepository.GetByIdAsync(employeeRowId);

            if (employee != null)
            {
                {
                    var withBlock = employee;
                    withBlock.LeaveBalance = 0;
                    withBlock.SickLeaveBalance = 0;
                }
            }
        }

        private LeaveTransaction CreateLeaveTransaction(PayrollContext context, LeaveTransactionType leaveTransactionType, LeaveLedger leaveLedger, PayPeriod payPeriod, decimal unusedLeaveHours)
        {
            var employeeRowId = leaveLedger.EmployeeID;

            var lt = context.LeaveTransactions.
                            Where(lTrans => lTrans.OrganizationID == _organizationId).
                            Where(lTrans => lTrans.EmployeeID == employeeRowId).
                            Where(lTrans => lTrans.LeaveLedgerID == leaveLedger.RowID).
                            Where(lTrans => lTrans.PayPeriodID == payPeriod.RowID).
                            Where(lTrans => lTrans.Type == leaveTransactionType.ToString()).
                            Where(lTrans => lTrans.Balance == 0).
                            Where(lTrans => lTrans.Amount == unusedLeaveHours).
                            Where(lTrans => lTrans.TransactionDate == payPeriod.PayToDate).
                            FirstOrDefault();

            if (lt == null)
            {
                lt = new LeaveTransaction()
                {
                    OrganizationID = _organizationId,
                    Created = DateTime.Now,
                    CreatedBy = _userId,
                    LastUpd = DateTime.Now,
                    LastUpdBy = _userId,
                    EmployeeID = employeeRowId,
                    LeaveLedgerID = leaveLedger.RowID,
                    PayPeriodID = payPeriod.RowID,
                    ReferenceID = null,
                    TransactionDate = payPeriod.PayToDate,
                    Type = leaveTransactionType.ToString(),
                    Balance = 0,
                    Amount = unusedLeaveHours
                };

                context.LeaveTransactions.Add(lt);

                ZeroOutEmployeeLeaveBalance(employeeRowId.Value);
            }

            return lt;
        }

        internal enum LeaveTransactionType
        {
            Credit,
            Debit
        }

        internal enum LeaveType
        {
            Vacation,
            Sick,
            All
        }

        internal enum AmountTreatment
        {
            Adjustment,
            Gross
        }
    }
}