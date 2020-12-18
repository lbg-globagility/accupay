Option Strict On

Imports System.Threading.Tasks
Imports AccuPay.Data.Entities
Imports AccuPay.Data.Interfaces.Excel
Imports AccuPay.Data.Repositories
Imports AccuPay.Data.Services
Imports AccuPay.Desktop.Utilities
Imports AccuPay.Desktop.Helpers
Imports AccuPay.Utilities.Attributes
Imports AccuPay.Utilities.Extensions
Imports Microsoft.Extensions.DependencyInjection

Public Class ImportEmployeeForm

#Region "VariableDeclarations"

    Private Const FormEntityName As String = "Employee"
    Private _filePath As String
    Private _okModels As List(Of EmployeeModel)
    Private _failModels As List(Of EmployeeModel)

    Private _divisionService As DivisionDataService
    Private _branchRepository As BranchRepository
    Private _employeeRepository As EmployeeRepository
    Private _userActivityRepository As UserActivityRepository

#End Region

    Sub New()

        InitializeComponent()

        _divisionService = MainServiceProvider.GetRequiredService(Of DivisionDataService)

        _branchRepository = MainServiceProvider.GetRequiredService(Of BranchRepository)

        _employeeRepository = MainServiceProvider.GetRequiredService(Of EmployeeRepository)

        _userActivityRepository = MainServiceProvider.GetRequiredService(Of UserActivityRepository)

    End Sub

    Private Class EmployeeModel
        Implements IExcelRowRecord

        Private FIXED_AND_MONTHLY_TYPES As String() = {"monthly", "fixed"}
        Private _monthlyHasNoWorkDaysPerYear As Boolean

        Private _noEmployeeNo,
        _employeeAlreadyExists,
        _noLastName,
        _noFirstName,
        _noBirthDate,
        _invalidBirthDate,
        _noGender,
        _noMaritalStatus,
        _noJob,
        _noEmploymentDate,
        _invalidEmploymentDate,
        _noEmploymentStatus As Boolean

        <ColumnName("Employee ID")>
        Public Property EmployeeNo As String

        <ColumnName("Last name")>
        Public Property LastName As String

        <ColumnName("First name")>
        Public Property FirstName As String

        <ColumnName("Middle name")>
        Public Property MiddleName As String

        <ColumnName("Birth date(MM/dd/yyyy)")>
        Public Property BirthDate As Date?

        <ColumnName("Gender(M/F)")>
        Public Property Gender As String

        <ColumnName("Nickname")>
        Public Property Nickname As String

        <ColumnName("Marital Status(Single/Married/N/A)")>
        Public Property MaritalStatus As String

        <ColumnName("Salutation")>
        Public Property Salutation As String

        <ColumnName("Address")>
        Public Property Address As String

        <ColumnName("Contact No.")>
        Public Property ContactNo As String

        <ColumnName("Job position")>
        Public Property Position As String

        <Ignore>
        Public Property PositionId As Integer?

        <ColumnName("TIN")>
        Public Property TIN As String

        <ColumnName("SSS No.")>
        Public Property SSSNo As String

        <ColumnName("PhilHealth No.")>
        Public Property PhilHealthNo As String

        <ColumnName("PAGIBIG No.")>
        Public Property HDMFNo As String

        <ColumnName("Date employed(MM/dd/yyyy)")>
        Public Property DateEmployed As Date?

        <ColumnName("Employee Type(Daily/Monthly/Fixed)")>
        Public Property EmployeeType As String

        <ColumnName("Employment status(Probationary/Regular/Resigned/Terminated)")>
        Public Property EmploymentStatus As String

        <ColumnName("VL allowance per year (hours)")>
        Public Property VacationLeaveAllowance As Decimal

        <ColumnName("SL allowance per year (hours)")>
        Public Property SickLeaveAllowance As Decimal

        <ColumnName("Branch")>
        Public Property Branch As String

        <Ignore>
        Public Property BranchId As Integer?

        <ColumnName("Works days per year")>
        Public Property WorkDaysPerYear As Decimal

        <ColumnName("Current VL balance (hours)")>
        Public Property VacationLeaveBalance As Decimal

        <ColumnName("Current SL balance (hours)")>
        Public Property SickLeaveBalance As Decimal

        <ColumnName("ATM No./Account No.")>
        Public Property AtmNumber As String

        <ColumnName("Email Address")>
        Public Property Email As String

        <ColumnName("Rest Day")>
        Public Property RestDay As String

        <ColumnName("Grace Period (mins.)")>
        Public Property LateGracePeriod As Decimal

        <Ignore>
        Public Property LineNumber As Integer Implements IExcelRowRecord.LineNumber

        Public ReadOnly Property FailureDescription As String
            Get
                Dim resultStrings As New List(Of String)

                If _noEmployeeNo Then resultStrings.Add("no Employee ID")
                If _employeeAlreadyExists Then resultStrings.Add("Employee ID already exists")
                If _noLastName Then resultStrings.Add("no Last Name")
                If _noFirstName Then resultStrings.Add("no First Name")
                If _noBirthDate Then resultStrings.Add("no Birth Date")
                If _invalidBirthDate Then resultStrings.Add("Birth Date cannot be earlier than January 1, 1753")
                If _noGender Then resultStrings.Add("no Gender")
                If _noMaritalStatus Then resultStrings.Add("no Marital Status")
                If _noJob Then resultStrings.Add("no Job Position")
                If _noEmploymentDate Then resultStrings.Add("no Employment Date")
                If _invalidEmploymentDate Then resultStrings.Add("Employment Date cannot be earlier than January 1, 1753")
                If _noEmploymentStatus Then resultStrings.Add("no Employment Status")
                If _monthlyHasNoWorkDaysPerYear Then resultStrings.Add("no Work Days Per Year")

                Return String.Join("; ", resultStrings.Where(Function(s) Not String.IsNullOrWhiteSpace(s)).ToArray())
            End Get
        End Property

        Public Function ConsideredFailed(allEmployees As IEnumerable(Of Employee)) As Boolean

            _noEmployeeNo = String.IsNullOrWhiteSpace(EmployeeNo)
            _employeeAlreadyExists = Not _noEmployeeNo AndAlso allEmployees.Any(Function(e) e.EmployeeNo = EmployeeNo)

            Dim noEmployeeType = String.IsNullOrWhiteSpace(EmployeeType)
            _monthlyHasNoWorkDaysPerYear = FIXED_AND_MONTHLY_TYPES.Any(Function(s) s = If(noEmployeeType, s, EmployeeType.ToLower())) And WorkDaysPerYear = 0

            _noLastName = String.IsNullOrWhiteSpace(LastName)
            _noFirstName = String.IsNullOrWhiteSpace(FirstName)
            _noBirthDate = Not BirthDate.HasValue
            _invalidBirthDate = _noBirthDate = False AndAlso BirthDate.Value < Data.Helpers.PayrollTools.SqlServerMinimumDate
            _noGender = String.IsNullOrWhiteSpace(Gender)
            _noMaritalStatus = String.IsNullOrWhiteSpace(MaritalStatus)
            _noJob = String.IsNullOrWhiteSpace(Position)
            _noEmploymentDate = Not DateEmployed.HasValue
            _invalidEmploymentDate = _noEmploymentDate = False AndAlso DateEmployed.Value < Data.Helpers.PayrollTools.SqlServerMinimumDate
            _noEmploymentStatus = String.IsNullOrWhiteSpace(EmploymentStatus)

            Return _noEmployeeNo _
            Or _employeeAlreadyExists _
            Or _monthlyHasNoWorkDaysPerYear _
            Or _noLastName _
            Or _noFirstName _
            Or _noBirthDate _
            Or _invalidBirthDate _
            Or _noGender _
            Or _noMaritalStatus _
            Or _noJob _
            Or _noEmploymentDate _
            Or _invalidEmploymentDate _
            Or _noEmploymentStatus
        End Function

    End Class

