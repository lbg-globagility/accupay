namespace AccuPay.Core.Helpers
{
    public class ProductConstant
    {
        public const string ADJUSTMENT_TYPE_ADDITION = "ADDITION";
        public const string ADJUSTMENT_TYPE_CATEGORY = "Adjustment Type";
        public const string ADJUSTMENT_TYPE_DEDUCTION = "DEDUCTION";
        public const string ALLOWANCE_TYPE_CATEGORY = "Allowance Type";
        public const string BPI_INSURANCE_ADJUSTMENT = "BPI Insurance";
        public const string THIRTEENTH_MONTH_PAY_ADJUSTMENT = "13th Month Pay";
        public const string BONUS_TYPE_CATEGORY = "Bonus";
        public const string ECOLA = "ecola";
        public const string EMPLOYEE_DISCIPLINARY_CATEGORY = "Employee Disciplinary";
        public const string HMO_LOAN = "HMO";
        public const string LEAVE_TYPE_CATEGORY = "Leave Type";
        public const string LOAN_TYPE_CATEGORY = "Loan Type";
        public const string MATERNITY_LEAVE = "Maternity leave";
        public const string OTHERS_LEAVE = "Others";
        public const string PAG_IBIG_LOAN = "PAGIBIG Loan";
        public const string PARENTAL_LEAVE = "Parental";
        public const string SICK_LEAVE = "Sick leave";
        public const string SSS_LOAN = "SSS Loan";
        public const string VACATION_LEAVE = "Vacation leave";
        public const string PARENT_LEAVE = "Maternity/paternity leave";

        public const string SSS_SALARY_LOAN = "SSS Salary Loan";
        public const string SSS_CALAMITY_LOAN = "SSS Calamity Loan";
        public const string SSS_EMERGENCY_LOAN = "SSS Emergency Loan";
        public const string PAG_IBIG_SALARY_LOAN = "Pag-IBIG Salary Loan";
        public const string PAG_IBIG_CALAMITY_LOAN = "Pag-IBIG Calamity Loan";

        public static readonly string[] BENCHMARK_SUPPORTED_LOAN_TYPES = new[] { SSS_SALARY_LOAN,
            SSS_CALAMITY_LOAN,
            SSS_EMERGENCY_LOAN,
            PAG_IBIG_SALARY_LOAN,
            PAG_IBIG_CALAMITY_LOAN};
    }
}
