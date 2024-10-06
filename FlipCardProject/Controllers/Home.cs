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

            // Initialize the first FlipcardSet
            FlipcardSet test = new FlipcardSet("testas");
            Serialization s = new Serialization(test, "testas");
            s.LoadData();
            _CardSet.Add(test);

            // Initialize the second FlipcardSet
            FlipcardSet test2 = new FlipcardSet("test2");
            test2.AddFlipcard(new FlipcardState(), "question","blabla", "b");
            test2.AddFlipcard(new FlipcardState(), "question","whateverrr", "whtv");
            test2.AddFlipcard(new FlipcardState(),"question", "lets gooo", "last goat");
            _CardSet.Add(test2);
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
            _CardSet.Add(new FlipcardSet(newSet));

            
            return CreatedAtAction(nameof(GetSet), new { setName = newSet.SetName }, newSet);
        }

        
        
        
        [HttpPut]
        public ActionResult Put([FromBody] FlipcardSetDto updatedSet)
        {   
            
            var set = _CardSet.FirstOrDefault(f => f.SetName == updatedSet.SetName);
            if (set == null)
            {
                return NotFound();
            }

            set.SetName = updatedSet.SetName;
            set.FlipcardsList = updatedSet.FlipcardsList;
            return NoContent();
        }
        
        [HttpPut("{setName},{question},{concept},{mnemonic}", Name = "AddCard")]
        public ActionResult<FlipcardSet> Put(string setName, string question,string concept, string mnemonic)
        {
            var set = _CardSet.FirstOrDefault(f => f.SetName == setName);
            if (set == null)
            {
                return NotFound();
            }
            set.AddFlipcard(new FlipcardState(),question, concept, mnemonic);
            return NoContent();
        }
        
        [HttpDelete("{setName}")]
        public ActionResult Delete(string setName)
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
