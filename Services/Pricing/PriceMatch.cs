using System;
using System.Globalization;

namespace Dupont_Price_Lists.Services.Pricing
{
    public static class PriceMath
    {
        public static decimal ParseDecimal(string? s)
        {
            if (string.IsNullOrWhiteSpace(s)) return 0m;
            s = s.Trim().Replace(",", "");
            return decimal.TryParse(s, NumberStyles.Number, CultureInfo.InvariantCulture, out var v) ? v : 0m;
        }

        public static decimal ApplyDiscountChain(decimal msrp, string rule)
        {
            if (string.IsNullOrWhiteSpace(rule)) return msrp;

            var clean = rule.Replace("%", "").Replace(" ", "");
            var parts = clean.Split(new[] { '/', '\\', '-', ',' }, StringSplitOptions.RemoveEmptyEntries);

            decimal price = msrp;
            foreach (var p in parts)
            {
                if (decimal.TryParse(p, NumberStyles.Number, CultureInfo.InvariantCulture, out var pct))
                    price *= (1m - pct / 100m);
            }

            return decimal.Round(price, 2, MidpointRounding.AwayFromZero);
        }
    }
}
