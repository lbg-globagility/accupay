Option Strict On

Imports AccuPay.Utilities

<TestFixture>
Public Class AccuMathTest

    <Test>
    Public Sub BenchmarkTruncateIssue()
        'on the issue discovered by benchmark,
        '151.20 (302.40 total, 10,080.00 basic pay)
        'used to be computed as 151.19 and 151.21
        'for employee and employer respectively
        'which should have been 151.20
        Assert.True(AccuMath.Truncate(151.2D, 2) = 151.2)

    End Sub

End Class