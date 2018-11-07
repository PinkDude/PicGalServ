using Client.Services;
using Client.Views;
using Microsoft.Win32;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
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
    /// Логика взаимодействия для AutorById.xaml
    /// </summary>
    public partial class AutorById : UserControl
    {
        private readonly int Id;
        private readonly Grid mainGrid;
        private readonly string AppPath;

        public AutorById(int id, Grid main, string app)
        {
            AppPath = app;
            Id = id;
            mainGrid = main;
            InitializeComponent();

            if (MainWindow.Id == null)
            {
                Change.Visibility = Visibility.Hidden;
            }
            else
            {
                if (Id != MainWindow.Id)
                {
                    Change.Visibility = Visibility.Hidden;
                }
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            mainGrid.Children.Remove(this);
            LogProf.grid = LogProf.Grids.Nothing;
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            FirstName.IsReadOnly = false;
            LastName.IsReadOnly = false;
            MiddleName.IsReadOnly = false;
            Description.IsReadOnly = false;
            Date.IsReadOnly = false;
            Phone.IsReadOnly = false;
            LoadImg.Visibility = Visibility.Visible;
            if (MainWindow.Role == "Admin")
            {
                Autor.IsReadOnly = false;
                Autor.IsEnabled = true;
            }

            Ok.Visibility = Visibility.Visible;
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
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

                    PersonInfo person = new PersonInfo
                    {
                        Id = this.Id,
                        Autor = this.Autor.SelectedIndex == 0 ? true : false,
                        FirstName = this.FirstName.Text,
                        LastName = this.LastName.Text,
                        MiddleName = MiddleName.Text,
                        Description = this.Description.Text,
                        Birthday = Convert.ToDateTime(this.Date.Text),
                        Phone = this.Phone.Text,
                        Photo = this.AutorImg.Source?.ToString().Replace(AppPath, "")
                    };

                    var response = await client.PutAsJsonAsync($"api/picture/person-info/{MainWindow.Id}", person);

                    if (response.IsSuccessStatusCode)
                    {
                        var json = await response.Content.ReadAsStringAsync();

                        var per = await JsonConvert.DeserializeObjectAsync<PersonInfo>(json);

                        Uri uri = new Uri(AppPath + per.Photo);
                        BitmapImage bm = new BitmapImage(uri);
                        AutorImg.Source = bm;

                        Autor.SelectedIndex = per.Autor ? 0 : 1;

                        Phone.Text = per.Phone;
                        Description.Text = per.Description;
                        FirstName.Text = per.FirstName;
                        LastName.Text = per.LastName;
                        MiddleName.Text = per.MiddleName;
                        Date.Text = per.Birthday?.ToShortDateString();

                        Ok.Visibility = Visibility.Hidden;
                        FirstName.IsReadOnly = true;
                        LastName.IsReadOnly = true;
                        MiddleName.IsReadOnly = true;
                        Description.IsReadOnly = true;
                        Date.IsReadOnly = true;
                        Phone.IsReadOnly = true;
                        Autor.IsReadOnly = true;
                        Autor.IsEnabled = false;
                        LoadImg.Visibility = Visibility.Hidden;
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

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            FileFial();
        }

        private async void FileFial()
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
                    var response = new WebClient().UploadFile(AppPath + $"api/picture/person-info/{MainWindow.Id}/pic", "POST", openFileDialog1.FileName);
                    var s = await GetAutor();
                    MainWindow.logGrid.Children.Clear();
                    LogProf lp = new LogProf(AppPath, s, mainGrid);
                    MainWindow.logGrid.Children.Add(lp);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: Could not read file from disk. Original error: " + ex.Message);
                }
            }
        }

        private async Task<string> GetAutor()
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

                        Uri uri = new Uri(AppPath + per.Photo);
                        BitmapImage bm = new BitmapImage(uri);
                        AutorImg.Source = bm;

                        Autor.SelectedIndex = per.Autor ? 0 : 1;

                        Phone.Text = per.Phone;
                        Description.Text = per.Description;
                        FirstName.Text = per.FirstName;
                        LastName.Text = per.LastName;
                        MiddleName.Text = per.MiddleName;
                        Date.Text = per.Birthday?.ToShortDateString();

                        return per.Photo;
                    }
                    return null;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
