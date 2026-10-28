using System;
using System.Windows;

namespace WpfApp1
{
    public partial class OrderDetailsWindow : Window
    {
        private readonly int _kodZakaza;
        private readonly User _currentUser;

        public OrderDetailsWindow(int kodZakaza, User currentUser)
        {
            InitializeComponent();
            _kodZakaza = kodZakaza;
            _currentUser = currentUser;

            txtTitle.Text = "О заказе № " + kodZakaza;

            LoadStatuses();
            LoadItems();

            // Смена статуса — только Оператор и Администратор
            if (_currentUser.Rol == "Клиент")
            {
                cmbStatus.IsEnabled = false;
                btnChangeStatus.Visibility = Visibility.Collapsed;
            }
        }

        private void LoadStatuses()
        {
            try
            {
                string currentStatus = OrderStatusService.GetStatusName(_kodZakaza);
                var statuses = OrderStatusService.GetAllStatuses();
                cmbStatus.ItemsSource = statuses;

                foreach (var s in statuses)
                {
                    if (s == currentStatus) { cmbStatus.SelectedItem = s; break; }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void LoadItems()
        {
            try
            {
                var items = OrderItemService.GetByOrder(_kodZakaza);
                dgItems.ItemsSource = items;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void BtnChangeStatus_Click(object sender, RoutedEventArgs e)
        {
            if (cmbStatus.SelectedItem == null) return;

            try
            {
                string newStatus = cmbStatus.SelectedItem.ToString();
                OrderStatusService.ChangeStatus(_kodZakaza, newStatus,
                                                _currentUser.KodPolzovatelya);

                MessageBox.Show("Статус изменён на \"" + newStatus + "\".",
                    "Готово", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void BtnPrint_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Печать будет реализована позже.");
        }

        private void BtnClose_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}