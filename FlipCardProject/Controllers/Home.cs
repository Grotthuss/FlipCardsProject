using FlipCardProject.Models;
using FlipCardProject.Records;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using FlipCardProject.Data;
namespace FlipCardProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class Home : ControllerBase
    {
        // GET: api/<Home>
        [HttpGet]
        public IActionResult Get()
        {
            FlipcardSet test = new FlipcardSet("testas");
            /*
            test.AddFlipcard(new FlipcardState(), "Red Orange Yellow Green Blue Indigo Violet", "Richard Of York Gave Battle In Vain");
            test.AddFlipcard(new FlipcardState(), "Mercury Venus Earth Mars Jupiter Saturn Uranus Neptune", "My Very Educated Mother Just Served Us Noodles");
            test.AddFlipcard(new FlipcardState(), "Domain Kingdom Phylum Class Order Family Genus Species", "Dear King Philip Came Over For Good Soup");
            test.AddFlipcard(new FlipcardState(),"parentheses, exponents, multiplication, division, addition, subtraction","Please Excuse My Dear Aunt Sally");
            test.AddFlipcard(new FlipcardState(),"Black(0), Brown(1), Red(2), Orange(3), Yellow(4), Green(5), Blue(6), Violet(7), Gray(8), White(9), Gold(5%), Silver(10%), None(20%)","Big brown rabbits often yield great big vocal groans when gingerly slapped");
            */
            
            Serialization s = new Serialization(test,"testas");
            ///s.saveData(test);
            s.LoadData();
            return Ok(test.FlipcardsList);
            
            
        }
        
    }
}
