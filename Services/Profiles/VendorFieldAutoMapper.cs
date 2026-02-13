using System;
using System.Collections.Generic;
using System.Linq;

namespace Dupont_Price_Lists.Services.Profiles
{
    public static class VendorFieldAutoMapper
    {
        public static string? GuessField(IEnumerable<string> headers, params string[] candidates)
        {
            var list = headers.ToList();
            foreach (var c in candidates)
            {
                var hit = list.FirstOrDefault(h => h.Equals(c, StringComparison.OrdinalIgnoreCase));
                if (hit != null) return hit;
            }

            // also support contains matches (ex: "New List Price USD")
            foreach (var c in candidates)
            {
                var hit = list.FirstOrDefault(h => h.IndexOf(c, StringComparison.OrdinalIgnoreCase) >= 0);
                if (hit != null) return hit;
            }

            return null;
        }
    }
}
