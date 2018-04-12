using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using TheUndergroundTower;
using TheUndergroundTower.OtherClasses;
using TheUndergroundTower.Pages;
using TheUndergroundTower.Windows.MetaMenus;
using WpfApp1.Creatures;
using WpfApp1.GameProperties;
using WpfApp1.OtherClasses;

namespace WpfApp1.Pages
{
    /// <summary>
    /// Interaction logic for CharacterCreationPage.xaml
    /// </summary>
    public partial class pageCharacterCreation : Page
    {

        private List<Border> RaceElements { get; set; }
        private List<Border> ClassElements { get; set; }
        private Race ChosenRace { get; set; }
        private Career ChosenClass { get; set; }
        private TowerDepth ChosenDepth { get; set; }
        private Difficulty ChosenDifficulty { get; set; }
        private int[] PlayerStats { get; set; }

        /// <summary>
        /// Initialize the Character Creation page.
        /// </summary>
        public pageCharacterCreation()
        {
            GameData.InitializeRaces();
            GameData.InitializeCareer();
            GameData.InitializeDifficulties();
            GameData.InitializeTowerDepths();
            InitializeComponent();
            CreateRaceChoiceStack();
            CreateCareerChoiceStack();
            InitCharacterFinalization();
        }

        #region General Initializations Functions
        /// <summary>
        /// Defines the border and stackpanel for each race.
        /// </summary>
        /// <param name="gameObject">The gameObject for which you want the stack to display info on.</param>
        /// <returns>The Border object containing the stackpanel.</returns>
        private Border CreateBorderAndStack(GameObject gameObject)
        {
            Border gameObjectBorder = new Border();
            gameObjectBorder.BorderThickness = new Thickness { Left = 1, Top = 1, Right = 1, Bottom = 1 };
            gameObjectBorder.BorderBrush = Brushes.Black;
            gameObjectBorder.Margin = new Thickness { Left = 3 };
            StackPanel stack = new StackPanel();
            gameObjectBorder.Child = stack;
            stack.Height = 300;
            stack.Width = 192;
            return gameObjectBorder;
        }

        /// <summary>
        /// Creates the title and description of the parameter race.
        /// </summary>
        /// <param name="gameObject">The gameObject you want to display information on.</param>
        /// <returns>The ScrollViewer with the stackpanel containing title and description.</returns>
        private StackPanel CreateTitleAndDescription(GameObject gameObject)
        {

            StackPanel descPanel = new StackPanel();
            TextBlock gameObjectName = new TextBlock();
            descPanel.Children.Add(gameObjectName);
            gameObjectName.Text = gameObject.Name;
            gameObjectName.FontSize = 16;
            gameObjectName.HorizontalAlignment = HorizontalAlignment.Center;
            gameObjectName.FontWeight = FontWeights.Bold;
            ScrollViewer descScroller = new ScrollViewer();
            descScroller.VerticalScrollBarVisibility = ScrollBarVisibility.Auto;
            descScroller.Height = 150;
            descPanel.Children.Add(descScroller);
            TextBlock description = new TextBlock();
            description.Text = gameObject.Description;
            description.HorizontalAlignment = HorizontalAlignment.Center;
            description.TextWrapping = TextWrapping.Wrap;
            descScroller.Content = description;
            return descPanel;
        }
        #endregion

        #region Race Initialization

