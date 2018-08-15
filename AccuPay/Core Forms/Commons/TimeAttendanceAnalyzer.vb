Option Strict On

Imports System.Linq
Imports AccuPay.Entity

Public Class TimeAttendanceAnalyzer

    Public Function FourHoursRule(logs As IList(Of TimeAttendanceLog), employeeShifts As IList(Of ShiftSchedule)) As IList(Of TimeLogInOut)

        Dim list = New List(Of TimeLogInOut)
        Dim listIn = New List(Of TimeLogsTempIn)
        Dim listOut = New List(Of TimeLogsTempOut)

        Dim shift4HoursBefore = employeeShifts.FirstOrDefault.Shift.TimeFrom.Add(TimeSpan.FromHours(-4))
        Dim shift4HoursAfter = employeeShifts.FirstOrDefault.Shift.TimeFrom.Add(TimeSpan.FromHours(4))

        For Each log In logs
            Dim itemIn = New TimeLogsTempIn
            Dim itemOut = New TimeLogsTempOut
            Dim itemList = New TimeLogInOut

            Dim a = log
            Dim item4HoursBefore As DateTime = Nothing
            Dim item4HoursAfter As DateTime = Nothing
            Dim outOnly As DateTime = Nothing
            Dim endOfTheDay As DateTime = Nothing

            ' CHECK LISTIN AND LISTOUT TO INITIALIZE

            If (listIn.Count = 0 And listOut.Count <> 0) Then
                Dim dateOut As Date = CDate(listOut.LastOrDefault().DateOut)
                outOnly = dateOut.Date.Add(shift4HoursBefore)
                item4HoursBefore = a.DateTime.Date.Add(shift4HoursBefore)
                item4HoursAfter = a.DateTime.Date.Add(shift4HoursAfter)

            ElseIf listIn.Count <> 0 Then
                Dim dateIn As Date = CDate(listIn.FirstOrDefault().DateIn)
                endOfTheDay = dateIn.AddDays(1).Date.Add(shift4HoursBefore)
                item4HoursBefore = dateIn.Date.Add(shift4HoursBefore)
                item4HoursAfter = dateIn.Date.Add(shift4HoursAfter)
            Else
                item4HoursBefore = a.DateTime.Date.Add(shift4HoursBefore)
                item4HoursAfter = a.DateTime.Date.Add(shift4HoursAfter)

            End If
            ' END CHECK LISTIN AND LISTOUT TO INITIALIZE

            ' CHECK EMPLOYEE NUMBER IF NOT THE SAME TO STORE TO LIST
            If listIn.Count <> 0 And listOut.Count <> 0 Then
                If a.EmployeeNo <> listOut.LastOrDefault().EmployeeID Then
                    itemList.DateIn = listIn.FirstOrDefault().DateIn
                    itemList.DateOut = listOut.LastOrDefault().DateOut
                    itemList.TimeIn = listIn.FirstOrDefault().TimeIn
                    itemList.TimeOut = listOut.LastOrDefault().TimeOut
                    If listIn.FirstOrDefault().EmployeeID = listOut.LastOrDefault().EmployeeID Then
                        itemList.EmployeeID = listIn.FirstOrDefault().EmployeeID
                    End If
                    itemList.LogDate = CDate(listIn.FirstOrDefault().DateIn).Date.Add(TimeSpan.Parse("00:00:00"))
                    list.Add(itemList)

                    listIn.Clear()
                    listOut.Clear()

                    item4HoursBefore = a.DateTime.Date.Add(shift4HoursBefore)
                    item4HoursAfter = a.DateTime.Date.Add(shift4HoursAfter)
                End If
            ElseIf listIn.Count <> 0 And listOut.Count = 0 Then
                If a.EmployeeNo <> listIn.FirstOrDefault().EmployeeID Then
                    itemList.DateIn = listIn.FirstOrDefault().DateIn
                    itemList.DateOut = Nothing
                    itemList.TimeIn = listIn.FirstOrDefault().TimeIn
                    itemList.TimeOut = Nothing
                    itemList.EmployeeID = listIn.FirstOrDefault().EmployeeID

                    itemList.LogDate = CDate(listIn.FirstOrDefault().DateIn).Date.Add(TimeSpan.Parse("00:00:00"))

                    list.Add(itemList)

                    listIn.Clear()
                    listOut.Clear()
                    '//if clear initialize

                    item4HoursBefore = a.DateTime.Date.Add(shift4HoursBefore)
                    item4HoursAfter = a.DateTime.Date.Add(shift4HoursAfter)
                End If
            ElseIf listIn.Count = 0 And listOut.Count <> 0 Then
                If a.EmployeeNo <> listOut.LastOrDefault().EmployeeID Then
                    itemList.DateIn = Nothing
                    itemList.DateOut = listOut.LastOrDefault().DateOut
                    itemList.TimeIn = Nothing
                    itemList.TimeOut = listOut.LastOrDefault().TimeOut
                    itemList.EmployeeID = listOut.LastOrDefault().EmployeeID

                    If listOut.LastOrDefault().TimeOut <= shift4HoursBefore Then
                        itemList.LogDate = CDate(listOut.FirstOrDefault().DateOut).AddDays(-1).Date.Add(TimeSpan.Parse("00:00:00"))
                    Else
                        itemList.LogDate = CDate(listOut.FirstOrDefault().DateOut).Date.Add(TimeSpan.Parse("00:00:00"))
                    End If

                    list.Add(itemList)

                    listIn.Clear()
                    listOut.Clear()

                    '//if clear initialize
                    item4HoursBefore = a.DateTime.Date.Add(shift4HoursBefore)
                    item4HoursAfter = a.DateTime.Date.Add(shift4HoursAfter)
                End If
            Else

            End If
            ' END CHECK EMPLOYEE NUMBER IF NOT THE SAME TO STORE TO LIST

            '  CHECK LISTIN AND LISTOUT AND CURRENT DATA ITERATED TO BE COMPARE AT END OF THE DAY TO STORE LIST

            If listIn.Count <> 0 And listOut.Count <> 0 And endOfTheDay <= a.DateTime Then
                itemList.DateIn = listIn.FirstOrDefault().DateIn
                itemList.DateOut = listOut.LastOrDefault().DateOut
                itemList.TimeIn = listIn.FirstOrDefault().TimeIn
                itemList.TimeOut = listOut.LastOrDefault().TimeOut

                If listIn.FirstOrDefault().EmployeeID = listOut.LastOrDefault().EmployeeID Then
                    itemList.EmployeeID = listIn.FirstOrDefault().EmployeeID
                End If
                itemList.LogDate = CDate(listIn.FirstOrDefault().DateIn).Date.Add(TimeSpan.Parse("00:00:00"))
                list.Add(itemList)

                listIn.Clear()
                listOut.Clear()
                '//if clear initialize

                item4HoursBefore = a.DateTime.Date.Add(shift4HoursBefore)
                item4HoursAfter = a.DateTime.Date.Add(shift4HoursAfter)

            ElseIf listIn.Count <> 0 And listOut.Count = 0 And endOfTheDay <= a.DateTime Then
                itemList.DateIn = listIn.FirstOrDefault().DateIn
                itemList.DateOut = Nothing
                itemList.TimeIn = listIn.FirstOrDefault().TimeIn
                itemList.TimeOut = Nothing
                itemList.EmployeeID = listIn.FirstOrDefault().EmployeeID
                itemList.LogDate = CDate(listIn.FirstOrDefault().DateIn).Date.Add(TimeSpan.Parse("00:00:00"))
                list.Add(itemList)

                listIn.Clear()
                listOut.Clear()
                '//if clear initialize

                item4HoursBefore = a.DateTime.Date.Add(shift4HoursBefore)
                item4HoursAfter = a.DateTime.Date.Add(shift4HoursAfter)

            ElseIf listIn.Count = 0 And listOut.Count <> 0 And outOnly <= a.DateTime Then
                itemList.DateIn = Nothing
                itemList.DateOut = listOut.LastOrDefault().DateOut
                itemList.TimeIn = Nothing
                itemList.TimeOut = listOut.LastOrDefault().TimeOut
                itemList.EmployeeID = listOut.LastOrDefault().EmployeeID
                If listOut.LastOrDefault().TimeOut < shift4HoursBefore Then
                    itemList.LogDate = CDate(listOut.FirstOrDefault().DateOut).AddDays(-1).Date.Add(TimeSpan.Parse("00:00:00"))
                Else
                    itemList.LogDate = CDate(listOut.FirstOrDefault().DateOut).Date.Add(TimeSpan.Parse("00:00:00"))
                End If

                list.Add(itemList)

                listIn.Clear()
                listOut.Clear()
                '//if clear initialize

                item4HoursBefore = a.DateTime.Date.Add(shift4HoursBefore)
                item4HoursAfter = a.DateTime.Date.Add(shift4HoursAfter)
            Else

            End If
            'END CHECK LISTIN AND LISTOUT AND CURRENT DATA ITERATED TO BE COMPARE AT END OF THE DAY TO STORE LIST

            'STORE IN AND sTORE OUT
            If a.DateTime >= item4HoursBefore And a.DateTime <= item4HoursAfter Then
                itemIn.TimeIn = a.DateTime.TimeOfDay
                itemIn.DateIn = a.DateTime
                itemIn.EmployeeID = a.EmployeeNo
                listIn.Add(itemIn)
            Else
                itemOut.TimeOut = a.DateTime.TimeOfDay
                itemOut.DateOut = a.DateTime
                itemOut.EmployeeID = a.EmployeeNo
                listOut.Add(itemOut)
            End If

        Next
        ' IF END OF RANGE OF ITERATED ROW HAS LISTIN OR LISTOUT
        Dim itemListExcess = New TimeLogInOut

        If listIn.Count <> 0 And listOut.Count = 0 Then
            itemListExcess.DateIn = listIn.FirstOrDefault().DateIn
            itemListExcess.TimeIn = listIn.FirstOrDefault().TimeIn
            itemListExcess.DateOut = Nothing
            itemListExcess.TimeOut = Nothing
            itemListExcess.EmployeeID = listIn.FirstOrDefault().EmployeeID
            itemListExcess.LogDate = CDate(listIn.FirstOrDefault().DateIn).Date.Add(TimeSpan.Parse("00:00:00"))
            list.Add(itemListExcess)
            listIn.Clear()
            listOut.Clear()
        End If

        If listOut.Count <> 0 And listIn.Count = 0 Then
            itemListExcess.DateOut = listOut.LastOrDefault().DateOut
            itemListExcess.TimeOut = listOut.LastOrDefault().TimeOut
            itemListExcess.DateIn = Nothing
            itemListExcess.TimeIn = Nothing
            itemListExcess.EmployeeID = listOut.LastOrDefault().EmployeeID
            If listOut.LastOrDefault().TimeOut < shift4HoursBefore Then
                itemListExcess.LogDate = CDate(listOut.FirstOrDefault().DateOut).AddDays(-1).Date.Add(TimeSpan.Parse("00:00:00"))
            Else
                itemListExcess.LogDate = CDate(listOut.FirstOrDefault().DateOut).Date.Add(TimeSpan.Parse("00:00:00"))
            End If

            list.Add(itemListExcess)
            listIn.Clear()
            listOut.Clear()

        End If

        If listOut.Count <> 0 And listIn.Count <> 0 Then
            itemListExcess.DateOut = listOut.LastOrDefault().DateOut
            itemListExcess.TimeOut = listOut.LastOrDefault().TimeOut
            itemListExcess.DateIn = listIn.FirstOrDefault().DateIn
            itemListExcess.TimeIn = listIn.FirstOrDefault().TimeIn
            itemListExcess.LogDate = CDate(listIn.FirstOrDefault().DateIn).Date.Add(TimeSpan.Parse("00:00:00"))
            If listIn.FirstOrDefault().EmployeeID = listOut.LastOrDefault().EmployeeID Then
                itemListExcess.EmployeeID = listIn.FirstOrDefault().EmployeeID
            End If
            list.Add(itemListExcess)
            listIn.Clear()
            listOut.Clear()

        End If
        ' END IF END OF RANGE OF ITERATED ROW HAS LISTIN OR LISTOUT

        Return list
    End Function

    Public Function FourHoursRuleBasedOnEmployeeShift(logs As IList(Of TimeAttendanceLog), employeeShifts As IList(Of ShiftSchedule)) As IList(Of TimeLogInOut)

        Dim list = New List(Of TimeLogInOut)
        Dim listIn = New List(Of TimeLogsTempIn)
        Dim listOut = New List(Of TimeLogsTempOut)

        For Each employeeShift In employeeShifts

            Dim shift4HoursBefore = employeeShift.Shift.TimeFrom.Add(TimeSpan.FromHours(-4))
            Dim shift4HoursAfter = employeeShift.Shift.TimeFrom.Add(TimeSpan.FromHours(4))
            Dim effectiveFrom = employeeShift.EffectiveFrom.Date

            For Each log In logs
                Dim itemIn = New TimeLogsTempIn
                Dim itemOut = New TimeLogsTempOut
                Dim itemList = New TimeLogInOut

                Dim a = log
                Dim item4HoursBefore As DateTime = Nothing
                Dim item4HoursAfter As DateTime = Nothing
                Dim outOnly As DateTime = Nothing
                Dim endOfTheDay As DateTime = Nothing

                ' CHECK LISTIN AND LISTOUT TO INITIALIZE

                If (listIn.Count = 0 And listOut.Count <> 0) Then
                    Dim dateOut As Date = CDate(listOut.LastOrDefault().DateOut)
                    outOnly = dateOut.Date.Add(shift4HoursBefore)
                    item4HoursBefore = a.DateTime.Date.Add(shift4HoursBefore)
                    item4HoursAfter = a.DateTime.Date.Add(shift4HoursAfter)

                ElseIf listIn.Count <> 0 Then
                    Dim dateIn As Date = CDate(listIn.FirstOrDefault().DateIn)
                    endOfTheDay = dateIn.AddDays(1).Date.Add(shift4HoursBefore)
                    item4HoursBefore = dateIn.Date.Add(shift4HoursBefore)
                    item4HoursAfter = dateIn.Date.Add(shift4HoursAfter)
                Else
                    item4HoursBefore = a.DateTime.Date.Add(shift4HoursBefore)
                    item4HoursAfter = a.DateTime.Date.Add(shift4HoursAfter)

                End If
                ' END CHECK LISTIN AND LISTOUT TO INITIALIZE
                If listIn.Count = 0 And listOut.Count = 0 Then

                    If a.DateTime >= effectiveFrom.Date.Add(shift4HoursBefore) And a.DateTime <= effectiveFrom.Date.Add(shift4HoursAfter) Then
                        'in
                    ElseIf a.DateTime > effectiveFrom.Date.Add(shift4HoursAfter) And a.DateTime < effectiveFrom.AddDays(1).Date.Add(shift4HoursBefore) Then
                        'out
                    Else

                        Continue For

                    End If

                ElseIf listIn.Count <> 0 And listOut.Count = 0 Then
                    If a.DateTime < endOfTheDay Then
                        'in
                    Else

                        Continue For

                    End If

                ElseIf listIn.Count = 0 And listOut.Count <> 0 Then
                    If a.DateTime >= effectiveFrom.Date.Add(shift4HoursBefore) And a.DateTime <= effectiveFrom.Date.Add(shift4HoursAfter) Then
                        'in
                    ElseIf effectiveFrom.AddDays(1).Date.Add(shift4HoursBefore) >= a.DateTime Then
                        'out
                    Else

                        Continue For

                    End If
                Else
                    If a.DateTime >= effectiveFrom.Date.Add(shift4HoursBefore) And a.DateTime <= effectiveFrom.Date.Add(shift4HoursAfter) Then
                        'in
                    ElseIf effectiveFrom.AddDays(1).Date.Add(shift4HoursBefore) > a.DateTime Then
                        'out
                    ElseIf effectiveFrom.AddDays(1).Date.Add(shift4HoursBefore) > a.DateTime Then
                        'out
                    Else

                        Continue For

                    End If

                End If

                ' CHECK EMPLOYEE NUMBER IF NOT THE SAME TO STORE TO LIST
                If listIn.Count <> 0 And listOut.Count <> 0 Then
                    If a.EmployeeNo <> listOut.LastOrDefault().EmployeeID Then
                        itemList.DateIn = listIn.FirstOrDefault().DateIn
                        itemList.DateOut = listOut.LastOrDefault().DateOut
                        itemList.TimeIn = listIn.FirstOrDefault().TimeIn
                        itemList.TimeOut = listOut.LastOrDefault().TimeOut
                        If listIn.FirstOrDefault().EmployeeID = listOut.LastOrDefault().EmployeeID Then
                            itemList.EmployeeID = listIn.FirstOrDefault().EmployeeID
                        End If
                        itemList.LogDate = CDate(listIn.FirstOrDefault().DateIn).Date.Add(TimeSpan.Parse("00:00:00"))
                        list.Add(itemList)

                        listIn.Clear()
                        listOut.Clear()

                        item4HoursBefore = a.DateTime.Date.Add(shift4HoursBefore)
                        item4HoursAfter = a.DateTime.Date.Add(shift4HoursAfter)
                    End If
                ElseIf listIn.Count <> 0 And listOut.Count = 0 Then
                    If a.EmployeeNo <> listIn.FirstOrDefault().EmployeeID Then
                        itemList.DateIn = listIn.FirstOrDefault().DateIn
                        itemList.DateOut = Nothing
                        itemList.TimeIn = listIn.FirstOrDefault().TimeIn
                        itemList.TimeOut = Nothing
                        itemList.EmployeeID = listIn.FirstOrDefault().EmployeeID

                        itemList.LogDate = CDate(listIn.FirstOrDefault().DateIn).Date.Add(TimeSpan.Parse("00:00:00"))

                        list.Add(itemList)

                        listIn.Clear()
                        listOut.Clear()
                        '//if clear initialize

                        item4HoursBefore = a.DateTime.Date.Add(shift4HoursBefore)
                        item4HoursAfter = a.DateTime.Date.Add(shift4HoursAfter)
                    End If
                ElseIf listIn.Count = 0 And listOut.Count <> 0 Then
                    If a.EmployeeNo <> listOut.LastOrDefault().EmployeeID Then
                        itemList.DateIn = Nothing
                        itemList.DateOut = listOut.LastOrDefault().DateOut
                        itemList.TimeIn = Nothing
                        itemList.TimeOut = listOut.LastOrDefault().TimeOut
                        itemList.EmployeeID = listOut.LastOrDefault().EmployeeID

                        If listOut.LastOrDefault().TimeOut <= shift4HoursBefore Then
                            itemList.LogDate = CDate(listOut.FirstOrDefault().DateOut).AddDays(-1).Date.Add(TimeSpan.Parse("00:00:00"))
                        Else
                            itemList.LogDate = CDate(listOut.FirstOrDefault().DateOut).Date.Add(TimeSpan.Parse("00:00:00"))
                        End If

                        list.Add(itemList)

                        listIn.Clear()
                        listOut.Clear()

                        '//if clear initialize
                        item4HoursBefore = a.DateTime.Date.Add(shift4HoursBefore)
                        item4HoursAfter = a.DateTime.Date.Add(shift4HoursAfter)
                    End If
                Else

                End If
                ' END CHECK EMPLOYEE NUMBER IF NOT THE SAME TO STORE TO LIST

                '  CHECK LISTIN AND LISTOUT AND CURRENT DATA ITERATED TO BE COMPARE AT END OF THE DAY TO STORE LIST

                If listIn.Count <> 0 And listOut.Count <> 0 And endOfTheDay <= a.DateTime Then
                    itemList.DateIn = listIn.FirstOrDefault().DateIn
                    itemList.DateOut = listOut.LastOrDefault().DateOut
                    itemList.TimeIn = listIn.FirstOrDefault().TimeIn
                    itemList.TimeOut = listOut.LastOrDefault().TimeOut

                    If listIn.FirstOrDefault().EmployeeID = listOut.LastOrDefault().EmployeeID Then
                        itemList.EmployeeID = listIn.FirstOrDefault().EmployeeID
                    End If
                    itemList.LogDate = CDate(listIn.FirstOrDefault().DateIn).Date.Add(TimeSpan.Parse("00:00:00"))
                    list.Add(itemList)

                    listIn.Clear()
                    listOut.Clear()
                    '//if clear initialize

                    item4HoursBefore = a.DateTime.Date.Add(shift4HoursBefore)
                    item4HoursAfter = a.DateTime.Date.Add(shift4HoursAfter)

                ElseIf listIn.Count <> 0 And listOut.Count = 0 And endOfTheDay <= a.DateTime Then
                    itemList.DateIn = listIn.FirstOrDefault().DateIn
                    itemList.DateOut = Nothing
                    itemList.TimeIn = listIn.FirstOrDefault().TimeIn
                    itemList.TimeOut = Nothing
                    itemList.EmployeeID = listIn.FirstOrDefault().EmployeeID
                    itemList.LogDate = CDate(listIn.FirstOrDefault().DateIn).Date.Add(TimeSpan.Parse("00:00:00"))
                    list.Add(itemList)

                    listIn.Clear()
                    listOut.Clear()
                    '//if clear initialize

                    item4HoursBefore = a.DateTime.Date.Add(shift4HoursBefore)
                    item4HoursAfter = a.DateTime.Date.Add(shift4HoursAfter)

                ElseIf listIn.Count = 0 And listOut.Count <> 0 And outOnly <= a.DateTime Then
                    itemList.DateIn = Nothing
                    itemList.DateOut = listOut.LastOrDefault().DateOut
                    itemList.TimeIn = Nothing
                    itemList.TimeOut = listOut.LastOrDefault().TimeOut
                    itemList.EmployeeID = listOut.LastOrDefault().EmployeeID
                    If listOut.LastOrDefault().TimeOut < shift4HoursBefore Then
                        itemList.LogDate = CDate(listOut.FirstOrDefault().DateOut).AddDays(-1).Date.Add(TimeSpan.Parse("00:00:00"))
                    Else
                        itemList.LogDate = CDate(listOut.FirstOrDefault().DateOut).Date.Add(TimeSpan.Parse("00:00:00"))
                    End If

                    list.Add(itemList)

                    listIn.Clear()
                    listOut.Clear()
                    '//if clear initialize

                    item4HoursBefore = a.DateTime.Date.Add(shift4HoursBefore)
                    item4HoursAfter = a.DateTime.Date.Add(shift4HoursAfter)
                Else

                End If
                'END CHECK LISTIN AND LISTOUT AND CURRENT DATA ITERATED TO BE COMPARE AT END OF THE DAY TO STORE LIST

                'STORE IN AND sTORE OUT
                If a.DateTime >= item4HoursBefore And a.DateTime <= item4HoursAfter Then
                    itemIn.TimeIn = a.DateTime.TimeOfDay
                    itemIn.DateIn = a.DateTime
                    itemIn.EmployeeID = a.EmployeeNo
                    listIn.Add(itemIn)
                Else
                    itemOut.TimeOut = a.DateTime.TimeOfDay
                    itemOut.DateOut = a.DateTime
                    itemOut.EmployeeID = a.EmployeeNo
                    listOut.Add(itemOut)
                End If

            Next
            ' IF END OF RANGE OF ITERATED ROW HAS LISTIN OR LISTOUT
            Dim itemListExcess = New TimeLogInOut

            If listIn.Count <> 0 And listOut.Count = 0 Then
                If effectiveFrom.Date.Add(shift4HoursAfter) >= listIn.FirstOrDefault().DateIn And
                    effectiveFrom.Date.Add(shift4HoursBefore) <= listIn.FirstOrDefault().DateIn Then

                    itemListExcess.DateIn = listIn.FirstOrDefault().DateIn
                    itemListExcess.TimeIn = listIn.FirstOrDefault().TimeIn
                    itemListExcess.DateOut = Nothing
                    itemListExcess.TimeOut = Nothing
                    itemListExcess.EmployeeID = listIn.FirstOrDefault().EmployeeID
                    itemListExcess.LogDate = CDate(listIn.FirstOrDefault().DateIn).Date.Add(TimeSpan.Parse("00:00:00"))
                    list.Add(itemListExcess)
                    listIn.Clear()
                    listOut.Clear()
                Else
                    listIn.Clear()
                    listOut.Clear()
                End If



            End If

            If listOut.Count <> 0 And listIn.Count = 0 Then
                If effectiveFrom.AddDays(1).Date.Add(shift4HoursBefore) >= listOut.LastOrDefault().DateOut Then
                    itemListExcess.DateOut = listOut.LastOrDefault().DateOut
                    itemListExcess.TimeOut = listOut.LastOrDefault().TimeOut
                    itemListExcess.DateIn = Nothing
                    itemListExcess.TimeIn = Nothing
                    itemListExcess.EmployeeID = listOut.LastOrDefault().EmployeeID
                    If listOut.LastOrDefault().TimeOut < shift4HoursBefore Then
                        itemListExcess.LogDate = CDate(listOut.FirstOrDefault().DateOut).AddDays(-1).Date.Add(TimeSpan.Parse("00:00:00"))
                    Else
                        itemListExcess.LogDate = CDate(listOut.FirstOrDefault().DateOut).Date.Add(TimeSpan.Parse("00:00:00"))
                    End If

                    list.Add(itemListExcess)
                    listIn.Clear()
                    listOut.Clear()
                Else
                    listIn.Clear()
                    listOut.Clear()
                End If


            End If

            If listOut.Count <> 0 And listIn.Count <> 0 Then

                If effectiveFrom.Date.Add(shift4HoursAfter) >= listIn.FirstOrDefault().DateIn And
                    effectiveFrom.Date.Add(shift4HoursBefore) <= listIn.FirstOrDefault().DateIn And
                    effectiveFrom.AddDays(1).Date.Add(shift4HoursBefore) >= listOut.LastOrDefault().DateOut Then
                    itemListExcess.DateOut = listOut.LastOrDefault().DateOut
                    itemListExcess.TimeOut = listOut.LastOrDefault().TimeOut
                    itemListExcess.DateIn = listIn.FirstOrDefault().DateIn
                    itemListExcess.TimeIn = listIn.FirstOrDefault().TimeIn
                    itemListExcess.LogDate = CDate(listIn.FirstOrDefault().DateIn).Date.Add(TimeSpan.Parse("00:00:00"))
                    If listIn.FirstOrDefault().EmployeeID = listOut.LastOrDefault().EmployeeID Then
                        itemListExcess.EmployeeID = listIn.FirstOrDefault().EmployeeID
                    End If
                    list.Add(itemListExcess)
                    listIn.Clear()
                    listOut.Clear()
                Else
                    listIn.Clear()
                    listOut.Clear()

                End If


            End If
            ' END IF END OF RANGE OF ITERATED ROW HAS LISTIN OR LISTOUT

        Next

        Return list
    End Function

End Class