using MyFinances.Services;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace MyFinances.ViewModels
{
    public class BaseViewModel : INotifyPropertyChanged
    {
       
        static readonly NLog.Logger _logger = NLog.LogManager.GetCurrentClassLogger();
        string _title = string.Empty;
        bool _isBusy = false;

        public static IOperationService OperationService =>
    Microsoft.Maui.Controls.Application.Current.Handler.MauiContext.Services.GetService<IOperationService>();

        public bool IsBusy
        {
            get { return _isBusy; }
            set { SetProperty(ref _isBusy, value); }
        }

        public string Title
        {
            get { return _title; }
            set { SetProperty(ref _title, value); }
        }

        protected bool SetProperty<T>(ref T backingStore, T value,
            [CallerMemberName] string propertyName = "",
            Action onChanged = null)
        {
            if (EqualityComparer<T>.Default.Equals(backingStore, value))
                return false;

            backingStore = value;
            onChanged?.Invoke();
            OnPropertyChanged(propertyName);
            return true;
        }

        public static async Task ShowErrorAlert(string message, string logDetails)
        {
            await Shell.Current.DisplayAlert("Wystąpił Błąd!", message, "Ok");
            _logger.Error($"Error: {message}. Details: {logDetails}");
        }

        #region INotifyPropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            var changed = PropertyChanged;
            if (changed == null)
                return;

            changed.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion
    }
}
