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
    /// Interaction logic for WelcomPage.xaml
    /// </summary>
    public partial class WelcomPage : Page
    {
        public WelcomPage()
        {
            InitializeComponent();
        }

        public WelcomPage(string msg)
        {
            InitializeComponent();
            MessageBox.Show(msg);
        }

        private void Button_Login(object sender, RoutedEventArgs e)
        {
            
            this.NavigationService.Navigate(new LoginPage());
        }

        private void Button_Register(object sender, RoutedEventArgs e)
        {
            this.NavigationService.Navigate(new RegisterPage());
        }

        
    }
}
