using ProjektZespolowy.Models;
using ProjektZespolowy.Models.DTO;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
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

namespace ProjektZespolowy.Desktop.Pages
{
    public partial class ActivitiesPage : Page
    {
        public class PlaceBasicInfo
        {
            public int Id { get; set; }
            public string Name { get; set; }
            public string Address { get; set; }
            public string EstimatedCost { get; set; }
            public string HoursOpen { get; set; }
            public string TimeSpent { get; set; }
            public string ImgSrc { get; set; }
        }



        List<Place> m_Places = new List<Place>();
        List<PlaceBasicInfo> m_PlacesBinding = new List<PlaceBasicInfo>();



        public ActivitiesPage(int estimatedCost, int averageTimeSpentSort, int nameSort, int crowdCount)
        {
        }


        public ActivitiesPage()
        {
            InitializeComponent();
            MainWindow window = (MainWindow)Application.Current.MainWindow;
            if (window.GetType() == typeof(MainWindow))
            {
                Configuration.m_MenuVisible = Visibility.Visible;
                (window as MainWindow).m_menu.Visibility = Visibility.Visible;
                (window as MainWindow).m_ActivePage = MainWindow.ActivePage.Activities;
            }

            HttpClient client = new HttpClient();
            HttpResponseMessage response = client.GetAsync("https://localhost:44441/api/Places").Result;
            if (response.IsSuccessStatusCode)
            {
                var placesResp = response.Content.ReadFromJsonAsync<IEnumerable<Place>>().Result;
                if (placesResp != null)
                {
                    m_Places = new List<Place>(placesResp);
                }
            }
            else
            {
                MessageBox.Show("Incorrect response for url: \"https://localhost:7118/api/Places\" is: \"" + response.IsSuccessStatusCode.ToString() + '"');
            }


            m_PlacesBinding = preparePlacesInf(m_Places);

            m_ActivitiesList.Items.Clear();
            m_ActivitiesList.ItemsSource = m_PlacesBinding;
        }

        private void PlaceholdersListBox_OnPreviewMouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            var item = ItemsControl.ContainerFromElement(sender as ListBox, e.OriginalSource as DependencyObject) as ListBoxItem;
            if (item != null)
            {
                PlaceBasicInfo placeBI = (PlaceBasicInfo)item.Content;
                this.NavigationService.Navigate(new SelectedActivityPage(placeBI.Id));
            }
        }
        
        private void ComboBox_SelectionChanged(object sender, RoutedEventArgs e)
        {
            ComboBox cmb = sender as ComboBox;
            int sel = cmb.SelectedIndex;

            switch (sel)
            {
                case 0:
                    m_PlacesBinding.Sort((a, b) => a.EstimatedCost.CompareTo(b.EstimatedCost));
                    m_ActivitiesList.Items.Refresh();
                    break;
                case 1:
                    m_PlacesBinding.Sort((a, b) => a.EstimatedCost.CompareTo(b.EstimatedCost));
                    m_PlacesBinding.Reverse();
                    m_ActivitiesList.Items.Refresh();
                    break;
                case 2:
                    m_PlacesBinding.Sort((a, b) => a.TimeSpent.CompareTo(b.TimeSpent));
                    m_ActivitiesList.Items.Refresh();
                    break;
                case 3:
                    m_PlacesBinding.Sort((a, b) => a.TimeSpent.CompareTo(b.TimeSpent));
                    m_PlacesBinding.Reverse();
                    m_ActivitiesList.Items.Refresh();
                    break;
                case 4:
                    m_PlacesBinding.Sort((a, b) => a.Name.CompareTo(b.Name));
                    m_ActivitiesList.Items.Refresh();
                    break;
                default:
                    break;
            }
        }

