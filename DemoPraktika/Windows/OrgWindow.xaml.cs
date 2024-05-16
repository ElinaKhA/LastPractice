using DemoPraktika.DataBase;
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

namespace DemoPraktika.Windows
{
    /// <summary>
    /// Логика взаимодействия для OrgWindow.xaml
    /// </summary>
    public partial class OrgWindow : Window
    {
        User usforreg;
        public OrgWindow(User _user)
        {
            InitializeComponent();
            try
            {
                usforreg = _user;
                var time = Convert.ToInt32(DateTime.Now.ToString("HH:mm:ss").Split(':')[0]);
                if (time >= 9 && time <= 11) timeTb.Content = "Доброе утро";
                else if (time > 11 && time <= 18) timeTb.Content = "Добрый день";
                else timeTb.Content = "Добрый вечер";
                if (_user.GenderId == 1) IOTb.Content = $"Mr {_user.FullName}";
                else IOTb.Content = $"Mrs {_user.FullName}";
                string imagePath = $"C:/Users/khami/OneDrive/Рабочий стол/DemoPr/DemoPraktika/DemoPraktika/Resources/{_user.Photo}";
                userImg.Source = new BitmapImage(new Uri(imagePath));

            }
            catch { MessageBox.Show("Ошибка"); }
           
        }

        private void btnMer_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btnUch_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btnZhur_Click(object sender, RoutedEventArgs e)
        {
            RegZhModWindow rw = new RegZhModWindow(usforreg);
            rw.Show();
            Close();
        }

        private void btnProf_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btnExit_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mw = new MainWindow();
            mw.Show();
            Close();
        }
    }
}
