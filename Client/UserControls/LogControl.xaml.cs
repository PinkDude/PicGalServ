using Client.Views;
using Client.Windows;
using Newtonsoft.Json;
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

namespace Client.UserControls
{
    /// <summary>
    /// Логика взаимодействия для LogControl.xaml
    /// </summary>
    public partial class LogControl : UserControl
    {
        private readonly string AppPath;
        private readonly Grid LogGrid;
        private readonly Grid commonGrid;

        public LogControl(string app, Grid log, Grid com)
        {
            AppPath = app;
            LogGrid = log;
            commonGrid = com;
            InitializeComponent();
        }

        private async void Button_Click_1(object sender, RoutedEventArgs e)
        {
            var token = await GetTokenDictionary(UserName.Text, Password.Text);
            MainWindow.token = token.Access_token;
            MainWindow.Id = token.Id;
            MainWindow.Role = token.Role.Replace(" ", "");

            LogProf prof = new LogProf(AppPath, token.Username, token.Photo, commonGrid);
            LogGrid.Children.Add(prof);

            LogGrid.Children.Remove(this);
        }

        private async Task<TokenView> GetTokenDictionary(string userName, string password)
        {
            var pairs = new
            {
                Email = userName,
                Password = password
            };

            using (var client = new HttpClient())
            {
                var response =
                    await client.PostAsJsonAsync(AppPath + "api/account/token", pairs);
                var result = await response.Content.ReadAsStringAsync();

                var res = await JsonConvert.DeserializeObjectAsync<TokenView>(result);
                return res;
            }
        }


        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            Registration reg = new Registration(AppPath);
            reg.ShowDialog();
        }
    }
}
