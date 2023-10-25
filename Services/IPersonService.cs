using StaffingPortalBackend.DTO;
using StaffingPortalBackend.Models;

namespace StaffingPortalBackend.Services
{
    public interface IPersonService
    {
        Task<IEnumerable<PersonReadDto>> GetAllPersons();
        Task<PersonReadDto> GetPersonById(int id);
        Task<PersonReadDto> CreatePerson(Person person);
        Task UpdatePerson(PersonUpdateDto personDto);
        Task DeletePerson(int id);
    }
}
