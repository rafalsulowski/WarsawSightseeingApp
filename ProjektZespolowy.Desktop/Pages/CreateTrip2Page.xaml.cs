using Microsoft.Identity.Client;
using Microsoft.IdentityModel.Tokens;
using ProjektZespolowy.Models;
using ProjektZespolowy.Models.DTO;
using ProjektZespolowy.Models.JoinedTables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
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
using TransportType = ProjektZespolowy.Models.TransportType;

namespace ProjektZespolowy.Desktop.Pages
{
    /// <summary>
    /// Interaction logic for CreateTrip2Page.xaml
    /// </summary>
    public partial class CreateTrip2Page : Page
    {
        private int m_TimeCounter = 0;
        private int m_BudgetCounter = 0;
        private int m_TotalTime = 0;
        private bool m_Modify = false;
        private Trip? m_Trip = null;
        private List<Place>? m_Places = null;
        private List<Place>? m_PlacesRef = null;
        public CreateTrip2Page(Trip trip, bool modify)
        {
            InitializeComponent();
            //pokaz gorne menu
            MainWindow window = (MainWindow)Application.Current.MainWindow;
            if (window.GetType() == typeof(MainWindow))
            {
                Configuration.m_MenuVisible = Visibility.Visible;
                (window as MainWindow).m_menu.Visibility = Visibility.Visible;
                (window as MainWindow).m_ActivePage = MainWindow.ActivePage.CreateTrip2;
            }

            HttpResponseMessage response = Configuration.m_Client.GetAsync("https://localhost:44441/api/Places").Result;
            if (response.IsSuccessStatusCode)
            {
                var Resp = response.Content.ReadFromJsonAsync<IEnumerable<Place>>().Result;
                if (Resp != null)
                {
                    m_Places = new List<Place>(Resp);
                    m_PlacesRef = new List<Place>(m_Places);
                }
            }
            else
            {
                MessageBox.Show("Incorrect response for url: \"https://localhost:7118/api/Places\" is: \"" + response.IsSuccessStatusCode.ToString() + '"');
            }

            m_Modify = modify;
            m_Trip = trip;
            m_StartHour.Content = trip.StartHour.ToString();
            m_EndHour.Content = trip.StopHour.ToString();
            m_Date.Content = trip.Date.ToShortDateString();
            switch (m_Trip.TransportType)
            {
                case TransportType.Car:
                    m_transportType.Content = "samochód";
                    break;
                case TransportType.Bike:
                    m_transportType.Content = "rower";
                    break;
                case TransportType.Onfoot:
                    m_transportType.Content = "pieszo";
                    break;
                case TransportType.PublicTransport:
                    m_transportType.Content = "transport publiczny";
                    break;
                default:
                    m_transportType.Content = "samochód";
                    break;
            }
            if (m_Trip.IsPublic == true)
                m_Visibility.Content = "Publiczna";
            else
                m_Visibility.Content = "Prywatna";
            m_Desc.Text = m_Trip.Description;
            m_Budget.Content = m_Trip.Budget;
            m_ActivitiesList.ItemsSource = m_Places;

            //wyznaczenie liczby pelnych godzin
            int hourStart = Convert.ToInt32(m_Trip.StartHour.Substring(0, m_Trip.StartHour.IndexOf(":")));
            int hourStop = Convert.ToInt32(m_Trip.StopHour.Substring(0, m_Trip.StopHour.IndexOf(":")));
            m_TotalTime = hourStop - hourStart;

            if (m_Modify == true) //uzupelnij tablice z zaznaczonymi miejscami dla trybu edycji wycieczki
            {
                m_PageTitle.Content = "Edycja wycieczki o tytule: " + m_Trip.Title + " krok 2/3";

                if (m_Trip.Places != null)
                {
                    m_Trip.Places.Sort((u, y) => u.Sequence.CompareTo(y.Sequence));
                    foreach (PlacesTrips placesTrips in m_Trip.Places)
                    {
                        m_TimeCounter += placesTrips.TimeForPlace;
                        m_BudgetCounter += placesTrips.BudgetForPlace;

                        m_SummaryPlaces.Items.Add(
                            new PlacesTrips { 
                            Place = m_Places.FirstOrDefault(u => u.Id == placesTrips.PlaceId),
                            PlaceId = placesTrips.PlaceId, TimeForPlace = placesTrips.TimeForPlace,
                            BudgetForPlace = placesTrips.BudgetForPlace,
                            Sequence = placesTrips.Sequence
                        });
                    }
                }
            }

            m_TimeCapacity.Content = m_TimeCounter + "/" + m_TotalTime + " [godz.]";
            m_BudgetCapacity.Content = m_BudgetCounter + "/" + m_Trip.Budget + " [PLN]";
            m_NumberOfPlace.Content = m_SummaryPlaces.Items.Count + 1;
        }

