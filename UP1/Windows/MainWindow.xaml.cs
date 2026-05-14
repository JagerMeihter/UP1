using System.Windows;
using System.Windows.Controls;

namespace UP1.Windows
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            LoadDefaultPage();
        }

        private void LoadDefaultPage()
        {
            // По умолчанию открываем Каталог книг
            ContentFrame.Navigate(new Views.BookCatalogPage());
        }

        private void BtnCatalog_Click(object sender, RoutedEventArgs e)
        {
            ContentFrame.Navigate(new Views.BookCatalogPage());
            tbPageTitle.Text = "Каталог книг";
        }

        private void BtnLists_Click(object sender, RoutedEventArgs e)
        {
            ContentFrame.Navigate(new Views.BookListsPage());
            tbPageTitle.Text = "Мои списки книг";
        }

        private void BtnAuthor_Click(object sender, RoutedEventArgs e)
        {
            ContentFrame.Navigate(new Views.AuthorPage());
            tbPageTitle.Text = "Страница автора";
        }

        private void BtnAdmin_Click(object sender, RoutedEventArgs e)
        {
            ContentFrame.Navigate(new Views.AdminPage());
            tbPageTitle.Text = "Администрирование";
        }

        private void BtnProfile_Click(object sender, RoutedEventArgs e)
        {
            ContentFrame.Navigate(new Views.ProfilePage());
            tbPageTitle.Text = "Профиль";
        }

        private void BtnLogout_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Выйти из аккаунта?", "Выход", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                LoginWindow login = new LoginWindow();
                login.Show();
                this.Close();
            }
        }
    }
}