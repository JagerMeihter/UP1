using System.Linq;
using System.Windows;
using UP1.Models;
using UP1.Services;

namespace UP1.Views
{
    public partial class EditBookWindow : Window
    {
        private Book currentBook;

        public EditBookWindow(Book book)
        {
            InitializeComponent();
            currentBook = book;
            LoadBookData();
        }

        private void LoadBookData()
        {
            txtTitle.Text = currentBook.Title;
            cmbGenre.Items.Add("Фантастика"); cmbGenre.Items.Add("Детектив");
            cmbGenre.Items.Add("Романтика"); cmbGenre.Items.Add("Поэзия");
            cmbGenre.Text = currentBook.Genres?.FirstOrDefault() ?? "Фантастика";
            txtDescription.Text = currentBook.Description;
            txtText.Text = currentBook.Text;
        }

        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtTitle.Text))
            {
                MessageBox.Show("Название обязательно!");
                return;
            }

            currentBook.Title = txtTitle.Text;
            currentBook.Description = txtDescription.Text;
            currentBook.Text = txtText.Text;
            currentBook.Genres = new System.Collections.Generic.List<string> { cmbGenre.Text };

            App.DataService.SaveBooks();

            MessageBox.Show("Книга успешно обновлена!", "Успех");
            this.DialogResult = true;
            this.Close();
        }
    }
}