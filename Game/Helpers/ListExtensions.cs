using System.Collections.Generic;
using System;
using System.Linq;

namespace ExtensionMethods
{
    public static class ListExtensions
    {
        private static readonly Random rand = new Random();

        /// <summary>
        /// Adds an item at a random spot in the list
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        /// <param name="item"></param>
        public static void AddRandomly<T>(this List<T> list, T item)
        {
            list.Insert(list.GetRandomIndex(), item);
        }

        /// <summary>
        /// Adds a group of items randomly to a list.
        /// <para>Note: The items do not get added as a chunk, use AddRandomlyChunk for that.</para>
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        /// <param name="values"></param>
        public static void AddRandomly<T>(this List<T> list, IEnumerable<T> values)
        {
            foreach (var item in values)
            {
                list.AddRandomly(item);
            }   
        }
        
        /// <summary>
        /// Randomly adds items consecutively into a list maintaining order
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        /// <param name="values"></param>
        public static void AddRandomlyChunk<T>(this List<T> list, IEnumerable<T> values)
        {
            list.InsertRange(list.GetRandomIndex(), values);
        }

        /// <summary>
        /// Determines whether a list contains a sublist
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="mainList"></param>
        /// <param name="sublist"></param>
        /// <returns></returns>
        public static bool ContainsSublist<T>(this List<T> mainList, List<T> sublist)
        {
            if (sublist.Count > mainList.Count)
            {
                return false;
            }

            int numWindows = mainList.Count - sublist.Count + 1;

            for(int i = 0; i < numWindows; i += 1)
            {
                if (mainList.Skip(i).Take(sublist.Count).SequenceEqual(sublist))
                {
                    return true;
                }
            }
            
            return false;
        }

        /// <summary>
        /// Gets a random index in the list
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        /// <returns></returns>
        public static int GetRandomIndex<T>(this List<T> list) 
        {
            return rand.Next(0, list.Count);
        }

        /// <summary>
        /// Gets a random item in the collection
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        /// <returns></returns>
        public static T GetRandomItem<T>(this List<T> list)
        {
            return list[rand.Next(0, list.Count)];
        }

        /// <summary>
        /// Shuffles the list using Fischer Yates shuffle
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        /// <returns></returns>
        public static void Shuffle<T>(this List<T> list)
        {
            int n = list.Count;
            while (n > 1)
            {
                n--;
                int k = rand.Next(n + 1);
                (list[n], list[k]) = (list[k], list[n]);
            }
        }
    }
    
}
