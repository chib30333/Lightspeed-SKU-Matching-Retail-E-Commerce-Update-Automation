using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dupont_Price_Lists.Services
{
    public static class PricingService
    {
        public static decimal ApplyStackedDiscount(decimal listPrice, string formula)
        {
            if (string.IsNullOrWhiteSpace(formula)) return listPrice;
            var parts = formula.Split('/', StringSplitOptions.RemoveEmptyEntries);
            decimal price = listPrice;
            foreach (var p in parts)
            {
                if (decimal.TryParse(p, out var pct))
                {
                    var factor = pct / 100m;
                    price -= price * factor;
                }
            }
            return Math.Round(price, 2);
        }

        public static bool IsPart(string description)
        {
            if (string.IsNullOrWhiteSpace(description)) return false;
            var d = description.ToLowerInvariant();
            return d.Contains("part") || d.Contains("spare") || d.Contains("repair");
        }

        public static decimal ApplyRetailRules(decimal basePrice, string type)
        {
            if (type == "Part")
                return basePrice * 0.95m;
            else
                return basePrice;
        }

        public static decimal ApplyEcomRules(decimal basePrice, string type)
        {
            decimal shippingBuffer = 5.00m;

            if (type == "Part")
                return basePrice + shippingBuffer;
            else
                return basePrice * 1.02m + shippingBuffer;
        }
    }
}
