using System.Linq;
using System.Windows;
using UP1.Services;
using UP1.Windows;

namespace UP1.Windows
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

            if (string.IsNullOrWhiteSpace(login) || string.IsNullOrWhiteSpace(password))
            {
                MessageBox.Show("Введите логин и пароль!", "Ошибка");
                return;
            }

            var user = App.DataService.Users.FirstOrDefault(u =>
                u.Login == login && u.Password == password);

            if (user != null)
            {
                MainWindow main = new MainWindow(user);
                main.Show();
                this.Close();
            }
            else
            {
                MessageBox.Show("Неверный логин или пароль!");
            }
        }

        private void BtnRegister_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Регистрация будет добавлена позже.\n\nТестовые аккаунты:\nadmin / admin123\nauthor / author123\nuser / user123");
        }
    }
}