using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp1.GameProperties
{
    /// <summary>
    /// Static definitions and constants we will use for the game.
    /// </summary>
    public static class Definitions
    {
        /// <summary>
        /// The main WPF window that everything happens on.
        /// We will use smaller windows but only as prompts.
        /// </summary>
        public static MainWindow MAIN_WINDOW { get; set; }

        /// <summary>
        /// The number of stats that the player has.
        /// </summary>
        public const int NUMBER_OF_CHARACTER_STATS = 6;

        /// <summary>
        /// The number of tiles displayed along the screen's width
        /// </summary>
        public const int WINDOW_X_SIZE = 12;

        /// <summary>
        /// The number of tiels displayed along the screen's height
        /// </summary>
        public const int WINDOW_Y_SIZE = 12;

        /// <summary>
        /// An enum for easy access to the stats.
        /// </summary>
        public enum EnumCharacterStats
        {
            Strength=0,
            Dexterity=1,
            Constitution=2,
            Intelligence=3,
            Wisdom=4,
            Charisma=5
        };

        /// <summary>
        /// The XML files that the game uses.
        /// </summary>
        public static string[] XML_FILES = { @"../../Assets/XML Files/Races.xml", @"../../Assets/XML Files/Careers.xml" ,
            @"../../Assets/XML Files/Difficulties.xml",@"../../Assets/XML Files/TowerDepths.xml",
            @"../../Assets/XML Files/Tiles.xml",@"../../Assets/XML Files/Monsters.xml",@"../../Assets/XML Files/Items.xml",
            @"../../Assets/XML Files/XmlFileHighScores.xml"};

        /// <summary>
        /// Easy access to the XML files.
        /// </summary>
        public enum EnumXmlFiles
        {
            XmlFileRaces=0,
            XmlFileCareers=1,
            XmlFileDifficulties=2,
            XmlFileTowerDepths=3,
            XmlFileTiles=4,
            XmlFileMonsters=5,
            XmlFileItems=6,
            XmlFileHighScores=7
        };

        /// <summary>
        /// The sound files we use.
        /// </summary>
        public static string[] SOUND_FILES = { @"../../Assets/Sounds/Nonstop.mp3", @"../../Assets/Sounds/Sword Slash.mp3",@"../../Assets/Sounds/Bite.mp3",@"../../Assets/Sounds/Miss.mp3",
                                               @"../../Assets/Sounds/Dungeon Music.mp3"};
        /// <summary>
        /// Enum for easy access to the sound files.
        /// </summary>
        public enum EnumSoundFiles
        {
            MainMenuMusic=0,
            SwordSlash=1,
            Bite=2,
            Miss=3,
            MainGameMusic=4
        };

        /// <summary>
        /// Used instead of showing the player raw stats.
        /// </summary>
        /// <param name="rating">The player stats.</param>
        /// <returns>A nicely-shown string</returns>
        public static string SkillRating(double rating)
        {
            if (rating == 0)
                return "Incapable";
            if (rating < 0.5)
                return "Bad";
            if (rating < 1)
                return "Poor";
            if (rating == 1)
                return "Normal";
            if (rating < 1.5)
                return "Good";
            else
                return "Excellent";
        }

        /// <summary>
        /// Hat, Amulet, Shirt, Gloves, Hands x2, Ring x2, Pants, Boots.
        /// Hands are what the player is holding.
        /// </summary>
        public const int NUM_OF_EQUIPMENT_SLOTS = 10;
        
        /// <summary>
        /// Enum for easy access.
        /// </summary>
        public enum EnumBodyParts
        {
            Hat, Amulet, Shirt, Gloves, LeftHand,
            RightHand, LeftRing, RightRing, Pants, Boots
        };

    }
}
