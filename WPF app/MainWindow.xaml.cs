using System;
using System.Collections.Generic;
using System.Data;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using Microsoft.Data.SqlClient;

namespace Взаимодействие_приложений_с_Базой_Данных_при_помощи_Microsoft_2
{
    public partial class MainWindow : Window
    {
        public static string connectionString = "Server=sql.bsite.net\\MSSQL2016;Database=cerberus_;User Id=cerberus_;Password=1351925zxc!;TrustServerCertificate=true;";


        public MainWindow()
        {
            InitializeComponent();
            LoadCategories();
        }


        private void LoadCategories()
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    using (SqlCommand command = new SqlCommand("SELECT * FROM Category", connection))
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        DataTable dt = new DataTable();
                        dt.Load(reader);
                        dataGridCategories.ItemsSource = dt.DefaultView;
                    }
                }
            }
            catch (SqlException ex)
            {
                MessageBox.Show($"Ошибка загрузки данных: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }


        private void btnAddCategory_Click(object sender, RoutedEventArgs e)
        {
            AddCategoryWindow addWindow = new AddCategoryWindow(connectionString);
            if (addWindow.ShowDialog() == true)
            {
                LoadCategories();
            }
        }


        private void btnUpdateCategory_Click(object sender, RoutedEventArgs e)
        {
            if (dataGridCategories.SelectedItem == null)
            {
                MessageBox.Show("Выберите категорию для обновления.");
                return;
            }
            DataRowView selectedCategory = (DataRowView)dataGridCategories.SelectedItem;
            int categoryId = (int)selectedCategory["CategoryID"];
            string categoryName = (string)selectedCategory["Name"];
            UpdateCategoryWindow updateWindow = new UpdateCategoryWindow(connectionString, categoryId, categoryName);
            if (updateWindow.ShowDialog() == true)
            {
                LoadCategories();
            }
        }

        private void btnDeleteCategory_Click(object sender, RoutedEventArgs e)
        {
            if (dataGridCategories.SelectedItem == null)
            {
                MessageBox.Show("Выберите категорию для удаления.");
                return;
            }
            DataRowView row = (DataRowView)dataGridCategories.SelectedItem;
            int categoryId = (int)row["CategoryID"];
            if (MessageBox.Show($"Удалить категорию с ID {categoryId}?", "Подтверждение", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                DeleteCategory(categoryId);
                LoadCategories();
            }
        }

        private void dataGridCategories_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            btnUpdateCategory.IsEnabled = dataGridCategories.SelectedItem != null;
            btnDeleteCategory.IsEnabled = dataGridCategories.SelectedItem != null;
        }

        private void DeleteCategory(int categoryId)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string sql = "DELETE FROM Category WHERE CategoryID = @CategoryID";
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@CategoryID", categoryId);
                        command.ExecuteNonQuery();
                    }
                }
            }
            catch (SqlException ex)
            {
                MessageBox.Show($"Ошибка удаления категории: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }


    }
}