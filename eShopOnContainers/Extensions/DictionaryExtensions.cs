namespace eShopOnContainers.Extensions
{
    public static class DictionaryExtensions
    {
        public static bool ValueAsBool(this IDictionary<string, object> dictionary, string key, out bool value)
        {
            if ( dictionary.ContainsKey(key) && dictionary[key] is bool dictValue)
            {
                value = dictValue;
                return true;
            }

            value = default(bool);
            return false;
        }

        public static bool ValueAsInt(this IDictionary<string, object> dictionary, string key, out int value)
        {
            if (dictionary.ContainsKey(key) && dictionary[key] is int intValue)
            {
                value = intValue;
                return true;
            }

            value = default(int);
            return false;
        }
    }
}