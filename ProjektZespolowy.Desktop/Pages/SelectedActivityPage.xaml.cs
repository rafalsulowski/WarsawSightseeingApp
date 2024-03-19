using ProjektZespolowy.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
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
using DotNetBrowser.Browser;
using ProjektZespolowy.Models.DTO;

namespace ProjektZespolowy.Desktop.Pages
{
    /// <summary>
    /// Interaction logic for SelectedActivityPage.xaml
    /// </summary>
    public partial class SelectedActivityPage : Page
    {
        private class CommentPlaceDTO
        {
            public int Id { get; set; }
            public string Content { get; set; }
            public int UserId { get; set; }
            public string Author { get; set; }
            public int PlaceId { get; set; }
            public double Note { get; set; }
            public int LikeCount { get; set; }
            public int DisLikeCount { get; set; }
        }


        PlaceDTO m_Activiti = new PlaceDTO();
        List<CommentPlace> m_Comments = new List<CommentPlace>();

        public SelectedActivityPage(int id)
        {
            InitializeComponent();
            m_webBrowser.InitializeFrom(Configuration.browser);

            MainWindow window = (MainWindow)Application.Current.MainWindow;
            if (window.GetType() == typeof(MainWindow))
            {
                Configuration.m_MenuVisible = Visibility.Visible;
                (window as MainWindow).m_menu.Visibility = Visibility.Visible;
                (window as MainWindow).m_ActivePage = MainWindow.ActivePage.SelectedActiviti;
            }


            HttpClient client = new HttpClient();
            HttpResponseMessage response = client.GetAsync($"https://localhost:44441/api/Places/GetPlaceWithCommentsAndLikes/{id}").Result;
            if (response.IsSuccessStatusCode)
            {
                var placeResp = response.Content.ReadFromJsonAsync<PlaceDTO>().Result;
                if (placeResp != null)
                {
                    m_Activiti = placeResp;
                }
            }
            else
            {
                MessageBox.Show("Incorrect response for url: \"https://localhost:44441/api/Places/GetWithCommentsAndLikes/" + id + "\" is: \"" + response.IsSuccessStatusCode.ToString() + '"');
            }


            //wyswietlenie na mapie miejsca
            string uri = $"https://www.google.com/maps/place/{Configuration.encodingUrlChracter(m_Activiti.Address)}/@{m_Activiti.Coordinates.Split(',')[0].Substring(0,10)},{m_Activiti.Coordinates.Split(',')[1].Substring(0, 10)},17z?hl=pl-PL&entry=ttu";
            Configuration.browser.Navigation.LoadUrl(uri);

            //wyswietl inforamcje
            m_Name.Content = m_Activiti.Name;
            m_Coordinates.Content = m_Activiti.Coordinates;
            m_Address.Content = m_Activiti.Address;
            m_Description.Text = m_Activiti.Description;

            int hour = m_Activiti.AverageTimeSpent / 60;
            int minutes = m_Activiti.AverageTimeSpent % 60;
            m_TimeSpent.Content = (hour == 0 ? hour + "0 godz. " : hour + " godz. ") + (minutes == 0 ? minutes + "0 min." : minutes + " min.");

            m_EstimatedCost.Content = m_Activiti.EstimatedCost;

            if(m_Activiti.PlaceAvailabilityTime != null)
            {
                m_OpenSun.Content =  dateToString(m_Activiti.PlaceAvailabilityTime.OpeningSunday);
                m_CloseSun.Content = dateToString(m_Activiti.PlaceAvailabilityTime.ClosingSunday);
                m_OpenMon.Content =  dateToString(m_Activiti.PlaceAvailabilityTime.OpeningMonday);
                m_CloseMon.Content = dateToString(m_Activiti.PlaceAvailabilityTime.ClosingMonday);
                m_OpenTue.Content =  dateToString(m_Activiti.PlaceAvailabilityTime.OpeningTuesday);
                m_CloseTue.Content = dateToString(m_Activiti.PlaceAvailabilityTime.ClosingTuesday);
                m_OpenWen.Content =  dateToString(m_Activiti.PlaceAvailabilityTime.OpeningWednesday);
                m_CloseWen.Content = dateToString(m_Activiti.PlaceAvailabilityTime.ClosingWednesday);
                m_OpenThu.Content =  dateToString(m_Activiti.PlaceAvailabilityTime.OpeningThursday);
                m_CloseThu.Content = dateToString(m_Activiti.PlaceAvailabilityTime.ClosingThursday);
                m_OpenFri.Content =  dateToString(m_Activiti.PlaceAvailabilityTime.OpeningFriday);
                m_CloseFri.Content = dateToString(m_Activiti.PlaceAvailabilityTime.ClosingFriday);
                m_OpenSat.Content =  dateToString(m_Activiti.PlaceAvailabilityTime.OpeningSaturday);
                m_CloseSat.Content = dateToString(m_Activiti.PlaceAvailabilityTime.ClosingSaturday);
            }

        }


        private string dateToString(int minutes)
        {
            string res = "";
            int hour = minutes / 60;
            int minute = minutes % 60;

            res = hour == 0 ? hour + "0:" : hour + ":";
            res += minute == 0 ? minute + "0" : minute.ToString();
            return res;
        }

        private double calculateAverageNote()
        {
            if (m_Activiti.Comments == null || m_Activiti.Comments.Count == 0)
                return 0;

            double total = 0.0;
            foreach(var com in m_Activiti.Comments)
            {
                total += com.Note;
            }

            return total / m_Activiti.Comments.Count;
        }

        private void Button_AddComment(object sender, RoutedEventArgs e)
        {
            this.NavigationService.GoBack();
        }

        private void Button_GoBack(object sender, RoutedEventArgs e)
        {
            this.NavigationService.GoBack();
        }
    }
}
