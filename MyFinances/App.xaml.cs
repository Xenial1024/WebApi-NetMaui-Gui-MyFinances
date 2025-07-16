using MyFinances.Models.Domains;
using MyFinances.Services;

namespace MyFinances
{
    public partial class App : Application
    {
        public static string BackendUrl = Settings.ApiAddress;
        public App() =>  InitializeComponent();

        protected override Window CreateWindow(IActivationState? activationState) => new(new AppShell());

        public static void ConfigureServices()
        {
            if (Settings.DataBaseType == "SQL Server")
                DependencyService.Register<OperationService>();
            else
                DependencyService.Register<OperationSqliteService>();
        }
    }
}