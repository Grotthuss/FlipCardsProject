using System.Security.Cryptography.X509Certificates;
using FlipCardProject.Data;
using FlipCardProject.Models;
using Microsoft.EntityFrameworkCore;

namespace FlipCardProject.Services;

/*
public interface IFlipcardRepository
{
    Task<List<FlipcardSet>> GetAllFlipcardSetsAsync();
    Task<FlipcardSet> GetFlipcardSetByIdAsync(int setId);
    Task<FlipcardSet> AddFlipcardSetAsync(FlipcardSet flipcardSet);
    Task AddFlipcardAsync(int setId,Flipcard flipcard);
    Task<FlipcardSet> UpdateFlipcardSetAsync(FlipcardSet flipcardSet);
    Task UpdateFlipcardAsync(int setId, Flipcard flipcard);
    Task DeleteFlipcardSetAsync(int setId);
    Task DeleteFlipcardAsync(int setId, int cardId);
}
*/




public class FlipcardRepository //: IFlipcardRepository
{
    private readonly DataContext _context;

    public FlipcardRepository(DataContext context)
    {
        _context = context;
    }

    public async Task<List<FlipcardSet>> GetAllFlipcardSetsAsync()
    {   
        var t = await _context.Users.FindAsync(1);
         //return await _context.FlipCardSets.ToListAsync();
         return t.FlipcardSets.ToList();
    }

    public async Task<FlipcardSet> GetFlipcardSetByIdAsync(int setId)
    {
        var t = await _context.Users.FindAsync(1);
        
        //return await _context.FlipCardSets.FindAsync(setId);
        return t.FlipcardSets.FirstOrDefault(x => x.Id == setId);
    }

    public async Task<FlipcardSet> AddFlipcardSetAsync(FlipcardSet flipcardSet)
    {
        // Only start a transaction if the database provider supports it (InMemory database used for entity tests doesnt support transactions)
        var transaction = _context.Database.ProviderName != "Microsoft.EntityFrameworkCore.InMemory" 
            ? await _context.Database.BeginTransactionAsync()
            : null;
        
        try
        {
            var t = await _context.Users.FindAsync(1);
            if (t == null)
            {
                throw new NullReferenceException("User not found");
            }
            flipcardSet.Id = 0;
            foreach (var card in flipcardSet.FlipcardsList)
            {
                card.Id = 0;
                _context.Entry(card).State = EntityState.Added;
            }
            t.FlipcardSets.Add(flipcardSet);
            _context.Entry(flipcardSet).State = EntityState.Added;
            //await t.FlipCardSets.AddAsync(flipcardSet);
            await _context.SaveChangesAsync();
            if (transaction != null)
            {
                await transaction.CommitAsync();
            }
            return flipcardSet;
        }
        catch (Exception e)
        {
            // Only rollback if transaction is not null
            if (transaction != null)
            {
                await transaction.RollbackAsync();
            }
            throw;
        }
        
        
    }

    public async Task AddFlipcardAsync(int setId, Flipcard flipcard)
    {
        var transaction = _context.Database.ProviderName != "Microsoft.EntityFrameworkCore.InMemory" 
            ? await _context.Database.BeginTransactionAsync()
            : null;

        try
        {
            var t = await _context.Users.FindAsync(1);

            if (t == null)
            {
                throw new NullReferenceException("User not found");
            }

            var set = t.FlipcardSets.FirstOrDefault(x => x.Id == setId);//await _context.FlipCardSets.FindAsync(setId);
            if (set == null)
            {
                throw new NullReferenceException();
            }

            if (set.FlipcardsList.Contains(flipcard))
            {
                throw new NullReferenceException();
            }
            set.AddFlipcard(flipcard.State,flipcard.Question,flipcard.Concept,flipcard.Mnemonic);
            var t_card = set.FlipcardsList.Last();
            _context.Entry(t_card).State = EntityState.Added;
            await _context.SaveChangesAsync();
            if (transaction != null) await transaction.CommitAsync();
        }
        catch (Exception e)
        {
            if (transaction != null)
            {
                transaction.Rollback();
            }
            throw;
        }
    }

