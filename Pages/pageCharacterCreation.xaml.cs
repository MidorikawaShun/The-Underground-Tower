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
using TheUndergroundTower.Windows.MetaMenus;
using WpfApp1.Creatures;
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
        private Class ChosenClass { get; set; }
        private TowerDepth ChosenDepth { get; set; }
        private Difficulty ChosenDifficulty { get; set; }
        private int[] PlayerStats { get; set; }

        /// <summary>
        /// Initialize the Character Creation page.
        /// </summary>
        public pageCharacterCreation()
        {
            GameData.InitializeRaces();
            GameData.InitializeClasses();
            GameData.InitializeDifficulties();
            GameData.InitializeTowerDepths();
            InitializeComponent();
            CreateRaceChoiceStack();
            CreateClassChoiceStack();
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
            foreach (Race race in GameData.RACES)
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
        private void CreateClassChoiceStack()
        {
            ClassElements = new List<Border>();
            foreach (Class CLASS in GameData.CLASSES)
            {
                Border classBorder = CreateBorderAndStack(CLASS);
                classBorder.Name = CLASS.ID;
                ClassElements.Add(classBorder);
                StackPanel descPanel = CreateTitleAndDescription(CLASS);
                WrapPanel wrapPanel = CreateClassInformationDisplay(CLASS);
                Button classSelectButton = new Button();
                classSelectButton.Height = 40;
                classSelectButton.Margin = new Thickness { Left = 10, Top = 5, Right = 10, Bottom = 10 };
                classSelectButton.Content = "Choose this class";
                classSelectButton.Click += btnSelectClass_Click;
                StackPanel stack = classBorder.Child as StackPanel;
                stack.Children.Add(descPanel);
                stack.Children.Add(wrapPanel);
                stack.Children.Add(classSelectButton);
                ClassWrapPanel.Children.Add(classBorder);
            }
        }

        private WrapPanel CreateClassInformationDisplay(Class CLASS)
        {
            WrapPanel wrapPanel = new WrapPanel();
            wrapPanel.Margin = new Thickness(0, 15, 0, -15);
            wrapPanel.Height = 78;
            wrapPanel.Width = 170;
            for (int i = 0; i < 3; i++)
            {
                TextBlock skill = new TextBlock();
                skill.Width = 170;
                skill.Height = 20;
                skill.TextWrapping = TextWrapping.Wrap;
                switch (i)
                {
                    case 0:
                        skill.Text = "Melee skill: " + Definitions.SkillRating(CLASS.MeleeSkill);
                        break;
                    case 1:
                        skill.Text = "Ranged skill: " + Definitions.SkillRating(CLASS.RangedSkill);
                        break;
                    case 2:
                        skill.Text = "Magic skill: " + Definitions.SkillRating(CLASS.MagicSkill);
                        break;
                }
                wrapPanel.Children.Add(skill);
            }
            return wrapPanel;
        }

        #endregion
        #region Character Creation Finalization
        public void InitCharacterFinalization()
        {

            cmbDifficulties.ItemsSource = GameData.DIFFICULTIES.Select(x => x.Name);
            cmbDifficulties.SelectedItem = cmbDifficulties.Items[0];
            cmbTowerDepth.ItemsSource = GameData.TOWER_DEPTHS.Select(x => x.Name);
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

        private void btnSelectRace_Click(object sender, RoutedEventArgs e)
        {
            foreach (Border border in RaceElements)
                if (border.HasChild(border, sender))
                {
                    ChosenRace = GameObject.GetByID(border.Name) as Race;
                    break;
                }
            lblRace.Content = ChosenRace.Name;
            CanvasRaceChoice.Visibility = Visibility.Hidden;
            CanvasClassChoice.Visibility = Visibility.Visible;
        }

        private void btnSelectClass_Click(object sender, RoutedEventArgs e)
        {
            //CanvasNameChoice = Utilities.GetObjectByName<Canvas>(this, "NameChoice");
            foreach (Border border in ClassElements)
                if (border.HasChild(border, sender))
                    ChosenClass = GameObject.GetByID(border.Name) as Class;
            lblClass.Content = ChosenClass.Name;
            PlayerStats = new int[6];
            RollAndDisplayStats();
            CanvasClassChoice.Visibility = Visibility.Hidden;
            CanvasCharacterFinalization.Visibility = Visibility.Visible;
        }

        private void cmbDifficulties_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ChosenDifficulty = GameData.DIFFICULTIES.Where(x => x.Name.Equals(e.AddedItems[0])).FirstOrDefault();
            txtDifficulties.Text = ChosenDifficulty.Description;
        }

        private void cmbTowerDepth_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ChosenDepth = GameData.TOWER_DEPTHS.Where(x => x.Name.Equals(e.AddedItems[0])).FirstOrDefault();
            txtTowerDepth.Text = ChosenDepth.Description;
        }

        private void btnReroll_Click(object sender, RoutedEventArgs e)
        {
            RollAndDisplayStats();
        }

        private void btnRestart_Click(object sender, RoutedEventArgs e)
        {
            CanvasCharacterFinalization.Visibility = Visibility.Hidden;
            CanvasNameChoice.Visibility = Visibility.Visible;
        }

        private void btnProceed_Click(object sender, RoutedEventArgs e)
        {
            Player player = GameStatus.PLAYER = new Creatures.Player();
            for (int i = 0; i < Definitions.NUMBER_OF_CHARACTER_STATS; i++)
                player[i] = PlayerStats[i];
            player.PlayerRace = ChosenRace;
            player.PlayerClass = ChosenClass;
            player.Name = lblName.Content as string;

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
                    sum += GameStatus.RANDOM.Next(1, 7);
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
