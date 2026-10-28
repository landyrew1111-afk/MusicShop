using System;
using System.Windows;

namespace WpfApp1
{
    public partial class CatalogWindow : Window
    {
        private readonly User _currentUser;

        public CatalogWindow(User user)
        {
            InitializeComponent();
            _currentUser = user;

            txtUserInfo.Text = "Пользователь: " + _currentUser.Fio + " | Роль: " + _currentUser.Rol;

            // Кнопки CRUD — только у Администратора
            if (_currentUser.Rol != "Администратор")
            {
                btnAdd.Visibility = Visibility.Collapsed;
                btnEdit.Visibility = Visibility.Collapsed;
                btnDelete.Visibility = Visibility.Collapsed;
            }

            // Отчёт — только у Оператора и Администратора
            if (_currentUser.Rol == "Клиент")
            {
                btnReport.Visibility = Visibility.Collapsed;
            }

            LoadProducts();
        }

        private void LoadProducts()
        {
            try
            {
                var products = ProductService.GetAll();
                dgProducts.ItemsSource = products;
                txtCount.Text = "Всего товаров: " + products.Count;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void BtnSearch_Click(object sender, RoutedEventArgs e)
        {
            string searchText = txtSearch.Text.Trim();
            if (searchText == "")
            {
                LoadProducts();
                return;
            }

            try
            {
                var products = ProductService.Search(searchText);
                dgProducts.ItemsSource = products;
                txtCount.Text = "Найдено: " + products.Count;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void BtnRefresh_Click(object sender, RoutedEventArgs e)
        {
            txtSearch.Text = "";
            LoadProducts();
        }

        private void BtnAdd_Click(object sender, RoutedEventArgs e)
        {
            var w = new ProductEditWindow(null);
            w.ShowDialog();
            LoadProducts();
        }

        private void BtnEdit_Click(object sender, RoutedEventArgs e)
        {
            Product selected = dgProducts.SelectedItem as Product;
            if (selected == null)
            {
                MessageBox.Show("Выберите товар.");
                return;
            }

            var w = new ProductEditWindow(selected);
            w.ShowDialog();
            LoadProducts();
        }

        private void BtnDelete_Click(object sender, RoutedEventArgs e)
        {
            Product selected = dgProducts.SelectedItem as Product;
            if (selected == null)
            {
                MessageBox.Show("Выберите товар.");
                return;
            }

            var result = MessageBox.Show(
                "Удалить \"" + selected.Naimenovanie + "\"?",
                "Подтверждение", MessageBoxButton.YesNo);

            if (result == MessageBoxResult.Yes)
            {
                try
                {
                    ProductEditService.Delete(selected.KodTovara);
                    LoadProducts();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Не удалось удалить (возможно, есть заказы с этим товаром).\n" + ex.Message);
                }
            }
        }

        private void BtnOrders_Click(object sender, RoutedEventArgs e)
        {
            var w = new OrdersWindow(_currentUser);
            w.ShowDialog();
        }

        private void BtnNewOrder_Click(object sender, RoutedEventArgs e)
        {
            var w = new OrderCreateWindow(_currentUser);
            w.ShowDialog();
            LoadProducts();
        }

        private void BtnReport_Click(object sender, RoutedEventArgs e)
        {
            var w = new ReportWindow();
            w.ShowDialog();
        }
    }
}