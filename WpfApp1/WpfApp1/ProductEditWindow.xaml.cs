using System;
using System.Windows;

namespace WpfApp1
{
    public partial class ProductEditWindow : Window
    {
        private readonly int _kodTovara; 

        public ProductEditWindow(Product product)
        {
            InitializeComponent();

            if (product == null)
            {
                _kodTovara = 0;
                txtTitle.Text = "Новый товар";
                txtGod.Text = "2024";
                txtTsenaRozn.Text = "0";
                txtTsenaOpt.Text = "0";
                txtOstatok.Text = "0";
            }
            else
            {
                _kodTovara = product.KodTovara;
                txtTitle.Text = "Редактирование товара";
                txtNaimenovanie.Text = product.Naimenovanie;
                txtIspolnitel.Text = product.Ispolnitel;
                txtZhanr.Text = product.Zhanr;
                txtGod.Text = product.GodVypuska.ToString();
                txtTsenaRozn.Text = product.TsenaRoznitsa.ToString("0.##");
                txtTsenaOpt.Text = product.TsenaOpt.ToString("0.##");
                txtOstatok.Text = product.Ostatki.ToString();
            }

            LoadCategories();

            if (product != null)
                cmbKategoriya.Text = product.Kategoriya;
        }

        private void LoadCategories()
        {
            try
            {
                var cats = ProductEditService.GetCategories();
                cmbKategoriya.ItemsSource = cats;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            if (txtNaimenovanie.Text.Trim() == "")
            {
                txtStatus.Text = "Введите наименование.";
                return;
            }
            if (cmbKategoriya.SelectedItem == null)
            {
                txtStatus.Text = "Выберите категорию.";
                return;
            }

            decimal tsenaRozn, tsenaOpt;
            int god, ostatok;

            if (!decimal.TryParse(txtTsenaRozn.Text, out tsenaRozn))
            {
                txtStatus.Text = "Неверная розничная цена.";
                return;
            }
            if (!decimal.TryParse(txtTsenaOpt.Text, out tsenaOpt))
            {
                tsenaOpt = 0;
            }
            if (!int.TryParse(txtGod.Text, out god))
            {
                god = 0;
            }
            if (!int.TryParse(txtOstatok.Text, out ostatok))
            {
                ostatok = 0;
            }

            try
            {
                string kategoriya = cmbKategoriya.SelectedItem.ToString();
                int kodKategorii = ProductEditService.GetCategoryId(kategoriya);

                if (_kodTovara == 0)
                {
                    ProductEditService.Insert(
                        txtNaimenovanie.Text.Trim(),
                        txtIspolnitel.Text.Trim(),
                        kodKategorii,
                        txtZhanr.Text.Trim(),
                        god, tsenaRozn, tsenaOpt, ostatok);
                }
                else
                {
                    ProductEditService.Update(
                        _kodTovara,
                        txtNaimenovanie.Text.Trim(),
                        txtIspolnitel.Text.Trim(),
                        kodKategorii,
                        txtZhanr.Text.Trim(),
                        god, tsenaRozn, tsenaOpt, ostatok);
                }

                MessageBox.Show("Сохранено.", "Готово",
                    MessageBoxButton.OK, MessageBoxImage.Information);
                this.Close();
            }
            catch (Exception ex)
            {
                txtStatus.Text = ex.Message;
            }
        }

        private void BtnCancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}