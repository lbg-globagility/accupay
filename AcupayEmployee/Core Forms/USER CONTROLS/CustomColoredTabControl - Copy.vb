
Public Class CustomColoredTabControl

    Inherits TabControl

    Sub New()

        Me.DrawMode = TabDrawMode.OwnerDrawFixed

        Me.ItemSize = New Size(152, 25)

        Me.Alignment = TabAlignment.Bottom

    End Sub

    Protected Overrides Sub OnDrawItem(e As DrawItemEventArgs)

        ThisTabControlColor(Me, e, Color.White)

        MyBase.OnDrawItem(e)

    End Sub

    Sub ThisTabControlColor(ByVal TabCntrl As TabControl, _
                        ByVal ee As System.Windows.Forms.DrawItemEventArgs, _
                        Optional formColor As Color = Nothing)

        Dim g As Graphics = ee.Graphics
        Dim tp As TabPage = TabCntrl.TabPages(ee.Index)
        Dim br As Brush
        Dim sf As New StringFormat

        Dim r As New RectangleF(ee.Bounds.X, ee.Bounds.Y + 7, ee.Bounds.Width, ee.Bounds.Height - 7) '

        If formColor <> Nothing Then

            'ee.Graphics.FillRectangle(BackBrush, myTabRect)

            'Dim transparBackBrush = New SolidBrush(Color.Red)

            'BackBrush.Dispose()
            'transparBackBrush.Dispose()

            '====ito yung pagkulay sa puwang ng Items ng Tabcontrol
            'Dim custPen = New Pen(Color.Transparent, 2)

            '====ito yung pagkulay sa Border ng Tabcontrol
            If TabCntrl.Alignment = TabAlignment.Top Then
                Dim _myPen As New Pen(formColor, 7) 'Color.Red
                '- ((TabCntrl.Bounds.X * 0.01) + 2)
                'TabCntrl.Bounds.X - ((TabCntrl.Bounds.X * 0.05))
                ' + 2
                Dim myTabRect As Rectangle = New Rectangle(0, 0, TabCntrl.Width - ((TabCntrl.Width * 0.01)), TabCntrl.Height - 3)
                'Dim myTabRect As Rectangle = New Rectangle(0, 0, TabCntrl.Width, TabCntrl.Height)

                'Dim BackBrush = New SolidBrush(formColor)

                ee.Graphics.DrawRectangle(_myPen, myTabRect)

                Dim custBr = New SolidBrush(formColor)

                Dim x = 0
                For i = 0 To TabCntrl.TabCount - 1
                    x += ee.Bounds.Width
                Next
                '                                                                                             '+ x - (x * 0.08)
                Dim myCustRect = New Rectangle(x - (x * 0.08), 0, TabCntrl.Width - ((TabCntrl.Width * 0.02) - 2), ee.Bounds.Height)

                'ee.Graphics.DrawRectangle(custPen, myCustRect)

                ee.Graphics.FillRectangle(custBr, myCustRect)
                '====ito yung pagkulay sa puwang ng Items ng Tabcontrol

            ElseIf TabCntrl.Alignment = TabAlignment.Bottom Then

                '====ito yung pagkulay sa puwang ng Items ng Tabcontrol

                'Dim custBrBot = New SolidBrush(Color.Red) 'formColor

                'Dim x = 0
                'For i = 0 To TabCntrl.TabCount - 1
                '    x += ee.Bounds.Width
                'Next

                ''Dim _myPen As New Pen(Color.Red, 7) 'Color.Red'formColor
                ' ''- ((TabCntrl.Bounds.X * 0.01) + 2)
                ' ''TabCntrl.Bounds.X - ((TabCntrl.Bounds.X * 0.05))
                ' '' + 2
                ''Dim myTabRect As Rectangle = New Rectangle(x + (x * 0.01), ee.Bounds.Y, TabCntrl.Width - x, TabCntrl.Height - 3)
                ' ''Dim myTabRect As Rectangle = New Rectangle(0, 0, TabCntrl.Width, TabCntrl.Height)

                ' ''Dim BackBrush = New SolidBrush(formColor)

                ''ee.Graphics.DrawRectangle(_myPen, myTabRect)


                'Dim myCustRectBot = New Rectangle(x, ee.Bounds.Y, TabCntrl.Width - x, ee.Bounds.Height) 'TabCntrl.Width 

                ''ee.Graphics.DrawRectangle(custPen, myCustRect)

                'ee.Graphics.FillRectangle(custBrBot, myCustRectBot)
                '====ito yung pagkulay sa puwang ng Items ng Tabcontrol

            End If
            '====ito yung pagkulay sa Border ng Tabcontrol

        End If

        'Dim TabTextBrush As Brush = New SolidBrush(Color.White)
        Dim TabTextBrush As Brush = New SolidBrush(Color.FromArgb(142, 33, 11))
        'Dim TabBackBrush As Brush = New SolidBrush(Color.FromArgb(255, 242, 157))'255, 200, 80
        Dim TabBackBrush As Brush = New SolidBrush(Color.FromArgb(255, 245, 160)) '255, 255, 85
        '200, 190, 110
        'Dim TabTextBrush As Brush = New SolidBrush(Color.Black) 'FromArgb(142, 33, 11)
        'Dim TabBackBrush As Brush = New SolidBrush(Color.FromArgb(255, 242, 157))

        sf.Alignment = StringAlignment.Center

        Dim strTitle As String = tp.Text
        'If the current index is the Selected Index, change the color
        If TabCntrl.SelectedIndex = ee.Index Then
            'this is the background color of the tabpage
            'you could make this a standard color for the selected page
            'br = New SolidBrush(tp.BackColor)

            br = TabBackBrush
            'br = New SolidBrush(Color.PowderBlue)

            'this is the background color of the tab page
            g.FillRectangle(br, ee.Bounds)
            'this is the background color of the tab page
            'you could make this a stndard color for the selected page
            'br = New SolidBrush(tp.ForeColor)
            ' I changed to specific color
            br = TabTextBrush
            ' Tried bold, didn't like it
            Dim ff As Font
            ff = New Font(TabCntrl.Font, FontStyle.Bold)
            g.DrawString(strTitle, ff, br, r, sf)
            'g.DrawString("TAB PAGE 1", TabCntrl.Font, br, r, sf)
        Else
            'these are the standard colors for the unselected tab pages

            'br = New SolidBrush(Color.WhiteSmoke)

            'Dim small_rect As Rectangle = New Rectangle(ee.Bounds.X, _
            '                                            ee.Bounds.Y + 7, _
            '                                            ee.Bounds.Width, _
            '                                            ee.Bounds.Height - 7)

            'Color.FromArgb(formColor.ToArgb)
            br = New SolidBrush(Color.WhiteSmoke) 'formColor
            g.FillRectangle(br, ee.Bounds)
            br = New SolidBrush(Color.Black)
            Dim ff As Font
            ff = New Font(TabCntrl.Font, FontStyle.Regular)
            g.DrawString(strTitle, TabCntrl.Font, br, r, sf)

        End If

    End Sub

End Class