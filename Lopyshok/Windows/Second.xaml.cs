using Lopyshok.Classes;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
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

namespace Lopyshok.Windows
{

    public partial class Second : Window
    {
        public Second()
        {
            InitializeComponent();
        }
        public Main Main;
        private string file_Name = "";

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            Main.Show();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            using (SqlConnection connection = new SqlConnection(Connection.String))
            {
                connection.Open();
                SqlCommand command = new SqlCommand($@"SELECT [Title] FROM [user2].[dbo].[ProductType]", connection);
                SqlDataReader reader = command.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        Type.Items.Add(reader[0].ToString());
                    }
                }
            }
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            if (ID.Text.ToString() != "ID")
            {
                using (SqlConnection connection = new SqlConnection(Connection.String))
                {
                    connection.Open();
                    SqlCommand command = new SqlCommand($@"UPDATE [dbo].[Product]
                                                           SET ArticleNumber = {Article.Text},
                                                               Title = '{Name.Text}',
                                                               ProductTypeID = (SELECT ID FROM [dbo].[ProductType] WHERE Title = '{Type.SelectedItem}'),
                                                               ProductionPersonCount = {Person.Text},
                                                               ProductionWorkshopNumber = {Number.Text},
                                                               MinCostForAgent = {Minimum.Text.Replace(',','.')},
                                                               Description = '{Description.Text}'
                                                               {(file_Name != "" ? @",[Image] = 'products\" + file_Name + "' " : "")}
                                                           WHERE ID = {ID.Text}", connection);
                    try
                    {
                        command.ExecuteNonQuery();

                        
                        if (MessageBox.Show("Сохранить? ", "Сохранение", MessageBoxButton.YesNo, MessageBoxImage.Information) == MessageBoxResult.Yes)
                        {
                            MessageBox.Show("Успешно ", "Сохранение", MessageBoxButton.OK, MessageBoxImage.Information);
                        }
                        else
                        { 
                        
                        }
                        Main.Load_data("");
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.ToString());
                        MessageBox.Show("Пожалуйста проверьте правильность заполненных данных", "Ошибка заполнения", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }

            else
            {
                using (SqlConnection connection = new SqlConnection(Connection.String))
                {
                    connection.Open();
                    SqlCommand command = new SqlCommand($@"INSERT INTO [dbo].[Product] 
                                                                            ([ArticleNumber]
                                                                            ,[Title]
                                                                            ,[ProductTypeID]
                                                                            ,[ProductionPersonCount]
                                                                            ,[ProductionWorkshopNumber]
                                                                            ,[MinCostForAgent]
                                                                            ,[Description]
                                                                            ,[Image])
                                                                            VALUES 
                                                                           ( {Article.Text}
                                                                            ,'{Name.Text}'
                                                                            ,{(Type.SelectedIndex +1).ToString()}
                                                                            ,{Person.Text}
                                                                            ,{Number.Text}
                                                                            ,{Minimum.Text}
                                                                            ,'{Description.Text}'
                                                                            ,{(file_Name != "" ? @"'products\" + file_Name + "'" : "'products\\picture.jpg'")})", connection);

                    try
                    {
                        if (MessageBox.Show("Сохранить? ", "Сохранение", MessageBoxButton.YesNo, MessageBoxImage.Information) == MessageBoxResult.Yes)
                        {
                            command.ExecuteNonQuery();
                            MessageBox.Show("Успешно ", "Сохранение", MessageBoxButton.OK, MessageBoxImage.Information);
                        }
                        else
                        {

                        }
                        
                        Main.Load_data("");
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.ToString());
                        MessageBox.Show("Пожалуйста проверьте правильность заполненных данных", "Ошибка заполнения", MessageBoxButton.OK, MessageBoxImage.Error);


                    }
                }
            }
        }

        private void Photo_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            OpenFileDialog saveFile = new OpenFileDialog();
            saveFile.ShowDialog();
            if (saveFile.FileName != "")
            {
                Photo.Source = new BitmapImage(new Uri(saveFile.FileName));
                try
                {
                    File.Copy(saveFile.FileName, @".\products\" + saveFile.SafeFileName, true);
                    file_Name = saveFile.SafeFileName;
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.ToString());
                    MessageBox.Show("Выберите другую фотку!");
                }

            }
        }
    }
}
