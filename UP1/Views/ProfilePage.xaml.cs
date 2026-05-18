using System.Windows;
using System.Windows.Controls;
using UP1.Models;
using UP1.Windows;

namespace UP1.Views
{
    public partial class ProfilePage : Page
    {
        private User currentUser;

        public ProfilePage()
        {
            InitializeComponent();
            LoadUserInfo();
        }

        private void LoadUserInfo()
        {
            currentUser = MainWindow.CurrentUser; // будем брать из MainWindow

            if (currentUser == null) return;

            tbFullName.Text = $"Имя: {currentUser.FullName}";
            tbLogin.Text = $"Логин: {currentUser.Login}";
            tbEmail.Text = $"Email: {currentUser.Email}";
            tbRole.Text = $"Роль: {currentUser.Role}";

            if (currentUser.IsFrozen)
            {
                tbFreezeWarning.Visibility = Visibility.Visible;
                tbFreezeWarning.Text = $"⚠️ Аккаунт заморожен!\nПричина: {currentUser.FreezeReason}\n\n" +
                                       "Вы можете оспорить решение в Администрации.";
            }

            btnApplyAuthor.Visibility = currentUser.Role == "User" ? Visibility.Visible : Visibility.Collapsed;
        }

        private void BtnApplyAuthor_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Заявка на роль Автора отправлена администратору!", "Успешно", MessageBoxButton.OK, MessageBoxImage.Information);
            // В будущем будет сохраняться в список заявок
        }
    }
}