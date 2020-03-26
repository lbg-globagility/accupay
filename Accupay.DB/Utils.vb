Public Class Utils

    Public Shared Function getErrExcptn(ByVal ex As Exception, Optional FormNam As String = Nothing) As String
        Dim st As StackTrace = New StackTrace(ex, True)
        Dim sf As StackFrame = st.GetFrame(st.FrameCount - 1)

        Dim op_FrmNam As String = If(FormNam = Nothing, "", FormNam & ".")
        'Form Name '.' Method Name '@Line' Code Line Number

        Dim mystr As String = ex.Message & vbNewLine & vbNewLine &
                        "ERROR occured in " & op_FrmNam &
                        st.GetFrame(st.FrameCount - 1).GetMethod.Name &
                        " " & sf.GetFileLineNumber() &
                        " " & ex.ToString()
        '               'ito ung line number sa code editor
        'ErrorLog(mystr)

        Return mystr

        'Return MsgBox(mystr, , "Unexpected Message")
    End Function

End Class