#Region "Properties"

    Private Async Function SetFileDirectory(value As String) As Task
        _filePath = value

        Await FilePathChanged()
    End Function

#End Region

#Region "Functions"

    Public Async Function SaveAsync() As Task
        If Not _okModels.Any() Then
            Return
        End If

        Dim employees = New List(Of EmployeeWithLeaveBalanceData)

        For Each model In _okModels

            Dim newEmployee = Employee.NewEmployee(z_OrganizationID)

            AssignChanges(model, newEmployee)

            employees.Add(
                New EmployeeWithLeaveBalanceData(
                    newEmployee,
                    vacationLeaveBalance:=model.VacationLeaveBalance,
                    sickLeaveBalance:=model.SickLeaveBalance))

        Next

        Await SaveToDatabase(employees)
    End Function

    Private Async Function SaveToDatabase(employees As List(Of EmployeeWithLeaveBalanceData)) As Task

        If employees.Any Then

            Dim service = MainServiceProvider.GetRequiredService(Of EmployeeDataService)

            Await service.ImportAsync(
                employees,
                organizationId:=z_OrganizationID,
                userId:=z_User)

            Dim importList = New List(Of UserActivityItem)

            For Each item In employees

                importList.Add(New UserActivityItem() With
                {
                    .Description = $"Created a new employee.",
                    .EntityId = item.Employee.RowID.Value,
                    .ChangedEmployeeId = item.Employee.RowID.Value
                })
            Next

            _userActivityRepository.CreateRecord(z_User, FormEntityName, z_OrganizationID, UserActivityRepository.RecordTypeImport, importList)

        End If
    End Function

