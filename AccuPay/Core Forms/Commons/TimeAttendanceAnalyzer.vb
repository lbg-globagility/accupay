Option Strict On
Imports AccuPay.Entity

Public Class TimeAttendanceAnalyzer


    Public Function AnalyzeMe(logs As IList(Of TimeAttendanceLog), employeeshifts As IList(Of ShiftSchedule)) As IList(Of TimeLogInOut)


        Dim list = New List(Of TimeLogInOut)
        Dim listin = New List(Of TimeLogsTempIn)
        Dim listout = New List(Of TimeLogsTempOut)


        For Each log In logs
            Dim itemin = New TimeLogsTempIn
            Dim itemout = New TimeLogsTempOut
            Dim itemlist = New TimeLogInOut

            Dim shift4hoursbefore = employeeshifts.FirstOrDefault.Shift.TimeFrom.Add(TimeSpan.FromHours(-4))
            Dim shift4hoursafter = employeeshifts.FirstOrDefault.Shift.TimeFrom.Add(TimeSpan.FromHours(4))

            Dim a = log
            Dim Item4hoursbefore As DateTime = Nothing
            Dim Item4hoursafter As DateTime = Nothing
            Dim Outonly As DateTime = Nothing
            Dim EndOfTheDay As DateTime = Nothing




            ' CHECK LISTIN AND LISTOUT TO INITIALIZE

            If (listin.Count = 0 And listout.Count <> 0) Then
                Dim DateOut As Date = CDate(listout.LastOrDefault().DateOut)
                Outonly = DateOut.Date.Add(shift4hoursbefore)
                Item4hoursbefore = a.DateTime.Date.Add(shift4hoursbefore)
                Item4hoursafter = a.DateTime.Date.Add(shift4hoursafter)

            ElseIf listin.Count <> 0 Then
                Dim DateIn As Date = CDate(listin.FirstOrDefault().DateIn)
                EndOfTheDay = DateIn.AddDays(1).Date.Add(shift4hoursbefore)
                Item4hoursbefore = DateIn.Date.Add(shift4hoursbefore)
                Item4hoursafter = DateIn.Date.Add(shift4hoursafter)

            Else
                Item4hoursbefore = a.DateTime.Date.Add(shift4hoursbefore)
                Item4hoursafter = a.DateTime.Date.Add(shift4hoursafter)

            End If
            ' END CHECK LISTIN AND LISTOUT TO INITIALIZE

            ' CHECK EMPLOYEE NUMBER IF NOT THE SAME TO STORE TO LIST
            If listin.Count <> 0 And listout.Count <> 0 Then
                If CInt(a.EmployeeNo) <> listout.LastOrDefault().EmployeeID Then
                    itemlist.DateIn = listin.FirstOrDefault().DateIn
                    itemlist.DateOut = listout.LastOrDefault().DateOut
                    itemlist.TimeIn = listin.FirstOrDefault().TimeIn
                    itemlist.TimeOut = listout.LastOrDefault().TimeOut
                    If listin.FirstOrDefault().EmployeeID = listout.LastOrDefault().EmployeeID Then
                        itemlist.EmployeeID = listin.FirstOrDefault().EmployeeID
                    End If
                    list.Add(itemlist)

                    listin.Clear()
                    listout.Clear()


                    Item4hoursbefore = a.DateTime.Date.Add(shift4hoursbefore)
                    Item4hoursafter = a.DateTime.Date.Add(shift4hoursafter)
                End If
            ElseIf listin.Count <> 0 And listout.Count = 0 Then
                If CInt(a.EmployeeNo) <> listin.FirstOrDefault().EmployeeID Then
                    itemlist.DateIn = listin.FirstOrDefault().DateIn
                    itemlist.DateOut = Nothing
                    itemlist.TimeIn = listin.FirstOrDefault().TimeIn
                    itemlist.TimeOut = Nothing
                    itemlist.EmployeeID = listin.FirstOrDefault().EmployeeID
                    list.Add(itemlist)

                    listin.Clear()
                    listout.Clear()
                    '//if clear initialize

                    Item4hoursbefore = a.DateTime.Date.Add(shift4hoursbefore)
                    Item4hoursafter = a.DateTime.Date.Add(shift4hoursafter)
                End If
            ElseIf listin.Count = 0 And listout.Count <> 0 Then
                If CInt(a.EmployeeNo) <> listout.LastOrDefault().EmployeeID Then
                    itemlist.DateIn = Nothing
                    itemlist.DateOut = listout.LastOrDefault().DateOut
                    itemlist.TimeIn = Nothing
                    itemlist.TimeOut = listout.LastOrDefault().TimeOut
                    itemlist.EmployeeID = listout.LastOrDefault().EmployeeID
                    list.Add(itemlist)

                    listin.Clear()
                    listout.Clear()

                    '//if clear initialize
                    Item4hoursbefore = a.DateTime.Date.Add(shift4hoursbefore)
                    Item4hoursafter = a.DateTime.Date.Add(shift4hoursafter)
                End If
            Else


            End If
            ' END CHECK EMPLOYEE NUMBER IF NOT THE SAME TO STORE TO LIST

            '  CHECK LISTIN AND LISTOUT AND CURRENT DATA ITERATED TO BE COMPARE AT END OF THE DAY TO STORE LIST

            If listin.Count <> 0 And listout.Count <> 0 And EndOfTheDay <= a.DateTime Then
                itemlist.DateIn = listin.FirstOrDefault().DateIn
                itemlist.DateOut = listout.LastOrDefault().DateOut
                itemlist.TimeIn = listin.FirstOrDefault().TimeIn
                itemlist.TimeOut = listout.LastOrDefault().TimeOut

                If listin.FirstOrDefault().EmployeeID = listout.LastOrDefault().EmployeeID Then
                    itemlist.EmployeeID = listin.FirstOrDefault().EmployeeID
                End If

                list.Add(itemlist)

                listin.Clear()
                listout.Clear()
                '//if clear initialize

                Item4hoursbefore = a.DateTime.Date.Add(shift4hoursbefore)
                Item4hoursafter = a.DateTime.Date.Add(shift4hoursafter)

            ElseIf listin.Count <> 0 And listout.Count = 0 And EndOfTheDay <= a.DateTime Then
                itemlist.DateIn = listin.FirstOrDefault().DateIn
                itemlist.DateOut = Nothing
                itemlist.TimeIn = listin.FirstOrDefault().TimeIn
                itemlist.TimeOut = Nothing
                itemlist.EmployeeID = listin.FirstOrDefault().EmployeeID
                list.Add(itemlist)

                listin.Clear()
                listout.Clear()
                '//if clear initialize

                Item4hoursbefore = a.DateTime.Date.Add(shift4hoursbefore)
                Item4hoursafter = a.DateTime.Date.Add(shift4hoursafter)

            ElseIf listin.Count = 0 And listout.Count <> 0 And Outonly <= a.DateTime Then
                itemlist.DateIn = Nothing
                itemlist.DateOut = listout.LastOrDefault().DateOut
                itemlist.TimeIn = Nothing
                itemlist.TimeOut = listout.LastOrDefault().TimeOut
                itemlist.EmployeeID = listout.LastOrDefault().EmployeeID
                list.Add(itemlist)

                listin.Clear()
                listout.Clear()
                '//if clear initialize

                Item4hoursbefore = a.DateTime.Date.Add(shift4hoursbefore)
                Item4hoursafter = a.DateTime.Date.Add(shift4hoursafter)
            Else

            End If
            'END CHECK LISTIN AND LISTOUT AND CURRENT DATA ITERATED TO BE COMPARE AT END OF THE DAY TO STORE LIST

            'STORE IN AND sTORE OUT
            If a.DateTime >= Item4hoursbefore And a.DateTime <= Item4hoursafter Then
                itemin.TimeIn = a.DateTime.TimeOfDay
                itemin.DateIn = a.DateTime
                itemin.EmployeeID = CInt(a.EmployeeNo)
                listin.Add(itemin)
            Else
                itemout.TimeOut = a.DateTime.TimeOfDay
                itemout.DateOut = a.DateTime
                itemout.EmployeeID = CInt(a.EmployeeNo)
                listout.Add(itemout)
            End If




        Next
        ' IF END OF RANGE OF ITERATED ROW HAS LISTIN OR LISTOUT
        Dim itemlistexcess = New TimeLogInOut

        If listin.Count <> 0 And listout.Count = 0 Then
            itemlistexcess.DateIn = listin.FirstOrDefault().DateIn
            itemlistexcess.TimeIn = listin.FirstOrDefault().TimeIn
            itemlistexcess.DateOut = Nothing
            itemlistexcess.TimeOut = Nothing
            itemlistexcess.EmployeeID = listin.FirstOrDefault().EmployeeID
            list.Add(itemlistexcess)
            listin.Clear()
            listout.Clear()
        End If

        If listout.Count <> 0 And listin.Count = 0 Then
            itemlistexcess.DateOut = listout.LastOrDefault().DateOut
            itemlistexcess.TimeOut = listout.LastOrDefault().TimeOut
            itemlistexcess.DateIn = Nothing
            itemlistexcess.TimeIn = Nothing
            itemlistexcess.EmployeeID = listout.LastOrDefault().EmployeeID
            list.Add(itemlistexcess)
            listin.Clear()
            listout.Clear()

        End If

        If listout.Count <> 0 And listin.Count <> 0 Then
            itemlistexcess.DateOut = listout.LastOrDefault().DateOut
            itemlistexcess.TimeOut = listout.LastOrDefault().TimeOut
            itemlistexcess.DateIn = listin.FirstOrDefault().DateIn
            itemlistexcess.TimeIn = listin.FirstOrDefault().TimeIn
            If listin.FirstOrDefault().EmployeeID = listout.LastOrDefault().EmployeeID Then
                itemlistexcess.EmployeeID = listin.FirstOrDefault().EmployeeID
            End If
            list.Add(itemlistexcess)
            listin.Clear()
            listout.Clear()

        End If
        ' END IF END OF RANGE OF ITERATED ROW HAS LISTIN OR LISTOUT

        Return list
    End Function





End Class
