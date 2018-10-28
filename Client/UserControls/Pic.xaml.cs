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
using Client;

namespace Client.UserControls
{
    /// <summary>
    /// Логика взаимодействия для Pic.xaml
    /// </summary>
    public partial class Pic : UserControl
    {
        private readonly int id;
        private readonly Grid MainGrid;
        private readonly string AppPath;

        public Pic(int id, Grid main, string app)
        {
            this.id = id;
            MainGrid = main;
            AppPath = app;
            InitializeComponent();
        }

        private void UserControl_KeyDown(object sender, KeyEventArgs e)
        {

        }

        private void UserControl_MouseEnter(object sender, MouseEventArgs e)
        {
           
        }

        private async void UserControl_MouseDown(object sender, MouseButtonEventArgs e)
        {
            await GetPictureById(id);
        }

        public async Task GetPictureById(int id)
        {
            try
            {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(AppPath);
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                    var response = await client.GetAsync($"api/picture/pictures/{id}");

                    if (response.IsSuccessStatusCode)
                    {
                        //MainGrid.Children.Remove(PictureGr);
                        var json = await response.Content.ReadAsStringAsync();

                        var per = await JsonConvert.DeserializeObjectAsync<Views.Picture>(json);

                        UserControls.Picture pic = new UserControls.Picture(MainGrid, per.Id, per.AutorId, AppPath);

                        Uri uri = new Uri(AppPath + per.PicturePath);
                        BitmapImage bm = new BitmapImage(uri);
                        pic.Picture1.Source = bm;

                        uri = new Uri(AppPath + per.Autor.Photo);
                        bm = new BitmapImage(uri);
                        pic.AutorImg.Source = bm;

                        pic.Name.Text = per.Name;

                        pic.Autor.Text = per.Autor.FullName;
                        
                        pic.Genre.Text = per.Genre;
                        pic.Date.Text = per.Date.ToShortDateString();
                        pic.Description.Text = per.Description;

                        pic.Margin = new Thickness(20d, 150, 20, 0);

                        MainGrid.Children.Add(pic);
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
