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
            ResizeMode = ResizeMode.NoResize;

            Loaded += (o, e) => ToFullScreen("Loaded");
            LocationChanged += (o, e) => ToFullScreen("LocationChanged");
            SizeChanged += (o, e) => ToFullScreen("SizeChanged");
        }

        bool _isRelocating;

        void ToFullScreen(string message)
        {
            if (!IsLoaded) return;
            if (_isRelocating) return;
            _isRelocating = true;
            window.FullScreenForCurrent();
            _isRelocating = false;

            System.Diagnostics.Debug.WriteLine(message);
        }
    }
}
