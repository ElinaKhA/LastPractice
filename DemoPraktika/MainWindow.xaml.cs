using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using DemoPraktika.Windows;
using DemoPraktika.DataBase;
using Xceed.Wpf.Toolkit.PropertyGrid.Attributes;


namespace DemoPraktika
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public ObservableCollection<EventsData> data { get; set; }
        public ObservableCollection<string> EventNames { get; set; }

        public class EventsData
        {
            public BitmapImage Logo { get; set; }
            public string Name { get; set; }
            public string Direction { get; set; }
            public DateTime Date { get; set; }
        }
        public MainWindow()
        {
            InitializeComponent();
            this.data = new ObservableCollection<EventsData>();
            this.EventNames = new ObservableCollection<string>();
            LoadEventNames(); 
            Load();  
            this.DataContext = this;  
        }

        private void LoadEventNames()
        {
            var db = new MerContext();  
            var eventNamesQuery = db.Events.Select(e => e.EventName).Distinct().ToList();
            EventNames.Clear();
            EventNames.Add("Все"); 
            foreach (var name in eventNamesQuery)
            {
                EventNames.Add(name);
            }
        }

        private void BtnGuest_Click(object sender, RoutedEventArgs e)
        {
            AuthWindow aw = new AuthWindow();
            aw.Show();
            Close();
        }


        private void Load(DateTime? startDateFilter = null, string eventNameFilter = null)
        {
            try
            {
                data.Clear();
                var db = new MerContext();
                var query = from ev in db.Events
                            join act in db.Activityies on ev.Id equals act.IdEvent
                            select new { Event = ev, Activity = act };
                if (startDateFilter.HasValue)
                {
                    query = query.Where(x => x.Event.StartDate == startDateFilter.Value);
                }
                if (!string.IsNullOrEmpty(eventNameFilter))
                {
                    query = query.Where(x => x.Event.EventName == eventNameFilter);
                }
                foreach (var item in query.ToList())
                {
                    BitmapImage img = LoadImage(item.Activity.Id);
                    if (img != null)
                    {
                        this.data.Add(new EventsData()
                        {
                            Name = item.Activity.ActivityName.Trim(),
                            Date = item.Event.StartDate,
                            Direction = item.Event.EventName,
                            Logo = img
                        });
                    }
                }
            }
            catch { MessageBox.Show("Ошибка"); }

        }

        private BitmapImage LoadImage(int activityId)
        {
            string[] extensions = { ".jpg", ".jpeg", ".png" };
            foreach (var ext in extensions)
            {
                var imgPath = $"C:/Users/khami/OneDrive/Рабочий стол/DemoPr/DemoPraktika/DemoPraktika/Resources/{activityId}{ext}";
                try
                {
                    BitmapImage img = new BitmapImage();
                    img.BeginInit();
                    img.UriSource = new Uri(imgPath, UriKind.Absolute);
                    img.CacheOption = BitmapCacheOption.OnLoad;
                    img.EndInit();
                    if (img.Width > 0)
                        return img;
                }
                catch {  }
            }
            return null;
        }



        private void DateFilter_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                if (DateFilter.SelectedDate.HasValue)
                {
                    Load(DateFilter.SelectedDate.Value);
                }
                else
                {
                    Load();
                }
            }
            catch { MessageBox.Show("Ошибка"); }
        }

        private void DirectionFilter_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                if (DirectionFilter.SelectedItem != null)
                {
                    var selectedEventName = DirectionFilter.SelectedItem as string;
                    if (!string.IsNullOrEmpty(selectedEventName) && selectedEventName != "Все")
                    {
                        Load(eventNameFilter: selectedEventName);
                    }
                    else
                    {
                        Load();
                    }
                }
            }
            catch { MessageBox.Show("Ошибка"); }
        }
    }
}
