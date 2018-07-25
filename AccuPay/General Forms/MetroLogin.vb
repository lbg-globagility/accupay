Imports System.IO
Imports System.Threading.Tasks
Imports MetroFramework
Imports System.Threading
Imports System.Linq.Expressions
Imports MySql.Data.MySqlClient
Imports System.Xml

Public Class MetroLogin

    'Public n_FileObserver As New FileObserver(sys_apppath)

    Protected Overrides Sub OnLoad(e As EventArgs)

        'dbconn()

        Task.Factory.StartNew(Sub()

                                  'Write_XML()

                              End Sub).ContinueWith(Sub()

                                                        'Dim xmdoc As XDocument = XDocument.Load(xm_datafile)

                                                        ''Console.WriteLine(String.Concat("# # # # # # # #", Environment.NewLine,
                                                        ''                                xmdoc, Environment.NewLine,
                                                        ''                                "# # # # # # # #"))

                                                        ''# #############################
                                                        ''encoding="utf-8" 
                                                        'Dim doc As XDocument = _
                                                        '    <?xml version="1.0" standalone="yes"?>
                                                        '    <!--This is a comment-->
                                                        '    <Root>
                                                        '        <%= From el In xmdoc.<root>.Elements _
                                                        '            Where CStr(el) _
                                                        '            Select el %>
                                                        '    </Root>

                                                        ''.StartsWith("data")

                                                        'Console.WriteLine("#############################")

                                                        'For Each xmnod In doc.Nodes

                                                        '    Console.WriteLine(xmnod.ToString())

                                                        'Next

                                                        '# #############################
                                                        'Read_XML()
                                                        '# #############################

                                                    End Sub, TaskScheduler.FromCurrentSynchronizationContext)

        'Dim objReader As New System.IO.StreamReader("D:\DOWNLOADS\logfile 02.21.17 to 03.05.17.txt")

        ''ExecSQLMultiDimArrProcedure

        'Dim i As Integer = objReader.Peek()

        'Dim log_readlin(i) As String

        'Dim sdfsdf As New ArrayList

        'Do While objReader.Peek() >= 0

        '    Dim rl As String = Convert.ToString(objReader.ReadLine())

        '    sdfsdf.Add(rl)

        'Loop

        'Console.WriteLine(sdfsdf.Count)

        'objReader.Close()
        'objReader.Dispose()

        ReloadOrganization()

        setAppropriateInterface()

        MyBase.OnLoad(e)

    End Sub

    Private xm_datafile As String = String.Concat(sys_db, "_xmlschemadata.xml")

    Private xm_schemafile As String = String.Concat(sys_db, "_xmlschema.xml")

    Private Sub Write_XML()

        Dim ds As New DataSet(sys_db)

        Dim e_table As New SQLQueryToDatatable("SELECT * FROM goldwingspayrolldb_curr.agency WHERE OrganizationID=3 AND IsActive=1;")

        Dim catchdt As New DataTable
        catchdt = e_table.ResultTable
        catchdt.TableName = "agency"

        ds.Tables.Add(catchdt)

        catchdt = New SQLQueryToDatatable("SELECT e.EmployeeID,e.FirstName,e.LastName,e.AgencyID FROM goldwingspayrolldb_curr.employee e WHERE e.OrganizationID=3 AND e.AgencyID IS NOT NULL LIMIT 40;").ResultTable
        catchdt.TableName = "employee"

        ds.Tables.Add(catchdt)

        ds.Relations.Add("EmployeeAgency",
                           ds.Tables(0).Columns(0),
                           ds.Tables(1).Columns(3), True)
        'Column must belong to a table.

        Dim str_writer As New System.IO.StringWriter()
        'catchdt.WriteXmlSchema(writer, False)
        'catchdt.WriteXml(writer, True)
        ds.WriteXml(str_writer) ', "EmployeeAgency"

        'Console.WriteLine(str_writer.ToString())

        If str_writer IsNot Nothing Then
            str_writer.Dispose()
        End If

        ds.WriteXmlSchema(xm_schemafile)
        ds.WriteXml(xm_datafile)



    End Sub

    Dim xmlsett As XmlWriterSettings

    Private Sub Write_XML2()

        Dim ds As New DataSet(sys_db)

        Dim e_table As New SQLQueryToDatatable("SELECT * FROM agency WHERE OrganizationID=3 AND IsActive=1;")

        Dim catchdt As New DataTable
        catchdt = e_table.ResultTable
        catchdt.TableName = "agency"

        ds.Tables.Add(catchdt)

        Dim xmlfilename As String = "nested.xml"

        ds.WriteXml(xmlfilename)

        ds.Dispose()

        ds = New DataSet(String.Concat(sys_db, 2))

        ds.ReadXml(xmlfilename)

        xmlsett.Indent = True

        Dim xmlwrit As XmlWriter = XmlWriter.Create("goldwingspayrolldb_curr_xmlschemadata.xml", xmlsett)

        With xmlwrit
            .WriteStartDocument()

            .WriteAttributeString("Key", "Value")

            .WriteEndDocument()

            .Close()
        End With

    End Sub

    Sub Mein()

        Dim writer As XmlWriter = Nothing

        writer = XmlWriter.Create("sampledata.xml")

        ' Write the root element.
        writer.WriteStartElement("book")

        ' Write the xmlns:bk="urn:book" namespace declaration.
        writer.WriteAttributeString("xmlns", "bk", Nothing, "urn:book")

        ' Write the bk:ISBN="1-800-925" attribute.
        writer.WriteAttributeString("ISBN", "urn:book", "1-800-925")

        writer.WriteElementString("price", "19.95")

        ' Write the close tag for the root element.
        writer.WriteEndElement()

        ' Write the XML to file and close the writer.
        writer.Flush()
        writer.Close()

    End Sub

    Dim xmdoc As New XmlDocument

    Sub Read_XML()

        'xmdoc.Load(xm_datafile)

        ''Dim xmnodelist As XmlNode

        ''xmnodelist = xmdoc.ChildNodes

        'For Each xmnod As XmlNodeList In xmdoc.ChildNodes

        'Next

        'For Each xmnod As XmlNodeList In xmdoc.ChildNodes
        '    Console.WriteLine(xmnod.ToString())

        'Next

        'For Each node As XmlElement In nodelist
        '    Console.WriteLine(node("agency").InnerText)

        'Next

        '# ###########################

        'check if file myxml.xml is existing

        'create a new xmltextreader object
        'this is the object that we will loop and will be used to read the xml file
        Dim document As XmlReader = New XmlTextReader(xm_datafile)

        'loop through the xml file
        While (document.Read())

            Dim type = document.NodeType

            'Console.WriteLine(Convert.ToString(type.ToString))

            'if node type was element
            If (type = XmlNodeType.Element) Then

                ''if the loop found a <FirstName> tag
                'If (document.Name = "FirstName") Then

                '    Console.WriteLine(document.ReadInnerXml.ToString())

                'End If

                ''if the loop found a <LastName tag
                'If (document.Name = "LastName") Then

                'Console.WriteLine(document.ReadInnerXml)
                Console.WriteLine(document.ReadOuterXml)
                'Console.WriteLine(document.ReadElementContentAsString)

                'End If

            End If

        End While

    End Sub

    Protected Overrides Function ProcessCmdKey(ByRef msg As Message, keyData As Keys) As Boolean

        If keyData = Keys.Escape Then

            Me.Close()

            Return True

        ElseIf keyData = Keys.Oem5 Then

            Static thrice As Integer = -1

            thrice += 1

            If thrice = 5 Then

                thrice = 0

                Dim n_ShiftTemplater As _
                    New ViewTimeEntryEmployeeLevel

                'n_ShiftTemplater.Show()
                'n_ShiftTemplater.PrintPayslip(Me, New EventArgs)
                n_ShiftTemplater.Dispose()

            End If

            Return True

        Else
            'ShiftTemplater
            Return MyBase.ProcessCmdKey(msg, keyData)

        End If

    End Function

    Private Sub MetroLogin_FormClosing(sender As Object, e As FormClosingEventArgs) Handles Me.FormClosing

        'n_FileObserver.Undetect()

        Application.Exit()

    End Sub

    Private Sub MetroLogin_Load(sender As Object, e As EventArgs) Handles Me.Load

    End Sub

    Private Sub cbxorganiz_DropDown(sender As Object, e As EventArgs) Handles cbxorganiz.DropDown

        Static cb_font As Font = cbxorganiz.Font

        'Dim cb_width As Integer = cbxorganiz.DropDownWidth

        Dim grp As Graphics = cbxorganiz.CreateGraphics()

        Dim vertScrollBarWidth As Integer = If(cbxorganiz.Items.Count > cbxorganiz.MaxDropDownItems, SystemInformation.VerticalScrollBarWidth, 0)

        Dim wiidth As Integer = 0

        Dim data_source As New DataTable

        data_source = cbxorganiz.DataSource

        Dim i = 0

        Dim drp_downwidhths As Integer()

        ReDim drp_downwidhths(data_source.Rows.Count - 1)

        For Each strRow As DataRow In data_source.Rows

            wiidth = CInt(grp.MeasureString(CStr(strRow(1)), cb_font).Width) + vertScrollBarWidth

            drp_downwidhths(i) = wiidth

            'If cb_width < wiidth Then
            '    wiidth = wiidth
            'End If

            i += 1

        Next

        Dim max_drp_downwidhth As Integer = drp_downwidhths.Max

        cbxorganiz.DropDownWidth = max_drp_downwidhth 'wiidth, cb_width

    End Sub

    Private Sub Login_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txtbxPword.KeyPress,
                                                                                    txtbxUserID.KeyPress,
                                                                                    cbxorganiz.KeyPress

        Dim e_asc = Asc(e.KeyChar)

        If e_asc = 13 Then

            btnlogin_Click(btnlogin, New EventArgs)

        End If

    End Sub

    Private Const err_log_limit As SByte = 3 'the max limit of failed log in attempts

    Sub btnlogin_Click(sender As Object, e As EventArgs) Handles btnlogin.Click

        btnlogin.Enabled = False

        Dim n_edUserID As New EncryptData(txtbxUserID.Text)

        Dim n_edPWord As New EncryptData(txtbxPword.Text)

        Dim n_ReadSQLFunction As New ReadSQLFunction("user_improper_out",
                                                     "return_val",
                                                     orgztnID,
                                                     n_edUserID.ResultValue,
                                                     n_edPWord.ResultValue) 'New EncryptData(txtbxPword.Text).ResultValue

        z_User = UserAuthentication(n_edPWord.ResultValue)

        Console.WriteLine(DecrypedData("¯õæøøüô÷"))

        Static err_count As SByte = 0

        If z_User > 0 Then

            Try

                If cbxorganiz.SelectedIndex = -1 Then

                    WarnBalloon("Please select a company.", "Invalid company", btnlogin, btnlogin.Width - 18, -69)

                    cbxorganiz.Focus()
                    btnlogin.Enabled = True
                    Exit Sub

                End If



                err_count = 0 'resets the failed log in attempt count

                Dim userFNameLName = EXECQUER("SELECT CONCAT(COALESCE(FirstName,'.'),',',COALESCE(LastName,'.')) FROM user WHERE RowID='" & z_User & "';")

                Dim splitFNameLName = Split(userFNameLName, ",")

                userFirstName = splitFNameLName(0).ToString.Replace(".", "")

                If userFirstName = "" Then
                Else
                    userFirstName = StrConv(userFirstName, VbStrConv.ProperCase)

                End If

                userLastName = splitFNameLName(1).ToString.Replace(".", "")

                If userLastName = "" Then

                Else
                    userLastName = StrConv(userLastName, VbStrConv.ProperCase)

                End If

                If dbnow = Nothing Then

                    dbnow = EXECQUER(CURDATE_MDY)

                End If

                Static freq As Integer = -1

                If cbxorganiz.SelectedIndex <> -1 Then

                    numofdaysthisyear = EXECQUER("SELECT DAYOFYEAR(LAST_DAY(CONCAT(YEAR(CURRENT_DATE()),'-12-01')));")

                    If freq <> orgztnID Then
                        freq = orgztnID

                    End If

                    position_view_table =
                        New SQLQueryToDatatable(String.Concat("SELECT pv.*",
                                                              " FROM position_view pv",
                                                              " INNER JOIN `user` u ON u.RowID=", z_User,
                                                              " INNER JOIN `position` pos ON pos.RowID=u.PositionID",
                                                              " INNER JOIN `position` p ON p.PositionName=pos.PositionName AND p.OrganizationID=pv.OrganizationID",
                                                              " WHERE pv.PositionID=p.RowID",
                                                              " AND pv.OrganizationID='", orgztnID, "';")).ResultTable

                    Dim i = position_view_table.Rows.Count

                End If

                z_postName = EXECQUER("SELECT p.PositionName FROM user u INNER JOIN position p ON p.RowID=u.PositionID WHERE u.RowID='" & z_User & "';")

                If orgztnID <> Nothing Then
                    'Dim n_MDIPrimaryForm As New MDIPrimaryForm
                    MDIPrimaryForm.Show()
                    'n_MDIPrimaryForm.Show()
                End If

            Catch ex As Exception

                MsgBox(getErrExcptn(ex, Me.Name))

            End Try

        Else
            WarnBalloon("Please input your correct credentials.", "Invalid credentials", btnlogin, btnlogin.Width - 18, -69)

            txtbxUserID.Focus()

            err_count += 1 'increments the failed log in attempt

            'If (err_log_limit > err_count) = False Then 'failed log in attempt reaches its limit
            '    'prompts the log in failure
            '    MessageBox.Show("You have reached the failed login attempts." & Environment.NewLine & "Sorry, please retry later.",
            '                    "Failed login",
            '                    MessageBoxButtons.OK,
            '                    MessageBoxIcon.Stop)
            '    'then exits the application
            '    Me.Close()

            'End If

        End If
        btnlogin.Enabled = True
    End Sub

    Function UserAuthentication(Optional pass_word As Object = Nothing)
        'Optional user_id As Object = Nothing

        Dim returnvalue As Integer = 0

        'Dim params(1, 2) As Object

        'params(0, 0) = "user_name"
        'params(1, 0) = "pass_word"


        'Dim n_EncryptData As New EncryptData(txtbxUserID.Text)

        'params(0, 1) = n_EncryptData.ResultValue


        'n_EncryptData = New EncryptData(txtbxPword.Text)

        'params(1, 1) = n_EncryptData.ResultValue


        Dim returnobj = Nothing
        'EXEC_INSUPD_PROCEDURE(params,
        '                      "UserAuthentication",
        '                      "returnvaue")

        Dim pass_wordValue As Object

        If pass_word = Nothing Then

            pass_wordValue = Nothing
        Else

            pass_wordValue = pass_word
        End If

        Dim n_ReadSQLFunction As New ReadSQLFunction("UserAuthentication",
                                                     "returnvaue",
                                                     New EncryptData(txtbxUserID.Text).ResultValue,
                                                     pass_wordValue,
                                                     orgztnID) 'New EncryptData(txtbxPword.Text).ResultValue

        returnobj = n_ReadSQLFunction.ReturnValue

        returnvalue = ValNoComma(returnobj)

        Return returnvalue

    End Function

    Private Sub cbxorganiz_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cbxorganiz.SelectedIndexChanged

        PhotoImages.Image = Nothing

        orgNam = cbxorganiz.Text

        z_CompanyName = orgNam

        ''orgztnID = EXECQUER("SELECT RowID FROM organization WHERE Name='" & orgNam & "' LIMIT 1;")

        orgztnID = cbxorganiz.SelectedValue

        z_OrganizationID = ValNoComma(orgztnID)

        Dim org_emblem As New DataTable

        Dim n_SQLQueryToDatatable As New SQLQueryToDatatable("SELECT Image FROM organization WHERE RowID='" & orgztnID & "' AND Image IS NOT NULL;")

        org_emblem = n_SQLQueryToDatatable.ResultTable

        If org_emblem.Rows.Count > 0 Then

            PhotoImages.Image = ConvertByteToImage(org_emblem.Rows(0)("Image"))

        End If

    End Sub

    Private Sub cbxorganiz_SelectedValueChanged(sender As Object, e As EventArgs) Handles cbxorganiz.SelectedValueChanged

    End Sub

    Private Sub MetroLink1_Click(sender As Object, e As EventArgs) Handles MetroLink1.Click

        Dim n_ForgotPasswordForm As New ForgotPasswordForm

        n_ForgotPasswordForm.ShowDialog()
        '# ####################################### #
        'ForgotPasswordForm.Show()

        'MsgBox(Convert.ToBoolean("1"))

        'cbxorganiz.Enabled = Convert.ToBoolean("1")

        ''MsgBox(Convert.ToBoolean("0").ToString)

        'cbxorganiz.Enabled = Convert.ToBoolean("0")



        'Dim dialog_box = MessageBox.Show("Come on", "", MessageBoxButtons.YesNoCancel)

        'If dialog_box = Windows.Forms.DialogResult.Yes Then
        '    cbxorganiz.Enabled = Convert.ToBoolean(1)
        'Else
        '    cbxorganiz.Enabled = Convert.ToBoolean(0)
        'End If
        '# ####################################### #

    End Sub

    Private Sub MetroLogin_Resize(sender As Object, e As EventArgs) Handles Me.Resize

    End Sub

    Sub ReloadOrganization()

        Dim strQuery As String = "SELECT RowID,Name FROM organization WHERE NoPurpose='0' ORDER BY Name;"

        Static once As SByte = 0

        If once = 0 Then

            once = 1

            Dim n_SQLQueryToDatatable As New SQLQueryToDatatable(strQuery)

            cbxorganiz.ValueMember = n_SQLQueryToDatatable.ResultTable.Columns(0).ColumnName

            cbxorganiz.DisplayMember = n_SQLQueryToDatatable.ResultTable.Columns(1).ColumnName

            cbxorganiz.DataSource = n_SQLQueryToDatatable.ResultTable

        Else

            Dim isThereSomeNewToOrganization =
                EXECQUER("SELECT EXISTS(SELECT RowID FROM organization WHERE DATE_FORMAT(Created,'%Y-%m-%d')=CURDATE() OR DATE_FORMAT(LastUpd,'%Y-%m-%d')=CURDATE() LIMIT 1);")

            If isThereSomeNewToOrganization = "1" Then

                Dim n_SQLQueryToDatatable As New SQLQueryToDatatable(strQuery)

                cbxorganiz.ValueMember = n_SQLQueryToDatatable.ResultTable.Columns(0).ColumnName

                cbxorganiz.DisplayMember = n_SQLQueryToDatatable.ResultTable.Columns(1).ColumnName

                cbxorganiz.DataSource = n_SQLQueryToDatatable.ResultTable

            Else

            End If

        End If

    End Sub

    Private Sub txtbxUserID_Click(sender As Object, e As EventArgs) Handles txtbxUserID.Click

    End Sub

    Private Sub lnklblleave_LinkClicked(sender As Object, e As LinkLabelLinkClickedEventArgs) Handles lnklblleave.LinkClicked

        Dim n_LeaveForm As New LeaveForm

        With n_LeaveForm

            .CboListOfValue1.Visible = False

            .Label3.Visible = False

            .Label4.Visible = False

            .Show()

        End With

    End Sub

    Private Sub lnklblovertime_LinkClicked(sender As Object, e As LinkLabelLinkClickedEventArgs) Handles lnklblovertime.LinkClicked

        Dim n_OverTimeForm As New OverTimeForm

        With n_OverTimeForm

            .cboOTStatus.Visible = False

            .Label186.Visible = False

            .Label4.Visible = False

            .Show()

        End With

    End Sub

    Private Sub lnklblobf_LinkClicked(sender As Object, e As LinkLabelLinkClickedEventArgs) Handles lnklblobf.LinkClicked

        Dim n_OBFForm As New OBFForm

        With n_OBFForm

            .cboOBFStatus.Visible = False

            .Label186.Visible = False

            .Label4.Visible = False

            .Show()

        End With

    End Sub

    Private Sub setAppropriateInterface()

        Dim sys_ownr As New SystemOwner

        Dim current_system_owner As String = sys_ownr.CurrentSystemOwner

        Dim allowed_systemowners = New String() {SystemOwner.Hyundai}

        Dim disallowed_systemowners = New String() {SystemOwner.Goldwings, SystemOwner.Cinema2000, SystemOwner.DefaultOwner}

        If disallowed_systemowners.Contains(current_system_owner) Then

            Size = New Size(Size.Width, 319)
            PhotoImages.Location = New Point(242, 89)
            Panel1.Visible = False

        ElseIf allowed_systemowners.Contains(current_system_owner) Then

            Size = New Size(Size.Width, 371)
            PhotoImages.Location = New Point(242, 89)
            Panel1.Visible = True

        End If

    End Sub

    Private Sub LinkLabel1_LinkClicked(sender As Object, e As LinkLabelLinkClickedEventArgs) Handles LinkLabel1.LinkClicked

        Dim my_time_entry As New ViewTimeEntryEmployeeLevel

        my_time_entry.Show()

    End Sub

