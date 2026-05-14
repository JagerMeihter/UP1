using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using UP1.Models;
using UP1.Services;
using UP1.Windows;

namespace UP1.Views
{
    public partial class AuthorPage : Page
    {
        public AuthorPage()
        {
            InitializeComponent();
            LoadAuthorBooks();
        }

        private void LoadAuthorBooks()
        {
            var user = MainWindow.CurrentUser;
            if (user == null) return;

            // Пока показываем все книги (в будущем фильтр по автору)
            var books = App.DataService.Books;

            authorBooksPanel.Children.Clear();

            foreach (var book in books)
            {
                var card = CreateBookCard(book);
                authorBooksPanel.Children.Add(card);
            }
        }

        private Border CreateBookCard(Book book)
        {
            var border = new Border
            {
                Width = 160,
                Height = 250,
                Background = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Color.FromRgb(45, 45, 48)),
                CornerRadius = new CornerRadius(10),
                Margin = new Thickness(12),
                Cursor = System.Windows.Input.Cursors.Hand
            };

            var stack = new StackPanel { Margin = new Thickness(10) };

            var cover = new TextBlock { Text = book.Cover, FontSize = 60, HorizontalAlignment = HorizontalAlignment.Center };
            var title = new TextBlock
            {
                Text = book.Title,
                FontWeight = FontWeights.Bold,
                TextWrapping = TextWrapping.Wrap,
                TextAlignment = TextAlignment.Center,
                Foreground = System.Windows.Media.Brushes.White
            };

            stack.Children.Add(cover);
            stack.Children.Add(title);

            border.Child = stack;

            border.MouseLeftButtonUp += (s, e) =>
            {
                NavigationService.Navigate(new BookDetailsPage(book));
            };

            return border;
        }

        private void BtnAddNewBook_Click(object sender, RoutedEventArgs e)
        {
            var addWindow = new AddBookWindow();
            if (addWindow.ShowDialog() == true)
            {
                LoadAuthorBooks(); // обновляем список после добавления
            }
        }
    }
}