        /// <summary>
        /// Creates the visible race descriptions, information and "choose this race" button.
        /// </summary>
        private void CreateRaceChoiceStack()
        {
            RaceElements = new List<Border>();
            foreach (Race race in GameData.POSSIBLE_RACES)
            {
                Border raceBorder = CreateBorderAndStack(race);
                raceBorder.Name = race.ID;
                RaceElements.Add(raceBorder);
                StackPanel descPanel = CreateTitleAndDescription(race);
                WrapPanel wrapPanel = CreateRaceStatDisplay(race);
                Button raceSelectButton = new Button();
                raceSelectButton.Height = 40;
                raceSelectButton.Margin = new Thickness { Left = 10, Top = 5, Right = 10, Bottom = 10 };
                raceSelectButton.Content = "Choose this race";
                raceSelectButton.Click += btnSelectRace_Click;
                StackPanel stack = raceBorder.Child as StackPanel;
                stack.Children.Add(descPanel);
                stack.Children.Add(wrapPanel);
                stack.Children.Add(raceSelectButton);
                RaceWrapPanel.Children.Add(raceBorder);
            }
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
                    Definitions.EnumCharacterStats statName = (Definitions.EnumCharacterStats)i;
                    stat.Text = $"{statName.ToString()}: {race[i]}";
                    wrapPanel.Children.Add(stat);
                }
            }
            return wrapPanel;
        }

        #endregion
        #region Class Initialization

        /// <summary>
        /// Creates the visible race descriptions, information and "choose this race" button.
        /// </summary>
        private void CreateCareerChoiceStack()
        {
            ClassElements = new List<Border>();
            foreach (Career career in GameData.POSSIBLE_CAREERS)
            {
                Border classBorder = CreateBorderAndStack(career);
                classBorder.Name = career.ID;
                ClassElements.Add(classBorder);
                StackPanel descPanel = CreateTitleAndDescription(career);
                WrapPanel wrapPanel = CreateCareerInformationDisplay(career);
                Button classSelectButton = new Button();
                classSelectButton.Height = 40;
                classSelectButton.Margin = new Thickness { Left = 10, Top = 5, Right = 10, Bottom = 10 };
                classSelectButton.Content = "Choose this career";
                classSelectButton.Click += btnSelectClass_Click;
                StackPanel stack = classBorder.Child as StackPanel;
                stack.Children.Add(descPanel);
                stack.Children.Add(wrapPanel);
                stack.Children.Add(classSelectButton);
                ClassWrapPanel.Children.Add(classBorder);
            }
        }

        /// <summary>
        /// Shows how good the career is at the skills.
        /// </summary>
        /// <param name="career">The career we want to display stuff</param>
        /// <returns>A Wrappanel with the required WPF elements</returns>
        private WrapPanel CreateCareerInformationDisplay(Career career)
        {
            WrapPanel wrapPanel = new WrapPanel();
            wrapPanel.Margin = new Thickness(0, 15, 0, -15);
            wrapPanel.Height = 78;
            wrapPanel.Width = 170;
            //3 is the number of skills that exist for each career.
            //Create a textblock displaying information about each skill.
            for (int i = 0; i < 3; i++)
            {
                TextBlock skill = new TextBlock();
                skill.Width = 170;
                skill.Height = 20;
                skill.TextWrapping = TextWrapping.Wrap;
                switch (i)
                {
                    case 0:
                        skill.Text = "Melee skill: " + Definitions.SkillRating(career.MeleeSkill);
                        break;
                    case 1:
                        skill.Text = "Ranged skill: " + Definitions.SkillRating(career.RangedSkill);
                        break;
                    case 2:
                        skill.Text = "Magic skill: " + Definitions.SkillRating(career.MagicSkill);
                        break;
                }
                wrapPanel.Children.Add(skill);
            }
            return wrapPanel;
        }

        #endregion
        #region Character Creation Finalization
        /// <summary>
        /// Creates the comboboxes at the final character creation screen.
        /// </summary>
        public void InitCharacterFinalization()
        {
            //Gets a List<string> of all the Difficulty names in POSSIBLE_DIFFICULTIES list.
            cmbDifficulties.ItemsSource = GameData.POSSIBLE_DIFFICULTIES.Select(x => x.Name);
            //If the below line doesn't exist, the combobox will have an empty choice.
            cmbDifficulties.SelectedItem = cmbDifficulties.Items[0];
            //Same as above.
            cmbTowerDepth.ItemsSource = GameData.POSSIBLE_TOWER_DEPTHS.Select(x => x.Name);
            cmbTowerDepth.SelectedItem = cmbTowerDepth.Items[0];
        }
        #endregion
        #region Events

        /// <summary>
        /// Hides the name-input canvas and shows the race-choice canvas.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnCharacterCreationNameChoice_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(txtCharacterCreationNameChoice.Text))
                new windowMessage("You must enter a name to proceed");
            else
            {
                lblName.Content = txtCharacterCreationNameChoice.Text;
                CanvasNameChoice.Visibility = Visibility.Hidden;
                CanvasRaceChoice.Visibility = Visibility.Visible;
            }
        }

        /// <summary>
        /// Hides the canvas for race-choice and displays career choice canvas.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSelectRace_Click(object sender, RoutedEventArgs e)
        {
            /*
                The ID of the race exists on the border as it's name.
                So iterate over every border and check whether or not
                it has the button that was clicked. If it does, then
                we know what race the user has selected.
            */
            foreach (Border border in RaceElements)
                if (border.HasChild(sender))
                {
                    ChosenRace = GameObject.GetByID(border.Name) as Race;
                    break;
                }
            lblRace.Content = ChosenRace.Name;
            CanvasRaceChoice.Visibility = Visibility.Hidden;
            CanvasClassChoice.Visibility = Visibility.Visible;
        }

        /// <summary>
        /// Hides the canvas for career-choice and displays the character creation finalization canvas.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSelectClass_Click(object sender, RoutedEventArgs e)
        {
            /*
                The ID of the career exists on the border as it's name.
                So iterate over every border and check whether or not
                it has the button that was clicked. If it does, then
                we know what career the user has selected.
            */
            foreach (Border border in ClassElements)
                if (border.HasChild(sender))
                {
                    ChosenClass = GameObject.GetByID(border.Name) as Career;
                    break;
                }
            lblClass.Content = ChosenClass.Name;
            //Initialize the stats the player will have
            PlayerStats = new int[6];
            RollAndDisplayStats();
            CanvasClassChoice.Visibility = Visibility.Hidden;
            CanvasCharacterFinalization.Visibility = Visibility.Visible;
        }

        /// <summary>
        /// An event for when the user chooses a different difficulty.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cmbDifficulties_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ChosenDifficulty = GameData.POSSIBLE_DIFFICULTIES.Where(x => x.Name.Equals(e.AddedItems[0])).FirstOrDefault();
            txtDifficulties.Text = ChosenDifficulty.Description;
        }

        /// <summary>
        /// An event for when the user chooses a different tower depth.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cmbTowerDepth_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ChosenDepth = GameData.POSSIBLE_TOWER_DEPTHS.Where(x => x.Name.Equals(e.AddedItems[0])).FirstOrDefault();
            txtTowerDepth.Text = ChosenDepth.Description;
        }

        /// <summary>
        /// An event to change the stats the player will start with.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnReroll_Click(object sender, RoutedEventArgs e)
        {
            RollAndDisplayStats();
        }

        /// <summary>
        /// Restart character creation.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnRestart_Click(object sender, RoutedEventArgs e)
        {
            CanvasCharacterFinalization.Visibility = Visibility.Hidden;
            CanvasNameChoice.Visibility = Visibility.Visible;
        }

        /// <summary>
        /// Finishing character creation
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnProceed_Click(object sender, RoutedEventArgs e)
        {
            Player player = GameStatus.Player = new Player();
            for (int i = 0; i < Definitions.NUMBER_OF_CHARACTER_STATS; i++)
                player[i] = PlayerStats[i];
            player.SetRace(ChosenRace);
            player.SetCareer(ChosenClass);
            player.Name = lblName.Content as string;
            player.Description = "This is you.";
            GameStatus.ChosenDifficulty = ChosenDifficulty;
            GameStatus.ChosenDepth = ChosenDepth;
            Definitions.MAIN_WINDOW.Main.Content = new pageMainGame();
        }

        #endregion

        /// <summary>
        /// Determines starting stats for player character, adjusting for race selection.
        /// </summary>
        private void RollAndDisplayStats()
        {
            int sum;
            for (int i = 0; i < Definitions.NUMBER_OF_CHARACTER_STATS; i++)
            {
                sum = 0;
                for (int j = 0; j < 3; j++)
                    sum += GameStatus.Random.Next(1, 7);
                PlayerStats[i] = sum + ChosenRace[i];
            }
            lblStrength.Content = PlayerStats[0];
            lblDexterity.Content = PlayerStats[1];
            lblConstitution.Content = PlayerStats[2];
            lblIntelligence.Content = PlayerStats[3];
            lblWisdom.Content = PlayerStats[4];
            lblCharisma.Content = PlayerStats[5];
        }

    }

}
