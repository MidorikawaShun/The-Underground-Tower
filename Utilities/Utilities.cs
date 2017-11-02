using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Media;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Xml;
using TheUndergroundTower.OtherClasses;
using TheUndergroundTower.Pathfinding;
using WpfApp1.Windows.MetaMenus;
using static WpfApp1.Definitions;

namespace WpfApp1
{
    public static class Options
    {
        /// <summary>
        /// This class handles XML-related actions.
        /// </summary>
        public static class Xml
        {

            public static XmlDocument ReadXml(string XmlFilePath)
            {
                XmlDocument doc = new XmlDocument();
                doc.Load(XmlFilePath);
                return doc;
            }

            /// <summary>
            /// Fill GameData.RACES list from XML file.
            /// </summary>
            public static void PopulateRaces()
            {
                try
                {
                    XmlDocument doc = ReadXml(Files.GetDefinitionFilePath(EnumXmlFiles.XmlFileRaces));
                    XmlNode races = doc.ChildNodes[1];
                    foreach (XmlNode race in races)
                        new Race(race);
                }
                catch (Exception ex)
                {
                    ErrorLog.Log(ex, "An error has occured while attempting to populate races from XML.");
                }
            }

            public static void PopulateCareers()
            {
                try
                {
                    XmlDocument doc = ReadXml(Files.GetDefinitionFilePath(EnumXmlFiles.XmlFileCareers));
                    XmlNode careers = doc.ChildNodes[1];
                    foreach (XmlNode newCareer in careers)
                        new Career(newCareer);
                }
                catch (Exception ex)
                {
                    ErrorLog.Log(ex, "An error has occured while attempting to populate classes from XML.");
                }
            }

            public static void PopulateDifficulties()
            {
                try
                {
                    XmlDocument doc = ReadXml(Files.GetDefinitionFilePath(EnumXmlFiles.XmlFileDifficulties));
                    XmlNode difficulties = doc.ChildNodes[1];
                    foreach (XmlNode newDifficulty in difficulties)
                        new Difficulty(newDifficulty);
                }
                catch (Exception ex)
                {
                    ErrorLog.Log(ex, "An error has occured while attempting to populate difficulties from XML.");
                }
            }

            public static void PopulateTowerDifficulties()
            {
                try
                {
                    XmlDocument doc = ReadXml(Files.GetDefinitionFilePath(EnumXmlFiles.XmlFileTowerDepths));
                    XmlNode difficulties = doc.ChildNodes[1];
                    foreach (XmlNode newTowerDepth in difficulties)
                        new TowerDepth(newTowerDepth);
                }
                catch (Exception ex)
                {
                    ErrorLog.Log(ex, "An error has occured while attempting to populate Tower Depths from XML.");
                }
            }

            public static void PopulateTiles()
            {
                try
                {
                    XmlDocument doc = ReadXml(Files.GetDefinitionFilePath(EnumXmlFiles.XmlFileTiles));
                    XmlNode tiles = doc.ChildNodes[1];
                    foreach (XmlNode tile in tiles)
                        new Tile(tile);
                }
                catch (Exception ex)
                {
                    ErrorLog.Log(ex, "An error has occured while attempting to populate Tower Depths from XML.");
                }
            }

        }

        /// <summary>
        /// This class handles general file-related actions.
        /// </summary>
        public static class Files
        {
            public static string GetDefinitionFilePath(EnumXmlFiles file)
            {
                string path = AppDomain.CurrentDomain.BaseDirectory;
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
                string[] directories = Directory.GetDirectories(Directory.GetCurrentDirectory());
                path = directories.Where(x => x.Equals(path + "Logs")).FirstOrDefault();
                DirectoryInfo logsDirectory = String.IsNullOrEmpty(path) ? Directory.CreateDirectory("Logs") : new DirectoryInfo(path);
                Directory.SetCurrentDirectory(logsDirectory.FullName);
                FileInfo[] logFiles = logsDirectory.GetFiles();
                FileInfo logFile = logFiles.Where(x => x.CreationTime.Date.Equals(DateTime.Now.Date)).FirstOrDefault();
                logFile = logFile ?? new FileInfo("Error Log - " + DateTime.Now.Date.ToString("dd/MM/yyyy") + ".txt");
                FileStream logFileStream = logFile.Open(FileMode.OpenOrCreate);
                logFileStream.Close();
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

    /// <summary>
    /// get a tile in Bitmap from the given tileset
    /// </summary>
    public static class CreateTile
    {

        public enum Tiles
        {
            OrdinaryWall = 1365
        }

        private const int TILESET_WIDTH = 30, TILE_WIDTH = 32, TILE_HEIGHT = 30;
        private const string TILESET = "../../Assets/Images/rltiles-2d.png";

        /// <summary>
        /// get a cropped Bitmap image with a tile
        /// </summary>
        /// <param name="index"></param>
        /// <param name="imagePath"></param>
        /// <returns></returns>
        public static ImageSource GetImageFromTileset(int index)
        {
            int row = index / TILESET_WIDTH * TILE_HEIGHT;
            int col = (index % TILESET_WIDTH) * TILE_WIDTH;
            Bitmap source = new Bitmap(TILESET);

            Rectangle r = new Rectangle(row, col, TILE_WIDTH, TILE_HEIGHT);
            Bitmap bmp = new Bitmap(r.Width, r.Height);

            Graphics g = Graphics.FromImage(bmp);
            g.DrawImage(source, -r.X, -r.Y);
            
            var handle = bmp.GetHbitmap();
            return Imaging.CreateBitmapSourceFromHBitmap(handle, IntPtr.Zero, Int32Rect.Empty, BitmapSizeOptions.FromEmptyOptions());
        }

        
    }
}
