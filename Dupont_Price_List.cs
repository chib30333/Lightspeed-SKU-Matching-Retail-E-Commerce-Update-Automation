using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using Dupont_Price_Lists.Models;
using Dupont_Price_Lists.Services;
using Dupont_Price_Lists.Services.Pipeline;

namespace Dupont_Price_Lists
{
    public partial class Dupont_Price_List : Form
    {
        private readonly PipelineOrchestrator _pipe = new();

        private readonly Dictionary<string, List<string>> allPaths = new();

        private string vendorPath = "";
        private string lightspeedPath = "";
        private string categoryPath = "";
        private string masterDiscountPath = "";
        private string onlinePath = "";

        private List<string> vendorHeaders = new();
        private List<ItemRecord> vendorRows = new();

        private List<string> lightspeedHeaders = new();
        private List<ItemRecord> lightspeedRows = new();

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
            lightspeedPath = ResolveEffectivePath("currentItems", TextBoxCurrentItems.Text);
            vendorPath = ResolveEffectivePath("newPriceList", TextBoxNewPriceList.Text);

            if (!File.Exists(vendorPath) || !File.Exists(lightspeedPath))
            {
                MessageBox.Show("Please select valid Current Items and New Price List files.", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            var (vh, vr) = UnifiedReaderService.ReadAll(vendorPath, 1);
            vendorHeaders = vh;
            vendorRows = vr;

            var (lh, lr) = UnifiedReaderService.ReadAll(lightspeedPath, 1);
            lightspeedHeaders = lh;
            lightspeedRows = lr;

            LoadData(vendorRows, DataGridViewRecord);
            DataGridViewRecord.Visible = true;

            PopulateMappingDropdowns(vendorHeaders);

            ButtonRetailUpdate.Visible = true;
            ButtonOnlineUpdate.Visible = true;
        }

        private void ButtonReadMasterPriceList_Click(object sender, EventArgs e)
        {
            masterDiscountPath = ResolveEffectivePath("masterDiscountList", TextBoxMasterDiscountList.Text);

            if (!File.Exists(masterDiscountPath))
            {
                MessageBox.Show("Please select a valid Master Discount List file.", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            var (_, rows) = UnifiedReaderService.ReadAll(masterDiscountPath, 1);
            LoadData(rows, DataGridViewMasterDiscountList);
        }

        private async void ButtonRetailUpdate_Click(object sender, EventArgs e)
        {
            try
            {
                if (ComboBoxNewSKU.SelectedIndex <= 0)
                {
                    MessageBox.Show("Please map SKU at minimum.");
                    return;
                }

                categoryPath = ResolveEffectivePath("category", TextBoxCategoryList.Text);
                masterDiscountPath = ResolveEffectivePath("masterDiscountList", TextBoxMasterDiscountList.Text);

                if (!File.Exists(categoryPath) || !File.Exists(masterDiscountPath))
                {
                    MessageBox.Show("Please select valid Category List and Master Discount List files.", "Error",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                var profile = BuildProfileFromUi();

                ProgressBarUpdate.Style = ProgressBarStyle.Marquee;
                TextStatus.Text = "Building Retail update...";

                var result = await _pipe.BuildRetailAsync(
                    vendorPath, lightspeedPath, categoryPath, masterDiscountPath, profile);

                if (result.Warnings.Any())
                    MessageBox.Show(string.Join("\n", result.Warnings), "Warnings");

                Directory.CreateDirectory("save");
                var outputPath = Path.Combine("save", "Retail.xlsx");
                await _pipe.WriteRetailAsync(outputPath, result.Rows);

                ProgressBarUpdate.Style = ProgressBarStyle.Continuous;
                ProgressBarUpdate.Value = 100;
                TextStatus.Text = $"Done. Found={result.Found}, New={result.NewItems}, Out={result.Rows.Count}";

                MessageBox.Show($"Exported:\n{outputPath}", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}", "Processing Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async void ButtonOnlineUpdate_Click(object sender, EventArgs e)
        {
            try
            {
                if (ComboBoxNewSKU.SelectedIndex <= 0)
                {
                    MessageBox.Show("Please map SKU at minimum.");
                    return;
                }

                onlinePath = ResolveEffectivePath("onlineItems", TextBoxOnlineItems.Text);
                masterDiscountPath = ResolveEffectivePath("masterDiscountList", TextBoxMasterDiscountList.Text);

                if (!File.Exists(onlinePath) || !File.Exists(masterDiscountPath))
                {
                    MessageBox.Show("Please select valid Online Items and Master Discount List files.", "Error",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                var profile = BuildProfileFromUi();

                ProgressBarUpdate.Style = ProgressBarStyle.Marquee;
                TextStatus.Text = "Building Online update...";

                var result = await _pipe.BuildOnlineAsync(
                    onlinePath, vendorPath, lightspeedPath, masterDiscountPath, profile);

                Directory.CreateDirectory("save");
                var outputPath = Path.Combine("save", "Online.xlsx");
                await _pipe.WriteOnlineAsync(outputPath, result.Rows);

                ProgressBarUpdate.Style = ProgressBarStyle.Continuous;
                ProgressBarUpdate.Value = 100;
                TextStatus.Text = $"Done. Online Out={result.Rows.Count}";

                MessageBox.Show($"Exported:\n{outputPath}", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}", "Processing Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void CheckBoxUseField_CheckedChanged(object sender, EventArgs e)
        {
            ComboBoxNewBrand.Enabled = CheckBoxUseField.Checked;
        }

        private static string? SelectedMappingOrNull(ComboBox cb)
        {
            var placeholder = "----- Select -----";
            var val = cb.SelectedItem?.ToString();
            if (string.IsNullOrWhiteSpace(val) || val == placeholder) return null;
            return val;
        }

        private MappingProfile BuildProfileFromUi()
        {
            var profile = new MappingProfile
            {
                VendorSkuField = SelectedMappingOrNull(ComboBoxNewSKU) ?? "",
                VendorDescriptionField = SelectedMappingOrNull(ComboBoxNewDescription),
                VendorFinishField = SelectedMappingOrNull(ComboBoxNewFinish),
                VendorUpcField = SelectedMappingOrNull(ComboBoxNewUPC),
                VendorPriceField = SelectedMappingOrNull(ComboBoxNewListPrice),
                VendorWeightField = SelectedMappingOrNull(ComboBoxNewWeight),
                VendorDimensionsField = SelectedMappingOrNull(ComboBoxNewDimention),
                VendorVendorField = "Vendor",

                UseBrandFromField = CheckBoxUseField.Checked,
                VendorBrandField = CheckBoxUseField.Checked ? SelectedMappingOrNull(ComboBoxNewBrand) : null,

                // fixed brand/vendor dropdowns
                UseFixedBrand = ComboBoxBrand?.SelectedIndex > 0,
                FixedBrand = ComboBoxBrand?.SelectedIndex > 0 ? ComboBoxBrand.SelectedItem?.ToString() : null,

                UseFixedVendor = ComboBoxVendor?.SelectedIndex > 0,
                FixedVendor = ComboBoxVendor?.SelectedIndex > 0 ? ComboBoxVendor.SelectedItem?.ToString() : null,
            };

            return profile;
        }

        private string ResolveEffectivePath(string key, string fallbackTextBoxPath)
        {
            if (allPaths.TryGetValue(key, out var list) && list.Count == 2)
                return list[1]; // converted xlsx path
            return fallbackTextBoxPath;
        }

        private void PopulateMappingDropdowns(List<string> headers)
        {
            string placeholder = "----- Select -----";

            var combos = new[]
            {
                ComboBoxNewSKU, ComboBoxNewUPC, ComboBoxNewListPrice, ComboBoxNewBrand,
                ComboBoxNewDescription, ComboBoxNewWeight, ComboBoxNewDimention, ComboBoxNewFinish
            };

            foreach (var cb in combos)
            {
                cb.Items.Clear();
                cb.Items.Add(placeholder);
                foreach (var h in headers) cb.Items.Add(h);
                cb.SelectedIndex = 0;
            }
        }

        private void LoadData(List<ItemRecord> rows, DataGridView grid)
        {
            if (rows == null || rows.Count == 0) return;

            DataTable table = new DataTable();
            foreach (var field in rows[0].GetFields().Keys)
                table.Columns.Add(field);

            foreach (var row in rows)
            {
                var dr = table.NewRow();
                foreach (var kvp in row.GetFields())
                    dr[kvp.Key] = kvp.Value ?? "";
                table.Rows.Add(dr);
            }

            grid.DataSource = new DataTable();
            grid.DataSource = table;
        }

        private void TextBoxCurrentItems_TextChanged(object sender, EventArgs e)
        {
            ButtonReadNewPriceList.Enabled = !string.IsNullOrWhiteSpace(TextBoxCurrentItems.Text) &&
                                            !string.IsNullOrWhiteSpace(TextBoxNewPriceList.Text);
        }

        private void TextBoxNewPriceList_TextChanged(object sender, EventArgs e)
        {
            ButtonReadNewPriceList.Enabled = !string.IsNullOrWhiteSpace(TextBoxCurrentItems.Text) &&
                                            !string.IsNullOrWhiteSpace(TextBoxNewPriceList.Text);
        }

        private void TextBoxMasterDiscountList_TextChanged(object sender, EventArgs e)
        {
            ButtonReadMasterPriceList.Enabled = !string.IsNullOrWhiteSpace(TextBoxMasterDiscountList.Text);
        }
    }
}
