using Microsoft.EntityFrameworkCore;
using AutoMapper;
using StaffingPortalBackend.DTO;
using StaffingPortalBackend.Models;

namespace StaffingPortalBackend.Services
{
    public class PersonService : IPersonService
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;

        public PersonService(AppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<IEnumerable<PersonReadDto>> GetAllPersons()
        {
            var persons = await _context.People.ToListAsync();
            return _mapper.Map<IEnumerable<PersonReadDto>>(persons);
        }

        public async Task<IEnumerable<PersonReadDto>> GetAllActivePersons()
        {
            var persons = await _context.People.Where(p => !p.IsDismissed).ToListAsync();
            return _mapper.Map<IEnumerable<PersonReadDto>>(persons);
        }

        public async Task<PersonReadDto> GetPersonById(int id)
        {
            var person = await _context.People.FindAsync(id);
            return _mapper.Map<PersonReadDto>(person);
        }

        public async Task<PersonReadDto> CreatePerson(Person person)
        {
            _context.People.Add(person);
            await _context.SaveChangesAsync();
            return _mapper.Map<PersonReadDto>(person);
        }

        public async Task UpdatePerson(PersonUpdateDto personDto)
        {
            var person = await _context.People.FindAsync(personDto.Id);
            _mapper.Map(personDto, person);
            await _context.SaveChangesAsync();
        }

        public async Task DeletePerson(int id)
        {
            var person = await _context.People.FindAsync(id);
            person.IsDismissed = true;
            await _context.SaveChangesAsync();
        }
    }
}
