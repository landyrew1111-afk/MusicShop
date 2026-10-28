using System;
using System.Windows;

namespace WpfApp1
{
    public partial class LoginWindow : Window
    {
        public LoginWindow()
        {
            InitializeComponent();
        }

        private void BtnLogin_Click(object sender, RoutedEventArgs e)
        {
            string login = txtLogin.Text.Trim();
            string password = txtPassword.Password;

            if (login == "" || password == "")
            {
                txtStatus.Text = "Введите логин и пароль!";
                return;
            }

            try
            {
                var user = AuthService.Login(login, password);

                if (user == null)
                {
                    txtStatus.Text = "Неверный логин или пароль!";
                    return;
                }

                var mainWindow = new CatalogWindow(user);
                mainWindow.Show();
                this.Close();
            }
            catch (Exception ex)
            {
                txtStatus.Text = ex.Message;
            }
        }

        private void BtnRegister_Click(object sender, RoutedEventArgs e)
        {
            var reg = new RegisterWindow();
            reg.ShowDialog();
        }
    }
}