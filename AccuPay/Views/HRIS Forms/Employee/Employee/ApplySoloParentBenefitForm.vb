Option Strict On

Imports System.IO
Imports System.Threading.Tasks
Imports AccuPay.Core.Entities
Imports AccuPay.Core.Interfaces
Imports Microsoft.Extensions.DependencyInjection

Public Class ApplySoloParentBenefitForm
    Private ReadOnly _orgId As Integer
    Private ReadOnly _singleParentBeneficiaryRepository As ISoloParentBeneficiaryRepository
    Private ReadOnly _employeeRepository As IEmployeeRepository
    Private ReadOnly _payPeriodRepository As IPayPeriodRepository

    Public Sub New(orgId As Integer)
        InitializeComponent()

        _orgId = orgId

        _singleParentBeneficiaryRepository = MainServiceProvider.GetRequiredService(Of ISoloParentBeneficiaryRepository)
        _employeeRepository = MainServiceProvider.GetRequiredService(Of IEmployeeRepository)
        _payPeriodRepository = MainServiceProvider.GetRequiredService(Of IPayPeriodRepository)
    End Sub

    Private Async Sub ApplySingleParentBenefitForm_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        dataGrid.AutoGenerateColumns = False

        Await LoadSingleParentBeneficiariesAsync()

    End Sub

    Public Async Function GetSingleParentBeneficiariesAsync() As Task(Of ICollection(Of SoloParentBeneficiary))
        'Dim singleParentBeneficiaryRepository = MainServiceProvider.GetRequiredService(Of ISingleParentBeneficiaryRepository)
        '_singleParentBeneficiaryRepository
        Return Await _singleParentBeneficiaryRepository.GetAllByOrganizationIdAsync(_orgId)

    End Function

    Public Async Function LoadSingleParentBeneficiariesAsync() As Task
        Dim beneficiaries = Await GetSingleParentBeneficiariesAsync()
        Dim employees = Await GetEmployeesAsync()

        dataGrid.DataSource = employees.
            Select(Function(e) New SoloParentBeneficiaryDto(employee:=e, beneficiaries.FirstOrDefault(Function(t) t.EmployeeId = If(e.RowID, 0)))).
            ToList()

    End Function

    Private Async Function GetEmployeesAsync() As Task(Of ICollection(Of Employee))
        Dim _selectedPayPeriod = Await _payPeriodRepository.GetCurrentPayPeriodAsync(organizationId:=_orgId, z_User)
        Dim periodEndDate = If(_selectedPayPeriod Is Nothing, Date.Now(), _selectedPayPeriod.PayToDate)

        Return (Await _employeeRepository.GetAllWithinServicePeriodWithPositionAsync(
            organizationId:=z_OrganizationID,
            currentDate:=periodEndDate)).
            OrderBy(Function(e) e.FullNameWithMiddleInitialLastNameFirst).
            ToList()

    End Function

    Private Async Sub dataGrid_CellContentClick(sender As Object, e As DataGridViewCellEventArgs) Handles dataGrid.CellContentClick
        If e.ColumnIndex = Column5.Index Then
            Await DirectCast(dataGrid.CurrentRow.DataBoundItem, SoloParentBeneficiaryDto).ViewFileAsync()

        ElseIf e.ColumnIndex = Column6.Index Then
            Dim form = New EditSoloParentBenefitForm(userId:=z_User,
                orgId:=_orgId,
                DirectCast(dataGrid.CurrentRow.DataBoundItem, SoloParentBeneficiaryDto))

            If form.ShowDialog() = DialogResult.OK Then
                Await LoadSingleParentBeneficiariesAsync()

            End If

        End If

    End Sub

End Class

Public Class SoloParentBeneficiaryDto
    Private ReadOnly _t As SoloParentBeneficiary

    Public Sub New(employee As Employee, t As SoloParentBeneficiary)
        _t = If(t Is Nothing, SoloParentBeneficiary.Create(userId:=z_User, orgId:=z_OrganizationID, employeeId:=employee.RowID.Value), t)
        _Employee = employee
        EmployeeNo = employee.EmployeeNo
        LastName = employee.LastName
        FirstName = employee.FirstName
        Validity = If(String.IsNullOrEmpty(t?.AttachmentFileName), "N/A", "✔")
        If Not String.IsNullOrEmpty(t?.AttachmentFileName) Then TempFileName = Path.Combine(Path.GetTempPath(), Path.GetFileName(t?.AttachmentFileName))
        View = "View ID"
        Edit = "Actions"

    End Sub

    Public ReadOnly Property SoloParentBeneficiary As SoloParentBeneficiary
        Get
            Return _t
        End Get
    End Property

    Public ReadOnly Property Employee As Employee
    Public ReadOnly Property EmployeeNo As String
    Public ReadOnly Property LastName As String
    Public ReadOnly Property FirstName As String
    Public ReadOnly Property Validity As String
    Public ReadOnly Property View As String
    Public ReadOnly Property Edit As String
    Public ReadOnly Property TempFileName As String

    Public Async Function ViewFileAsync() As Task
        'If Not If(_t?.HasSingleParentID, False) Then
        '    MessageBoxHelper.Information("No file fetched.")
        '    Return
        'End If

        Await _t?.ViewFileAsync()

    End Function

End Class
