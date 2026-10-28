using System;
using System.Collections.ObjectModel;
using System.Windows;

namespace WpfApp1
{
    public partial class OrderCreateWindow : Window
    {
        private readonly User _currentUser;
        private readonly ObservableCollection<CartItem> _cart;

        public OrderCreateWindow(User user)
        {
            InitializeComponent();
            _currentUser = user;
            _cart = new ObservableCollection<CartItem>();
            dgCart.ItemsSource = _cart;

            LoadCatalog();
        }

        private void LoadCatalog()
        {
            try
            {
                var products = ProductService.GetAll();
                dgCatalog.ItemsSource = products;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void BtnSearch_Click(object sender, RoutedEventArgs e)
        {
            string s = txtSearch.Text.Trim();
            try
            {
                if (s == "")
                    dgCatalog.ItemsSource = ProductService.GetAll();
                else
                    dgCatalog.ItemsSource = ProductService.Search(s);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void BtnAddToCart_Click(object sender, RoutedEventArgs e)
        {
            Product selected = dgCatalog.SelectedItem as Product;
            if (selected == null)
            {
                MessageBox.Show("Выберите товар.");
                return;
            }

            if (selected.Ostatki <= 0)
            {
                MessageBox.Show("Товар закончился.");
                return;
            }

            foreach (var it in _cart)
            {
                if (it.KodTovara == selected.KodTovara)
                {
                    if (it.Kolichestvo + 1 > selected.Ostatki)
                    {
                        MessageBox.Show("Больше нет на складе.");
                        return;
                    }
                    it.Kolichestvo++;
                    dgCart.Items.Refresh();
                    RecalcTotal();
                    return;
                }
            }

            var ci = new CartItem();
            ci.KodTovara = selected.KodTovara;
            ci.Naimenovanie = selected.Naimenovanie;
            ci.Tsena = selected.TsenaRoznitsa;
            ci.Kolichestvo = 1;
            _cart.Add(ci);
            RecalcTotal();
        }

        private void BtnRemove_Click(object sender, RoutedEventArgs e)
        {
            CartItem selected = dgCart.SelectedItem as CartItem;
            if (selected == null) return;

            _cart.Remove(selected);
            RecalcTotal();
        }

        private void RecalcTotal()
        {
            decimal total = 0;
            foreach (var it in _cart) total += it.Summa;
            txtTotal.Text = "Итого: " + total.ToString("N2") + " руб.";
        }

        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            if (_cart.Count == 0)
            {
                MessageBox.Show("Корзина пуста.");
                return;
            }

            try
            {
                int kodDostavki = cmbDelivery.SelectedIndex + 1;

                int kodZakaza = OrderCreateService.CreateOrder(
                    _currentUser.KodPolzovatelya,
                    kodDostavki,
                    _cart);

                MessageBox.Show("Заказ № " + kodZakaza + " оформлен!",
                    "Готово", MessageBoxButton.OK, MessageBoxImage.Information);

                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void BtnCancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}