using System;
using System.Linq;
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
            tbDescription.Text = currentBook.Description ?? "Описание книги отсутствует.";

            // Кнопка заморозки видна только админу
            btnFreezeBook.Visibility = MainWindow.CurrentUser?.Role == "Administrator"
                                     ? Visibility.Visible : Visibility.Collapsed;
        }

        private void LoadReviews()
        {
            reviewsPanel.Children.Clear();
            var reviews = App.DataService.GetReviewsForBook(currentBook.Id);

            if (reviews.Count == 0)
            {
                var tb = new TextBlock
                {
                    Text = "Пока нет отзывов. Будьте первым!",
                    Foreground = System.Windows.Media.Brushes.Gray,
                    FontStyle = FontStyles.Italic,
                    Margin = new Thickness(10)
                };
                reviewsPanel.Children.Add(tb);
                return;
            }

            foreach (var review in reviews)
            {
                var reviewBorder = new Border
                {
                    Background = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Color.FromRgb(40, 40, 40)),
                    CornerRadius = new CornerRadius(8),
                    Margin = new Thickness(0, 0, 0, 10),
                    Padding = new Thickness(12)
                };

                var stack = new StackPanel();
                stack.Children.Add(new TextBlock
                {
                    Text = $"{review.UserLogin} — {new string('★', review.Rating)}",
                    Foreground = System.Windows.Media.Brushes.Gold,
                    FontWeight = FontWeights.Bold
                });
                stack.Children.Add(new TextBlock
                {
                    Text = review.Text,
                    Foreground = System.Windows.Media.Brushes.White,
                    TextWrapping = TextWrapping.Wrap,
                    Margin = new Thickness(0, 5, 0, 0)
                });

                reviewBorder.Child = stack;
                reviewsPanel.Children.Add(reviewBorder);
            }
        }

        private void BtnRead_Click(object sender, RoutedEventArgs e)
        {
            ReadBookWindow readWindow = new ReadBookWindow(currentBook);
            readWindow.ShowDialog();
        }

        private void BtnPublishReview_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtReviewText.Text))
            {
                MessageBox.Show("Напишите текст отзыва!", "Ошибка");
                return;
            }

            int rating = 5 - cmbRating.SelectedIndex;

            var review = new Review
            {
                Id = App.DataService.Reviews.Count + 1,
                BookId = currentBook.Id,
                UserId = MainWindow.CurrentUser.Id,
                UserLogin = MainWindow.CurrentUser.Login,
                Text = txtReviewText.Text,
                Rating = rating
            };

            App.DataService.Reviews.Add(review);
            App.DataService.SaveReviews();

            MessageBox.Show("Отзыв успешно опубликован!", "Спасибо");
            txtReviewText.Clear();
            LoadReviews();

            // Пересчёт среднего рейтинга книги (упрощённо)
            var allReviews = App.DataService.GetReviewsForBook(currentBook.Id);
            if (allReviews.Count > 0)
            {
                currentBook.Rating = Math.Round(allReviews.Average(r => r.Rating), 1);
                App.DataService.SaveBooks();
            }
        }

        private void BtnReportBook_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Жалоба на книгу отправлена администратору.", "Жалоба");
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
                Foreground = System.Windows.Media.Brushes.Black
            };

            var btnOk = new Button
            {
                Content = "Заморозить книгу",
                Height = 40,
                Margin = new Thickness(0, 15, 0, 0),
                Background = System.Windows.Media.Brushes.OrangeRed,
                Foreground = System.Windows.Media.Brushes.White
            };

            stack.Children.Add(tbReason);
            stack.Children.Add(btnOk);
            inputWindow.Content = stack;

            btnOk.Click += (s, args) =>
            {
                string reason = tbReason.Text.Trim();
                if (!string.IsNullOrWhiteSpace(reason))
                {
                    MessageBox.Show($"Книга «{currentBook.Title}» заморожена.\nПричина: {reason}",
                                  "Заморозка", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
                inputWindow.Close();
            };

            inputWindow.ShowDialog();
        }
    }
}