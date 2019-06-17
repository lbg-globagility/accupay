Imports AccuPay
Imports AccuPay.Attributes
Imports AccuPay.Entity
Imports AccuPay.Helpers
Imports AccuPay.Utils
Imports Globagility.AccuPay
Imports log4net
Imports Microsoft.EntityFrameworkCore

Public Class ImportEmployeeForm

#Region "VariableDeclarations"

    Private Shared logger As ILog = LogManager.GetLogger("EmployeeFormAppender")

    Private _worksheetName As String
    Private _ep As New ExcelParser(Of EmployeeModel)
    Private _filePath As String
    Private _okModels As List(Of EmployeeModel)
    Private _failModels As List(Of EmployeeModel)

#End Region

    Private Class EmployeeModel
        Implements IExcelRowRecord

        Private FIXED_AND_MONTHLY_TYPES As String() = {"monthly", "fixed"}
        Private _monthlyHasNoWorkDaysPerYear As Boolean

        Private _noEmployeeNo,
        _noLastName,
        _noFirstName,
        _noBirthDate,
        _noGender,
        _noMaritalStatus,
        _noJob,
        _noEmploymentDate,
        _noPayFrequency,
        _noEmploymentStatus As Boolean

        <ColumnName("Employee ID")>
        Public Property EmployeeNo As String

        <ColumnName("Last name")>
        Public Property LastName As String

        <ColumnName("First name")>
        Public Property FirstName As String

        <ColumnName("Middle name")>
        Public Property MiddleName As String

        <ColumnName("Surname")>
        Public Property Surname As String

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
        Public Property Job As String

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

        <ColumnName("Pay frequency(Weekly/Semi-monthly)")>
        Public Property PayFrequency As String

        <ColumnName("Employee Type(Daily/Monthly/Fixed)")>
        Public Property EmployeeType As String

        <ColumnName("Employment status(Probationary/Regular/Resigned/Terminated)")>
        Public Property EmploymentStatus As String

        <ColumnName("Leave allowance per year")>
        Public Property LeaveAllowance As Decimal

        '<ColumnName("Leave per pay period")>
        'Public Property fsdfsd As String

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
                If _noBirthDate Then resultStrings.Add("no Birthdate")
                If _noGender Then resultStrings.Add("no Gender")
                If _noMaritalStatus Then resultStrings.Add("no Marital Status")
                If _noJob Then resultStrings.Add("no Job Position")
                If _noEmploymentDate Then resultStrings.Add("no Employment Date")
                If _noPayFrequency Then resultStrings.Add("no Pay Frequency")
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
                _noGender = String.IsNullOrWhiteSpace(Gender)
                _noMaritalStatus = String.IsNullOrWhiteSpace(MaritalStatus)
                _noJob = String.IsNullOrWhiteSpace(Job)
                _noEmploymentDate = Not DateEmployed.HasValue
                _noPayFrequency = String.IsNullOrWhiteSpace(PayFrequency)
                _noEmploymentStatus = String.IsNullOrWhiteSpace(EmploymentStatus)

                Return _monthlyHasNoWorkDaysPerYear _
                Or _noEmployeeNo _
                Or _noLastName _
                Or _noFirstName _
                Or _noBirthDate _
                Or _noGender _
                Or _noMaritalStatus _
                Or _noJob _
                Or _noEmploymentDate _
                Or _noPayFrequency _
                Or _noEmploymentStatus
            End Get
        End Property

    End Class

#Region "Properties"

    Private Property FileDirectory As String
        Get
            Return _filePath
        End Get
        Set(value As String)
            _filePath = value

            FilePathChanged()
        End Set
    End Property

#End Region

