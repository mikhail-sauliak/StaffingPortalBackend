using StaffingPortalBackend.DTO;

namespace StaffingPortalBackend.Services
{
    public interface IAdministrationService
    {
        Task<IEnumerable<PersonAdminDto>> GetPeopleAsync();
        Task<PersonReadDto> CreatePersonAsync(PersonCreateDto personCreateDto);
        Task UpdatePersonAsync(int id, PersonUpdateDto personUpdateDto);
        Task DeletePersonAsync(int id);
    }
}