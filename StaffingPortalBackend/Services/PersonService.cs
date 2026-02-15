using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
            var currentDate = DateTime.Today;

            var persons = await _context.People
                .Where(p => !p.IsDismissed && (p.EndDate == null || p.EndDate >= currentDate))
                .ToListAsync();

            return _mapper.Map<IEnumerable<PersonReadDto>>(persons);
        }

        public async Task<PersonReadDto> GetPersonById(int id)
        {
            var person = await _context.People.FindAsync(id);
            return _mapper.Map<PersonReadDto>(person);
        }

        public async Task<ServiceResponse<PersonReadDto>> CreatePerson(Person person)
        {
            var response = new ServiceResponse<PersonReadDto>();

            var existingPerson = await _context.People
                .FirstOrDefaultAsync(p => p.FirstName == person.FirstName && p.LastName == person.LastName);

            if (existingPerson != null)
            {
                response.Success = false;
                response.Message = $"User with name {person.FirstName} {person.LastName} already exists.";
                return response;
            }

            _context.People.Add(person);
            await _context.SaveChangesAsync();

            response.Data = _mapper.Map<PersonReadDto>(person);
            response.Message = "User created successfully";

            return response;
        }

        public async Task UpdatePerson(PersonUpdateDto personDto)
        {
            var person = await _context.People.FindAsync(personDto.Id);
            if (person == null)
            {
                throw new Exception("Person not found");
            }
            _mapper.Map(personDto, person);
            if (personDto.UserId.HasValue)
            {
                person.UserId = personDto.UserId.Value;
            }
            await _context.SaveChangesAsync();
        }

        public async Task DeletePerson(int id)
        {
            var person = await _context.People.FindAsync(id);
            if (person == null)
            {
                throw new Exception("Person not found");
            }
            person.IsDismissed = true;
            await _context.SaveChangesAsync();
        }

        public async Task DismissPerson(int id, DateTime? endDate)
        {
            var person = await _context.People.FindAsync(id);
            if (person != null)
            {
                person.IsDismissed = true;
                person.EndDate = endDate;
                await _context.SaveChangesAsync();
            }
        }

        public async Task<ServiceResponse<PersonReadDto>> UpdatePersonLevel(int personId, string newLevel, string justification)
        {
            var response = new ServiceResponse<PersonReadDto>();
            var person = await _context.People.FindAsync(personId);

            if (person == null)
            {
                response.Success = false;
                response.Message = "Person not found";
                return response;
            }

            var oldTitle = person.Level;
            person.Level = newLevel;

            var titleHistory = new TitleHistory
            {
                PersonId = personId,
                OldTitle = oldTitle,
                NewTitle = newLevel,
                DateChanged = DateTime.UtcNow,
                Justification = justification
            };

            _context.TitleHistories.Add(titleHistory);
            await _context.SaveChangesAsync();

            response.Data = _mapper.Map<PersonReadDto>(person);
            response.Message = "Person level updated successfully";

            return response;
        }

        public async Task<IEnumerable<TitleHistoryDto>?> GetPersonTitleHistory(int personId)
        {
            var titleHistories = await _context.TitleHistories
                .Where(th => th.PersonId == personId)
                .OrderByDescending(th => th.DateChanged)
                .ToListAsync();

            if (titleHistories.Any())
            {
                return _mapper.Map<IEnumerable<TitleHistoryDto>>(titleHistories);
            }

            var personProjection = await _context.People
                .AsNoTracking()
                .Where(p => p.Id == personId)
                .Select(p => new
                {
                    p.Id,
                    p.Level,
                    p.AvailableFrom
                })
                .FirstOrDefaultAsync();

            if (personProjection == null)
            {
                return null;
            }

            var currentLevel = personProjection.Level ?? string.Empty;
            var effectiveDate = personProjection.AvailableFrom == default
                ? DateTime.UtcNow
                : DateTime.SpecifyKind(personProjection.AvailableFrom, DateTimeKind.Utc);

            var fallbackEntry = new TitleHistoryDto
            {
                Id = 0,
                PersonId = personProjection.Id,
                OldTitle = currentLevel,
                NewTitle = currentLevel,
                DateChanged = effectiveDate,
                Justification = "Current title"
            };

            return new[] { fallbackEntry };
        }

        public async Task<ServiceResponse<TitleHistoryDto>> CreateTitleHistory(int personId, TitleHistoryDto titleHistoryDto)
        {
            var response = new ServiceResponse<TitleHistoryDto>();

            var person = await _context.People.FindAsync(personId);
            if (person == null)
            {
                response.Success = false;
                response.Message = "Person not found";
                return response;
            }

            var titleHistory = _mapper.Map<TitleHistory>(titleHistoryDto);
            titleHistory.PersonId = personId;
            titleHistory.DateChanged = DateTime.UtcNow;

            _context.TitleHistories.Add(titleHistory);
            await _context.SaveChangesAsync();

            response.Data = _mapper.Map<TitleHistoryDto>(titleHistory);
            response.Message = "Title history created successfully";

            return response;
        }

        public async Task<ServiceResponse<TitleHistoryDto>> UpdateTitleHistory(int personId, int titleHistoryId, TitleHistoryDto titleHistoryDto)
        {
            var response = new ServiceResponse<TitleHistoryDto>();

            var titleHistory = await _context.TitleHistories.FindAsync(titleHistoryId);
            if (titleHistory == null || titleHistory.PersonId != personId)
            {
                response.Success = false;
                response.Message = "Title history not found or does not belong to the specified person";
                return response;
            }

            _mapper.Map(titleHistoryDto, titleHistory);
            await _context.SaveChangesAsync();

            response.Data = _mapper.Map<TitleHistoryDto>(titleHistory);
            response.Message = "Title history updated successfully";

            return response;
        }

        public async Task<ServiceResponse<bool>> DeleteTitleHistory(int personId, int titleHistoryId)
        {
            var response = new ServiceResponse<bool>();

            var titleHistory = await _context.TitleHistories.FindAsync(titleHistoryId);
            if (titleHistory == null || titleHistory.PersonId != personId)
            {
                response.Success = false;
                response.Message = "Title history not found or does not belong to the specified person";
                return response;
            }

            _context.TitleHistories.Remove(titleHistory);
            await _context.SaveChangesAsync();

            response.Data = true;
            response.Message = "Title history deleted successfully";

            return response;
        }
    }
}
