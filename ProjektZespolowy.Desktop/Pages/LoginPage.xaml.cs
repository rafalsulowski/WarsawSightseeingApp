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
using ProjektZespolowy.Models;
using System.Net.Http.Headers;
using System.Net.Http;
using System.Net.Http.Json;
using System.Security.Policy;
using System.Text.Json.Nodes;
using System.Net;
using System.Text.Json.Serialization;
using Newtonsoft.Json;
using ProjektZespolowy.Models.DTO;
using Azure;
using Microsoft.IdentityModel.Tokens;

namespace ProjektZespolowy.Desktop.Pages
{
    /// <summary>
    /// Interaction logic for LoginPage.xaml
    /// </summary>
    public partial class LoginPage : Page
    {
        public LoginPage()
        {
            InitializeComponent();
            m_BtnGoogleLogin.IsEnabled = false;
        }

        private void Button_Go_Back(object sender, RoutedEventArgs e)
        {
            this.NavigationService.Navigate(new WelcomPage());
        }

        private void Button_Login(object sender, RoutedEventArgs e)
        {
            string login = m_loginTextBox.Text;
            string pass = m_PasswordTextBox.Password;
            //string login = "qwe";
            //string pass = "qwe";
            if (login.IsNullOrEmpty() || pass.IsNullOrEmpty())
            {
                MessageBox.Show("Login lub hasło zostały nie uzupełnione!");
                return;
            }


            string jsonString = "{\"email\": \"" + login + "\", \"password\": \"" + pass + "\"}";
            HttpResponseMessage response = Configuration.m_Client.PostAsync("https://localhost:44441/api/Users/login", new StringContent(jsonString, Encoding.UTF8, "application/json")).Result;

            if (response.IsSuccessStatusCode)
            {
                string token = response.Content.ReadAsStringAsync().Result;
                Configuration.m_JwtToken = token;

                //pobranie danych uzupelniajacych o uzytkowniku
                response = Configuration.m_Client.GetAsync($"https://localhost:44441/api/Users/email?email={login}").Result;
                User? user = response.Content.ReadFromJsonAsync<User>().Result;
                if(user != null)
                {
                    //jeśli wszystko poszło zgodnie z planem:
                    Configuration.m_ActualLoggerdUser = user;
                    Configuration.m_Client.DefaultRequestHeaders.Add("Authorization", "Bearer " + Configuration.m_JwtToken);

                    MessageBox.Show("Poprawnie zalogowano!");
                    this.NavigationService.Navigate(new HomePage());
                }
                else
                {
                    MessageBox.Show("Nie odnaleziono użytkownika w bazie danych!");
                    return;
                }
            }
            else
            {
                MessageBox.Show("Nieprawidłowe dane logowania!");
            }
        }

        private void Button_Login_With_Google(object sender, RoutedEventArgs e)
        {
            //api do logowania z google
        }

    }
}
