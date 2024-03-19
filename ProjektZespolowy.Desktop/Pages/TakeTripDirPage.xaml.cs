using DotNetBrowser.Browser;
using ProjektZespolowy.Models;
using ProjektZespolowy.Models.DTO;
using SharpVectors.Dom.Svg;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Drawing.Drawing2D;
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
using System.Threading;

namespace ProjektZespolowy.Desktop.Pages
{

    public partial class TakeTripDirPage : Page
    {
        private class PlacesDtoBasicInfo
        {
            public int seq { get; set; }
            public string Name {get; set;}
            public string cost {get; set;}
            public string time { get; set; }
        }


        private int m_Step;
        private bool m_MapOpen;
        private bool m_ListOpen;
        private TripDTO m_Trip;
        private List<PlacesDtoBasicInfo> m_PlacesInfo = new List<PlacesDtoBasicInfo>();

        public TakeTripDirPage(TripDTO trip, int step, bool mapOpen)
        {
            InitializeComponent();
            m_webBrowser2.InitializeFrom(Configuration.browser);
            MainWindow window = (MainWindow)Application.Current.MainWindow;
            if (window.GetType() == typeof(MainWindow))
            {
                Configuration.m_MenuVisible = Visibility.Visible;
                (window as MainWindow).m_menu.Visibility = Visibility.Visible;
                (window as MainWindow).m_ActivePage = MainWindow.ActivePage.TakeTrip;
            }

            m_ProgressBar.Minimum = 0;
            m_ProgressBar.Maximum = 100;
            m_ListOpen = false;
            m_Step = step;
            m_Trip = trip;

            m_MapOpen = mapOpen;
            decideToOpenMap();
            
            m_PageCapturTitle.Content = "Kolejny przystanek";
            m_TitleOfPage.Content = "Wycieczka: " + m_Trip.Title;
            m_ProgressBar.Value = ((m_Step + 0.1) / m_Trip.Places.Count) * 100;
            m_PlaceName.Content = m_Trip.Places[m_Step].Name;

            foreach(var el in m_Trip.Places)
            {
                PlacesDtoBasicInfo n = new PlacesDtoBasicInfo
                {
                    Name = el.Name,
                    cost = el.EstimatedCost + " PLN",
                    seq = el.Sequence,
                    time = calcTime(el)
                };
                m_PlacesInfo.Add(n);
            }

            m_ActivitiesList.ItemsSource = m_PlacesInfo;
            m_ActualTime.Content = DateTime.Now.ToString("dddd, dd MMMM yyyy HH: mm");
            //new Thread(() =>
            //{
            //    Thread.CurrentThread.IsBackground = true;
            //    while(true)
            //    {
            //        this.Dispatcher.Invoke(() =>
            //        {
            //            m_ActualTime.Content = DateTime.Now.ToString("dddd, dd MMMM yyyy HH: mm: ss");
            //        });
            //        Thread.Sleep(999);
            //    }

            //}).Start();
        }

        private string calcTime(PlaceDTO pl)
        {
            string result = pl.TimeForPlace + "h (";

            int counter = 0; //zliczenie liczby godzin od rozpoczecia
            for (int i = 0; i < pl.Sequence - 1; i++)
                counter += m_Trip.Places[i].TimeForPlace;

            int hourStart = Convert.ToInt32(m_Trip.StartHour.Substring(0, m_Trip.StartHour.IndexOf(":")));
            hourStart += counter;

            if (m_Trip.StartHour.IndexOf(":30") >= 0)
                result += hourStart + ":30-" + (hourStart + pl.TimeForPlace) + ":30)";
            else
                result += hourStart + ":00-" + (hourStart + pl.TimeForPlace) + ":00)";
            return result;
        }

        private void decideToOpenMap()
        {
            if (m_MapOpen == false) //jesli mapa zamknieta to otworz
            {
                m_BtnShowMap.Content = "Ukryj mape";
                m_webBrowser.Visibility = Visibility.Visible;
                m_webBrowser.Margin = new Thickness(681, 75, 0, 0);
                m_ContentPage.Margin = new Thickness(66, 75, 0, 0);

                string uri = $"https://www.google.com/maps/place/{Configuration.encodingUrlChracter(m_Trip.Places[m_Step].Address)}/@{m_Trip.Places[m_Step].Coordinates.Split(',')[0].Substring(0, 10)},{m_Trip.Places[m_Step].Coordinates.Split(',')[1].Substring(0, 10)},17z?hl=pl-PL&entry=ttu";
                Configuration.browser.Navigation.LoadUrl(uri);
                m_MapOpen = true;
            }
            else //zamkniecie mapy
            {
                m_BtnShowMap.Content = "Wyświetl na mapie";
                m_webBrowser.Visibility = Visibility.Hidden;
                m_webBrowser.Margin = new Thickness(681, 75, 0, 0);
                m_ContentPage.Margin = new Thickness(378, 75, 0, 0);
                m_MapOpen = false;
            }
        }

        private void Button_ShowMap(object sender, RoutedEventArgs e)
        {
            decideToOpenMap();
        }

        private void m_BtnShowList_Click(object sender, RoutedEventArgs e) //pokaz liste miejsc
        {
            if(m_ListOpen == false)
            {
                m_BtnShowList.Content = "Zwiń listę";
                m_ActivitiesList.Visibility = Visibility.Visible;
                m_ListOpen = true;
            }
            else
            {
                m_BtnShowList.Content = "Rozwiń listę miejsc";
                m_ActivitiesList.Visibility = Visibility.Hidden;
                m_ListOpen = false;
            }
        }

        private void Button_Arrived(object sender, RoutedEventArgs e) //juz dotarlem
        {
            //zmiany na UI:
            m_PageCapturTitle.Content = "Aktualny przystanek";
            m_ProgressBar.Value = ((m_Step + 0.6) / m_Trip.Places.Count) * 100;
            m_BtnArrived.Visibility = Visibility.Hidden;
            m_BtnShowMap.Visibility = Visibility.Hidden;
            m_BtnNextPlace.Visibility = Visibility.Hidden;
            m_BtnGoNext.Visibility = Visibility.Visible;
            m_Prompt.Visibility = Visibility.Visible;
        }

        private void Button_NextPlace(object sender, RoutedEventArgs e) //pomin to miejsce 
        {
            if(m_Step < m_Trip.Places.Count - 1)
                this.NavigationService.Navigate(new TakeTripDirPage(m_Trip, m_Step + 1, !m_MapOpen));
            else
            {
                this.NavigationService.Navigate(new TakeTripEndPage(m_Trip));
            }
        }

        private void Button_GoNext(object sender, RoutedEventArgs e) //chce isc dalej
        {
            Button_NextPlace(sender,e);
        }
    }
}
