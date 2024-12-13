
using FlipCardProject.Data;
using FlipCardProject.Exceptions;
using FlipCardProject.logs;

namespace FlipCardProject.Services;
using Microsoft.EntityFrameworkCore;

using FlipCardProject.Models;
using FlipCardProject.Helpers;

public class FlipcardRepository
{
    private readonly DataContext _context;

    public FlipcardRepository(DataContext context)
    {
        _context = context;
    }


    public async Task<User> CreateAccount(string email, string password)
    {
        if (await _context.Users.AnyAsync(u => u.Email == email))
        {
            return null;
        }

        byte[] salt  = Hashers.GenerateSalt();
        string hashedPassword = Hashers.HashPassword(password, salt );

        string saltString = Convert.ToBase64String(salt);

        User user = new User
        {   
            Id = 0,
            Email = email,
            Password = hashedPassword,
            Salt = saltString, //PERVADINTI I SALT LENETELEJE
            FlipcardSets = new List<FlipcardSet>()
        };
        await _context.Users.AddAsync(user);
        await _context.SaveChangesAsync();
        return user;
    }

    public async Task<int> LoginUser(string email, string password)
    {
        User user = await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
        if (user == null)
        {
            return 0;
        }

        if (!(user.Password == Hashers.HashPassword(password, Convert.FromBase64String(user.Salt)))) //IRGI NAME PERVADINTI I SALT
        {
            return 0;
        }
        return user.Id;
    }

    public async Task DeleteUser(int userId)
    {
        User user = await _context.Users.FirstOrDefaultAsync(u => u.Id == userId);

        if (user == null)
        {
            
        }
        else
        {
            _context.Users.Remove(user);
            await _context.SaveChangesAsync();
            
        }
    }
    
    public async Task<List<FlipcardSet>> GetAllFlipcardSetsAsync(int userId)
    {   
        var t = await _context.Users.FindAsync(userId);
        
         return t.FlipcardSets.ToList();
    }

    public async Task<FlipcardSet> GetFlipcardSetByIdAsync(int userId,int setId)
    {
        var t = await _context.Users.FindAsync(userId);
        if (t == null)
        {
            return null;
        }
     
        return t.FlipcardSets.FirstOrDefault(x => x.Id == setId);
    }

    public async Task<FlipcardSet> AddFlipcardSetAsync(FlipcardSet flipcardSet)
    {
        
        
        try
        {
            var t = await _context.Users.FindAsync(flipcardSet.UserId);
            if (t == null)
            {
                
                var exception = new UserNotFound("User not found.");
                ErrorLogger.LogError(exception);
                throw exception;
            }
            flipcardSet.Id = 0;
            foreach (var card in flipcardSet.FlipcardsList)
            {
                card.Id = 0;
                _context.Entry(card).State = EntityState.Added;
            }
            t.FlipcardSets.Add(flipcardSet);
            _context.Entry(flipcardSet).State = EntityState.Added;
           
            await _context.SaveChangesAsync();
            
           
            return flipcardSet;
        }
        catch (Exception e)
        {
           
           
            throw e;
        }
        
        
    }

    public async Task AddFlipcardAsync(int userId,int setId, Flipcard flipcard)
    {
        try
        {
            var t = await _context.Users.FindAsync(userId);

            if (t == null)
            {
                var exception = new UserNotFound("User not found.");
                ErrorLogger.LogError(exception);
                throw exception;
               
            }

            var set = t.FlipcardSets.FirstOrDefault(x => x.Id == setId);
            if (set == null)
            {
                throw new NullReferenceException();
            }

            if (set.FlipcardsList.Contains(flipcard))
            {
                throw new NullReferenceException();
            }
            set.AddFlipcard(flipcard.Question,flipcard.Concept,flipcard.Mnemonic);
            var t_card = set.FlipcardsList.Last();
            _context.Entry(t_card).State = EntityState.Added;
            await _context.SaveChangesAsync();
           
        }
        catch (Exception e)
        {
            
            throw e;
        }
    }

