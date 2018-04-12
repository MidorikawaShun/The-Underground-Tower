using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Xml;
using TheUndergroundTower.Creatures;
using TheUndergroundTower.OtherClasses;
using TheUndergroundTower.Pathfinding;
using TheUndergroundTower.Windows.MetaMenus;
using WpfApp1.Windows.MetaMenus;
using static WpfApp1.GameProperties.Definitions;

namespace WpfApp1
{
    ///
    public static class Utilities
    {

        /// <summary>
        /// This class handles XML-related actions.
        /// </summary>
        public static class Xml
        {
            /// <summary>
            /// Opens an Xml file.
            /// </summary>
            /// <param name="XmlFilePath">The path of the file</param>
            /// <returns>The XmlDocument that we opened.</returns>
            public static XmlDocument ReadXml(string XmlFilePath)
            {
                XmlDocument doc = new XmlDocument();
                doc.Load(XmlFilePath);
                return doc;
            }

            /// <summary>
            /// Fill GameData.POSSIBLE_RACES list from XML file.
            /// </summary>
            public static void PopulateRaces()
            {
                try
                {
                    //Opens the Races XML file.
                    XmlDocument doc = ReadXml(Files.GetDefinitionFilePath(EnumXmlFiles.XmlFileRaces));
                    XmlNode races = doc.ChildNodes[1];
                    //Creates the race objects and adds them to the list.
                    foreach (XmlNode race in races)
                        new Race(race);
                }
                catch (Exception ex)
                {
                    ErrorLog.Log(ex, "An error has occured while attempting to populate races from XML.");
                }
            }

            /// <summary>
            /// Fill GameData.POSSIBLE_CAREERS list from XML file.
            /// </summary>
            public static void PopulateCareers()
            {
                try
                {
                    //Opens the Careers XML file.
                    XmlDocument doc = ReadXml(Files.GetDefinitionFilePath(EnumXmlFiles.XmlFileCareers));
                    XmlNode careers = doc.ChildNodes[1];
                    //Creates the careers objects and adds them to the list.
                    foreach (XmlNode newCareer in careers)
                        new Career(newCareer);
                }
                catch (Exception ex)
                {
                    ErrorLog.Log(ex, "An error has occured while attempting to populate classes from XML.");
                }
            }

            /// <summary>
            /// Fill GameData.POSSIBLE_DIFFICULTIES list from XML file.
            /// </summary>
            public static void PopulateDifficulties()
            {
                try
                {
                    //Opens the Difficulties XML file.
                    XmlDocument doc = ReadXml(Files.GetDefinitionFilePath(EnumXmlFiles.XmlFileDifficulties));
                    XmlNode difficulties = doc.ChildNodes[1];
                    //Creates the difficulties objects and adds them to the list.
                    foreach (XmlNode newDifficulty in difficulties)
                        new Difficulty(newDifficulty);
                }
                catch (Exception ex)
                {
                    ErrorLog.Log(ex, "An error has occured while attempting to populate difficulties from XML.");
                }
            }

            /// <summary>
            /// Fill GameData.POSSIBLE_TOWER_DEPTHS list from XML file.
            /// </summary>
            public static void PopulateTowerDepths()
            {
                try
                {
                    //Opens the Tower Depths XML file.
                    XmlDocument doc = ReadXml(Files.GetDefinitionFilePath(EnumXmlFiles.XmlFileTowerDepths));
                    XmlNode towerDepths = doc.ChildNodes[1];
                    //Creates the tower depths objects and adds them to the list.
                    foreach (XmlNode newTowerDepth in towerDepths)
                        new TowerDepth(newTowerDepth);
                }
                catch (Exception ex)
                {
                    ErrorLog.Log(ex, "An error has occured while attempting to populate Tower Depths from XML.");
                }
            }
            /// <summary>
            /// Fill GameData.POSSIBLE_TILES list from XML file.
            /// </summary>
            public static void PopulateTiles()
            {
                try
                {
                    //Opens the Tiles XML file.
                    XmlDocument doc = ReadXml(Files.GetDefinitionFilePath(EnumXmlFiles.XmlFileTiles));
                    XmlNode tiles = doc.ChildNodes[1];
                    //Creates the tiles objects and adds them to the list.
                    foreach (XmlNode tile in tiles)
                        new Tile(tile);
                }
                catch (Exception ex)
                {
                    ErrorLog.Log(ex, "An error has occured while attempting to populate Tiles from XML.");
                }
            }

