using MyFinances.Models.Domains;

namespace MyFinances.Models.Wrappers
{
    public class SettingsWrapper
    {
        public static void SaveSettings()
        {
            Preferences.Set("OperationsPerPage", Settings.OperationsPerPage);
            Preferences.Set("UsePagination", Settings.UsePagination);
            Preferences.Set("DataBaseType", Settings.DataBaseType);
            Preferences.Set("ApiAddress", Settings.ApiAddress);
        }

        public static void LoadSettings()
        {
            Settings.OperationsPerPage = (byte)Preferences.Get("OperationsPerPage", 10);
            Settings.UsePagination = Preferences.Get("UsePagination", true);
            Settings.DataBaseType = Preferences.Get("DataBaseType", "SQL Server"); //połączenie z SQL Server nastąpi przez Web API
            Settings.ApiAddress = Preferences.Get("ApiAddress", "http://localhost:81/api/");
        }
    }
}
