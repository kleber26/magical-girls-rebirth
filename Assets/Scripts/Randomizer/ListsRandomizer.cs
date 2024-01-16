using System;
using System.Collections.Generic;

namespace Randomizer
{
    public static class ListsRandomizer
    {
        public static List<T> GetRandomElementsFromList<T>(List<T> elements, int numberOfRandomElements)
        {
            List<T> randomElements = new List<T>();
            var random = new Random();
           
            for (int i = 0; i < numberOfRandomElements; i++)
            {
                int index = random.Next(elements.Count);
                randomElements.Add(elements[index]);
                elements.RemoveAt(index);
            }

            return randomElements;
        }
    }
}