    public async Task<FlipcardSet> UpdateFlipcardSetAsync(FlipcardSet flipcardSet)
    {
       
        
        try
        {
            var t = await _context.Users.FindAsync(flipcardSet.UserId);
            if (t == null)
            {
                var exception = new UserNotFound("User not found.");
                ErrorLogger.LogError(exception);
                throw exception;
               
            }

            var existingSet = t.FlipcardSets.FirstOrDefault(x => x.Id == flipcardSet.Id);
            if (existingSet == null)
            {
                existingSet  = new FlipcardSet(flipcardSet.Name);
                existingSet.UserId = flipcardSet.UserId;
                foreach (var card in flipcardSet.FlipcardsList)
                {
                    
                    existingSet.AddFlipcard(card.Question,card.Concept,card.Mnemonic);
                    var t1 = existingSet.FlipcardsList.Last();
                    _context.Entry(t1).State = EntityState.Added;
                }
                
                
                t.FlipcardSets.Add(existingSet);
                _context.Entry(existingSet).State = EntityState.Added;
            }
            else
            {
                existingSet.Name = flipcardSet.Name;


                var updatedCardIds = flipcardSet.FlipcardsList.Select(x => x.Id).ToList();
                var cardsToRemove = existingSet.FlipcardsList.Where(x => !updatedCardIds.Contains(x.Id)).ToList();
                _context.RemoveRange(cardsToRemove);

                foreach (var updatedCard in flipcardSet.FlipcardsList)
                {
                    var existingCard = existingSet.FlipcardsList.FirstOrDefault(c => c.Id == updatedCard.Id);
                    if ((existingCard != null) && (updatedCard.Id != 0) )
                    {

                        existingCard.Question = updatedCard.Question;
                        existingCard.Concept = updatedCard.Concept;
                        existingCard.Mnemonic = updatedCard.Mnemonic;
                        
                    }
                    else
                    {
                        existingSet.AddFlipcard(updatedCard.Question,updatedCard.Concept,updatedCard.Mnemonic);
                        var card = existingSet.FlipcardsList.Last();
                        _context.Entry(card).State = EntityState.Added;
                    }
                }
                
            }

            await _context.SaveChangesAsync();
           
            return existingSet;
        }
        catch (Exception e)
        {
          
            throw e;
        }
    }

    public async Task UpdateFlipcardAsync(int userId,int setId, Flipcard flipcard)
    {
        

        try
        {
            var t = await _context.Users.FindAsync(userId);
            if (t == null)
            {
                var exception = new UserNotFound("User not found.");
                ErrorLogger.LogError(exception);
                throw exception;
             
            }

            var set = t.FlipcardSets.FirstOrDefault(x => x.Id == setId);
            if (set == null)
            {
              throw new NullReferenceException();
            }

            var existingCard = set.FlipcardsList.FirstOrDefault(x => x.Id == flipcard.Id);
            if (existingCard == null)
            {
                set.AddFlipcard( flipcard.Question, flipcard.Concept, flipcard.Mnemonic);
                var lastCard = set.FlipcardsList.Last();
                _context.Entry(lastCard).State = EntityState.Added;
            }
            else
            {
                existingCard.Question = flipcard.Question;
                existingCard.Concept = flipcard.Concept;
                existingCard.Mnemonic = flipcard.Mnemonic;
               
            }
            await _context.SaveChangesAsync();
           

        }
        catch (Exception e)
        {
           
            throw e;
        }
    }
    public async Task DeleteFlipcardSetAsync(int userID,int setId)
    {
      

        try
        {   
            var t = await _context.Users.FindAsync(userID);
            if (t == null)
            {
                var exception = new UserNotFound("User not found.");
                ErrorLogger.LogError(exception);
                throw exception;
                
            }

            var set = t.FlipcardSets.FirstOrDefault(x => x.Id == setId);
            if (set == null)
            {
                throw new NullReferenceException();
            }

            _context.Remove(set);
            await _context.SaveChangesAsync();
          

        }
        catch (Exception e)
        {
          
            throw e;
        }
    }

    public async Task DeleteFlipcardAsync(int userId,int setId, int cardId)
    {
      
        
        try
        {
            var t = await _context.Users.FindAsync(userId);

            if (t == null)
            {
                var exception = new UserNotFound("User not found.");
                ErrorLogger.LogError(exception);
                throw exception;
               
            }

            var set = t.FlipcardSets.FirstOrDefault(x => x.Id == setId);
            if (set == null)
            {
                throw new NullReferenceException();
            }
            var card = set.FlipcardsList.FirstOrDefault(c => c.Id == cardId);
        
            if (card == null)
            {
                throw new NullReferenceException();
            }
            _context.Remove(card);
            await _context.SaveChangesAsync();
            
        }
        catch (Exception e)
        {
          
            throw e;
        }
    }
    
    
    
    
}