            public static void PopulateMonsters()
            {
                try
                {
                    //Opens the Monsters XML file.
                    XmlDocument doc = ReadXml(Files.GetDefinitionFilePath(EnumXmlFiles.XmlFileMonsters));
                    XmlNode monsters = doc.ChildNodes[1];
                    //Creates the monsters objects and adds them to the list.
                    foreach (XmlNode monster in monsters)
                        new Monster(monster);
                }
                catch (Exception ex)
                {
                    ErrorLog.Log(ex, "An error has occured while attempting to populate Monsters from XML.");
                }
            }

            public static void PopulateItems()
            {
                try
                {
                    //Opens the Items XML file.
                    XmlDocument doc = ReadXml(Files.GetDefinitionFilePath(EnumXmlFiles.XmlFileItems));
                    XmlNode items = doc.ChildNodes[1];
                    //Creates the item objects and adds them to the list.
                    foreach (XmlNode item in items)
                        switch (item.Name)
                        {
                            case "Weapon":
                                new Weapon(item);
                                break;
                            case "Armor":
                                new Armor(item);
                                break;
                            case "Item":
                                new Item(item);
                                break;
                            default: break;
                        }
                }
                catch (Exception ex)
                {
                    ErrorLog.Log(ex, "An error has occured while attempting to populate Items from XML.");
                }
            }

            public static List<HighScore> ReadHighScores()
            {
                if (!File.Exists(Files.GetDefinitionFilePath(EnumXmlFiles.XmlFileHighScores)))
                    CreateHighScoreXmlFile();
                XmlDocument xml = new XmlDocument();
                xml.Load(Files.GetDefinitionFilePath(EnumXmlFiles.XmlFileHighScores));
                List<HighScore> highScores = new List<HighScore>();
                foreach (XmlNode highScore in xml.ChildNodes[1].ChildNodes)
                    highScores.Add(new HighScore()
                    {
                        CharacterName = highScore.ChildNodes[0].InnerText,
                        Score = Convert.ToInt32(highScore.ChildNodes[1].InnerText),
                        Date = highScore.ChildNodes[2].InnerText
                    });
                return highScores;
            }

            //adds a highscore
            public static void AddHighScore(string characterName, string score)
            {
                if (!File.Exists(Files.GetDefinitionFilePath(EnumXmlFiles.XmlFileHighScores)))
                    CreateHighScoreXmlFile(); //create the file if it does not already exist
                HighScore newScore = new HighScore()
                {
                    CharacterName = characterName,
                    Score = Convert.ToInt32(score),
                    Date = DateTime.Now.ToString("dd/MM/yyyy-HH:mm")
                };
                GameStatus.FinalizedHighScore = newScore;
                SortHighScores(newScore);
            }

            private static void CreateHighScoreXmlFile()
            {
                XmlWriterSettings settings = new XmlWriterSettings()
                {
                    Indent = true,
                    //Encoding = System.Text.Encoding.UTF32
                };
                using (XmlWriter xml = XmlWriter.Create(Files.GetDefinitionFilePath(EnumXmlFiles.XmlFileHighScores), settings))
                {
                    xml.WriteStartDocument();
                    xml.WriteStartElement("HighScores");
                    xml.WriteEndElement();
                    xml.WriteEndDocument();
                    xml.Close();
                }
            }

