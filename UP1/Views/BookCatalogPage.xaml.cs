using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using UP1.Models;
using UP1.Services;
using UP1.Windows;

namespace UP1.Views
{
    public partial class BookCatalogPage : Page
    {
        private List<Book> allBooks;

        public BookCatalogPage()
        {
            InitializeComponent();
            LoadBooks();
        }

        private void LoadBooks()
        {
            allBooks = App.DataService.Books;
            DisplayBooks(allBooks);
        }

        private void DisplayBooks(List<Book> books)
        {
            booksPanel.Children.Clear();

            foreach (var book in books)
            {
                var card = CreateBookCard(book);
                booksPanel.Children.Add(card);
            }
        }

        private Border CreateBookCard(Book book)
        {
            var border = new Border
            {
                Width = 170,
                Height = 280,
                Background = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Color.FromRgb(45, 45, 48)),
                CornerRadius = new CornerRadius(10),
                Margin = new Thickness(12),
                Cursor = System.Windows.Input.Cursors.Hand
            };

            var stack = new StackPanel { Margin = new Thickness(10) };

            var cover = new TextBlock
            {
                Text = book.Cover,
                FontSize = 65,
                HorizontalAlignment = HorizontalAlignment.Center,
                Margin = new Thickness(0, 10, 0, 8)
            };

            var title = new TextBlock
            {
                Text = book.Title,
                FontWeight = FontWeights.Bold,
                TextWrapping = TextWrapping.Wrap,
                TextAlignment = TextAlignment.Center,
                Foreground = System.Windows.Media.Brushes.White,
                Margin = new Thickness(0, 0, 0, 5),
                Height = 48
            };

            var author = new TextBlock
            {
                Text = book.Author,
                TextAlignment = TextAlignment.Center,
                Foreground = System.Windows.Media.Brushes.LightGray,
                Margin = new Thickness(0, 0, 0, 10)
            };

            var rating = new TextBlock
            {
                Text = $"⭐ {book.Rating}",
                Foreground = System.Windows.Media.Brushes.Gold,
                HorizontalAlignment = HorizontalAlignment.Center
            };

            var addButton = new Button
            {
                Content = "➕ В список",
                Height = 30,
                Margin = new Thickness(0, 8, 0, 0),
                Background = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Color.FromRgb(33, 150, 243)),
                Foreground = System.Windows.Media.Brushes.White
            };

            stack.Children.Add(cover);
            stack.Children.Add(title);
            stack.Children.Add(author);
            stack.Children.Add(rating);
            stack.Children.Add(addButton);

            border.Child = stack;

            // Клик по всей карточке — открыть книгу
            border.MouseLeftButtonUp += (s, e) =>
            {
                if (s != addButton) // чтобы кнопка не конфликтовала
                    NavigationService.Navigate(new BookDetailsPage(book));
            };

            // Клик по кнопке "В список"
            addButton.Click += (s, e) =>
            {
                ShowAddToShelfMenu(book);
            };

            return border;
        }

        private void TxtSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (allBooks == null) return;

            var searchText = txtSearch.Text.ToLower();
            var filtered = allBooks.Where(b =>
                b.Title.ToLower().Contains(searchText) ||
                b.Author.ToLower().Contains(searchText)
            ).ToList();

            DisplayBooks(filtered);
        }

        private void CmbSort_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (allBooks == null || cmbSort.SelectedIndex == -1) return;

            List<Book> sorted = allBooks.ToList();

            switch (cmbSort.SelectedIndex)
            {
                case 0: // А-Я
                    sorted = sorted.OrderBy(b => b.Title).ToList();
                    break;
                case 1: // Я-А
                    sorted = sorted.OrderByDescending(b => b.Title).ToList();
                    break;
                case 2: // По оценке (высокая)
                    sorted = sorted.OrderByDescending(b => b.Rating).ToList();
                    break;
                case 3: // По оценке (низкая)
                    sorted = sorted.OrderBy(b => b.Rating).ToList();
                    break;
            }

            DisplayBooks(sorted);
        }
        private void ShowAddToShelfMenu(Book book)
        {
            var user = MainWindow.CurrentUser;
            if (user == null) return;

            var menu = new ContextMenu();

            string[] shelves = { "В планах", "Читаю", "Прочитано", "Заброшено" };

            foreach (var shelf in shelves)
            {
                var item = new MenuItem { Header = shelf };
                item.Click += (s, e) =>
                {
                    App.DataService.AddBookToShelf(user.Id, book.Id, shelf);
                    MessageBox.Show($"Книга «{book.Title}» добавлена в «{shelf}»", "Успешно");
                };
                menu.Items.Add(item);
            }

            menu.IsOpen = true;
        }
    }
}