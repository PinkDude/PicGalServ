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
    /// Логика взаимодействия для LogProf.xaml
    /// </summary>
    public partial class LogProf : UserControl
    {
        private readonly string AppPath;
        private readonly string Photo;
        private readonly Grid commonGrid;

        public static Grids grid = Grids.Nothing;

        public enum Grids { Prodile, Pictures, AddPicture, ConfChanges, Nothing };

        public LogProf(string app, string ph, Grid com)
        {
            AppPath = app;
            Photo = ph;
            commonGrid = com;

            InitializeComponent();

            Email.Text = MainWindow.Mail;

            Uri uri = new Uri(AppPath + Photo);
            BitmapImage bm = new BitmapImage(uri);
            Img.Source = bm;

            if (MainWindow.Role == "User")
            {
                ConfChanges.Visibility = Visibility.Hidden;
                MyPic.Visibility = Visibility.Hidden;
                AddPic.Visibility = Visibility.Hidden;
            }
            else
            {
                if (MainWindow.Role == "Autor")
                {
                    ConfChanges.Visibility = Visibility.Hidden;
                }
            }
        }

        private async void Профиль_Click(object sender, RoutedEventArgs e)
        {
            if (grid != Grids.Prodile)
            {
                await GetAutor();
                MainWindow.gr = MainWindow.Griding.Other;
                grid = Grids.Prodile;
            }
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

                    var response = await client.GetAsync($"api/picture/person-info/{MainWindow.Id}");

                    if (response.IsSuccessStatusCode)
                    {
                        var json = await response.Content.ReadAsStringAsync();

                        var per = await JsonConvert.DeserializeObjectAsync<PersonInfo>(json);

                        AutorById au = new AutorById(per.Id, commonGrid, AppPath);

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

                        commonGrid.Children.Add(au);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        private void AddPic_Click(object sender, RoutedEventArgs e)
        {
            if(grid != Grids.AddPicture)
            {
                Picture pic = new Picture(commonGrid, 0, (int)MainWindow.Id, 0, AppPath);
                pic.Margin = new Thickness(10, 0, 10, 0);
                commonGrid.Children.Add(pic);
                grid = Grids.AddPicture;
                MainWindow.gr = MainWindow.Griding.Other;
            }
        }

        private void MyPic_Click(object sender, RoutedEventArgs e)
        {
            if (grid != Grids.Pictures)
            {
                PictureGrid picGr = new PictureGrid(AppPath, commonGrid, false, MainWindow.Id);
                picGr.Margin = new Thickness(10, 0, 10, 0);
                commonGrid.Children.Clear();
                commonGrid.Children.Add(picGr);
                grid = Grids.Pictures;
                MainWindow.gr = MainWindow.Griding.Other;
            }
        }

        private void ConfChanges_Click(object sender, RoutedEventArgs e)
        {
            if(grid != Grids.ConfChanges)
            {
                PictureGrid picGr = new PictureGrid(AppPath, commonGrid, true, MainWindow.Id);
                picGr.Margin = new Thickness(10, 0, 10, 0);
                commonGrid.Children.Clear();
                commonGrid.Children.Add(picGr);
                grid = Grids.Pictures;
                grid = Grids.ConfChanges;
                MainWindow.gr = MainWindow.Griding.Other;
            }
        }
    }
}
