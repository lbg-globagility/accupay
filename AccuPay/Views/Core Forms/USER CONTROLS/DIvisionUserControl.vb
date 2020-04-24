Imports AccuPay.Entity
Imports AccuPay.Utilities.Extensions

Public Class DivisionUserControl

    Private _division As Division
    Private _parentDivisions As List(Of Division)
    Private _positions As List(Of Position)
    Private _payFrequencies As List(Of Data.Entities.PayFrequency)
    Private _divisionTypes As List(Of String)
    Private _deductionSchedules As List(Of String)

    Private Sub DivisionUserControl_Load(sender As Object, e As EventArgs) Handles Me.Load

        'Hide weekly for the moment until this Is supported
        DefaultDeductionTabControl.TabPages.Remove(WeeklyTabPage)
        WithAgencyDeductionScheduleTabControl.TabPages.Remove(AgencyWeeklyTabPage)

        PrepareForm()

    End Sub

    Public Sub SetDivision(
                division As Division,
                parentDivisions As List(Of Division),
                positions As List(Of Position),
                divisionTypes As List(Of String),
                payFrequencies As List(Of Data.Entities.PayFrequency),
                deductionSchedules As List(Of String))

        _division = division
        _parentDivisions = parentDivisions
        _positions = positions
        _divisionTypes = divisionTypes
        _payFrequencies = payFrequencies
        _deductionSchedules = deductionSchedules

        ParentDivisionComboBox.DataSource = _parentDivisions
        DivisionHeadComboBox.DataSource = _positions
        PayFrequencyComboBox.DataSource = _payFrequencies

        Dim divisionTypesWithNull = _divisionTypes.CloneListJson()
        divisionTypesWithNull.Insert(0, "")
        DivisionTypeComboBox.DataSource = divisionTypesWithNull

        SemiMonthlyPhilHealthDeductionScheduleComboBox.DataSource = _deductionSchedules.CloneListJson()
        SemiMonthlySSSDeductionScheduleComboBox.DataSource = _deductionSchedules.CloneListJson()
        SemiMonthlyHDMFDeductionScheduleComboBox.DataSource = _deductionSchedules.CloneListJson()
        SemiMonthlyWithholdingTaxDeductionScheduleComboBox.DataSource = _deductionSchedules.CloneListJson()

        'WeeklyPhilHealthDeductionScheduleComboBox.DataSource = _deductionSchedules.CloneListJson()
        'WeeklySSSDeductionScheduleComboBox.DataSource = _deductionSchedules.CloneListJson()
        'WeeklyHDMFDeductionScheduleComboBox.DataSource = _deductionSchedules.CloneListJson()
        'WeeklyWithholdingTaxDeductionScheduleComboBox.DataSource = _deductionSchedules.CloneListJson()

        SemiMonthlyAgencyPhilHealthDeductionScheduleComboBox.DataSource = _deductionSchedules.CloneListJson()
        SemiMonthlyAgencySSSDeductionScheduleComboBox.DataSource = _deductionSchedules.CloneListJson()
        SemiMonthlyAgencyHDMFDeductionScheduleComboBox.DataSource = _deductionSchedules.CloneListJson()
        SemiMonthlyAgencyWithholdingTaxDeductionScheduleComboBox.DataSource = _deductionSchedules.CloneListJson()

        'WeeklyAgencyPhilHealthDeductionScheduleComboBox.DataSource = _deductionSchedules.CloneListJson()
        'WeeklyAgencySSSDeductionScheduleComboBox.DataSource = _deductionSchedules.CloneListJson()
        'WeeklyAgencyHDMFDeductionScheduleComboBox.DataSource = _deductionSchedules.CloneListJson()
        'WeeklyAgencyWithholdingTaxDeductionScheduleComboBox.DataSource = _deductionSchedules.CloneListJson()

        PrepareForm()

    End Sub

