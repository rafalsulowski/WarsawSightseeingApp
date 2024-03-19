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
using DotNetBrowser;
using DotNetBrowser.Browser;
using DotNetBrowser.Wpf;
using ProjektZespolowy.Models.JoinedTables;
using ProjektZespolowy.Models.DTO;
using Microsoft.EntityFrameworkCore.Storage;

namespace ProjektZespolowy.Desktop.Pages
{
    public partial class CreateTrip3Page : Page
    {
        public int m_TimeCounter = 0;
        public int m_BudgetCounter = 0;
        public int m_TotalTime = 0;
        private bool m_Modify = false;
        private bool m_FirstRun = true;
        private string m_UrlCompleteDir = "";
        private Trip m_Trip { get; set; } = new Trip();
        private List<PlacesTrips> m_PlacesTrips = new List<PlacesTrips>();
        public CreateTrip3Page(Trip trip, ItemCollection places, bool modify, int totaltime, int timecounter, int budgetcounter)
        {
            InitializeComponent();//pokaz gorne menu
            m_webBrowser.InitializeFrom(Configuration.browser);

            MainWindow window = (MainWindow)Application.Current.MainWindow;
            if (window.GetType() == typeof(MainWindow))
            {
                Configuration.m_MenuVisible = Visibility.Visible;
                (window as MainWindow).m_menu.Visibility = Visibility.Visible;
                (window as MainWindow).m_ActivePage = MainWindow.ActivePage.CreateTrip3;
            }

            m_TotalTime = totaltime;
            m_TimeCounter = timecounter;
            m_BudgetCounter = budgetcounter;

            m_FirstRun = true;
            m_Modify = modify;
            m_Trip = trip;
            foreach (PlacesTrips placesTrip in places)
            {
                m_PlacesTrips.Add(placesTrip);
            }

            //utworzenie adresu URL dla map google na podstawie danych wycieczki:
            m_UrlCompleteDir = Configuration.UrlBuilder(m_PlacesTrips.Select(u => u.Place.MapToDTO()).ToList(), m_Trip.TransportType);
            Configuration.browser.Navigation.LoadUrl(m_UrlCompleteDir);

            m_StopHour.Content = m_Trip.StopHour;
            m_BeginHour.Content = m_Trip.StartHour + $" (zajęto {m_TimeCounter}/{m_TotalTime} [godz.])";
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
            m_PlaceCount.Content = m_PlacesTrips.Count;
            m_TitleLabel.Content = m_Trip.Title;
            m_AuthorLabel.Content = m_Trip.Author;
            m_ActivitiesList.ItemsSource = m_PlacesTrips;
            m_Budget.Content = m_Trip.Budget + $" PLN (wykorzystano {m_BudgetCounter}/{m_Trip.Budget} PLN)";
            m_Desc.Text = m_Trip.Description;

            if (m_Trip.IsPublic == true)
                m_Visibility.Content = "Publiczna";
            else
                m_Visibility.Content = "Prywatna";

            if(m_Modify == true)
            {
                m_PageTitle.Content = "Edycja wycieczki o tytule: " + m_Trip.Title + " krok 3/3";
                m_BtnCreateTrip.Content = "Zatwierdź edycję";
            }
        }

        public void Init(int totaltime, int timecounter, int budgetcounter)
        {
            m_TotalTime = totaltime;
            m_TimeCounter = timecounter;
            m_BudgetCounter = budgetcounter;
        }

        private void Button_CreateTrip(object sender, RoutedEventArgs e)
        {
            HttpResponseMessage response;

            if (m_Modify == true)
            {
                //modyfikacja wycieczki

                //1. Usunięcie
                response = Configuration.m_Client.DeleteAsync($"https://localhost:44441/api/Trip/{m_Trip.Id}").Result;
                if (!response.IsSuccessStatusCode)
                {
                    MessageBox.Show("Nie udało się zmodyfikować wycieczki");
                    return;
                }

                //2. Utworzenie nowej wycieczki w else
            }

            //stworzenie wycieczki API
            m_Trip.UserId = Configuration.m_ActualLoggerdUser.Id;
            string jsonString = JsonSerializer.Serialize(m_Trip);
            response = Configuration.m_Client.PostAsync("https://localhost:44441/api/Trip/AddTrip", new StringContent(jsonString, Encoding.UTF8, "application/json")).Result;

            if (!response.IsSuccessStatusCode)
            {
                MessageBox.Show("Nie udało się utworzyć wycieczki");
                return;
            }

            int tripId = Convert.ToInt32(response.Content.ReadAsStringAsync().Result);

            //dodanie miejsc do wycieczki za pomoca api
            foreach (PlacesTrips placesTrip in m_PlacesTrips)
            {
                jsonString = "{\"placeId\": " + placesTrip.PlaceId + ", \"tripId\": " + tripId 
                    + ", \"sequence\": " + placesTrip.Sequence + ", \"timeForPlace\": " 
                    + placesTrip.TimeForPlace+ ", \"budgetForPlace\": " + placesTrip.BudgetForPlace + "}";
                response = Configuration.m_Client.PostAsync("https://localhost:44441/api/Trip/AddPlaceToTrip", new StringContent(jsonString, Encoding.UTF8, "application/json")).Result;

                if (!response.IsSuccessStatusCode)
                {
                    MessageBox.Show($"Nie udało się dodać miejsca \"{placesTrip.Place.Name}\" do wycieczki");
                }
            }

            if(m_Modify)
                MessageBox.Show("Poprawnie zmodyfikowano wycieczkę!", "Sukces", MessageBoxButton.OK);
            else
                MessageBox.Show("Poprawnie utworzono wycieczkę!", "Sukces", MessageBoxButton.OK);

            this.NavigationService.Navigate(new TripsPage());
        }

        private void Button_GoBack(object sender, RoutedEventArgs e)
        {
            this.NavigationService.GoBack();
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

            PlacesTrips place = (sender as ListBox).SelectedItem as PlacesTrips;

            if (place != null)
            {
                string uri = $"https://www.google.com/maps/place/{Configuration.encodingUrlChracter(place.Place.Address)}/@{place.Place.Coordinates.Split(',')[0].Substring(0, 10)},{place.Place.Coordinates.Split(',')[1].Substring(0, 10)},17z?hl=pl-PL&entry=ttu";
                Configuration.browser.Navigation.LoadUrl(uri);
            }
        }
    }
}
