using Microsoft.Identity.Client;
using Microsoft.IdentityModel.Tokens;
using ProjektZespolowy.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
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
    /// Interaction logic for CreateTripPage.xaml
    /// </summary>
    public partial class CreateTripPage : Page
    {
        private int m_TimeCounter = 0;
        private int m_BudgetCounter = 0;
        private int m_TotalTime = 0;
        private Trip m_Trip = new Trip();
        private bool m_Modyfi = false;

        //trip - istniejacy obiekt wycieczki ktory zostal tutaj przekazany w celu uzupelnienia danych na ekranie podczas
        //np. edycji lub powrotu z następnych kroków tworzenia wycieczki
        //parametr modify to flaga ktora jest true - tylko w tedy kiedy dokonujemy edycji wycieczki, fals czy tworzymy nową
        public CreateTripPage(Trip trip, bool modify) 
        {
            InitializeComponent();
            //pokaz gorne menu
            MainWindow window = (MainWindow)Application.Current.MainWindow;
            if (window.GetType() == typeof(MainWindow))
            {
                Configuration.m_MenuVisible = Visibility.Visible;
                (window as MainWindow).m_menu.Visibility = Visibility.Visible;
                (window as MainWindow).m_ActivePage = MainWindow.ActivePage.CreateTrip;
            }

            m_Trip = trip == null ? new Trip() : trip;
            m_Modyfi = modify;

            if (modify == true)
            {
                //wyznaczenie liczby pelnych godzin
                int hourStart = Convert.ToInt32(m_Trip.StartHour.Substring(0, m_Trip.StartHour.IndexOf(":")));
                int hourStop = Convert.ToInt32(m_Trip.StopHour.Substring(0, m_Trip.StopHour.IndexOf(":")));
                m_TotalTime = hourStop - hourStart;

                //obliczenie zajetosci czasowej i budzetowej
                foreach (var place in m_Trip.Places)
                {
                    m_TimeCounter += place.TimeForPlace;
                    m_BudgetCounter += place.BudgetForPlace;
                }
                m_PageTitle.Content = "Edycja wycieczki o tytule: " + m_Trip.Title + " krok 1/3";
                m_btnCancel.Content = "Wyjdź z edycji wycieczki";
                m_TimeCapacityPrompt.Visibility = Visibility.Visible;
                m_BudgetCapacityPrompt.Visibility = Visibility.Visible;
                m_TimeCapacity.Visibility = Visibility.Visible;
                m_BudgetCapacity.Visibility = Visibility.Visible;
                m_TimeCapacity.Content = $"{m_TimeCounter}/{m_TotalTime} [godz.]";
                m_BudgetCapacity.Content = $"{m_BudgetCounter}/{m_Trip.Budget} [PLN]";
            }

            //ponowne ustawienie danych podczas powrotu z okna dodawania miejsc do wycieczki lub podczsa edycji istniejacej wycieczki
            m_TitleTB.Text = m_Trip.Title;
            m_transportType.SelectedIndex = (int)m_Trip.TransportType;
            m_Visibility.SelectedIndex = m_Trip.IsPublic ? 1 : 0;

            int hour = int.Parse(m_Trip.StartHour.Substring(0, m_Trip.StartHour.IndexOf(':'))) * 2;
            if (int.Parse(m_Trip.StartHour.Substring(m_Trip.StartHour.IndexOf(':') + 1)) != 0)
                hour++;
            m_startHour.SelectedIndex = hour;

            hour = int.Parse(m_Trip.StopHour.Substring(0, m_Trip.StopHour.IndexOf(':'))) * 2;
            if (int.Parse(m_Trip.StopHour.Substring(m_Trip.StopHour.IndexOf(':') + 1)) != 0)
                hour++;
            m_stopHour.SelectedIndex = hour;

            m_date.SelectedDate = m_Trip.Date;
            m_Budget.Text = m_Trip.Budget.ToString();
            m_Desc.Text = m_Trip.Description;
        }

        public CreateTripPage()
        {
            InitializeComponent();
            //pokaz gorne menu
            MainWindow window = (MainWindow)Application.Current.MainWindow;
            if (window.GetType() == typeof(MainWindow))
            {
                Configuration.m_MenuVisible = Visibility.Visible;
                (window as MainWindow).m_menu.Visibility = Visibility.Visible;
                (window as MainWindow).m_ActivePage = MainWindow.ActivePage.CreateTrip;
            }
        }

        private void Button_CreateTrip2(object sender, RoutedEventArgs e)
        {
            //Odczyt parametrów i walidacja
            string? startHour = m_startHour.SelectionBoxItem.ToString();
            string? stopHour = m_stopHour.SelectionBoxItem.ToString();
            string? title = m_TitleTB.Text;
            int budget = Convert.ToInt32(m_Budget.Text);
            string? descr = m_Desc.Text;
            string? author = Configuration.m_ActualLoggerdUser.Username;
            int transportType = m_transportType.SelectedIndex;
            bool date = m_date.SelectedDate.HasValue;
            string? visibility = m_Visibility.SelectionBoxItem.ToString();

            if (budget < 0 || Convert.ToInt32(m_Budget.Text) > 100000000 || visibility.IsNullOrEmpty() || date == false || transportType == -1 || string.IsNullOrEmpty(startHour) || string.IsNullOrEmpty(stopHour) || string.IsNullOrEmpty(title) || string.IsNullOrEmpty(author))
            {
                MessageBox.Show("Niekompletne dane!", "Błąd");
                return;
            }

            if(m_Modyfi)
            {
                //podczas edycji istniejacej wycieczki sprawdzenie czy nie ma sprzecznosci
                //w nowych ramach czasowych i budzetowych z dodanymi juz miejscami

                //przeliczenie totaltime
                int hourStart = Convert.ToInt32(startHour.Substring(0, startHour.IndexOf(":")));
                int hourStop = Convert.ToInt32(stopHour.Substring(0, stopHour.IndexOf(":")));
                m_TotalTime = hourStop - hourStart;
                if (m_TimeCounter > m_TotalTime)
                {
                    MessageBox.Show("Przewidziałeś zbyt mało czasu na aktualnie zaplanowane miejsca do odwiedzenia! Przywróć pierwotną wartość i najpierw usuń nadmiarowe miejsca w kroku 2/3", "Błąd");
                    return;
                }
                else if(m_BudgetCounter > budget)
                {
                    MessageBox.Show("Przewidziałeś zbyt mały budżet na aktualnie zaplanowane miejsca do odwiedzenia! Przywróć pierwotną wartość i najpierw usuń nadmiarowe miejsca w kroku 2/3!", "Błąd");
                    return;
                }
            }

            //sprawdzenie czy nie ma juz wyciezki o takiej nazwie
            List<Trip> Trips = new List<Trip>();
            HttpResponseMessage response = Configuration.m_Client.GetAsync("https://localhost:44441/api/Trip/GetAllTrips").Result;
            if (response.IsSuccessStatusCode)
            {
                var TripsResp = response.Content.ReadFromJsonAsync<IEnumerable<Trip>>().Result;
                if (TripsResp != null)
                {
                    Trips = new List<Trip>(TripsResp);
                }
            }
            else
            {
                MessageBox.Show("Incorrect response for url: \"https://localhost:7118/api/Trip/GetAllTrips\" is: \"" + response.IsSuccessStatusCode.ToString() + '"');
                return;
            }

            foreach(Trip t in Trips)
            {
                if(t.Title == title && m_Modyfi == false)
                {
                    MessageBox.Show("Stworzyłeś już wycieczkę z takim tutułem", "Błąd zaznaczenia opcji");
                    return;
                }
            }


            if (startHour.Length >= stopHour.Length && String.Compare(startHour, stopHour) > 0)
            {
                MessageBox.Show("Godzina zakończenia wycieczki wypada przed godziną rozpoczęcia", "Błąd zaznaczenia opcji");
                return;
            }
            else
            {
                //uzupełnienie informacji, przejscie do 2 kroku
                m_Trip.Author = author;
                m_Trip.Title = title;
                m_Trip.Budget = budget;
                m_Trip.Description = descr;

                switch (visibility)
                {
                    case "Prywatna":
                        m_Trip.IsPublic = false;
                        break;
                    case "Publiczna":
                        m_Trip.IsPublic = true;
                        break;
                    default:
                        MessageBox.Show("Zły tryb widoczności wycieczki!", "Błąd zaznaczenia opcji");
                        return;
                }

                m_Trip.StartHour = startHour;
                m_Trip.StopHour = stopHour;
                m_Trip.Date = m_date.SelectedDate.GetValueOrDefault();
                m_Trip.TransportType = (TransportType)m_transportType.SelectedIndex;
                this.NavigationService.Navigate(new CreateTrip2Page(m_Trip, m_Modyfi));

            }

        }

        private void Button_Trips(object sender, RoutedEventArgs e)
        {
            this.NavigationService.Navigate(new TripsPage());
        }

        private void RewriteUserInputs(Trip trip)
        {

        }
    }
}
