using System.Windows;
using System.Windows.Controls;
using UP1.Models;

namespace UP1.Views
{
    public partial class BookDetailsPage : Page
    {
        private Book currentBook;

        public BookDetailsPage(Book book)
        {
            InitializeComponent();
            currentBook = book;
            LoadBookInfo();
        }

        private void LoadBookInfo()
        {
            tbCover.Text = currentBook.Cover;
            tbTitle.Text = currentBook.Title;
            tbAuthor.Text = "Автор: " + currentBook.Author;
            tbRating.Text = $"⭐ Рейтинг: {currentBook.Rating}";
            tbDescription.Text = currentBook.Description ?? "Описание книги отсутствует.";
        }

        private void BtnRead_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show($"Открывается книга:\n\n{currentBook.Title}\n\n{currentBook.Text ?? "Текст книги будет здесь..."}",
                          "Чтение книги", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void BtnReview_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Форма добавления отзыва будет здесь (на следующем этапе)", "Отзыв");
        }

        private void BtnReportBook_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Жалоба на книгу отправлена (прототип)", "Жалоба");
        }
    }
}