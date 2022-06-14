namespace eShopOnContainers.Validations
{
    public class IsNotNullOrEmptyRule<T> : IValidationRule<T>
    {
        public string ValidationMessage { get; set; }

        public bool Check(T value)
        {
            if (value is not string str)
            {
                return false;
            }

            return !string.IsNullOrWhiteSpace(str);
        }
    }
}
