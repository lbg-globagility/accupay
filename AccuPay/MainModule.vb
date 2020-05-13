Option Strict On

Imports AccuPay.Data
Imports AccuPay.Data.Repositories
Imports AccuPay.Data.Services
Imports AccuPay.Payslip
Imports Microsoft.EntityFrameworkCore
Imports Microsoft.Extensions.DependencyInjection

Module MainModule

    Public MainServiceProvider As ServiceProvider

    Public Sub Main()

        Application.EnableVisualStyles()
        Application.SetCompatibleTextRenderingDefault(False)

        Dim services = New ServiceCollection()

        ConfigureServices(services)

        MainServiceProvider = services.BuildServiceProvider()

        Using MainServiceProvider

            Dim form1 = MainServiceProvider.GetRequiredService(Of MetroLogin)()
            Application.Run(form1)

        End Using

    End Sub

    Private Sub ConfigureServices(services As ServiceCollection)

        services.AddDbContext(Of PayrollContext)(
            Sub(options As DbContextOptionsBuilder)
                options.UseMySql("server=localhost;user id=root;password=globagility;database=accupaydb_cinema2k;")
                options.EnableSensitiveDataLogging()
            End Sub)

        'services.AddScoped(Of OrganizationRepository)
        'services.AddScoped(Of PayPeriodRepository)
        'services.AddScoped(Of AddressRepository)
        'services.AddScoped(Of SystemOwnerService)
        'services.AddScoped(Of PayslipCreator)

        'services.AddScoped(Of PaystubEmailRepository)
        services.AddScoped(Of ListOfValueService)
        services.AddScoped(Of OrganizationRepository)
        services.AddScoped(Of UserRepository)
        services.AddTransient(Of MetroLogin)
    End Sub

End Module