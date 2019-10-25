Imports System.Data.OleDb
Imports System.Drawing
Imports System.IO
Imports System.Windows.Forms
Imports CrystalDecisions.CrystalReports.Engine
Imports CrystalDecisions.Shared

Imports Microsoft.Win32
Imports MySql.Data.MySqlClient

#Disable Warning BC40056 ' Namespace or type specified in the Imports 'Microsoft.Office.Interop.Excel' doesn't contain any public member or cannot be found. Make sure the namespace or the type is defined and contains at least one public member. Make sure the imported element name doesn't use any aliases.

Imports Excel = Microsoft.Office.Interop.Excel

#Enable Warning BC40056 ' Namespace or type specified in the Imports 'Microsoft.Office.Interop.Excel' doesn't contain any public member or cannot be found. Make sure the namespace or the type is defined and contains at least one public member. Make sure the imported element name doesn't use any aliases.

Public Module myModule

    Public firstchar_requiredforparametername As String = "?"

    Public n_DataBaseConnection As New DataBaseConnection()
    Public sys_servername, sys_userid, sys_password, sys_db, sys_apppath As String
    Public installerpath As String = String.Empty
    Public db_connectinstring = ""
    Public mysql_conn_text As String = n_DataBaseConnection.GetStringMySQLConnectionString

End Module