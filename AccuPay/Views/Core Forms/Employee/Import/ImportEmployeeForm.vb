Option Strict On

Imports System.Threading.Tasks
Imports AccuPay.Data.Entities
Imports AccuPay.Data.Repositories
Imports AccuPay.Data.Services
Imports AccuPay.Helpers
Imports AccuPay.Infrastructure.Services.Excel
Imports AccuPay.Utilities.Extensions
Imports AccuPay.Utils
Imports Globagility.AccuPay
Imports Microsoft.Extensions.DependencyInjection

Public Class ImportEmployeeForm

#Region "VariableDeclarations"

    Private Const FormEntityName As String = "Employee"
    Private _filePath As String
    Private _okModels As List(Of EmployeeModel)
    Private _failModels As List(Of EmployeeModel)

    Private _divisionService As DivisionDataService
    Private _positionService As PositionDataService
    Private _userActivityRepository As UserActivityRepository

#End Region

    Sub New()

        InitializeComponent()

        _divisionService = MainServiceProvider.GetRequiredService(Of DivisionDataService)

        _positionService = MainServiceProvider.GetRequiredService(Of PositionDataService)

        _userActivityRepository = MainServiceProvider.GetRequiredService(Of UserActivityRepository)

    End Sub

    Private Class EmployeeModel
        Implements IExcelRowRecord

        Private FIXED_AND_MONTHLY_TYPES As String() = {"monthly", "fixed"}
        Private _monthlyHasNoWorkDaysPerYear As Boolean

        Private _noEmployeeNo,
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

        <ColumnName("Leave allowance per year")>
        Public Property LeaveAllowance As Decimal

        <ColumnName("Works days per year")>
        Public Property WorkDaysPerYear As Decimal

        <Ignore>
        Public Property LineNumber As Integer Implements IExcelRowRecord.LineNumber

        Public ReadOnly Property FailureDescription As String
            Get
                Dim resultStrings As New List(Of String)

                If _monthlyHasNoWorkDaysPerYear Then resultStrings.Add("no Work Days Per Year")
                If _noEmployeeNo Then resultStrings.Add("no Employee No")
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

                Return String.Join("; ", resultStrings.Where(Function(s) Not String.IsNullOrWhiteSpace(s)).ToArray())
            End Get
        End Property

        Public ReadOnly Property ConsideredFailed As Boolean
            Get
                Dim noEmployeeType = String.IsNullOrWhiteSpace(EmployeeType)
                _monthlyHasNoWorkDaysPerYear = FIXED_AND_MONTHLY_TYPES.Any(Function(s) s = If(noEmployeeType, s, EmployeeType.ToLower())) And WorkDaysPerYear = 0
                _noEmployeeNo = String.IsNullOrWhiteSpace(EmployeeNo)
                _noLastName = String.IsNullOrWhiteSpace(LastName)
                _noFirstName = String.IsNullOrWhiteSpace(FirstName)
                _noBirthDate = Not BirthDate.HasValue
                _invalidBirthDate = _noBirthDate = False AndAlso BirthDate.Value < Data.Helpers.PayrollTools.MinimumMicrosoftDate
                _noGender = String.IsNullOrWhiteSpace(Gender)
                _noMaritalStatus = String.IsNullOrWhiteSpace(MaritalStatus)
                _noJob = String.IsNullOrWhiteSpace(Position)
                _noEmploymentDate = Not DateEmployed.HasValue
                _invalidEmploymentDate = _noEmploymentDate = False AndAlso DateEmployed.Value < Data.Helpers.PayrollTools.MinimumMicrosoftDate
                _noEmploymentStatus = String.IsNullOrWhiteSpace(EmploymentStatus)

                Return _monthlyHasNoWorkDaysPerYear _
                Or _noEmployeeNo _
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
            End Get
        End Property

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

        Dim models = _okModels

        Dim employeeNos = models.Select(Function(e) e.EmployeeNo).ToList()

        Dim employeeRepo = MainServiceProvider.GetRequiredService(Of EmployeeRepository)
        Dim employees1 = Await employeeRepo.GetAllAsync(z_OrganizationID)

        Dim employees = employees1.
            Where(Function(e) employeeNos.Contains(e.EmployeeNo)).
            ToList()

        Dim existingEmployees = New List(Of Employee)
        'for updates
        For Each employee In employees
            Dim model = models.
                FirstOrDefault(Function(m) m.EmployeeNo = employee.EmployeeNo)

            If model IsNot Nothing Then
                AssignChanges(model, employee)
                employee.LastUpdBy = z_User

                existingEmployees.Add(employee)
            End If

        Next

        'for insert
        Dim notExistEmployees = models.
            Where(Function(em) Not employees.Any(Function(e) e.EmployeeNo = em.EmployeeNo)).
            ToList()

        Dim newEmployees = New List(Of Employee)

        For Each model In notExistEmployees

            Dim employee = New Employee With {
                .RowID = Nothing,
                .OrganizationID = z_OrganizationID,
                .Created = Now,
                .CreatedBy = z_User,
                .PayFrequencyID = Data.Helpers.PayrollTools.PayFrequencySemiMonthlyId
            }

            AssignChanges(model, employee)

            newEmployees.Add(employee)

        Next

        Await SaveToDatabase(existingEmployees, newEmployees)
    End Function

    Private Async Function SaveToDatabase(existingEmployees As List(Of Employee),
                                        newEmployees As List(Of Employee)) As Task

        Dim importedEmployees = existingEmployees.CloneListJson()
        importedEmployees.AddRange(newEmployees)

        If importedEmployees.Any Then

            Dim employeeRepo = MainServiceProvider.GetRequiredService(Of EmployeeRepository)
            Await employeeRepo.SaveManyAsync(importedEmployees)

            Dim importList = New List(Of UserActivityItem)
            Dim entityName = FormEntityName.ToLower()

            For Each item In newEmployees

                importList.Add(New UserActivityItem() With
                        {
                        .Description = $"Imported a new {entityName}.",
                        .EntityId = item.RowID.Value
                        })
            Next

            For Each model In existingEmployees
                importList.Add(New UserActivityItem() With
                    {
                    .Description = $"Updated an {entityName} on import.",
                    .EntityId = model.RowID.Value
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

            e.PositionID = em.PositionId

            If Not String.IsNullOrWhiteSpace(em.LastName) Then .LastName = em.LastName?.Trim()

            If em.LeaveAllowance > 0 Then .VacationLeaveAllowance = em.LeaveAllowance

            If Not String.IsNullOrWhiteSpace(em.MaritalStatus) Then .MaritalStatus = em.MaritalStatus?.Trim()

            If Not String.IsNullOrWhiteSpace(em.MiddleName) Then .MiddleName = em.MiddleName?.Trim()

            If Not String.IsNullOrWhiteSpace(em.Nickname) Then .Nickname = em.Nickname?.Trim()

            If Not String.IsNullOrWhiteSpace(em.PhilHealthNo) Then .PhilHealthNo = em.PhilHealthNo?.Trim()

            If Not String.IsNullOrWhiteSpace(em.Salutation) Then .Salutation = em.Salutation?.Trim()

            If Not String.IsNullOrWhiteSpace(em.SSSNo) Then .SssNo = em.SSSNo?.Trim()

            If Not String.IsNullOrWhiteSpace(em.TIN) Then .TinNo = em.TIN?.Trim()

            If em.WorkDaysPerYear > 0 Then .WorkDaysPerYear = em.WorkDaysPerYear

        End With

    End Sub

    Private Async Function FilePathChanged() As Task

        Dim models As New List(Of EmployeeModel)

        Dim parsedSuccessfully = FunctionUtils.TryCatchExcelParserReadFunction(
            Sub()
                models = ExcelService(Of EmployeeModel).
                            Read(_filePath).
                            ToList()
            End Sub)

        If parsedSuccessfully = False Then Return

        If models Is Nothing Then

            MessageBoxHelper.ErrorMessage("Cannot read the template.")

            Return
        End If

        Await AddPositionIdToModels(models)

        _okModels = models.Where(Function(ee) Not ee.ConsideredFailed).ToList()
        _failModels = models.Where(Function(ee) ee.ConsideredFailed).ToList()

        DataGridView1.DataSource = _okModels
        DataGridView2.DataSource = _failModels

        SaveButton.Enabled = _okModels.Count > 0

        TabPage1.Text = $"Ok ({Me._okModels.Count})"
        TabPage2.Text = $"Failed ({Me._failModels.Count})"

        UpdateStatusLabel(_failModels.Count)

    End Function

    Private Async Function AddPositionIdToModels(models As List(Of EmployeeModel)) As Task

        Dim existingPositions = (Await _positionService.GetAllAsync(z_OrganizationID)).ToList()

        For Each model In models
            Dim currentPosition = existingPositions.
                           FirstOrDefault(Function(p) p.Name.ToTrimmedLowerCase() =
                                                        model.Position.ToTrimmedLowerCase())

            If currentPosition IsNot Nothing Then

                model.PositionId = currentPosition.RowID
            Else

                currentPosition = Await _positionService.
                                    GetByNameOrCreateAsync(model.Position,
                                                           organizationId:=z_OrganizationID,
                                                           userId:=z_User)

                model.PositionId = currentPosition?.RowID

                'add the newly added position to the list of existing positions
                If currentPosition IsNot Nothing Then

                    existingPositions.Add(currentPosition)

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