using System.Text.Json;
using FlipCardProject.Models;
using FlipCardProject.Records;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using FlipCardProject.Data;
using FlipCardProject.Extensions;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
namespace FlipCardProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class Home : ControllerBase
    {
        private static List<FlipcardSet> _CardSet;
        private readonly DataContext _context;
        public Home(DataContext context)
        {
            _CardSet = new List<FlipcardSet>();
            Serialization s = new Serialization();
            s.LoadData(_CardSet);
            _context = context;
        }

        [HttpGet("GetAllSets")]
        public async Task<ActionResult<IEnumerable<FlipcardSet>>> GetCardSets()
        {
            var sets = await _context.sets.ToListAsync();
            return Ok(sets);
            
            
        }
        

        [HttpGet("{setName}/GetCardSet")]
        public async Task<ActionResult<IEnumerable<FlipcardSet>>> GetSet(string setName)
        {
            var set = await _context.sets.FindAsync(setName);
            if (set == null)
            {
                return NotFound();
            }
            
            return Ok(set);
        }
        
        
        
        
        [HttpGet("{setName}/CardsOfAnotherState")]
        public async Task<ActionResult<IEnumerable<FlipcardSet>>> GetCardsOfSomeState(string setName, [FromQuery]  FlipcardState state)
        {
            var set = await _context.sets.FindAsync(setName);
            if (set == null)
            {
                return NotFound();
            }
            
            var cards = set.FlipcardsList.FindAll(x => x.State == state);
            return Ok(cards);
        }
        

        [HttpGet("{setName}/ShuffleCards")]

        public async Task<ActionResult<List<Flipcard>>> GetShuffleCards(string setName)
        {
            var set = await _context.sets.FindAsync(setName);
            if (set == null)
            {
                return NotFound();
            }
            set.CardShuffle();
            return Ok(set);
            
        }
        
        
        
        [HttpPost("CreateFullSet")]
        
        public async Task<ActionResult<FlipcardSet>> PostAsync([FromBody] FlipcardSet newSet)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();

            try
            {
               
                if (_context.sets.Any(s => s.SetName == newSet.SetName))
                {
                    return Conflict("A set with this name already exists.");
                }

               
                _context.Add(newSet);
                await _context.SaveChangesAsync();
        
                await transaction.CommitAsync();

                return Ok(await GetSet(newSet.SetName));
            }
            catch (Exception)
            {
                await transaction.RollbackAsync();
                return StatusCode(500, "An error occurred while creating the set.");
            }
            return Ok();
        }

        [HttpPost("{setName}/CreateEmptySet", Name = "PostSet")]

        public async Task<ActionResult<FlipcardSet>> PostSet(string setName)
        {
            var set = await _context.sets.FindAsync(setName);
            if (!(set == null))
            {
                return Conflict("A set with this name already exists.");
            }
            
            using var transaction = await _context.Database.BeginTransactionAsync();

            try
            {
                await _context.AddAsync(new FlipcardSet { SetName = setName });
                await _context.SaveChangesAsync();
                await transaction.CommitAsync();
                
            }
            catch (Exception e)
            {
                
                await transaction.RollbackAsync();
                return StatusCode(500, "An error occurred while creating the set.");
                
            }
            return Ok(await GetSet(setName));
        }
        
        

        [HttpPost("{setName}/CreateCard", Name = "PostCard")]
        public async Task<ActionResult<Flipcard>> PostCard(string setName, [FromBody] Flipcard card)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();

            try
            {
                var set = await _context.sets.FindAsync(setName);
                if (set == null)
                {
                    return Conflict("A set with this name was not found.");
                }

                if (set.FlipcardsList.Contains(card))
                {
                    return Conflict("A card like this already exists.");
                }
                set.AddFlipcard(card.State,card.Question,card.Concept,card.Mnemonic);
                
                var card_t = set.FlipcardsList.Last();
                _context.Entry(card_t).State = EntityState.Added;
                
                await _context.SaveChangesAsync();
                await transaction.CommitAsync();
                
                
            }
            catch (Exception e)
            {
                await transaction.RollbackAsync();
                return StatusCode(500, "An error occurred while creating the card.");
            }
            return Ok(await GetSet(setName));
            
        }
        

        [HttpPut("UpdateSet")]
        public async Task<ActionResult<FlipcardSet>> PutSet([FromBody] FlipcardSet set)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();

            try
            {
                var existingSet = await _context.sets.FindAsync(set.SetName);
                if (existingSet == null)
                {
                    ///return Conflict("A set with this name doesnt exist.");
                    existingSet = new FlipcardSet { SetName = set.SetName };
                    _context.Entry(existingSet).State = EntityState.Added;
                }
                
                var updatedCardIds = set.FlipcardsList.Select(x => x.Id).ToList();
                var cardsToRemove = existingSet.FlipcardsList.Where(x => !updatedCardIds.Contains(x.Id)).ToList();
                _context.RemoveRange(cardsToRemove);

                foreach (var updatedCard in set.FlipcardsList)
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

                
                await _context.SaveChangesAsync();
                
                await transaction.CommitAsync();

                return NoContent(); 

            }
            catch (Exception e)
            {
                await transaction.RollbackAsync();
                return StatusCode(500, "An error occurred while updating the set.");
            }
        }
        

        [HttpPut("{setName}/UpdateorAddCard", Name = "PutCard")]
        public async Task<ActionResult<Flipcard>> PutCard(string setName,[FromBody] Flipcard card)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();

            try
            {
                var set = await _context.sets.FindAsync(setName);
                if (set == null)
                {
                    return NotFound("A set with this name was not found.");
                }

                var existingCard = set.FlipcardsList.FirstOrDefault(x => x.Id == card.Id);
                if (existingCard == null)
                {
                    set.AddFlipcard(card.State, card.Question, card.Concept, card.Mnemonic);
                    var lastCard = set.FlipcardsList.Last();
                    _context.Entry(lastCard).State = EntityState.Added;
                }
                else
                {
                    existingCard.Question = card.Question;
                    existingCard.Concept = card.Concept;
                    existingCard.Mnemonic = card.Mnemonic;
                    existingCard.State = card.State;   
                }

                await _context.SaveChangesAsync();
                await transaction.CommitAsync();
            }
            catch (Exception e)
            {
                await transaction.RollbackAsync();
                return StatusCode(500, "An error occurred while updating the set.");
            }
            return Ok(await GetSet(setName));
        }
        
        [HttpDelete("{setName}/DeleteSet")]
        public async Task<ActionResult<FlipcardSet>> DeleteSet(string setName)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();

            try
            {
                
                var set = await _context.sets.FindAsync(setName);
                if (set == null)
                {
                    return NotFound("A set with this name was not found.");
                }

                _context.sets.Remove(set);
                await _context.SaveChangesAsync();
                await transaction.CommitAsync();
                return Ok($"Set with name {setName} has been deleted.");

            }
            catch (Exception e)
            {
                transaction.Rollback();
                return StatusCode(500, "An error occurred while deleting the set.");
            }
        }
        
        
        
        [HttpDelete("{setName}/{Id}/DeleteCard", Name = "DeleteCard")]
        public async Task<ActionResult<FlipcardSet>> DeleteCard(string setName, int Id)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();

            try
            {
                
                var set = await _context.sets.FindAsync(setName);
                if (set == null)
                {
                    return NotFound("A set with this name was not found.");
                }
                
                var card = set.FlipcardsList.FirstOrDefault(c => c.Id == Id);
        
                if (card == null)
                {
                    return NotFound("The specified card was not found in this set.");
                }
                _context.Remove(card);
                await _context.SaveChangesAsync();
                await transaction.CommitAsync();
                return Ok($"Card with ID {Id} has been deleted from set {setName}.");
            }
            catch (Exception e)
            {
                transaction.Rollback();
                return StatusCode(500, "An error occurred while deleting the card.");
            }
            
        }
        
    }
}