            private static XmlElement CreateNewHighScore(XmlDocument xml, HighScore newScore)
            {
                XmlElement highScore = xml.CreateElement("HighScore");
                XmlElement xmlName = xml.CreateElement("CharacterName");
                xmlName.InnerText = newScore.CharacterName;
                XmlElement xmlScore = xml.CreateElement("Score");
                xmlScore.InnerText = newScore.Score.ToString();
                XmlElement datetime = xml.CreateElement("Date");
                datetime.InnerText = newScore.Date;
                highScore.AppendChild(xmlName);
                highScore.AppendChild(xmlScore);
                highScore.AppendChild(datetime);
                return highScore;
            }

            private static void SortHighScores(HighScore highScore)
            {
                List<HighScore> scores = ReadHighScores();
                int lowestOfOldScores = scores.Count() > 0 ? Convert.ToInt32(scores.Min(x => x.Score)) : 0;
                int newScore = Convert.ToInt32(highScore.Score);
                if (lowestOfOldScores > newScore) //if the new score is the lowest score, don't add it
                    return;
                XmlDocument xml = new XmlDocument();
                xml.Load(Files.GetDefinitionFilePath(EnumXmlFiles.XmlFileHighScores));
                xml.ChildNodes[1].RemoveAll(); //remove the previous scores from the file -- but we have them saved in 'scores'
                scores.Add(highScore);
                IEnumerable<HighScore> orderedScores = scores.OrderByDescending(x => x.Score).Take(10); //take the 10 best highscores
                foreach (HighScore score in orderedScores)
                    xml.ChildNodes[1].AppendChild(CreateNewHighScore(xml, score));
                xml.Save(Files.GetDefinitionFilePath(EnumXmlFiles.XmlFileHighScores));
            }
        }

        /// <summary>
        /// This class handles general file-related actions.
        /// </summary>
        public static class Files
        {
            /// <summary>
            /// Gets the full path of the parameter file.
            /// </summary>
            /// <param name="file">The file we want to get the path of.</param>
            /// <returns>The full path.</returns>
            public static string GetDefinitionFilePath(EnumXmlFiles file)
            {
                //Gets the path of the project.
                string path = AppDomain.CurrentDomain.BaseDirectory;
                //Combines the project path with the relative file path.
                path = Path.Combine(path, XML_FILES[(int)file]);
                return path;
            }

            /// <summary>
            /// Concat the project's base directory to the parameter string.
            /// </summary>
            /// <param name="pathToAdd">The path whose full path you want.</param>
            /// <returns>Full path.</returns>
            public static string GetFullPath(string pathToAdd)
            {
                string path = AppDomain.CurrentDomain.BaseDirectory;
                path = Path.Combine(path, "/", pathToAdd);
                return path;
            }
        }

        /// <summary>
        /// This class documents errors that may have occured, such as bad XML format and exceptions
        /// </summary>
        public static class ErrorLog
        {
            /// <summary>
            /// If the error is fatal, stop running the game.
            /// </summary>
            public enum EnumErrorSeverity
            {
                Light = 0,
                Fatal = 1
            }


            /// <summary>
            /// Creates an error-log entry and quits the application.
            /// </summary>
            /// <param name="ex">The exception details that will be written to the file.</param>
            /// <param name="details">(Optional) A custom message to further elaborate on what the error is.</param>
            public static void Log(Exception ex, string details = null, EnumErrorSeverity severity = 0)
            {
                FileInfo logFile = GetLogFile();
                using (System.IO.StreamWriter file =
                        new System.IO.StreamWriter(logFile.FullName, true))
                {
                    //No need to close, as "Using" does so automatically.
                    file.WriteLine(Environment.NewLine);
                    file.WriteLine(DateTime.Now + " - " + details + Environment.NewLine + ex);
                }
                new ErrorPrompt(severity);
            }

