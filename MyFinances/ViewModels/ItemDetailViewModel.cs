using MyFinances.Core.Dtos;
using MyFinances.Models.Domains;

namespace MyFinances.ViewModels
{
    [QueryProperty(nameof(ItemId), nameof(ItemId))]
    public class ItemDetailViewModel : BaseViewModel
    {
        private OperationDto _operation;

        private string itemId;

        public ItemDetailViewModel() => Title = "Podgląd operacji";

        public string CategoryName => Categories.Items.FirstOrDefault(x => x.Id == Operation?.CategoryId)?.Name ?? "Nieznana";
        public OperationDto Operation
        {
            get => _operation;
            set
            {
                SetProperty(ref _operation, value);
                OnPropertyChanged(nameof(CategoryName));
            }

        }

        public string ItemId
        {
            get => itemId;
            set
            {
                itemId = value;
                LoadItemId(int.Parse(value));
            }
        }

        public async void LoadItemId(int itemId)
        {
            var response = await BaseViewModel.OperationService.GetAsync(itemId);

            if (!response.IsSuccess)
                await ShowErrorAlert(string.Join("", response.Errors.Select(x => x.Message)), System.Text.Json.JsonSerializer.Serialize(response));

            Operation = response.Data;
        }
    }
}
