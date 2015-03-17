using KLibrary.Labs.UI;
using System;
using System.Collections.Generic;
using System.Linq;
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

namespace TestWpf
{
    /// <summary>
    /// MainWindow.xaml の相互作用ロジック
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            this.SetBorderless();
            //this.FullScreenForAll();
            ResizeMode = ResizeMode.NoResize;

            Loaded += MainWindow_Loaded;
            LocationChanged += MainWindow_LocationChanged;
            SizeChanged += MainWindow_SizeChanged;
        }

        void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            window.FullScreenForCurrent();

            System.Diagnostics.Debug.WriteLine("Loaded");
        }

        void MainWindow_LocationChanged(object sender, EventArgs e)
        {
            if (!IsLoaded) return;
            window.FullScreenForCurrent();

            System.Diagnostics.Debug.WriteLine("LocationChanged");
        }

        void MainWindow_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            if (!IsLoaded) return;
            window.FullScreenForCurrent();

            System.Diagnostics.Debug.WriteLine("SizeChanged");
        }
    }
}
