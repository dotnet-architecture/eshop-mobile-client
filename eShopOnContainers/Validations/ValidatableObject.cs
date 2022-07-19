using CommunityToolkit.Mvvm.ComponentModel;

namespace eShopOnContainers.Validations;

public class ValidatableObject<T> : ObservableObject, IValidity
{
    private readonly List<IValidationRule<T>> _validations;

    private IEnumerable<string> _errors;
    private bool _isValid;
    private T _value;

    public List<IValidationRule<T>> Validations => _validations;

    public IEnumerable<string> Errors
    {
        get => _errors;
        private set => SetProperty(ref _errors, value);
    }

    public bool IsValid
    {
        get => _isValid;
        private set => SetProperty(ref _isValid, value);
    }

    public T Value
    {
        get => _value;
        set => SetProperty(ref _value, value);
    }

    public ValidatableObject()
    {
        _isValid = true;
        _errors = Enumerable.Empty<string>();
        _validations = new List<IValidationRule<T>>();
    }

    public bool Validate()
    {
        Errors = _validations
            ?.Where(v => !v.Check(Value))
            ?.Select(v => v.ValidationMessage)
            ?.ToArray()
            ?? Enumerable.Empty<string>();

        IsValid = !Errors.Any();

        return IsValid;
    }
}