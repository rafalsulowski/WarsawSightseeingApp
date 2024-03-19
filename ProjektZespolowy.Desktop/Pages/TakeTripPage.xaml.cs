using ProjektZespolowy.Models;
using ProjektZespolowy.Models.DTO;
using System;
using System.Collections.Generic;
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

namespace ProjektZespolowy.Desktop.Pages
{
    /// <summary>
    /// Interaction logic for TakeTripPage.xaml
    /// </summary>
    public partial class TakeTripPage : Page
    {
        private TripDTO m_Trip;
        public TakeTripPage(TripDTO trip)
        {
            InitializeComponent();
            MainWindow window = (MainWindow)Application.Current.MainWindow;
            if (window.GetType() == typeof(MainWindow))
            {
                Configuration.m_MenuVisible = Visibility.Visible;
                (window as MainWindow).m_menu.Visibility = Visibility.Visible;
                (window as MainWindow).m_ActivePage = MainWindow.ActivePage.TakeTrip;
            }

            m_Trip = trip;
            m_TitleOfPage.Content = "Wycieczka: " + m_Trip.Title;
        }

        private void Button_Take(object sender, RoutedEventArgs e)
        {
            if(m_Trip.Places.Count == 0)
            {
                MessageBox.Show("Wycieczka nie posaida zaplanowanych miejsc!");
                return;
            }

            this.NavigationService.Navigate(new TakeTripDirPage(m_Trip, 0, true));
        }

        private void Button_Cancel(object sender, RoutedEventArgs e)
        {

            this.NavigationService.Navigate(new SelectedTripPage(m_Trip.Id));
        }
    }
}
