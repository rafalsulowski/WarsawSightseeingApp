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
    /// Interaction logic for UserPage.xaml
    /// </summary>
    public partial class UserPage : Page
    {
        public UserPage()
        {
            InitializeComponent();

            //pokaz gorne menu
            MainWindow window = (MainWindow)Application.Current.MainWindow;
            if (window.GetType() == typeof(MainWindow))
            {
                Configuration.m_MenuVisible = Visibility.Visible;
                (window as MainWindow).m_menu.Visibility = Visibility.Visible;
                (window as MainWindow).m_ActivePage = MainWindow.ActivePage.User;
            }

            if (Configuration.m_ActualLoggerdUser == null)
            {
                User user = new User();
                user.Id = 0;
                user.Email = "";
                user.Username = "Error, actual logged user is null!";
                user.PasswordHash = "";

                this.DataContext = user;
            }
            else
            {
                this.DataContext = Configuration.m_ActualLoggerdUser;

                m_TripsCounter.Content = Configuration.m_ActualLoggerdUser.Trips.Count;
                m_PostsCounter.Content = Configuration.m_ActualLoggerdUser.Posts.Count;
                
                //na szybko
                //pobranie informacji o ilości postów
                HttpClient client = new HttpClient();
                HttpResponseMessage response = client.GetAsync($"https://localhost:44441/api/Users/{Configuration.m_ActualLoggerdUser.Id}/Posts").Result;
                if (response.IsSuccessStatusCode)
                {
                    var resp = response.Content.ReadFromJsonAsync<UserDTO<PostDTO>>().Result;
                    if (resp != null)
                    {
                        m_PostsCounter.Content = resp.Data.Count;
                        if(resp.Data.Count == 0)
                            m_BtnPosts.IsEnabled = false;
                    }
                }
                else
                {
                    MessageBox.Show($"Incorrect response for url: \"https://localhost:44441/api/Users/{Configuration.m_ActualLoggerdUser.Id}/Posts\" is: \"" + response.IsSuccessStatusCode.ToString() + '"');
                }

                //pobranie informacji o ilości wycieczek
                response = client.GetAsync($"https://localhost:44441/api/Users/{Configuration.m_ActualLoggerdUser.Id}/Trips").Result;
                if (response.IsSuccessStatusCode)
                {
                    var resp = response.Content.ReadFromJsonAsync<UserDTO<TripDTO>>().Result;
                    if (resp != null)
                    {
                        m_TripsCounter.Content = resp.Data.Count; 
                        if (resp.Data.Count == 0)
                            m_BtnTrips.IsEnabled = false;
                    }
                }
                else
                {
                    MessageBox.Show($"Incorrect response for url: \"https://localhost:44441/api/Users/{Configuration.m_ActualLoggerdUser.Id}/Trips\" is: \"" + response.IsSuccessStatusCode.ToString() + '"');
                }

            }
        }

        private void Button_Logout(object sender, RoutedEventArgs e)
        {
            //usuniecie akutalnie zalogowanego uzytkownika
            Configuration.m_ActualLoggerdUser = null;

            //schowanie menu
            MainWindow window = (MainWindow)Application.Current.MainWindow;
            if (window.GetType() == typeof(MainWindow))
            {
                Configuration.m_MenuVisible = Visibility.Hidden;
                (window as MainWindow).m_menu.Visibility = Visibility.Hidden;
                (window as MainWindow).m_ActivePage = MainWindow.ActivePage.Welcom;
            }

            Configuration.m_Client.DefaultRequestHeaders.Remove("Authorization");

            //pokazanie potwierdzenia wylogowania
            this.NavigationService.Navigate(new LogoutPage());
        }

        private void Button_ShowTrips(object sender, RoutedEventArgs e)
        {
            this.NavigationService.Navigate(new TripsPage(Configuration.m_ActualLoggerdUser));
        }

        private void Button_ShowPosts(object sender, RoutedEventArgs e)
        {
            this.NavigationService.Navigate(new ForumPage(Configuration.m_ActualLoggerdUser));
        }
    }
}
