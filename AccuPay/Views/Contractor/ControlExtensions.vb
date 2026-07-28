Imports System.Runtime.CompilerServices

Public Module ControlExtensions

    <Extension()>
    Public Sub FocusToNext(ctrl As Control, Optional stopAtEnd As Boolean = False)
        If ctrl Is Nothing OrElse ctrl.Parent Is Nothing Then Return

        ' 1. Find the top-level container holding the pages (or the parent holding the sibling pages)
        '    We step up until we find a parent that has multiple child controls (pages).
        Dim page As Control = ctrl
        Dim container As Control = ctrl.Parent

        ' Walk up until we find a container with more than 1 child control
        While container IsNot Nothing AndAlso container.Controls.Count <= 1
            page = container
            container = container.Parent
        End While

        ' If no multi-control container exists, exit
        If container Is Nothing OrElse container.Controls.Count <= 1 Then Return

        ' 2. Find the current page's index among its siblings
        Dim currentIndex As Integer = container.Controls.IndexOf(page)
        If currentIndex = -1 Then Return

        ' 3. Calculate next index (WinForms Controls collection is usually ordered reverse Z-order, 
        '    so index progression depends on how pages are stacked, but generally Index + 1 or -1)
        Dim nextIndex As Integer

        If stopAtEnd Then
            If currentIndex < container.Controls.Count - 1 Then
                nextIndex = currentIndex + 1
            Else
                Return ' Already at the end
            End If
        Else
            nextIndex = (currentIndex + 1) Mod container.Controls.Count
        End If

        ' 4. Activate/Bring to Front the next view
        Dim nextPage As Control = container.Controls(nextIndex)
        nextPage.BringToFront()
        nextPage.Focus()

        ' Optionally, if using hidden/visible panels for page switching:
        ' For Each c As Control In container.Controls
        '     c.Visible = (c Equals nextPage)
        ' Next
    End Sub

End Module
