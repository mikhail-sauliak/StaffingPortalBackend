using Microsoft.EntityFrameworkCore;
using AutoMapper;
using StaffingPortalBackend.DTO;
using StaffingPortalBackend.Models;
using System.Linq;
using System.Collections.Generic;

namespace StaffingPortalBackend.Services
{
    public class AdministrationService : IAdministrationService
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;
        private readonly IManagementHierarchyService _hierarchyService;

        public AdministrationService(AppDbContext context, IMapper mapper, IManagementHierarchyService hierarchyService)
        {
            _context = context;
            _mapper = mapper;
            _hierarchyService = hierarchyService;
        }

        public async Task<IEnumerable<PersonAdminDto>> GetPeopleAsync()
        {
            var people = await _context.People
                .Include(p => p.User)
                .Include(p => p.DivisionManager)
                .Include(p => p.TalentManager)
                .Include(p => p.Head)
                .Include(p => p.DeputyHead)
                .ToListAsync();
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
                throw new KeyNotFoundException("Person not found");

            _mapper.Map(personUpdateDto, personEntity);
            await _context.SaveChangesAsync();
        }

        public async Task DeletePersonAsync(int id)
        {
            var personEntity = await _context.People.FindAsync(id);
            if (personEntity == null)
                throw new KeyNotFoundException("Person not found");

            _context.People.Remove(personEntity);
            await _context.SaveChangesAsync();
        }

        public async Task DismissPersonAsync(int id, PersonDismissDto dismissDto)
        {
            var personEntity = await _context.People.FindAsync(id);
            if (personEntity == null)
            {
                throw new KeyNotFoundException("Person not found");
            }

            personEntity.IsDismissed = true;
            personEntity.EndDate = dismissDto?.EndDate;
            await _context.SaveChangesAsync();
        }

        public async Task<PersonAdminDto> GetPersonAdminDetailsAsync(int personId)
        {
            var person = await _context.People
                .Include(p => p.User)
                .Include(p => p.DivisionManager)
                .Include(p => p.TalentManager)
                .Include(p => p.Head)
                .Include(p => p.DeputyHead)
                .SingleOrDefaultAsync(p => p.Id == personId);

            if (person == null)
                throw new KeyNotFoundException("Person not found");

            var personAdminDto = _mapper.Map<PersonAdminDto>(person);
            return personAdminDto;
        }

        public async Task<IEnumerable<PersonAdminDto>> GetSubordinatesAsync(int managerId)
        {
            var manager = await _context.People.FindAsync(managerId);
            if (manager == null)
                throw new KeyNotFoundException("Manager not found");

            var subordinateIds = await _hierarchyService.GetSubordinatePersonIdsAsync(managerId, includeManager: false);

            if (!subordinateIds.Any())
            {
                return Enumerable.Empty<PersonAdminDto>();
            }

            var subordinates = await _context.People
                .Include(p => p.User)
                .Include(p => p.DivisionManager)
                .Include(p => p.TalentManager)
                .Include(p => p.Head)
                .Include(p => p.DeputyHead)
                .Where(p => subordinateIds.Contains(p.Id))
                .ToListAsync();

            return _mapper.Map<IEnumerable<PersonAdminDto>>(subordinates);
        }

        public async Task<IEnumerable<PersonAdminDto>> GetPeopleByStreamAsync(string stream)
        {
            var people = await _context.People
                .Include(p => p.User)
                .Include(p => p.DivisionManager)
                .Include(p => p.TalentManager)
                .Include(p => p.Head)
                .Include(p => p.DeputyHead)
                .Where(p => p.Stream == stream)
                .ToListAsync();

            return _mapper.Map<IEnumerable<PersonAdminDto>>(people);
        }

        public async Task<IEnumerable<UserReadDto>> GetDivisionManagersAsync()
        {
            var divisionManagers = await _context.Users
                .Where(u => u.Role == "DivisionManager" || u.Role == "Head" || u.Role == "DeputyHead")
                .Include(u => u.Person)
                    .ThenInclude(p => p.DivisionManager)
                .Include(u => u.Person)
                    .ThenInclude(p => p.TalentManager)
                .ToListAsync();

            return _mapper.Map<IEnumerable<UserReadDto>>(divisionManagers);
        }

        public async Task<IEnumerable<UserReadDto>> GetTalentManagersByDivisionManagerAsync(int divisionManagerId)
        {
            Console.WriteLine($"divisionManagerId = {divisionManagerId} ");
            
            var talentManagers = await _context.Users
                .Include(u => u.Person)
                    .ThenInclude(p => p.DivisionManager)
                .Include(u => u.Person)
                    .ThenInclude(p => p.TalentManager)
                .Where(u => u.Role == "TalentManager" && u.Person != null && u.Person.DivisionManagerId == divisionManagerId)
                .ToListAsync();

            Console.WriteLine($"Number of talent managers found after mapping: {talentManagers.Count}");

            return _mapper.Map<IEnumerable<UserReadDto>>(talentManagers);
        }


        public async Task<IEnumerable<UserReadDto>> GetAllTalentManagersAsync()
        {
            var talentManagers = await _context.Users
                .Where(u => u.Role == "TalentManager")
                .Include(u => u.Person)
                    .ThenInclude(p => p.DivisionManager)
                .Include(u => u.Person)
                    .ThenInclude(p => p.TalentManager)
                .ToListAsync();

            return _mapper.Map<IEnumerable<UserReadDto>>(talentManagers);
        }
    }
}
