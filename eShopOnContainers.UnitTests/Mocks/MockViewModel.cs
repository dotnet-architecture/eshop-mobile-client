using eShopOnContainers.Validations;
using eShopOnContainers.ViewModels.Base;

namespace eShopOnContainers.UnitTests;

public class MockViewModel : ViewModelBase
{
    public ValidatableObject<string> Forename { get; } = new();

    public ValidatableObject<string> Surname { get; } = new();

    public MockViewModel(IDialogService dialogService, INavigationService navigationService, ISettingsService settingsService)
        : base(dialogService, navigationService, settingsService)
    {
        Forename = new ValidatableObject<string>();
        Surname = new ValidatableObject<string>();

        Forename.Validations.Add(new IsNotNullOrEmptyRule<string> { ValidationMessage = "Forename is required." });
        Surname.Validations.Add(new IsNotNullOrEmptyRule<string> { ValidationMessage = "Surname name is required." });
    }

    public bool Validate()
    {
        bool isValidForename = Forename.Validate();
        bool isValidSurname = Surname.Validate();
        return isValidForename && isValidSurname;
    }
}