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
using static WpfApp1.Utilities.ErrorLog;

namespace WpfApp1.Windows.MetaMenus
{
    /// <summary>
    /// Interaction logic for ErrorPrompt.xaml
    /// </summary>
    public partial class ErrorPrompt : Window
    {

        private EnumErrorSeverity Severity { get; set; }

        public ErrorPrompt(EnumErrorSeverity severity)
        {
            InitializeComponent();
            Severity = severity;
            string errorMessage = $"A" + (severity == EnumErrorSeverity.Fatal ? " fatal" : "n") +
                " error has occured: Detailed information can be found" + Environment.NewLine + "in the Log file named \"Error Log - " + DateTime.Now.Date.ToString("dd/MM/yyyy") + "\"";
            this.DataContext = new { message = errorMessage };
            SizeToContent = SizeToContent.WidthAndHeight;
            Window mainWindow = Application.Current.MainWindow;
            this.Left = mainWindow.Left + (mainWindow.Width - this.Width) / 2;
            this.Top = mainWindow.Top + (mainWindow.Height - this.Height) / 2;
            ShowDialog();
        }

        private void ErrorPromptExit_Click(object sender, RoutedEventArgs e)
        {
            if (Severity == EnumErrorSeverity.Fatal)
                Application.Current.Shutdown();
            else Close();
        }

        private void ErrorPromptWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (Severity == EnumErrorSeverity.Fatal)
                Application.Current.Shutdown();
        }
    }
}
