using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dupont_Price_Lists.Services
{
    internal static class PriceMath
    {
        public static decimal ParseDecimal(string? s)
        {
            if (string.IsNullOrWhiteSpace(s)) return 0m;
            s = s.Trim().Replace(",", "");
            return decimal.TryParse(s, NumberStyles.Number, CultureInfo.InvariantCulture, out var v) ? v : 0m;
        }

        // Supports "50/10/5" and tolerates spaces and "%"
        public static decimal ApplyDiscountChain(decimal msrp, string rule)
        {
            if (string.IsNullOrWhiteSpace(rule)) return msrp;
            switch(rule)
            {
                case "MSRP":
                    return msrp;
                    break;
                case "M.A.P":
                    return msrp;
                    break;
                case "Unit Price":
                    return msrp;
                    break;
                case "Net":
                    return msrp;
                    break;
                case "Given":
                    return msrp;
                    break;
                case "Provided":
                    return msrp;
                    break;
            }
            var clean = rule.Replace("%", "").Replace(" ", "");
            var parts = clean.Split(new[] { '/', '\\', '-', ',' }, StringSplitOptions.RemoveEmptyEntries);

            decimal price = msrp;
            foreach (var p in parts)
                if (decimal.TryParse(p, NumberStyles.Number, CultureInfo.InvariantCulture, out var pct))
                    price *= (1m - pct / 100m);

            return decimal.Round(price, 2, MidpointRounding.AwayFromZero);
        }
    }
}
