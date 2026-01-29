using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace Dupont_Price_Lists.Models
{
    public class ItemRecord
    {
        private readonly Dictionary<string, string> _fields = new();

        public void SetField(string key, string value)
        {
            if (!_fields.ContainsKey(key))
                _fields.Add(key, value);
            else
                _fields[key] = value;
        }

        public string GetField(string key)
        {
            return _fields.ContainsKey(key) ? _fields[key] : string.Empty;
        }

        public IReadOnlyDictionary<string, string> GetFields()
        {
            return _fields;
        }
    }
}
