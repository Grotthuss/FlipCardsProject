namespace TestProject1;
using FlipCardProject.Data;
using FlipCardProject.Enums;
using FlipCardProject.Models;
using FlipCardProject.Records;
using FlipCardProject.Services;
using Microsoft.EntityFrameworkCore;
public class IntegrationTests : IDisposable
{
    private readonly DataContext _context;
    private readonly FlipcardRepository _repository;

    public IntegrationTests()
    {
        var options = new DbContextOptionsBuilder<DataContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        _context = new DataContext(options);

        var user = new User
        {
            Id = 1,
            Name = "Jonas",
            Email = "jonas@gmail.com",
            Password = "password",
            FlipcardSets = new List<FlipcardSet>
            {
            }
        };

        var set1 = new FlipcardSet("set1");
        set1.AddFlipcard("1","1","1");
        set1.AddFlipcard("2","2","2");
        set1.AddFlipcard("3","3","3");
        set1.Id = 1;
        
        
        var set2 = new FlipcardSet("set2");
        set2.AddFlipcard("1","1","1");
        set2.AddFlipcard("2","2","2");
        set2.AddFlipcard("3","3","3");
        set2.Id = 2;
        user.FlipcardSets.Add(set1);
        user.FlipcardSets.Add(set2);
        
        _context.Users.Add(user);
        _context.SaveChanges();

        _repository = new FlipcardRepository(_context);
    }

    [Fact]
    public async Task GetAllFlipcardSetsAsync_ReturnsAllFlipcardSets()
    {
        var sets = await _repository.GetAllFlipcardSetsAsync(1);
        
        Assert.NotNull(sets);
        Assert.Equal(2, sets.Count);
        Assert.Contains(sets, s => s.Name == "set1");
        Assert.Contains(sets, s => s.Name == "set2");
        
        var set1 = sets.First(s => s.Id == 1);
        Assert.Equal(3, set1.FlipcardsList.Count);
        
        var flipcard1 = set1.FlipcardsList.FirstOrDefault(f => f.Id == 1);
        Assert.NotNull(flipcard1);
       // Assert.Equal(FlipCardStateEnum.UNANSWERED, flipcard1.State._state);
        Assert.Equal("1", flipcard1.Question);
        Assert.Equal("1", flipcard1.Concept);
        Assert.Equal("1", flipcard1.Mnemonic);

        var flipcard2 = set1.FlipcardsList.FirstOrDefault(f => f.Id== 2);
        Assert.NotNull(flipcard2);
        //Assert.Equal(FlipCardStateEnum.UNANSWERED, flipcard2.State._state);
        Assert.Equal("2", flipcard2.Question);
        Assert.Equal("2", flipcard2.Concept);
        Assert.Equal("2", flipcard2.Mnemonic);

        var flipcard3 = set1.FlipcardsList.FirstOrDefault(f => f.Id == 3);
        Assert.NotNull(flipcard3);
        //Assert.Equal(FlipCardStateEnum.UNANSWERED, flipcard3.State._state);
        Assert.Equal("3", flipcard3.Question);
        Assert.Equal("3", flipcard3.Concept);
        Assert.Equal("3", flipcard3.Mnemonic);
        
        var set2 = sets.First(s => s.Id == 2);
        Assert.Equal(3, set2.FlipcardsList.Count);
        
        var flipcard11 = set2.FlipcardsList.FirstOrDefault(f => f.Id == 4);
        Assert.NotNull(flipcard11);
        //Assert.Equal(FlipCardStateEnum.UNANSWERED, flipcard11.State._state);
        Assert.Equal("1", flipcard11.Question);
        Assert.Equal("1", flipcard11.Concept);
        Assert.Equal("1", flipcard11.Mnemonic);

        var flipcard22 = set2.FlipcardsList.FirstOrDefault(f => f.Id== 5);
        Assert.NotNull(flipcard22);
        //Assert.Equal(FlipCardStateEnum.UNANSWERED, flipcard22.State._state);
        Assert.Equal("2", flipcard22.Question);
        Assert.Equal("2", flipcard22.Concept);
        Assert.Equal("2", flipcard22.Mnemonic);

        var flipcard33 = set2.FlipcardsList.FirstOrDefault(f => f.Id == 6);
        Assert.NotNull(flipcard33);
        //Assert.Equal(FlipCardStateEnum.UNANSWERED, flipcard33.State._state);
        Assert.Equal("3", flipcard33.Question);
        Assert.Equal("3", flipcard33.Concept);
        Assert.Equal("3", flipcard33.Mnemonic);
        
    }