#Region "Functions"

    Public Async Function SaveAsync() As Threading.Tasks.Task(Of Boolean)
        Dim succeed As Boolean = False

        If Not _okModels.Any() Then Return False

        Dim importedEmployees = _okModels

        Dim employeeNos = importedEmployees.Select(Function(e) e.EmployeeNo).ToList()

        Using context = New PayrollContext
            Dim employees = Await context.Employees.
                Where(Function(e) e.OrganizationID.Value = z_OrganizationID).
                Where(Function(e) employeeNos.Contains(e.EmployeeNo)).
                ToListAsync()

            'for updates
            For Each e In employees
                Dim em = importedEmployees.Where(Function(model) model.EmployeeNo = e.EmployeeNo).FirstOrDefault

                If em IsNot Nothing Then AssignChanges(em, e)
            Next

            Dim department =
                Await context.Divisions.Where(Function(d) d.OrganizationID = z_OrganizationID).FirstOrDefaultAsync(Function(d) d.ParentDivisionID.HasValue)
            Dim departmentID As Integer = If(department IsNot Nothing, department.RowID.Value, 0)

            'for insert
            Dim notExistEmployees = importedEmployees.
                Where(Function(em) Not employees.Any(Function(e) e.EmployeeNo = em.EmployeeNo)).
                ToList()
            For Each em In notExistEmployees
                Dim position = Await context.Positions.
                    Where(Function(p) p.OrganizationID = z_OrganizationID).
                    Where(Function(p) p.Name = em.Job).
                    FirstOrDefaultAsync

                Dim positionRowID = position?.RowID
                If Not positionRowID.HasValue Then _
                    positionRowID = Await CreatePositionAsync(context, em.Job, departmentID)

                Dim e As New Employee With {
                    .OrganizationID = z_OrganizationID,
                    .Created = Now,
                    .CreatedBy = z_User,
                    .PayFrequencyID = If(em.PayFrequency.ToLower() = "semi-monthly", 1, 4),
                    .PositionID = positionRowID}

                AssignChanges(em, e)

                context.Employees.Add(e)
            Next

            Try
                Await context.SaveChangesAsync()

                succeed = True
            Catch ex As Exception
                succeed = False
                logger.Error("EmployeeImportProfile", ex)
                Dim errMsg = String.Concat("Oops! something went wrong, please", Environment.NewLine, "contact ", My.Resources.AppCreator, " for assistance.")
                MessageBox.Show(errMsg, "Import Failed", MessageBoxButtons.OK, MessageBoxIcon.Error)

            End Try

        End Using

        Return succeed
    End Function

    Private Async Function CreatePositionAsync(context As PayrollContext, positionName As String, departmentID As Integer) As Threading.Tasks.Task(Of Integer)
        Dim employeeJob As Position = New Position With {
            .Name = positionName,
            .OrganizationID = z_OrganizationID,
            .Created = Now,
            .CreatedBy = z_User}
        If departmentID > 0 Then employeeJob.DivisionID = departmentID
        Dim jobPosition = context.Positions.
            Add(employeeJob)

        Await context.SaveChangesAsync()

        Return jobPosition.Entity.RowID.Value
    End Function

#End Region

