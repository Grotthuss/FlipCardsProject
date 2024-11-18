using System.Text.Json;
using FlipCardProject.Models;
using FlipCardProject.Records;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using FlipCardProject.Data;
using FlipCardProject.Extensions;
using FlipCardProject.Services;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
namespace FlipCardProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class Home : ControllerBase
    {
        private readonly UserTrackingService _userTrackingService;
        private readonly FlipcardRepository _flipcardRepository;
        public Home(UserTrackingService userTrackingService,FlipcardRepository flipcardRepository)
        {
            _flipcardRepository = flipcardRepository;
            _userTrackingService = userTrackingService;
        }

        [HttpPost("{UserID}/StartGame")]
        public IActionResult StartGame(int UserID)
        {
            _userTrackingService.AddPlayer(UserID);
            return Ok();
        }

        [HttpPost("{UserId}/EndGame")]
        public IActionResult EndGame(int UserID)
        {
            _userTrackingService.RemovePlayer(UserID);
            return Ok();
        }

        [HttpGet("ActivePlayerCount")]
        public ActionResult<int> GetCurrentPlayerCount()
        {
            var count = _userTrackingService.GetCurrentPlayerCount();
            return Ok(count);
        }

        [HttpGet("ActivePlayers")]
        public ActionResult<List<int>> GetActivePlayers()
        {
            var players = _userTrackingService.GetActivePlayers();
            return Ok(players);
        }
        [HttpGet("GetAllSets")]
        public async Task<ActionResult<IEnumerable<FlipcardSet>>> GetCardSets()
        {
           
            return Ok(await _flipcardRepository.GetAllFlipcardSetsAsync());
        }
        

        [HttpGet("{setId}/GetCardSet")]
        public async Task<ActionResult<IEnumerable<FlipcardSet>>> GetSet(int setId)
        {
            
            var set = await _flipcardRepository.GetFlipcardSetByIdAsync(setId);
            if (set == null)
            {
                return NotFound();
            }
            
            return Ok(set);
        }
        
        
        
        
        [HttpGet("{setId}/CardsOfAnotherState")]
        public async Task<ActionResult<IEnumerable<FlipcardSet>>> GetCardsOfSomeState(int setId, [FromQuery]  FlipcardState state)
        {
            var set = await _flipcardRepository.GetFlipcardSetByIdAsync(setId);
            if (set == null)
            {
                return NotFound();
            }
            
            var cards = set.FlipcardsList.FindAll(x => x.State == state);
            return Ok(cards);
        }
        

        [HttpGet("{setId}/ShuffleCards")]

        public async Task<ActionResult<List<Flipcard>>> GetShuffleCards(int setId)
        {
           
            var set = await _flipcardRepository.GetFlipcardSetByIdAsync(setId);
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
            
            try
            {   
                var set = await _flipcardRepository.AddFlipcardSetAsync(newSet);
                return Ok(set);
            }
            catch (Exception e)
            {
                return StatusCode(500, "An error occurred while creating the set.");
                
            }
        }

        
        
        

        [HttpPost("{setId}/CreateCard", Name = "PostCard")]
        public async Task<ActionResult<Flipcard>> PostCard(int setId, [FromBody] Flipcard card)
        {
           

            try
            {
                
                await _flipcardRepository.AddFlipcardAsync(setId, card);
                return Ok();
                
            }
            catch (Exception e)
            {
               
                return StatusCode(500, "An error occurred while creating the card.");
            }
            
            
        }
        

        [HttpPut("UpdateSet")]
        public async Task<ActionResult<FlipcardSet>> PutSet([FromBody] FlipcardSet set)
        {
            

            try
            {
               
                return Ok(await _flipcardRepository.UpdateFlipcardSetAsync(set));
            }
            catch (Exception e)
            {
                
                return StatusCode(500, "An error occurred while updating the set.");
            }
        }
        

        [HttpPut("{setId}/UpdateorAddCard", Name = "PutCard")]
        public async Task<ActionResult<Flipcard>> PutCard(int setId,[FromBody] Flipcard card)
        {
            

            try
            {
                
                await _flipcardRepository.UpdateFlipcardAsync(setId,card);
                return Ok();
            }
            catch (Exception e)
            {
                
                return StatusCode(500, "An error occurred while updating the set.");
            }
            
        }
        
        [HttpDelete("{setId}/DeleteSet")]
        public async Task<ActionResult<FlipcardSet>> DeleteSet(int setId)
        {
           

            try
            {
                await _flipcardRepository.DeleteFlipcardSetAsync(setId);
                return Ok();


            }
            catch (Exception e)
            {
                
                return StatusCode(500, "An error occurred while deleting the set.");
            }
        }
        
        
        
        [HttpDelete("{setId}/{Id}/DeleteCard", Name = "DeleteCard")]
        public async Task<ActionResult<FlipcardSet>> DeleteCard(int setId, int Id)
        {
            

            try
            {
                
                await _flipcardRepository.DeleteFlipcardAsync(setId, Id);
                return Ok();
            }
            catch (Exception e)
            {
                
                return StatusCode(500, "An error occurred while deleting the card.");
            }
            
        }
        
        [HttpPost("LogError")]
        public IActionResult LogError([FromBody] ErrorLog log)
        {
            string logFilePath = Path.Combine(Directory.GetCurrentDirectory(), "logs", "errorLog.txt");
        
            Directory.CreateDirectory(Path.GetDirectoryName(logFilePath));
        
            System.IO.File.AppendAllText(logFilePath, $"{DateTime.Now}: {log.Message}\n");
        
            return Ok();
        }
        
        public class ErrorLog
        {
            public string Message { get; set; }
        }
        
    }
}