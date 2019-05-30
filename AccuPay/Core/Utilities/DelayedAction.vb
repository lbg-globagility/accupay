Option Strict On

Imports System.Threading.Tasks

Namespace Global.AccuPay.Utils

    ''' <summary>
    '''
    ''' </summary>
    ''' <typeparam name="T">The return type of the passed function.</typeparam>
    Public Class DelayedAction(Of T)

        Private _timeoutTimer As Windows.Forms.Timer

        Private _actionAfterTimeOut As Func(Of Task(Of T))

        Private _timeOutDuration As Integer

        ''' <summary>
        ''' Delays a call to a function until it time outs. If the delay is 3 seconds, the passed function will only be activated after 3 seconds of idle time.
        ''' </summary>
        ''' <param name="timeOutDuration">Delay in milliseconds before it timeouts and activate the passed function.</param>
        Sub New(Optional timeOutDuration As Integer = 300)

            _timeOutDuration = timeOutDuration

        End Sub

        ''' <summary>
        ''' Receives the function to be activated after timeout
        ''' </summary>
        ''' <param name="actionAfterTimeOut">The function to be activated after timeout.</param>
        Public Sub ProcessAsync(actionAfterTimeOut As Func(Of Task(Of T)))

            _actionAfterTimeOut = actionAfterTimeOut

            If _timeoutTimer Is Nothing Then

                _timeoutTimer = New Windows.Forms.Timer
                _timeoutTimer.Interval = _timeOutDuration

                AddHandler _timeoutTimer.Tick, AddressOf HandleTypingTimerTimeout
            End If

            _timeoutTimer.Stop()
            _timeoutTimer.Start()

        End Sub

        Private Async Sub HandleTypingTimerTimeout(sender As Object, e As EventArgs)

            Dim timer = CType(sender, Windows.Forms.Timer)

            If timer Is Nothing Then Return

            Await _actionAfterTimeOut()

            timer.Stop()

        End Sub

    End Class

End Namespace