        public List<PlaceBasicInfo> preparePlacesInf(List<Place> places)
        {
            List<PlaceBasicInfo> newList = new List<PlaceBasicInfo>();
            foreach (var place in places)
            {
                var newplace = new PlaceBasicInfo
                {
                    Id = place.Id,
                    Address = place.Address,
                    Name = place.Name
                };

                newplace.EstimatedCost = place.EstimatedCost.ToString();

                int hour, minutes;
                if (place.PlaceAvailabilityTime != null)
                {
                    DateTime dt = new DateTime();
                    dt = DateTime.Now;
                    if (dt.DayOfWeek == DayOfWeek.Sunday)
                    {
                        hour = place.PlaceAvailabilityTime.OpeningSunday / 60;
                        minutes = place.PlaceAvailabilityTime.OpeningSunday % 60;
                        newplace.HoursOpen = (hour == 0 ? hour + "0:" : hour + ":") + (minutes == 0 ? minutes + "0" : minutes);

                        hour = place.PlaceAvailabilityTime.ClosingSunday / 60;
                        minutes = place.PlaceAvailabilityTime.ClosingSunday % 60;
                        newplace.HoursOpen += " - " + (hour == 0 ? hour + "0:" : hour + ":") + (minutes == 0 ? minutes + "0" : minutes);
                    }
                    else if (dt.DayOfWeek == DayOfWeek.Monday)
                    {
                        hour = place.PlaceAvailabilityTime.OpeningMonday / 60;
                        minutes = place.PlaceAvailabilityTime.OpeningMonday % 60;
                        newplace.HoursOpen = (hour == 0 ? hour + "0:" : hour + ":") + (minutes == 0 ? minutes + "0" : minutes);

                        hour = place.PlaceAvailabilityTime.ClosingMonday / 60;
                        minutes = place.PlaceAvailabilityTime.ClosingMonday % 60;
                        newplace.HoursOpen += " - " + (hour == 0 ? hour + "0:" : hour + ":") + (minutes == 0 ? minutes + "0" : minutes);

                    }
                    else if (dt.DayOfWeek == DayOfWeek.Tuesday)
                    {
                        hour = place.PlaceAvailabilityTime.OpeningTuesday / 60;
                        minutes = place.PlaceAvailabilityTime.OpeningTuesday % 60;
                        newplace.HoursOpen = (hour == 0 ? hour + "0:" : hour + ":") + (minutes == 0 ? minutes + "0" : minutes);

                        hour = place.PlaceAvailabilityTime.ClosingTuesday / 60;
                        minutes = place.PlaceAvailabilityTime.ClosingTuesday % 60;
                        newplace.HoursOpen += " - " + (hour == 0 ? hour + "0:" : hour + ":") + (minutes == 0 ? minutes + "0" : minutes);
                    }
                    else if (dt.DayOfWeek == DayOfWeek.Wednesday)
                    {
                        hour = place.PlaceAvailabilityTime.OpeningWednesday / 60;
                        minutes = place.PlaceAvailabilityTime.OpeningWednesday % 60;
                        newplace.HoursOpen = (hour == 0 ? hour + "0:" : hour + ":") + (minutes == 0 ? minutes + "0" : minutes);

                        hour = place.PlaceAvailabilityTime.ClosingWednesday / 60;
                        minutes = place.PlaceAvailabilityTime.ClosingWednesday % 60;
                        newplace.HoursOpen += " - " + (hour == 0 ? hour + "0:" : hour + ":") + (minutes == 0 ? minutes + "0" : minutes);
                    }
                    else if (dt.DayOfWeek == DayOfWeek.Thursday)
                    {
                        hour = place.PlaceAvailabilityTime.OpeningThursday / 60;
                        minutes = place.PlaceAvailabilityTime.OpeningThursday % 60;
                        newplace.HoursOpen = (hour == 0 ? hour + "0:" : hour + ":") + (minutes == 0 ? minutes + "0" : minutes);

                        hour = place.PlaceAvailabilityTime.ClosingThursday / 60;
                        minutes = place.PlaceAvailabilityTime.ClosingThursday % 60;
                        newplace.HoursOpen += " - " + (hour == 0 ? hour + "0:" : hour + ":") + (minutes == 0 ? minutes + "0" : minutes);
                    }
                    else if (dt.DayOfWeek == DayOfWeek.Friday)
                    {
                        hour = place.PlaceAvailabilityTime.OpeningFriday / 60;
                        minutes = place.PlaceAvailabilityTime.OpeningFriday % 60;
                        newplace.HoursOpen = (hour == 0 ? hour + "0:" : hour + ":") + (minutes == 0 ? minutes + "0" : minutes);

                        hour = place.PlaceAvailabilityTime.ClosingFriday / 60;
                        minutes = place.PlaceAvailabilityTime.ClosingFriday % 60;
                        newplace.HoursOpen += " - " + (hour == 0 ? hour + "0:" : hour + ":") + (minutes == 0 ? minutes + "0" : minutes);
                    }
                    else if (dt.DayOfWeek == DayOfWeek.Saturday)
                    {
                        hour = place.PlaceAvailabilityTime.OpeningSaturday / 60;
                        minutes = place.PlaceAvailabilityTime.OpeningSaturday % 60;
                        newplace.HoursOpen = (hour == 0 ? hour + "0:" : hour + ":") + (minutes == 0 ? minutes + "0" : minutes);

                        hour = place.PlaceAvailabilityTime.ClosingSaturday / 60;
                        minutes = place.PlaceAvailabilityTime.ClosingSaturday % 60;
                        newplace.HoursOpen += " - " + (hour == 0 ? hour + "0:" : hour + ":") + (minutes == 0 ? minutes + "0" : minutes);
                    }
                }

                hour = place.AverageTimeSpent / 60;
                minutes = place.AverageTimeSpent % 60;
                newplace.TimeSpent = (hour == 0 ? hour + "0 godz. " : hour + " godz. ") + (minutes == 0 ? minutes + "0 min." : minutes + " min.");
                newplace.ImgSrc = $"C:\\Users\\01159502\\Desktop\\develop\\ProjektZespolowy\\ProjektZespolowy.Web\\wwwroot\\images\\places\\{newplace.Id}.jpg";

                newList.Add(newplace);
            }

            return newList;
        }
    }
}
