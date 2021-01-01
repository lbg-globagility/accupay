Option Strict On

Imports AccuPay.CrystalReports
Imports AccuPay.Core
Imports AccuPay.Core.Interfaces
Imports AccuPay.Core.Repositories
Imports AccuPay.Core.Services
Imports AccuPay.Infrastructure.Services.Encryption
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

            Dim form1 = MainServiceProvider.GetRequiredService(Of Form1)()
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
        services.AddScoped(Of PayslipDataService)
        services.AddScoped(Of SystemOwnerService)
        services.AddScoped(Of PayslipBuilder)
        services.AddScoped(Of IPolicyHelper, PolicyHelper)
        services.AddScoped(Of ListOfValueService)

        services.AddScoped(Of PaystubEmailRepository)
        services.AddScoped(Of Form1)

        services.AddTransient(Of IEncryption, AccuPayDesktopEncryption)
    End Sub

End Module
