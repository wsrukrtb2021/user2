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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.IO;
using System.Data.SqlClient;
using BigPack.Class;
using BigPack.Controls;






namespace BigPack
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        internal void Load_data(string a)
        {
            material_list.Children.Clear(); // Очистка врап панели
            using (SqlConnection connection = new SqlConnection(Conn.String)) // инициализация подключению к бд
            {
                
                connection.Open(); // Открытие подключения к бд
                SqlCommand command = new SqlCommand($@"SELECT [ID]                    
                                                         ,[Title]
                                                         ,[CountInPack]
                                                         ,[Unit]
                                                         ,[CountInStock]
                                                         ,[MinCount]
                                                         ,[Description]
                                                         ,[Cost]
                                                         ,[Image]
                                                         ,[MaterialTypeID]
                                                          FROM [dbo].[Material]" + a, connection); // инициализация запроса к бд

                SqlDataReader reader = command.ExecuteReader(); // чтение строк
                if (reader.HasRows) // проверка наличия строк
                {
                    while (reader.Read()) // цикл для чтения строк и перенос данных в лэйблы
                    {
                        Materials materials = new Materials();
                        materials.ID.Content = reader[0];
                        materials.Title.Content = reader[1];
                        materials.Pack.Content = reader[2];
                        materials.un1.Content = reader[3];
                        materials.un2.Content = reader[3];
                        materials.Stock.Content = reader[4];
                        materials.Min.Content = reader[5];
                        materials.Description.Content = reader[6];
                        materials.Cost.Content = reader[7];
                        materials.image.Source = new BitmapImage(new Uri(Directory.GetCurrentDirectory() + "\\" + reader[8]));
                        materials.MaterialType.Content = reader[9];
                        material_list.Children.Add(materials);
                         
                        

                    }
                }

                
            }
           
        }
        

       

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            Load_data(""); // запуск события чтения бд при запуска приложения
        }
    }
}
