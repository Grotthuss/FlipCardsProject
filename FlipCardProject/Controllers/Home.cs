using System.Text.Json;
using FlipCardProject.Models;
using FlipCardProject.Records;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using FlipCardProject.Data;

using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
namespace FlipCardProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class Home : ControllerBase
    {
        private static List<FlipcardSet> _CardSet;

        static Home()
        {
            _CardSet = new List<FlipcardSet>();
            Serialization s = new Serialization();
            s.LoadData(_CardSet);
        }
        
        
        // GET: api/<Home>
        [HttpGet]
        public IActionResult Get()
        {
            return Content(JsonConvert.SerializeObject(_CardSet), "application/json");
        }
        
        
        [HttpGet("{setName}", Name = "GetCardSet")]
        public ActionResult<FlipcardSet> GetSet(string setName)
        {
            var set = _CardSet.FirstOrDefault(f => f.SetName == setName);
            if (set == null)
            {
                return NotFound();
            }
            return Content(JsonConvert.SerializeObject(set),"application/json");
        }
        
        
        [HttpPost]
        public ActionResult<FlipcardSet> Post([FromBody] FlipcardSetDto newSet)
        {
            
            var set = _CardSet.FirstOrDefault(f => f.SetName == newSet.SetName);
            if (set == null)
            {
                newSet.FlipcardsList.Sort((x,y) => x.Id.CompareTo(y.Id));
                for (int i = 0; i < newSet.FlipcardsList.Count; i++)
                {
                    Flipcard t = newSet.FlipcardsList[i];
                    t.Id = i + 1;
                    newSet.FlipcardsList[i] = t;
                }
                
                _CardSet.Add(new FlipcardSet(newSet));
                
            }
            else
            {
                return Conflict();
            }
            
            
            return CreatedAtAction(nameof(GetSet), new { setName = newSet.SetName }, newSet);
        }


        [HttpPost("{setName}", Name = "PostSet")]
        public ActionResult<FlipcardSet> PostSet(string setName)
        {
            var set = _CardSet.FirstOrDefault(f => f.SetName == setName);

            if (set == null)
            {
                _CardSet.Add(new FlipcardSet(setName));
                return Ok();
            }
            else
            {
                return Conflict();
            }
            
        }

        [HttpPost("{setName}/CreateCard", Name = "PostCard")]
        public ActionResult<FlipcardSet> PostCard(string setName, [FromBody] Flipcard card)
        {
            var set = _CardSet.FirstOrDefault(f => f.SetName == setName);
            if (set == null)
            {
                return Conflict();
            }
            else
            {
                if (!set.FlipcardsList.Contains(card))
                {
                    set.AddFlipcard(card.State,card.Question,card.Concept,card.Mnemonic);
                    
                }

                return Ok();
            }
        }
        
        
        
        [HttpPut]
        public ActionResult<FlipcardSet> PutWholeSet([FromBody] FlipcardSetDto updatedSet)
        {   
            
            updatedSet.FlipcardsList.Sort((x,y) => x.Id.CompareTo(y.Id));
            for (int i = 0; i < updatedSet.FlipcardsList.Count; i++)
            {
                Flipcard t = updatedSet.FlipcardsList[i];
                t.Id = i + 1;
                updatedSet.FlipcardsList[i] = t;
            }
            
            var set = _CardSet.FirstOrDefault(f => f.SetName == updatedSet.SetName);
            if (set == null)
            {
                FlipcardSet newSet = new FlipcardSet(updatedSet);
                _CardSet.Add(newSet);   
            }
            else
            {
                set.FlipcardsList = updatedSet.FlipcardsList;
            }

            return Ok();

        }
        
        [HttpPut("{setName}", Name = "UpdateOrAddCard")]
        public ActionResult<FlipcardSet> PutCard(string setName, [FromBody] Flipcard updatedCard)
        {   
            
            var set = _CardSet.FirstOrDefault(f => f.SetName == setName);
            if (set == null)
            {
                return NotFound();
                
            }

            var card = set.FlipcardsList.FirstOrDefault(f => f.Id == updatedCard.Id);
            if (card.Equals(default(Flipcard)))
            {
                set.AddFlipcard(updatedCard.State,updatedCard.Question,updatedCard.Concept,updatedCard.Mnemonic);
            }
            else
            {
                set.FlipcardsList[updatedCard.Id - 1 ] = updatedCard;
                
            }
            return NoContent();
            
        }
        
        [HttpDelete("{setName}")]
        public ActionResult DeleteSet(string setName)
        {
            var set = _CardSet.FirstOrDefault(f => f.SetName == setName);
            if (set == null)
            {
                return NotFound();
            }

            _CardSet.Remove(set);
            return NoContent();
        }

        [HttpDelete("{setName}/{Id}", Name = "DeleteCard")]
        public ActionResult<FlipcardSet> DeleteCard(string setName,int Id)
        {
            var set = _CardSet.FirstOrDefault(f => f.SetName == setName);
            if (set == null)
            {
                return NotFound();
            }

           
            int t = set.FlipcardsList.Count;
            if (t >= Id)
            {
                for (int i = Id; i < t ; i++)
                {
                    Flipcard t1 = set.FlipcardsList[i];
                    t1.Id--;
                    set.FlipcardsList[i] = t1;
                }
                set.FlipcardsList.Remove(set.FlipcardsList[Id - 1]);
                return NoContent();
            }
            return NotFound();
            
        }
        
    }
}