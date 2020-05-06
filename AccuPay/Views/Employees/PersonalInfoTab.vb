Option Strict On

Imports System.Threading.Tasks
Imports AccuPay.Data.Entities
Imports AccuPay.Data.Repositories

Public Class PersonalInfoTab

    Private Sub PersonalInfoTab_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        If DesignMode Then
            Return
        End If

        LoadPositions()
    End Sub

    Public Async Sub LoadPositions()
        CboPosition.DataSource = Await GetPositions()
    End Sub

    Public Sub SetEmployee(employee As Employee)
        TxtEmployeeNo.Text = employee.EmployeeNo
        TxtLastName.Text = employee.LastName
        TxtFirstName.Text = employee.FirstName
        TxtMiddleName.Text = employee.MiddleName
        TxtSurname.Text = employee.Surname
        TxtNickname.Text = employee.Nickname
        DtpBirthDate.Value = employee.BirthDate
        ' TODO: Marital Status
        ' Gender
        TxtNoOfDependents.Text = If(employee.NoOfDependents.HasValue, CStr(employee.NoOfDependents), String.Empty)
        ' Pay Frequency

        TxtEmailAddress.Text = employee.EmailAddress
        TxtHomeAddress.Text = employee.HomeAddress
        TxtWorkPhoneNo.Text = employee.WorkPhone
        TxtHomePhoneNo.Text = employee.HomePhone
        TxtMobilePhoneNo.Text = employee.MobilePhone

        'Division
        'Position
        CboPosition.SelectedValue = employee.PositionID
        'Agency

        TxtTIN.Text = employee.TinNo
        TxtSSSNo.Text = employee.SssNo
        TxtPhilHealthNo.Text = employee.PhilHealthNo
        TxtPagIbig.Text = employee.HdmfNo

        'P ay frequency
        'Status
        CboEmployeeType.Text = employee.EmployeeType
        DtpStartDate.Value = employee.StartDate
        DtpEvaluationDate.Checked = employee.DateEvaluated.HasValue
        If employee.DateEvaluated.HasValue Then
            DtpEvaluationDate.Value = employee.DateEvaluated.Value
        End If
        DtpRegularizationDate.Checked = employee.DateRegularized.HasValue
        If employee.DateRegularized.HasValue Then
            DtpRegularizationDate.Value = employee.DateRegularized.Value
        End If

        CboRestDay.Text = If(employee.DayOfRest.HasValue, WeekdayName(employee.DayOfRest.Value), String.Empty)
    End Sub

    Public Async Function GetPositions() As Task(Of IList(Of PositionDto))
        Dim positionDtos = New List(Of PositionDto) From {
            New PositionDto(Nothing)}

        Dim positionRepository As New PositionRepository
        Dim positions = Await positionRepository.GetAllAsync(z_OrganizationID)

        positionDtos.AddRange(positions.Select(Function(p) New PositionDto(p)))

        Return positionDtos
    End Function

    Public Class PositionDto

        Private ReadOnly _position As Position

        Public Sub New(position As Position)
            _position = position
        End Sub

        Public ReadOnly Property RowID As Integer?
            Get
                Return _position?.RowID
            End Get
        End Property

        Public ReadOnly Property Name As String
            Get
                Return _position?.Name
            End Get
        End Property

    End Class

End Class