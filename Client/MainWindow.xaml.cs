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
        private AutorGrid AutorGr;
        private Grid gr = Grid.PictureGrid;

        public enum Grid { PictureGrid, AutorGrid};

        public MainWindow()
        {
            
            InitializeComponent();
            AddPictureGrid();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            AddPictureGrid();
        }

        private void AddPictureGrid()
        {
            PictureGr = new PictureGrid(token, CommonGrid);
            PictureGr.Margin = new Thickness(10d, 0, 10, 0);
            CommonGrid.Children.Add(PictureGr);
        }

        private void AddAutorGrid()
        {
            AutorGr = new AutorGrid(AppPath, token, CommonGrid);
            AutorGr.Margin = new Thickness(10d, 0, 10d, 0);
            CommonGrid.Children.Add(AutorGr);
        }
        
        private async void Button_Click(object sender, RoutedEventArgs e)
        {
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            if (gr != Grid.PictureGrid)
            {
                CommonGrid.Children.Clear();
                AddPictureGrid();
                gr = Grid.PictureGrid;
            }
        }

        private void Autors_Click(object sender, RoutedEventArgs e)
        {
            if(gr != Grid.AutorGrid)
            {
                CommonGrid.Children.Clear();
                AddAutorGrid();
                gr = Grid.AutorGrid;
            }
        }
    }
}
