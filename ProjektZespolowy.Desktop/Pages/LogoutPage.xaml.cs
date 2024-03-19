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
    /// Interaction logic for LogoutPage.xaml
    /// </summary>
    public partial class LogoutPage : Page
    {
        public LogoutPage()
        {
            InitializeComponent();
        }
        private void Button_WelcomPage(object sender, RoutedEventArgs e)
        {
            this.NavigationService.Navigate(new WelcomPage());
        }

    }
}
