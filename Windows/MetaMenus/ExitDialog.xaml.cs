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
    public partial class windowExit : Window
    {
        /// <summary>
        /// Constructor.
        /// </summary>
        public windowExit()
        {
            InitializeComponent();
            //Sets the window to fit the content.
            SizeToContent = SizeToContent.WidthAndHeight;
            Window mainWindow = Application.Current.MainWindow;
            //Owner = mainWindow means that this new window will appear within the mainWindow.
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

        /// <summary>
        /// Return to what you were doing
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Return_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