        private void Button_Add(object sender, RoutedEventArgs e)
        {
            if(m_SummaryPlaces.Items.Count >= 9)
            {
                if(MessageBox.Show("Uwaga! Przekroczono maksymalną liczę atrakcji dla trasy (maks= 9), jeśli dodasz tą atrakcję trasa dla wycieczki nie zostanie" +
                    "wytyczona! Miejsca będą tylko nanoszone na mapę, Czy chcesz dodać to miejsce?", "Uwaga", MessageBoxButton.YesNo) == MessageBoxResult.No)
                    return;
            }

            //1. odczytanie danych, walidacja
            if(m_TimeSpend.Text.IsNullOrEmpty())
            {
                MessageBox.Show("Pusta wartość czasu pobytu!");
                return;
            }

            if (m_BudgetSpend.Text.IsNullOrEmpty())
            {
                MessageBox.Show("Pusta wartość przeznaczanego budgetu!");
                return;
            }

            int timeSpend = Convert.ToInt32(m_TimeSpend.Text);
            if(timeSpend < 0 || timeSpend > 100000)
            {
                MessageBox.Show("Wartość czasu pobytu musi być liczbą całkowitą z zakresu 0 - 100'000!");
                return;
            }

            int budgetSpend = Convert.ToInt32(m_BudgetSpend.Text);
            if (budgetSpend < 0 || budgetSpend > 1000000)
            {
                MessageBox.Show("Wartość przeznacznego budżetu musi być liczbą całkowitą z zakresu 0 - 1'000'000!");
                return;
            }


            if (m_SelectedItemFromList.Content != null && String.IsNullOrEmpty(m_SelectedItemFromList.Content.ToString()))
            {
                MessageBox.Show("Miejsce nie zostało wybrane!");
                return;
            }

            PlacesTrips placesTrips = new PlacesTrips();
            placesTrips.Place = m_Places.Find(e => e.Name == m_SelectedItemFromList.Content.ToString());
            if (placesTrips.Place == null)
            {
                MessageBox.Show("Zostało wybrane miejsce, które nie istnieje!");
                return;
            }

            foreach(PlacesTrips p in m_SummaryPlaces.Items)
            {
                if (p.Place.Name  == m_SelectedItemFromList.Content)
                {
                    MessageBox.Show("Uwaga! Dana atrakcja znajduje się już na liście wycieczki!");
                    return;
                }
            }

            m_TimeCounter += timeSpend;
            if (m_TimeCounter > m_TotalTime)
            {
                m_TimeCounter -= timeSpend;
                MessageBox.Show("Nie wystarczny czasu na tą atrakcję!");
                return;
            }

            m_BudgetCounter += budgetSpend;
            if (m_BudgetCounter > m_Trip.Budget)
            {
                m_BudgetCounter -= budgetSpend;
                MessageBox.Show("Nie wystarczny budżetu na tą atrakcję!");
                return;
            }


            //3.dodanie miejsca do listy z prawej strony
            placesTrips.TimeForPlace = timeSpend;
            placesTrips.BudgetForPlace = budgetSpend;
            placesTrips.Sequence = m_SummaryPlaces.Items.Count + 1;
            placesTrips.PlaceId = placesTrips.Place.Id;
            m_SummaryPlaces.Items.Add(placesTrips);

            //4.pokazanie zmiany na omówieniu wycieczki po prawej stronie
            m_NumberOfPlace.Content = m_SummaryPlaces.Items.Count + 1;
            m_TimeCapacity.Content = m_TimeCounter + "/" + m_TotalTime + " [godz.]";
            m_BudgetCapacity.Content = m_BudgetCounter + "/" + m_Trip.Budget + " [PLN]";
        }

