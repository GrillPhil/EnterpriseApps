using System.Collections.Generic;
using System.Linq;

namespace EnterpriseApps.Portable.ExtensionMethods
{
    public static class ListExtensions
    {
        public static void Filter<T>(this IList<T> collection, IEnumerable<T> filteredCollection)
        {
            var itemsToRemove = collection.Except(filteredCollection).ToArray();

            foreach (var item in itemsToRemove)
            {
                collection.Remove(item);
            }

            foreach (var item in filteredCollection)
            {
                if (!collection.Contains(item))
                    collection.Add(item);
            }
        }
    }
}
