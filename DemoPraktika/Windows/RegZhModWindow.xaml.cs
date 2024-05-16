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
using DemoPraktika.DataBase;
using Microsoft.Win32;

namespace DemoPraktika.Windows
{
    /// <summary>
    /// Логика взаимодействия для RegZhModWindow.xaml
    /// </summary>
    public partial class RegZhModWindow : Window
    {
        private string imgName = "";
        bool passincor = false;
        User usOrg = new User();
        public RegZhModWindow(User _userOrg)
        {
            InitializeComponent();
            try
            {
                usOrg = _userOrg;
                using (MerContext db = new MerContext())
                {
                    IList<User> user = db.Users.ToList();
                    var lastUser = user.OrderByDescending(u => u.Id).FirstOrDefault();
                    if (lastUser != null)
                    {
                        int newId = lastUser.Id +1;
                        idTb.Text = newId.ToString();
                    }
                    var eventNames = db.Directions.Select(e => e.DirectionName).ToList();
                    directionCb.ItemsSource = eventNames;
                    var activNames = db.Activityies.Select(e => e.ActivityName).ToList();
                    activCb.ItemsSource = activNames;
                }
            }
            catch { MessageBox.Show("Ошибка"); }
        }

        private void SaveNewUser(int _id, string _fio, string email, DateTime _birthdate, int _genderid, string _phone, string _password, string _photo, int _countryid, int _roleid, int _directionid, int _activid)
        {
            using (MerContext _con = new MerContext())
            {
                User newUser = new User
                {
                    Id = _id,
                    FullName = _fio,
                    Email = email,
                    BirthDay = _birthdate,
                    GenderId = _genderid,
                    Phone = _phone,
                    Password = _password,
                    Photo = _photo,
                    CountryId = _countryid,
                    RoleId = _roleid,
                    DirectionId = _directionid
                };
                _con.Users.Add(newUser);
                try
                {
                    _con.SaveChanges();
                    if (_roleid == 4)
                    {
                        int lastId = _con.UserJuryActivities.OrderByDescending(u => u.Id).Select(u => u.Id).FirstOrDefault();
                        UserJuryActivity newUserJuryActivity = new UserJuryActivity
                        {
                            Id = lastId + 1,
                            IdUser = _id,
                            IdActivity = _activid
                        };
                        _con.UserJuryActivities.Add(newUserJuryActivity);
                        _con.SaveChanges();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Ошибка при сохранении пользователя в базе данных: " + ex.Message);
                }
            } 
        }


        private void btnOk_Click(object sender, RoutedEventArgs e)
        {
            if (passTb.Text == repassTb.Text)
            {
                try
                {
                    int roleid=3;
                    if (roleCb.SelectedItem != null)
                    {
                        string selectedRole = ((ComboBoxItem)roleCb.SelectedItem).Content.ToString();
                        switch (selectedRole)
                        {
                            case "Модератор":
                                roleid = 3;
                                break;
                            case "Жюри":
                                roleid = 4;
                                break;
                        }
                    }
                    else
                    {
                        MessageBox.Show("Выберите роль");
                    }
                    int genderid=1;
                    if (genCb.SelectedItem != null)
                    {
                        string selectedGender = ((ComboBoxItem)genCb.SelectedItem).Content.ToString();
                        switch (selectedGender)
                        {
                            case "Мужской":
                                genderid = 1;
                                break;
                            case "Женский":
                                genderid = 2;
                                break;
                        }
                    }
                    else
                    {
                        MessageBox.Show("Укажите пол");
                    }
                    int eventid=1;
                    if (directionCb.SelectedItem != null)
                    {
                        using (MerContext context = new MerContext())
                        {
                            eventid = context.Directions
                                            .Where(d => d.DirectionName == directionCb.SelectedItem.ToString())
                                            .Select(d => (int)d.Id)
                                            .FirstOrDefault();
                        }
                    }
                    else { MessageBox.Show("Введите направление"); }
                    int activid = 1;
                    if (activCb.SelectedItem != null)
                    {
                        using (MerContext context = new MerContext())
                        {
                            activid = context.Activityies
                                            .Where(d => d.ActivityName == activCb.SelectedItem.ToString())
                                            .Select(d => (int)d.Id)
                                            .FirstOrDefault();
                        }
                    }
                    else { MessageBox.Show("Введите мероприятие"); }
                    int id = Convert.ToInt32(idTb.Text);
                    var fio = fioTb.Text;
                    string email = emailTb.Text;
                    DateTime date = new DateTime();
                    int country = 1;
                    string phone = phoneTb.Text;
                    if (!(phoneTb as Xceed.Wpf.Toolkit.MaskedTextBox).IsMaskCompleted)
                    {
                        MessageBox.Show("Введите номер телефона");
                        return;
                    }

                    if (passincor == false)
                    {
                        MessageBox.Show("Пароль не соответствует требованиям");
                    }
                    else
                    {
                        string password = passTb.Text;
                        string confirmPassword = repassTb.Text;

                        SaveNewUser(id, fio, email, date, genderid, phone, password, imgName, country, roleid, eventid, activid);
                    }
                    MessageBox.Show("Пользователь зарегистрирован.");
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Произошла ошибка при регистрации: " + ex.Message);
                }
            }
            else
            {
                MessageBox.Show("Пароли не совпадают");
            }
        }

        private void passPb_LostFocus(object sender, RoutedEventArgs e)
        {
            string password = passTb.Text;
            int res = CheckPassword(password);
            if (res == 1 )
            {
                MessageBox.Show("Пароль должен содержать не менее 6 символов");
            }
            else if (res == 2)
            {
                MessageBox.Show("Пароль должен содержать заглавные и строчные буквы");
            }            
            else if (res == 3)
            {
                MessageBox.Show("Пароль должен содержать не менее одной цифры");
            }
            else if (res == 4)
            {
                MessageBox.Show("Пароль должен содержать не менее одного спецсимвола");
            }
        }

        public int CheckPassword(string password)
        {
            try
            {
                if (password.Length < 6)
                {
                    passincor = false;
                    return 1;
                }
                else if (!password.Any(char.IsUpper) || !password.Any(char.IsLower))
                {
                    passincor = false;
                    return 2;
                }
                else if (!password.Any(char.IsDigit))
                {
                    passincor = false;
                    return 3;
                }
                else if (!password.Any(c => !char.IsLetterOrDigit(c)))
                {
                    
                    passincor = false;
                    return 4;
                }
                else
                {
                    passincor = true;
                }
            }
            catch { MessageBox.Show("Ошибка"); }
            return 5;
        }
        private void passchb_Checked(object sender, RoutedEventArgs e)
        {
            passTb.Visibility = Visibility.Visible;
            repassTb.Visibility = Visibility.Visible;
            passPb.Visibility = Visibility.Collapsed;
            repassPb.Visibility = Visibility.Collapsed;
            passTb.Text = passPb.Password;
            repassTb.Text = repassPb.Password;
        }

        private void passchb_Unchecked(object sender, RoutedEventArgs e)
        {
            passTb.Visibility = Visibility.Collapsed;
            repassTb.Visibility = Visibility.Collapsed;
            passPb.Visibility = Visibility.Visible;
            repassPb.Visibility = Visibility.Visible;
            passPb.Password = passTb.Text;
            repassPb.Password = repassTb.Text;
        }


        private void emailTb_LostFocus(object sender, RoutedEventArgs e)
        {
            System.Windows.Controls.TextBox textBox = (System.Windows.Controls.TextBox)sender;
            string text = emailTb.Text;

            if (text.Contains("@")) { }
            else
            {
                MessageBox.Show("Email должен содержать символ @");
            }
        }
        private void userImg_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Image files (*.png;*.jpeg;*.jpg)|*.png;*.jpeg;*.jpg|All files (*.*)|*.*";

            if (openFileDialog.ShowDialog() == true)
            {
                string selectedFilePath = openFileDialog.FileName;
                imgName = System.IO.Path.GetFileName(selectedFilePath);
                userImg.Source = new BitmapImage(new Uri(selectedFilePath));
               
            }
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            OrgWindow ow = new OrgWindow(usOrg);
            ow.Show();
            Close();
        }
        private void passPb_PasswordChanged(object sender, RoutedEventArgs e)
        {
            passTb.Text = passPb.Password;
        }
    }
}
