using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace SpotifyEditor.Helpers
{
    public static class VisualTreeHelpers
    {
        public static T? FindParentOfType<T>(DependencyObject child) where T : class
        {
            while (child != null)
            {
                if (child is T match)
                    return match;

                child = VisualTreeHelper.GetParent(child);
            }
            return null;
        }
    }
}
