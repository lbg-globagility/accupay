Public Class PFF_053Form
    Dim dt As New DataTable
    Dim rpt As New DSReportList.PFF053DataTable
    Dim crys As New CrystalReport2
    Dim objReport As Object

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles btnPrint.Click
        'objReport.Sections("PageHeaderSection3").ReportObjects("Picture1").XScaling = 1
        'objReport.Sections("PageHeaderSection3").ReportObjects("Picture1").YScaling = 0.5

        For Each drow As DataGridViewRow In dgvemplist.Rows
            With drow
                If drow.Cells(0).Value = Nothing Then
                Else
                    rpt.AddPFF053Row(txtEmpID.Text, txtEmpbName.Text, txtunitroomflr.Text, txtbuildingname.Text, txtlotblockphasehouse.Text, _
                                         txtstname.Text, txtsubdivision.Text, txtbrgy.Text, txtmunicipalitycity.Text, txtprovincestatecountry.Text, _
                                         txtzipcode.Text, drow.Cells(c_pagibigmidno.Index).Value, drow.Cells(c_acctno.Index).Value, drow.Cells(c_membershipprog.Index).Value, _
                                         drow.Cells(c_lname.Index).Value, drow.Cells(c_fname.Index).Value, drow.Cells(c_nameexit.Index).Value, drow.Cells(c_mname.Index).Value, _
                                         drow.Cells(c_periodcovered.Index).Value, drow.Cells(c_monthlycompensation.Index).Value, drow.Cells(c_eeshare.Index).Value, drow.Cells(c_ershare.Index).Value, _
                                         drow.Cells(c_total.Index).Value, drow.Cells(c_remarks.Index).Value, drow.Cells(c_remarks.Index).Value, "", "", "", "", "")

                End If
            End With
          
        Next



        Dim newrep As New ReportForm
        newrep.CrystalReportViewer1.ReportSource = crys
        dt = rpt
        crys.SetDataSource(dt)

        'newrep.TopLevel = False
        'MDImain.Panel1.Controls.Add(newrep)
        newrep.Show()
    End Sub
End Class