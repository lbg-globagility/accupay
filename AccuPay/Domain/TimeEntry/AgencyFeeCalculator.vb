Option Strict On

Imports AccuPay
Imports AccuPay.Entity
Imports AccuPay.Utils

Public Class AgencyFeeCalculator

    Private Const HoursPerDay As Decimal = 8

    Private ReadOnly _employee As Employee
    Private ReadOnly _agency As Agency
    Private ReadOnly _agencyFees As IList(Of AgencyFee)

    Public Sub New(employee As Employee, agency As Agency, agencyFees As IList(Of AgencyFee))
        _employee = employee
        _agency = agency
        _agencyFees = agencyFees
    End Sub

    Public Function Compute(timeEntries As IList(Of TimeEntry)) As IList(Of AgencyFee)
        Dim agencyFees = New List(Of AgencyFee)

        For Each timeEntry In timeEntries
            Dim agencyFee = _agencyFees.SingleOrDefault(Function(a) a.Date = timeEntry.Date)

            If agencyFee Is Nothing Then
                agencyFee = New AgencyFee() With {
                    .OrganizationID = _employee.OrganizationID,
                    .EmployeeID = _employee.RowID,
                    .AgencyID = _agency.RowID,
                    .Date = timeEntry.Date}
            End If

            Dim basicHours = AccuMath.CommercialRound(timeEntry.BasicHours)
            Dim hourlyFee = _agency.Fee / HoursPerDay
            agencyFee.Amount = basicHours * hourlyFee

            agencyFees.Add(agencyFee)
        Next

        Return agencyFees
    End Function

End Class