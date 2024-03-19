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
using Azure;
using System.Drawing.Drawing2D;

namespace ProjektZespolowy.Desktop.Pages
{
    /// <summary>
    /// Interaction logic for SelectedPostPage.xaml
    /// </summary>
    public partial class SelectedPostPage : Page
    {
        PostDTO m_Post = new PostDTO();
        public SelectedPostPage(int id)
        {
            InitializeComponent();
            MainWindow window = (MainWindow)Application.Current.MainWindow;
            if (window.GetType() == typeof(MainWindow))
            {
                Configuration.m_MenuVisible = Visibility.Visible;
                (window as MainWindow).m_menu.Visibility = Visibility.Visible;
                (window as MainWindow).m_ActivePage = MainWindow.ActivePage.SelectedActiviti;
            }

            //pobierz informacje o poscie
            HttpResponseMessage response = Configuration.m_Client.GetAsync($"https://localhost:44441/api/Post/GetPostWithCommentsAndLikes/{id}").Result;
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
                MessageBox.Show("Incorrect response for url: \"https://localhost:44441/api/Post/GetPostWithCommentsAndLikes/" + id + "\" is: \"" + response.IsSuccessStatusCode.ToString() + '"');
            }

            //jesli zalogowany uzytkownik to tworca posta to udostepnij opcje usuwania i edycji
            if (m_Post.UserId == Configuration.m_ActualLoggerdUser.Id)
            {
                m_BtnDelete.Visibility = Visibility.Visible;
                m_BtnEdit.Visibility = Visibility.Visible;
            }

            //wyswietl inforamcje
            m_Author.Content = m_Post.Author;
            m_Title.Content = "Post \"" + m_Post.Title + "\"";
            m_LikesCount.Content = m_Post.LikesCount;
            m_commentCount.Content = m_Post.CommentsCount;
            m_Content.Text = m_Post.Content;

            if (m_Post.Type == PostType.Discussion)
                m_PostType.Content = "Dyskusja";
            else if (m_Post.Type == PostType.Information)
                m_PostType.Content = "Informacyjny";
            else if (m_Post.Type == PostType.Voting)
                m_PostType.Content = "Ankieta";

            if(m_Post.CommentsCount == 0)
                m_BtnGoToComments.IsEnabled = false;


            //sprawdzic jaki obraz dla laika zaladować, wypiełniony dla polubionych już atrakcji
            m_LikeIcon.Source = new Uri("..\\images\\beforelike.svg", UriKind.Relative);
            /*foreach(var like in m_Post.Likes)
                if(like.UserId == Configuration.m_ActualLoggerdUser.Id)
                    m_LikeIcon.Source = new Uri("C:\\Users\\01159502\\Desktop\\develop\\ProjektZespolowy\\ProjektZespolowy.Desktop\\images\\afterlike.svg");*/

            if (m_Post.Type != PostType.Voting)
            if (m_Post.Type != PostType.Voting)
            {
                m_BtnVotesFor.IsEnabled = false;
                m_BtnVotesAgainst.IsEnabled = false;
            }

            m_BtnVotesFor.Content = "Zagłosowano za: " + m_Post.VotesFor;
            m_BtnVotesAgainst.Content = "Zagłosowano przeciw: " + m_Post.VotesAgainst;
        }

        private void Button_GoBack(object sender, RoutedEventArgs e)
        {
            this.NavigationService.Navigate(new ForumPage());
        }
        private void Button_AddComment(object sender, RoutedEventArgs e)
        {
            this.NavigationService.Navigate(new AddComment(m_Post));
        }
        private void Button_VoteFor(object sender, RoutedEventArgs e)
        {
            if (m_BtnVotesAgainst.IsEnabled == false)
            {
                m_Post.VotesAgainst--;
                m_BtnVotesAgainst.Content = "Zagłosowano przeciw: " + m_Post.VotesAgainst;

            }
            m_Post.VotesFor++;
            m_BtnVotesFor.IsEnabled = false;
            m_BtnVotesAgainst.IsEnabled = true;
            m_PostVotingInformation.Content = "(Oddano swój głos za)";
            m_BtnVotesFor.Content = "Zagłosowano za: " + m_Post.VotesFor;
        }