    [Fact]
    public async Task GetFlipcardSetByIdAsync_ReturnsCorrectFlipcardSet()
    {
        var set = await _repository.GetFlipcardSetByIdAsync(1,1);

        Assert.NotNull(set);
        Assert.Equal("set1", set.Name);
        Assert.Equal(3, set.FlipcardsList.Count);
        
        var flipcard1 = set.FlipcardsList.FirstOrDefault(f => f.Id == 1);
        Assert.NotNull(flipcard1);
        //Assert.Equal(FlipCardStateEnum.UNANSWERED, flipcard1.State._state);
        Assert.Equal("1", flipcard1.Question);
        Assert.Equal("1", flipcard1.Concept);
        Assert.Equal("1", flipcard1.Mnemonic);

        var flipcard2 = set.FlipcardsList.FirstOrDefault(f => f.Id== 2);
        Assert.NotNull(flipcard2);
        //Assert.Equal(FlipCardStateEnum.UNANSWERED, flipcard2.State._state);
        Assert.Equal("2", flipcard2.Question);
        Assert.Equal("2", flipcard2.Concept);
        Assert.Equal("2", flipcard2.Mnemonic);

        var flipcard3 = set.FlipcardsList.FirstOrDefault(f => f.Id == 3);
        Assert.NotNull(flipcard3);
        //Assert.Equal(FlipCardStateEnum.UNANSWERED, flipcard3.State._state);
        Assert.Equal("3", flipcard3.Question);
        Assert.Equal("3", flipcard3.Concept);
        Assert.Equal("3", flipcard3.Mnemonic);
        
        set = await _repository.GetFlipcardSetByIdAsync(1,2);
        
        Assert.NotNull(set);
        Assert.Equal("set2", set.Name);
        Assert.Equal(3, set.FlipcardsList.Count);
        
        flipcard1 = set.FlipcardsList.FirstOrDefault(f => f.Id == 4);
        Assert.NotNull(flipcard1);
        //Assert.Equal(FlipCardStateEnum.UNANSWERED, flipcard1.State._state);
        Assert.Equal("1", flipcard1.Question);
        Assert.Equal("1", flipcard1.Concept);
        Assert.Equal("1", flipcard1.Mnemonic);

        flipcard2 = set.FlipcardsList.FirstOrDefault(f => f.Id== 5);
        Assert.NotNull(flipcard2);
        //Assert.Equal(FlipCardStateEnum.UNANSWERED, flipcard2.State._state);
        Assert.Equal("2", flipcard2.Question);
        Assert.Equal("2", flipcard2.Concept);
        Assert.Equal("2", flipcard2.Mnemonic);

        flipcard3 = set.FlipcardsList.FirstOrDefault(f => f.Id == 6);
        Assert.NotNull(flipcard3);
        //Assert.Equal(FlipCardStateEnum.UNANSWERED, flipcard3.State._state);
        Assert.Equal("3", flipcard3.Question);
        Assert.Equal("3", flipcard3.Concept);
        Assert.Equal("3", flipcard3.Mnemonic);
    }

