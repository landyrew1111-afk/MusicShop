using System;
using System.Windows;
using System.Windows.Controls;

namespace WpfApp1
{
    public partial class OrdersWindow : Window
    {
        private readonly User _currentUser;

        public OrdersWindow(User user)
        {
            InitializeComponent();
            _currentUser = user;
            LoadOrders();
        }

        private void LoadOrders()
        {
            try
            {
                string selected = ((ComboBoxItem)cmbStatus.SelectedItem).Content.ToString();

                System.Collections.ObjectModel.ObservableCollection<Order> orders;

                if (selected == "Все")
                {
                    if (_currentUser.Rol == "Клиент")
                        orders = OrderService.GetByUser(_currentUser.KodPolzovatelya);
                    else
                        orders = OrderService.GetAll();
                }
                else
                {
                    orders = OrderService.FilterByStatus(selected);

                    if (_currentUser.Rol == "Клиент")
                    {
                        var onlyMine = new System.Collections.ObjectModel.ObservableCollection<Order>();
                        foreach (var o in orders)
                        {
                            foreach (var mine in OrderService.GetByUser(_currentUser.KodPolzovatelya))
                            {
                                if (mine.KodZakaza == o.KodZakaza) { onlyMine.Add(o); break; }
                            }
                        }
                        orders = onlyMine;
                    }
                }

                dgOrders.ItemsSource = orders;
                txtCount.Text = "Всего заказов: " + orders.Count;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void BtnApply_Click(object sender, RoutedEventArgs e)
        {
            LoadOrders();
        }

        private void BtnDetails_Click(object sender, RoutedEventArgs e)
        {
            Order selected = dgOrders.SelectedItem as Order;
            if (selected == null)
            {
                MessageBox.Show("Выберите заказ.");
                return;
            }

            var dlg = new OrderDetailsWindow(selected.KodZakaza, _currentUser);
            dlg.ShowDialog();
            LoadOrders();
        }
    }
}