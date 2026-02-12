using DocumentFormat.OpenXml.InkML;
using DocumentFormat.OpenXml.Math;
using DocumentFormat.OpenXml.Wordprocessing;
using Dupont_Price_Lists.Forms;
using Dupont_Price_Lists.Models;
using Dupont_Price_Lists.Services;
using System.Data;

namespace Dupont_Price_Lists
{
    public partial class Dupont_Price_List : Form
    {
        public FieldMapping SelectedMapping { get; private set; }
        private Dictionary<string, List<string>> allPaths = new Dictionary<string, List<string>>();

        private string vendorPath, lightspeedPath, categoryPath, masterDiscountPath, onlinePath;
        private string? brand, vendor;
        private List<string> vendorHeaders, lightspeedHeaders, masterDiscountHeaders, onlineHeaders;
        private List<ItemRecord> vendorRows, lightspeedRows, masterDiscountRows, onlineRows;

        public Dupont_Price_List()
        {
            InitializeComponent();

            ButtonRetailUpdate.Visible = false;
            ButtonOnlineUpdate.Visible = false;
            DataGridViewRecord.Visible = false;

            ComboBoxNewBrand.Enabled = false;
            ButtonReadNewPriceList.Enabled = false;
            ButtonReadMasterPriceList.Enabled = false;
        }

        private void ButtonCurrentItems_Click(object sender, EventArgs e)
        {
            var paths = FileManagerService.OpenFile();
            if (paths == null) return;

            TextBoxCurrentItems.Text = paths.First();
            allPaths["currentItems"] = paths;
        }

        private void ButtonOnlineItems_Click(object sender, EventArgs e)
        {
            var paths = FileManagerService.OpenFile();
            if (paths == null) return;

            TextBoxOnlineItems.Text = paths.First();
            allPaths["onlineItems"] = paths;
        }

        private void ButtonNewPriceList_Click(object sender, EventArgs e)
        {
            var paths = FileManagerService.OpenFile();
            if (paths == null) return;

            TextBoxNewPriceList.Text = paths.First();
            allPaths["newPriceList"] = paths;
        }

        private void ButtonCategoryList_Click(object sender, EventArgs e)
        {
            var paths = FileManagerService.OpenFile();
            if (paths == null) return;

            TextBoxCategoryList.Text = paths.First();
            allPaths["category"] = paths;
        }

        private void ButtonMasterDiscountList_Click(object sender, EventArgs e)
        {
            var paths = FileManagerService.OpenFile();
            if (paths == null) return;

            TextBoxMasterDiscountList.Text = paths.First();
            allPaths["masterDiscountList"] = paths;
        }

