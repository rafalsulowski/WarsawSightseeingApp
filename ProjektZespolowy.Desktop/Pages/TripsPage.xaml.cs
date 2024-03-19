using ProjektZespolowy.Models;
using ProjektZespolowy.Models.DTO;
using System;
using System.Collections.Generic;
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
    /// <summary>
    /// Interaction logic for TripsPage.xaml
    /// </summary>
    public partial class TripsPage : Page
    {

        private class TripBasicInfo
        {
            public int Id { get; set; }
            public string Title { get; set; }
            public string Author { get; set; }
            public string TransportType { get; set; }
            public int PlaceCount { get; set; }
            public string Date { get; set; }
            public string Visibility { get; set; }
        }


        List<TripDTO> m_Trips = new List<TripDTO>();
        List<TripBasicInfo> m_TripsBinding = new List<TripBasicInfo>();

        public TripsPage(User? user = null)
        {
            InitializeComponent();
            //pokaz gorne menu
            MainWindow window = (MainWindow)Application.Current.MainWindow;
            if (window.GetType() == typeof(MainWindow))
            {
                Configuration.m_MenuVisible = Visibility.Visible;
                (window as MainWindow).m_menu.Visibility = Visibility.Visible;
                (window as MainWindow).m_ActivePage = MainWindow.ActivePage.Trips;
            }

            HttpResponseMessage response;
            if (user == null)
            {
                response = Configuration.m_Client.GetAsync("https://localhost:44441/api/Trip/GetAllTripsWithPlaces").Result;
                if (response.IsSuccessStatusCode)
                {
                    var TripsResp = response.Content.ReadFromJsonAsync<IEnumerable<TripDTO>>().Result;
                    if (TripsResp != null)
                    {
                        var list = new List<TripDTO>(TripsResp);
                        foreach(var el in list)
                        {
                            if(el.IsPublic || el.UserId == Configuration.m_ActualLoggerdUser.Id)
                                m_Trips.Add(el);
                        }
                    }
                }
                else
                {
                    MessageBox.Show("Incorrect response for url: \"https://localhost:44441/api/Trip/GetAllTripsWithPlaces\" is: \"" + response.IsSuccessStatusCode.ToString() + '"');
                }
            }
            else
            {
                response = Configuration.m_Client.GetAsync($"https://localhost:44441/api/Users/{user.Id}/Trips").Result;
                if (response.IsSuccessStatusCode)
                {
                    var resp = response.Content.ReadFromJsonAsync<UserDTO<TripDTO>>().Result;
                    if (resp != null)
                    {
                        m_Trips = resp.Data;
                    }
                }
                else
                {
                    MessageBox.Show($"Incorrect response for url: \"https://localhost:44441/api/Users/{user.Id}/Trips\" is: \"" + response.IsSuccessStatusCode.ToString() + '"');
                }
            }


            foreach (var Trip in m_Trips)
            {
                var newTrip = new TripBasicInfo
                {
                    Id = Trip.Id,
                    Author = Trip.Author,
                    Title = Trip.Title,
                    PlaceCount = Trip.Places != null ? Trip.Places.Count : 0
                };

                if (Trip.TransportType == TransportType.Car)
                    newTrip.TransportType = "samochód";
                else if (Trip.TransportType == TransportType.Bike)
                    newTrip.TransportType = "rower";
                else if (Trip.TransportType == TransportType.PublicTransport)
                    newTrip.TransportType = "transport publiczny";
                else if (Trip.TransportType == TransportType.Onfoot)
                    newTrip.TransportType = "pieszo";


                newTrip.Date = Trip.StartHour + "  " + Trip.Date.ToShortDateString();
                
                m_TripsBinding.Add(newTrip);
            }

            m_TripsList.Items.Clear();
            m_TripsList.ItemsSource = m_TripsBinding;
        }

        private void Button_AddNewTrip(object sender, RoutedEventArgs e)
        {
            //przekierowac do strony z szablonem tworzenia nowej wcieczki
            this.NavigationService.Navigate(new CreateTripPage());
        }

        private void OnPreviewMouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            var item = ItemsControl.ContainerFromElement(sender as ListBox, e.OriginalSource as DependencyObject) as ListBoxItem;
            if (item != null)
            {
                TripBasicInfo TripBI = (TripBasicInfo)item.Content;
                this.NavigationService.Navigate(new SelectedTripPage(TripBI.Id));
            }
        }

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ComboBox cmb = sender as ComboBox;
            int sel = cmb.SelectedIndex;

            switch (sel)
            {
                case 0:
                    m_TripsBinding.Sort((a, b) => a.PlaceCount.CompareTo(b.PlaceCount));
                    m_TripsList.Items.Refresh();
                    break;
                case 1:
                    m_TripsBinding.Sort((a, b) => a.PlaceCount.CompareTo(b.PlaceCount));
                    m_TripsBinding.Reverse();
                    m_TripsList.Items.Refresh();
                    break;
                case 2:
                    m_TripsBinding.Sort((a, b) => a.TransportType.CompareTo(b.TransportType));
                    m_TripsList.Items.Refresh();
                    break;
                case 3:
                    m_TripsBinding.Sort((a, b) => a.Date.CompareTo(b.Date));
                    m_TripsList.Items.Refresh();
                    break;
                case 4:
                    m_TripsBinding.Sort((a, b) => a.Date.CompareTo(b.Date));
                    m_TripsBinding.Reverse();
                    m_TripsList.Items.Refresh();
                    break;
                case 5:
                    m_TripsBinding.Sort((a, b) => a.Title.CompareTo(b.Title));
                    m_TripsList.Items.Refresh();
                    break;
                default:
                    break;
            }
        }
    }
}
