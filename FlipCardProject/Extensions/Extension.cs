using FlipCardProject.Models;

namespace FlipCardProject.Extensions;
using FlipCardProject.Models;

static public class Extension
{
    
        public static void CardShuffle(this FlipcardSet set)
        {
            Random rng = new Random();
            int n = set.FlipcardsList.Count;
            for (int i = 0; i < n - 1; i++)
            {
                int j = rng.Next(i, n);
                Flipcard temp = set.FlipcardsList[i];
                set.FlipcardsList[i] = set.FlipcardsList[j];
                set.FlipcardsList[j] = temp;
            }
        }
        
        public static void FromDtoToCardSet(this FlipcardSet set, FlipcardSetDto dtoSet)
        {
            set.FlipcardsList = dtoSet.FlipcardsList;
            set.SetName = dtoSet.SetName;
        }
        
        
        
}