#End Region

#Region "Methods"

    Private Sub AssignChanges(em As EmployeeModel, e As Employee)
        With e
            If Not String.IsNullOrWhiteSpace(em.Address) Then .HomeAddress = em.Address?.Trim()

            If em.BirthDate.HasValue Then .BirthDate = em.BirthDate.Value.Date

            If Not String.IsNullOrWhiteSpace(em.ContactNo) Then .MobilePhone = em.ContactNo?.Trim()

            If em.DateEmployed.HasValue Then .StartDate = em.DateEmployed.Value.Date

            If Not String.IsNullOrWhiteSpace(em.EmployeeNo) Then .EmployeeNo = em.EmployeeNo?.Trim()

            If Not String.IsNullOrWhiteSpace(em.EmployeeType) Then .EmployeeType = em.EmployeeType?.Trim()

            If Not String.IsNullOrWhiteSpace(em.EmploymentStatus) Then .EmploymentStatus = em.EmploymentStatus?.Trim()

            If Not String.IsNullOrWhiteSpace(em.FirstName) Then .FirstName = em.FirstName?.Trim()

            If Not String.IsNullOrWhiteSpace(em.Gender) Then .Gender = em.Gender?.Trim()

            If Not String.IsNullOrWhiteSpace(em.HDMFNo) Then .HdmfNo = em.HDMFNo?.Trim()

            If Not String.IsNullOrWhiteSpace(em.HDMFNo) Then .HdmfNo = em.HDMFNo?.Trim()

            .PositionID = em.PositionId

            If Not String.IsNullOrWhiteSpace(em.LastName) Then .LastName = em.LastName?.Trim()

            If em.VacationLeaveAllowance > 0 Then .VacationLeaveAllowance = em.VacationLeaveAllowance

            If em.SickLeaveAllowance > 0 Then .SickLeaveAllowance = em.SickLeaveAllowance

            If Not String.IsNullOrWhiteSpace(em.MaritalStatus) Then .MaritalStatus = em.MaritalStatus?.Trim()

            If Not String.IsNullOrWhiteSpace(em.MiddleName) Then .MiddleName = em.MiddleName?.Trim()

            If Not String.IsNullOrWhiteSpace(em.Nickname) Then .Nickname = em.Nickname?.Trim()

            If Not String.IsNullOrWhiteSpace(em.PhilHealthNo) Then .PhilHealthNo = em.PhilHealthNo?.Trim()

            If Not String.IsNullOrWhiteSpace(em.Salutation) Then .Salutation = em.Salutation?.Trim()

            If Not String.IsNullOrWhiteSpace(em.SSSNo) Then .SssNo = em.SSSNo?.Trim()

            If Not String.IsNullOrWhiteSpace(em.TIN) Then .TinNo = em.TIN?.Trim()

            If em.WorkDaysPerYear > 0 Then .WorkDaysPerYear = em.WorkDaysPerYear

            If Not String.IsNullOrWhiteSpace(em.AtmNumber) Then .AtmNo = em.AtmNumber

            If Not String.IsNullOrWhiteSpace(em.Email) Then .EmailAddress = em.Email

            .DayOfRest = ParseRestDay(em.RestDay)

            .LateGracePeriod = em.LateGracePeriod

            .BranchID = em.BranchId

        End With

    End Sub

    Private Function ParseRestDay(restDay As String) As Integer?
        If restDay Is Nothing Then Return Nothing

        Dim input = restDay.Trim().ToLower()

        Select Case input
            Case "sunday" : Return 0
            Case "monday" : Return 1
            Case "tuesday" : Return 2
            Case "wednesday" : Return 3
            Case "thursday" : Return 4
            Case "friday" : Return 5
            Case "saturday" : Return 6

            Case Else
                Return Nothing
        End Select

    End Function

    Private Async Function FilePathChanged() As Task

        Dim models As New List(Of EmployeeModel)

        Dim parsedSuccessfully = FunctionUtils.TryCatchExcelParserReadFunction(
            Sub()
                models = ExcelService(Of EmployeeModel).
                    Read(_filePath).
                    ToList()

                models.ForEach(
                Sub(model)
                    If (Not String.IsNullOrWhiteSpace(model.RestDay)) Then
                        model.RestDay = model.RestDay.Trim().ToUpper()
                    End If
                End Sub)
            End Sub)

        If parsedSuccessfully = False Then Return

        If models Is Nothing Then

            MessageBoxHelper.ErrorMessage("Cannot read the template.")

            Return
        End If

        Dim allEmployees = Await _employeeRepository.GetAllAsync(z_OrganizationID)

        _okModels = models.Where(Function(ee) Not ee.ConsideredFailed(allEmployees)).ToList()
        _failModels = models.Where(Function(ee) ee.ConsideredFailed(allEmployees)).ToList()

        Await AddPositionIdToModels(_okModels)

        Await AddBranchIdToModels(_okModels)

        DataGridView1.DataSource = _okModels
        DataGridView2.DataSource = _failModels

        SaveButton.Enabled = _okModels.Count > 0

        TabPage1.Text = $"Ok ({Me._okModels.Count})"
        TabPage2.Text = $"Failed ({Me._failModels.Count})"

        UpdateStatusLabel(_failModels.Count)

    End Function

    Private Async Function AddPositionIdToModels(models As List(Of EmployeeModel)) As Task

        'fresh instance of repository
        Dim repository = MainServiceProvider.GetRequiredService(Of PositionRepository)
        Dim existingPositions = (Await repository.GetAllAsync(z_OrganizationID)).ToList()

        For Each model In models

            If model.Position Is Nothing Then Continue For

            Dim currentPosition = existingPositions.
                FirstOrDefault(Function(p) p.Name.ToTrimmedLowerCase() = model.Position.ToTrimmedLowerCase())

            If currentPosition IsNot Nothing Then

                model.PositionId = currentPosition.RowID
            Else

                Dim dataService = MainServiceProvider.GetRequiredService(Of PositionDataService)
                currentPosition = Await dataService.GetByNameOrCreateAsync(
                    model.Position,
                    organizationId:=z_OrganizationID,
                    currentlyLoggedInUserId:=z_User)

                model.PositionId = currentPosition?.RowID

                'add the newly added position to the list of existing positions
                If currentPosition IsNot Nothing Then

                    existingPositions.Add(currentPosition)

                End If
            End If

        Next

    End Function

    Private Async Function AddBranchIdToModels(models As List(Of EmployeeModel)) As Task

        Dim existingBranches = (Await _branchRepository.GetAllAsync()).ToList()

        For Each model In models

            If model.Branch Is Nothing Then Continue For

            Dim currentBranch = existingBranches.
                FirstOrDefault(Function(b) b.Name.ToTrimmedLowerCase() = model.Branch.ToTrimmedLowerCase())

            If currentBranch IsNot Nothing Then

                model.BranchId = currentBranch.RowID
            Else
                currentBranch = New Branch
                currentBranch.CreatedBy = z_User
                currentBranch.Code = model.Branch.Trim()
                currentBranch.Name = currentBranch.Code
                currentBranch.CalendarID = Nothing

                Await _branchRepository.CreateAsync(currentBranch)

                model.BranchId = currentBranch?.RowID
                model.Branch = currentBranch?.Name

                'add the newly added branch to the list of existing branches
                If currentBranch IsNot Nothing Then

                    existingBranches.Add(currentBranch)

                End If
            End If

        Next

    End Function

    Private Sub UpdateStatusLabel(errorCount As Integer)
        If errorCount > 0 Then

            If errorCount = 1 Then
                lblStatus.Text = "There is 1 error."
            Else
                lblStatus.Text = $"There are {errorCount} errors."

            End If

            lblStatus.Text += " Failed records will not be saved."
            lblStatus.BackColor = Color.Red
        Else
            lblStatus.Text = "No errors found."
            lblStatus.BackColor = Color.Green
        End If
    End Sub

