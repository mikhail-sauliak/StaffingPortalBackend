using Microsoft.AspNetCore.Mvc;
using StaffingPortalBackend.DTO;
using StaffingPortalBackend.Services;
using StaffingPortalBackend.Models;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;

namespace StaffingPortalBackend.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class PeopleController : ControllerBase
    {
        private readonly IPersonService _personService;
        private readonly IMapper _mapper;

        public PeopleController(IPersonService personService, IMapper mapper)
        {
            _personService = personService;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<PersonReadDto>>> GetPersons()
        {
            try
            {
                var persons = await _personService.GetAllPersons();
                return Ok(persons);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error" + ex.Message);
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<PersonReadDto>> GetPerson(int id)
        {
            try
            {
                var person = await _personService.GetPersonById(id);
                if (person == null)
                {
                    return NotFound();
                }
                return Ok(_mapper.Map<PersonReadDto>(person));
            }
            catch
            {
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPost]
        public async Task<ActionResult<PersonReadDto>> CreatePerson([FromBody] PersonCreateDto personCreateDto)
        {
            if (personCreateDto == null)
            {
                return BadRequest("Person object is null");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var personEntity = _mapper.Map<Person>(personCreateDto);
                var createdPerson = await _personService.CreatePerson(personEntity);
                return CreatedAtAction(nameof(GetPerson), new { id = createdPerson.Id }, _mapper.Map<PersonReadDto>(createdPerson));
            }
            catch
            {
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdatePerson(int id, [FromBody] PersonUpdateDto personUpdateDto)
        {
            if (personUpdateDto == null)
            {
                return BadRequest("Person object is null");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var existingPerson = await _personService.GetPersonById(id);
                if (existingPerson == null)
                {
                    return NotFound();
                }

                await _personService.UpdatePerson(personUpdateDto);
                return NoContent();
            }
            catch
            {
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePerson(int id)
        {
            try
            {
                var person = await _personService.GetPersonById(id);
                if (person == null)
                {
                    return NotFound();
                }

                await _personService.DeletePerson(id);

                return NoContent();
            }
            catch
            {
                return StatusCode(500, "Internal server error");
            }
        }
    }
}
