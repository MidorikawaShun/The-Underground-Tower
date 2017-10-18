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
using System.Windows.Navigation;
using System.Windows.Shapes;
using WpfApp1.OtherClasses;

namespace WpfApp1.Pages
{
    /// <summary>
    /// Interaction logic for CharacterCreationPage.xaml
    /// </summary>
    public partial class CharacterCreationPage : Page
    {

        /// <summary>
        /// Initialize the Character Creation page.
        /// </summary>
        public CharacterCreationPage()
        {
            GameData.RACES = new List<Race>();
            Utilities.Xml.PopulateRaces();
            InitializeComponent();
            CreateRaceChoiceStack();
        }

        /// <summary>
        /// Creates the visible race descriptions, information and "choose this race" button.
        /// </summary>
        private void CreateRaceChoiceStack()
        {
            foreach (Race race in GameData.RACES)
            {
                Border raceBorder = CreateRaceBorderAndStack(race);
                ScrollViewer descScroller = CreateRaceTitleAndDescription(race);
                WrapPanel wrapPanel = CreateRaceStatDisplay(race);
                Button raceSelectButton = new Button();
                raceSelectButton.Height = 40;
                raceSelectButton.Margin = new Thickness{ Left = 10, Top = 5, Right = 10, Bottom = 10 };
                raceSelectButton.Content = "Choose this race";
                raceSelectButton.Click += btnSelectRace_Click;
                StackPanel stack = raceBorder.Child as StackPanel;
                stack.Children.Add(descScroller);
                stack.Children.Add(wrapPanel);
                stack.Children.Add(raceSelectButton);
                RaceWrapPanel.Children.Add(raceBorder);
            }
        }

        /// <summary>
        /// Hides the name-input canvas and shows the race-choice canvas.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnCharacterCreationNameChoice_Click(object sender, RoutedEventArgs e)
        {
            //CanvasNameChoice = Utilities.GetObjectByName<Canvas>(this, "NameChoice");
            CanvasNameChoice.Visibility = Visibility.Hidden;
            CanvasRaceChoice.Visibility = Visibility.Visible;
        }

        /// <summary>
        /// Defines the border and stackpanel for each race.
        /// </summary>
        /// <param name="race">The race for which you want the stack to display info on.</param>
        /// <returns>The Border object containing the stackpanel.</returns>
        private Border CreateRaceBorderAndStack(Race race)
        {
            Border raceBorder = new Border();
            raceBorder.BorderThickness = new Thickness { Left = 1, Top = 1, Right = 1, Bottom = 1 };
            raceBorder.BorderBrush = Brushes.Black;
            raceBorder.Margin = new Thickness { Left = 3 };
            StackPanel stack = new StackPanel();
            raceBorder.Child = stack;
            stack.Height = 300;
            stack.Width = 192;
            return raceBorder;
        }

        /// <summary>
        /// Creates the title and description of the parameter race.
        /// </summary>
        /// <param name="race">The race you want to display information on.</param>
        /// <returns>The ScrollViewer with the stackpanel containing title and description.</returns>
        private ScrollViewer CreateRaceTitleAndDescription(Race race)
        {
            ScrollViewer descScroller = new ScrollViewer();
            descScroller.VerticalScrollBarVisibility = ScrollBarVisibility.Auto;
            StackPanel descPanel = new StackPanel();
            descScroller.Content = descPanel;
            {
                TextBlock raceName = new TextBlock();
                descPanel.Children.Add(raceName);
                raceName.Text = race.RaceName;
                raceName.FontSize = 16;
                raceName.HorizontalAlignment = HorizontalAlignment.Center;
                raceName.FontWeight = FontWeights.Bold;
                TextBlock description = new TextBlock();
                descPanel.Children.Add(description);
                description.Text = race.RaceDescription;
                description.HorizontalAlignment = HorizontalAlignment.Center;
                description.TextWrapping = TextWrapping.Wrap;
                description.MaxHeight = 150;
                description.Height = 150;
            }
            return descScroller;
        }

        /// <summary>
        /// Creates the WrapPanel with the stat bonus displays.
        /// </summary>
        /// <param name="race">The race you want to display information on.</param>
        /// <returns>The WrapPanel.</returns>
        private WrapPanel CreateRaceStatDisplay(Race race)
        {
            WrapPanel wrapPanel = new WrapPanel();
            wrapPanel.Height = 78;
            wrapPanel.Width = 170;
            {
                for (int i = 0; i < Definitions.NUMBER_OF_CHARACTER_STATS; i++)
                {
                    TextBlock stat = new TextBlock();
                    stat.Width = 84;
                    stat.Height = 26;
                    Definitions.CHARACTER_STATS statName = (Definitions.CHARACTER_STATS)i;
                    stat.Text = $"{statName.ToString()}: {race[i]}";
                    wrapPanel.Children.Add(stat);
                }
            }
            return wrapPanel;
        }

        private void btnSelectRace_Click(object sender, RoutedEventArgs e)
        {
            //CanvasNameChoice = Utilities.GetObjectByName<Canvas>(this, "NameChoice");
            CanvasRaceChoice.Visibility = Visibility.Hidden;
            CanvasClassChoice.Visibility = Visibility.Visible;
        }

    }

}
