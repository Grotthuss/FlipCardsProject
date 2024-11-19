using FlipCardProject.Data;
using FlipCardProject.Enums;
using FlipCardProject.Models;
using FlipCardProject.Records;
using FlipCardProject.Services;
using Microsoft.EntityFrameworkCore;

namespace TestProject1;

public class FlipcardRepositoryTests : IDisposable
{
    private readonly DataContext _context;
    private readonly FlipcardRepository _repository;

    public FlipcardRepositoryTests()
    {
        var options = new DbContextOptionsBuilder<DataContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()) // Unique DB for each test
            .Options;

        _context = new DataContext(options);

        // Seed test data
        var user = new User
        {
            Id = 1,
            Name = "Jonas",
            Email = "jonas@gmail.com",
            FlipcardSets = new List<FlipcardSet>
            {
            }
        };

        var set1 = new FlipcardSet("set1");
        set1.AddFlipcard(new FlipcardState(),"1","1","1");
        set1.AddFlipcard(new FlipcardState(),"2","2","2");
        set1.AddFlipcard(new FlipcardState(),"3","3","3");
        set1.Id = 1;
        //set1.UserId = 1;
        
        
        var set2 = new FlipcardSet("set2");
        set2.AddFlipcard(new FlipcardState(),"1","1","1");
        set2.AddFlipcard(new FlipcardState(),"2","2","2");
        set2.AddFlipcard(new FlipcardState(),"3","3","3");
        set2.Id = 2;
        //set2.UserId = 1;
        user.FlipcardSets.Add(set1);
        user.FlipcardSets.Add(set2);
        
        _context.Users.Add(user);
        _context.SaveChanges();

        _repository = new FlipcardRepository(_context);
    }

    [Fact]
    public async Task GetAllFlipcardSetsAsync_ReturnsAllFlipcardSets()
    {
        // Act
        var sets = await _repository.GetAllFlipcardSetsAsync();
        
        // Assert
        Assert.NotNull(sets);
        Assert.Equal(2, sets.Count); // Expecting 2 sets
        Assert.Contains(sets, s => s.Name == "set1");
        Assert.Contains(sets, s => s.Name == "set2");
        
        var set1 = sets.First(s => s.Id == 1);
        Assert.Equal(3, set1.FlipcardsList.Count);
        
        var flipcard1 = set1.FlipcardsList.FirstOrDefault(f => f.Id == 1);
        Assert.NotNull(flipcard1);
        Assert.Equal(FlipCardStateEnum.UNANSWERED, flipcard1.State._state);
        Assert.Equal("1", flipcard1.Question);
        Assert.Equal("1", flipcard1.Concept);
        Assert.Equal("1", flipcard1.Mnemonic);

        var flipcard2 = set1.FlipcardsList.FirstOrDefault(f => f.Id== 2);
        Assert.NotNull(flipcard2);
        Assert.Equal(FlipCardStateEnum.UNANSWERED, flipcard2.State._state);
        Assert.Equal("2", flipcard2.Question);
        Assert.Equal("2", flipcard2.Concept);
        Assert.Equal("2", flipcard2.Mnemonic);

        var flipcard3 = set1.FlipcardsList.FirstOrDefault(f => f.Id == 3);
        Assert.NotNull(flipcard3);
        Assert.Equal(FlipCardStateEnum.UNANSWERED, flipcard3.State._state);
        Assert.Equal("3", flipcard3.Question);
        Assert.Equal("3", flipcard3.Concept);
        Assert.Equal("3", flipcard3.Mnemonic);
        
        var set2 = sets.First(s => s.Id == 2);
        Assert.Equal(3, set2.FlipcardsList.Count);
        
        var flipcard11 = set2.FlipcardsList.FirstOrDefault(f => f.Id == 4);
        Assert.NotNull(flipcard11);
        Assert.Equal(FlipCardStateEnum.UNANSWERED, flipcard11.State._state);
        Assert.Equal("1", flipcard11.Question);
        Assert.Equal("1", flipcard11.Concept);
        Assert.Equal("1", flipcard11.Mnemonic);

        var flipcard22 = set2.FlipcardsList.FirstOrDefault(f => f.Id== 5);
        Assert.NotNull(flipcard22);
        Assert.Equal(FlipCardStateEnum.UNANSWERED, flipcard22.State._state);
        Assert.Equal("2", flipcard22.Question);
        Assert.Equal("2", flipcard22.Concept);
        Assert.Equal("2", flipcard22.Mnemonic);

        var flipcard33 = set2.FlipcardsList.FirstOrDefault(f => f.Id == 6);
        Assert.NotNull(flipcard33);
        Assert.Equal(FlipCardStateEnum.UNANSWERED, flipcard33.State._state);
        Assert.Equal("3", flipcard33.Question);
        Assert.Equal("3", flipcard33.Concept);
        Assert.Equal("3", flipcard33.Mnemonic);
        
    }

    [Fact]
    public async Task GetFlipcardSetByIdAsync_ReturnsCorrectFlipcardSet()
    {
        // Act
        var set = await _repository.GetFlipcardSetByIdAsync(1);

        // Assert
        Assert.NotNull(set);
        Assert.Equal("set1", set.Name); // Matching the seeded name
        Assert.Equal(3, set.FlipcardsList.Count);
    }

    public void Dispose()
    {
        _context.Dispose();
    }
}
