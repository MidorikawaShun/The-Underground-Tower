using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace TheUndergroundTower
{
    public static class ExtensionMethods
    {
        public static bool HasChild(this DependencyObject me, DependencyObject parent, object target)
        {
            for (int i = 0; i < VisualTreeHelper.GetChildrenCount(parent); i++)
            {
                DependencyObject child = VisualTreeHelper.GetChild(parent, i);
                if (child != null && child == target)
                    return true;
                else
                    if (HasChild(me,child, target))
                    return true;
            }
            return false;
        }
    }
}
