using Client.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Client.Windows
{
    /// <summary>
    /// Логика взаимодействия для Registration.xaml
    /// </summary>
    public partial class Registration : Window
    {
        private readonly string AppPath;

        public Registration(string app)
        {
            AppPath = app;
            InitializeComponent();
        }

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            if(Password.Text != Password2.Text || Password.Text == string.Empty)
            {
                MessageBox.Show("Пароли не совпадают");
            }
            else
            {
                string emailPattern = @"^\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*$";
                bool isItEmail = Regex.IsMatch(Email.Text, emailPattern);
                if (!isItEmail)
                {
                    MessageBox.Show("Это не email-адрес, дурить вздумал?!");
                }
                else
                {
                    using (var client = new HttpClient())
                    {
                        var content = new RegistrationDTO
                        {
                            FirstName = FirstName.Text,
                            LastName = LastName.Text,
                            MiddleName = MiddleName.Text,
                            Email = Email.Text,
                            Number = Number.Text,
                            Password = Password.Text
                        };
                        if (Birthday.Text != string.Empty)
                            content.Birthday = Convert.ToDateTime(Birthday.Text);
                        else
                            content.Birthday = null;

                        client.BaseAddress = new Uri(AppPath);
                        client.DefaultRequestHeaders.Accept.Clear();
                        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                        var response =
                            await client.PostAsJsonAsync("api/account/registration", content);

                        if (response.IsSuccessStatusCode)
                        {
                            MessageBox.Show("Регистрация прошла успешно!");
                            this.Close();
                        }
                        else
                        {
                            MessageBox.Show(response.RequestMessage.ToString());
                        }
                    }
                }
            }
        }
    }
}
