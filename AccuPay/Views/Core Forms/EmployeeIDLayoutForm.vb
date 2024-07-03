Imports System.Drawing.Printing
Imports AccuPay.Core.Interfaces
Imports CrystalDecisions.[Shared].Json
Imports Microsoft.Extensions.DependencyInjection
Imports AccuPay.Infrastructure.Data
Imports AccuPay.Core.Entities
Imports Xceed.Document.NET
Imports Xceed.Words.NET
Imports AccuPay.Desktop.Helpers
Imports Microsoft.Office.Interop.Excel
Imports System.Net.Mime.MediaTypeNames

Public Class EmployeeIDLayoutForm

    Private ReadOnly _employeeRepository As IEmployeeRepository
    Private ReadOnly _context As PayrollContext




    Public Sub New(employeeId As Integer, position As String, status As String)

        ' This call is required by the designer.
        InitializeComponent()

        _employeeRepository = MainServiceProvider.GetRequiredService(Of IEmployeeRepository)

        Dim employee = _employeeRepository.GetById(employeeId)
        EmployeeName.Text = employee.FirstName
        EmployeeSurName.Text = employee.LastName
        EmpPicture.Image = Nothing
        EmpPicture.Image = ConvByteToImage(DirectCast(employee.Image, Byte()))
        EmpExpireDate.Text = employee.TerminationDate?.ToString("MMMM dd, yyyy")
        EmpIDNo.Text = employee.EmployeeNo
        EmpBirthdate.Text = employee.BirthDate.ToString("MMMM dd, yyyy")
        EmpAddress.Text = employee.HomeAddress
        EmpContactNo.Text = employee.MobilePhone
        EmpSSS.Text = employee.SssNo
        EmpPhilhealth.Text = employee.PhilHealthNo
        EmpHDMF.Text = employee.HdmfNo
        EmpTIN.Text = employee.TinNo
        EmpPosition.Text = position


        Dim gp = New Drawing2D.GraphicsPath
        gp.AddEllipse(0, 0, EmpPicture.Width - 3, EmpPicture.Height - 3)
        Dim rg = New Region(gp)
        EmpPicture.Region = rg
        PictureBox1.Image = My.Resources.Resources.RegularID_F
        PictureBox2.Image = My.Resources.Resources.RegularID
        EmpExpireDate.BackColor = Color.FromArgb(42, 55, 75)
        EmpIDNo.BackColor = Color.FromArgb(1, 2, 18)
        'If (status.ToLower() = "regular") Then
        '    PictureBox1.Image = My.Resources.Resources.RegularID_F
        '    PictureBox2.Image = My.Resources.Resources.RegularID
        '    EmpExpireDate.BackColor = Color.FromArgb(42, 55, 75)
        '    EmpIDNo.BackColor = Color.FromArgb(1, 2, 18)
        'ElseIf (status.ToLower() = "probationary") Then

        '    PictureBox1.Image = My.Resources.Resources.ProbitionaryID_F
        '    PictureBox2.Image = My.Resources.Resources.ProbitionaryID
        '    EmpExpireDate.BackColor = Color.FromArgb(247, 167, 27)
        '    EmpIDNo.BackColor = Color.FromArgb(56, 33, 0)
        'ElseIf (status.ToLower() = "seasonal") Then
        '    'trainee
        '    PictureBox1.Image = My.Resources.Resources.TraineeID_F
        '    PictureBox2.Image = My.Resources.Resources.TraineeID
        '    EmpExpireDate.BackColor = Color.FromArgb(237, 28, 36)
        '    EmpIDNo.BackColor = Color.FromArgb(55, 0, 0)
        'End If

        ' Add any initialization after the InitializeComponent() call.

    End Sub


    Private Sub PrintBtn_Click(sender As Object, e As EventArgs) Handles PrintBtn.Click





        Dim tmpImg As New Bitmap(DataGridView1.Width, DataGridView1.Height)
        Using g As Graphics = Graphics.FromImage(tmpImg)
            g.CopyFromScreen(DataGridView1.PointToScreen(New System.Drawing.Point(0, 0)), New System.Drawing.Point(0, 0), New Size(DataGridView1.Width, DataGridView1.Height))
        End Using
        tmpImg.Save("Resources/IDLayout.png", System.Drawing.Imaging.ImageFormat.Png)

        Dim filename = "EmployeeIDLayout.docx"
        Dim document = DocX.Create(filename)
        Dim p1 = document.InsertParagraph()
        Dim imge = document.AddImage($"Resources/IDLayout.png")
        Dim picture = imge.CreatePicture()
        p1.AppendPicture(picture)


        document.Save(filename)
        Dim saveFileDialogHelperOutPut = SaveFileDialogHelper.BrowseFile(filename, "docx", "DOCX(*.docx)|*.docx;")

        If saveFileDialogHelperOutPut.IsSuccess = False Then Return

        System.IO.File.Copy(filename, saveFileDialogHelperOutPut.FileInfo.FullName)

        System.Diagnostics.Process.Start(saveFileDialogHelperOutPut.FileInfo.FullName)

    End Sub

End Class
