using StaffingPortalBackend.DTO;
using StaffingPortalBackend.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace StaffingPortalBackend.Services
{
    public interface IPersonService
    {
        Task<IEnumerable<PersonReadDto>> GetAllPersons();
        Task<PersonReadDto> GetPersonById(int id);
        Task<ServiceResponse<PersonReadDto>> CreatePerson(Person person);
        Task UpdatePerson(PersonUpdateDto personDto);
        Task DeletePerson(int id);
        Task<IEnumerable<PersonReadDto>> GetAllActivePersons();
        Task DismissPerson(int id, DateTime? endDate);
        Task<ServiceResponse<PersonReadDto>> UpdatePersonLevel(int personId, string newLevel, string justification);
    Task<IEnumerable<TitleHistoryDto>?> GetPersonTitleHistory(int personId);
        Task<ServiceResponse<TitleHistoryDto>> CreateTitleHistory(int personId, TitleHistoryDto titleHistoryDto);
        Task<ServiceResponse<TitleHistoryDto>> UpdateTitleHistory(int personId, int titleHistoryId, TitleHistoryDto titleHistoryDto);
        Task<ServiceResponse<bool>> DeleteTitleHistory(int personId, int titleHistoryId);
    }
}
