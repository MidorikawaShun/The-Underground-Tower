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
using System.Windows.Media.Effects;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using WpfApp1;
using WpfApp1.GameProperties;
using WpfApp1.Pages;

namespace TheUndergroundTower.Pages
{
    /// <summary>
    /// Interaction logic for pageHighScores.xaml
    /// </summary>
    public partial class pageHighScores : Page
    {
        public pageHighScores()
        {
            InitializeComponent();
            if (GameStatus.GameEnded)
                HighScoreCanvas.Children.Remove(ToMainMenu);
            else
                HighScoreCanvas.Children.Remove(Exit);
            List<HighScore> allHighScores = Utilities.Xml.ReadHighScores().Take(10).ToList();
            for (int i = 0; i < allHighScores.Count; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    TextBlock elem = new TextBlock();
                    if (j == 0) elem.Text = (i + 1).ToString(); //row number
                    if (j == 1) elem.Text = allHighScores[i].CharacterName; //character name
                    if (j == 2) elem.Text = allHighScores[i].Score.ToString(); //character score
                    if (j == 3) elem.Text = allHighScores[i].Date; //date achieved
                    elem.TextAlignment = TextAlignment.Center;
                    elem.Effect = new DropShadowEffect();
                    elem.FontSize = 20;
                    elem.Foreground = new SolidColorBrush(Colors.DarkGoldenrod);
                    if (GameStatus.FinalizedHighScore != null && allHighScores[i].Date == GameStatus.FinalizedHighScore.Date)
                        elem.Background = new SolidColorBrush(Color.FromArgb(125, 255, 0, 0));
                    Grid.SetRow(elem, i);
                    Grid.SetColumn(elem, j);
                    HighScoresGrid.Children.Add(elem);
                }
            }
        }

        private void Exit_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void ToMainMenu_Click(object sender, RoutedEventArgs e)
        {
            Definitions.MAIN_WINDOW.Main.Content = new pageMainMenu();
        }
    }
}
