using Models;
using System;
using System.Collections.Generic;

namespace Services
{
    public static class ReceptSort
    {
        public static void SortirajPoKategoriji(List<Recipe> recepti)
        {
            //QuickSort(recepti, 0, recepti.Count - 1);
            BadQuickSort(recepti);
        }

        /*private static void QuickSort(List<Recipe> recepti, int left, int right)
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
        }*/
        /*public static void BadQuickSort(List<Recipe> recepti)
        {
            
            string globalPivot;
            
            void DoTheSort(int l, int r)
            {
                
                var x= l;
                var y= r;
               
                if(r-l<=5) return;
                
                globalPivot = recepti[l+(r-l)/2+(r-l)%2].Kategorija;

               
                while(true){

                    while(true){

                        if(string.Compare(recepti[x].Kategorija, globalPivot,
                            StringComparison.OrdinalIgnoreCase) >= 0) break;
                        x++; }
                    while(true){

                        if(string.Compare(recepti[y].Kategorija, globalPivot,
                            StringComparison.OrdinalIgnoreCase) <= 0) break;
                        y--; }
                   
                    if(x<=y)
                    {
                        var temp= recepti[x];
                        recepti[x]= recepti[y];
                        recepti[y]= temp;
                        x++;
                        y--; }

                    if(x>y)break;
                }              
                if(l<y) DoTheSort(l, y);
                if(x<r) DoTheSort(x, r);
            }     
                DoTheSort(0,recepti.Count-1);    
        }*/


        public static void QuickSortRecipes(List<Recipe> recipes)
        {
            
            if(recipes == null || recipes.Count == 0)
                return;

            const int MIN_SIZE = 5; 
            QuickSort(recipes, 0, recipes.Count - 1, MIN_SIZE);
        }

        private static void QuickSort(List<Recipe> recipes, int left, int right, int minSize)
        {
            if (right-left <= minSize) return; 
            
            string pivot = recipes[left + (right - left) / 2].Kategorija;

            int i= left;
            int j= right;

            while(i<=j)
            {      
                while(string.Compare(recipes[i].Kategorija, pivot, StringComparison.OrdinalIgnoreCase) < 0) i++;
                while(string.Compare(recipes[j].Kategorija, pivot, StringComparison.OrdinalIgnoreCase) > 0) j--;
               
                if(i<=j)
                {
                    Swap(recipes, i, j);
                    i++;
                    j--;
                }
            }
           
            if(left<j) QuickSort(recipes, left, j, minSize);
            if(i < right) QuickSort(recipes, i, right, minSize);
        }
        private static void Swap(List<Recipe> recipes, int i, int j)
        {
            var temp = recipes[i];
            recipes[i] = recipes[j];
            recipes[j] = temp;

        }
    }
}
