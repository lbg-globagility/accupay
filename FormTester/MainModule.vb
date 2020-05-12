Imports AccuPay.Data
Imports AccuPay.Data.Repositories
Imports AccuPay.Data.Services
Imports AccuPay.Payslip
Imports Microsoft.EntityFrameworkCore
Imports Microsoft.Extensions.DependencyInjection
Imports Microsoft.Extensions.Options

Module MainModule

    Public Sub Main()

        Application.EnableVisualStyles()
        Application.SetCompatibleTextRenderingDefault(False)

        Dim services = New ServiceCollection()

        ConfigureServices(services)

        Using serviceProvider = services.BuildServiceProvider()

            Dim form1 = serviceProvider.GetRequiredService(Of Form1)()
            Application.Run(form1)

        End Using

    End Sub

    Private Sub ConfigureServices(services As ServiceCollection)

        services.AddDbContext(Of PayrollContext)(
            Sub(options As DbContextOptionsBuilder)
                options.UseMySql("server=localhost;user id=root;password=globagility;database=accupaydb_cinema2k;")
                options.EnableSensitiveDataLogging()
            End Sub)

        services.AddScoped(Of OrganizationRepository)
        services.AddScoped(Of PayPeriodRepository)
        services.AddScoped(Of AddressRepository)
        services.AddScoped(Of SystemOwnerService)
        services.AddScoped(Of PayslipCreator)

        services.AddScoped(Of PaystubEmailRepository)
        services.AddScoped(Of Form1)
    End Sub

End Module