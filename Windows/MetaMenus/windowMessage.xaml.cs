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

namespace TheUndergroundTower.Windows.MetaMenus
{
    /// <summary>
    /// Interaction logic for MessagePrompt.xaml
    /// </summary>
    public partial class windowMessage : Window
    {
        public windowMessage(string message)
        {
            InitializeComponent();
            //Sets the error message
            this.DataContext = new { message = message };
            SizeToContent = SizeToContent.WidthAndHeight;
            Window mainWindow = Application.Current.MainWindow;
            //Centers this window on the parent window
            this.Owner = mainWindow;
            ShowDialog();
        }
        
        private void MessagePrompt_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
