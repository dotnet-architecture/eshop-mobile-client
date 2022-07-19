namespace eShopOnContainers;

public static class DictionaryExtensions
{
    public static bool ValueAsBool(this IDictionary<string, object> dictionary, string key, bool defaultValue = false)
    {
        if ( dictionary.ContainsKey(key) && dictionary[key] is bool dictValue)
        {
            return dictValue;
        }

        return defaultValue;
    }

    public static int ValueAsInt(this IDictionary<string, object> dictionary, string key, int defaultValue = 0)
    {
        if (dictionary.ContainsKey(key) && dictionary[key] is int intValue)
        {
            return intValue;
        }

        return defaultValue;
    }
}
