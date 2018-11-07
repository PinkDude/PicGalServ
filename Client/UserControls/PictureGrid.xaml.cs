using Client.Services;
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
        private readonly string AppPath;
        private readonly Grid MainGrid;
        private const double cLeft = 5d, cTop = 5d;
        private int Skip = 0;
        private int CountPage;
        private int PageNow = 1;
        private const int Take = 10;
        private readonly int? autorId;
        private readonly bool admin;

        public static List<Pic> listPic = new List<Pic>();

        public PictureGrid(string app, Grid main, bool adm, int? autId = null)
        {
            AppPath = app;
            MainGrid = main;
            autorId = autId;
            admin = adm;
            InitializeComponent();
        }

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            Skip = 0;
            PageNow = 1;
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
                using (var client = HttpHelper.CreateClient(MainWindow.token))
                {
                    client.BaseAddress = new Uri(AppPath);
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                    HttpResponseMessage response;
                    if (admin)
                    {
                        response = await client.GetAsync($"api/picture/pictures/conf?Name={Search.Text}&genreId={Genres.SelectedIndex.ToString()}&Take={Take}&Skip={Skip}");
                    }
                    else
                    {
                        response = await client.GetAsync($"api/picture/pictures?Name={Search.Text}&genreId={Genres.SelectedIndex.ToString()}&Take={Take}&Skip={Skip}&autorId={autorId}");
                    }

                    if (response.IsSuccessStatusCode)
                    {
                        PictureView.Children.Clear();
                        listPic.Clear();

                        var json = await response.Content.ReadAsStringAsync();

                        var per = await JsonConvert.DeserializeObjectAsync<PicAndPage>(json);

                        CountPage = (int)Math.Ceiling((double)per.Count/(double)Take);

                        Pages.Content = $"{PageNow}/{Math.Ceiling((double)(per.Count) / (double)Take)}";

                        double left = cLeft, top = cTop;

                        Pic picTest = new Pic(0, null, "");

                        int Count = (int)(ActualWidth/picTest.Width);
                        int i = 0;
                        foreach (var s in per.Pictures)
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

                            if (!s.Status)
                                pic.Status.Visibility = Visibility.Visible;

                            pic.Autor.Content = s.Autor.ShotName;
                            pic.Genre.Content = s.Genre;
                            pic.Date.Content = s.Date?.ToShortDateString();

                            pic.Margin = new Thickness(left, top, 0, 0);

                            if (i > Count - 2)
                            {
                                left = cLeft;
                                top += pic.Height + cTop;
                                i = 0;
                            }
                            else
                            {
                                left += pic.Width + cLeft;
                                i++;
                            }

                            listPic.Add(pic);
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
            if (IsLoaded)
            {
                Skip = 0;
                PageNow = 1;
                await SearchPicturesAsync();
            }
        }

        private async void Prev_Click(object sender, RoutedEventArgs e)
        {
            if (Skip >= Take)
            {
                Skip -= Take;
                PageNow--;
                await SearchPicturesAsync();
            }
        }

        private async void Next_Click(object sender, RoutedEventArgs e)
        {
            if(Skip < (CountPage - 1) * Take)
            {
                Skip += Take;
                PageNow++;
                await SearchPicturesAsync();
            }
        }

        private async void PictureGrid1_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            if (listPic.Count != 0 && IsLoaded)
            {
                PictureView.Children.Clear();
                double left = cLeft, top = cTop;
                int Count = (int)(ActualWidth / listPic[0].Width);
                int i = 0;
                foreach (var s in listPic)
                {
                    s.Margin = new Thickness(left, top, 0, 0);
                    if (i > Count - 2)
                    {
                        left = cLeft;
                        top += s.Height + cTop;
                        i = 0;
                    }
                    else
                    {
                        left += s.Width + cLeft;
                        i++;
                    }
                    PictureView.Children.Add(s);
                }
            }
        }
    }
}
