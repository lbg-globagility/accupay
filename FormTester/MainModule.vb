Option Strict On

Imports AccuPay.Core.Interfaces
Imports AccuPay.CrystalReports
Imports AccuPay.Infrastructure.Data
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

        services.AddScoped(Of IOrganizationRepository, OrganizationRepository)
        services.AddScoped(Of IPayPeriodRepository, PayPeriodRepository)
        services.AddScoped(Of IPayslipDataService, PayslipDataService)
        services.AddScoped(Of ISystemOwnerService, SystemOwnerService)
        services.AddScoped(Of IPayslipBuilder, PayslipBuilder)
        services.AddScoped(Of IPolicyHelper, PolicyHelper)
        services.AddScoped(Of IListOfValueService, ListOfValueService)

        services.AddScoped(Of IPaystubEmailRepository, PaystubEmailRepository)
        services.AddScoped(Of Form1)

        services.AddTransient(Of IEncryption, AccuPayDesktopEncryption)
    End Sub

End Module
