using System;
using System.Collections.Generic;

namespace eShopOnContainers.Core.Extensions
{
    public static class DictionaryExtensions
    {
        public static (bool ContainsKeyAndValue, bool Value) GetValueAsBool(this IDictionary<string, string> dictionary, string key)
        {
            return dictionary.ContainsKey (key) && bool.TryParse (dictionary[key], out var parsed) ? (true, parsed) : (false, default);
        }

        public static (bool ContainsKeyAndValue, int Value) GetValueAsInt (this IDictionary<string, string> dictionary, string key)
        {
            return dictionary.ContainsKey (key) && int.TryParse (dictionary[key], out var parsed) ? (true, parsed) : (false, default);
        }
    }
}
