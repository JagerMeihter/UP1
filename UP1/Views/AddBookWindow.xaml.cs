using System.Windows;
using UP1.Models;
using UP1.Services;
using UP1.Windows;

namespace UP1.Views
{
    public partial class AddBookWindow : Window
    {
        public AddBookWindow()
        {
            InitializeComponent();
            txtAuthor.Text = MainWindow.CurrentUser?.FullName ?? "Автор";
        }

        private void BtnPublish_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtTitle.Text))
            {
                MessageBox.Show("Название книги обязательно!", "Ошибка");
                return;
            }

            var newBook = new Book
            {
                Id = App.DataService.Books.Count + 1,
                Title = txtTitle.Text,
                Author = txtAuthor.Text,
                Genre = cmbGenre.Text,
                Description = txtDescription.Text,
                Text = txtText.Text,
                Rating = 0.0,
                Cover = "📖"
            };

            App.DataService.Books.Add(newBook);
            App.DataService.SaveBooks();

            MessageBox.Show("Книга успешно опубликована!", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
            this.DialogResult = true;
            this.Close();
        }
    }
}