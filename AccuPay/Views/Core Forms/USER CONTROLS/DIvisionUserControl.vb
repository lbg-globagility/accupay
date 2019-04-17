Imports AccuPay.Entity

Public Class DivisionUserControl

    Private _division As Division
    Private _parentDivisions As List(Of Division)
    Private _positions As List(Of Position)
    Private _payFrequencies As List(Of PayFrequency)
    Private _divisionTypes As List(Of String)
    Private _deductionSchedules As List(Of String)

    Private Sub DivisionUserControl_Load(sender As Object, e As EventArgs) Handles Me.Load

        'Hide weekly for the moment until this is supported
        DefaultDeductionTabControl.TabPages.Remove(WeeklyTabPage)
        WithAgencyDeductionScheduleTabControl.TabPages.Remove(AgencyWeeklyTabPage)

        PrepareForm()

    End Sub

    Public Sub SetDivision(
                division As Division,
                parentDivisions As List(Of Division),
                positions As List(Of Position),
                divisionTypes As List(Of String),
                payFrequencies As List(Of PayFrequency),
                deductionSchedules As List(Of String))

        _division = division
        _parentDivisions = parentDivisions
        _positions = positions
        _divisionTypes = divisionTypes
        _payFrequencies = payFrequencies
        _deductionSchedules = deductionSchedules

        ParentDivisionComboBox.DataSource = _parentDivisions
        DivisionHeadComboBox.DataSource = _positions
        DivisionTypeComboBox.DataSource = _divisionTypes
        PayFrequencyComboBox.DataSource = _payFrequencies

        SemiMonthlyPhilHealthDeductionScheduleComboBox.DataSource = _deductionSchedules
        SemiMonthlySSSDeductionScheduleComboBox.DataSource = _deductionSchedules
        SemiMonthlyHDMFDeductionScheduleComboBox.DataSource = _deductionSchedules
        SemiMonthlyWithholdingTaxDeductionScheduleComboBox.DataSource = _deductionSchedules

        WeeklyPhilHealthDeductionScheduleComboBox.DataSource = _deductionSchedules
        WeeklySSSDeductionScheduleComboBox.DataSource = _deductionSchedules
        WeeklyHDMFDeductionScheduleComboBox.DataSource = _deductionSchedules
        WeeklyWithholdingTaxDeductionScheduleComboBox.DataSource = _deductionSchedules

        SemiMonthlyAgencyPhilHealthDeductionScheduleComboBox.DataSource = _deductionSchedules
        SemiMonthlyAgencySSSDeductionScheduleComboBox.DataSource = _deductionSchedules
        SemiMonthlyAgencyHDMFDeductionScheduleComboBox.DataSource = _deductionSchedules
        SemiMonthlyAgencyWithholdingTaxDeductionScheduleComboBox.DataSource = _deductionSchedules

        WeeklyAgencyPhilHealthDeductionScheduleComboBox.DataSource = _deductionSchedules
        WeeklyAgencySSSDeductionScheduleComboBox.DataSource = _deductionSchedules
        WeeklyAgencyHDMFDeductionScheduleComboBox.DataSource = _deductionSchedules
        WeeklyAgencyWithholdingTaxDeductionScheduleComboBox.DataSource = _deductionSchedules

    End Sub


