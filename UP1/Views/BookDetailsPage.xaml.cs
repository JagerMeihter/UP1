using System;
using System.Windows;
using System.Windows.Controls;
using UP1.Models;
using UP1.Services;

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
            LoadReviews();
        }

        private void LoadBookInfo()
        {
            tbCover.Text = currentBook.Cover;
            tbTitle.Text = currentBook.Title;
            tbAuthor.Text = "Автор: " + currentBook.Author;
            tbRating.Text = $"⭐ {currentBook.Rating}";
            tbDescription.Text = currentBook.Description ?? "Описание отсутствует.";
        }

        private void LoadReviews()
        {
            reviewsPanel.Children.Clear();
            // Пока просто заглушка
            var placeholder = new TextBlock
            {
                Text = "Пока нет отзывов. Будьте первым!",
                Foreground = System.Windows.Media.Brushes.Gray,
                Margin = new Thickness(10),
                FontStyle = FontStyles.Italic
            };
            reviewsPanel.Children.Add(placeholder);
        }

        private void BtnRead_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show(currentBook.Text ?? "Полный текст книги будет здесь...",
                          $"Чтение: {currentBook.Title}", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void BtnReview_Click(object sender, RoutedEventArgs e)
        {
            // Можно прокрутить к форме отзыва
            MessageBox.Show("Заполните форму ниже и нажмите «Опубликовать отзыв»");
        }

        private void BtnPublishReview_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtReviewText.Text))
            {
                MessageBox.Show("Напишите текст отзыва!", "Ошибка");
                return;
            }

            int rating = 5;
            if (cmbRating.SelectedIndex >= 0)
                rating = 5 - cmbRating.SelectedIndex;

            MessageBox.Show($"Отзыв на {rating} звёзд опубликован!\n\n«{txtReviewText.Text}»",
                          "Спасибо!", MessageBoxButton.OK, MessageBoxImage.Information);

            txtReviewText.Clear();
            LoadReviews(); // обновление списка
        }

        private void BtnReportBook_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Жалоба на книгу отправлена администратору.", "Жалоба отправлена");
        }
    }
}