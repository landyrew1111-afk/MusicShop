using System;
using System.Windows;

namespace WpfApp1
{
    public partial class RegisterWindow : Window
    {
        public RegisterWindow()
        {
            InitializeComponent();
        }

        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            if (txtFamiliya.Text.Trim() == "" || txtImya.Text.Trim() == "" ||
                txtLogin.Text.Trim() == "" || txtParol.Password == "")
            {
                txtStatus.Text = "Заполните: Фамилия, Имя, Логин, Пароль.";
                return;
            }

            try
            {
                if (AuthService.LoginExists(txtLogin.Text.Trim()))
                {
                    txtStatus.Text = "Такой логин уже занят.";
                    return;
                }

                AuthService.Register(
                    txtFamiliya.Text.Trim(),
                    txtImya.Text.Trim(),
                    txtOtchestvo.Text.Trim(),
                    txtLogin.Text.Trim(),
                    txtParol.Password,
                    1,
                    txtTelefon.Text.Trim(),
                    txtEmail.Text.Trim(),
                    txtAdres.Text.Trim());

                MessageBox.Show("Регистрация прошла успешно! Теперь войдите в систему.",
                    "Готово", MessageBoxButton.OK, MessageBoxImage.Information);

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