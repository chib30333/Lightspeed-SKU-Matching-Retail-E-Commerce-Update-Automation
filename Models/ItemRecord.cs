using System;
using System.Collections.Generic;

namespace Dupont_Price_Lists.Models
{
    public class ItemRecord
    {
        private readonly Dictionary<string, string> _fields = new(StringComparer.OrdinalIgnoreCase);

        public void SetField(string key, string value)
        {
            if (string.IsNullOrWhiteSpace(key)) return;
            _fields[key.Trim()] = value ?? "";
        }

        public string GetField(string key)
        {
            if (string.IsNullOrWhiteSpace(key)) return "";
            return _fields.TryGetValue(key.Trim(), out var v) ? (v ?? "") : "";
        }

        public IReadOnlyDictionary<string, string> GetFields() => _fields;

        public bool HasField(string key) => _fields.ContainsKey(key);
    }
}
