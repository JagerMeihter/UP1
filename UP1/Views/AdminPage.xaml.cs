using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using UP1.Models;
using UP1.Services;

namespace UP1.Views
{
    public partial class AdminPage : Page
    {
        public AdminPage()
        {
            InitializeComponent();
            LoadAllData();
        }

        private void LoadAllData()
        {
            LoadUsers();
            LoadFrozenItems();
            // Заглушки для остальных вкладок
            lbAuthorRequests.Items.Add("Заявка от user2 → Автор (ожидает)");
            lbComplaints.Items.Add("Жалоба на книгу 'Тени прошлого' от user3");
        }

        private void LoadUsers()
        {
            lbUsers.Items.Clear();
            foreach (var user in App.DataService.Users)
            {
                lbUsers.Items.Add($"{user.FullName} ({user.Login}) — {user.Role} {(user.IsFrozen ? "❄️" : "")}");
            }
        }

        private void LoadFrozenItems()
        {
            lbFrozen.Items.Clear();
            var frozenUsers = App.DataService.Users.Where(u => u.IsFrozen);
            foreach (var user in frozenUsers)
            {
                lbFrozen.Items.Add($"👤 {user.FullName} ({user.Login}) — {user.FreezeReason}");
            }

            // Можно добавить замороженные книги позже
        }

        private void BtnChangeRole_Click(object sender, RoutedEventArgs e)
        {
            if (lbUsers.SelectedIndex == -1)
            {
                MessageBox.Show("Выберите пользователя");
                return;
            }

            MessageBox.Show("Здесь будет выбор новой роли (User / Author / Administrator)", "Смена роли");
            // Можно расширить позже
        }

        private void BtnResetPassword_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Пароль сброшен на '12345' (прототип)", "Успешно");
        }

        private void BtnUnfreeze_Click(object sender, RoutedEventArgs e)
        {
            if (lbFrozen.SelectedIndex == -1)
            {
                MessageBox.Show("Выберите элемент для разморозки");
                return;
            }

            MessageBox.Show("Элемент успешно разморожен!", "Разморозка");
            LoadFrozenItems();
        }

        private void BtnAcceptAuthor_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Заявка принята! Пользователь получил роль Автора.", "Успешно");
        }

        private void BtnRejectAuthor_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Заявка отклонена.", "Отклонено");
        }

        private void BtnReviewComplaint_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Жалоба принята к рассмотрению.", "Обработка");
        }

        private void BtnRejectComplaint_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Жалоба отклонена.", "Отклонено");
        }
    }
}