            /// <summary>
            /// Look for the log file and creates it if it does not exist.
            /// </summary>
            /// <returns>The file information.</returns>
            private static FileInfo GetLogFile()
            {
                string path = AppDomain.CurrentDomain.BaseDirectory;
                //Find the directories that exist in the project folder
                string[] directories = Directory.GetDirectories(Directory.GetCurrentDirectory());
                //Find the Logs Directory
                path = directories.Where(x => x.Equals(path + "Logs")).FirstOrDefault();
                //If the Logs Directory exists, get it. If it doesn't, create it.
                DirectoryInfo logsDirectory = String.IsNullOrEmpty(path) ? Directory.CreateDirectory("Logs") : new DirectoryInfo(path);
                //Navigate to the Logs Directory.
                Directory.SetCurrentDirectory(logsDirectory.FullName);
                //Get all the log files that exist.
                FileInfo[] logFiles = logsDirectory.GetFiles();
                //Find the file that has a creation date for the current date.
                FileInfo logFile = logFiles.Where(x => x.CreationTime.Date.Equals(DateTime.Now.Date)).FirstOrDefault();
                //If the file doesn't exist, create it. If it does, return it.
                logFile = logFile ?? new FileInfo("Error Log - " + DateTime.Now.Date.ToString("dd/MM/yyyy") + ".txt");
                //Opens or creates the file
                /* CHECK WHAT IT DOES
                 * 
                FileStream logFileStream = logFile.Open(FileMode.OpenOrCreate);
                logFileStream.Close();
                */
                return logFile;
            }

        }


        /// <summary>
        /// Taken from: https://stackoverflow.com/questions/7153248/wpf-getting-a-collection-of-all-controls-for-a-given-type-on-a-page 
        /// These two methods return all the objects of the specified type.
        /// </summary>
        /// <typeparam name="T">The type of object collection you want to retrieve (Button, Image, Label..)</typeparam>
        /// <param name="parent">Where you want to retrieve it from. (Canvas, Grid, Window..)</param>
        /// <returns></returns>
        public static List<T> GetLogicalChildCollection<T>(object parent) where T : DependencyObject
        {
            List<T> logicalCollection = new List<T>();
            GetLogicalChildCollection(parent as DependencyObject, logicalCollection);
            return logicalCollection;
        }

        /// <summary>
        ///Taken from: https://stackoverflow.com/questions/7153248/wpf-getting-a-collection-of-all-controls-for-a-given-type-on-a-page
        /// These two methods return all the objects of the specified type.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="parent"></param>
        /// <param name="logicalCollection"></param>
        private static void GetLogicalChildCollection<T>(DependencyObject parent, List<T> logicalCollection) where T : DependencyObject
        {
            IEnumerable children = LogicalTreeHelper.GetChildren(parent);
            foreach (object child in children)
            {
                if (child is DependencyObject)
                {
                    DependencyObject depChild = child as DependencyObject;
                    if (child is T)
                    {
                        logicalCollection.Add(child as T);
                    }
                    GetLogicalChildCollection(depChild, logicalCollection);
                }
            }
        }

        /// <summary>
        /// Get an object by it's type, parent and name.
        /// </summary>
        /// <typeparam name="T">The requested object's type (Button, Label, Textbox..)</typeparam>
        /// <param name="parent">The requested object's parent. (Grid, Canvas, Window, Page..)</param>
        /// <param name="name">The requested object's x:name.</param>
        /// <returns>The object.</returns>
        public static T GetObjectByName<T>(DependencyObject parent, string name)
        {
            T result = default(T);
            FindObjectByName(parent, name, ref result);
            return result;
        }

        /// <summary>
        /// Finds an object by it's type, parent and name.
        /// </summary>
        /// <typeparam name="T">The requested object's type (Button, Label, Textbox..)</typeparam>
        /// <param name="parent">The requested object's parent. (Grid, Canvas, Window, Page..)</param>
        /// <param name="name">The requested object's x:name.</param>
        /// <param name="result">Where the result reference is saved.</param>
        private static void FindObjectByName<T>(DependencyObject parent, string name, ref T result)
        {
            IEnumerable children = LogicalTreeHelper.GetChildren(parent);
            foreach (object child in children)
            {
                if (child is DependencyObject)
                {
                    DependencyObject depChild = child as DependencyObject;
                    if (((FrameworkElement)child).Name.Equals(name))
                    {
                        result = (T)child;
                        return;
                    }
                    if (result != null) return;
                    FindObjectByName<T>(depChild, name, ref result);
                }
            }
        }


    }

