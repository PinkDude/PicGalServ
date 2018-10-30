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
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            mainGrid.Children.Remove(this);
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            FirstName.IsReadOnly = false;
            LastName.IsReadOnly = false;
            MiddleName.IsReadOnly = false;
            Description.IsReadOnly = false;
            Date.IsReadOnly = false;
            Phone.IsReadOnly = false;

            Ok.Visibility = Visibility.Visible;
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {

        }

        private async void Ok_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                using(var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(AppPath);
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                    PersonInfo person = new PersonInfo
                    {
                        Autor = Autor.IsChecked.HasValue,
                        FirstName = FirstName.Text,
                        LastName = LastName.Text,
                        MiddleName = MiddleName.Text,
                        Description = Description.Text,
                        Birthday = Convert.ToDateTime(Date.Text),
                        Phone = Phone.Text,
                        Photo = AutorImg.Source.ToString().Replace(AppPath, "")
                    };

                    var response = await client.PutAsJsonAsync($"api/picture/person-info/{Id}", person);

                    if (response.IsSuccessStatusCode)
                    {
                        var json = await response.Content.ReadAsStringAsync();

                        var per = await JsonConvert.DeserializeObjectAsync<PersonInfo>(json);

                        Uri uri = new Uri(AppPath + per.Photo);
                        BitmapImage bm = new BitmapImage(uri);
                        AutorImg.Source = bm;

                        if (per.Autor)
                            Autor.IsChecked = true;
                        else
                            Autor.IsChecked = false;

                        Phone.Text = per.Phone;
                        Description.Text = per.Description;
                        FirstName.Text = per.FirstName;
                        LastName.Text = per.LastName;
                        MiddleName.Text = per.MiddleName;
                        Date.Text = per.Birthday?.ToShortDateString();

                        Ok.Visibility = Visibility.Hidden;
                    }
                    else
                    {
                        MessageBox.Show("Что то пошло не так?!");
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