#Region "Private Functions"

    Private Sub PrepareForm()

        PrepareComboBoxes()

        CreateFieldDataBindings()

    End Sub

    Private Sub PrepareComboBoxes()

        ParentDivisionComboBox.DisplayMember = "Name"
        ParentDivisionComboBox.ValueMember = "RowID"

        DivisionHeadComboBox.DisplayMember = "Name"
        DivisionHeadComboBox.ValueMember = "RowID"

        PayFrequencyComboBox.DisplayMember = "Type"
        PayFrequencyComboBox.ValueMember = "RowID"

    End Sub

    Private Sub CreateFieldDataBindings()

        If Me._division Is Nothing Then Return

        CreateDivisionDetailsDataBindings()

        CreateContactDetailsDataBindings()

        CreateLeaveHoursDataBindings()

        CreatePayrollDetailsDataBindings()

        CreateDeductionSchedulesDataBindings()

    End Sub

    Private Sub CreateDivisionDetailsDataBindings()

        DivisionTypeComboBox.DataBindings.Clear()
        DivisionTypeComboBox.DataBindings.Add("Text", Me._division, "DivisionType")

        ParentDivisionComboBox.DataBindings.Clear()
        ParentDivisionComboBox.DataBindings.Add("SelectedValue", Me._division, "ParentDivisionID", True, DataSourceUpdateMode.OnPropertyChanged)

        DivisionNameTextBox.DataBindings.Clear()
        DivisionNameTextBox.DataBindings.Add("Text", Me._division, "Name")

        TradeNameTextBox.DataBindings.Clear()
        TradeNameTextBox.DataBindings.Add("Text", Me._division, "TradeName")

        TINTextBox.DataBindings.Clear()
        TINTextBox.DataBindings.Add("Text", Me._division, "TINNo")

        BusinessAddressTextBox.DataBindings.Clear()
        BusinessAddressTextBox.DataBindings.Add("Text", Me._division, "BusinessAddress")

        DivisionHeadComboBox.DataBindings.Clear()
        DivisionHeadComboBox.DataBindings.Add("SelectedValue", Me._division, "DivisionHeadID", True, DataSourceUpdateMode.OnPropertyChanged)

    End Sub

    Private Sub CreateContactDetailsDataBindings()
        MainPhoneTextBox.DataBindings.Clear()
        MainPhoneTextBox.DataBindings.Add("Text", Me._division, "MainPhone")

        AlternatePhoneTextBox.DataBindings.Clear()
        AlternatePhoneTextBox.DataBindings.Add("Text", Me._division, "AltPhone")

        EmailAddressTextBox.DataBindings.Clear()
        EmailAddressTextBox.DataBindings.Add("Text", Me._division, "EmailAddress")

        AlternateEmailAddressTextBox.DataBindings.Clear()
        AlternateEmailAddressTextBox.DataBindings.Add("Text", Me._division, "AltEmailAddress")

        ContactNameTextBox.DataBindings.Clear()
        ContactNameTextBox.DataBindings.Add("Text", Me._division, "ContactName")

        FaxNumberTextBox.DataBindings.Clear()
        FaxNumberTextBox.DataBindings.Add("Text", Me._division, "FaxNumber")

        UrlTextBox.DataBindings.Clear()
        UrlTextBox.DataBindings.Add("Text", Me._division, "URL")
    End Sub

    Private Sub CreateLeaveHoursDataBindings()
        VacationLeaveTextBox.DataBindings.Clear()
        VacationLeaveTextBox.DataBindings.Add("Text", Me._division, "DefaultVacationLeave", True, DataSourceUpdateMode.OnPropertyChanged, Nothing, "N2")

        SickLeaveTextBox.DataBindings.Clear()
        SickLeaveTextBox.DataBindings.Add("Text", Me._division, "DefaultSickLeave", True, DataSourceUpdateMode.OnPropertyChanged, Nothing, "N2")

        OthersLeaveTextBox.DataBindings.Clear()
        OthersLeaveTextBox.DataBindings.Add("Text", Me._division, "DefaultOtherLeave", True, DataSourceUpdateMode.OnPropertyChanged, Nothing, "N2")
    End Sub

    Private Sub CreatePayrollDetailsDataBindings()
        PayFrequencyComboBox.DataBindings.Clear()
        PayFrequencyComboBox.DataBindings.Add("SelectedValue", Me._division, "PayFrequencyID", True, DataSourceUpdateMode.OnPropertyChanged)

        GracePeriodTextBox.DataBindings.Clear()
        GracePeriodTextBox.DataBindings.Add("Text", Me._division, "GracePeriod", True, DataSourceUpdateMode.OnPropertyChanged, Nothing, "N2")

        WorkDaysPerYearTextBox.DataBindings.Clear()
        WorkDaysPerYearTextBox.DataBindings.Add("Text", Me._division, "WorkDaysPerYear", True, DataSourceUpdateMode.OnPropertyChanged, Nothing, "N0")

        AutomaticOvertimeCheckBox.DataBindings.Clear()
        AutomaticOvertimeCheckBox.DataBindings.Add("Checked", Me._division, "AutomaticOvertimeFiling", True, DataSourceUpdateMode.OnPropertyChanged)
    End Sub

    Private Sub CreateDeductionSchedulesDataBindings()

        'Semi Monthly
        SemiMonthlyPhilHealthDeductionScheduleComboBox.DataBindings.Clear()
        SemiMonthlyPhilHealthDeductionScheduleComboBox.DataBindings.Add("Text", Me._division, "PhilHealthDeductionSchedule")

        SemiMonthlySSSDeductionScheduleComboBox.DataBindings.Clear()
        SemiMonthlySSSDeductionScheduleComboBox.DataBindings.Add("Text", Me._division, "SssDeductionSchedule")

        SemiMonthlyHDMFDeductionScheduleComboBox.DataBindings.Clear()
        SemiMonthlyHDMFDeductionScheduleComboBox.DataBindings.Add("Text", Me._division, "PagIBIGDeductionSchedule")

        SemiMonthlyWithholdingTaxDeductionScheduleComboBox.DataBindings.Clear()
        SemiMonthlyWithholdingTaxDeductionScheduleComboBox.DataBindings.Add("Text", Me._division, "WithholdingTaxSchedule")

        'Weekly
        'WeeklyPhilHealthDeductionScheduleComboBox.DataBindings.Clear()
        'WeeklyPhilHealthDeductionScheduleComboBox.DataBindings.Add("Text", Me._division, "WeeklyPhilHealthDeductionSchedule")

        'WeeklySSSDeductionScheduleComboBox.DataBindings.Clear()
        'WeeklySSSDeductionScheduleComboBox.DataBindings.Add("Text", Me._division, "WeeklySSSDeductionSchedule")

        'WeeklyHDMFDeductionScheduleComboBox.DataBindings.Clear()
        'WeeklyHDMFDeductionScheduleComboBox.DataBindings.Add("Text", Me._division, "WeeklyPagIBIGDeductionSchedule")

        'WeeklyWithholdingTaxDeductionScheduleComboBox.DataBindings.Clear()
        'WeeklyWithholdingTaxDeductionScheduleComboBox.DataBindings.Add("Text", Me._division, "WeeklyWithholdingTaxSchedule")

        'Semi Monthly Agency
        SemiMonthlyAgencyPhilHealthDeductionScheduleComboBox.DataBindings.Clear()
        SemiMonthlyAgencyPhilHealthDeductionScheduleComboBox.DataBindings.Add("Text", Me._division, "AgencyPhilHealthDeductionSchedule")

        SemiMonthlyAgencySSSDeductionScheduleComboBox.DataBindings.Clear()
        SemiMonthlyAgencySSSDeductionScheduleComboBox.DataBindings.Add("Text", Me._division, "AgencySssDeductionSchedule")

        SemiMonthlyAgencyHDMFDeductionScheduleComboBox.DataBindings.Clear()
        SemiMonthlyAgencyHDMFDeductionScheduleComboBox.DataBindings.Add("Text", Me._division, "AgencyPagIBIGDeductionSchedule")

        SemiMonthlyAgencyWithholdingTaxDeductionScheduleComboBox.DataBindings.Clear()
        SemiMonthlyAgencyWithholdingTaxDeductionScheduleComboBox.DataBindings.Add("Text", Me._division, "AgencyWithholdingTaxSchedule")

        'Weekly Agency
        'WeeklyAgencyPhilHealthDeductionScheduleComboBox.DataBindings.Clear()
        'WeeklyAgencyPhilHealthDeductionScheduleComboBox.DataBindings.Add("Text", Me._division, "WeeklyAgencyPhilHealthDeductionSchedule")

        'WeeklyAgencySSSDeductionScheduleComboBox.DataBindings.Clear()
        'WeeklyAgencySSSDeductionScheduleComboBox.DataBindings.Add("Text", Me._division, "WeeklyAgencySssDeductionSchedule")

        'WeeklyAgencyHDMFDeductionScheduleComboBox.DataBindings.Clear()
        'WeeklyAgencyHDMFDeductionScheduleComboBox.DataBindings.Add("Text", Me._division, "WeeklyAgencyPagIBIGDeductionSchedule")

        'WeeklyAgencyWithholdingTaxDeductionScheduleComboBox.DataBindings.Clear()
        'WeeklyAgencyWithholdingTaxDeductionScheduleComboBox.DataBindings.Add("Text", Me._division, "WeeklyAgencyWithholdingTaxSchedule")

    End Sub

    Public Sub ShowError(ColumnName As String, ErrorMessage As String, Optional x As Integer = 0, Optional y As Integer = 0)

        If ColumnName = "DivisionType" Then

            ShowBalloonInfo(ErrorMessage, "Division Type", DivisionTypeComboBox, x, y)

        ElseIf ColumnName = "ParentDivisionID" Then

            ShowBalloonInfo(ErrorMessage, "Parent Division", ParentDivisionComboBox, x, y)

        ElseIf ColumnName = "Name" Then

            ShowBalloonInfo(ErrorMessage, "Name", DivisionNameTextBox, x, y)

        ElseIf ColumnName = "WorkDaysPerYear" Then

            ShowBalloonInfo(ErrorMessage, "Number of days work per year", WorkDaysPerYearTextBox, x, y)

        End If

    End Sub

    Private Sub ShowBalloonInfo(content As String, title As String, control As Control, Optional x As Integer = 0, Optional y As Integer = 0)

        Dim win32Window = CType(control, IWin32Window)

        myBalloon(content, title, win32Window, x, y)

        control.Focus()

    End Sub

#End Region

End Class