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
using DotNetBrowser;
using DotNetBrowser.Browser;
using DotNetBrowser.Wpf;
using ProjektZespolowy.Models;

namespace ProjektZespolowy.Desktop.Pages
{
    public partial class HomePage : Page
    {
        public List<Place> m_Places = new List<Place>();

        public HomePage()
        {
            InitializeComponent();
            m_webBrowser.InitializeFrom(Configuration.browser);

            //pokaz gorne menu
            MainWindow window = (MainWindow)Application.Current.MainWindow;
            if(window.GetType() == typeof(MainWindow))
            {
                Configuration.m_MenuVisible = Visibility.Visible;
                (window as MainWindow).m_menu.Visibility= Visibility.Visible;
                (window as MainWindow).m_ActivePage = MainWindow.ActivePage.Home;
            }


            HttpClient client = new HttpClient();
            HttpResponseMessage response = client.GetAsync("https://localhost:44441/api/Places").Result;
            if (response.IsSuccessStatusCode)
            {
                var Resp = response.Content.ReadFromJsonAsync<IEnumerable<Place>>().Result;
                if (Resp != null)
                {
                    m_Places = new List<Place>(Resp);
                }
            }
            else
            {
                MessageBox.Show("Incorrect response for url: \"https://localhost:7118/api/Places\" is: \"" + response.IsSuccessStatusCode.ToString() + '"');
            }


            m_ActivitiesList.ItemsSource = m_Places;
            
            string ur1 = "https://www.google.com/maps/d/u/0/edit?mid=1-Ddl0Yp3L-inom_mP7Lww0ibGBs0e30&ll=52.23075138538318%2C20.985290315952984&z=13";
            Configuration.browser.Navigation.LoadUrl(ur1);
        }

        private void UnSelectedPlace(object sender, RoutedEventArgs e)
        {
            string ur1 = "https://www.google.com/maps/d/u/0/edit?mid=1-Ddl0Yp3L-inom_mP7Lww0ibGBs0e30&ll=52.23075138538318%2C20.985290315952984&z=13";
            Configuration.browser.Navigation.LoadUrl(ur1);
        }

        private void OnSelectionChanged(object sender, RoutedEventArgs e)
        {
            Place place = (sender as ListBox).SelectedItem as Place;

            if (place != null)
            {
                string uri = $"https://www.google.com/maps/d/u/0/edit?mid=1-Ddl0Yp3L-inom_mP7Lww0ibGBs0e30&ll={place.Coordinates.Split(',')[0]}%2C{place.Coordinates.Split(',')[1]}&z=18";
                Configuration.browser.Navigation.LoadUrl(uri);
            }
        }
    }
}
