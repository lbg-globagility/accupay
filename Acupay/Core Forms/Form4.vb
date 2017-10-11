Imports MySql.Data.MySqlClient

Public Class Form4
    Public q_employee As String = "SELECT e.RowID," & _
        "COALESCE(CAST(e.EmployeeID AS CHAR),'') 'Employee ID'," & _
        "COALESCE(e.FirstName,'') 'First Name'," & _
        "COALESCE(e.MiddleName,'') 'Middle Name'," & _
        "COALESCE(e.LastName,'') 'Last Name'," & _
        "COALESCE(e.Surname,'') 'Surname'," & _
        "COALESCE(e.Nickname,'') 'Nickname'," & _
        "COALESCE(e.MaritalStatus,'') 'Marital Status'," & _
        "COALESCE(e.NoOfDependents,0) 'No Of Dependents'," & _
        "COALESCE(DATE_FORMAT(e.Birthdate,'%m-%d-%Y'),'') 'Birthdate'," & _
        "COALESCE(e.JobTitle,'') 'Job Title'," & _
        "COALESCE(pos.PositionName,'') 'Position Name'," & _
        "COALESCE(e.Salutation,'') 'Salutation'," & _
        "COALESCE(e.TINNo,'') 'TIN'," & _
        "COALESCE(e.SSSNo,'') 'SSS No'," & _
        "COALESCE(e.HDMFNo,'') 'PAGIBIG No'," & _
        "COALESCE(e.PhilHealthNo,'') 'PhilHealth No'," & _
        "COALESCE(e.WorkPhone,'') 'Work Phone'," & _
        "COALESCE(e.HomePhone,'') 'Home Phone'," & _
        "COALESCE(e.MobilePhone,'') 'Mobile Phone'," & _
        "COALESCE(e.HomeAddress,'') 'Home Address'," & _
        "COALESCE(e.EmailAddress,'') 'Email Address'," & _
        "COALESCE(IF(e.Gender='M','Male','Female'),'') 'Gender'," & _
        "COALESCE(e.EmploymentStatus,'') 'Employment Status'," & _
        "COALESCE(pf.PayFrequencyType,'') 'Pay Frequency'," & _
        "COALESCE(e.UndertimeOverride,'') 'UndertimeOverride'," & _
        "COALESCE(e.OvertimeOverride,'') 'OvertimeOverride'," & _
        "DATE_FORMAT(e.Created,'%m-%d-%Y') 'Creation Date'," & _
        "CONCAT(CONCAT(UCASE(LEFT(u.FirstName, 1)), SUBSTRING(u.FirstName, 2)),' ',CONCAT(UCASE(LEFT(u.LastName, 1)), SUBSTRING(u.LastName, 2))) 'Created by'," & _
        "COALESCE(DATE_FORMAT(e.LastUpd,'%m-%d-%Y'),'') 'Last Update'," & _
        "(SELECT CONCAT(CONCAT(UCASE(LEFT(u.FirstName, 1)), SUBSTRING(u.FirstName, 2)),' ',CONCAT(UCASE(LEFT(u.LastName, 1)), SUBSTRING(u.LastName, 2)))  FROM user WHERE RowID=e.LastUpdBy) 'LastUpdate by'" & _
        ",COALESCE(pos.RowID,'') 'pos_RowID'" & _
        ",COALESCE(e.PayFrequencyID,'') 'PayFrequencyID'" & _
        ",COALESCE(e.LeavePerPayPeriod,'') 'LeavePerPayPeriod'" & _
        ",COALESCE(e.EmployeeType,'') 'EmployeeType'" & _
        " " & _
        "FROM employee e " & _
        "INNER JOIN user u ON e.CreatedBy=u.RowID " & _
        "LEFT JOIN position pos ON e.PositionID=pos.RowID " & _
        "LEFT JOIN payfrequency pf ON e.PayFrequencyID=pf.RowID " & _
        "WHERE e.OrganizationID=" & orgztnID

    Dim allproduct As New DataTable

    Private Sub Form4_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        'dbconn()

        loadDivis()
        'loadEmployees()

        Dim custompostbl As New MySqlDataAdapter("SELECT * FROM position WHERE OrganizationID=" & orgztnID, _
                                             conn) 'ParentDivisionID

        'Dim customdivtblParent As New MySqlDataAdapter("SELECT * FROM division WHERE ParentDivisionID IS NULL AND OrganizationID=" & orgztnID, _
        '                                     conn)

        Dim customdivtbl As New MySqlDataAdapter("SELECT * FROM division WHERE OrganizationID=" & orgztnID, _
                                             conn)

        Dim customemptbl As New MySqlDataAdapter("SELECT * FROM employee WHERE OrganizationID=" & orgztnID, _
                                             conn)

        customTbl = fillDattbl("SELECT pos.RowID 'pos_RowID',pos.PositionName,pos.ParentPositionID,pos.DivisionId,divis.RowID 'divis_RowID' ,divis.Name ,COALESCE(CONCAT(emp.FirstName,' ',IF(emp.MiddleName IS NULL,'',CONCAT(emp.MiddleName,' ')),emp.LastName,' ',IF(emp.Surname IS NULL,'',emp.Surname)),'Open') 'Emp Full name' FROM position pos LEFT JOIN division divis ON divis.RowID=pos.DivisionId LEFT JOIN employee emp ON emp.PositionID=pos.RowID WHERE pos.OrganizationID=" & orgztnID & ";")

        custompostbl.Fill(customDatset, "custompostbl")

        'custompostbl.Fill(customDatset, "customdivtblParent")

        customdivtbl.Fill(customDatset, "customdivtbl")

        customemptbl.Fill(customDatset, "customemptbl")

        customDatset.Relations.Add("Div", customDatset.Tables("customdivtbl").Columns("RowID"), customDatset.Tables("customdivtbl").Columns("ParentDivisionID"))

        customDatset.Relations.Add("PosToDiv", customDatset.Tables("customdivtbl").Columns("RowID"), customDatset.Tables("custompostbl").Columns("DivisionID"))

        customDatset.Relations.Add("Pos", customDatset.Tables("custompostbl").Columns("RowID"), customDatset.Tables("custompostbl").Columns("ParentPositionID"))

        customDatset.Relations.Add("PosToEmp", customDatset.Tables("custompostbl").Columns("RowID"), customDatset.Tables("customemptbl").Columns("PositionID"))

        TextBox2.Focus()

        allproduct = retAsDatTbl("SELECT PartNo FROM product;")

    End Sub

    Sub loadEmployees()
        Try

            filltable(DataGridViewX1, q_employee)

        Catch ex As Exception
            MsgBox(getErrExcptn(ex, Me.Name), , "Unexpected Message")
        End Try

    End Sub
    Private Sub DataGridViewX1_Scroll(sender As Object, e As ScrollEventArgs) Handles DataGridViewX1.Scroll
        Label1.Text = e.NewValue

        Label1.Text = e.OldValue

    End Sub

    Dim hasFinEdit As SByte
    Dim colmnName As String

    Private Sub DataGridViewX2_CellBeginEdit(sender As Object, e As DataGridViewCellCancelEventArgs) Handles DataGridViewX2.CellBeginEdit
        hasFinEdit = 0
        colmnName = DataGridViewX2.Columns(e.ColumnIndex).Name
    End Sub

    Private Sub DataGridViewX2_CellEndEdit(sender As Object, e As DataGridViewCellEventArgs) 'Handles DataGridViewX2.CellEndEdit
        'RemoveHandler DataGridViewX2.CellValidating, AddressOf DataGridViewX2_CellValidating
        hasFinEdit = 1

        If DataGridViewX2.CurrentRow.IsNewRow = False Then
            If colmnName = "Column1" Then

            ElseIf colmnName = "Column2" Then
                'CheckDate(e, DataGridViewX2)

            End If
        End If

        'AddHandler DataGridViewX2.CellValidating, AddressOf DataGridViewX2_CellValidating
    End Sub




    Private Sub DataGridViewX2_CellContentClick(sender As Object, e As DataGridViewCellEventArgs) Handles DataGridViewX2.CellContentClick

    End Sub

    Private Sub DataGridViewX2_CellValidating(sender As Object, e As DataGridViewCellValidatingEventArgs) Handles DataGridViewX2.CellValidating
        'Dim column As DataGridViewColumn = _
        'DataGridViewX2.Columns(e.ColumnIndex)

        'Dim e_edit As DataGridViewCellValidatingEventArgs

        Dim colName As String = DataGridViewX2.Columns(e.ColumnIndex).Name
        Dim rowIndx As Integer ' colIndx,
        'colIndx = e.ColumnIndex
        rowIndx = e.RowIndex

        If DataGridViewX2.CurrentRow.IsNewRow = False Then

            Try
                If colName = "Column1" Then
                    Dim x_int = CInt(e.FormattedValue)
                ElseIf colName = "Column2" Then
                    Format(CDate(e.FormattedValue), "MM-dd-yyyy")
                End If
                DataGridViewX2.Item(colName, rowIndx).ErrorText = Nothing
            Catch ex As Exception
                If ex.Message.Contains("Date") Then
                    DataGridViewX2.Item(colName, rowIndx).ErrorText = "     Invalid date value"
                ElseIf ex.Message.Contains("Integer") Then
                    DataGridViewX2.Item(colName, rowIndx).ErrorText = "     Invalid numeric value"
                End If
            End Try
        End If

    End Sub

    Private Sub DataGridViewX2_RowLeave(sender As Object, e As DataGridViewCellEventArgs) Handles DataGridViewX2.RowLeave
        DataGridViewX2.Rows(e.RowIndex).Height = 22
    End Sub

    Private Sub DataGridViewX2_CellEnter(sender As Object, e As DataGridViewCellEventArgs) Handles DataGridViewX2.CellEnter
        DataGridViewX2.CurrentRow.Height = 35
    End Sub

    Private Shared Sub CheckTrack(ByVal newValue As DataGridViewCellValidatingEventArgs, _
                                  Optional dgvobj As Object = Nothing)

        If String.IsNullOrEmpty(newValue.FormattedValue.ToString()) Then
            NotifyUserAndForceRedo("Please enter a track", newValue)
        ElseIf Not Integer.TryParse( _
            newValue.FormattedValue.ToString(), New Integer()) Then
            NotifyUserAndForceRedo("A Track must be a number", newValue)
        ElseIf Integer.Parse(newValue.FormattedValue.ToString()) < 1 Then
            NotifyUserAndForceRedo("Not a valid track", newValue)
        End If

    End Sub

    Private Shared Sub NotifyUserAndForceRedo(ByVal errorMessage As String, _
                                              ByVal newValue As DataGridViewCellValidatingEventArgs, _
                                              Optional dgvobj As Object = Nothing)
        'MessageBox.Show(errorMessage)

        'Dim cell As DataGridViewCell = dgvobj.rows(newValue.RowIndex).cells(newValue.ColumnIndex)

        ''dgvobj.Item(newValue.ColumnIndex, newValue.RowIndex)

        'cell.ErrorText = New String(errorMessage)

        dgvobj.Rows(newValue.RowIndex).Cells(newValue.ColumnIndex).ErrorText = errorMessage

        'newValue.Cancel = True
    End Sub
    'DataGridViewCellValidatingEventArgs
    'DataGridViewCellEventArgs
    Private Sub CheckDate(ByVal newValue As DataGridViewCellValidatingEventArgs, _
                          Optional dgvobj As Object = Nothing)

        Try
            'MsgBox(Format(CDate(dgvobj.Item(newValue.ColumnIndex, newValue.RowIndex).Value), "MM-dd-yyyy"))
            'DataGridViewX2.Item(newValue.ColumnIndex, newValue.RowIndex).Value = _
            '                                    Format(CDate(DataGridViewX2.Item(newValue.ColumnIndex, newValue.RowIndex).Value), "MM-dd-yyyy")

            'dgvobj.Item(newValue.ColumnIndex, newValue.RowIndex).Value = _
            Format(Date.Parse(newValue.FormattedValue.ToString), "MM-dd-yyyy")

            'Date.Parse(newValue.FormattedValue.ToString()).ToShortDateString()

            AnnotateCell_ERR(String.Empty, newValue, dgvobj)

        Catch ex As FormatException
            'MsgBox(ex.Message)
            AnnotateCell_ERR(ex.Message, newValue, dgvobj)

        End Try
    End Sub
    'DataGridViewCellValidatingEventArgs
    Sub AnnotateCell_ERR(ByVal errorMessage As String, _
                         ByVal dgvCellValidatingEArgs As DataGridViewCellValidatingEventArgs, _
                         Optional dgvobj As Object = Nothing)

        Dim cell As DataGridViewCell

        cell = dgvobj.Item(dgvCellValidatingEArgs.ColumnIndex, dgvCellValidatingEArgs.RowIndex)

        cell.ErrorText = New String(errorMessage)

    End Sub

    Dim customDatset As New DataSet

    'Private Sub Button1_Click(sender As Object, e As EventArgs) 'Handles Button1.Click

    '    Dim custompostbl As New MySqlDataAdapter("SELECT * FROM position WHERE OrganizationID=" & orgztnID, _
    '                                         conn) 'ParentDivisionID

    '    'Dim customdivtblParent As New MySqlDataAdapter("SELECT * FROM division WHERE ParentDivisionID IS NULL AND OrganizationID=" & orgztnID, _
    '    '                                     conn)

    '    Dim customdivtbl As New MySqlDataAdapter("SELECT * FROM division WHERE OrganizationID=" & orgztnID, _
    '                                         conn)

    '    Dim customemptbl As New MySqlDataAdapter("SELECT * FROM employee WHERE OrganizationID=" & orgztnID, _
    '                                         conn)

    '    customTbl = fillDattbl("SELECT pos.RowID 'pos_RowID',pos.PositionName,pos.ParentPositionID,pos.DivisionId,divis.RowID 'divis_RowID' ,divis.Name ,COALESCE(CONCAT(emp.FirstName,' ',IF(emp.MiddleName IS NULL,'',CONCAT(emp.MiddleName,' ')),emp.LastName,' ',IF(emp.Surname IS NULL,'',emp.Surname)),'Open') 'Emp Full name' FROM position pos LEFT JOIN division divis ON divis.RowID=pos.DivisionId LEFT JOIN employee emp ON emp.PositionID=pos.RowID WHERE pos.OrganizationID=" & orgztnID & ";")

    '    custompostbl.Fill(customDatset, "custompostbl")

    '    'custompostbl.Fill(customDatset, "customdivtblParent")

    '    customdivtbl.Fill(customDatset, "customdivtbl")

    '    customemptbl.Fill(customDatset, "customemptbl")

    '    customDatset.Relations.Add("Div", customDatset.Tables("customdivtbl").Columns("RowID"), customDatset.Tables("customdivtbl").Columns("ParentDivisionID"))

    '    customDatset.Relations.Add("PosToDiv", customDatset.Tables("customdivtbl").Columns("RowID"), customDatset.Tables("custompostbl").Columns("DivisionID"))

    '    customDatset.Relations.Add("Pos", customDatset.Tables("custompostbl").Columns("RowID"), customDatset.Tables("custompostbl").Columns("ParentPositionID"))

    '    customDatset.Relations.Add("PosToEmp", customDatset.Tables("custompostbl").Columns("RowID"), customDatset.Tables("customemptbl").Columns("PositionID"))

    '    tvPosit.Nodes.Clear()

    '    Dim i, n As Integer
    '    i = 0 : n = 0
    '    Dim parentrow As DataRow

    '    Dim ParentTable As DataTable

    '    Dim ChildTable As DataTable

    '    Dim Y As DataRow

    '    Dim ChildTableRows As DataTable

    '    Dim divtable As DataTable

    '    divtable = customDatset.Tables("customdivtbl")

    '    For Each divrow In divtable.Rows

    '        Dim divnode As TreeNode

    '        divnode = New TreeNode(divrow.Item(1))

    '        If IsDBNull(divrow.Item("ParentDivisionID")) Then
    '            tvPosit.Nodes.Add(divnode)
    '        End If

    '        Dim divchildrow As DataRow

    '        Dim divchildnode As TreeNode

    '        divchildnode = New TreeNode

    '        For Each divchildrow In divrow.GetChildRows("Div")
    '            divchildnode = divnode.Nodes.Add(divchildrow(1)) ' & "(" & childrow(0) & ")")


    '        Next

    '    Next

    '    'ParentTable = customDatset.Tables("custompostbl")

    '    ''relation2.tables? "parentToRelation2"

    '    'For Each parentrow In ParentTable.Rows

    '    '    Dim parentnode As TreeNode

    '    '    parentnode = New TreeNode(parentrow.Item(1))

    '    '    tvPosit.Nodes.Add(parentnode)

    '    '    ''''populate child'''''

    '    '    '''''''''''''''''''''''

    '    '    Dim childrow As DataRow

    '    '    Dim childnode As TreeNode

    '    '    childnode = New TreeNode

    '    '    ' This is looking at the parent table

    '    '    For Each childrow In parentrow.GetChildRows("Pos")
    '    '        childnode = parentnode.Nodes.Add(childrow(1)) ' & "(" & childrow(0) & ")")
    '    '        'childnode.Tag = childrow("RowID")

    '    '        ''''populate child2''''

    '    '        ''''''''''''''''''''''''''
    '    '        Dim childrow2 As DataRow

    '    '        Dim childnode2 As TreeNode

    '    '        childnode2 = New TreeNode

    '    '        'For Each childrow2 In childrow.GetChildRows("PosToEmp")
    '    '        '    childnode2 = childnode.Nodes.Add(childrow2("RowID"))
    '    '        'Next childrow2

    '    '    Next childrow

    '    'Next parentrow
    'End Sub

    'Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click

    '    tvPosit.Nodes.Clear()

    '    Dim i, n As Integer

    '    Dim parentrow As DataRow

    '    Dim ParentTable As DataTable

    '    Dim ChildTable As DataTable

    '    Dim Y As DataRow

    '    Dim ChildTableRows As DataTable

    '    Dim postable As DataTable

    '    postable = customDatset.Tables("custompostbl")

    '    Dim divrow As DataRow

    '    For Each divrow In postable.Rows

    '        Dim posnode As TreeNode

    '        posnode = New TreeNode 'New TreeNode(divrow.Item(1))

    '        If IsDBNull(divrow.Item("ParentPositionID")) Then
    '            posnode = tv1.Nodes.Add("p" & divrow.Item("RowID"), divrow.Item("PositionName"))
    '        End If

    '        Dim poschildrow As DataRow

    '        Dim poschildnode As TreeNode

    '        For Each poschildrow In divrow.GetChildRows("Pos")

    '            poschildnode = New TreeNode

    '            poschildnode = posnode.Nodes.Add(poschildrow("PositionName")) ' & "(" & childrow(0) & ")")

    '            'nodeAddr(divrow, _
    '            '         poschildnode, _
    '            '         "Pos", _
    '            '         "PositionName")

    '            Dim poschildnode1 As TreeNode
    '            poschildnode1 = New TreeNode

    '            For Each poschildrow1 In poschildrow.GetChildRows("Pos")
    '                poschildnode1 = poschildnode.Nodes.Add(poschildrow1("PositionName")) ' & "(" & childrow(0) & ")")

    '            Next

    '        Next

    '    Next
    'End Sub

    Sub nodeAddr(ByVal tblRow As DataRow, _
                 ByVal tvnod As TreeNode, _
                 ByVal childsrc As String, _
                 ByVal drowCol As String)

        Dim childrow As DataRow

        Dim childnod As TreeNode

        For Each childrow In tblRow.GetChildRows(childsrc)

            childnod = New TreeNode

            childnod = tvnod.Nodes.Add(childrow(drowCol)) ' & "(" & childrow(0) & ")")

            nodeAddr(tblRow, _
                     childnod, _
                     childsrc, _
                     drowCol)
        Next

    End Sub

    Dim customTbl As New DataTable

    Private Sub treeVAddr(Optional treeviewobj As Object = Nothing)

        Dim posnodaddtn As New List(Of String)

        Try
            For Each tnod As TreeNode In treeviewobj.Nodes

                Dim divisRowID = tnod.Name

                Dim listnod As New List(Of String)
                listnod = GetChildren(tnod)

                If listnod.Count <> 0 Then
                    For Each strlist In listnod

                        Dim posID As String = getStrBetween(strlist, "", "@")

                        Dim nodlvl As String = StrReverse(getStrBetween(StrReverse(strlist), "", "@"))

                        Dim rowselect() As DataRow = customTbl.Select("ParentPositionID=" & posID)

                        For i = 0 To rowselect.Count - 1
                            tnod.Nodes.Add("p" & rowselect(i).Item("pos_RowID").ToString, _
                                           rowselect(i).Item("PositionName").ToString)

                            treeVAddr(tvPosit)
                        Next

                    Next
                Else
                    Dim rowselect() As DataRow = customTbl.Select("DivisionID=" & divisRowID)

                    For i = 0 To rowselect.Count - 1
                        tnod.Nodes.Add("p" & rowselect(i).Item("pos_RowID").ToString, _
                                       rowselect(i).Item("PositionName").ToString)

                        treeVAddr(tvPosit)
                    Next
                End If

            Next

        Catch ex As Exception
            MsgBox(getErrExcptn(ex, Me.Name), , "Unexpected Message")
            'Finally
            '    For Each strng In posnodaddtn

            '    Next
        End Try

    End Sub

    Function GetChildren(parentNode As TreeNode) As List(Of String)
        Dim nodes As List(Of String) = New List(Of String)
        GetAllChildren(parentNode, nodes)
        Return nodes
    End Function

    Sub GetAllChildren(parentNode As TreeNode, nodes As List(Of String))
        For Each childNode As TreeNode In parentNode.Nodes
            If childNode.Name.Contains("p") Then
                Dim posID As String = StrReverse(getStrBetween(StrReverse(childNode.Name), "", "p"))

                nodes.Add(posID & "@" & childNode.Level)
                GetAllChildren(childNode, nodes)
            End If
        Next
    End Sub
    Private Sub TabControl1_DrawItem(sender As Object, e As DrawItemEventArgs) Handles TabControl1.DrawItem

        Dim g As Graphics = e.Graphics
        Dim tp As TabPage = TabControl1.TabPages(e.Index)
        Dim br As Brush
        Dim sf As New StringFormat

        Dim r As New RectangleF(e.Bounds.X, e.Bounds.Y + 7, e.Bounds.Width, e.Bounds.Height - 7) '

        Dim TabTextBrush As Brush = New SolidBrush(Color.Black) 'FromArgb(142, 33, 11)

        Dim TabBackBrush As Brush = New SolidBrush(Color.FromArgb(255, 242, 157))

        sf.Alignment = StringAlignment.Center

        Dim strTitle As String = tp.Text

        If TabControl1.SelectedIndex = e.Index Then

            br = TabBackBrush

            g.FillRectangle(br, e.Bounds)

            br = TabTextBrush

            Dim ff As Font
            ff = New Font(TabControl1.Font, FontStyle.Bold)
            g.DrawString(strTitle, ff, br, r, sf)

        Else

            br = New SolidBrush(Color.FromArgb(54, 78, 111))
            g.FillRectangle(br, e.Bounds)
            br = New SolidBrush(Color.White)
            Dim ff As Font
            ff = New Font(TabControl1.Font, FontStyle.Regular)
            g.DrawString(strTitle, TabControl1.Font, br, r, sf)

        End If

    End Sub

#Region "searched tabcontrol coloring doesn't work"

    '    Try
    ''This line of code will help you to 
    ''change the apperance like size,name,style.
    'Dim f As Font
    ''For background color
    'Dim backBrush As Brush
    ''For forground color
    'Dim foreBrush As Brush
    ''This construct will hell you to decide 
    ''which tab page have current focus
    ''to change the style.
    '        If e.Index = Me.TabControl1.SelectedIndex Then
    ''This line of code will help you to 
    ''change the apperance like size,name,style.
    '            f = New Font(e.Font, FontStyle.Bold Or FontStyle.Bold)
    '            f = New Font(e.Font, FontStyle.Bold)
    '            backBrush = New System.Drawing.SolidBrush(Color.DarkGray)
    '            foreBrush = Brushes.White
    '        Else
    '            f = e.Font
    '            backBrush = New SolidBrush(e.BackColor)
    '            foreBrush = New SolidBrush(e.ForeColor)
    '        End If
    ''To set the alignment of the caption.
    'Dim tabName As String = Me.TabControl1.TabPages(e.Index).Text
    'Dim sf As New StringFormat()
    ''Thsi will help you to fill the interior portion of
    ''selected tabpage.
    ''Continue.........
    '        sf.Alignment = StringAlignment.Center
    '    Catch ex As Exception
    '        MsgBox(ex.Message)
    '    End Try

    ''=======================================================
    ''Service provided by Telerik (www.telerik.com)
    ''Conversion powered by NRefactory.
    ''Twitter: @telerik
    ''Facebook: facebook.com/telerik
    ''=======================================================

#End Region

    Private Sub Button3_Click(sender As Object, e As EventArgs) Handles Button3.Click
        CreateTree()

    End Sub
    '=========================================================================================
    Dim dattabDivis As New DataTable
    Private Sub loadDivis()
        Try
            dattabDivis.Columns.Clear()
            dattabDivis.Rows.Clear()
            If conn.State = ConnectionState.Open Then conn.Close()
            Dim CatID As Integer = 0
            'If AccountingTab.SelectedIndex = 0 Then
            '    CatID = accountname.Item(0)
            'ElseIf AccountingTab.SelectedIndex = 1 Then
            '    CatID = accountname.Item(1)
            'ElseIf AccountingTab.SelectedIndex = 2 Then
            '    CatID = accountname.Item(2)
            'ElseIf AccountingTab.SelectedIndex = 3 Then
            '    CatID = accountname.Item(3)
            'ElseIf AccountingTab.SelectedIndex = 4 Then
            '    CatID = accountname.Item(4)
            'End If
            Dim dt As New DataTable
            'position
            dt = retAsDatTbl("SELECT * FROM division WHERE OrganizationID=" & orgztnID & ";") 'categoryId='" & CatID & "' and 
            dattabDivis.Columns.Add("RowID", GetType(Integer))
            'dattabDivis.Columns.Add("PositionName", GetType(String))
            dattabDivis.Columns.Add("Name", GetType(String)) 'PositionName
            'dattabDivis.Columns.Add("ParentPositionID", GetType(Integer))
            dattabDivis.Columns.Add("ParentDivisionID", GetType(Integer)) 'ParentPositionID
            dattabDivis.Columns.Add("LEVEL", GetType(Integer))
            For Each dr As DataRow In dt.Rows
                'dattabDivis.Rows.Add(dr("RowID"), dr("PositionName"), dr("ParentPositionID"))
                dattabDivis.Rows.Add(dr("RowID"), dr("Name"), dr("ParentDivisionID"))
            Next
            Dim i As Integer
            For i = 0 To dattabDivis.Rows.Count - 1
                Dim ID1 As String = dattabDivis.Rows(i).Item("RowID").ToString
                dattabDivis.Rows(i).Item("LEVEL") = FindLevel(ID1, 0)
            Next
        Catch ex As Exception
            MsgBox(getErrExcptn(ex, Me.Name), , "Unexpected Message")
        End Try
    End Sub
    Private Function FindLevel(ByVal ID As String, ByRef Level As Integer) As Integer
        Dim i As Integer
        For i = 0 To dattabDivis.Rows.Count - 1
            Dim ID1 As String = dattabDivis.Rows(i).Item("RowID").ToString
            'Dim Parent1 As String = If(IsDBNull(dattabDivis.Rows(i).Item("ParentPositionID")), _
            '                           "", _
            '                           dattabDivis.Rows(i).Item("ParentPositionID").ToString)
            Dim Parent1 As String = If(IsDBNull(dattabDivis.Rows(i).Item("ParentDivisionID")), _
                                       "", _
                                       dattabDivis.Rows(i).Item("ParentDivisionID").ToString)
            If ID = ID1 Then
                If Parent1 = "" Then
                    Return Level
                Else
                    Level += 1
                    FindLevel(Parent1, Level)
                End If
            End If
        Next
        Return Level
    End Function

    Private Sub CreateTree()
        Dim MaxLevel1 As Integer = CInt(dattabDivis.Compute("MAX(LEVEL)", ""))
        Dim i, j As Integer
        For i = 0 To MaxLevel1
            Dim Rows1() As DataRow = dattabDivis.Select("LEVEL = " & i)

            For j = 0 To Rows1.Count - 1
                Dim ID1 As String = Rows1(j).Item("RowID").ToString
                'Dim Name1 As String = Rows1(j).Item("PositionName").ToString
                'Dim Parent1 As String = Rows1(j).Item("ParentPositionID").ToString
                Dim Name1 As String = Rows1(j).Item("Name").ToString
                Dim Parent1 As String = Rows1(j).Item("ParentDivisionID").ToString
                If Parent1 = "" Then
                    tvPosit.Nodes.Add(ID1, Name1)
                Else
                    Dim TreeNodes1() As TreeNode = tvPosit.Nodes.Find(Parent1, True)

                    If TreeNodes1.Length > 0 Then
                        TreeNodes1(0).Nodes.Add(ID1, Name1)
                    End If
                End If
            Next
        Next
    End Sub

    Private Sub TextBox2_KeyPress(sender As Object, e As KeyPressEventArgs) Handles TextBox2.KeyPress
        Dim e_asc As String = Asc(e.KeyChar)

        'EnterKeyAsTabKey(e_asc, TextBox1)

    End Sub

    Private Sub TextBox2_TextChanged(sender As Object, e As EventArgs) Handles TextBox2.TextChanged

    End Sub

    Private Sub DataGridViewX2_SelectionChanged(sender As Object, e As EventArgs) Handles DataGridViewX2.SelectionChanged
        myEllipseButton(DataGridViewX2, "Column1", btnotherbranch)

    End Sub

    Private Sub btnotherbranch_VisibleChanged(sender As Object, e As EventArgs) Handles btnotherbranch.VisibleChanged
        'ComboBox1.Visible = 0
        txtcombo.Visible = 0
        ListBox1.Visible = 0

    End Sub

    Private Sub ComboBox1_KeyPress(sender As Object, e As KeyPressEventArgs) Handles ComboBox1.KeyPress

    End Sub

    Private Sub ComboBox1_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ComboBox1.SelectedIndexChanged

    End Sub

    Private Sub btnotherbranch_Click(sender As Object, e As EventArgs) Handles btnotherbranch.Click
        Dim newpoint As Point = btnotherbranch.Location

        txtcombo.Parent = DataGridViewX2
        ListBox1.Parent = DataGridViewX2

        newpoint.Y = newpoint.Y + btnotherbranch.Height
        'newpoint.X = newpoint.X + btnotherbranch.Width

        txtcombo.Visible = 1
        txtcombo.Location = newpoint
        txtcombo.BringToFront()

        txtcombo.PerformLayout()

        newpoint.Y = newpoint.Y + txtcombo.Height

        ListBox1.Visible = 1
        ListBox1.Location = newpoint
        ListBox1.BringToFront()

        ListBox1.PerformLayout()

        'txtcombo.Parent = Me

    End Sub

    Private Sub txtcombo_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txtcombo.KeyPress
        Dim e_asc As String = Asc(e.KeyChar)

        If e_asc = 13 Then
            If ListBox1.Items.Count <> 0 Then
                If DataGridViewX2.RowCount <> 0 Then
                    DataGridViewX2.CurrentRow.Cells("Column1").Value = ListBox1.SelectedItem.ToString

                End If

            End If
        End If

    End Sub

    Private Sub txtcombo_TextChanged(sender As Object, e As EventArgs) Handles txtcombo.TextChanged
        Static currentval As String = Nothing

        If currentval <> Trim(txtcombo.Text) Then
            currentval = Trim(txtcombo.Text)

            DataGridViewX2.EndEdit(1)

            If Trim(txtcombo.Text) = "" Then

                ListBox1.Items.Clear()

            Else

                Dim selproduct = allproduct.Select("PartNo LIKE '%" & Trim(txtcombo.Text) & "%'", "PartNo ASC")

                ListBox1.Items.Clear()

                For Each drow In selproduct
                    ListBox1.Items.Add(drow("PartNo").ToString)
                Next

                If txtcombo.Text.Length <> 0 Then
                    'txtcombo.Text.Insert(txtcombo.Text.Length - 1, "")

                    txtcombo.Focus()

                    txtcombo.Select(txtcombo.Text.Length, 0)

                End If

            End If

        End If

    End Sub

    Private Sub DataGridViewX2_KeyDown(sender As Object, e As KeyEventArgs) Handles DataGridViewX2.KeyDown
        'If e.KeyCode = Keys.Enter Then
        '    txtcombo_KeyPress(sender, New KeyPressEventArgs(Keys.Enter.ToString))
        'End If

    End Sub
End Class