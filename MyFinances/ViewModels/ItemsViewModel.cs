using MyFinances.Core.Dtos;
using MyFinances.Core.Response;
using MyFinances.Models.Domains;
using MyFinances.Views;
using System.Collections.ObjectModel;

namespace MyFinances.ViewModels
{
    public class ItemsViewModel : BaseViewModel
    {
        static readonly NLog.Logger _logger = NLog.LogManager.GetCurrentClassLogger();
        public ObservableCollection<OperationDto> Operations { get; }
        public Command LoadItemsCommand { get; }
        public Command AddItemCommand { get; }
        public Command DeleteItemCommand { get; }
        public Command<OperationDto> ItemTapped { get; }
        public Command PreviousPageCommand { get; }
        public Command NextPageCommand { get; }

        int _pageNumber = 1;
        int _totalOperations = 0;

        public int PageNumber
        {
            get => _pageNumber;
            private set
            {
                if (_pageNumber != value)
                {
                    _pageNumber = value;
                    OnPropertyChanged(nameof(PageNumber));
                    RefreshPaginationButtons();
                }
            }
        }

        public int TotalOperations
        {
            get => _totalOperations;
            private set
            {
                if (_totalOperations != value)
                {
                    _totalOperations = value;
                    OnPropertyChanged(nameof(TotalOperations));
                    RefreshPaginationButtons();
                }
            }
        }

        public void ResetPagination()
        {
            PageNumber = 1;
            RefreshPaginationButtons();
        }

        public bool CanGoPrevious => Settings.UsePagination && PageNumber > 1;
        public bool CanGoNext  => Settings.UsePagination && TotalOperations > PageNumber * Settings.OperationsPerPage;

        public void RefreshPaginationButtons()
        {
            OnPropertyChanged(nameof(CanGoPrevious));
            OnPropertyChanged(nameof(CanGoNext));
            ((Command)PreviousPageCommand).ChangeCanExecute();
            ((Command)NextPageCommand).ChangeCanExecute();
        }

        public ItemsViewModel()
        {
            Title = "Operacje";
            Operations = [];
            LoadItemsCommand = new Command(async () => await ExecuteLoadItemsCommand());

            ItemTapped = new Command<OperationDto>(OnItemSelected);

            AddItemCommand = new Command(OnAddItem);
            DeleteItemCommand = new Command<OperationDto>(async (x) => 
                await OnDeleteItem(x));
            PreviousPageCommand = new Command(
                async () => await OnPreviousPage(),
                () => CanGoPrevious);

            NextPageCommand = new Command(
                async () => await OnNextPage(),
                () => CanGoNext);

            MessagingCenter.Subscribe<object>(this, "SettingsChanged", async (sender) =>
            {
                ResetPagination();
                await ExecuteLoadItemsCommand();
            });
        }

        private async Task OnNextPage()
        {
            PageNumber++;
            await ExecuteLoadItemsCommand();
        }

        private async Task OnPreviousPage()
        {
            PageNumber--;
            await ExecuteLoadItemsCommand();
        }

        private async Task OnDeleteItem(OperationDto operation)
        {
            if (operation == null)
                return;

            var dialog = await Shell.Current.DisplayAlert("Usuwanie!", $"Czy na pewno chcesz usunąć operacje: {operation.Name}?", "Tak", "Nie");

            if (!dialog)
                return;

            var response = await BaseViewModel.OperationService.DeleteAsync(operation.Id);

            if (!response.IsSuccess)
                await ShowErrorAlert(string.Join("", response.Errors.Select(x => x.Message)), System.Text.Json.JsonSerializer.Serialize(response));

            await ExecuteLoadItemsCommand();
        }

        public async Task ExecuteLoadItemsCommand()
        {
            IsBusy = true;

            try
            {
                DataResponse<IEnumerable<OperationDto>> response;

                if (Settings.UsePagination)
                {
                    var pagedResponse = await BaseViewModel.OperationService.GetPagedAsync(Settings.OperationsPerPage, _pageNumber);

                    if (pagedResponse.IsSuccess)
                    {
                        response = new DataResponse<IEnumerable<OperationDto>>
                        {
                            Data = pagedResponse.Data.Items,
                            Errors = pagedResponse.Errors
                        };
                        TotalOperations = pagedResponse.Data.TotalCount;
                    }
                    else
                    {
                        _logger.Error($"GetPagedAsync failed: {pagedResponse}");
                        response = new DataResponse<IEnumerable<OperationDto>>
                        {
                            Errors = pagedResponse.Errors
                        };
                    }
                }
                else
                {
                    response = await BaseViewModel.OperationService.GetAsync();
                    TotalOperations = response.Data?.Count() ?? 0;
                }

                if (!response.IsSuccess)
                    await ShowErrorAlert(string.Join(". ", response.Errors.Select(x => x.Message)), System.Text.Json.JsonSerializer.Serialize(response));

                Operations.Clear();

                if (response.Data != null)
                {
                    foreach (var item in response.Data)
                        Operations.Add(item);
                }
                else 
                    _logger.Warn($"Response.Data is null. IsSuccess: {response.IsSuccess}");

            }
            catch (Exception exception)
            {
                await ShowErrorAlert(exception.Message, $"StackTrace: {exception.StackTrace}");
            }
            finally
            {
                IsBusy = false;
            }
        }

        public void OnAppearing() => IsBusy = true;

        private async void OnAddItem(object obj) => await Shell.Current.GoToAsync(nameof(NewItemPage));

        async void OnItemSelected(OperationDto operation)
        {
            if (operation == null)
                return;

            await Shell.Current.GoToAsync($"{nameof(ItemDetailPage)}?{nameof(ItemDetailViewModel.ItemId)}={operation.Id}");
        }
    }
}