Option Strict On

Imports System.Threading.Tasks
Imports AccuPay.Entity
Imports Microsoft.EntityFrameworkCore

Public Class PersonalInfoTab

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
        'Agency

        TxtTIN.Text = employee.TinNo
        TxtSSSNo.Text = employee.SssNo
        TxtPhilHealthNo.Text = employee.PhilHealthNo
        TxtPagIbig.Text = employee.HdmfNo

        'P ay frequency
        'Status
        'Type
        DtpStartDate.Value = employee.StartDate
        DtpEvaluationDate.Checked = employee.DateEvaluated.HasValue
        If employee.DateEvaluated.HasValue Then
            DtpEvaluationDate.Value = employee.DateEvaluated.Value
        End If
        DtpRegularizationDate.Checked = employee.DateRegularized.HasValue
        If employee.DateRegularized.HasValue Then
            DtpRegularizationDate.Value = employee.DateRegularized.Value
        End If
        'Rest day
    End Sub

    Public Async Function GetPositions() As Task(Of IList(Of Position))
        Using context = New PayrollContext()
            Return Await context.Positions.
                Where(Function(p) Nullable.Equals(p.OrganizationID, z_OrganizationID)).
                ToListAsync()
        End Using
    End Function

End Class
