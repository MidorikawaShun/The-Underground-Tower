using System;
using System.Windows;
using static WpfApp1.Utilities.ErrorLog;

namespace WpfApp1.Windows.MetaMenus
{
    /// <summary>
    /// Interaction logic for ErrorPrompt.xaml
    /// </summary>
    public partial class ErrorPrompt : Window
    {

        private EnumErrorSeverity Severity { get; set; }

        /// <summary>
        /// Constructor.
        /// Shows the error message and determines if the game will shut down based on severity.
        /// </summary>
        /// <param name="severity"></param>
        public ErrorPrompt(EnumErrorSeverity severity)
        {
            InitializeComponent();
            Severity = severity;
            //The error message displayed
            string errorMessage = $"A" + (severity == EnumErrorSeverity.Fatal ? " fatal" : "n") +
                " error has occured: Detailed information can be found" + Environment.NewLine + "in the Log file named \"Error Log - " + DateTime.Now.Date.ToString("dd/MM/yyyy") + "\"";
            this.DataContext = new { message = errorMessage };
            //Make the window fit itself to the content (the message and the buttons)
            SizeToContent = SizeToContent.WidthAndHeight;
            Window mainWindow = Application.Current.MainWindow;
            this.Left = mainWindow.Left + (mainWindow.Width - this.Width) / 2;
            this.Top = mainWindow.Top + (mainWindow.Height - this.Height) / 2;
            //Show the window
            ShowDialog();
        }

        /// <summary>
        /// Event for clicking on the cancel button.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ErrorPromptExit_Click(object sender, RoutedEventArgs e)
        {
            if (Severity == EnumErrorSeverity.Fatal)
                Application.Current.Shutdown();
            else Close();
        }

        /// <summary>
        /// Event for closing the window in any other way.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ErrorPromptWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (Severity == EnumErrorSeverity.Fatal)
                Application.Current.Shutdown();
            else Close();
        }
    }
}
