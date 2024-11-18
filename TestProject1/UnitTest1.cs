using FlipCardProject.Records;

namespace TestProject1;
using FlipCardProject.Models;
using Xunit;
using FlipCardProject.Extensions;
public class UnitTest1
{
    [Fact]
    public void Test1()
    {
    }
    public class CardShuffleTests
    {
        [Fact]
        public void CardShuffle_ShouldChangeOrderOfCards()
        {
            // Arrange
            FlipcardSet flipcardSet = new FlipcardSet("whatevz");
            
            flipcardSet.AddFlipcard(new FlipcardState(),"1","1","1");
            flipcardSet.AddFlipcard(new FlipcardState(),"2","2","2");
            flipcardSet.AddFlipcard(new FlipcardState(),"3","3","3");
            
            var originalOrder = flipcardSet.FlipcardsList.ToList();

            bool isShuffled = false;

            
            for (int i = 0; i < 10; i++) 
            {
                flipcardSet.CardShuffle();
                if (!originalOrder.SequenceEqual(flipcardSet.FlipcardsList))
                {
                    isShuffled = true;
                    break;
                }
            }

           
            Assert.True(isShuffled, "The order of cards should be shuffled.");
        }
    }
}