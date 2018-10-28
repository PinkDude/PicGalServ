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
    /// Логика взаимодействия для Picture.xaml
    /// </summary>
    public partial class Picture : UserControl
    {
        private readonly int id;
        private readonly int autorId;
        private readonly Grid mainGrid;
        private readonly string AppPath;

        public Picture(Grid mainGrid, int i, int a, string app)
        {
            AppPath = app;
            id = i;
            autorId = a;
            this.mainGrid = mainGrid;
            InitializeComponent();
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            mainGrid.Children.Remove(this);
        }

        private async void AutorImg_MouseDown(object sender, MouseButtonEventArgs e)
        {
            await GetAutor();
        }

        private async Task GetAutor()
        {
            try
            {
                using(var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(AppPath);
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                    var response = await client.GetAsync($"api/picture/person-info/{autorId}");

                    if (response.IsSuccessStatusCode)
                    {
                        var json = await response.Content.ReadAsStringAsync();

                        var per = await JsonConvert.DeserializeObjectAsync<PersonInfo>(json);

                        AutorById au = new AutorById(per.Id, mainGrid, AppPath);

                        Uri uri = new Uri(AppPath + per.Photo);
                        BitmapImage bm = new BitmapImage(uri);
                        au.AutorImg.Source = bm;

                        if(per.Autor)
                            au.Autor.IsChecked = true;

                        au.Phone.Text = per.Phone;
                        au.Description.Text = per.Description;
                        au.FirstName.Text = per.FirstName;
                        au.LastName.Text = per.LastName;
                        au.MiddleName.Text = per.MiddleName;
                        au.Date.Text = per.Birthday?.ToShortDateString();

                        au.Margin = new Thickness(10, 150, 10, 0);

                        mainGrid.Children.Add(au);
                    }
                }
            }
            catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
