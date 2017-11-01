using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp1
{
    public static class Definitions
    {

        public static MainWindow MAIN_WINDOW { get; set; }

        public const int NUMBER_OF_CHARACTER_STATS = 6;
        public enum EnumCharacterStats
        {
            Strength=0,
            Dexterity=1,
            Constitution=2,
            Intelligence=3,
            Wisdom=4,
            Charisma=5
        };

        public static string[] XML_FILES = { @"../../Assets/XML Files/Races.xml", @"../../Assets/XML Files/Careers.xml" ,
            @"../../Assets/XML Files/Difficulties.xml",@"../../Assets/XML Files/TowerDepths.xml",
            @"../../Assets/XML Files/Tiles.xml" };
        public enum EnumXmlFiles
        {
            XmlFileRaces=0,
            XmlFileCareers=1,
            XmlFileDifficulties=2,
            XmlFileTowerDepths=3,
            XmlFileTiles=4
        };

        public static string[] SOUND_FILES = { @"../../Assets/Sounds/Nonstop.mp3", @"../../Assets/Sounds/Sword Slash.mp3" };
        public enum EnumSoundFiles
        {
            MainMenuMusic=0,
            SwordSlash=1
        };

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

        //Hat, Amulet, Shirt, Gloves, Hands x2, Ring x2, Pants, Boots
        public const int NUM_OF_EQUIPMENT_SLOTS = 10;
        public enum EnumBodyParts
        {
            Hat, Amulet, Shirt, Gloves, LeftHand,
            RightHand, LeftRing, RightRing, Pants, Boots
        };

    }
}