        [Fact]
        public async Task AddFlipcardSetAsync_AddsFlipcardSetAndReturnsThatSet()
        {
            var flipcardSet = new FlipcardSet("set3");
            flipcardSet.AddFlipcard("1","1","1");
            flipcardSet.Id = 3;
            flipcardSet.UserId = 1;
            var returnedSet = await _repository.AddFlipcardSetAsync(flipcardSet);
            
            var originalFlipcard = flipcardSet.FlipcardsList.First();
            var returnedFlipcard = returnedSet.FlipcardsList.First();
            
            Assert.NotNull(returnedSet);
            Assert.Equal(flipcardSet.Name, returnedSet.Name);
            Assert.Equal(flipcardSet.FlipcardsList.Count, returnedSet.FlipcardsList.Count);
            Assert.Equal(originalFlipcard.Question, returnedFlipcard.Question);
            Assert.Equal(originalFlipcard.Concept, returnedFlipcard.Concept);
            Assert.Equal(originalFlipcard.Mnemonic, returnedFlipcard.Mnemonic);
        }

        [Fact]
        public async Task AddFlipcardAsync_AddsCardToCorrectSet()
        {
            var card = new Flipcard("4","4","4");

            await _repository.AddFlipcardAsync(1,2, card);
            
            var set = await _repository.GetFlipcardSetByIdAsync(1,2);
            var originalCard = set.FlipcardsList.FirstOrDefault(x => x.Id == 7);
            
            Assert.Equal(card.Question, originalCard.Question);
            Assert.Equal(card.Concept, originalCard.Concept);
            Assert.Equal(card.Mnemonic, originalCard.Mnemonic);
        }
        
        [Fact]
        public async Task UpdateFlipcardAsync_UpdatesCardCorrectly()
        {
            var card = new Flipcard("4","4","4");
            card.Id = 7;

            await _repository.UpdateFlipcardAsync(1,2, card);
            
            var set = await _repository.GetFlipcardSetByIdAsync(1,2);
            var updatedCard = set.FlipcardsList.FirstOrDefault(x => x.Id == 7);
            
            Assert.Equal(card.Question, updatedCard.Question);
            Assert.Equal(card.Concept, updatedCard.Concept);
            Assert.Equal(card.Mnemonic, updatedCard.Mnemonic);
        }

        [Fact]
        public async Task UpdateFlipcardSetAsync_UpdatesSetCorrectly()
        {
            var flipcardSet = new FlipcardSet("set3");
            flipcardSet.AddFlipcard("1","1","1");
            flipcardSet.Id = 2;
            flipcardSet.UserId = 1;
            await _repository.UpdateFlipcardSetAsync(flipcardSet);
            
            var updatedSet = await _repository.GetFlipcardSetByIdAsync(1,2);
            
            Assert.NotNull(updatedSet);
            Assert.NotEqual(3, updatedSet.FlipcardsList.Count);
        }
        
        [Fact]
        public async Task DeleteFlipcardSetAsync_DeletesCorrectSet()
        {
            int SetCountBeforeDeletion = (await _repository.GetAllFlipcardSetsAsync(1)).Count();
            await _repository.DeleteFlipcardSetAsync(1,2);
            
            int SetCountAfterDeletion = (await _repository.GetAllFlipcardSetsAsync(1)).Count();
            
            Assert.Equal(SetCountBeforeDeletion, SetCountAfterDeletion + 1);
        }
        
        [Fact]
        public async Task DeleteFlipcardAsync_DeletesCorrectCard()
        {
            int CardCountBeforeDeletion = (await _repository.GetFlipcardSetByIdAsync(1,2)).FlipcardsList.Count();
            
            await _repository.DeleteFlipcardAsync(1,2, 4);

            var affectedSet = await _repository.GetFlipcardSetByIdAsync(1,2);
            
            int CardCountAfterDeletion = affectedSet.FlipcardsList.Count();
            Assert.Equal(CardCountAfterDeletion, CardCountBeforeDeletion -1);
            
            var deletedCard = affectedSet.FlipcardsList.FirstOrDefault(c => c.Id == 4);
            Assert.Null(deletedCard);
        }

    public void Dispose()
    {
        _context.Dispose();
    }
}
