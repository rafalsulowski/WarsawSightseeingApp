using Microsoft.IdentityModel.Tokens;
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

namespace ProjektZespolowy.Desktop.Pages
{
    /// <summary>
    /// Interaction logic for CreatePlacePage.xaml
    /// </summary>
    public partial class CreatePlacePage : Page
    {
        public CreatePlacePage()
        {
            InitializeComponent(); 
            MainWindow window = (MainWindow)Application.Current.MainWindow;
            if (window.GetType() == typeof(MainWindow))
            {
                Configuration.m_MenuVisible = Visibility.Visible;
                (window as MainWindow).m_menu.Visibility = Visibility.Visible;
                (window as MainWindow).m_ActivePage = MainWindow.ActivePage.CreatePlace;
            }
        }

        private void Button_CreatePlace(object sender, RoutedEventArgs e)
        {
            //walidacja
            string name = m_Name.Text;
            string desc = m_Desc.Text;
            string cord = m_Cords.Text;
            string adres = m_Address.Text;
            string estimatedCost = m_EstimatedCost.Text;
            int startHour = m_startHour.SelectedIndex;
            int closeHour = m_stopHour.SelectedIndex;
            int minAge = m_minAge.SelectedIndex;
            int maxAge = m_maxAge.SelectedIndex;

            if (name.IsNullOrEmpty() || desc.IsNullOrEmpty() || cord.IsNullOrEmpty()
                || adres.IsNullOrEmpty() || estimatedCost.IsNullOrEmpty() || minAge == -1
                || maxAge == -1 || startHour == -1 || closeHour == -1)
            {
                MessageBox.Show("Zostały wporwadzone błędne dane");
                return;
            }


            //stworzenie posta w webapi (brak webapi)
            HttpClient client = new HttpClient();
            //HttpResponseMessage response = client.PostAsync("https://localhost:7118/api/Places").Result;

            HttpResponseMessage resp = new HttpResponseMessage(System.Net.HttpStatusCode.OK);
            if (resp.IsSuccessStatusCode)
            {
                //var placesResp = response.Content.ReadFromJsonAsync<IEnumerable<Place>>().Result;
                //if(placesResp != null)
                //{
                //    m_Places = new List<Place>(placesResp);
                //    m_ActivitiesList.ItemsSource = m_Places;
                //}
            }
            else
            {
                //MessageBox.Show("Incorrect response for url: \"https://localhost:7118/api/Places\" is: \"" + response.IsSuccessStatusCode.ToString() + '"');
            }

            MessageBox.Show("Poprawnie utworzono nowe miejsce", "Sukces", MessageBoxButton.OK);
            this.NavigationService.Navigate(new ActivitiesPage());
        }
    }
}
