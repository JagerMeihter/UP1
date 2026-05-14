using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using UP1.Models;
using UP1.Services;
using UP1.Windows;

namespace UP1.Views
{
    public partial class BookListsPage : Page
    {
        private string currentShelf = "В планах";

        public BookListsPage()
        {
            InitializeComponent();
            InitializeSortComboBox();
            LoadShelf("В планах");
        }

        private void InitializeSortComboBox()
        {
            cmbSortLists.Items.Clear();
            cmbSortLists.Items.Add("По названию (А-Я)");
            cmbSortLists.Items.Add("По названию (Я-А)");
            cmbSortLists.Items.Add("По оценке (высокая)");
            cmbSortLists.Items.Add("По оценке (низкая)");
            cmbSortLists.SelectedIndex = 0;
        }

        private void LoadShelf(string shelf)
        {
            currentShelf = shelf;
            var user = MainWindow.CurrentUser;
            if (user == null) return;

            var books = App.DataService.GetBooksOnShelf(user.Id, shelf);
            DisplayBooks(books);
        }

        private void DisplayBooks(List<Book> books)
        {
            listsBooksPanel.Children.Clear();

            foreach (var book in books)
            {
                var card = CreateBookCard(book);
                listsBooksPanel.Children.Add(card);
            }

            if (books.Count == 0)
            {
                var tb = new TextBlock
                {
                    Text = "На этой полке пока пусто",
                    Foreground = System.Windows.Media.Brushes.Gray,
                    FontSize = 16,
                    HorizontalAlignment = HorizontalAlignment.Center,
                    Margin = new Thickness(50)
                };
                listsBooksPanel.Children.Add(tb);
            }
        }

        private Border CreateBookCard(Book book)
        {
            var border = new Border
            {
                Width = 170,
                Height = 260,
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
                Foreground = System.Windows.Media.Brushes.White,
                Margin = new Thickness(0, 0, 0, 5)
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

            // Контекстное меню для перемещения
            border.ContextMenu = new ContextMenu();
            string[] shelves = { "В планах", "Читаю", "Прочитано", "Заброшено" };

            foreach (var s in shelves)
            {
                if (s != currentShelf)
                {
                    var mi = new MenuItem { Header = $"Переместить в «{s}»" };
                    mi.Click += (sender, e) => MoveBookToShelf(book, s);
                    border.ContextMenu.Items.Add(mi);
                }
            }

            border.MouseLeftButtonUp += (s, e) => NavigationService.Navigate(new BookDetailsPage(book));

            return border;
        }

        private void MoveBookToShelf(Book book, string newShelf)
        {
            var user = MainWindow.CurrentUser;
            if (user == null) return;

            App.DataService.AddBookToShelf(user.Id, book.Id, newShelf);
            LoadShelf(currentShelf); // обновляем текущую полку
        }

        // Обработчики кнопок вкладок
        private void BtnPlan_Click(object sender, RoutedEventArgs e) => LoadShelf("В планах");
        private void BtnReading_Click(object sender, RoutedEventArgs e) => LoadShelf("Читаю");
        private void BtnFinished_Click(object sender, RoutedEventArgs e) => LoadShelf("Прочитано");
        private void BtnDropped_Click(object sender, RoutedEventArgs e) => LoadShelf("Заброшено");

        private void TxtSearchLists_TextChanged(object sender, TextChangedEventArgs e)
        {
            // Поиск будет работать только по текущей полке
            // Можно улучшить позже
        }
    }
}