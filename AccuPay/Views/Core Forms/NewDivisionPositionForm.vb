Option Strict On

Imports System.ComponentModel
Imports System.Threading.Tasks
Imports AccuPay.Data.Entities
Imports AccuPay.Data.Enums
Imports AccuPay.Data.Repositories
Imports AccuPay.Utilities.Extensions
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

    Private _listOfValueRepository As New Repository.ListOfValueRepository

    Private _employeeRepository As New EmployeeRepository

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
                              Await SaveDivision(messageTitle, isRoot)
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

        Dim changedPosition = _positions.
        FirstOrDefault(Function(p) Nullable.Equals(p.RowID, Me._currentPosition.RowID))

        If changedPosition Is Nothing Then
            MessageBoxHelper.Warning("No position selected!")

            Return
        End If

        SetPosition(changedPosition)

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

        Dim changedDivision = _divisions.
        FirstOrDefault(Function(p) Nullable.Equals(p.RowID, Me._currentDivision.RowID))

        If changedDivision Is Nothing Then
            MessageBoxHelper.Warning("No division selected!")

            Return
        End If

        SetDivision(changedDivision)
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

        Dim jobLevels = Await _jobLevelRepository.GetAllAsync(z_OrganizationID)

        _jobLevels = jobLevels.OrderBy(Function(j) j.Name).ToList

    End Function

    Private Async Function LoadDivisions() As Task

        Dim divisions = Await _divisionRepository.GetAllAsync(z_OrganizationID)

        _divisions = divisions.OrderBy(Function(d) d.Name).ToList

    End Function

    Private Async Function LoadPositions() As Task

        Dim positions = Await _positionRepository.GetAllAsync(z_OrganizationID)

        _positions = positions.OrderBy(Function(p) p.Name).ToList

    End Function

    Private Async Function LoadPayFrequencies() As Task

        Dim payFrequencies = Await _payFrequencyRepository.GetAllAsync()

        _payFrequencies = payFrequencies.
                                Where(Function(p) p.RowID.Value = PayFrequencyType.SemiMonthly OrElse
                                    p.RowID.Value = PayFrequencyType.Weekly).ToList

    End Function

    Private Sub GetDivisionTypes()

        _divisionTypes = _divisionRepository.GetDivisionTypeList

    End Sub

    Private Async Function GetDeductionSchedules() As Task

        _deductionSchedules = _listOfValueRepository.
                    ConvertToStringList(Await _listOfValueRepository.GetDeductionSchedules())

    End Function

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

        If selectedPosition.RowID Is Nothing Then Return

        SetPosition(selectedPosition)

        EmployeeDataGrid.DataSource = Await _employeeRepository.
                                                GetByPositionAsync(selectedPosition.RowID.Value)

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

        Await _positionRepository.SaveAsync(Me._currentPosition,
                                            organizationId:=z_OrganizationID,
                                            userId:=z_User)

        RecordUpdatePosition()

        Await RefreshTreeView()

        Dim currentTreeNode As TreeNode = GetTreeNode(Me._currentPosition.RowID, ObjectType.Position)

        DivisionPositionTreeView.SelectedNode = currentTreeNode

        ShowBalloonInfo($"Position: {Me._currentPosition.Name} was successfully updated.", messageTitle)

    End Function

    Private Function RecordUpdatePosition() As Boolean

        Dim oldPosition =
            Me._positions.
                FirstOrDefault(Function(l) Nullable.Equals(l.RowID, _currentPosition.RowID))

        If oldPosition Is Nothing Then Return False

        Dim changes = New List(Of Data.Entities.UserActivityItem)

        If _currentPosition.DivisionID <> oldPosition.DivisionID Then
            Dim oldDivision = Me._divisions.FirstOrDefault(Function(l) Nullable.Equals(l.RowID, oldPosition.DivisionID)).Name
            Dim newDivision = Me._divisions.FirstOrDefault(Function(l) Nullable.Equals(l.RowID, _currentPosition.DivisionID)).Name
            changes.Add(New Data.Entities.UserActivityItem() With
                        {
                        .EntityId = CInt(oldPosition.RowID),
                        .Description = $"Update position division from '{oldDivision}' to '{newDivision}'"
                        })
        End If
        If _currentPosition.Name <> oldPosition.Name Then
            changes.Add(New Data.Entities.UserActivityItem() With
                        {
                        .EntityId = CInt(oldPosition.RowID),
                        .Description = $"Update position name from '{oldPosition.Name}' to '{_currentPosition.Name}'"
                        })
        End If

        If changes.Count > 0 Then
            Dim repo = New UserActivityRepository
            repo.CreateRecord(z_User, "Position", z_OrganizationID, UserActivityRepository.RecordTypeEdit, changes)

            Return True
        End If

        Return False
    End Function

    Private Async Function DeletePosition(messageTitle As String) As Task

        Dim positionName = Me._currentPosition.Name

        If Me._currentPosition.RowID.HasValue = False Then

            MessageBoxHelper.ErrorMessage("No selected position.")
            Return
        End If

        Dim hasEmployee = (Await _employeeRepository.
                                        GetByPositionAsync(Me._currentPosition.RowID.Value))?.Any()

        If hasEmployee Is Nothing OrElse hasEmployee Then
            MessageBoxHelper.ErrorMessage("Position already has at least one assigned employee therefore cannot be deleted.")
            Return
        End If

        Await _positionRepository.DeleteAsync(Me._currentPosition.RowID.Value)

        Dim repo As New UserActivityRepository
        repo.RecordDelete(z_User, "Position", CInt(Me._currentPosition.RowID), z_OrganizationID)

        Await RefreshTreeView()

        ShowBalloonInfo($"Position: {positionName} was successfully deleted.", messageTitle)

    End Function

    Private Async Function DeleteDivision(messageTitle As String) As Task

        Dim divisionName = Me._currentDivision.Name

        Await _divisionRepository.DeleteAsync(Me._currentDivision.RowID)

        If Me._currentDivision.IsRoot Then

            Dim repo As New UserActivityRepository
            repo.RecordDelete(z_User, "Division Location", CInt(Me._currentDivision.RowID), z_OrganizationID)
        Else

            Dim repo As New UserActivityRepository
            repo.RecordDelete(z_User, "Division", CInt(Me._currentDivision.RowID), z_OrganizationID)

        End If

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

    Private Function RecordUpdateDivisionLocation() As Boolean

        Dim oldDivisionLocation =
            Me._divisions.
                FirstOrDefault(Function(l) Nullable.Equals(l.RowID, _currentDivision.RowID))

        If oldDivisionLocation Is Nothing Then Return False

        Dim changes = New List(Of Data.Entities.UserActivityItem)

        If _currentDivision.Name <> oldDivisionLocation.Name Then
            changes.Add(New Data.Entities.UserActivityItem() With
                        {
                        .EntityId = CInt(oldDivisionLocation.RowID),
                        .Description = $"Update division type from '{oldDivisionLocation.Name}' to '{_currentDivision.Name}'"
                        })
        End If

        If changes.Count > 0 Then
            Dim repo = New UserActivityRepository
            repo.CreateRecord(z_User, "Division Location", z_OrganizationID, UserActivityRepository.RecordTypeEdit, changes)

            Return True
        End If

        Return False
    End Function

    Private Function RecordUpdateDivision() As Boolean

        Dim oldDivision =
            Me._divisions.
                FirstOrDefault(Function(l) Nullable.Equals(l.RowID, _currentDivision.RowID))

        If oldDivision Is Nothing Then Return False

        Dim changes = New List(Of Data.Entities.UserActivityItem)

        If _currentDivision.DivisionType <> oldDivision.DivisionType Then
            changes.Add(New Data.Entities.UserActivityItem() With
                        {
                        .EntityId = CInt(oldDivision.RowID),
                        .Description = $"Update division type from '{oldDivision.DivisionType}' to '{_currentDivision.DivisionType}'"
                        })
        End If
        If _currentDivision.ParentDivisionID <> oldDivision.ParentDivisionID Then

            Dim oldParentDivision = Me._divisions.FirstOrDefault(Function(l) Nullable.Equals(l.RowID, oldDivision.ParentDivisionID)).Name
            Dim newParentDivision = Me._divisions.FirstOrDefault(Function(l) Nullable.Equals(l.RowID, _currentDivision.ParentDivisionID)).Name
            changes.Add(New Data.Entities.UserActivityItem() With
                        {
                        .EntityId = CInt(oldDivision.RowID),
                        .Description = $"Update division parent from '{oldParentDivision}' to '{newParentDivision}'"
                        })
        End If
        If _currentDivision.Name <> oldDivision.Name Then
            changes.Add(New Data.Entities.UserActivityItem() With
                        {
                        .EntityId = CInt(oldDivision.RowID),
                        .Description = $"Update division name from '{oldDivision.Name}' to '{_currentDivision.Name}'"
                        })
        End If
        If _currentDivision.TradeName <> oldDivision.TradeName Then
            changes.Add(New Data.Entities.UserActivityItem() With
                        {
                        .EntityId = CInt(oldDivision.RowID),
                        .Description = $"Update division trade name from '{oldDivision.TradeName}' to '{_currentDivision.TradeName}'"
                        })
        End If
        If _currentDivision.TINNo <> oldDivision.TINNo Then
            changes.Add(New Data.Entities.UserActivityItem() With
                        {
                        .EntityId = CInt(oldDivision.RowID),
                        .Description = $"Update division tin from '{oldDivision.TINNo}' to '{_currentDivision.TINNo}'"
                        })
        End If
        If _currentDivision.BusinessAddress <> oldDivision.BusinessAddress Then
            changes.Add(New Data.Entities.UserActivityItem() With
                        {
                        .EntityId = CInt(oldDivision.RowID),
                        .Description = $"Update division business address from '{oldDivision.BusinessAddress}' to '{_currentDivision.BusinessAddress}'"
                        })
        End If
        If oldDivision.DivisionHeadID Is Nothing And _currentDivision.DivisionHeadID IsNot Nothing Then
            Dim newDivisionHead = Me._positions.FirstOrDefault(Function(l) Nullable.Equals(l.RowID, _currentDivision.DivisionHeadID)).Name
            changes.Add(New Data.Entities.UserActivityItem() With
                        {
                        .EntityId = CInt(oldDivision.RowID),
                        .Description = $"Update division head from '' to '{newDivisionHead}'"
                        })

        ElseIf _currentDivision.DivisionHeadID <> oldDivision.DivisionHeadID Then
            Dim oldDivisionHead = Me._positions.FirstOrDefault(Function(l) Nullable.Equals(l.RowID, oldDivision.DivisionHeadID)).Name
            Dim newDivisionHead = Me._positions.FirstOrDefault(Function(l) Nullable.Equals(l.RowID, _currentDivision.DivisionHeadID)).Name
            changes.Add(New Data.Entities.UserActivityItem() With
                        {
                        .EntityId = CInt(oldDivision.RowID),
                        .Description = $"Update division head from '{oldDivisionHead}' to '{newDivisionHead}'"
                        })
        End If
        If oldDivision.PayFrequencyID Is Nothing And _currentDivision.PayFrequencyID IsNot Nothing Then
            Dim newPayFrequency = Me._payFrequencies.FirstOrDefault(Function(l) Nullable.Equals(l.RowID, _currentDivision.PayFrequencyID)).Type
            changes.Add(New Data.Entities.UserActivityItem() With
                        {
                        .EntityId = CInt(oldDivision.RowID),
                        .Description = $"Update division pay frequency from '' to '{newPayFrequency}'"
                        })
        ElseIf _currentDivision.PayFrequencyID <> oldDivision.PayFrequencyID Then
            Dim oldPayFrequency = Me._payFrequencies.FirstOrDefault(Function(l) Nullable.Equals(l.RowID, oldDivision.PayFrequencyID)).Type
            Dim newPayFrequency = Me._payFrequencies.FirstOrDefault(Function(l) Nullable.Equals(l.RowID, _currentDivision.PayFrequencyID)).Type
            changes.Add(New Data.Entities.UserActivityItem() With
                        {
                        .EntityId = CInt(oldDivision.RowID),
                        .Description = $"Update division pay frequency from '{oldPayFrequency}' to '{newPayFrequency}'"
                        })
        End If
        If _currentDivision.GracePeriod <> oldDivision.GracePeriod Then
            changes.Add(New Data.Entities.UserActivityItem() With
                        {
                        .EntityId = CInt(oldDivision.RowID),
                        .Description = $"Update division grace period from '{oldDivision.GracePeriod.ToString}' to '{_currentDivision.GracePeriod.ToString}'"
                        })
        End If
        If _currentDivision.WorkDaysPerYear <> oldDivision.WorkDaysPerYear Then
            changes.Add(New Data.Entities.UserActivityItem() With
                        {
                        .EntityId = CInt(oldDivision.RowID),
                        .Description = $"Update division work days per year from '{oldDivision.WorkDaysPerYear.ToString}' to '{_currentDivision.WorkDaysPerYear.ToString}'"
                        })
        End If
        If _currentDivision.AutomaticOvertimeFiling <> oldDivision.AutomaticOvertimeFiling Then
            changes.Add(New Data.Entities.UserActivityItem() With
                        {
                        .EntityId = CInt(oldDivision.RowID),
                        .Description = $"Update division automatic overtime filing from '{oldDivision.AutomaticOvertimeFiling.ToString}' to '{_currentDivision.AutomaticOvertimeFiling.ToString}'"
                        })
        End If
        If _currentDivision.MainPhone <> oldDivision.MainPhone Then
            changes.Add(New Data.Entities.UserActivityItem() With
                        {
                        .EntityId = CInt(oldDivision.RowID),
                        .Description = $"Update division main phone from '{oldDivision.MainPhone}' to '{_currentDivision.MainPhone}'"
                        })
        End If
        If _currentDivision.AltPhone <> oldDivision.AltPhone Then
            changes.Add(New Data.Entities.UserActivityItem() With
                        {
                        .EntityId = CInt(oldDivision.RowID),
                        .Description = $"Update division alternate phone from '{oldDivision.AltPhone}' to '{_currentDivision.AltPhone}'"
                        })
        End If
        If _currentDivision.EmailAddress <> oldDivision.EmailAddress Then
            changes.Add(New Data.Entities.UserActivityItem() With
                        {
                        .EntityId = CInt(oldDivision.RowID),
                        .Description = $"Update division email address from '{oldDivision.EmailAddress}' to '{_currentDivision.EmailAddress}'"
                        })
        End If
        If _currentDivision.AltEmailAddress <> oldDivision.AltEmailAddress Then
            changes.Add(New Data.Entities.UserActivityItem() With
                        {
                        .EntityId = CInt(oldDivision.RowID),
                        .Description = $"Update division alternate email address from '{oldDivision.AltEmailAddress}' to '{_currentDivision.AltEmailAddress}'"
                        })
        End If
        If _currentDivision.ContactName <> oldDivision.ContactName Then
            changes.Add(New Data.Entities.UserActivityItem() With
                        {
                        .EntityId = CInt(oldDivision.RowID),
                        .Description = $"Update division contact name from '{oldDivision.ContactName}' to '{_currentDivision.ContactName}'"
                        })
        End If
        If _currentDivision.FaxNumber <> oldDivision.FaxNumber Then
            changes.Add(New Data.Entities.UserActivityItem() With
                        {
                        .EntityId = CInt(oldDivision.RowID),
                        .Description = $"Update division fax number from '{oldDivision.FaxNumber}' to '{_currentDivision.FaxNumber}'"
                        })
        End If
        If _currentDivision.URL <> oldDivision.URL Then
            changes.Add(New Data.Entities.UserActivityItem() With
                        {
                        .EntityId = CInt(oldDivision.RowID),
                        .Description = $"Update division URL from '{oldDivision.URL}' to '{_currentDivision.URL}'"
                        })
        End If
        If _currentDivision.PhilHealthDeductionSchedule <> oldDivision.PhilHealthDeductionSchedule Then
            changes.Add(New Data.Entities.UserActivityItem() With
                        {
                        .EntityId = CInt(oldDivision.RowID),
                        .Description = $"Update division PhilHealth deduction schedule from '{oldDivision.PhilHealthDeductionSchedule}' to '{_currentDivision.PhilHealthDeductionSchedule}'"
                        })
        End If
        If _currentDivision.SssDeductionSchedule <> oldDivision.SssDeductionSchedule Then
            changes.Add(New Data.Entities.UserActivityItem() With
                        {
                        .EntityId = CInt(oldDivision.RowID),
                        .Description = $"Update division SSS deduction schedule from '{oldDivision.SssDeductionSchedule}' to '{_currentDivision.SssDeductionSchedule}'"
                        })
        End If
        If _currentDivision.PagIBIGDeductionSchedule <> oldDivision.PagIBIGDeductionSchedule Then
            changes.Add(New Data.Entities.UserActivityItem() With
                        {
                        .EntityId = CInt(oldDivision.RowID),
                        .Description = $"Update division PagIbig deduction schedule from '{oldDivision.PagIBIGDeductionSchedule}' to '{_currentDivision.PagIBIGDeductionSchedule}'"
                        })
        End If
        If _currentDivision.WithholdingTaxSchedule <> oldDivision.WithholdingTaxSchedule Then
            changes.Add(New Data.Entities.UserActivityItem() With
                        {
                        .EntityId = CInt(oldDivision.RowID),
                        .Description = $"Update division withholding tax deduction schedule from '{oldDivision.WithholdingTaxSchedule}' to '{_currentDivision.WithholdingTaxSchedule}'"
                        })
        End If
        If _currentDivision.AgencyPhilHealthDeductionSchedule <> oldDivision.AgencyPhilHealthDeductionSchedule Then
            changes.Add(New Data.Entities.UserActivityItem() With
                        {
                        .EntityId = CInt(oldDivision.RowID),
                        .Description = $"Update division PhilHealth deduction schedule w/ agency from '{oldDivision.AgencyPhilHealthDeductionSchedule}' to '{_currentDivision.AgencyPhilHealthDeductionSchedule}'"
                        })
        End If
        If _currentDivision.AgencySssDeductionSchedule <> oldDivision.AgencySssDeductionSchedule Then
            changes.Add(New Data.Entities.UserActivityItem() With
                        {
                        .EntityId = CInt(oldDivision.RowID),
                        .Description = $"Update division SSS deduction schedule w/ agency from '{oldDivision.AgencySssDeductionSchedule}' to '{_currentDivision.AgencySssDeductionSchedule}'"
                        })
        End If
        If _currentDivision.AgencyPagIBIGDeductionSchedule <> oldDivision.AgencyPagIBIGDeductionSchedule Then
            changes.Add(New Data.Entities.UserActivityItem() With
                        {
                        .EntityId = CInt(oldDivision.RowID),
                        .Description = $"Update division PagIbig deduction schedule w/ agency from '{oldDivision.AgencyPagIBIGDeductionSchedule}' to '{_currentDivision.AgencyPagIBIGDeductionSchedule}'"
                        })
        End If
        If _currentDivision.AgencyWithholdingTaxSchedule <> oldDivision.AgencyWithholdingTaxSchedule Then
            changes.Add(New Data.Entities.UserActivityItem() With
                        {
                        .EntityId = CInt(oldDivision.RowID),
                        .Description = $"Update division withholding tax deduction schedule w/ agency from '{oldDivision.AgencyWithholdingTaxSchedule}' to '{_currentDivision.AgencyWithholdingTaxSchedule}'"
                        })
        End If
        If _currentDivision.DefaultVacationLeave <> oldDivision.DefaultVacationLeave Then
            changes.Add(New Data.Entities.UserActivityItem() With
                        {
                        .EntityId = CInt(oldDivision.RowID),
                        .Description = $"Update division vacation leave from '{oldDivision.DefaultVacationLeave.ToString}' to '{_currentDivision.DefaultVacationLeave.ToString}'"
                        })
        End If
        If _currentDivision.DefaultSickLeave <> oldDivision.DefaultSickLeave Then
            changes.Add(New Data.Entities.UserActivityItem() With
                        {
                        .EntityId = CInt(oldDivision.RowID),
                        .Description = $"Update division sick leave from '{oldDivision.DefaultSickLeave.ToString}' to '{_currentDivision.DefaultSickLeave.ToString}'"
                        })
        End If
        If _currentDivision.DefaultOtherLeave <> oldDivision.DefaultOtherLeave Then
            changes.Add(New Data.Entities.UserActivityItem() With
                        {
                        .EntityId = CInt(oldDivision.RowID),
                        .Description = $"Update division other leave from '{oldDivision.DefaultOtherLeave.ToString}' to '{_currentDivision.DefaultOtherLeave.ToString}'"
                        })
        End If

        If changes.Count > 0 Then
            Dim repo = New UserActivityRepository
            repo.CreateRecord(z_User, "Division", z_OrganizationID, UserActivityRepository.RecordTypeEdit, changes)

            Return True
        End If

        Return False
    End Function

    Private Async Function SaveDivision(messageTitle As String, isRoot As Boolean) As Task

        Dim division = Me._currentDivision.CloneJson()
        division.ParentDivision = Nothing

        Await _divisionRepository.SaveAsync(division, organizationId:=z_OrganizationID, userId:=z_User)

        If isRoot Then
            RecordUpdateDivisionLocation()
        Else
            RecordUpdateDivision()
        End If

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

    Private Sub UserActivityToolStripButton_Click(sender As Object, e As EventArgs) Handles UserActivityToolStripButton.Click

        If Me._currentDivision.IsRoot Then
            Dim userActivity As New UserActivityForm("Division Location")
            userActivity.ShowDialog()
        Else
            Dim userActivity As New UserActivityForm("Division")
            userActivity.ShowDialog()
        End If

    End Sub

    Private Sub UserActivityPositionToolStripButton_Click(sender As Object, e As EventArgs) Handles UserActivityPositionToolStripButton.Click
        Dim userActivity As New UserActivityForm("Position")
        userActivity.ShowDialog()
    End Sub

End Class