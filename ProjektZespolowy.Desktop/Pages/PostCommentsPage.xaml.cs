using ProjektZespolowy.Models.DTO;
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
using System.Net.Http.Json;
using Azure;
using System.Text.RegularExpressions;
using System.Text.Json;

namespace ProjektZespolowy.Desktop.Pages
{
    public partial class PostCommentsPage : Page
    {
        public class CommentBasicInfo {
            public CommentDTO Comment { get; set; }
            public string Src { get; set; }
            public string Visibility { get; set; }
        }


        PostDTO m_Post = new PostDTO();
        List<CommentBasicInfo> m_comBI = new List<CommentBasicInfo>();
        public PostCommentsPage(PostDTO post)
        {
            InitializeComponent();
            MainWindow window = (MainWindow)Application.Current.MainWindow;
            if (window.GetType() == typeof(MainWindow))
            {
                Configuration.m_MenuVisible = Visibility.Visible;
                (window as MainWindow).m_menu.Visibility = Visibility.Visible;
                (window as MainWindow).m_ActivePage = MainWindow.ActivePage.PostComments;
            }

            HttpResponseMessage response = Configuration.m_Client.GetAsync($"https://localhost:44441/api/Post/GetPostWithCommentsAndCommentLikes/{post.Id}").Result;
            if (response.IsSuccessStatusCode)
            {
                var PostResp = response.Content.ReadFromJsonAsync<PostDTO>().Result;
                if (PostResp != null)
                {
                    m_Post = PostResp;
                }
            }
            else
            {
                MessageBox.Show("Incorrect response for url: \"https://localhost:44441/api/Post/GetPostWithCommentsAndCommentLikes/" + post.Id + "\" is: \"" + response.IsSuccessStatusCode.ToString() + '"');
            }

            m_Title.Content = "Komentarze do posta: \"" + m_Post.Title.Substring(0,m_Post.Title.Length < 21 ? m_Post.Title.Length : 21) + "\"";


            foreach(var el in m_Post.Comments)
            {
                CommentBasicInfo cbi = new CommentBasicInfo { Comment = el };

                if (el.AuthorId == Configuration.m_ActualLoggerdUser.Id)
                    cbi.Visibility = "Visible";
                else
                    cbi.Visibility = "Hidden";
                
                if (el.Likes != null && el.Likes.Count != 0 && el.Likes.Find(u => u.UserId == Configuration.m_ActualLoggerdUser.Id) != null)
                {
                    cbi.Src = "C:\\Users\\01159502\\Desktop\\develop\\ProjektZespolowy\\ProjektZespolowy.Desktop\\images\\afterlike.svg";
                }
                else
                {
                    cbi.Src = "C:\\Users\\01159502\\Desktop\\develop\\ProjektZespolowy\\ProjektZespolowy.Desktop\\images\\beforelike.svg";
                }
                m_comBI.Add(cbi);
            }

            m_ActivitiesList.ItemsSource = m_comBI;
        }

        private void Button_Like(object sender, RoutedEventArgs e)
        {
            Button button = sender as Button;
            CommentBasicInfo cbi = button.DataContext as CommentBasicInfo;
            int match = m_comBI.FindIndex(u => u.Comment.Id == cbi.Comment.Id);

            if(match == -1)
            {
                MessageBox.Show($"Nie udało się zidentyfikowac komentarza");
                return;
            }            

            HttpResponseMessage response;
            if (m_comBI[match].Comment.Likes != null && m_comBI[match].Comment.Likes.Count != 0 && m_comBI[match].Comment.Likes.Find(u => u.UserId == Configuration.m_ActualLoggerdUser.Id) != null)
            {
                response = Configuration.m_Client.DeleteAsync($"https://localhost:44441/api/Post/DeleteLikeCommentPost?commentId={m_comBI[match].Comment.Id}").Result;
                if (!response.IsSuccessStatusCode)
                {
                    MessageBox.Show($"Nie udało się anulowac like'a");
                }
                else
                {
                    RefreshInfo(match, false);
                }
            }
            else
            {
                string jsonString = "{\"commentId\": " + m_comBI[match].Comment.Id + ", \"isLiked\": true}";
                response = Configuration.m_Client.PostAsync("https://localhost:44441/api/Post/LikeComment", new StringContent(jsonString, Encoding.UTF8, "application/json")).Result;

                if (!response.IsSuccessStatusCode)
                {
                    MessageBox.Show($"Nie udało się dać like'a");
                }
                else
                {
                    RefreshInfo(match, true);
                }
            }

            
            m_ActivitiesList.ItemsSource = m_comBI;
        }

        private void Button_GoBack(object sender, RoutedEventArgs e)
        {
            this.NavigationService.Navigate(new SelectedPostPage(m_Post.Id));
        }

        private void Button_AddComment(object sender, RoutedEventArgs e)
        {
            this.NavigationService.Navigate(new AddComment(m_Post));
        }

        private void Button_Delete(object sender, RoutedEventArgs e)
        {
            Button button = sender as Button;
            CommentBasicInfo cbi = button.DataContext as CommentBasicInfo;

            if (MessageBox.Show("Czy na pewno chcesz usunąc komentarz?", "Uwaga!", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                HttpResponseMessage response = Configuration.m_Client.DeleteAsync($"https://localhost:44441/api/Post/DeleteCommentPost?commentId={cbi.Comment.Id}").Result;
                if (response.IsSuccessStatusCode)
                {
                    MessageBox.Show("Usunięto komentarz");
                    this.NavigationService.Navigate(new PostCommentsPage(m_Post));
                }
                else
                {
                    MessageBox.Show("Nie udało się usunąć komentarza");
                }
            }
        }

        

        private void RefreshInfo(int match, bool isLike)
        {
            //aktualizacja informacji na stronie
            HttpResponseMessage response2 = Configuration.m_Client.GetAsync($"https://localhost:44441/api/Post/GetPostWithCommentsAndCommentLikes/{m_Post.Id}").Result;
            if (response2.IsSuccessStatusCode)
            {
                var PostResp = response2.Content.ReadFromJsonAsync<PostDTO>().Result;
                if (PostResp != null)
                {
                    //aktualizacja tego komentarza
                    CommentDTO com = PostResp.Comments.Find(u => u.Id == m_comBI[match].Comment.Id);
                    if (com != null)
                    {
                        m_comBI[match].Comment = com;
                        if(isLike)
                            m_comBI[match].Src = "C:\\Users\\01159502\\Desktop\\develop\\ProjektZespolowy\\ProjektZespolowy.Desktop\\images\\afterlike.svg";
                        else
                            m_comBI[match].Src = "C:\\Users\\01159502\\Desktop\\develop\\ProjektZespolowy\\ProjektZespolowy.Desktop\\images\\beforelike.svg";
                    }
                    else
                    {
                        MessageBox.Show($"Nie udało się odswiezyc strony");
                    }
                }
            }
            else
            {
                MessageBox.Show("Incorrect response for url: \"https://localhost:44441/api/Post/GetPostWithCommentsAndCommentLikes/" + m_Post.Id + "\" is: \"" + response2.IsSuccessStatusCode.ToString() + '"');
            }
            m_ActivitiesList.Items.Refresh();
        }
    }
}
