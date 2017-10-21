﻿using System;
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

        public static string[] XML_FILES = { "../../Assets/XML Files/Races.xml", "../../Assets/XML Files/Classes.xml" };
        public enum EnumXmlFiles
        {
            XmlFileRaces=0,
            XmlFileClasses=1
        };

        public static string[] SFX_FILES = { };
        public enum EnumSfxFiles
        {

        }

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

    }
}
