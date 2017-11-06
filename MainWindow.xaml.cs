using System.Collections.Generic;
using System.Windows;
using WpfApp1.Creatures;
using TheUndergroundTower.Pages;
using WpfApp1.GameProperties;
using TheUndergroundTower.Windows.MetaMenus;
using WpfApp1.Pages;

namespace WpfApp1
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            GameStatus.CREATURES = new List<Creature>();
            Definitions.MAIN_WINDOW = this;
            Main.Content = new pageMainMenu();
            //PlaySound(EnumSoundFiles.MainMenuMusic,EnumMediaPlayers.MusicPlayer);
            //Main.Content = new pageMainGame();
        }


    }
}
