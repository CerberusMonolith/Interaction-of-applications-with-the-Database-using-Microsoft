using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Data.SqlClient;

namespace Взаимодействие_приложений_с_Базой_Данных_при_помощи_Microsoft_2
{
    public partial class UpdateCategoryWindow : Window
    {
        private string connectionString;
        private int categoryId;

        public UpdateCategoryWindow(string connectionString, int categoryId, string currentName)
        {
            InitializeComponent();
            this.connectionString = connectionString;
            this.categoryId = categoryId;
            txtCategoryID.Text = categoryId.ToString();
            txtName.Text = currentName;
        }

        private void btnUpdate_Click(object sender, RoutedEventArgs e)
        {
            string name = txtName.Text.Trim();
            if (string.IsNullOrEmpty(name))
            {
                MessageBox.Show("Введите название категории.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    using (SqlCommand command = new SqlCommand("UPDATE Category SET Name = @Name WHERE CategoryID = @CategoryID", connection))
                    {
                        command.Parameters.AddWithValue("@Name", name);
                        command.Parameters.AddWithValue("@CategoryID", categoryId);
                        command.ExecuteNonQuery();
                    }
                }
                DialogResult = true;
                Close();
            }
            catch (SqlException ex)
            {
                MessageBox.Show($"Ошибка обновления категории: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}