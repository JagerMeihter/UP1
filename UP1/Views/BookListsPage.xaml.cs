using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using UP1.Models;
using UP1.Services;

namespace UP1.Views
{
    public partial class BookListsPage : Page
    {
        private List<Book> currentListBooks = new List<Book>();
        private string currentShelf = "В планах"; // текущая полка

        public BookListsPage()
        {
            InitializeComponent();
            InitializeSortComboBox();
            LoadShelf("В планах");
        }

        private void InitializeSortComboBox()
        {
            cmbSortLists.Items.Add("По названию (А-Я)");
            cmbSortLists.Items.Add("По названию (Я-А)");
            cmbSortLists.Items.Add("По оценке (высокая)");
            cmbSortLists.Items.Add("По оценке (низкая)");
            cmbSortLists.SelectedIndex = 0;
        }

        private void LoadShelf(string shelf)
        {
            currentShelf = shelf;
            // Пока показываем все книги (в будущем будет разделение по полкам)
            currentListBooks = App.DataService.Books.ToList();
            DisplayBooks(currentListBooks);
        }

        private void DisplayBooks(List<Book> books)
        {
            listsBooksPanel.Children.Clear();

            foreach (var book in books)
            {
                var card = CreateBookCard(book);
                listsBooksPanel.Children.Add(card);
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
            var author = new TextBlock
            {
                Text = book.Author,
                TextAlignment = TextAlignment.Center,
                Foreground = System.Windows.Media.Brushes.LightGray
            };

            stack.Children.Add(cover);
            stack.Children.Add(title);
            stack.Children.Add(author);

            border.Child = stack;

            border.MouseLeftButtonUp += (s, e) =>
            {
                NavigationService.Navigate(new BookDetailsPage(book));
            };

            return border;
        }

        private void BtnPlan_Click(object sender, RoutedEventArgs e) => LoadShelf("В планах");
        private void BtnReading_Click(object sender, RoutedEventArgs e) => LoadShelf("Читаю");
        private void BtnFinished_Click(object sender, RoutedEventArgs e) => LoadShelf("Прочитано");
        private void BtnDropped_Click(object sender, RoutedEventArgs e) => LoadShelf("Заброшено");

        private void TxtSearchLists_TextChanged(object sender, TextChangedEventArgs e)
        {
            var filtered = currentListBooks.Where(b =>
                b.Title.ToLower().Contains(txtSearchLists.Text.ToLower()) ||
                b.Author.ToLower().Contains(txtSearchLists.Text.ToLower())
            ).ToList();

            DisplayBooks(filtered);
        }

        private void CmbSortLists_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // Простая сортировка (можно улучшить позже)
            DisplayBooks(currentListBooks);
        }
    }
}