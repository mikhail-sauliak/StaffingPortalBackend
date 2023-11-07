using Microsoft.EntityFrameworkCore;
using AutoMapper;
using StaffingPortalBackend.DTO;
using StaffingPortalBackend.Models;

namespace StaffingPortalBackend.Services
{
    public class AdministrationService : IAdministrationService
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;

        public AdministrationService(AppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<IEnumerable<PersonAdminDto>> GetPeopleAsync()
        {
            var people = await _context.People.ToListAsync();
            return _mapper.Map<IEnumerable<PersonAdminDto>>(people);
        }

        public async Task<PersonReadDto> CreatePersonAsync(PersonCreateDto personCreateDto)
        {
            var personEntity = _mapper.Map<Person>(personCreateDto);
            await _context.People.AddAsync(personEntity);
            await _context.SaveChangesAsync();
            return _mapper.Map<PersonReadDto>(personEntity);
        }

        public async Task UpdatePersonAsync(int id, PersonUpdateDto personUpdateDto)
        {
            var personEntity = await _context.People.FindAsync(id);
            if (personEntity == null)
                throw new Exception("Person not found");

            _mapper.Map(personUpdateDto, personEntity);
            await _context.SaveChangesAsync();
        }

        public async Task DeletePersonAsync(int id)
        {
            var personEntity = await _context.People.FindAsync(id);
            if (personEntity == null)
                throw new Exception("Person not found");

            _context.People.Remove(personEntity);
            await _context.SaveChangesAsync();
        }
    }
}
