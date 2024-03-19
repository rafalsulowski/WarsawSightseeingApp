using Microsoft.IdentityModel.Tokens;
using ProjektZespolowy.Models;
using ProjektZespolowy.Models.DTO;
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
using System.Xml.Linq;

namespace ProjektZespolowy.Desktop.Pages
{
    /// <summary>
    /// Interaction logic for AddComment.xaml
    /// </summary>
    public partial class AddComment : Page
    {
        private PostDTO m_Post = new PostDTO();
        public AddComment(PostDTO post)
        {
            InitializeComponent();
            m_Post = post;
            m_Title.Content = "Tworzenie komentarza do posta: \"" + m_Post.Title + "\"";
        }

        private void Button_Add(object sender, RoutedEventArgs e)
        {
            string content = m_Content.Text;

            if(content.IsNullOrEmpty())
            {
                MessageBox.Show("Treść komentarza nie może być pusta!");
                return;
            }


            string jsonString = "{\"content\":\"" + content + "\", \"userId\": " + 1 + ", \"postId\": " + m_Post.Id + "}";
            HttpResponseMessage response = Configuration.m_Client.PostAsync("https://localhost:44441/api/Post/AddCommentToPost", new StringContent(jsonString, Encoding.UTF8, "application/json")).Result;
            if (!response.IsSuccessStatusCode)
            {
                MessageBox.Show($"Niepowodzenie w dodawaniu komentarza!");
                return;
            }

            MessageBox.Show($"Dodano komentarz");
            this.NavigationService.Navigate(new PostCommentsPage(m_Post));
        }

        private void Button_GoBack(object sender, RoutedEventArgs e)
        {
            this.NavigationService.GoBack();
        }

        private void Button_Cancel(object sender, RoutedEventArgs e)
        {
            this.NavigationService.Navigate(new PostCommentsPage(m_Post));
        }
    }
}