    public async Task<FlipcardSet> UpdateFlipcardSetAsync(FlipcardSet flipcardSet)
    {
        var transaction = _context.Database.ProviderName != "Microsoft.EntityFrameworkCore.InMemory" 
            ? await _context.Database.BeginTransactionAsync()
            : null;
        
        try
        {
            var t = await _context.Users.FindAsync(1);
            if (t == null)
            {
                throw new NullReferenceException("User not found");
            }

            var existingSet = t.FlipcardSets.FirstOrDefault(x => x.Id == flipcardSet.Id);//await _context.FlipCardSets.FindAsync(flipcardSet.Id);
            if (existingSet == null)
            {
                existingSet  = new FlipcardSet(flipcardSet.Name);

                foreach (var card in flipcardSet.FlipcardsList)
                {
                    existingSet.AddFlipcard(card.State,card.Question,card.Concept,card.Mnemonic);
                }
                //await _context.FlipCardSets.AddAsync(existingSet);
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
                    if (existingCard != null)
                    {

                        existingCard.Question = updatedCard.Question;
                        existingCard.Concept = updatedCard.Concept;
                        existingCard.Mnemonic = updatedCard.Mnemonic;
                        existingCard.State = updatedCard.State;
                    }
                    else
                    {
                        existingSet.AddFlipcard(updatedCard.State,updatedCard.Question,updatedCard.Concept,updatedCard.Mnemonic);
                        var card = existingSet.FlipcardsList.Last();
                        _context.Entry(card).State = EntityState.Added;
                    }
                }
                
            }

            await _context.SaveChangesAsync();
            if (transaction != null) await transaction.CommitAsync();
            return existingSet;
        }
        catch (Exception e)
        {
            if (transaction != null) transaction.Rollback();
            throw;
        }
    }

    public async Task UpdateFlipcardAsync(int setId, Flipcard flipcard)
    {
        var transaction = _context.Database.ProviderName != "Microsoft.EntityFrameworkCore.InMemory" 
            ? await _context.Database.BeginTransactionAsync()
            : null;

        try
        {
            var t = await _context.Users.FindAsync(1);
            if (t == null)
            {
                throw new NullReferenceException("User not found");
            }

            var set = t.FlipcardSets.FirstOrDefault(x => x.Id == setId);//await _context.FlipCardSets.FindAsync(setId);
            if (set == null)
            {
              throw new NullReferenceException();
            }

            var existingCard = set.FlipcardsList.FirstOrDefault(x => x.Id == flipcard.Id);
            if (existingCard == null)
            {
                set.AddFlipcard(flipcard.State, flipcard.Question, flipcard.Concept, flipcard.Mnemonic);
                var lastCard = set.FlipcardsList.Last();
                _context.Entry(lastCard).State = EntityState.Added;
            }
            else
            {
                existingCard.Question = flipcard.Question;
                existingCard.Concept = flipcard.Concept;
                existingCard.Mnemonic = flipcard.Mnemonic;
                existingCard.State = flipcard.State;   
            }
            await _context.SaveChangesAsync();
            if (transaction != null) await transaction.CommitAsync();

        }
        catch (Exception e)
        {
            if (transaction != null) transaction.Rollback();
            throw;
        }
    }
    public async Task DeleteFlipcardSetAsync(int setId)
    {
        var transaction = _context.Database.ProviderName != "Microsoft.EntityFrameworkCore.InMemory" 
            ? await _context.Database.BeginTransactionAsync()
            : null;

        try
        {   
            var t = await _context.Users.FindAsync(1);
            if (t == null)
            {
                throw new NullReferenceException("User not found");
            }

            var set = t.FlipcardSets.FirstOrDefault(x => x.Id == setId);//await _context.FlipCardSets.FindAsync(setId);
            if (set == null)
            {
                throw new NullReferenceException();
            }

            _context.Remove(set);
            await _context.SaveChangesAsync();
            if (transaction != null) await transaction.CommitAsync();

        }
        catch (Exception e)
        {
            if (transaction != null) transaction.Rollback();
            throw;
        }
    }

    public async Task DeleteFlipcardAsync(int setId, int cardId)
    {
        var transaction = _context.Database.ProviderName != "Microsoft.EntityFrameworkCore.InMemory" 
            ? await _context.Database.BeginTransactionAsync()
            : null;
        
        try
        {
            var t = await _context.Users.FindAsync(1);

            if (t == null)
            {
                throw new NullReferenceException("no user found");
            }

            var set = t.FlipcardSets.FirstOrDefault(x => x.Id == setId);//await _context.FlipCardSets.FindAsync(setId);
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
            if (transaction != null) await transaction.CommitAsync();
        }
        catch (Exception e)
        {
            if (transaction != null) transaction.Rollback();
            throw;
        }
    }
    
    
    
    
}