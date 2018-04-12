using System;
using System.Collections.Generic;
using System.ComponentModel;
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
using WpfApp1;

namespace TheUndergroundTower.Windows.MetaMenus
{
    /// <summary>
    /// Interaction logic for Instructions.xaml
    /// </summary>
    public partial class Instructions : Window
    {
        public Instructions()
        {
            Closing += OnWindowClosing;
            GameStatus.GamePaused = true;
            InitializeComponent();
            GoUpAFloor.Text = "- < - go up a floor"; //required because xaml can't parse "<"
            ShowDialog();
        }

        //Unpause the game when the instruction screen closes.
        public void OnWindowClosing(object sender, CancelEventArgs c)
        {
            GameStatus.GamePaused = false;
        }
    }
}