        private void SelectedPlace(object sender, RoutedEventArgs e)
        {
            Place place = (sender as ListBox).SelectedItem as Place;
            if (place != null)
            {
                m_SelectedItemFromList.Content = place.Name;
                m_BudgetSpend.Text = place.EstimatedCost.ToString();
                m_TimeSpend.Text = Math.Ceiling((decimal)place.AverageTimeSpent / 60).ToString();
            }
        }

        private void SelectedPlaceAfterDoubleClick(object sender, RoutedEventArgs e)
        {
            Place place = (sender as ListBox).SelectedItem as Place;
            if (place != null)
            {
                m_SelectedItemFromList.Content = place.Name;
                m_BudgetSpend.Text = place.EstimatedCost.ToString();
                m_TimeSpend.Text = Math.Ceiling((decimal)place.AverageTimeSpent / 60).ToString();
                Button_Add(sender, e);
            }
        }

        private void SearchPlace(object sender, RoutedEventArgs e)
        {
            var word = (sender as TextBox).Text as String;
            m_Places.Clear();

            foreach (var place in m_PlacesRef)
            {
                if (word == "" || place.Name.Contains(word))
                {
                    m_Places.Add(place);
                }
            }
            m_ActivitiesList.Items.Refresh();
        }

        private void Button_GoBack(object sender, RoutedEventArgs e)
        {
            if(m_Modify)
            {
                //jesli modyfikujemy wycieczke to trzeba zaktualizowac liste places dla m_trip
                m_Trip.Places.Clear();

                if(m_Places != null)
                    foreach(PlacesTrips place in m_SummaryPlaces.Items)
                        m_Trip.Places.Add(place);
            }

            this.NavigationService.Navigate(new CreateTripPage(m_Trip, m_Modify));
        }

        private void Button_DoRoute(object sender, RoutedEventArgs e)
        {
            //przejsc na 3 strone gdzie bedzie wyswietlona trasa mapy
            //Główna walidacja danych wycieczki:
            if(m_SummaryPlaces.Items.Count== 0)
            {
                MessageBox.Show("Nie można utworzyć wycieczki bez żadnych miejsc!");
                return;
            }

            this.NavigationService.Navigate(new CreateTrip3Page(m_Trip, m_SummaryPlaces.Items, m_Modify, m_TotalTime, m_TimeCounter, m_BudgetCounter));
        }

        private void Button_DeletePlace(object sender, RoutedEventArgs e)
        {
            //1. odczytanie danych, walidacja
            int IndxtoDelete = m_SummaryPlaces.SelectedIndex;
            
            if(IndxtoDelete < 0 || IndxtoDelete >= m_SummaryPlaces.Items.Count) 
            {
                MessageBox.Show("Taki numer wiersza nie odnosi się do właściwego miejsca!");
                return;
            }

            //poprawa wyswietlania zajetosci
            PlacesTrips el = (PlacesTrips)m_SummaryPlaces.Items[IndxtoDelete];
            m_TimeCounter -= el.TimeForPlace;
            m_BudgetCounter -= el.BudgetForPlace;
            m_TimeCapacity.Content = m_TimeCounter + "/" + m_TotalTime + " [godz.]";
            m_BudgetCapacity.Content = m_BudgetCounter + "/" + m_Trip.Budget + " [PLN]";

            //usunięcie:
            m_SummaryPlaces.Items.RemoveAt(m_SummaryPlaces.SelectedIndex);
            m_NumberOfPlace.Content = m_SummaryPlaces.Items.Count + 1;

            //poprawa kolejnosci zwiedzania
            List<PlacesTrips> list = m_SummaryPlaces.Items.OfType<PlacesTrips>().ToList();
            m_SummaryPlaces.Items.Clear();
            for (int i = 0; i < list.Count; i++)
            {
                PlacesTrips elem = list[i];
                elem.Sequence = i + 1;
                m_SummaryPlaces.Items.Add(elem);
            }
        }

