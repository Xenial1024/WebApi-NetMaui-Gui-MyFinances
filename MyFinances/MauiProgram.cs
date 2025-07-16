using Microsoft.Extensions.Logging;
using MyFinances.Services;
using MyFinances.Models.Domains;

namespace MyFinances
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });
            if (Settings.DataBaseType == "SQL Server")
                builder.Services.AddSingleton<IOperationService, OperationService>();
            else
                builder.Services.AddSingleton<IOperationService, OperationSqliteService>();
#if DEBUG
            builder.Logging.AddDebug();
#endif
            return builder.Build();
        }
    }
}
