using ProjektZespolowy.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
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
using System.Text.Json;
using Microsoft.Identity.Client;
using System.Net.Http.Json;
using ProjektZespolowy.Models.DTO;
using System.Reflection;
using System.Diagnostics;
using DotNetBrowser;
using DotNetBrowser.Browser;
using DotNetBrowser.Wpf;
using System.IO;
using System.Net;
using TransportType = ProjektZespolowy.Models.TransportType;
using ProjektZespolowy.Models.JoinedTables;
using Azure;

namespace ProjektZespolowy.Desktop.Pages
{
    /// <summary>
    /// Interaction logic for SelectedTripPage.xaml
    /// </summary>
    public partial class SelectedTripPage : Page
    {
        private int m_TimeCounter = 0;
        private int m_BudgetCounter = 0;
        private int m_TotalTime = 0;
        private bool m_FirstRun = true;
        private string m_UrlCompleteDir = "";
        private TripDTO m_Trip = new TripDTO();
        public SelectedTripPage(int id)
        {
            InitializeComponent();//pokaz gorne menu
            m_webBrowser.InitializeFrom(Configuration.browser);

            MainWindow window = (MainWindow)Application.Current.MainWindow;
            if (window.GetType() == typeof(MainWindow))
            {
                Configuration.m_MenuVisible = Visibility.Visible;
                (window as MainWindow).m_menu.Visibility = Visibility.Visible;
                (window as MainWindow).m_ActivePage = MainWindow.ActivePage.SelectTrip;
            }

            //1. pobranie danych
            HttpResponseMessage response = Configuration.m_Client.GetAsync($"https://localhost:44441/api/Trip/GetTripWithPlaces/{id}").Result;
            if (response.IsSuccessStatusCode)
            {
                var TripResp = response.Content.ReadFromJsonAsync<TripDTO>().Result;
                if (TripResp != null)
                {
                    m_Trip = TripResp;
                }
            }
            else
            {
                MessageBox.Show($"Incorrect response for url: \"https://localhost:7118/api/Trip/GetTripWithPlaces/{id}\" is: \"" + response.IsSuccessStatusCode.ToString() + '"');
            }

            //wyznaczenie liczby pelnych godzin
            int hourStart = Convert.ToInt32(m_Trip.StartHour.Substring(0, m_Trip.StartHour.IndexOf(":")));
            int hourStop = Convert.ToInt32(m_Trip.StopHour.Substring(0, m_Trip.StopHour.IndexOf(":")));
            m_TotalTime = hourStop - hourStart;

            //wyznaczenie zajetości czasu wycieczki
            foreach(var place in m_Trip.Places)
            {
                m_TimeCounter += place.TimeForPlace;
                m_BudgetCounter += place.BudgetForPlace;
            }

            //jesli zalogowany uzytkownik to tworca wycieczki to udostepnij opcje usuwania i edycji
            if (m_Trip.UserId == Configuration.m_ActualLoggerdUser.Id)
            {
                m_BtnDelete.Visibility = Visibility.Visible;
                m_BtnEdit.Visibility = Visibility.Visible;
            }

            //2. utworzenie adresu URL dla map google na podstawie danych wycieczki:
            m_UrlCompleteDir = Configuration.UrlBuilder(m_Trip.Places, m_Trip.TransportType);
            Configuration.browser.Navigation.LoadUrl(m_UrlCompleteDir);

            //3. Uzupelnienie pozostalych informacji
            NameTrip.Content = "Wycieczka: " + m_Trip.Title;
            m_BeginHour.Content = m_Trip.StartHour;
            m_StopHour.Content = m_Trip.StopHour + $" (zajęto {m_TimeCounter}/{m_TotalTime} [godz.])";

            switch (m_Trip.TransportType)
            {
                case TransportType.Car:
                    m_TransportType.Content = "samochód";
                    break;
                case TransportType.Bike:
                    m_TransportType.Content = "rower";
                    break;
                case TransportType.Onfoot:
                    m_TransportType.Content = "pieszo";
                    break;
                case TransportType.PublicTransport:
                    m_TransportType.Content = "transport publiczny";
                    break;
                default:
                    m_TransportType.Content = "samochód";
                    break;
            }
            m_Date.Content = m_Trip.Date.Date.ToShortDateString();
            m_PlaceCount.Content = m_Trip.Places != null ? m_Trip.Places.Count : 0;
            m_TitleLabel.Content = m_Trip.Title;
            m_AuthorLabel.Content = m_Trip.Author;
            m_Budget.Content = m_Trip.Budget + $" PLN (wykorzystano {m_BudgetCounter}/{m_Trip.Budget} PLN)";
            m_Desc.Text = m_Trip.Description;

            if(m_Trip.IsPublic == true)
                m_Visibility.Content = "Publiczna";
            else
                m_Visibility.Content = "Prywatna";


            m_ActivitiesList.Items.Clear();
            if(m_Trip.Places != null)
            {
                m_Trip.Places.Sort((u, y) => u.Sequence.CompareTo(y.Sequence));
                foreach (var el in m_Trip.Places)
                {
                    m_ActivitiesList.Items.Add(el);
                }
            }

            m_FirstRun = true;
        }

