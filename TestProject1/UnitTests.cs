using FlipCardProject.Records;

namespace TestProject1;
using FlipCardProject.Models;
using Xunit;
using FlipCardProject.Extensions;
using FlipCardProject.Services;

public class UnitTests
{
    private readonly UserTrackingService<int> _tracking;
    private readonly FlipcardSetValidator _validator;
    
    public UnitTests()
    {
        _tracking = new UserTrackingService<int>();
        _validator = new FlipcardSetValidator();
    }
    public class CardShuffleTests
    {
        [Fact]
        public void CardShuffle_ShouldChangeOrderOfCards()
        {
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

    [Fact]
    public void AddPlayer_ShouldIncreasePlayerCount()
    {
        _tracking.AddPlayer(1);
        
        Assert.Equal(1, _tracking.GetCurrentPlayerCount());
    }
    
    [Fact]
    public void RemovePlayer_ShouldDecreasePlayerCount()
    {
        _tracking.AddPlayer(1);
        _tracking.AddPlayer(2);
        _tracking.AddPlayer(3);
        _tracking.RemovePlayer(2);
        
        Assert.Equal(2, _tracking.GetCurrentPlayerCount());
    }
    
    [Fact]
    public void GetActivePlayers_ShouldReturnCorrectPlayers()
    {
        _tracking.AddPlayer(1);
        _tracking.AddPlayer(2);
        _tracking.AddPlayer(3);

        var activePlayers = _tracking.GetActivePlayers();

        Assert.Contains(1, activePlayers);
        Assert.Contains(2, activePlayers);
        Assert.Contains(3, activePlayers);
        Assert.Equal(3, activePlayers.Count);
    }
    
    [Fact]
    public void Validate_ShouldReturnTrue_WhenFlipcardSetIsValid()
    {
        var flipcardSet = new FlipcardSet("name");
        flipcardSet.AddFlipcard(new FlipcardState(), "1", "1", "1");

        var isValid = _validator.Validate(flipcardSet, out var errors);

        Assert.True(isValid);
        Assert.Empty(errors);
    }

    [Fact]
    public void Validate_ShouldReturnFalse_WhenNameIsNullOrEmpty()
    {
        var flipcardSet = new FlipcardSet(" ");
        flipcardSet.AddFlipcard(new FlipcardState(), "1", "1", "1");

        var isValid = _validator.Validate(flipcardSet, out var errors);
        
        Assert.False(isValid);
    }

    [Fact]
    public void Validate_ShouldReturnFalse_WhenFlipcardsListIsNull()
    {
        var flipcardSet = new FlipcardSet("name");
        flipcardSet.FlipcardsList = null;

        var isValid = _validator.Validate(flipcardSet, out var errors);

        Assert.False(isValid);
    }

    [Fact]
    public void Validate_ShouldReturnFalse_WhenBothNameAndFlipcardsListAreInvalid()
    {
        var flipcardSet = new FlipcardSet("");
        flipcardSet.FlipcardsList = null;

        var isValid = _validator.Validate(flipcardSet, out var errors);

        Assert.False(isValid);
        Assert.Equal(2, errors.Count);
    }
}