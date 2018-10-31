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

namespace Client.UserControls
{
    /// <summary>
    /// Логика взаимодействия для Autor.xaml
    /// </summary>
    public partial class Autor : UserControl
    {
        private readonly string AppPath;
        private readonly string token;
        private Grid MainGrid;
        private readonly int Id;

        public Autor(string app, string tok, Grid gr, int id)
        {
            Id = id;
            AppPath = app;
            token = tok;
            MainGrid = gr;
            InitializeComponent();
        }

        private async void UserControl_MouseDown(object sender, MouseButtonEventArgs e)
        {
            await GetAutor();
        }

        private async Task GetAutor()
        {
            try
            {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(AppPath);
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                    var response = await client.GetAsync($"api/picture/person-info/{Id}");

                    if (response.IsSuccessStatusCode)
                    {
                        var json = await response.Content.ReadAsStringAsync();

                        var per = await JsonConvert.DeserializeObjectAsync<PersonInfo>(json);

                        AutorById au = new AutorById(per.Id, MainGrid, AppPath);

                        Uri uri = new Uri(AppPath + per.Photo);
                        BitmapImage bm = new BitmapImage(uri);
                        au.AutorImg.Source = bm;

                        au.Autor.SelectedIndex = per.Autor ? 0 : 1;

                        au.Phone.Text = per.Phone;
                        au.Description.Text = per.Description;
                        au.FirstName.Text = per.FirstName;
                        au.LastName.Text = per.LastName;
                        au.MiddleName.Text = per.MiddleName;
                        au.Date.Text = per.Birthday?.ToShortDateString();

                        au.Margin = new Thickness(30, 0, 30, 0);

                        MainGrid.Children.Add(au);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
