using Client.Services;
using Client.Views;
using Microsoft.Win32;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
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
        private readonly int genreId;
        private readonly Grid mainGrid;
        private readonly string AppPath;

        public Picture(Grid mainGrid, int i, int a, int g, string app, bool status = false)
        {
            AppPath = app;
            id = i;
            autorId = a;
            genreId = g;
            this.mainGrid = mainGrid;
            InitializeComponent();
            
            if (id == 0)
            {
                Redact.Visibility = Visibility.Hidden;
                Name.IsReadOnly = false;
                Genre.IsEnabled = true;
                Description.IsReadOnly = false;
                Date.IsEnabled = true;
                Ok.Visibility = Visibility.Visible;
            }
            else
            {
                if (MainWindow.Id == null)
                {
                    Redact.Visibility = Visibility.Hidden;
                }
                else
                {
                    if (MainWindow.Role == "Admin" && status)
                    {
                        Tr.Visibility = Visibility.Visible;
                        Fal.Visibility = Visibility.Visible;
                    }
                    else
                    if (autorId != MainWindow.Id)
                    {
                        Redact.Visibility = Visibility.Hidden;
                    }
                    
                }
            }
        }

        private async void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            await GetGenres();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if(id == 0)
            {
                LogProf.grid = LogProf.Grids.Nothing;
            }
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
                using (var client = new HttpClient())
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

                        au.Autor.SelectedIndex = per.Autor ? 0 : 1;

                        au.Phone.Text = per.Phone;
                        au.Description.Text = per.Description;
                        au.FirstName.Text = per.FirstName;
                        au.LastName.Text = per.LastName;
                        au.MiddleName.Text = per.MiddleName;
                        au.Date.Text = per.Birthday?.ToShortDateString();

                        au.Margin = new Thickness(30, 0, 30, 0);

                        mainGrid.Children.Add(au);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        private void Redact_Click(object sender, RoutedEventArgs e)
        {
            Name.IsReadOnly = false;
            Description.IsReadOnly = false;
            Date.IsEnabled = true;
            Genre.IsEnabled = true;
            Ok.Visibility = Visibility.Visible;
            LoadImg.Visibility = Visibility.Visible;
        }

        private async void Ok_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                using (var client = HttpHelper.CreateClient(MainWindow.token))
                {
                    client.BaseAddress = new Uri(AppPath);
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                    Views.PictureUpdate picture = new Views.PictureUpdate
                    {
                        Id = id,
                        AutorId = autorId,
                        Description = Description.Text,
                        GenreId = Genre.SelectedIndex + 1,
                        Name = Name.Text,
                        ParentId = id,
                        PicturePath = Picture1.Source?.ToString().Replace(AppPath, ""),
                        Status = false
                    };

                    if (Date.Text != string.Empty)
                        picture.Date = Convert.ToDateTime(Date.Text);

                    HttpResponseMessage response;

                    if (id == 0)
                    {
                        response = await client.PostAsJsonAsync($"api/picture/pictures", picture);
                    }
                    else
                    {
                        response = await client.PutAsJsonAsync($"api/picture/pictures/{MainWindow.Id}", picture);
                    }

                    if (response.IsSuccessStatusCode)
                    {
                        MessageBox.Show("Ваши изменения отправлены на подтверждение");

                        Ok.Visibility = Visibility.Hidden;
                        LoadImg.Visibility = Visibility.Hidden;
                        Name.IsReadOnly = true;
                        Description.IsReadOnly = true;
                        Genre.IsEnabled = false;
                        Date.IsEnabled = false;
                    }
                    else
                    {
                        MessageBox.Show("Что то пошло не так?!");
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
                using (var client = new HttpClient())
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

                        foreach (var s in per)
                        {
                            Genre.Items.Add(new ComboBoxItem { Content = s.Name });
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        private async void Button_Click_1(object sender, RoutedEventArgs e)
        {
            await Conf(true);
        }

        private async Task Conf(bool yes)
        {
            try
            {
                using (var client = HttpHelper.CreateClient(MainWindow.token))
                {
                    client.BaseAddress = new Uri(AppPath);
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                    var response = await client.GetAsync($"api/picture/pictures/{id}/conf?yes={yes}");
                    if (response.IsSuccessStatusCode)
                    {
                        MessageBox.Show("Данные дотверждены");
                        mainGrid.Children.Remove(this);
                    }
                    else
                    {
                        MessageBox.Show("Что-то пошло не так!");
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        private async void Fal_Click(object sender, RoutedEventArgs e)
        {
            await Conf(false);
        }

        private async void LoadImg_Click(object sender, RoutedEventArgs e)
        {
            await FileFial();
        }

        private async Task FileFial()
        {
            OpenFileDialog openFileDialog1 = new OpenFileDialog
            {
                InitialDirectory = "c:\\",
                Filter = "PNG Photos (*.jpg)|*.jpg",
                FilterIndex = 2,
                RestoreDirectory = true
            };

            if (openFileDialog1.ShowDialog() != null)
            {
                try
                {
                    var response = new WebClient().UploadFile(AppPath + $"api/picture/pictures/{id}/pic", "POST", openFileDialog1.FileName);
                    await GetPictureById(id);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: Could not read file from disk. Original error: " + ex.Message);
                }
            }
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
                        var json = await response.Content.ReadAsStringAsync();

                        var per = await JsonConvert.DeserializeObjectAsync<Views.Picture>(json);

                        Uri uri = new Uri(AppPath + per.PicturePath);
                        BitmapImage bm = new BitmapImage(uri);
                        Picture1.Source = bm;

                        uri = new Uri(AppPath + per.Autor.Photo);
                        bm = new BitmapImage(uri);
                        AutorImg.Source = bm;

                        Name.Text = per.Name;

                        if (!per.Status)
                             Status.Visibility = Visibility.Visible;

                        Autor.Text = per.Autor.FullName;

                        Genre.SelectedIndex = per.GenreId - 1;
                        Date.Text = per.Date?.ToShortDateString();
                        Description.Text = per.Description;
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
