using System;
using Dupont_Price_Lists.Services.Pricing;

namespace Dupont_Price_Lists.Services.Discounts
{
    public static class DiscountEngine
    {
        public static decimal Apply(decimal msrp, string? expr)
        {
            if (string.IsNullOrWhiteSpace(expr)) return msrp;

            var clean = expr.Trim().ToUpperInvariant();
            if (clean is "MSRP" or "M.A.P" or "MAP" or "NET" or "GIVEN" or "PROVIDED" or "UNIT PRICE")
                return msrp;

            return PriceMath.ApplyDiscountChain(msrp, expr);
        }
    }
}
