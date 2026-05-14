using System;
using System.Windows;
using System.Windows.Controls;
using UP1.Models;
using UP1.Services;
using UP1.Windows;

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
            ReadBookWindow readWindow = new ReadBookWindow(currentBook);
            readWindow.ShowDialog();
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

        private void BtnFreezeBook_Click(object sender, RoutedEventArgs e)
        {
            if (MainWindow.CurrentUser?.Role != "Administrator") return;

            var inputWindow = new Window
            {
                Title = "Заморозка книги",
                Width = 420,
                Height = 220,
                WindowStartupLocation = WindowStartupLocation.CenterScreen,
                Background = System.Windows.Media.Brushes.DarkSlateGray,
                ResizeMode = ResizeMode.NoResize
            };

            var stack = new StackPanel { Margin = new Thickness(20) };

            stack.Children.Add(new TextBlock
            {
                Text = "Введите причину заморозки книги:",
                Foreground = System.Windows.Media.Brushes.White,
                FontSize = 14,
                Margin = new Thickness(0, 0, 0, 10)
            });

            var tbReason = new TextBox
            {
                Height = 80,
                TextWrapping = TextWrapping.Wrap,
                AcceptsReturn = true,
                Background = System.Windows.Media.Brushes.White,
                Foreground = System.Windows.Media.Brushes.Black,
                VerticalScrollBarVisibility = ScrollBarVisibility.Auto
            };

            var btnOk = new Button
            {
                Content = "Заморозить книгу",
                Height = 40,
                Margin = new Thickness(0, 15, 0, 0),
                Background = System.Windows.Media.Brushes.OrangeRed,
                Foreground = System.Windows.Media.Brushes.White,
                FontSize = 14
            };

            stack.Children.Add(tbReason);
            stack.Children.Add(btnOk);
            inputWindow.Content = stack;

            btnOk.Click += (s, args) =>
            {
                string reason = tbReason.Text.Trim();
                if (!string.IsNullOrWhiteSpace(reason))
                {
                    MessageBox.Show($"Книга «{currentBook.Title}» была заморожена.\n\nПричина: {reason}",
                                  "Заморозка выполнена",
                                  MessageBoxButton.OK,
                                  MessageBoxImage.Warning);
                }
                inputWindow.Close();
            };

            inputWindow.ShowDialog();
        }

    }
}