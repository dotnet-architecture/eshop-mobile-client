using eShopOnContainers.Validations;
using eShopOnContainers.ViewModels.Base;

namespace eShopOnContainers.UnitTests;

public class MockViewModel : ViewModelBase
{
    private readonly ValidatableObject<string> _forename = new();
    private readonly ValidatableObject<string> _surname = new();

    public ValidatableObject<string> Forename => _forename;

    public ValidatableObject<string> Surname => _surname;

    public MockViewModel(IDialogService dialogService, INavigationService navigationService, ISettingsService settingsService)
        : base(dialogService, navigationService, settingsService)
    {
        _forename = new ValidatableObject<string>();
        _surname = new ValidatableObject<string>();

        _forename.Validations.Add(new IsNotNullOrEmptyRule<string> { ValidationMessage = "Forename is required." });
        _surname.Validations.Add(new IsNotNullOrEmptyRule<string> { ValidationMessage = "Surname name is required." });
    }

    public bool Validate()
    {
        bool isValidForename = _forename.Validate();
        bool isValidSurname = _surname.Validate();
        return isValidForename && isValidSurname;
    }
}