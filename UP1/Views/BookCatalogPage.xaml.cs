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
        private List<Book> allBooks = new List<Book>();
        private List<string> selectedGenres = new List<string>();

        public BookCatalogPage()
        {
            InitializeComponent();        // ← Это должно быть первым!
            LoadBooks();
            CreateGenreFilters();
        }

        private void LoadBooks()
        {
            allBooks = App.DataService.Books;
            DisplayBooks(allBooks);
        }

        private void CreateGenreFilters()
        {
            if (genresPanel == null) return;

            genresPanel.Children.Clear();
            var allGenres = allBooks.SelectMany(b => b.Genres).Distinct().ToList();

            foreach (var genre in allGenres)
            {
                var checkBox = new CheckBox
                {
                    Content = genre,
                    Margin = new Thickness(8, 0, 8, 0),
                    Foreground = System.Windows.Media.Brushes.White,
                    FontSize = 14
                };
                checkBox.Checked += Genre_Checked;
                checkBox.Unchecked += Genre_Unchecked;
                genresPanel.Children.Add(checkBox);
            }
        }

        private void DisplayBooks(List<Book> books)
        {
            if (booksPanel == null) return;   // ← Защита от null

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
                Foreground = System.Windows.Media.Brushes.LightGray
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

            border.MouseLeftButtonUp += (s, e) =>
            {
                if (s != addButton)
                    NavigationService.Navigate(new BookDetailsPage(book));
            };

            addButton.Click += (s, e) => ShowAddToShelfMenu(book);

            return border;
        }

        private void TxtSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            ApplyFilters();
        }

        private void CmbSort_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ApplyFilters();
        }

        private void Genre_Checked(object sender, RoutedEventArgs e)
        {
            var cb = sender as CheckBox;
            if (cb != null && !selectedGenres.Contains(cb.Content.ToString()))
                selectedGenres.Add(cb.Content.ToString());

            ApplyFilters();
        }

        private void Genre_Unchecked(object sender, RoutedEventArgs e)
        {
            var cb = sender as CheckBox;
            if (cb != null)
                selectedGenres.Remove(cb.Content.ToString());

            ApplyFilters();
        }

        private void ApplyFilters()
        {
            if (allBooks == null || txtSearch == null || cmbSort == null) return;

            var searchText = txtSearch.Text.ToLower();

            var filtered = allBooks.Where(b =>
            {
                bool matchSearch = string.IsNullOrEmpty(searchText) ||
                    b.Title.ToLower().Contains(searchText) ||
                    b.Author.ToLower().Contains(searchText);

                bool matchGenre = selectedGenres.Count == 0 ||
                    b.Genres.Any(g => selectedGenres.Contains(g));

                return matchSearch && matchGenre;
            }).ToList();

            // Сортировка
            switch (cmbSort.SelectedIndex)
            {
                case 0: filtered = filtered.OrderBy(b => b.Title).ToList(); break;
                case 1: filtered = filtered.OrderByDescending(b => b.Title).ToList(); break;
                case 2: filtered = filtered.OrderByDescending(b => b.Rating).ToList(); break;
                case 3: filtered = filtered.OrderBy(b => b.Rating).ToList(); break;
            }

            DisplayBooks(filtered);
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
                    MessageBox.Show($"Книга добавлена в «{shelf}»", "Успешно");
                };
                menu.Items.Add(item);
            }

            menu.IsOpen = true;
        }
    }
}