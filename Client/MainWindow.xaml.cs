using Client.UserControls;
using Client.Views;
using Client.Windows;
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
        public static string token;
        private PictureGrid PictureGr;
        private AutorGrid AutorGr;
        public static Griding gr = Griding.PictureGrid;
        public static int? Id = null;
        public static string Role;
        public static Grid logGrid;
        public static string Mail;

        public enum Griding { PictureGrid, AutorGrid, Other};

        public MainWindow()
        {
            InitializeComponent();
            AddLogInGrid();
            AddPictureGrid();
            logGrid = LogInGrid;
        }

        private void AddLogInGrid()
        {
            LogControl log = new LogControl(AppPath, LogInGrid, CommonGrid);
            log.Margin = new Thickness(0, 0, 5, 0);
            LogInGrid.Children.Add(log);
        }

        private void AddPictureGrid()
        {
            PictureGr = new PictureGrid(AppPath, CommonGrid, false);
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
            if (gr != Griding.PictureGrid)
            {
                CommonGrid.Children.Clear();
                AddPictureGrid();
                gr = Griding.PictureGrid;
                LogProf.grid = LogProf.Grids.Nothing;
            }
        }

        private void Autors_Click(object sender, RoutedEventArgs e)
        {
            if(gr != Griding.AutorGrid)
            {
                CommonGrid.Children.Clear();
                AddAutorGrid();
                gr = Griding.AutorGrid;
                LogProf.grid = LogProf.Grids.Nothing;
            }
        }

    }
}
