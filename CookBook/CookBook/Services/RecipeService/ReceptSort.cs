using Models;
using System;
using System.Collections.Generic;

namespace Services
{
    public static class ReceptSort
    {
        public static void SortirajPoKategoriji(List<Recipe> recepti)
        {
            QuickSort(recepti, 0, recepti.Count - 1);
        }

        private static void QuickSort(List<Recipe> recepti, int left, int right)
        {
            if (left < right)
            {
                int pivotIndex = Partition(recepti, left, right);
                QuickSort(recepti, left, pivotIndex - 1);
                QuickSort(recepti, pivotIndex + 1, right);
            }
        }

        private static int Partition(List<Recipe> recepti, int left, int right)
        {
            string pivot = recepti[right].Kategorija;
            int i = left - 1;

            for (int j = left; j < right; j++)
            {
                if (string.Compare(recepti[j].Kategorija, pivot, StringComparison.OrdinalIgnoreCase) < 0)
                {
                    i++;
                    Swap(recepti, i, j);
                }
            }
            Swap(recepti, i + 1, right);
            return i + 1;
        }

        private static void Swap(List<Recipe> recepti, int i, int j)
        {
            var temp = recepti[i];
            recepti[i] = recepti[j];
            recepti[j] = temp;
        }
    }
}