        private void Button_VoteAgainst(object sender, RoutedEventArgs e)
        {
            if (m_BtnVotesFor.IsEnabled == false)
            {
                m_Post.VotesFor--;
                m_BtnVotesFor.Content = "Zagłosowano za: " + m_Post.VotesFor;
            }
            m_Post.VotesAgainst++;
            m_BtnVotesAgainst.IsEnabled = false;
            m_BtnVotesFor.IsEnabled = true;
            m_PostVotingInformation.Content = "(Oddano swój głos przeciw)";
            m_BtnVotesAgainst.Content = "Zagłosowano przeciw: " + m_Post.VotesAgainst;
        }

        private void Button_Like(object sender, RoutedEventArgs e)
        {
            HttpResponseMessage response;

            if (m_Post.Likes != null && m_Post.Likes.Count != 0 && m_Post.Likes.Find(u => u.UserId == Configuration.m_ActualLoggerdUser.Id) != null)
            {
                response = Configuration.m_Client.DeleteAsync($"https://localhost:44441/api/Post/DeletePostLike?postId={m_Post.Id}").Result;
                if (!response.IsSuccessStatusCode)
                {
                    MessageBox.Show($"Nie udało się anulowac like'a");
                }
                else
                {
                    HttpResponseMessage response2 = Configuration.m_Client.GetAsync($"https://localhost:44441/api/Post/GetPostWithCommentsAndLikes/{m_Post.Id}").Result;
                    if (response2.IsSuccessStatusCode)
                    {
                        var PostResp = response2.Content.ReadFromJsonAsync<PostDTO>().Result;
                        if (PostResp != null)
                        {
                            m_Post = PostResp;
                        }
                    }
                    else
                    {
                        MessageBox.Show("Incorrect response for url: \"https://localhost:44441/api/Post/GetPostWithCommentsAndLikes/" + m_Post.Id + "\" is: \"" + response.IsSuccessStatusCode.ToString() + '"');
                    }

                    m_LikesCount.Content = m_Post.LikesCount; 
                    m_LikeIcon.Source = new Uri("C:\\Users\\01159502\\Desktop\\develop\\ProjektZespolowy\\ProjektZespolowy.Desktop\\images\\beforelike.svg");

                }
            }
            else
            {
                string jsonString = "{\"postId\": " + m_Post.Id + ", \"isLiked\": true}";
                response = Configuration.m_Client.PostAsync("https://localhost:44441/api/Post/LikePost", new StringContent(jsonString, Encoding.UTF8, "application/json")).Result;

                if (!response.IsSuccessStatusCode)
                {
                    MessageBox.Show($"Nie udało się dać like'a");
                }
                else
                {
                    HttpResponseMessage response2 = Configuration.m_Client.GetAsync($"https://localhost:44441/api/Post/GetPostWithCommentsAndLikes/{m_Post.Id}").Result;
                    if (response2.IsSuccessStatusCode)
                    {
                        var PostResp = response2.Content.ReadFromJsonAsync<PostDTO>().Result;
                        if (PostResp != null)
                        {
                            m_Post = PostResp;
                        }
                    }
                    else
                    {
                        MessageBox.Show("Incorrect response for url: \"https://localhost:44441/api/Post/GetPostWithCommentsAndLikes/" + m_Post.Id + "\" is: \"" + response.IsSuccessStatusCode.ToString() + '"');
                    }

                    m_LikeIcon.Source = new Uri("C:\\Users\\01159502\\Desktop\\develop\\ProjektZespolowy\\ProjektZespolowy.Desktop\\images\\afterlike.svg");
                    m_LikesCount.Content = m_Post.LikesCount;
                }
            }
        }

        private void Button_GoComments(object sender, RoutedEventArgs e)
        {
            this.NavigationService.Navigate(new PostCommentsPage(m_Post));
        }

        private void Button_Edit(object sender, RoutedEventArgs e)
        {

            this.NavigationService.Navigate(new CreatePostPage(m_Post, true));
        }

        private void Button_Delete(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Czy na pewno chcesz usunąc posta?", "Uwaga!", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                HttpResponseMessage response = Configuration.m_Client.DeleteAsync($"https://localhost:44441/api/Post/{m_Post.Id}").Result;
                if (response.IsSuccessStatusCode)
                {
                    MessageBox.Show("Usunięto posta");
                    this.NavigationService.Navigate(new ForumPage());
                }
                else
                {
                    MessageBox.Show("Nie udało się usunąć posta");
                }
            }
        }
    }
}
