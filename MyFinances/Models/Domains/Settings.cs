namespace MyFinances.Models.Domains
{
    public class Settings
    {
        static readonly NLog.Logger _logger = NLog.LogManager.GetCurrentClassLogger();
        static byte _operationsPerPage;
        static string _dataBaseType; 
        static string _apiAddress;
        static bool _usePagination;
        public static byte OperationsPerPage
        {
            get => _operationsPerPage;
            set => _operationsPerPage = value;
        }

        public static bool UsePagination
        {
            get => _usePagination;
            set => _usePagination = value;
        }

        public static string DataBaseType
        {
            get => _dataBaseType;
            set => _dataBaseType = value;
        }

        public static string ApiAddress
        {
            get => _apiAddress;
            set => _apiAddress = value;
        }
    }
}
