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
    /// Логика взаимодействия для PictureGrid.xaml
    /// </summary>
    public partial class PictureGrid : UserControl
    {
        private const string AppPath = "https://localhost:44323/";
        private static string token;
        private readonly Grid MainGrid;

        public PictureGrid(string tok, Grid main)
        {
            token = tok;
            MainGrid = main;
            InitializeComponent();
        }

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            await SearchPicturesAsync();
        }

        private async void Grid_Loaded(object sender, RoutedEventArgs e)
        {
            await GetGenres();
            await SearchPicturesAsync();
            //PictureView.MinWidth = BackGrid.Width - 20d;
        }

        private async Task SearchPicturesAsync()
        {
            try
            {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(AppPath);
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                    var response = await client.GetAsync($"api/picture/pictures?Name={Search.Text}&genreId={Genres.SelectedIndex.ToString()}");

                    if (response.IsSuccessStatusCode)
                    {
                        PictureView.Children.Clear();

                        var json = await response.Content.ReadAsStringAsync();

                        var per = await JsonConvert.DeserializeObjectAsync<List<Views.Picture>>(json);

                        double left = 5d, top = 5d;
                        int i = 0;
                        foreach (var s in per)
                        {
                            Pic pic = new Pic(s.Id, MainGrid, AppPath)
                            {
                                HorizontalAlignment = HorizontalAlignment.Left,
                                VerticalAlignment = VerticalAlignment.Top,
                                //Width = 250,
                                //Height = 450
                            };

                            pic.Id.Text = s.Id.ToString();

                            Uri uri = new Uri(AppPath + s.PicturePath);
                            BitmapImage bm = new BitmapImage(uri);
                            pic.Picture.Source = bm;

                            pic.Name.Content = s.Name;

                            pic.Autor.Content = s.Autor.ShotName;
                            pic.Genre.Content = s.Genre;
                            pic.Date.Content = s.Date.ToShortDateString();

                            pic.Margin = new Thickness(left, top, 0, 0);

                            if (i > 3)
                            {
                                left = 5;
                                top += pic.Height + 5d;
                                i = 0;
                            }
                            else
                            {
                                left += pic.Width + 5d;
                                i++;
                            }

                            PictureView.Children.Add(pic);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        private async Task GetGenres()
        {
            try
            {
                using(var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(AppPath);
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                    var response = await client.GetAsync($"api/picture/genres");
                    if (response.IsSuccessStatusCode)
                    {
                        var json = await response.Content.ReadAsStringAsync();

                        var per = await JsonConvert.DeserializeObjectAsync<List<Genre>>(json);

                        //per.Insert(0, new Genre { Id = 0, Name = "Все" });

                        foreach(var s in per)
                        {
                            Genres.Items.Add(new ComboBoxItem { Content = s.Name });
                        }
                    }
                }
            }
            catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        private async void Genres_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if(IsLoaded)
                await SearchPicturesAsync();
        }
    }
}
