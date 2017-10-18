﻿using System;
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

namespace WpfApp1.Windows.MetaMenus
{
    /// <summary>
    /// Interaction logic for ErrorPrompt.xaml
    /// </summary>
    public partial class ErrorPrompt : Window
    {
        public ErrorPrompt()
        {
            InitializeComponent();
            string errorMessage = "An error has occured: Detailed information can be found" + Environment.NewLine +  "in the Log file named \"Error Log - " + DateTime.Now.Date.ToString("dd/MM/yyyy") +"\"";
            this.DataContext = new { message = errorMessage };
        }

        private void ErrorPromptExit_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void ErrorPromptWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            Application.Current.Shutdown();
        }
    }
}
