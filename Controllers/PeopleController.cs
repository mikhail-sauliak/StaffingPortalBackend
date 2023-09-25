using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StaffingPortalBackend.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StaffingPortalBackend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PeopleController : ControllerBase
    {
        private readonly AppDbContext _context;

        public PeopleController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Person>>> GetPersons()
        {
            return await _context.People.ToListAsync();  // Используем People, а не Persons
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Person>> GetPerson(int id)  // Изменено с Project на Person
        {
            var person = await _context.People.FindAsync(id);  // Изменено с Projects на People

            if (person == null)
            {
                return NotFound();
            }

            return person;
        }

        [HttpPost]
        public async Task<ActionResult<Person>> CreatePerson(Person person)  // Изменено с Project на Person
        {
            _context.People.Add(person);  // Изменено с Projects на People
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetPerson), new { id = person.Id }, person);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdatePerson(int id, Person person)  // Изменено с Project на Person
        {
            if (id != person.Id)
            {
                return BadRequest();
            }

            _context.Entry(person).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePerson(int id)
        {
            var person = await _context.People.FindAsync(id);  // Изменено с Projects на People

            if (person == null)
            {
                return NotFound();
            }

            _context.People.Remove(person);  // Изменено с Projects на People
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }    
}
