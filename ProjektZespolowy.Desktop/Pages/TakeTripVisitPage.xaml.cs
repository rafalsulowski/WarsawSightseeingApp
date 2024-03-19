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

    public partial class TakeTripEndPage : Page
    {
        private TripDTO m_Trip;
        public TakeTripEndPage(TripDTO trip)
        {
            InitializeComponent();
        }

        private void Button_Go_Back(object sender, RoutedEventArgs e)
        {
            this.NavigationService.Navigate(new HomePage());
        }
    }
}
