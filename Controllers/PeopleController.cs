using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StaffingPortalBackend.Models;
using StaffingPortalBackend.DTO;
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
        [HttpGet]
        public async Task<ActionResult<IEnumerable<PersonReadDto>>> GetPersons()
        {
            var people = await _context.People.ToListAsync();
            var peopleDtos = people.Select(p => new PersonReadDto
            {
                Id = p.Id,
                FirstName = p.FirstName,
                LastName = p.LastName,
                Location = p.Location,
                DivisionManager = p.DivisionManager,
                ResourceManager = p.ResourceManager,
                AvailableFrom = p.AvailableFrom,
                Comments = p.Comments
                // Установите другие свойства
            }).ToList();
            return peopleDtos;
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
        public async Task<ActionResult<PersonReadDto>> CreatePerson(PersonCreateDto personCreateDto)
        {
            var person = new Person
            {
                FirstName = personCreateDto.FirstName,
                LastName = personCreateDto.LastName,
                Location = personCreateDto.Location,
                DivisionManager = personCreateDto.DivisionManager,
                ResourceManager = personCreateDto.TalentManager,
                AvailableFrom = personCreateDto.AvailableFrom,
                Comments = personCreateDto.Comments
                // Заполните другие свойства
            };
            _context.People.Add(person);
            await _context.SaveChangesAsync();

            var personReadDto = new PersonReadDto
            {
                Id = person.Id,
                FirstName = person.FirstName,
                LastName = person.LastName,
                Location = person.Location,
                DivisionManager = person.DivisionManager,
                ResourceManager = person.ResourceManager,
                AvailableFrom = person.AvailableFrom,
                Comments = person.Comments
            };
            return CreatedAtAction(nameof(GetPerson), new { id = person.Id }, personReadDto);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdatePerson(int id, PersonUpdateDto personUpdateDto)
        {
            var existingPerson = await _context.People
                .Include(p => p.ProjectCandidates)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (existingPerson == null)
            {
                return NotFound();
            }

            existingPerson.FirstName = personUpdateDto.FirstName;
            existingPerson.LastName = personUpdateDto.LastName;
            existingPerson.Location = personUpdateDto.Location;
            existingPerson.DivisionManager = personUpdateDto.DivisionManager;
            existingPerson.ResourceManager = personUpdateDto.ResourceManager;
            existingPerson.AvailableFrom = personUpdateDto.AvailableFrom;
            existingPerson.Comments = personUpdateDto.Comments;

            // Если вы хотите обновлять связанные сущности
            if (personUpdateDto.ProjectCandidateIds != null)
            {
                // Удалите существующие связи и добавьте новые
                _context.ProjectCandidates.RemoveRange(existingPerson.ProjectCandidates);
                foreach (var projectId in personUpdateDto.ProjectCandidateIds)
                {
                    existingPerson.ProjectCandidates.Add(new ProjectCandidate { PersonId = id, ProjectId = projectId });
                }
            }

            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePerson(int id)
        {
            var person = await _context.People.FindAsync(id);

            if (person == null)
            {
                return NotFound();
            }

            _context.People.Remove(person);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }    
}
