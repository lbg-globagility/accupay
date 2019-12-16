Option Strict On

Imports AccuPay.Domain.TimeLogs

<TestFixture>
Public Class RegexTimelogParserTest

    <Test>
    Public Sub TestRegex()
        Dim regex = "^(?<employeeno>[A-Za-z0-9\-]+) (?<year>\d{4})(?<month>\d{2})(?<day>\d{2})(?<hour>\d{2})(?<minute>\d{2})(?<entry>[AZ]*)$"
        Dim sample = "AAX-120 201912311231A"

        Dim parser = New RegexTimelogParser(regex, "A", "Z")

        Dim result = parser.Parse(sample)

        Assert.That(result.Date, [Is].EqualTo(Date.Parse("2019-12-31")))
        Assert.That(result.Time, [Is].EqualTo(TimeSpan.Parse("12:31")))
        Assert.That(result.EmployeeNo, [Is].EqualTo("AAX-120"))
        Assert.That(result.Status, [Is].EqualTo(EntryStatus.In))
    End Sub

End Class
