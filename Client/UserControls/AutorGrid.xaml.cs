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
    /// Логика взаимодействия для AutorGrid.xaml
    /// </summary>
    public partial class AutorGrid : UserControl
    {
        private readonly string AppPath;
        private static string token;
        private readonly Grid MainGrid;
        private const double cLeft = 5d, cTop = 5d;
        private int Skip = 0;
        private int CountPage;
        private int PageNow = 1;
        private const int Take = 10;

        static List<Autor> listAu = new List<Autor>();

        public AutorGrid(string app, string tok, Grid gr)
        {
            AppPath = app;
            token = tok;
            MainGrid = gr;
            InitializeComponent();
        }

        private async void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            await SearchAutorsAsync();
        }

        private async void Prev_Click(object sender, RoutedEventArgs e)
        {
            if (Skip >= Take)
            {
                Skip -= Take;
                PageNow--;
                await SearchAutorsAsync();
            }
        }

        private async void Next_Click(object sender, RoutedEventArgs e)
        {
            if (Skip < (CountPage - 1) * Take)
            {
                Skip += Take;
                PageNow++;
                await SearchAutorsAsync();
            }
        }

        private void UserControl_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            if (listAu.Count != 0 && IsLoaded)
            {
                AutorView.Children.Clear();
                double left = cLeft, top = cTop;
                int Count = (int)(ActualWidth / listAu[0].Width);
                int i = 0;
                foreach (var s in listAu)
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
                    AutorView.Children.Add(s);
                }
            }
        }

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            Skip = 0;
            PageNow = 1;
            await SearchAutorsAsync();
        }

        private async Task SearchAutorsAsync()
        {
            try
            {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(AppPath);
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                    var response = await client.GetAsync($"api/picture/person-info?Name={Search.Text}&Take={Take}&Skip={Skip}");
                    if (response.IsSuccessStatusCode)
                    {
                        AutorView.Children.Clear();

                        var json = await response.Content.ReadAsStringAsync();

                        var per = await JsonConvert.DeserializeObjectAsync<AuAndPage>(json);

                        CountPage = (int)Math.Ceiling((double)per.Count / (double)Take);

                        Pages.Content = $"{PageNow}/{Math.Ceiling((double)(per.Count) / (double)Take)}";

                        listAu.Clear();

                        Autor auTest = new Autor("", "", null, 0);

                        int Count = (int)(ActualWidth / auTest.Width);

                        double left = cLeft, top = cTop;

                        int i = 0;
                        foreach(var s in per.Persons)
                        {
                            UserControls.Autor au = new Autor(AppPath, token, MainGrid, s.Id);

                            Uri uri = new Uri(AppPath + s.Photo);
                            BitmapImage bm = new BitmapImage(uri);
                            au.AutorImg.Source = bm;

                            au.Autor1.Text = s.Autor ? "Да" : "Нет";

                            au.Name.Text = s.FullName;

                            au.Date.Text = s.Birthday?.ToShortDateString();

                            au.Margin = new Thickness(left, top, 0, 0);

                            if (i > Count - 2)
                            {
                                left = cLeft;
                                top += au.Height + cTop;
                                i = 0;
                            }
                            else
                            {
                                left += au.Width + cLeft;
                                i++;
                            }

                            listAu.Add(au);
                            AutorView.Children.Add(au);
                        }
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
