using Client.UserControls;
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

namespace Client
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private const string AppPath = "https://localhost:44323/";
        private static string token;
        private PictureGrid PictureGr;

        public MainWindow()
        {
            
            InitializeComponent();
            AddPictureGrid(token);
        }

        private async void Button_Click_1(object sender, RoutedEventArgs e)
        {
            AddPictureGrid(token);
        }

        public void AddPictureGrid(string token)
        {
            PictureGr = new PictureGrid(token, MainGrid);
            //pic.Name = "PictureGrid";
            PictureGr.Margin = new Thickness(10d, 150, 10, 0);
            MainGrid.Children.Add(PictureGr);
        }
        
        private async void Button_Click(object sender, RoutedEventArgs e)
        {
        }
    }
}
