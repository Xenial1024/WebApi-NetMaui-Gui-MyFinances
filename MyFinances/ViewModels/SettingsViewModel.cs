using MyFinances.Models.Domains;
using MyFinances.Models.Wrappers;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace MyFinances.ViewModels
{
    public class SettingsViewModel : INotifyPropertyChanged
    {
        static readonly NLog.Logger _logger = NLog.LogManager.GetCurrentClassLogger();
        string _apiAddress;
        string _dataBaseType;
        bool _usePagination;
        string _operationsPerPageText; 
        public event PropertyChangedEventHandler PropertyChanged;


        public string ApiAddress
        {
            get => _apiAddress;
            set { _apiAddress = value; OnPropertyChanged(); }
        }

        public string DataBaseType
        {
            get => _dataBaseType;
            set { _dataBaseType = value; OnPropertyChanged(); }
        }

        public bool UsePagination
        {
            get => _usePagination;
            set { _usePagination = value; OnPropertyChanged(); }
        }

        public string OperationsPerPageText
        {
            get => _operationsPerPageText;
            set { _operationsPerPageText = value; OnPropertyChanged(); }
        }


        public Command SaveSettingsCommand { get; }
        public Command CancelSettingsCommand { get; }

        public SettingsViewModel()
        {
            ApiAddress = Settings.ApiAddress;
            DataBaseType = Settings.DataBaseType;
            UsePagination = Settings.UsePagination;
            OperationsPerPageText = Settings.OperationsPerPage.ToString();

            SaveSettingsCommand = new Command(async () => await SaveSettingsAsync());
            CancelSettingsCommand = new Command(async () => await Shell.Current.GoToAsync("ItemsPage"));
        }
        public void SetDatabaseType(string type)
        {
            try
            {
                DataBaseType = type;
            }
            catch (Exception ex)
            {
                _logger.Error($"Błąd podczas ustawiania typu bazy danych: {ex.Message}");
                throw;
            }
        }

        private async Task SaveSettingsAsync()
        {
            try
            {
                byte.TryParse(OperationsPerPageText, out byte operations);
                Settings.OperationsPerPage = operations;

                _logger.Debug($"Zapis ustawień: Settings.ApiAddress={Settings.ApiAddress}, ApiAddress={ApiAddress}, Settings.DataBaseType={Settings.DataBaseType}, DataBaseType={DataBaseType}");

                bool requiredRestart = Settings.ApiAddress != ApiAddress ||
                                       Settings.DataBaseType != DataBaseType;

                Settings.ApiAddress = ApiAddress;
                Settings.DataBaseType = DataBaseType;
                Settings.UsePagination = UsePagination;

                SettingsWrapper.SaveSettings();
                
                MessagingCenter.Send<object>(this, "SettingsChanged");

                if (requiredRestart)
                {
                    bool answer = await Shell.Current.DisplayAlert(
                        "Wymagany restart",
                        "Ustawienia zostały zapisane. Aby połączyć się z innym typem bazy danych lub tym samym, ale o innym adresie, potrzebne jest ponowne uruchomienie aplikacji. Czy chcesz teraz ją zamknąć?",
                        "Tak", "Nie");

                    if (answer)
                        Process.GetCurrentProcess().Kill();
                    
                    await Shell.Current.GoToAsync("ItemsPage");
                    Application.Current.MainPage = new AppShell(); //zaznaczenie opcji "Operacje" w menu na dole ekranu
                }
                else
                {
                    await Shell.Current.DisplayAlert("Sukces", "Ustawienia zostały zapisane", "OK");
                    await Shell.Current.GoToAsync("ItemsPage");
                    Application.Current.MainPage = new AppShell();
                }
            }
            catch (Exception ex)
            {
                _logger.Error($"Błąd podczas zmiany ustawień: {ex.Message}");
                await Shell.Current.DisplayAlert("Błąd", "Wystąpił błąd podczas zapisywania ustawień. Spróbuj ponownie.", "OK");
            }
        }

        void OnPropertyChanged([CallerMemberName] string name = null) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}
