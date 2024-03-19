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
using System.Windows.Shapes;
using DotNetBrowser.Browser;
using ProjektZespolowy.Desktop.Pages;

namespace ProjektZespolowy.Desktop
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public enum ActivePage
        {
            Welcom,
            Home,
            User,
            Forum,
            Activities,
            SelectedActiviti,
            Trips,
            CreateTrip,
            CreateTrip2,
            CreateTrip3,
            CreatePost,
            CreatePlace,
            PostComments,
            EditTrip,
            TakeTrip,
            SelectTrip
        }
        public ActivePage m_ActivePage;


        public MainWindow()
        {
            InitializeComponent();
            m_frame.Content = new WelcomPage();
            
            m_menu.Visibility = Configuration.m_MenuVisible;
        }

        ~MainWindow()
        {
            //Configuration.browser?.Dispose();
            //Configuration.engine?.Dispose();
        }

        private void Button_UserInfo(object sender, RoutedEventArgs e)
        {
            if(m_ActivePage != ActivePage.User)
            {
                m_frame.Content = new UserPage();
                m_ActivePage = ActivePage.User;
            }
        }

        private void Button_Forum(object sender, RoutedEventArgs e)
        {
            if (m_ActivePage != ActivePage.Forum)
            {
                m_frame.Content = new ForumPage();
                m_ActivePage = ActivePage.Forum;
            }
            
        }

        private void Button_Map(object sender, RoutedEventArgs e)
        {
            if (m_ActivePage != ActivePage.Home)
            {
                m_frame.Content = new HomePage();
                m_ActivePage = ActivePage.Home;
            }
        }

        private void Button_Activities(object sender, RoutedEventArgs e)
        {
            if (m_ActivePage != ActivePage.Activities)
            {
                m_frame.Content = new ActivitiesPage();
                m_ActivePage = ActivePage.Activities;
            }
        }

        private void Button_Trips(object sender, RoutedEventArgs e)
        {
            if (m_ActivePage != ActivePage.Trips)
            {
                m_frame.Content = new TripsPage();
                m_ActivePage = ActivePage.Trips;
            }
        }
    }
}
