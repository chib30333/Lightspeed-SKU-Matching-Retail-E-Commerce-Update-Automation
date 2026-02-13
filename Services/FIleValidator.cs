using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;

namespace Dupont_Price_Lists.Services
{
    public class FileValidator
    {
        public bool HasColumn(DataTable table, string columnName)
        {
            return table.Columns.Contains(columnName);
        }


        public void ValidateVendorFile(DataTable vendorTable)
        {
            string[] requiredColumns = { "Manufact. SKU", "Description", "MSRP" };

            foreach (string col in requiredColumns)
            {
                if (!HasColumn(vendorTable, col))
                    throw new Exception($"❌ Missing required column: {col}");
            }

            if (vendorTable.Rows.Count == 0)
                throw new Exception("❌ The vendor file is empty!");
        }

        public void ValidateDiscountFile(DataTable discountTable)
        {
            if (!HasColumn(discountTable, "Brand") || !HasColumn(discountTable, "Discount"))
                throw new Exception("❌ Discount file missing Brand or Discount column!");
        }
    }
}