    public static class GameLogic
    {

        public static StackPanel GameLog;

        public static int DiceRoll(string dice, int bonus = 0)
        {
            int numOfDice = Convert.ToInt32(dice.Split('d')[0]);
            int typeOfDie = Convert.ToInt32(dice.Split('d')[1]);
            int sum = 0;
            for (int i = 0; i < numOfDice; i++)
                sum += GameStatus.Random.Next(1, typeOfDie + 1);
            return sum + bonus;
        }

        public static int Roll20(int numOf20s)
        {
            return DiceRoll($"{numOf20s}d20");
        }

        public static int RollDamage(string damageRange) //format {min limit}-{max limit}, for example 3-5
        {
            return GameStatus.Random.Next(Convert.ToInt32(damageRange.Split('-')[0]), Convert.ToInt32(damageRange.Split('-')[1]) + 1);
        }

        public static void PrintToGameLog(string message)
        {
            UIElementCollection textblocks = GameLog.Children;
            for (int i = textblocks.Count - 1; i > 0; i--)
                (textblocks[i] as TextBlock).Text = (textblocks[i - 1] as TextBlock).Text;
            (textblocks[0] as TextBlock).Text = message;
        }
    }

    /// <summary>
    /// get a tile in Bitmap from the given tileset
    /// </summary>
    public static class CreateTile
    {
        /// <summary>
        /// The tile indices that we will use to extract the image from the spritesheet.
        /// </summary>
        public enum XmlTileIndices
        {
            OrdinaryWall = 1365,
            OrdinaryFloor = 1434,
            OrdinaryStairsDown = 1381,
            OrdinaryStairsUp = 1387,
            Jelly = 146,
            AmuletOfTheTower = 1207
        }

        public enum Tiles
        {
            OrdinaryWall = 0,
            OrdinaryFloor = 1,
            OrdinaryStairsDown = 2,
            OrdinaryStairsUp = 3,
            Jelly = 4,
            AmuletOfTheTower = 5
        }

        /// <summary>
        /// Definitions for the size of each tile and the number of indices in the spritesheet
        /// </summary>
        private const int TILESET_WIDTH = 30, TILE_WIDTH = 32, TILE_HEIGHT = 32;
        private const string TILESET = "../../Assets/Images/rltiles-2d.png";

        /// <summary>
        /// get a cropped ImageSource of a tile
        /// </summary>
        /// <param name="index">The index of the image in the spritesheet</param>
        /// <returns></returns>
        public static ImageSource GetImageFromTileset(int index)
        {
            int row = index / TILESET_WIDTH * TILE_HEIGHT;
            int col = (index % TILESET_WIDTH) * TILE_WIDTH;

            //create a bitmap image with the whole spritesheet
            Bitmap source = new Bitmap(TILESET);

            //crop a bitmap image out of the spritesheet in accordance with the coordinates
            Bitmap bmp = source.Clone(new Rectangle(col, row, TILE_WIDTH, TILE_HEIGHT), source.PixelFormat);

            //convert bitmap into an imagesource and return it
            var handle = bmp.GetHbitmap();
            return Imaging.CreateBitmapSourceFromHBitmap(handle, IntPtr.Zero, Int32Rect.Empty, BitmapSizeOptions.FromEmptyOptions());
        }

        /// <summary>
        /// Overlays one image on top of the other
        /// </summary>
        /// <param name="first">The image at the background</param>
        /// <param name="second">The image at the foreground</param>
        /// <returns>The merged image</returns>
        public static ImageSource Overlay(ImageSource first, ImageSource second)
        {
            var group = new DrawingGroup();
            group.Children.Add(new ImageDrawing(first, new Rect(0, 0, first.Width, first.Height)));
            group.Children.Add(new ImageDrawing(second, new Rect(0, 0, first.Width, first.Height)));

            return new DrawingImage(group);

        }

    }

    public class HighScore
    {
        public string CharacterName { get; set; }
        public int Score { get; set; }
        public string Date { get; set; }
    }

}