End Class

Friend Class EncryptData

    Dim n_ResultValue = Nothing

    Property ResultValue As Object

        Get
            Return n_ResultValue

        End Get

        Set(value As Object)
            n_ResultValue = value

        End Set

    End Property

    Sub New(StringToEncrypt As String)

        n_ResultValue = EncrypedData(StringToEncrypt)

    End Sub

    Private Function EncrypedData(ByVal a As String)

        Dim Encryped = Nothing

        If Not a Is Nothing Then

            For Each x As Char In a

                Dim ToCOn As Integer = Convert.ToInt64(x) + 133

                Encryped &= Convert.ToChar(Convert.ToInt64(ToCOn))

            Next

        End If

        Return Encryped

    End Function

    Private Sub SampleExpression()
        ' Creating an expression tree. 
        Dim expr As Expression(Of Func(Of Integer, Boolean)) =
            Function(num) num < 5

        ' Compiling the expression tree into a delegate. 
        Dim result As Func(Of Integer, Boolean) = expr.Compile()

        ' Invoking the delegate and writing the result to the console.
        Console.WriteLine(result(4))

        ' Prints True. 

        ' You can also use simplified syntax 
        ' to compile and run an expression tree. 
        ' The following line can replace two previous statements.
        Console.WriteLine(expr.Compile()(4))

        ' Also prints True.

    End Sub

End Class