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

        public async Task<PersonAdminDto> GetPersonAdminDetailsAsync(int personId)
        {
            var person = await _context.People
                .Include(p => p.DivisionManager)
                .Include(p => p.TalentManager)
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

            var subordinates = await _context.People
                .Where(p => p.DivisionManagerId == managerId || p.TalentManagerId == managerId)
                .ToListAsync();

            return _mapper.Map<IEnumerable<PersonAdminDto>>(subordinates);
        }

        public async Task<IEnumerable<PersonAdminDto>> GetPeopleByStreamAsync(string stream)
        {
            var people = await _context.People
                .Where(p => p.Stream == stream)
                .ToListAsync();

            return _mapper.Map<IEnumerable<PersonAdminDto>>(people);
        }

        public async Task<IEnumerable<UserReadDto>> GetDivisionManagersAsync()
        {
            var divisionManagers = await _context.Users
                .Where(u => u.Role == "DivisionManager" || u.Role == "Head" || u.Role == "DeputyHead")
                .ToListAsync();

            return _mapper.Map<IEnumerable<UserReadDto>>(divisionManagers);
        }

        public async Task<IEnumerable<UserReadDto>> GetTalentManagersByDivisionManagerAsync(int divisionManagerId)
        {
            Console.WriteLine($"divisionManagerId = {divisionManagerId} ");
            // var divisionManager = await _context.Users.FindAsync(divisionManagerId);
            // if (divisionManager != null && 
            //     (divisionManager.Role == "Head" || divisionManager.Role == "DeputyHead" || divisionManager.Role == "DivisionManager"))
            // {
            //     // Возвращаем Head как талент менеджера
            //     var head = await _context.Users.FirstOrDefaultAsync(u => u.Role == "Head");
            //     return head != null ? new List<UserReadDto> { _mapper.Map<UserReadDto>(head) } : new List<UserReadDto>();
            // }
            var talentManagersQuery = _context.Users
                .Where(u => u.Role == "TalentManager")
                .Join(_context.People,
                    user => user.Id,
                    person => person.UserId,
                    (user, person) => new { User = user, Person = person })
                .Where(up => up.Person.DivisionManagerId == divisionManagerId);

            // Debugging: Fetch the query results before mapping
            var queryResult = await talentManagersQuery.ToListAsync();
            Console.WriteLine($"Query fetched {queryResult.Count} results before mapping.");

            // Debugging: Output the raw data to the console
            foreach (var item in queryResult)
            {
                Console.WriteLine($"User ID: {item.User.Id}, Person ID: {item.Person.Id}, DivisionManagerId: {item.Person.DivisionManagerId}");
            }

            // Execute the query and project the result to a list of UserReadDto
            var talentManagers = queryResult
                .Select(up => _mapper.Map<UserReadDto>(up.User))
                .ToList();

            Console.WriteLine($"Number of talent managers found after mapping: {talentManagers.Count}");

            return talentManagers;
        }


        public async Task<IEnumerable<UserReadDto>> GetAllTalentManagersAsync()
        {
            var talentManagers = await _context.Users
                .Where(u => u.Role == "TalentManager")
                .ToListAsync();

            return _mapper.Map<IEnumerable<UserReadDto>>(talentManagers);
        }
    }
}
