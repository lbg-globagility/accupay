Option Strict On

Imports System.ComponentModel
Imports System.Threading.Tasks
Imports AccuPay.Entity
Imports AccuPay.Extensions
Imports AccuPay.JobLevels
Imports AccuPay.Repository
Imports AccuPay.Utils

Public Class NewDivisionPositionForm

    Private _divisions As New List(Of Division)

    Private _positions As New List(Of Position)

    Private _jobLevels As New List(Of JobLevel)

    Private _payFrequencies As New List(Of PayFrequency)

    Private _divisionTypes As List(Of String)

    Private _deductionSchedules As List(Of String)

    Private _currentDivision As New Division

    Private _currentPosition As New Position

    Private _jobLevelRepository As New JobLevelRepository

    Private _positionRepository As New PositionRepository

    Private _divisionRepository As New DivisionRepository

    Private _payFrequencyRepository As New PayFrequencyRepository

    Public Property _currentTreeNodes As TreeNode()

    Private Async Sub NewEmployeePositionForm_Load(sender As Object, e As EventArgs) Handles Me.Load

        EmployeeDataGrid.AutoGenerateColumns = False

        Await LoadPayFrequencies()

        GetDivisionTypes()

        GetDeductionSchedules()

        Await RefreshTreeView()

        If ImageList1.Images.Count > 0 Then

            AddDivisionLocationToolStripMenuItem.Image = ImageList1.Images(0)
            AddDivisionToolStripMenuItem.Image = ImageList1.Images(1)
            AddPositionToolStripMenuItem.Image = ImageList1.Images(2)

        End If

        HideFormsTabControlHeader()

        FormsTabControl.Visible = True

    End Sub

    Private Async Function RefreshTreeView() As Task
        Await LoadJobLevels()

        Await LoadDivisions()

        Await LoadPositions()

        LoadTreeView()
    End Function

    Private Sub DivisionPositionTreeView_AfterSelect(sender As Object, e As TreeViewEventArgs) Handles DivisionPositionTreeView.AfterSelect

        ClearForms()

        Dim selectedNode = DivisionPositionTreeView.SelectedNode

        If selectedNode Is Nothing OrElse selectedNode.Tag Is Nothing Then Return

        Dim selectedNodeData = CType(selectedNode.Tag, NodeData)

        If selectedNodeData.ObjectType = ObjectType.Division Then

            Dim selectedDivision = _divisions.
                    FirstOrDefault(Function(d) Nullable.Equals(d.RowID, selectedNodeData.Id))


            If selectedDivision Is Nothing Then Return

            ShowDivisionForm(selectedDivision)

        ElseIf selectedNodeData.ObjectType = ObjectType.Position Then

            Dim selectedPosition = _positions.
                    FirstOrDefault(Function(d) Nullable.Equals(d.RowID, selectedNodeData.Id))


            If selectedPosition Is Nothing Then Return

            ShowPositionForm(selectedPosition)

        End If

    End Sub

    Private Async Sub SaveDivisionToolStripButton_Click(sender As Object, e As EventArgs) Handles SaveDivisionToolStripButton.Click

        'removing focus to DivisionUserControl1 updates the databind
        DivisionPositionTreeView.Focus()

        Dim messageTitle = "Save Division"
        Dim isRoot = False

        Dim selectedDivision = _divisions.
                    FirstOrDefault(Function(d) Nullable.Equals(d.RowID, Me._currentDivision.RowID))

        If selectedDivision.IsRoot Then

            messageTitle = "Save Division Location"
            isRoot = True
        End If

        If ValidateDivision(messageTitle, isRoot) = False Then

            Return

        End If

        Await FunctionUtils.TryCatchFunctionAsync(messageTitle,
                          Async Function()
                              Await SaveDivision(messageTitle)
                          End Function)

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

        If MessageBoxHelper.Confirm(Of Boolean) _
        ($"Are you sure you want to delete position: {Me._currentPosition.Name}?", "Confirm Deletion") = False Then

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

    Private Async Sub DeleteDivisionToolStripButton_Click(sender As Object, e As EventArgs) _
        Handles DeleteDivisionToolStripButton.Click

        If Me._currentDivision Is Nothing Then
            MessageBoxHelper.Warning("No division selected!")

            Return
        End If

        If MessageBoxHelper.Confirm(Of Boolean) _
        ($"Are you sure you want to delete division: {Me._currentDivision.Name}?", "Confirm Deletion") = False Then

            Return
        End If

        Dim messageTitle = "Delete Division"

        Await FunctionUtils.TryCatchFunctionAsync(messageTitle,
                          Async Function()
                              Await DeleteDivision(messageTitle)
                          End Function,
                          "Division already has transactions in the system therefore cannot be deleted")

    End Sub

    Private Sub CancelDivisionToolStripButton_Click(sender As Object, e As EventArgs) _
        Handles CancelDivisionToolStripButton.Click

        If Me._currentDivision Is Nothing Then
            MessageBoxHelper.Warning("No division selected!")

            Return
        End If

        Dim unchangedDivision = _divisions.
        FirstOrDefault(Function(p) Nullable.Equals(p.RowID, Me._currentDivision.RowID))

        If unchangedDivision Is Nothing Then
            MessageBoxHelper.Warning("No division selected!")

            Return
        End If

        SetDivision(unchangedDivision)
    End Sub

    Private Async Sub AddDivisionLocationToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles AddDivisionLocationToolStripMenuItem.Click

        Await InsertDivisionLocation()

    End Sub

    Private Async Sub AddDivisionToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles AddDivisionToolStripMenuItem.Click

        Await InsertDivision()

    End Sub

    Private Async Function InsertDivisionLocation() As Task
        Dim form As New AddDivisionLocationForm()
        form.ShowDialog()

        If form.IsSaved Then

            Await RefreshTreeView()

            Me._currentDivision = _divisions.FirstOrDefault(Function(d) Nullable.Equals(d.RowID, form.NewDivision.RowID))

            Dim currentTreeNode As TreeNode = GetTreeNode(Me._currentDivision.RowID, ObjectType.Division)

            DivisionPositionTreeView.SelectedNode = currentTreeNode

            ShowBalloonInfo($"Division Location: {Me._currentDivision.Name} was successfully added.", "Division Location Added")

        End If
    End Function

    Private Async Function InsertDivision() As Task
        Dim form As New AddDivisionForm()
        form.ShowDialog()

        If form.IsSaved Then

            Await RefreshTreeView()

            Me._currentDivision = _divisions.FirstOrDefault(Function(d) Nullable.Equals(d.RowID, form.LastDivisionAdded.RowID))

            Dim currentTreeNode As TreeNode = GetTreeNode(Me._currentDivision.RowID, ObjectType.Division)

            DivisionPositionTreeView.SelectedNode = currentTreeNode

            If form.ShowBalloonSuccess Then

                ShowBalloonInfo($"Division: {Me._currentDivision.Name} was successfully added.", "New Division")

            End If

        End If
    End Function

    Private Async Sub NewPositionToolStripButton_Click(sender As Object, e As EventArgs) Handles AddPositionToolStripMenuItem.Click

        Dim form As New AddPositionForm
        form.ShowDialog()

        If form.IsSaved Then

            Await RefreshTreeView()

            Me._currentPosition = _positions.FirstOrDefault(Function(p) Nullable.Equals(p.RowID, form.LastPositionAdded.RowID))

            Dim currentTreeNode As TreeNode = GetTreeNode(Me._currentPosition.RowID, ObjectType.Position)

            DivisionPositionTreeView.SelectedNode = currentTreeNode

            If form.ShowBalloonSuccess Then

                ShowBalloonInfo($"Position: {Me._currentPosition.Name} was successfully added.", "New Position")

            End If
        End If

    End Sub