        private void ButtonReadNewPriceList_Click(object sender, EventArgs e)
        {
            lightspeedPath = TextBoxCurrentItems.Text;
            vendorPath = TextBoxNewPriceList.Text;

            brand = ComboBoxBrand?.SelectedIndex == -1 ? "" : ComboBoxBrand?.SelectedItem?.ToString();
            vendor = ComboBoxVendor.SelectedIndex == -1 ? "" : ComboBoxVendor?.SelectedItem?.ToString();

            if (String.IsNullOrEmpty(vendorPath) || !File.Exists(vendorPath) || String.IsNullOrEmpty(lightspeedPath) || !File.Exists(lightspeedPath))
            {
                MessageBox.Show("Please enter or select a vaild files", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (allPaths["currentItems"].Count == 2)
            {
                lightspeedPath = allPaths["currentItems"][1];
            }
            if (allPaths["newPriceList"].Count == 2)
            {
                vendorPath = allPaths["newPriceList"][1];
            }

            string placeholder = "----- Select -----";

            var (vh, vr) = UnifiedReaderService.ReadAll(vendorPath, 1, brand, vendor);
            vendorHeaders = vh;
            vendorRows = vr;
            var (lh, lr) = UnifiedReaderService.ReadAll(lightspeedPath, 1, "", "");
            lightspeedHeaders = lh;
            lightspeedRows = lr;

            LoadData(vendorRows, "New Price List");

            ComboBoxNewSKU.Items.Clear();
            ComboBoxNewUPC.Items.Clear();
            ComboBoxNewListPrice.Items.Clear();
            ComboBoxNewBrand.Items.Clear();
            ComboBoxNewDescription.Items.Clear();
            ComboBoxNewWeight.Items.Clear();
            ComboBoxNewDimention.Items.Clear();

            ComboBoxNewSKU.Items.Add(placeholder);
            ComboBoxNewUPC.Items.Add(placeholder);
            ComboBoxNewListPrice.Items.Add(placeholder);
            ComboBoxNewBrand.Items.Add(placeholder);
            ComboBoxNewDescription.Items.Add(placeholder);
            ComboBoxNewWeight.Items.Add(placeholder);
            ComboBoxNewDimention.Items.Add(placeholder);

            foreach (var item in vendorHeaders)
            {
                ComboBoxNewSKU.Items.Add(item);
                ComboBoxNewUPC.Items.Add(item);
                ComboBoxNewListPrice.Items.Add(item);
                ComboBoxNewBrand.Items.Add(item);
                ComboBoxNewDescription.Items.Add(item);
                ComboBoxNewWeight.Items.Add(item);
                ComboBoxNewDimention.Items.Add(item);
            }

            ComboBoxNewSKU.SelectedIndex = 0;
            ComboBoxNewUPC.SelectedIndex = 0;
            ComboBoxNewListPrice.SelectedIndex = 0;
            ComboBoxNewBrand.SelectedIndex = 0;
            ComboBoxNewDescription.SelectedIndex = 0;
            ComboBoxNewWeight.SelectedIndex = 0;
            ComboBoxNewDimention.SelectedIndex = 0;

            DataGridViewRecord.Visible = true;
            ButtonRetailUpdate.Visible = true;
            ButtonOnlineUpdate.Visible = true;
        }

        private void ButtonReadMasterPriceList_Click(object sender, EventArgs e)
        {
            masterDiscountPath = TextBoxMasterDiscountList.Text;

            if (String.IsNullOrEmpty(masterDiscountPath) || !File.Exists(masterDiscountPath))
            {
                MessageBox.Show("Please enter or select a vaild files", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (allPaths["masterDiscountList"].Count == 2)
            {
                masterDiscountPath = allPaths["masterDiscountList"][1];
            }

            var (mh, mr) = UnifiedReaderService.ReadAll(masterDiscountPath, 1, "", "");
            masterDiscountHeaders = mh;
            masterDiscountRows = mr;

            LoadData(masterDiscountRows, "Master Discount List");
        }

        private void ButtonRetailUpdate_Click(object sender, EventArgs e)
        {
            //if (ComboBoxNewSKU.SelectedItem == null || ComboBoxNewListPrice.SelectedItem == null || ComboBoxNewSKU.SelectedIndex == 0)
            //{
            //    MessageBox.Show("Please map SKU and Price at minimum.");
            //    return;
            //}
            categoryPath = TextBoxCategoryList.Text;
            masterDiscountPath = TextBoxMasterDiscountList.Text;

            if (String.IsNullOrEmpty(categoryPath) || !File.Exists(categoryPath) || String.IsNullOrEmpty(masterDiscountPath) || !File.Exists(masterDiscountPath)) {
                MessageBox.Show("Please enter or select a vaild files", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (allPaths["category"].Count == 2)
            {
                categoryPath = allPaths["category"][1];
            }
            if (allPaths["masterDiscountList"].Count == 2)
            {
                categoryPath = allPaths["masterDiscountList"][1];
            }


            string placeholder = "----- Select -----";
            string specify = "retail";

            SelectedMapping = new FieldMapping
            {
                VendorSkuField = ComboBoxNewSKU.SelectedItem?.ToString() == placeholder ? "" : ComboBoxNewSKU.SelectedItem?.ToString(),
                VendorUpcField = ComboBoxNewUPC.SelectedItem?.ToString() == placeholder ? "" : ComboBoxNewUPC.SelectedItem?.ToString(),
                VendorPriceField = ComboBoxNewListPrice.SelectedItem?.ToString() == placeholder ? "" : ComboBoxNewListPrice.SelectedItem?.ToString(),
                VendorDescriptionField = ComboBoxNewDescription.SelectedItem?.ToString() == placeholder ? "" : ComboBoxNewDescription.SelectedItem?.ToString(),
                VendorWeightField = ComboBoxNewWeight.SelectedItem?.ToString() == placeholder ? "" : ComboBoxNewWeight.SelectedItem?.ToString(),
                VendorDimensionsField = ComboBoxNewDimention.SelectedItem?.ToString() == placeholder ? "" : ComboBoxNewDimention.SelectedItem?.ToString(),
                VendorBrandField = CheckBoxUseField.Checked ? ComboBoxNewBrand.SelectedItem?.ToString() == placeholder ? "" : ComboBoxNewBrand.SelectedItem?.ToString() : "",
            };

            FormProcess.FormProcess_Load_From_XSLX(
                ProgressBarUpdate, 
                vendorRows, 
                lightspeedRows, 
                vendorPath, 
                lightspeedPath, 
                SelectedMapping, 
                vendorHeaders, 
                lightspeedHeaders, 
                brand: brand ?? "",
                vendor: vendor ?? "",
                categoryPath, 
                masterDiscountPath,
                specify
            );
        }

        private void ButtonOnlineUpdate_Click(object sender, EventArgs e)
        {
            onlinePath = TextBoxOnlineItems.Text;

            if (!String.IsNullOrEmpty(onlinePath) && File.Exists(onlinePath))
            {
                if (allPaths["onlineItems"].Count == 2)
                {
                    categoryPath = allPaths["onlineItems"][1];
                }
            } else
            {
                MessageBox.Show("Please enter or select a vaild files", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

                var (oh, or) = UnifiedReaderService.ReadAll(onlinePath, 1, "", "");
            onlineHeaders = oh;
            onlineRows = or;

            string placeholder = "----- Select -----";
            string specify = "online";

            SelectedMapping = new FieldMapping
            {
                VendorSkuField = ComboBoxNewSKU.SelectedItem?.ToString() == placeholder ? "" : ComboBoxNewSKU.SelectedItem?.ToString(),
                VendorUpcField = ComboBoxNewUPC.SelectedItem?.ToString() == placeholder ? "" : ComboBoxNewUPC.SelectedItem?.ToString(),
                VendorPriceField = ComboBoxNewListPrice.SelectedItem?.ToString() == placeholder ? "" : ComboBoxNewListPrice.SelectedItem?.ToString(),
                VendorDescriptionField = ComboBoxNewDescription.SelectedItem?.ToString() == placeholder ? "" : ComboBoxNewDescription.SelectedItem?.ToString(),
                VendorWeightField = ComboBoxNewWeight.SelectedItem?.ToString() == placeholder ? "" : ComboBoxNewWeight.SelectedItem?.ToString(),
                VendorDimensionsField = ComboBoxNewDimention.SelectedItem?.ToString() == placeholder ? "" : ComboBoxNewDimention.SelectedItem?.ToString(),
                VendorBrandField = CheckBoxUseField.Checked ? ComboBoxNewBrand.SelectedItem?.ToString() == placeholder ? "" : ComboBoxNewBrand.SelectedItem?.ToString() : "",
            };

            FormProcess.FormProcess_Load_From_XSLX(
                ProgressBarUpdate,
                onlineRows,
                lightspeedRows,
                onlinePath,
                lightspeedPath,
                SelectedMapping,
                onlineHeaders,
                lightspeedHeaders,
                brand: brand ?? "",
                vendor: vendor ?? "",
                categoryPath,
                masterDiscountPath,
                specify
            );
        }

        private void CheckBoxUseField_CheckedChanged(object sender, EventArgs e)
        {
            if (CheckBoxUseField.Checked)
            {
                ComboBoxNewBrand.Enabled = true;
            }
            else
            {
                ComboBoxNewBrand.Enabled = false;
            }
        }

        private void LoadData(List<ItemRecord> rows, string filter)
        {
            if (rows == null || rows.Count == 0) return;

            DataTable table = new DataTable();

            foreach (var field in rows[0].GetFields().Keys)
            {
                table.Columns.Add(field);
            }

            foreach (var row in rows)
            {
                var dr = table.NewRow();
                foreach (var kvp in row.GetFields())
                {
                    dr[kvp.Key] = kvp.Value ?? string.Empty;
                }
                table.Rows.Add(dr);
            }

            if (filter == "New Price List")
            {
                DataGridViewRecord.DataSource = new DataTable();
                DataGridViewRecord.DataSource = table;
            }
            if (filter == "Master Discount List")
            {
                DataGridViewMasterDiscountList.DataSource = new DataTable();
                DataGridViewMasterDiscountList.DataSource = table;
            }
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void TextBoxCurrentItems_TextChanged(object sender, EventArgs e)
        {
            if (TextBoxCurrentItems.Text.ToString() != "" && TextBoxNewPriceList.Text.ToString() != "")
            {
                ButtonReadNewPriceList.Enabled = true;
            } else
            {
                ButtonReadNewPriceList.Enabled = false;
            }
        }

        private void TextBoxNewPriceList_TextChanged(object sender, EventArgs e)
        {
            if (TextBoxCurrentItems.Text.ToString() != "" && TextBoxNewPriceList.Text.ToString() != "")
            {
                ButtonReadNewPriceList.Enabled = true;
            } else
            {
                ButtonReadNewPriceList.Enabled = false;
            }
        }

        private void TextBoxMasterDiscountList_TextChanged(object sender, EventArgs e)
        {
            if(TextBoxMasterDiscountList.Text.ToString() != "")
            {
                ButtonReadMasterPriceList.Enabled = true;
            } else
            {
                ButtonReadMasterPriceList.Enabled = false;
            }
        }
    }
}