#Region "Private Functions"

    Private Sub PrepareForm()

        PrepareComboBoxes()

        CreateFieldDataBindings()

    End Sub

    Private Sub PrepareComboBoxes()

        ParentDivisionComboBox.DisplayMember = "name"
        ParentDivisionComboBox.ValueMember = "RowID"

        DivisionHeadComboBox.DisplayMember = "PositionName"
        DivisionHeadComboBox.ValueMember = "RowID"

        PayFrequencyComboBox.DisplayMember = "Type"
        PayFrequencyComboBox.ValueMember = "RowID"

    End Sub

    Private Sub CreateFieldDataBindings()

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
        ParentDivisionComboBox.DataBindings.Add("SelectedValue", Me._division, "ParentDivisionID")

        DivisionNameTextBox.DataBindings.Clear()
        DivisionNameTextBox.DataBindings.Add("Text", Me._division, "Name")

        TradeNameTextBox.DataBindings.Clear()
        TradeNameTextBox.DataBindings.Add("Text", Me._division, "TradeName")

        TINTextBox.DataBindings.Clear()
        TINTextBox.DataBindings.Add("Text", Me._division, "TINNo")

        BusinessAddressTextBox.DataBindings.Clear()
        BusinessAddressTextBox.DataBindings.Add("Text", Me._division, "BusinessAddress")

        DivisionHeadComboBox.DataBindings.Clear()
        DivisionHeadComboBox.DataBindings.Add("SelectedValue", Me._division, "DivisionHeadID")
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
        VacationLeaveTextBox.DataBindings.Add("Text", Me._division, "DefaultVacationLeave")

        SickLeaveTextBox.DataBindings.Clear()
        SickLeaveTextBox.DataBindings.Add("Text", Me._division, "DefaultSickLeave")
    End Sub

    Private Sub CreatePayrollDetailsDataBindings()
        PayFrequencyComboBox.DataBindings.Clear()
        PayFrequencyComboBox.DataBindings.Add("SelectedValue", Me._division, "PayFrequencyID")

        GracePeriodTextBox.DataBindings.Clear()
        GracePeriodTextBox.DataBindings.Add("Text", Me._division, "GracePeriod")

        WorkDaysPerYearTextBox.DataBindings.Clear()
        WorkDaysPerYearTextBox.DataBindings.Add("Text", Me._division, "WorkDaysPerYear")

        AutomaticOvertimeCheckBox.DataBindings.Clear()
        AutomaticOvertimeCheckBox.DataBindings.Add("Checked", Me._division, "AutomaticOvertimeFiling")
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
        WeeklyPhilHealthDeductionScheduleComboBox.DataBindings.Clear()
        WeeklyPhilHealthDeductionScheduleComboBox.DataBindings.Add("Text", Me._division, "WeeklyPhilHealthDeductionSchedule")

        WeeklySSSDeductionScheduleComboBox.DataBindings.Clear()
        WeeklySSSDeductionScheduleComboBox.DataBindings.Add("Text", Me._division, "WeeklySSSDeductionSchedule")

        WeeklyHDMFDeductionScheduleComboBox.DataBindings.Clear()
        WeeklyHDMFDeductionScheduleComboBox.DataBindings.Add("Text", Me._division, "WeeklyPagIBIGDeductionSchedule")

        WeeklyWithholdingTaxDeductionScheduleComboBox.DataBindings.Clear()
        WeeklyWithholdingTaxDeductionScheduleComboBox.DataBindings.Add("Text", Me._division, "WeeklyWithholdingTaxSchedule")

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
        WeeklyAgencyPhilHealthDeductionScheduleComboBox.DataBindings.Clear()
        WeeklyAgencyPhilHealthDeductionScheduleComboBox.DataBindings.Add("Text", Me._division, "WeeklyAgencyPhilHealthDeductionSchedule")

        WeeklyAgencySSSDeductionScheduleComboBox.DataBindings.Clear()
        WeeklyAgencySSSDeductionScheduleComboBox.DataBindings.Add("Text", Me._division, "WeeklyAgencySssDeductionSchedule")

        WeeklyAgencyHDMFDeductionScheduleComboBox.DataBindings.Clear()
        WeeklyAgencyHDMFDeductionScheduleComboBox.DataBindings.Add("Text", Me._division, "WeeklyAgencyPagIBIGDeductionSchedule")

        WeeklyAgencyWithholdingTaxDeductionScheduleComboBox.DataBindings.Clear()
        WeeklyAgencyWithholdingTaxDeductionScheduleComboBox.DataBindings.Add("Text", Me._division, "WeeklyAgencyWithholdingTaxSchedule")

    End Sub

    Public Sub ShowError(ColumnName As String, ErrorMessage As String)

        If ColumnName = "DivisionType" Then

            ShowBalloonInfo(ErrorMessage, "Division Type", DivisionTypeComboBox)

        ElseIf ColumnName = "ParentDivisionID" Then

            ShowBalloonInfo(ErrorMessage, "Parent Division", ParentDivisionComboBox)

        ElseIf ColumnName = "Name" Then

            ShowBalloonInfo(ErrorMessage, "Name", DivisionNameTextBox)

        ElseIf ColumnName = "WorkDaysPerYear" Then

            ShowBalloonInfo(ErrorMessage, "Number of days work per year", WorkDaysPerYearTextBox)

        End If

    End Sub

    Private Sub ShowBalloonInfo(content As String, title As String, control As Control)

        Dim win32Window = CType(control, IWin32Window)

        myBalloon(content, title, win32Window)

        control.Focus()

    End Sub

#End Region

End Class