#Region "Private Methods"
    Private Sub HideFormsTabControlHeader()
        FormsTabControl.Appearance = TabAppearance.FlatButtons
        FormsTabControl.ItemSize = New Size(0, 1)
        FormsTabControl.SizeMode = TabSizeMode.Fixed
    End Sub

    Private Sub LoadTreeView()

        DivisionPositionTreeView.Nodes.Clear()

        Dim treeNode = GenerateTreeView()

        Me._currentTreeNodes = treeNode.ToArray

        DivisionPositionTreeView.Nodes.AddRange(Me._currentTreeNodes)

        If treeNode.Count > 0 Then

            DivisionPositionTreeView.SelectedNode = Me._currentTreeNodes(0)

            If String.IsNullOrWhiteSpace(SearchToolStripTextBox.Text) = False Then

                DivisionPositionTreeView.ExpandAll()

            End If

        End If

    End Sub

    Private Function GenerateTreeView() As List(Of TreeNode)

        Dim parentDivisionImageIndex = 0
        Dim divisionImageIndex = 1
        Dim positionImageIndex = 2

        Dim parentDivisionNodes As New List(Of TreeNode)

        Dim parentDivisions = _divisions.
                                Where(Function(d) d.IsRoot).
                                ToList

        ' parent division
        For Each parentDivision In parentDivisions

            Dim parentDivisionNode = CreateNode(
                                        parentDivision.RowID,
                                        parentDivision.Name,
                                        ObjectType.Division,
                                        parentDivisionImageIndex)

            Dim childDivisions = _divisions.
                                    Where(Function(d) Nullable.Equals(d.ParentDivisionID, parentDivision.RowID)).
                                    ToList

            'child divisions
            For Each childDivision In childDivisions

                Dim childDivisionNode = CreateNode(
                                            childDivision.RowID,
                                            childDivision.Name,
                                            ObjectType.Division,
                                            divisionImageIndex)

                'positions
                Dim positions = _positions.
                                    Where(Function(p) Nullable.Equals(p.DivisionID, childDivision.RowID)).
                                    ToList

                For Each position In positions

                    Dim positionNode = CreateNode(
                                                position.RowID,
                                                position.Name,
                                                ObjectType.Position,
                                                positionImageIndex)

                    childDivisionNode.Nodes.Add(positionNode)

                Next

                parentDivisionNode.Nodes.Add(childDivisionNode)

            Next

            parentDivisionNodes.Add(parentDivisionNode)

        Next

        Return FilterTreeView(parentDivisionNodes)

    End Function

    Private Function FilterTreeView(nodes As List(Of TreeNode)) As List(Of TreeNode)

        If String.IsNullOrWhiteSpace(SearchToolStripTextBox.Text) Then Return nodes

        For i = nodes.Count - 1 To 0 Step -1

            Dim parentDivisionNode = nodes(i)

            If parentDivisionNode.Text.ToUpper.Contains(SearchToolStripTextBox.Text.ToUpper) = False Then


                For j = parentDivisionNode.Nodes.Count - 1 To 0 Step -1

                    Dim divisionNode = parentDivisionNode.Nodes(j)


                    If divisionNode.Text.ToUpper.Contains(SearchToolStripTextBox.Text.ToUpper) = False Then

                        For k = divisionNode.Nodes.Count - 1 To 0 Step -1

                            Dim positionNode = divisionNode.Nodes(k)

                            If positionNode.Text.ToUpper.Contains(SearchToolStripTextBox.Text.ToUpper) = False Then

                                positionNode.Remove()

                            Else

                                Continue For
                            End If

                        Next

                        If divisionNode.Nodes.Count = 0 Then
                            divisionNode.Remove()
                        End If

                    Else

                        Continue For

                    End If

                Next

                If parentDivisionNode.Nodes.Count = 0 Then

                    If nodes.Count = 1 Then

                        nodes.Clear()

                    Else
                        parentDivisionNode.Remove()
                    End If


                End If

            Else

                Continue For
            End If


        Next

        If nodes.Count = 0 Then
            nodes.Clear()
        End If

        Return nodes

    End Function


    Private Function CreateNode(
                        RowId As Integer?,
                        Name As String,
                        objectType As ObjectType,
                        imageIndex As Integer) As TreeNode

        Return New TreeNode With {
                        .Text = Name,
                        .Tag = New NodeData With {
                                    .Id = RowId,
                                    .ObjectType = objectType
                        },
                        .ImageIndex = imageIndex,
                        .SelectedImageIndex = imageIndex
        }
    End Function

    Private Async Function LoadJobLevels() As Task

        Dim jobLevels = Await _jobLevelRepository.GetAllAsync()

        _jobLevels = jobLevels.OrderBy(Function(j) j.Name).ToList

    End Function

    Private Async Function LoadDivisions() As Task

        Dim divisions = Await _divisionRepository.GetAllAsync()

        _divisions = divisions.OrderBy(Function(d) d.Name).ToList

    End Function

    Private Async Function LoadPositions() As Task

        Dim positions = Await _positionRepository.GetAllAsync()

        _positions = positions.OrderBy(Function(p) p.Name).ToList

    End Function

    Private Async Function LoadPayFrequencies() As Task

        Dim payFrequencies = Await _payFrequencyRepository.GetAllAsync()

        _payFrequencies = payFrequencies.OrderBy(Function(p) p.Type).ToList

    End Function

    Private Sub GetDivisionTypes()

        _divisionTypes = _divisionRepository.GetDivisionTypeList

    End Sub

    Private Sub GetDeductionSchedules()

        _deductionSchedules = ContributionSchedule.GetList()

    End Sub

    Private Sub ShowDivisionForm(selectedDivision As Division)

        Dim isRoot = selectedDivision.IsRoot

        SetDivision(selectedDivision)

        If isRoot Then
            CreateDivisionLocationDataBindings()
        End If

        DivisionLocationGroupBox.Visible = isRoot
        DivisionUserControl1.Visible = Not isRoot

        FormsTabControl.SelectedTab = DivisionTabPage

        lblFormTitle.Text = "DIVISION"
    End Sub

    Private Sub CreateDivisionLocationDataBindings()

        DivisionLocationTextBox.DataBindings.Clear()
        DivisionLocationTextBox.DataBindings.Add("Text", Me._currentDivision, "Name")
    End Sub

    Private Async Sub ShowPositionForm(selectedPosition As Position)

        SetPosition(selectedPosition)

        EmployeeDataGrid.DataSource = Await _positionRepository.GetEmployeesAsync(selectedPosition.RowID)

        FormsTabControl.SelectedTab = PositionTabPage

        lblFormTitle.Text = "POSITION"

    End Sub

    Private Sub SetPosition(selectedPosition As Position)

        Me._currentPosition = selectedPosition.CloneJson()

        Dim allChildDivisions = _divisions.Where(Function(d) d.IsRoot = False).ToList

        PositionUserControl1.SetPosition(Me._currentPosition, allChildDivisions, _jobLevels)
    End Sub

    Private Sub SetDivision(selectedDivision As Division)

        Me._currentDivision = selectedDivision.CloneJson()

        If Me._currentDivision.IsRoot = True Then

            CreateDivisionLocationDataBindings()
            Return
        End If

        Dim allParentDivisions = _divisions.Where(Function(d) d.IsRoot = True).ToList

        DivisionUserControl1.SetDivision(
                                Me._currentDivision,
                                allParentDivisions,
                                Me._positions,
                                Me._divisionTypes,
                                Me._payFrequencies,
                                Me._deductionSchedules)
    End Sub

    Private Sub ClearForms()

        DivisionLocationTextBox.DataBindings.Clear()

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

            MessageBoxHelper.Warning("No selected position!", messageTitle)
            Return False

        ElseIf Me._currentPosition.DivisionID Is Nothing Then

            PositionUserControl1.ShowError("DivisionID", "Please select a division")
            Return False

        ElseIf String.IsNullOrWhiteSpace(Me._currentPosition.Name) Then

            PositionUserControl1.ShowError("Name", "Please enter a name")
            Return False

        End If

        Return True

    End Function

    Private Async Function SavePosition(messageTitle As String) As Task

        Await _positionRepository.SaveAsync(Me._currentPosition)

        Await RefreshTreeView()

        Dim currentTreeNode As TreeNode = GetTreeNode(Me._currentPosition.RowID, ObjectType.Position)

        DivisionPositionTreeView.SelectedNode = currentTreeNode

        ShowBalloonInfo($"Position: {Me._currentPosition.Name} was successfully updated.", messageTitle)

    End Function

    Private Async Function DeletePosition(messageTitle As String) As Task

        Dim positionName = Me._currentPosition.Name

        Await _positionRepository.DeleteAsync(Me._currentPosition.RowID)

        Await RefreshTreeView()

        ShowBalloonInfo($"Position: {positionName} was successfully deleted.", messageTitle)

    End Function

    Private Async Function DeleteDivision(messageTitle As String) As Task

        Dim divisionName = Me._currentDivision.Name

        Await _divisionRepository.DeleteAsync(Me._currentDivision.RowID)

        Await RefreshTreeView()

        ShowBalloonInfo($"Division: {divisionName} was successfully deleted.", messageTitle)

    End Function

    Private Function ValidateDivision(messageTitle As String, Optional isDivisionLocation As Boolean = False) As Boolean

        If Me._currentDivision Is Nothing Then

            MessageBoxHelper.Warning("No selected division!", messageTitle)
            Return False
        End If

        If String.IsNullOrWhiteSpace(Me._currentDivision.Name) Then

            DivisionUserControl1.ShowError("Name", "Please enter a name")
            Return False
        End If

        'if parent division, name is the only field required
        If isDivisionLocation Then Return True

        If Me._currentDivision.ParentDivisionID Is Nothing Then

            DivisionUserControl1.ShowError("ParentDivisionID", "Please select a parent division")
            Return False

        ElseIf Me._currentDivision.WorkDaysPerYear Is Nothing OrElse Me._currentDivision.WorkDaysPerYear < 0 Then

            DivisionUserControl1.ShowError("WorkDaysPerYear", "Please enter a valid Number of days work per year")
            Return False

        End If

        Return True

    End Function

    Private Async Function SaveDivision(messageTitle As String) As Task

        Dim division = Me._currentDivision.CloneJson()
        division.ParentDivision = Nothing

        Await _divisionRepository.SaveAsync(division)

        RemoveHandler DivisionPositionTreeView.AfterSelect, AddressOf DivisionPositionTreeView_AfterSelect

        Await RefreshTreeView()

        AddHandler DivisionPositionTreeView.AfterSelect, AddressOf DivisionPositionTreeView_AfterSelect

        Dim currentTreeNode As TreeNode = GetTreeNode(Me._currentDivision.RowID, ObjectType.Division)

        DivisionPositionTreeView.SelectedNode = currentTreeNode

        ShowBalloonInfo($"Division: {Me._currentDivision.Name} was successfully updated.", messageTitle)

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

    Private Sub CloseFormToolStripItem2_Click(sender As Object, e As EventArgs) _
        Handles CloseFormToolStripItem2.Click, CloseFormToolStripItem.Click

        Me.Close()

    End Sub

    Private Sub NewDivisionPositionForm_Closing(sender As Object, e As CancelEventArgs) Handles Me.Closing

        If previousForm IsNot Nothing Then
            If previousForm.Name = Me.Name Then
                previousForm = Nothing
            End If
        End If


        HRISForm.listHRISForm.Remove(Me.Name)
    End Sub

    Private Sub SearchToolStripTextBox_TextChanged(sender As Object, e As EventArgs) Handles SearchToolStripTextBox.TextChanged

        LoadTreeView()

    End Sub
End Class