using System.Text.Json;
using FlipCardProject.Models;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using FlipCardProject.Data;
using FlipCardProject.Extensions;
using FlipCardProject.Services;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using FlipCardProject.Helpers;
namespace FlipCardProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class Home : ControllerBase
    {
        private readonly GenericValidator<FlipcardSet> _genericValidator;
        private readonly UserTrackingService<int> _userTrackingService;
        private readonly FlipcardRepository _flipcardRepository;
        public Home(UserTrackingService<int> userTrackingService,FlipcardRepository flipcardRepository,GenericValidator<FlipcardSet> genericValidator)
        {
            _flipcardRepository = flipcardRepository;
            _userTrackingService = userTrackingService;
            _genericValidator = genericValidator;
        }


        [HttpPost("Login")]
        public async Task<ActionResult<int>> Login([FromBody] LoginModel loginModel)
        {
            var result = await _flipcardRepository.LoginUser(loginModel.Email, loginModel.Password);
            if (result == 0)
            {
                return NotFound();
            }
            return Ok(result);
        }

        [HttpPost("Register")]
        public async Task<ActionResult<User>> Register([FromBody] RegisterModel registerModel)
        {
            var result = await _flipcardRepository.CreateAccount(registerModel.Email, registerModel.Password);
            if (result == null)
            {
                return BadRequest();
            }
            return Ok(result);
        }

        [HttpPost("DeleteUser")]
        public async Task DeleteUser([FromBody] DeleteUserModel deleteUserModel)
        {
            await _flipcardRepository.DeleteUser(deleteUserModel.UserId);
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
        [HttpGet("{userId}/GetAllSets")]
        public async Task<ActionResult<IEnumerable<FlipcardSet>>> GetCardSets(int userId)
        {
           
            return Ok(await _flipcardRepository.GetAllFlipcardSetsAsync(userId));
        }
        

        [HttpGet("{userId}/{setId}/GetCardSet")]
        public async Task<ActionResult<IEnumerable<FlipcardSet>>> GetSet(int userId,int setId)
        {
            
            var set = await _flipcardRepository.GetFlipcardSetByIdAsync(userId,setId);
            if (set == null)
            {
                return NotFound();
            }
            
            return Ok(set);
        }
        
        [HttpGet("{userId}/{setId}/ShuffleCards")]

        public async Task<ActionResult<List<Flipcard>>> GetShuffleCards(int userId,int setId)
        {
           
            var set = await _flipcardRepository.GetFlipcardSetByIdAsync(userId,setId);
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

        
        
        

        [HttpPost("{userId}/{setId}/CreateCard", Name = "PostCard")]
        public async Task<ActionResult<Flipcard>> PostCard(int userId,int setId, [FromBody] Flipcard card)
        {
           

            try
            {
                
                await _flipcardRepository.AddFlipcardAsync(userId,setId, card);
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
        

        [HttpPut("{userId}/{setId}/UpdateorAddCard", Name = "PutCard")]
        public async Task<ActionResult<Flipcard>> PutCard(int userId,int setId,[FromBody] Flipcard card)
        {
            

            try
            {
                
                await _flipcardRepository.UpdateFlipcardAsync(userId,setId,card);
                return Ok();
            }
            catch (Exception e)
            {
                
                return StatusCode(500, "An error occurred while updating the set.");
            }
            
        }
        
        [HttpDelete("{userId}/{setId}/DeleteSet")]
        public async Task<ActionResult<FlipcardSet>> DeleteSet(int userId,int setId)
        {
           

            try
            {
                await _flipcardRepository.DeleteFlipcardSetAsync(userId,setId);
                return Ok();


            }
            catch (Exception e)
            {
                
                return StatusCode(500, "An error occurred while deleting the set.");
            }
        }
        
        
        
        [HttpDelete("{userId},{setId}/{Id}/DeleteCard", Name = "DeleteCard")]
        public async Task<ActionResult<FlipcardSet>> DeleteCard(int userId,int setId, int Id)
        {
            

            try
            {
                
                await _flipcardRepository.DeleteFlipcardAsync(userId,setId, Id);
                return Ok();
            }
            catch (Exception e)
            {
                
                return StatusCode(500, "An error occurred while deleting the card.");
            }
            
        }
        
        [HttpPost("ValidateSet")]
        public IActionResult ValidateSet([FromBody] FlipcardSet flipcardSet)
        {
            if (flipcardSet == null)
            {
                return BadRequest("Flipcard set cannot be null.");
            }

            
            _genericValidator.AddRule(set => !string.IsNullOrWhiteSpace(set.Name));

            if (!_genericValidator.Validate(flipcardSet, out var errors))
            {
                Console.WriteLine($"Validation failed: {string.Join(", ", errors)}");
                return BadRequest(string.Join("\n", errors));
            }

            Console.WriteLine("Validation succeeded for Flipcard Set.");
            return Ok("Flipcard Set is valid.");
        }
        
    }
}