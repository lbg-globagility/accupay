Option Strict On

Imports System.Runtime.InteropServices
Imports AccuPay.Utilities

Public Class TimeTextBox
    Inherits TextBox

    <DllImport("user32")>
    Private Shared Function GetWindowDC(hwnd As IntPtr) As IntPtr
    End Function

    Private Const WM_NCPAINT As Integer = &H85

    Private _hasError As Boolean

    Private _value As TimeSpan?

    Public ReadOnly Property Value As TimeSpan?
        Get
            Return _value
        End Get
    End Property

    Public ReadOnly Property HasError As Boolean
        Get
            Return _hasError
        End Get
    End Property

    Public Sub New()
        MyBase.New()
    End Sub

    Protected Overrides Sub OnLeave(e As EventArgs)
        If DesignMode Then
            MyBase.OnLeave(e)
            Return
        End If

        _value = Calendar.ToTimespan(Text)
        If _value.HasValue Then
            Text = _value.Value.ToString("hh\:mm")
        End If

        _hasError = (Not _value.HasValue) And (Not String.IsNullOrWhiteSpace(Text))

        MyBase.OnLeave(e)
    End Sub

    Public Sub Commit()
        If DesignMode Then
            Return
        End If
        OnLeave(New EventArgs)
    End Sub

    Protected Overrides Sub WndProc(ByRef m As Message)
        MyBase.WndProc(m)
        If DesignMode Then
            Return
        End If

        If m.Msg = WM_NCPAINT And _hasError Then
            Dim dc = GetWindowDC(Handle)
            Using g = Graphics.FromHdc(dc)
                g.DrawRectangle(Pens.Red, 0, 0, Width - 1, Height - 1)
            End Using
        End If
    End Sub

End Class