#End Region

#Region "EventHandlers"

    Private Sub ImportEmployeeForm_Load(sender As Object, e As EventArgs) Handles Me.Load
        DataGridView1.AutoGenerateColumns = False
        DataGridView2.AutoGenerateColumns = False
    End Sub

    Private Sub DataGridView1_DataSourceChanged(sender As Object, e As EventArgs) Handles DataGridView1.DataSourceChanged
        TabPage1.Text = $"OK ({DataGridView1.Rows.Count()})"
    End Sub

    Private Sub DataGridView2_DataSourceChanged(sender As Object, e As EventArgs) Handles DataGridView2.DataSourceChanged
        TabPage2.Text = $"Failed ({DataGridView2.Rows.Count()})"
    End Sub

    Private Async Sub Button3_Click(sender As Object, e As EventArgs) Handles Button3.Click

        Dim browseFile = New OpenFileDialog With {
            .Filter = "Microsoft Excel Workbook Documents 2007-13 (*.xlsx)|*.xlsx|" &
                      "Microsoft Excel Documents 97-2003 (*.xls)|*.xls",
            .RestoreDirectory = True
        }

        If Not browseFile.ShowDialog() = DialogResult.OK Then Return

        Await FunctionUtils.TryCatchFunctionAsync("Load Employee Data",
            Async Function()
                Await SetFileDirectory(browseFile.FileName)
            End Function)
    End Sub

    Private Sub btnDownload_Click(sender As Object, e As EventArgs) Handles btnDownload.Click
        DownloadTemplateHelper.DownloadExcel(ExcelTemplates.Employee)
    End Sub

#End Region

End Class
