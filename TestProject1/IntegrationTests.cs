using FlipCardProject.Exceptions;
using FlipCardProject.Helpers;

namespace TestProject1;
using FlipCardProject.Data;

using FlipCardProject.Models;

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
            Id = 0,
            Email = "jonas@gmail.com",
            Salt = "hash",
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
        var nullSet = await _repository.GetAllFlipcardSetsAsync(2);
        Assert.Null(nullSet);
        
        var sets = await _repository.GetAllFlipcardSetsAsync(1);
        
        Assert.NotNull(sets);
        Assert.Equal(2, sets.Count);
        Assert.Contains(sets, s => s.Name == "set1");
        Assert.Contains(sets, s => s.Name == "set2");
        
        var set1 = sets.First(s => s.Id == 1);
        Assert.Equal(3, set1.FlipcardsList.Count);
        
        var flipcard1 = set1.FlipcardsList.FirstOrDefault(f => f.Id == 1);
        Assert.NotNull(flipcard1);
       
        Assert.Equal("1", flipcard1.Question);
        Assert.Equal("1", flipcard1.Concept);
        Assert.Equal("1", flipcard1.Mnemonic);

        var flipcard2 = set1.FlipcardsList.FirstOrDefault(f => f.Id== 2);
        Assert.NotNull(flipcard2);
        
        Assert.Equal("2", flipcard2.Question);
        Assert.Equal("2", flipcard2.Concept);
        Assert.Equal("2", flipcard2.Mnemonic);

        var flipcard3 = set1.FlipcardsList.FirstOrDefault(f => f.Id == 3);
        Assert.NotNull(flipcard3);
       
        Assert.Equal("3", flipcard3.Question);
        Assert.Equal("3", flipcard3.Concept);
        Assert.Equal("3", flipcard3.Mnemonic);
        
        var set2 = sets.First(s => s.Id == 2);
        Assert.Equal(3, set2.FlipcardsList.Count);
        
        var flipcard11 = set2.FlipcardsList.FirstOrDefault(f => f.Id == 4);
        Assert.NotNull(flipcard11);
       
        Assert.Equal("1", flipcard11.Question);
        Assert.Equal("1", flipcard11.Concept);
        Assert.Equal("1", flipcard11.Mnemonic);

        var flipcard22 = set2.FlipcardsList.FirstOrDefault(f => f.Id== 5);
        Assert.NotNull(flipcard22);
        
        Assert.Equal("2", flipcard22.Question);
        Assert.Equal("2", flipcard22.Concept);
        Assert.Equal("2", flipcard22.Mnemonic);

        var flipcard33 = set2.FlipcardsList.FirstOrDefault(f => f.Id == 6);
        Assert.NotNull(flipcard33);
        Assert.Equal("3", flipcard33.Question);
        Assert.Equal("3", flipcard33.Concept);
        Assert.Equal("3", flipcard33.Mnemonic);
        
    }

    [Fact]
    public async Task GetFlipcardSetByIdAsync_ReturnsCorrectFlipcardSet()
    {
        var nullSet = await _repository.GetFlipcardSetByIdAsync(2, 2);
        Assert.Null(nullSet);
        var nullSet1 = await _repository.GetFlipcardSetByIdAsync(1, 100);
        Assert.Null(nullSet1);
        var set = await _repository.GetFlipcardSetByIdAsync(1,1);

        Assert.NotNull(set);
        Assert.Equal("set1", set.Name);
        Assert.Equal(3, set.FlipcardsList.Count);
        
        var flipcard1 = set.FlipcardsList.FirstOrDefault(f => f.Id == 1);
        Assert.NotNull(flipcard1);
        
        Assert.Equal("1", flipcard1.Question);
        Assert.Equal("1", flipcard1.Concept);
        Assert.Equal("1", flipcard1.Mnemonic);

        var flipcard2 = set.FlipcardsList.FirstOrDefault(f => f.Id== 2);
        Assert.NotNull(flipcard2);
        
        Assert.Equal("2", flipcard2.Question);
        Assert.Equal("2", flipcard2.Concept);
        Assert.Equal("2", flipcard2.Mnemonic);

        var flipcard3 = set.FlipcardsList.FirstOrDefault(f => f.Id == 3);
        Assert.NotNull(flipcard3);
        
        Assert.Equal("3", flipcard3.Question);
        Assert.Equal("3", flipcard3.Concept);
        Assert.Equal("3", flipcard3.Mnemonic);
        
        set = await _repository.GetFlipcardSetByIdAsync(1,2);
        
        Assert.NotNull(set);
        Assert.Equal("set2", set.Name);
        Assert.Equal(3, set.FlipcardsList.Count);
        
        flipcard1 = set.FlipcardsList.FirstOrDefault(f => f.Id == 4);
        Assert.NotNull(flipcard1);
        
        Assert.Equal("1", flipcard1.Question);
        Assert.Equal("1", flipcard1.Concept);
        Assert.Equal("1", flipcard1.Mnemonic);

        flipcard2 = set.FlipcardsList.FirstOrDefault(f => f.Id== 5);
        Assert.NotNull(flipcard2);
       
        Assert.Equal("2", flipcard2.Question);
        Assert.Equal("2", flipcard2.Concept);
        Assert.Equal("2", flipcard2.Mnemonic);

        flipcard3 = set.FlipcardsList.FirstOrDefault(f => f.Id == 6);
        Assert.NotNull(flipcard3);
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
            flipcardSet.UserId = 100;
            
            var exception = await Assert.ThrowsAsync<UserNotFound>(async () =>
            {
                await _repository.AddFlipcardSetAsync(flipcardSet);
            });
            flipcardSet.UserId = 1;
            Assert.Equal("User not found.", exception.Message);
            
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
            
            var exception = await Assert.ThrowsAsync<UserNotFound>(async () =>
            {
                await _repository.AddFlipcardAsync(100,2, card);;
            });
            Assert.Equal("User not found.", exception.Message);
            
            var exception1 = await Assert.ThrowsAsync<NullReferenceException>(async () =>
            {
                await _repository.AddFlipcardAsync(1,100, card);;
            });
            
            
            
            
            await _repository.AddFlipcardAsync(1,2, card);
            
            var exception2 = await Assert.ThrowsAsync<NullReferenceException>(async () =>
            {
                await _repository.AddFlipcardAsync(1,2, card);;
            });
            
            
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
            
            var exception = await Assert.ThrowsAsync<UserNotFound>(async () =>
            {
                await _repository.AddFlipcardAsync(100,2, card);;
            });
            Assert.Equal("User not found.", exception.Message); 
            
            Assert.ThrowsAsync<NullReferenceException>(async () =>
            {
                await _repository.AddFlipcardAsync(1,100, card);;
            });
            
            await _repository.UpdateFlipcardAsync(1,2, card);
            
            var set = await _repository.GetFlipcardSetByIdAsync(1,2);
            var updatedCard = set.FlipcardsList.FirstOrDefault(x => x.Id == 7);
            int length = set.FlipcardsList.Count;
            Assert.Equal(card.Question, updatedCard.Question);
            Assert.Equal(card.Concept, updatedCard.Concept);
            Assert.Equal(card.Mnemonic, updatedCard.Mnemonic);
            
            var card1 = new Flipcard("4","4","4");
            card1.Id = 8;
            
            await _repository.UpdateFlipcardAsync(1,2, card1);
            var set1 = await _repository.GetFlipcardSetByIdAsync(1,2);
            Assert.Equal(length + 1, set1.FlipcardsList.Count);
            var card2 = new Flipcard("5","5","5");
            card2.Id = 8;
            await _repository.UpdateFlipcardAsync(1,2, card2);
            var set2 = await _repository.GetFlipcardSetByIdAsync(1,2);
            var updatedCard1 = set.FlipcardsList.FirstOrDefault(x => x.Id == 8);
            
            Assert.Equal(card2.Question, updatedCard1.Question);
            Assert.Equal(card2.Concept, updatedCard1.Concept);
            Assert.Equal(card2.Mnemonic, updatedCard1.Mnemonic);
            
        }

        [Fact]
        public async Task UpdateFlipcardSetAsync_UpdatesSetCorrectly()
        {
            var flipcardSet = new FlipcardSet("set3");
            flipcardSet.AddFlipcard("1","1","1");
            flipcardSet.Id = 2;
            flipcardSet.UserId = 100;
            
            var exception = await Assert.ThrowsAsync<UserNotFound>(async () =>
            {
                await _repository.UpdateFlipcardSetAsync(flipcardSet);
            });
            Assert.Equal("User not found.", exception.Message);
            
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
            
            var exception = await Assert.ThrowsAsync<UserNotFound>(async () =>
            {
                await _repository.DeleteFlipcardSetAsync(100,2);
            });
            
            Assert.Equal("User not found.", exception.Message);
            
            
             await Assert.ThrowsAsync<NullReferenceException>(async () =>
            {
                await _repository.DeleteFlipcardSetAsync(1,200);
            });
            
            await _repository.DeleteFlipcardSetAsync(1,2);
            
            int SetCountAfterDeletion = (await _repository.GetAllFlipcardSetsAsync(1)).Count();
            
            Assert.Equal(SetCountBeforeDeletion, SetCountAfterDeletion + 1);
        }
        
        [Fact]
        public async Task DeleteFlipcardAsync_DeletesCorrectCard()
        {
            var exception = await Assert.ThrowsAsync<UserNotFound>(async () =>
            {
                await _repository.DeleteFlipcardAsync(100,2, 4);
            });
            
            Assert.Equal("User not found.", exception.Message);
            
            
            await Assert.ThrowsAsync<NullReferenceException>(async () =>
            {
                await _repository.DeleteFlipcardAsync(1,200, 4);
            });
            
            await Assert.ThrowsAsync<NullReferenceException>(async () =>
            {
                await _repository.DeleteFlipcardAsync(1,2, 400);
            });
            
            int CardCountBeforeDeletion = (await _repository.GetFlipcardSetByIdAsync(1,2)).FlipcardsList.Count();
            
            await _repository.DeleteFlipcardAsync(1,2, 4);

            var affectedSet = await _repository.GetFlipcardSetByIdAsync(1,2);
            
            int CardCountAfterDeletion = affectedSet.FlipcardsList.Count();
            Assert.Equal(CardCountAfterDeletion, CardCountBeforeDeletion -1);
            
            var deletedCard = affectedSet.FlipcardsList.FirstOrDefault(c => c.Id == 4);
            Assert.Null(deletedCard);
        }

        [Fact]
        public async Task RegisterUserAsync_RegistersUserCorrectly()
        {
            
            var user = new User
            {
                Id = 0,
                Email = "test@gmail.com",
                Password = "password",
                FlipcardSets = new List<FlipcardSet>()
            };

            
            await _repository.CreateAccount(user.Email, user.Password);
    
            var userFromDb = _context.Users.FirstOrDefault(x => x.Email == user.Email);

            
            Assert.NotNull(userFromDb);
            Assert.NotEqual(user.Password, userFromDb.Password);

            
            Assert.False(string.IsNullOrEmpty(userFromDb.Salt));
            
            byte[] storedSalt = Convert.FromBase64String(userFromDb.Salt);
            string expectedHashedPassword = Hashers.HashPassword(user.Password, storedSalt);

            Assert.Equal(expectedHashedPassword, userFromDb.Password);
            
            
            var rezult = await _repository.CreateAccount(user.Email, user.Password);
            
            Assert.Null(rezult);
        }

        [Fact]
        public async Task LoginAsync_LoginsUserCorrectly()
        {
            var password = "correctPassword";
            var salt = Hashers.GenerateSalt();
            var hashedPassword = Hashers.HashPassword(password, salt);
            var saltString = Convert.ToBase64String(salt);

            var existingUser = new User
            {
                Id = 0,
                Email = "user@example.com",
                Password = hashedPassword,
                Salt = saltString,
                FlipcardSets = new List<FlipcardSet>()
            };

            _context.Users.Add(existingUser);
            await _context.SaveChangesAsync();

            
            var result = await _repository.LoginUser(existingUser.Email, password);
            Assert.Equal(2, result);
            
            result = await _repository.LoginUser(existingUser.Email, "1231");
            Assert.Equal(0, result);
            
            result = await _repository.LoginUser("kitas@gmail.com", password);
            Assert.Equal(0, result);
        }

        [Fact]
        public async Task DeleteUserAsync_DeletesUserCorrectly()
        {
            var existingUser = new User
            {
                Id = 0,
                Email = "user@example.com",
                Password = "hashedpassword",
                Salt = "storedsalt",
                FlipcardSets = new List<FlipcardSet>()
            };

            _context.Users.Add(existingUser);
            await _context.SaveChangesAsync();

            
            await _repository.DeleteUser(existingUser.Id); 
            var deletedUser = await _context.Users.FirstOrDefaultAsync(u => u.Id == existingUser.Id);
            Assert.Null(deletedUser);
            
            var nonExistingUserId = 999;

            
            await _repository.DeleteUser(nonExistingUserId);
            var nonExistingUser = await _context.Users.FirstOrDefaultAsync(u => u.Id == nonExistingUserId);
            Assert.Null(nonExistingUser);
        }
        
        

    public void Dispose()
    {
        _context.Dispose();
    }
}
