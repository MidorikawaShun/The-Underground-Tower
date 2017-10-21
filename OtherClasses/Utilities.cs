using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Xml;
using TheUndergroundTower.OtherClasses;
using WpfApp1.Windows.MetaMenus;
using static WpfApp1.Definitions;

namespace WpfApp1
{
    public static class Utilities
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
                    new ErrorPrompt();
                }
            }

            public static void PopulateClasses()
            {
                try
                {
                    XmlDocument doc = ReadXml(Files.GetDefinitionFilePath(EnumXmlFiles.XmlFileClasses));
                    XmlNode classes = doc.ChildNodes[1];
                    foreach (XmlNode newClass in classes)
                        new Class(newClass);
                }
                catch (Exception ex)
                {
                    ErrorLog.Log(ex, "An error has occured while attempting to populate classes from XML.");
                    new ErrorPrompt();
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
            /// <summary>
            /// Create the log entry.
            /// </summary>
            /// <param name="ex">The exception details that will be written to the file.</param>
            /// <param name="details">(Optional) A custom message to further elaborate on what the error is.</param>
            public static void Log(Exception ex, string details = null)
            {
                FileInfo logFile = GetLogFile();
                using (System.IO.StreamWriter file =
                        new System.IO.StreamWriter(logFile.FullName, true))
                {
                    //No need to close, as "Using" does so automatically.
                    file.WriteLine(Environment.NewLine);
                    file.WriteLine(DateTime.Now + " - " + details + Environment.NewLine + ex);
                }
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
        

        public static void PlaySFX(EnumSfxFiles sfx)
        {

        }

    }
}
