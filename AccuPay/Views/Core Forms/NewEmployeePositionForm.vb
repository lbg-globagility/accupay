Option Strict On

Imports System.Threading.Tasks
Imports AccuPay.Entity
Imports AccuPay.Extensions
Imports AccuPay.JobLevels
Imports AccuPay.Repository
Imports AccuPay.Utils
Imports Microsoft.EntityFrameworkCore

Public Class NewEmployeePositionForm

    Private _divisions As New List(Of Division)

    Private _positions As New List(Of Position)

    Private _jobLevels As New List(Of JobLevel)

    Private _currentDivision As New Division

    Private _currentPosition As New Position

    Private _positionRepository As New PositionRepository

    Public Property _currentTreeNodes As TreeNode()

    Private Async Sub NewEmployeePositionForm_Load(sender As Object, e As EventArgs) Handles Me.Load

        z_OrganizationID = 3

        Await GetJobLevels()

        Await GetDivisions()

        Await GetPositions()

        LoadTreeView()

        HideFormsTabControlHeader()

        FormsTabControl.Visible = True

    End Sub

    Private Sub PositionTreeView_AfterSelect(sender As Object, e As TreeViewEventArgs) Handles PositionTreeView.AfterSelect

        ClearForms()

        Dim selectedNode = PositionTreeView.SelectedNode

        If selectedNode Is Nothing OrElse selectedNode.Tag Is Nothing Then Return

        Dim selectedNodeData = CType(selectedNode.Tag, NodeData)

        If selectedNodeData.ObjectType = ObjectType.Division Then

            Dim selectedDivision = _divisions.
                    Where(Function(d) Nullable.Equals(d.RowID, selectedNodeData.Id)).
                    FirstOrDefault

            If selectedDivision Is Nothing Then Return

            ShowDivisionForm(selectedDivision)

        ElseIf selectedNodeData.ObjectType = ObjectType.Position Then

            Dim selectedPosition = _positions.
                    Where(Function(d) Nullable.Equals(d.RowID, selectedNodeData.Id)).
                    FirstOrDefault

            If selectedPosition Is Nothing Then Return

            ShowPositionForm(selectedPosition)

        End If

    End Sub

    Private Sub SaveDivisionToolStripButton_Click(sender As Object, e As EventArgs) Handles SaveDivisionToolStripButton.Click

    End Sub

    Private Async Sub SavePositionToolStripButton_Click(sender As Object, e As EventArgs) Handles SavePositionToolStripButton.Click

        'removing focus to PositionUserControl1 updates the databind
        PositionGroupBox.Focus()

        Dim messageTitle = "Save Position"

        If ValidatePosition(messageTitle) = False Then

            Return

        End If

        Await FunctionUtils.TryCatchFunctionAsync(messageTitle,
                          Async Function()
                              Await SavePosition(messageTitle)
                          End Function)

    End Sub

    Private Async Sub DeletePositionToolStripButton_Click(sender As Object, e As EventArgs) _
        Handles DeletePositionToolStripButton.Click

        If Me._currentPosition Is Nothing Then
            MessageBoxHelper.Warning("No position selected!")

            Return
        End If

        Dim messageTitle = "Delete Position"

        Await FunctionUtils.TryCatchFunctionAsync(messageTitle,
                          Async Function()
                              Await DeletePosition(messageTitle)
                          End Function)

    End Sub

    Private Sub CancelPositionToolStripButton_Click(sender As Object, e As EventArgs) _
        Handles CancelPositionToolStripButton.Click

        If Me._currentPosition Is Nothing Then
            MessageBoxHelper.Warning("No position selected!")

            Return
        End If

        Dim unchangedPosition = _positions.
        FirstOrDefault(Function(p) Nullable.Equals(p.RowID, Me._currentPosition.RowID))

        If unchangedPosition Is Nothing Then
            MessageBoxHelper.Warning("No position selected!")

            Return
        End If

        SetPosition(unchangedPosition)

    End Sub

    Private Sub DeleteDivisionToolStripButton_Click(sender As Object, e As EventArgs) _
        Handles DeleteDivisionToolStripButton.Click

    End Sub

    Private Sub CancelDivisionToolStripButton_Click(sender As Object, e As EventArgs) _
        Handles CancelDivisionToolStripButton.Click

    End Sub

