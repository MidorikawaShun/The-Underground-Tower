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
        public enum CHARACTER_STATS
        {
            Strength=0,
            Dexterity=1,
            Constitution=2,
            Intelligence=3,
            Wisdom=4,
            Charisma=5
        };

        public enum EnumXmlFiles
        {
            XmlFileRaces=0,
            XmlFileClasses=1
        };
        public static string[] XML_FILES = { "../../Assets/XML Files/Races.xml" , "../../Assets/XML Files/Classes.xml" };
    }
}
