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
using System.Windows.Shapes;

namespace WpfApp1.Windows
{
    /// <summary>
    /// Interaction logic for ExitDialog.xaml
    /// </summary>
    public partial class ExitDialog : Window
    {
        public ExitDialog()
        {
            InitializeComponent();
            SizeToContent = SizeToContent.WidthAndHeight;
            Window mainWindow = Application.Current.MainWindow;
            Owner = mainWindow;
            ShowDialog();
        }
        

        /// <summary>
        /// Quit the game
        /// </summary>
        private void Exit_Game1_Click(object sender,RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void Return_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