#Region "Private Methods"
    Private Sub HideFormsTabControlHeader()
        FormsTabControl.Appearance = TabAppearance.FlatButtons
        FormsTabControl.ItemSize = New Size(0, 1)
        FormsTabControl.SizeMode = TabSizeMode.Fixed
    End Sub

    Private Sub LoadTreeView()

        Dim treeNode = GenerateTreeView()

        Me._currentTreeNodes = treeNode.ToArray

        PositionTreeView.Nodes.AddRange(Me._currentTreeNodes)

        If treeNode.Count > 0 Then

            PositionTreeView.SelectedNode = Me._currentTreeNodes(0)

        End If

    End Sub

    Private Function GenerateTreeView() As List(Of TreeNode)

        Dim parentDivisionNodes As New List(Of TreeNode)

        Dim parentDivisions = _divisions.
                                Where(Function(d) d.IsRoot).
                                ToList

        ' parent division
        For Each parentDivision In parentDivisions

            Dim parentDivisionNode = CreateNode(
                                        parentDivision.RowID,
                                        parentDivision.Name,
                                        ObjectType.Division)

            Dim childDivisions = _divisions.
                                    Where(Function(d) Nullable.Equals(d.ParentDivisionID, parentDivision.RowID)).
                                    ToList

            'child divisions
            For Each childDivision In childDivisions

                Dim childDivisionNode = CreateNode(
                                            childDivision.RowID,
                                            childDivision.Name,
                                            ObjectType.Division)

                'positions
                Dim positions = _positions.
                                    Where(Function(p) Nullable.Equals(p.DivisionID, childDivision.RowID)).
                                    ToList

                For Each position In positions

                    Dim positionNode = CreateNode(
                                                position.RowID,
                                                position.Name,
                                                ObjectType.Position)

                    childDivisionNode.Nodes.Add(positionNode)

                Next

                parentDivisionNode.Nodes.Add(childDivisionNode)

            Next

            parentDivisionNodes.Add(parentDivisionNode)

        Next

        Return parentDivisionNodes

    End Function

    Private Function CreateNode(RowId As Integer?, Name As String, objectType As ObjectType) As TreeNode
        Return New TreeNode With {
                        .Text = Name,
                        .Tag = New NodeData With {
                                    .Id = RowId,
                                    .ObjectType = objectType
                        },
                        .ImageIndex = 4
        }
    End Function

    Private Async Function GetJobLevels() As Task

        Using context As New PayrollContext

            _jobLevels = Await context.JobLevels.
                            Where(Function(j) Nullable.Equals(j.OrganizationID, z_OrganizationID)).
                            OrderBy(Function(j) j.Name).
                            ToListAsync

        End Using

    End Function

    Private Async Function GetDivisions() As Task

        Using context As New PayrollContext

            _divisions = Await context.Divisions.
                            Where(Function(d) Nullable.Equals(d.OrganizationID, z_OrganizationID)).
                            OrderBy(Function(d) d.Name).
                            ToListAsync

        End Using

    End Function

    Private Async Function GetPositions() As Task

        Using context As New PayrollContext

            _positions = Await context.Positions.
                            Where(Function(p) Nullable.Equals(p.OrganizationID, z_OrganizationID)).
                            OrderBy(Function(p) p.Name).
                            ToListAsync

        End Using

    End Function

    Private Sub ShowDivisionForm(selectedDivision As Division)

        Dim isRoot = selectedDivision.IsRoot

        ChangeDivisionToolStripButtonsDescription(isRoot)

        _currentDivision = selectedDivision.CloneJson()

        If isRoot Then

            DivisionLocationTextBox.DataBindings.Add("Text", Me._currentDivision, "Name")

        End If

        DivisionLocationGroupBox.Visible = isRoot
        DivisionUserControl1.Visible = Not isRoot

        FormsTabControl.SelectedTab = DivisionTabPage

    End Sub

    Private Async Sub ShowPositionForm(selectedPosition As Position)

        SetPosition(selectedPosition)

        EmployeeDataGrid.DataSource = Await _positionRepository.GetEmployeesAsync(selectedPosition.RowID)

        FormsTabControl.SelectedTab = PositionTabPage

    End Sub

    Private Sub SetPosition(selectedPosition As Position)

        Dim allChildDivisions = _divisions.Where(Function(d) d.IsRoot = False).ToList

        Me._currentPosition = selectedPosition

        PositionUserControl1.SetPosition(Me._currentPosition, allChildDivisions, _jobLevels)
    End Sub

    Private Sub ClearForms()

        DivisionLocationTextBox.DataBindings.Clear()

    End Sub

    Private Sub ChangeDivisionToolStripButtonsDescription(isRoot As Boolean)
        Dim newDescription = "New Division"
        Dim saveDescription = "Save Division"
        Dim deleteDescription = "Delete Division"
        Dim cancelDescription = "Cancel Division"

        If isRoot Then

            newDescription &= " Location"
            saveDescription &= " Location"
            deleteDescription &= " Location"
            cancelDescription &= " Location"

        End If

        NewDivisionToolStripButton.Text = newDescription
        SaveDivisionToolStripButton.Text = saveDescription
        DeleteDivisionToolStripButton.Text = deleteDescription
        CancelDivisionToolStripButton.Text = cancelDescription
    End Sub

    Private Sub ShowBalloonInfo(content As String, title As String)
        myBalloon(content, title, lblFormTitle, 400)
    End Sub

    Private Function GetTreeNode(rowId As Integer?, objectType As ObjectType) As TreeNode

        If ObjectType.Division = objectType Then

            Return GetTreeNodeDivision(rowId)

        ElseIf ObjectType.Position = objectType Then

            Return GetTreeNodePosition(rowId)

        End If

        Return Nothing


    End Function

    Private Function GetTreeNodeDivision(rowId As Integer?) As TreeNode

        For Each parentDivisionNode As TreeNode In _currentTreeNodes

            If CheckNodeId(rowId, ObjectType.Division, parentDivisionNode) Then

                Return parentDivisionNode

            End If

            For Each childDivisionNode As TreeNode In parentDivisionNode.Nodes

                If CheckNodeId(rowId, ObjectType.Division, childDivisionNode) Then

                    Return childDivisionNode

                End If

            Next

        Next

        Return Nothing

    End Function

    Private Function CheckNodeId(
                        rowId As Integer?,
                        objectType As ObjectType,
                        treeNode As TreeNode) As Boolean

        Dim treeNodeTag = CType(treeNode.Tag, NodeData)

        If treeNode Is Nothing Then Return False

        If Nullable.Equals(treeNodeTag.Id, rowId) AndAlso
            treeNodeTag.ObjectType = objectType Then

            Return True

        End If

        Return False

    End Function

    Private Function GetTreeNodePosition(rowId As Integer?) As TreeNode
        For Each parentDivisionNode As TreeNode In _currentTreeNodes

            For Each childDivisionNode As TreeNode In parentDivisionNode.Nodes

                For Each positionNode As TreeNode In childDivisionNode.Nodes

                    If CheckNodeId(rowId, ObjectType.Position, positionNode) Then

                        Return positionNode

                    End If

                Next

            Next

        Next

        Return Nothing

    End Function

    Private Function ValidatePosition(messageTitle As String) As Boolean

        If Me._currentPosition Is Nothing Then

            MessageBoxHelper.ErrorMessage("No selected position!", messageTitle)
            Return False

        ElseIf Me._currentPosition.DivisionID Is Nothing Then

            PositionUserControl1.ShowError("DivisionID", "Please select a division")

        ElseIf String.IsNullOrWhiteSpace(Me._currentPosition.Name) Then

            PositionUserControl1.ShowError("Name", "Please enter a name")
            Return False

        End If

        Return True

    End Function

    Private Async Function SavePosition(messageTitle As String) As Task

        Await _positionRepository.SaveAsync(Me._currentPosition)

        Dim currentTreeNode As TreeNode = GetTreeNode(Me._currentPosition.RowID, ObjectType.Position)

        currentTreeNode.Text = Me._currentPosition.Name

        ShowBalloonInfo($"Position: {Me._currentPosition.Name} was successfully updated.", messageTitle)

    End Function

    Private Async Function DeletePosition(messageTitle As String) As Task

        Dim positionName = Me._currentPosition.Name

        Await _positionRepository.DeleteAsync(Me._currentPosition.RowID)

        PositionTreeView.Nodes.Remove(PositionTreeView.SelectedNode)

        _positions.Remove(Me._currentPosition)

        ShowBalloonInfo($"Position: {positionName} was successfully deleted.", messageTitle)

    End Function

#End Region

    Private Structure NodeData

        Property Id As Integer?
        Property ObjectType As ObjectType

    End Structure

    Private Enum ObjectType
        Division
        Position
    End Enum

End Class