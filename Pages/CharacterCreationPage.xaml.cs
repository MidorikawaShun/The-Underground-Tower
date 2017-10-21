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
using TheUndergroundTower.OtherClasses;
using TheUndergroundTower.Windows.MetaMenus;
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
            GameData.CLASSES = new List<Class>();
            Utilities.Xml.PopulateClasses();
            InitializeComponent();
            CreateRaceChoiceStack();
            CreateClassChoiceStack();
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
            foreach (Race race in GameData.RACES)
            {
                Border raceBorder = CreateBorderAndStack(race);
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
            foreach (Class CLASS in GameData.CLASSES)
            {
                Border classBorder = CreateBorderAndStack(CLASS);
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
        #region Events

        /// <summary>
        /// Hides the name-input canvas and shows the race-choice canvas.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnCharacterCreationNameChoice_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(txtCharacterCreationNameChoice.Text))
                new MessagePrompt("You must enter a name to proceed");
            else
            {
                CanvasNameChoice.Visibility = Visibility.Hidden;
                CanvasRaceChoice.Visibility = Visibility.Visible;
            }
        }

        private void btnSelectRace_Click(object sender, RoutedEventArgs e)
        {
            //CanvasNameChoice = Utilities.GetObjectByName<Canvas>(this, "NameChoice");
            CanvasRaceChoice.Visibility = Visibility.Hidden;
            CanvasClassChoice.Visibility = Visibility.Visible;
        }

        private void btnSelectClass_Click(object sender, RoutedEventArgs e)
        {
            //CanvasNameChoice = Utilities.GetObjectByName<Canvas>(this, "NameChoice");
            CanvasClassChoice.Visibility = Visibility.Hidden;
            //CanvasConfirmChoice.Visibility = Visibility.Visible;
        }

        #endregion

    }

}
