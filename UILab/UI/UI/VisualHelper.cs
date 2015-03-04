using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Media;

namespace KLibrary.Labs.UI
{
    public static class VisualHelper
    {
        public static IEnumerable<DependencyObject> EnumerateChildren(this DependencyObject obj)
        {
            if (obj == null) throw new ArgumentNullException("obj");

            var count = VisualTreeHelper.GetChildrenCount(obj);
            return Enumerable.Range(0, count).Select(i => VisualTreeHelper.GetChild(obj, i));
        }

        public static IEnumerable<DependencyObject> EnumerateChildrenAll(this DependencyObject obj)
        {
            if (obj == null) throw new ArgumentNullException("obj");

            foreach (var child in obj.EnumerateChildren())
            {
                yield return child;

                foreach (var grandchild in child.EnumerateChildrenAll())
                {
                    yield return grandchild;
                }
            }
        }

        public static IEnumerable<DependencyObject> EnumerateParentsAll(this DependencyObject obj)
        {
            if (obj == null) throw new ArgumentNullException("obj");

            var o = obj;
            while ((o = VisualTreeHelper.GetParent(o)) != null)
            {
                yield return o;
            }
        }
    }
}
