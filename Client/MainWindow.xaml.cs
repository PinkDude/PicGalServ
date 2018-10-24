using Client.Views;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
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

namespace Client
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private const string AppPath = "https://localhost:44323";
        private static string token;

        public MainWindow()
        {
            InitializeComponent();
        }

        private async void Button_Click_1(object sender, RoutedEventArgs e)
        {
            try
            {
                using(var client = new HttpClient())
                {
                    client.BaseAddress = new Uri("http://localhost:44323/");
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    Label1.Content = "В процессе";

                    var response = await client.GetAsync("api/picture/test");
                    if (response.IsSuccessStatusCode)
                        Label1.Content = response.Content.ReadAsStringAsync().Result;
                    else
                        Label1.Content = "Не получилось";
                }
            }
            catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
            //try
            //{
            //    List<PersonInfo> per = new List<PersonInfo>();
            //    using (var client = new HttpClient())
            //    {
            //        var responce = client.GetAsync(AppPath + "/api/picture/person-info").ContinueWith((taskwithresponse =>
            //        {
            //            var response = taskwithresponse.Result;
            //            var jsonString = response.Content.ReadAsStringAsync();
            //            jsonString.Wait();
            //            per = JsonConvert.DeserializeObject<List<PersonInfo>>(jsonString.Result);
            //        }));
            //        responce.Wait();
            //    }
            //    Label1.Content = per[0].LastName;
            //}
            //catch (Exception ex)
            //{
            //    throw new Exception(ex.Message);
            //}
        }
    }
}
