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
    /// Interaction logic for GenericWindow.xaml
    /// </summary>
    public partial class GenericWindow : Window
    {
        public string _resultText;
        public string ResultText
        {
            get { return _resultText; }
            set { _resultText = value; }
        }

        public GenericWindow(string windowTitle, string[] buttons)
        {
            Window mainWindow = Application.Current.MainWindow;
            this.Left = mainWindow.Left + (mainWindow.Width - this.Width) / 2;
            this.Top = mainWindow.Top + (mainWindow.Height - this.Height) / 2;
            this.Owner = mainWindow;
            InitializeComponent();
            UIElementCollection area = this.buttonArea.Children;
            foreach (string buttonText in buttons)
            {
                Button button = new Button() { Content = buttonText, Name = buttonText.Replace(" ","") }; //Name does not accept spaces
                button.Click += GetButtonNumber;
                area.Insert(area.Count - 1, button);
                if (buttonText != buttons[buttons.Length - 1])
                    area.Insert(area.Count - 1, new Label());
            }
        }
        private void GetButtonNumber(object sender, RoutedEventArgs e)
        {
            string name = (sender as FrameworkElement).Name;
            DependencyObject currentElement = sender as FrameworkElement;
            while (currentElement.GetType().Name != "GenericWindow")
                currentElement = (currentElement as FrameworkElement).Parent;
            _resultText = name;
            DialogResult = true;
            (currentElement as Window).Close();
        }

        public static string Create(string windowTitle, string[] buttons)
        {
            GenericWindow prompt = new GenericWindow(windowTitle, buttons);
            prompt.ShowDialog();
            if (prompt.DialogResult == true)
                return prompt._resultText;
            return null;
        }
    }
}