        private void Button_GoBack(object sender, RoutedEventArgs e)
        {
            this.NavigationService.Navigate(new TripsPage());
        }

        private void Button_Edit(object sender, RoutedEventArgs e)
        {
            HttpResponseMessage response = Configuration.m_Client.GetAsync($"https://localhost:44441/api/Trip/GetPlacesTripsDtoForTrip/{m_Trip.Id}").Result;
            if (response.IsSuccessStatusCode)
            {
                List<PlacesTripsDTO> ptDTO = response.Content.ReadFromJsonAsync<List<PlacesTripsDTO>>().Result;
                
                Trip trip = new Trip();
                trip.Id = m_Trip.Id;
                trip.Title = m_Trip.Title;
                trip.Description = m_Trip.Description;
                trip.Author = m_Trip.Author;
                trip.UserId = m_Trip.UserId;
                trip.StartHour = m_Trip.StartHour;
                trip.StopHour = m_Trip.StopHour;
                trip.Date = m_Trip.Date;
                trip.TransportType = m_Trip.TransportType;
                trip.IsPublic = m_Trip.IsPublic;
                trip.Budget = m_Trip.Budget;
                
                if (ptDTO != null)
                {
                    foreach (var el in ptDTO)
                        trip.Places.Add(new PlacesTrips { PlaceId = el.PlaceId, Sequence = el.Sequence, TripId = el.TripId, TimeForPlace = el.TimeForPlace, BudgetForPlace = el.BudgetForPlace });
                }

                this.NavigationService.Navigate(new CreateTripPage(trip, true));
            }
            else
            {
                MessageBox.Show("Nie udało się przejść do widoku edycji wycieczki");
            }
        }

        private void Button_Delete(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Czy na pewno chcesz usunąc wycieczkę?", "Uwaga!",MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                HttpResponseMessage response = Configuration.m_Client.DeleteAsync($"https://localhost:44441/api/Trip/{m_Trip.Id}").Result;
                if (response.IsSuccessStatusCode)
                {
                    MessageBox.Show("Usunięto wycieczkę");
                    this.NavigationService.Navigate(new TripsPage());
                }
                else
                {
                    MessageBox.Show("Nie udało się usunąć wycieczki");
                }
            }
        }

        private void UnSelectedPlace(object sender, RoutedEventArgs e)
        {
            Configuration.browser.Navigation.LoadUrl(m_UrlCompleteDir);
        }

        private void OnSelectionChanged(object sender, RoutedEventArgs e)
        {
            if (m_FirstRun == true)
            {
                m_FirstRun = false;
                return;
            }

            PlaceDTO place = (sender as ListBox).SelectedItem as PlaceDTO;

            if (place != null)
            {
                string uri = $"https://www.google.com/maps/place/{Configuration.encodingUrlChracter(place.Address)}/@{place.Coordinates.Split(',')[0].Substring(0, 10)},{place.Coordinates.Split(',')[1].Substring(0, 10)},17z?hl=pl-PL&entry=ttu";
                Configuration.browser.Navigation.LoadUrl(uri);
            }
        }

        private void Button_StartTrip(object sender, RoutedEventArgs e)
        {
            this.NavigationService.Navigate(new TakeTripPage(m_Trip));
        }
    }
}
