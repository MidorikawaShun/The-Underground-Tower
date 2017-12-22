using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace TheUndergroundTower
{
    /// <summary>
    /// A class for all our extension methods.
    /// </summary>
    public static class ExtensionMethods
    {
        /// <summary>
        /// Checks if a WPF object has a certain child
        /// </summary>
        /// <param name="me">the object that this method acts on.</param>
        /// <param name="target">The object we want to see if it exists.</param>
        /// <returns>True</returns>
        public static bool HasChild(this DependencyObject me, object target)
        {
            for (int i = 0; i < VisualTreeHelper.GetChildrenCount(me); i++)
            {
                DependencyObject child = VisualTreeHelper.GetChild(me, i);
                if (child == null) return false;
                if (child == target) return true;
                else
                {
                    if (HasChild(child, target))
                        return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Returns a random member of the list.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list">The list in question.</param>
        /// <param name="rand">An initialized Random object.</param>
        /// <returns>A random member of the list.</returns>
        public static T Random<T>(this List<T> list,Random rand)
        {
            return list[rand.Next(0, list.Count())];
        }
    }
}
