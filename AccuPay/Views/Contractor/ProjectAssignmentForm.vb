Option Strict On

Imports System.Threading.Tasks
Imports AccuPay.Core.Entities
Imports AccuPay.Core.Interfaces

Public Class ProjectAssignmentForm
    Private ReadOnly _userId As Integer

    Public Sub New(userId As Integer)
        _userId = userId

        InitializeComponent()

    End Sub

    Private Async Sub ProjectAssignmentForm_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        gridContractors.AutoGenerateColumns = False
        gridProjects.AutoGenerateColumns = False
        gridEmployees.AutoGenerateColumns = False

        InitButtonsClearSearchBox()

        Await LoadContractorsAsync()

        gridContractors_SelectionChanged(gridContractors, New EventArgs())

        gridContractors_SetSelectionChanged()

    End Sub

    Private Sub InitButtonsClearSearchBox()
        Dim size = Function(tb As TextBox) As Size
                       Return New Size(width:=tb.ClientSize.Height, height:=tb.ClientSize.Height)
                   End Function

        Dim point = Function(tb As TextBox, btn As Button) As Point
                        Return New Point(x:=tb.ClientSize.Width - (btn.Size.Width - 1), y:=0)
                    End Function

        Dim _font = New Font(Font.FontFamily, 7.5!, FontStyle.Regular, GraphicsUnit.Point, CType(0, Byte))

        With btnClearSearch1
            .Size = size(TextBoxSearch1)
            .Location = point(TextBoxSearch1, btnClearSearch1)
            .Font = _font
            .Cursor = Cursors.Default
        End With

        TextBoxSearch1.Controls.Clear()
        TextBoxSearch1.Controls.Add(btnClearSearch1)

        TextBoxSearch1_TextChanged(TextBoxSearch1, New EventArgs())


        With btnClearSearch2
            .Size = size(TextBoxSearch2)
            .Location = point(TextBoxSearch2, btnClearSearch2)
            .Font = _font
            .Cursor = Cursors.Default
        End With

        TextBoxSearch2.Controls.Clear()
        TextBoxSearch2.Controls.Add(btnClearSearch2)

        TextBoxSearch2_TextChanged(TextBoxSearch2, New EventArgs())


        With btnClearSearch3
            .Size = size(TextBoxSearch3)
            .Location = point(TextBoxSearch3, btnClearSearch3)
            .Font = _font
            .Cursor = Cursors.Default
        End With

        TextBoxSearch3.Controls.Clear()
        TextBoxSearch3.Controls.Add(btnClearSearch3)

        TextBoxSearch3_TextChanged(TextBoxSearch3, New EventArgs())

    End Sub

    Private Sub TextBoxSearch1_TextChanged(sender As Object, e As EventArgs) Handles TextBoxSearch1.TextChanged

    End Sub

    Private Sub TextBoxSearch2_TextChanged(sender As Object, e As EventArgs) Handles TextBoxSearch2.TextChanged

    End Sub

    Private Sub TextBoxSearch3_TextChanged(sender As Object, e As EventArgs) Handles TextBoxSearch3.TextChanged

    End Sub

    Private Sub btnClearSearch1_Click(sender As Object, e As EventArgs) Handles btnClearSearch1.Click
        TextBoxSearch1.Clear()

    End Sub

    Private Sub btnClearSearch2_Click(sender As Object, e As EventArgs) Handles btnClearSearch2.Click
        TextBoxSearch2.Clear()

    End Sub

    Private Sub btnClearSearch3_Click(sender As Object, e As EventArgs) Handles btnClearSearch3.Click
        TextBoxSearch3.Clear()

    End Sub

    Private Async Sub LinkLabel9_LinkClicked(sender As Object, e As LinkLabelLinkClickedEventArgs) Handles LinkLabel9.LinkClicked
        Dim form = New ContractorSaveChangesForm(_userId, Contractor.Create(_userId))
        If Not form.ShowDialog() = DialogResult.OK Then Return

        gridContractors_UnsetSelectionChanged()

        Await LoadContractorsAsync()

        gridContractors_SetSelectionChanged()

    End Sub

    Private Sub gridContractors_SetSelectionChanged()
        AddHandler gridContractors.SelectionChanged, AddressOf gridContractors_SelectionChanged

    End Sub

    Private Sub gridContractors_UnsetSelectionChanged()
        RemoveHandler gridContractors.SelectionChanged, AddressOf gridContractors_SelectionChanged

    End Sub

    Private Async Function GetContractorsAsync() As Task(Of List(Of Contractor))
        Dim contractorDataService = GetRequiredService(Of IContractorDataService)()
        Return (Await contractorDataService.GetAllAsync()).ToList()

    End Function

    Private Async Function GetContractorProjectsAsync(contractorId As Integer) As Task(Of List(Of ContractorProject))
        Dim contractorProjectDataService = GetRequiredService(Of IContractorProjectDataService)()
        Return (Await contractorProjectDataService.GetAllAsync()).Where(Function(cp) cp.ContractorId.Value = contractorId).ToList()

    End Function

    Private Async Function LoadContractorsAsync() As Task
        Dim contractors = Await GetContractorsAsync()
        gridContractors.DataSource = contractors

    End Function

    Private Async Function LoadContractorProjectsAsync(contractorId As Integer) As Task
        Dim contractorProjects = Await GetContractorProjectsAsync(contractorId)
        gridProjects.DataSource = contractorProjects

    End Function

    Private Async Function LoadEmployeesAsync(contractorProjectId As Integer) As Task
        Dim projectEmployees = Await GetProjectEmployeesAsync(contractorProjectId)
        gridEmployees.DataSource = projectEmployees

    End Function

    Private Async Function GetProjectEmployeesAsync(contractorProjectId As Integer) As Task(Of List(Of ProjectEmployee))
        Dim projectEmployeeDataService = GetRequiredService(Of IProjectEmployeeDataService)()
        Return (Await projectEmployeeDataService.GetAllByProjectIdAsync(contractorProjectId)).ToList()

    End Function

    Private Sub gridContractors_CellContentClick(sender As Object, e As DataGridViewCellEventArgs) Handles gridContractors.CellContentClick

    End Sub

    Private Async Sub gridContractors_SelectionChanged(sender As Object, e As EventArgs)
        If gridContractors.CurrentRow Is Nothing Then Return

        Dim contractor = CType(gridContractors.CurrentRow.DataBoundItem, Contractor)

        gridContractorProjects_UnsetSelectionChanged()

        Await LoadContractorProjectsAsync(contractor.RowID.Value)

        gridProjects_SetSelectionChanged()

        gridProjects_SelectionChanged(gridProjects, New EventArgs())

    End Sub

    Private Async Sub gridContractors_CellDoubleClick(sender As Object, e As DataGridViewCellEventArgs) Handles gridContractors.CellDoubleClick
        If gridContractors.CurrentRow Is Nothing Then Return

        Dim form = New ContractorSaveChangesForm(_userId, CType(gridContractors.CurrentRow.DataBoundItem, Contractor))
        If Not form.ShowDialog() = DialogResult.OK Then Return

        gridContractors_UnsetSelectionChanged()

        Await LoadContractorsAsync()

        gridContractors_SetSelectionChanged()

    End Sub

    Private Async Sub LinkLabel10_LinkClicked(sender As Object, e As LinkLabelLinkClickedEventArgs) Handles LinkLabel10.LinkClicked
        If gridContractors.CurrentRow Is Nothing Then Return

        Dim contractor = CType(gridContractors.CurrentRow.DataBoundItem, Contractor)

        Dim dummyContractorProject = ContractorProject.Create(userId:=_userId, contractor.RowID.Value, beginDate:=Date.Now)

        Dim form = New ContractorProjectSaveChangesForm(_userId, dummyContractorProject)
        If Not form.ShowDialog() = DialogResult.OK Then Return

        gridContractorProjects_UnsetSelectionChanged()

        Await LoadContractorProjectsAsync(contractor.RowID.Value)

        gridProjects_SetSelectionChanged()

    End Sub

    Private Sub gridProjects_SetSelectionChanged()
        AddHandler gridProjects.SelectionChanged, AddressOf gridProjects_SelectionChanged

    End Sub

    Private Sub gridContractorProjects_UnsetSelectionChanged()
        RemoveHandler gridProjects.SelectionChanged, AddressOf gridProjects_SelectionChanged

    End Sub

    Private Sub gridProjects_CellContentClick(sender As Object, e As DataGridViewCellEventArgs) Handles gridProjects.CellContentClick

    End Sub

    Private Async Sub gridProjects_SelectionChanged(sender As Object, e As EventArgs)
        If gridContractors.CurrentRow Is Nothing AndAlso gridProjects.CurrentRow Is Nothing Then Return

        Dim contractorProject = CType(gridProjects.CurrentRow.DataBoundItem, ContractorProject)

        Await LoadEmployeesAsync(contractorProject.RowID.Value)

    End Sub

    Private Async Sub gridProjects_CellDoubleClick(sender As Object, e As DataGridViewCellEventArgs) Handles gridProjects.CellDoubleClick
        If gridContractors.CurrentRow Is Nothing AndAlso gridProjects.CurrentRow Is Nothing Then Return

        Dim contractorProject = CType(gridProjects.CurrentRow.DataBoundItem, ContractorProject)

        Dim form = New ContractorProjectSaveChangesForm(_userId, contractorProject)
        If Not form.ShowDialog() = DialogResult.OK Then Return

        gridContractorProjects_UnsetSelectionChanged()

        Await LoadContractorProjectsAsync(CType(gridContractors.CurrentRow.DataBoundItem, Contractor).RowID.Value)

        gridProjects_SetSelectionChanged()

    End Sub

    Private Sub gridEmployees_CellContentClick(sender As Object, e As DataGridViewCellEventArgs) Handles gridEmployees.CellContentClick

    End Sub

    Private Sub gridEmployees_SelectionChanged(sender As Object, e As EventArgs)

    End Sub

    Private Async Sub gridEmployees_CellDoubleClick(sender As Object, e As DataGridViewCellEventArgs) Handles gridEmployees.CellDoubleClick
        If gridContractors.CurrentRow Is Nothing AndAlso gridProjects.CurrentRow Is Nothing Then Return

        Dim projectEmployee = CType(gridEmployees.CurrentRow.DataBoundItem, ProjectEmployee)

        Dim form = New ProjectEmployeeSaveChangesForm(userId:=_userId, projectEmployee:=projectEmployee)
        If Not form.ShowDialog() = DialogResult.OK Then Return

        Await LoadEmployeesAsync(CType(gridProjects.CurrentRow.DataBoundItem, ContractorProject).RowID.Value)

    End Sub

    Private Async Sub LinkLabel11_LinkClicked(sender As Object, e As LinkLabelLinkClickedEventArgs) Handles LinkLabel11.LinkClicked
        If gridContractors.CurrentRow Is Nothing AndAlso gridProjects.CurrentRow Is Nothing Then Return

        Dim contractor = CType(gridContractors.CurrentRow.DataBoundItem, Contractor)

        Dim contractorProject = CType(gridProjects.CurrentRow.DataBoundItem, ContractorProject)

        Dim dummyProjectEmployee = ProjectEmployee.Create(userId:=_userId, projectId:=contractorProject.RowID.Value, beginDate:=Date.Now)

        Dim form = New ProjectEmployeeSaveChangesForm(userId:=_userId, projectEmployee:=dummyProjectEmployee)
        If Not form.ShowDialog() = DialogResult.OK Then Return

        Await LoadEmployeesAsync(contractorProject.RowID.Value)

    End Sub

End Class
