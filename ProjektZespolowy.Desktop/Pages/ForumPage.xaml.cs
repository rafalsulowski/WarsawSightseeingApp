using ProjektZespolowy.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
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
using ProjektZespolowy.Models.JoinedTables;
using System.Net.Http.Json;
using ProjektZespolowy.Models.DTO;

namespace ProjektZespolowy.Desktop.Pages
{
    /// <summary>
    /// Interaction logic for ForumPage.xaml
    /// </summary>
    public partial class ForumPage : Page
    {
        private class PostBasicInfo
        {
            public int Id { get; set; }
            public string Title { get; set; }
            public string Author { get; set; }
            public string PostType { get; set; }
            public int LikesCount { get; set; }
            public int CommentCount { get; set; }
        }

        List<PostDTO> m_Posts = new List<PostDTO>();
        List<PostBasicInfo> m_PostsBI = new List<PostBasicInfo>();

        public ForumPage(User? user = null)
        {
            InitializeComponent();
            MainWindow window = (MainWindow)Application.Current.MainWindow;
            if (window.GetType() == typeof(MainWindow))
            {
                Configuration.m_MenuVisible = Visibility.Visible;
                (window as MainWindow).m_menu.Visibility = Visibility.Visible;
                (window as MainWindow).m_ActivePage = MainWindow.ActivePage.Forum;
            }


            HttpClient client = new HttpClient();
            HttpResponseMessage response;
            if (user == null)
            {
                response = client.GetAsync("https://localhost:44441/api/Post/GetAllPostsWithCommentsAndLikes").Result;
                if (response.IsSuccessStatusCode)
                {
                    var resp = response.Content.ReadFromJsonAsync<IEnumerable<PostDTO>>().Result;
                    if (resp != null)
                    {
                        m_Posts = new List<PostDTO>(resp);
                    }
                }
                else
                {
                    MessageBox.Show("Incorrect response for url: \"https://localhost:44441/api/Post/GetAllPostsWithCommentsAndLikes\" is: \"" + response.IsSuccessStatusCode.ToString() + '"');
                }
            }
            else
            {
                response = client.GetAsync($"https://localhost:44441/api/Users/{user.Id}/Posts").Result;
                if (response.IsSuccessStatusCode)
                {
                    var resp = response.Content.ReadFromJsonAsync<UserDTO<PostDTO>>().Result;
                    if (resp != null)
                    {
                        m_Posts = resp.Data;
                    }
                }
                else
                {
                    MessageBox.Show($"Incorrect response for url: \"https://localhost:44441/api/Users/{user.Id}/Posts\" is: \"" + response.IsSuccessStatusCode.ToString() + '"');
                }
            }

            foreach (var post in m_Posts)
            {
                var newpost = new PostBasicInfo
                {
                    Id = post.Id,
                    Author = post.Author,
                    Title = post.Title,
                    LikesCount = post.LikesCount,
                    CommentCount = post.CommentsCount
                };

                if (post.Type == PostType.Discussion)
                    newpost.PostType = "dyskusja";
                else if (post.Type == PostType.Information)
                    newpost.PostType = "informacyjny";
                else if (post.Type == PostType.Voting)
                    newpost.PostType = "ankieta";

                m_PostsBI.Add(newpost);
            }

            m_ActivitiesList.Items.Clear();
            m_ActivitiesList.ItemsSource = m_PostsBI;
        }
    
        private void PlaceholdersListBox_OnPreviewMouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            var item = ItemsControl.ContainerFromElement(sender as ListBox, e.OriginalSource as DependencyObject) as ListBoxItem;
            if (item != null)
            {
                PostBasicInfo post = (PostBasicInfo)item.Content;
                this.NavigationService.Navigate(new SelectedPostPage(post.Id));
            }
        }

        private void ComboBox_SelectionChanged(object sender, RoutedEventArgs e)
        {
            ComboBox cmb = sender as ComboBox;
            int sel = cmb.SelectedIndex;

            switch (sel)
            {
                case 0:
                    m_PostsBI.Sort((a, b) => a.Title.CompareTo(b.Title));
                    m_ActivitiesList.Items.Refresh();
                    break;
                case 1:
                    m_PostsBI.Sort((a, b) => a.Title.CompareTo(b.Title));
                    m_PostsBI.Reverse();
                    m_ActivitiesList.Items.Refresh();
                    break;
                case 2:
                    m_PostsBI.Sort((a, b) => a.CommentCount.CompareTo(b.CommentCount));
                    m_ActivitiesList.Items.Refresh();
                    break;
                case 3:
                    m_PostsBI.Sort((a, b) => a.CommentCount.CompareTo(b.CommentCount));
                    m_PostsBI.Reverse();
                    m_ActivitiesList.Items.Refresh();
                    break;
                case 4:
                    m_PostsBI.Sort((a, b) => a.LikesCount.CompareTo(b.LikesCount));
                    m_ActivitiesList.Items.Refresh();
                    break;
                case 5:
                    m_PostsBI.Sort((a, b) => a.LikesCount.CompareTo(b.LikesCount));
                    m_PostsBI.Reverse();
                    m_ActivitiesList.Items.Refresh();
                    break;
                default:
                    break;
            }
        }

        private void Button_AddPost(object sender, RoutedEventArgs e)
        {
            this.NavigationService.Navigate(new CreatePostPage());
        }
    }
}
