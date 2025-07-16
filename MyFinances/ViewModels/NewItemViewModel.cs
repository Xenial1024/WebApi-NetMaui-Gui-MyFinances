using MyFinances.Core.Dtos;
using MyFinances.Models;
using System.Globalization;

namespace MyFinances.ViewModels
{
    public class NewItemViewModel : BaseViewModel
    {
        private string _name;
        private string _description;
        private decimal _value;
        private DateTime _createdDate;
        private LookupItem _selectedCategory;
        private IEnumerable<LookupItem> _categories =
        [
            new () { Id = 1, Name = "Ogólna" },
            new () { Id = 2, Name = "Media i usługi" },
            new () { Id = 3, Name = "Nieruchomości" },
            new () { Id = 4, Name = "Wyżywienie" },
            new () { Id = 5, Name = "Rozrywka" },
            new () { Id = 6, Name = "Ubrania" },
            new () { Id = 7, Name = "Transport" },
            new () { Id = 8, Name = "Edukacja" },
            new () { Id = 9, Name = "Podatki" },
            new () { Id = 10, Name = "Zwierzęta" },
            new () { Id = 11, Name = "Darowizny" },
            new () { Id = 12, Name = "Zdrowie" },
            new () { Id = 13, Name = "Inwestycje" },
            new () { Id = 14, Name = "Kary" }
        ];

        public NewItemViewModel()
        {
            CreatedDate = DateTime.Now;
            SaveCommand = new Command(OnSave, ValidateSave);
            CancelCommand = new Command(OnCancel);
            this.PropertyChanged +=
                (_, __) => SaveCommand.ChangeCanExecute();
        }

        private bool ValidateSave()
        {
            return !String.IsNullOrWhiteSpace(Name)
                && !String.IsNullOrWhiteSpace(Description)
                && SelectedCategory != null;
        }

        public string Name
        {
            get => _name;
            set => SetProperty(ref _name, value);
        }

        public string Description
        {
            get => _description;
            set => SetProperty(ref _description, value);
        }

        public decimal Value
        {
            get => _value;
            set => SetProperty(ref _value, value);
        }

        public DateTime CreatedDate
        {
            get => _createdDate;
            set => SetProperty(ref _createdDate, value);
        }

        public LookupItem SelectedCategory
        {
            get => _selectedCategory;
            set => SetProperty(ref _selectedCategory, value);
        }

        public IEnumerable<LookupItem> Categories
        {
            get => _categories;
            set => SetProperty(ref _categories, value);
        }

        public Command SaveCommand { get; }
        public Command CancelCommand { get; }

        private async void OnCancel() =>  await Shell.Current.GoToAsync("..");

        private async void OnSave()
        {
            if (!Validate(out string error))
            {
                await Application.Current.MainPage.DisplayAlert("Błąd walidacji", error, "OK");
                return;
            }
            var operation = new OperationDto()
            {
                Name = Name,
                Description = Description,
                CategoryId = SelectedCategory.Id,
                Date = DateTime.Now,
                Value = Value
            };

            var response = await BaseViewModel.OperationService.AddAsync(operation);

            if (!response.IsSuccess)
                await ShowErrorAlert(string.Join(". ", response.Errors.Select(x => x.Message)), System.Text.Json.JsonSerializer.Serialize(response));

            // This will pop the current page off the navigation stack
            await Shell.Current.GoToAsync("..");
        }

        public bool Validate(out string errorMessage)
        {
            if (Name.Length > 50)
            {
                errorMessage = "Nazwa nie może przekraczać 50 znaków.";
                return false;
            }

            if (Description?.Length > 500)
            {
                errorMessage = "Opis nie może przekraczać 500 znaków.";
                return false;
            }

            if (Value < 0.01m || Value > 99999999.99m)
            {
                errorMessage = "Wartość musi być w zakresie 0,01 – 99999999,99.";
                return false;
            }

            errorMessage = null;
            return true;
        }
    }
}
