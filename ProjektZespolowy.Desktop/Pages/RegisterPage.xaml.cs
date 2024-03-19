using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
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

namespace ProjektZespolowy.Desktop.Pages
{
    /// <summary>
    /// Interaction logic for RegisterPage.xaml
    /// </summary>
    public partial class RegisterPage : Page
    {
        public RegisterPage()
        {
            InitializeComponent();
        }

        private void Button_Go_Back(object sender, RoutedEventArgs e)
        {
            this.NavigationService.Navigate(new WelcomPage());
        }

        private void Button_Register(object sender, RoutedEventArgs e)
        {
            string login = m_Login.Text;
            string pass = m_Password.Password;
            string pass2 = m_RetypePassword.Password;
            string username = m_UserName.Text;

            if(pass.IsNullOrEmpty() || pass2.IsNullOrEmpty() || login.IsNullOrEmpty() || username.IsNullOrEmpty())
            {
                MessageBox.Show("Niepoprawne dane!");
                return;
            }

            if(pass != pass2)
            {
                MessageBox.Show("Hasła muszą być takie same!");
                return;
            }

            RegisterUserDTO registerUserDTO = new RegisterUserDTO { Email = m_Login.Text, Password = m_Password.Password, 
                ConfirmPassword = m_RetypePassword.Password, Username = m_UserName.Text};

            HttpClient client = new HttpClient();
            var content = new StringContent(JsonConvert.SerializeObject(registerUserDTO), Encoding.UTF8, "application/json");
            HttpResponseMessage response = client.PostAsync("https://localhost:44441/api/Users", content).Result;
            if (response.IsSuccessStatusCode)
            {
                this.NavigationService.Navigate(new SuccessRegisterPage());
            }
            else
            {
                MessageBox.Show("Incorrect credentials!");
            }
        }
    }
}
