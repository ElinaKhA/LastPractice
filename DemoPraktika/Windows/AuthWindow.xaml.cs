using DemoPraktika.DataBase;
using EasyCaptcha.Wpf;
using System;
using System.Linq;
using System.Windows;
using System.Windows.Threading;

namespace DemoPraktika.Windows
{
    /// <summary>
    /// Логика взаимодействия для AuthWindow.xaml
    /// </summary>
    public partial class AuthWindow : Window
    {
       // MerContext _con = new MerContext();
        int baddatacount = 0;

        public AuthWindow()
        {
            InitializeComponent();
            UniformGridCaptcha.Visibility = Visibility.Hidden;
          
        }

        private void BtnAuth_Click(object sender, RoutedEventArgs e)
        {
            // Вызываем метод авторизации при клике на кнопку
            Authenticate(tbLogin.Text, tbPassword.Text);
        }

        // Метод для авторизации
        public bool Authenticate(string login, string password)
        {
            try
            {
                using (MerContext _con = new MerContext())
                {
                    if (_con.Users.Any(u => u.Id.ToString() == login && u.Password == password && u.RoleId == 1))
                    {
                        User SelectedUser = _con.Users.FirstOrDefault(u => u.Id.ToString() == login && u.Password == password && u.RoleId == 1);
                        UchastnikWindow ow = new UchastnikWindow();
                        ow.Show();
                        Close();
                        return true; // Возвращаем true при успешной аутентификации
                    }
                    else if (_con.Users.Any(u => u.Id.ToString() == login && u.Password == password && u.RoleId == 2))
                    {
                        User SelectedUser = _con.Users.FirstOrDefault(u => u.Id.ToString() == login && u.Password == password && u.RoleId == 2);
                        OrgWindow ow = new OrgWindow(SelectedUser);
                        ow.Show();
                        Close();
                        return true; // Возвращаем true при успешной аутентификации
                    }
                    else if (_con.Users.Any(u => u.Id.ToString() == login && u.Password == password && u.RoleId == 3))
                    {
                        User SelectedUser = _con.Users.FirstOrDefault(u => u.Id.ToString() == login && u.Password == password && u.RoleId == 3);
                        ModWindow ow = new ModWindow();
                        ow.Show();
                        Close();
                        return true; // Возвращаем true при успешной аутентификации
                    }
                    else if (_con.Users.Any(u => u.Id.ToString() == login && u.Password == password && u.RoleId == 4))
                    {
                        User SelectedUser = _con.Users.FirstOrDefault(u => u.Id.ToString() == login && u.Password == password && u.RoleId == 4);
                        ZhurWindow ow = new ZhurWindow();
                        ow.Show();
                        Close();
                        return true; // Возвращаем true при успешной аутентификации
                    }
                    else
                    {
                        MessageBox.Show("Такого пользователя не существует");
                        baddatacount++;
                        if (baddatacount > 3)
                        {
                            tbLogin.IsEnabled = false;
                            tbPassword.IsEnabled = false;
                            MessageBox.Show("Такого пользователя не существует. Повторите попытку через 10 секунд");
                            DispatcherTimer timer = new DispatcherTimer();
                            timer.Interval = TimeSpan.FromSeconds(10);
                            timer.Tick += Timer_Tick;
                            timer.Start();
                        }
                        return false; // Возвращаем false при неудачной аутентификации
                    }
                }
            }
            catch
            {
                MessageBox.Show("Ошибка");
                return false; // Возвращаем false в случае возникновения исключения
            }
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            baddatacount = 0;
            DispatcherTimer timer = (DispatcherTimer)sender;
            timer.Stop();
            MessageBox.Show("Введите капчу");
            MyCaptcha.CreateCaptcha(Captcha.LetterOption.Alphanumeric, 4);
            UniformGridCaptcha.Visibility = Visibility.Visible;
        }

        private void BtnAcceptCaptcha_Click(object sender, RoutedEventArgs e)
        {
            var answer = MyCaptcha.CaptchaText;
            if (answer == tbCaptcha.Text)
            {
                UniformGridCaptcha.Visibility = Visibility.Hidden;
                tbLogin.IsEnabled = true;
                tbPassword.IsEnabled = true;
            }
            else
            {
                MessageBox.Show("Ошибка. введите капчу снова");
                tbCaptcha.Text = "";
                MyCaptcha.CreateCaptcha(Captcha.LetterOption.Alphanumeric, 4);
            }
        }

        private void BtnGuest_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mw = new MainWindow();
            mw.Show();
            Close();
        }
    }
}