#Region "Methods"

    Private Sub AssignChanges(em As EmployeeModel, e As Employee)
        With e
            If Not String.IsNullOrWhiteSpace(em.Address) Then .HomeAddress = em.Address

            If em.BirthDate.HasValue Then .BirthDate = em.BirthDate.Value.Date

            If Not String.IsNullOrWhiteSpace(em.ContactNo) Then .MobilePhone = em.ContactNo

            If em.DateEmployed.HasValue Then .StartDate = em.DateEmployed.Value.Date

            If Not String.IsNullOrWhiteSpace(em.EmployeeNo) Then .EmployeeNo = em.EmployeeNo

            If Not String.IsNullOrWhiteSpace(em.EmployeeType) Then .EmployeeType = em.EmployeeType

            If Not String.IsNullOrWhiteSpace(em.EmploymentStatus) Then .EmploymentStatus = em.EmploymentStatus

            If Not String.IsNullOrWhiteSpace(em.FirstName) Then .FirstName = em.FirstName

            If Not String.IsNullOrWhiteSpace(em.Gender) Then .Gender = em.Gender

            If Not String.IsNullOrWhiteSpace(em.HDMFNo) Then .HdmfNo = em.HDMFNo

            If Not String.IsNullOrWhiteSpace(em.HDMFNo) Then .HdmfNo = em.HDMFNo

            'em.Job

            If Not String.IsNullOrWhiteSpace(em.LastName) Then .LastName = em.LastName

            If em.LeaveAllowance > 0 Then .LeaveAllowance = em.LeaveAllowance

            If Not String.IsNullOrWhiteSpace(em.MaritalStatus) Then .MaritalStatus = em.MaritalStatus

            If Not String.IsNullOrWhiteSpace(em.MiddleName) Then .MiddleName = em.MiddleName

            If Not String.IsNullOrWhiteSpace(em.Nickname) Then .Nickname = em.Nickname

            'em.PayFrequency

            If Not String.IsNullOrWhiteSpace(em.PhilHealthNo) Then .PhilHealthNo = em.PhilHealthNo

            If Not String.IsNullOrWhiteSpace(em.Salutation) Then .Salutation = em.Salutation

            If Not String.IsNullOrWhiteSpace(em.SSSNo) Then .SssNo = em.SSSNo

            'em.Surname

            If Not String.IsNullOrWhiteSpace(em.TIN) Then .TinNo = em.TIN

            If em.WorkDaysPerYear > 0 Then .WorkDaysPerYear = em.WorkDaysPerYear

        End With

    End Sub

    Private Sub ExcelParserPreparation(filePath As String, Optional worksheetName As String = Nothing)

        _filePath = filePath

        If Not String.IsNullOrWhiteSpace(worksheetName) Then
            _worksheetName = worksheetName
            _ep = New ExcelParser(Of EmployeeModel)(_worksheetName)
        End If

        _ep = New ExcelParser(Of EmployeeModel)
    End Sub

    Private Sub FilePathChanged()

        Dim models As IList(Of EmployeeModel)
        Try

            models = _ep.Read(_filePath)
        Catch ex As WorkSheetNotFoundException

            MessageBoxHelper.ErrorMessage(ex.Message)

            Return
        End Try

        If models Is Nothing Then

            MessageBoxHelper.ErrorMessage("Cannot read the template.")

            Return
        End If

        _okModels = models.Where(Function(ee) Not ee.ConsideredFailed).ToList()
        _failModels = models.Where(Function(ee) ee.ConsideredFailed).ToList()

        DataGridView1.DataSource = _okModels
        DataGridView2.DataSource = _failModels

        SaveButton.Enabled = _okModels.Count > 0

        TabPage1.Text = $"Ok ({Me._okModels.Count})"
        TabPage2.Text = $"Failed ({Me._failModels.Count})"

        UpdateStatusLabel(_failModels.Count)

    End Sub

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

    Private Sub DataGridView1_CellContentClick(sender As Object, e As DataGridViewCellEventArgs) Handles DataGridView1.CellContentClick

    End Sub

    Private Sub DataGridView1_DataSourceChanged(sender As Object, e As EventArgs) Handles DataGridView1.DataSourceChanged
        TabPage1.Text = $"OK ({DataGridView1.Rows.Count()})"
    End Sub

    Private Sub DataGridView2_CellContentClick(sender As Object, e As DataGridViewCellEventArgs)

    End Sub

    Private Sub DataGridView2_DataSourceChanged(sender As Object, e As EventArgs) Handles DataGridView2.DataSourceChanged
        TabPage2.Text = $"Failed ({DataGridView2.Rows.Count()})"
    End Sub

    Private Sub Button3_Click(sender As Object, e As EventArgs) Handles Button3.Click

        Dim browseFile = New OpenFileDialog With {
            .Filter = "Microsoft Excel Workbook Documents 2007-13 (*.xlsx)|*.xlsx|" &
                      "Microsoft Excel Documents 97-2003 (*.xls)|*.xls",
            .RestoreDirectory = True
        }

        If Not browseFile.ShowDialog() = DialogResult.OK Then Return

        FileDirectory = browseFile.FileName

    End Sub

    Private Sub btnDownload_Click(sender As Object, e As EventArgs) Handles btnDownload.Click
        DownloadTemplateHelper.Download(ExcelTemplates.Employee)
    End Sub

#End Region

End Class