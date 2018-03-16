using System.Windows;
using System.Windows.Controls;
using TheUndergroundTower.Pages;
using TheUndergroundTower.Windows;
using TheUndergroundTower.Windows.MetaMenus;
using WpfApp1.GameProperties;
using WpfApp1.Windows;
using static TheUndergroundTower.Options.Sound;
using static WpfApp1.GameProperties.Definitions;

namespace WpfApp1.Pages
{
    /// <summary>
    /// Interaction logic for MainMenu.xaml
    /// </summary>
    public partial class pageMainMenu : Page
    {
        /// <summary>
        /// Default constructor for a page.
        /// </summary>
        public pageMainMenu()
        {
            InitializeComponent();
            PlaySound(EnumSoundFiles.MainMenuMusic, EnumMediaPlayers.MusicPlayer);
        }

        /// <summary>
        /// When the user clicks the exit game button, show a prompt.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Exit_Game_Click(object sender, RoutedEventArgs e)
        {
            new windowExit();
        }

        /// <summary>
        /// Start character creation if user clicks on new game button.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void New_Game_Click(object sender, RoutedEventArgs e)
        {
            Definitions.MAIN_WINDOW.Main.Content = new pageCharacterCreation();
        }

        /// <summary>
        /// Show the leaderboard if user clicks leaderboard button.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Leaderboard_Click(object sender, RoutedEventArgs e)
        {
            Definitions.MAIN_WINDOW.Main.Content = new pageHighScores();
        }

        /// <summary>
        /// Show the options window if user clicks options button.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Options_Click(object sender, RoutedEventArgs e)
        {
            new windowOptions();
        }

        private void Instructions_Click(object sender, RoutedEventArgs e)
        {
            new Instructions();
        }
    }
}