        private void Button_MoveDown(object sender, RoutedEventArgs e)
        {
            MoveUp.IsEnabled = true;

            //1. odczytanie danych, walidacja
            int IndxToMoxe = m_SummaryPlaces.SelectedIndex;

            if (IndxToMoxe < 0 || IndxToMoxe >= m_SummaryPlaces.Items.Count)
            {
                MessageBox.Show("Taki numer wiersza nie odnosi się do właściwego miejsca!");
                return;
            }

            //dla ostatniego elementu na liscie nic nie rob
            if (IndxToMoxe == m_SummaryPlaces.Items.Count - 1)
            {
                MoveDown.IsEnabled= false;
                return;
            }

            //przesuniecie:
            PlacesTrips tmp = (PlacesTrips)m_SummaryPlaces.Items.GetItemAt(IndxToMoxe);
            m_SummaryPlaces.Items[IndxToMoxe] = m_SummaryPlaces.Items.GetItemAt(IndxToMoxe + 1);
            m_SummaryPlaces.Items[IndxToMoxe + 1] = tmp;

            //poprawa kolejnosci zwiedzania
            List<PlacesTrips> list = m_SummaryPlaces.Items.OfType<PlacesTrips>().ToList();
            m_SummaryPlaces.Items.Clear();
            for (int i = 0; i < list.Count; i++)
            {
                PlacesTrips elem = list[i];
                elem.Sequence = i + 1;
                m_SummaryPlaces.Items.Add(elem);
            }

            m_SummaryPlaces.SelectedIndex = IndxToMoxe + 1;
        }

        private void Button_MoveUp(object sender, RoutedEventArgs e)
        {
            MoveDown.IsEnabled = true;

            //1. odczytanie danych, walidacja
            int IndxToMoxe = m_SummaryPlaces.SelectedIndex;

            if (IndxToMoxe < 0 || IndxToMoxe >= m_SummaryPlaces.Items.Count)
            {
                MessageBox.Show("Taki numer wiersza nie odnosi się do właściwego miejsca!");
                return;
            }

            //dla pierwszego elementu na liscie nic nie rob
            if (IndxToMoxe == 0)
            {
                MoveUp.IsEnabled = false;
                return;
            }

            //przesuniecie:
            PlacesTrips tmp = (PlacesTrips)m_SummaryPlaces.Items.GetItemAt(IndxToMoxe);
            m_SummaryPlaces.Items[IndxToMoxe] = m_SummaryPlaces.Items.GetItemAt(IndxToMoxe - 1);
            m_SummaryPlaces.Items[IndxToMoxe - 1] = tmp;

            //poprawa kolejnosci zwiedzania
            List<PlacesTrips> list = m_SummaryPlaces.Items.OfType<PlacesTrips>().ToList();
            m_SummaryPlaces.Items.Clear();
            for (int i = 0; i < list.Count; i++)
            {
                PlacesTrips elem = list[i];
                elem.Sequence = i + 1;
                m_SummaryPlaces.Items.Add(elem);
            }

            m_SummaryPlaces.SelectedIndex = IndxToMoxe - 1;
        }
    }
}
