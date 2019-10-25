Imports System.Data.OleDb
Imports System.Drawing
Imports System.IO
Imports System.Windows.Forms
Imports CrystalDecisions.CrystalReports.Engine
Imports CrystalDecisions.Shared

Imports Microsoft.Win32
Imports MySql.Data.MySqlClient
Imports Excel = Microsoft.Office.Interop.Excel

Module myModule

    Public firstchar_requiredforparametername As String = "?"

    Public n_DataBaseConnection As New DataBaseConnection()
    Public sys_servername, sys_userid, sys_password, sys_db, sys_apppath As String
    Public installerpath As String = String.Empty
    Public db_connectinstring = ""
    Public mysql_conn_text As String = n_DataBaseConnection.GetStringMySQLConnectionString

End Module