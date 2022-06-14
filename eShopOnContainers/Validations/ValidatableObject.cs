using CommunityToolkit.Mvvm.ComponentModel;
using eShopOnContainers.ViewModels.Base;
using System.Collections.Generic;
using System.Linq;

namespace eShopOnContainers.Validations
{
    public class ValidatableObject<T> : ObservableObject, IValidity
    {
        private readonly List<IValidationRule<T>> _validations;
		private IEnumerable<string> _errors;
        private T _value;
        private bool _isValid;

        public List<IValidationRule<T>> Validations => _validations;

		public IEnumerable<string> Errors
        {
            get => _errors;
            private set => SetProperty(ref _errors, value);
        }

        public T Value
        {
            get => _value;
            set => SetProperty(ref _value, value);
        }

        public bool IsValid
        {
            get => _isValid;
            private set => SetProperty(ref _isValid, value);
        }

        public ValidatableObject()
        {
            _isValid = true;
            _errors = Enumerable.Empty<string>();
            _validations = new List<IValidationRule<T>>();
        }

        public bool Validate()
        {
            var errors =
                _validations
                    ?.Where(v => !v.Check(Value))
                    ?.Select(v => v.ValidationMessage)
                    ?.ToArray()
                    ?? Enumerable.Empty<string>();

            Errors = errors;

            IsValid = !errors.Any();

            return this.IsValid;
        }
    }
}
