using Azure.Core;
using Microsoft.IdentityModel.Tokens;
using ProjektZespolowy.Models;
using ProjektZespolowy.Models.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
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
    /// Interaction logic for CreatePostPage.xaml
    /// </summary>
    public partial class CreatePostPage : Page
    {
        private bool m_Modify = false;
        private PostDTO m_Post = new PostDTO();

        public CreatePostPage()
        {
            InitializeComponent();
            MainWindow window = (MainWindow)Application.Current.MainWindow;
            if (window.GetType() == typeof(MainWindow))
            {
                Configuration.m_MenuVisible = Visibility.Visible;
                (window as MainWindow).m_menu.Visibility = Visibility.Visible;
                (window as MainWindow).m_ActivePage = MainWindow.ActivePage.CreatePost;
            }
        }

        public CreatePostPage(PostDTO post, bool modify)
        {
            InitializeComponent();
            MainWindow window = (MainWindow)Application.Current.MainWindow;
            if (window.GetType() == typeof(MainWindow))
            {
                Configuration.m_MenuVisible = Visibility.Visible;
                (window as MainWindow).m_menu.Visibility = Visibility.Visible;
                (window as MainWindow).m_ActivePage = MainWindow.ActivePage.CreatePost;
            }

            m_Modify = modify;
            m_Post = post;

            if (m_Modify)
            {
                m_pageTitle.Content = "Edycja posta: " + m_Post.Title;
                m_Title.Text = m_Post.Title;
                m_Content.Text = m_Post.Content;
                m_postType.SelectedIndex = (int)m_Post.Type;
                m_BtnConfirm.Content = "Zatwierdz";
            }
        }

        private void Button_CreatePost(object sender, RoutedEventArgs e)
        {
            HttpResponseMessage response;


            //walidacja
            string title = m_Title.Text;
            string content = m_Content.Text;
            int postType = m_postType.SelectedIndex;
            PostType type = new PostType(); 

            switch (postType)
            {
                case 0:
                    type = PostType.Information;
                    break;
                case 1:
                    type = PostType.Discussion;
                    break;
                case 2:
                    type = PostType.Voting;
                    break;
                default:
                    type = PostType.Information;
                    break;
            }


            if(title.IsNullOrEmpty() || content.IsNullOrEmpty() || postType == -1)
            {
                MessageBox.Show("Zostały wporwadzone błędne dane");
                return;
            }

            string jsonString;
            if (m_Modify)
            {
                m_Post.Content = content;
                m_Post.Title = title;
                m_Post.Type = type;

                jsonString = JsonSerializer.Serialize(m_Post);
                response = Configuration.m_Client.PutAsync($"https://localhost:44441/api/Post", new StringContent(jsonString, Encoding.UTF8, "application/json")).Result;
                if (response.IsSuccessStatusCode)
                {
                    MessageBox.Show("Post został zmodyfikowany!");
                    this.NavigationService.Navigate(new ForumPage());
                }
                else
                {
                    MessageBox.Show("Nie udało się zmodyfikować posta!");
                }
                return;
            }


            CreatePostDTO newPost = new CreatePostDTO
            {
                Title = title,
                Content = content,
                Type = type,
                VotesAgainst = 0,
                VotesFor = 0
            };


            //stworzenie posta w webapi
            jsonString = JsonSerializer.Serialize(newPost);
            response = Configuration.m_Client.PostAsync("https://localhost:44441/api/Post/AddPost", new StringContent(jsonString, Encoding.UTF8, "application/json")).Result;
            if (response.IsSuccessStatusCode)
            {
                MessageBox.Show("Poprawnie utworzono nowy post", "Sukces", MessageBoxButton.OK);
                this.NavigationService.Navigate(new ForumPage());
            }
            else
            {
                MessageBox.Show("Incorrect response for url: \"https://localhost:7118/api/Post/AddPost\" is: \"" + response.IsSuccessStatusCode.ToString() + '"');
                return;
            }
        }

        private void Button_GoBack(object sender, RoutedEventArgs e)
        {
            this.NavigationService.GoBack();
        }
    }
}
