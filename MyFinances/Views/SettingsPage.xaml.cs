using MyFinances.ViewModels;

namespace MyFinances.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SettingsPage : ContentPage
    {
        static readonly NLog.Logger _logger = NLog.LogManager.GetCurrentClassLogger();  
        readonly SettingsViewModel _viewModel;

        public SettingsPage()
        {
            InitializeComponent();
            _viewModel = new SettingsViewModel();
            BindingContext = _viewModel;
        }
    }
}