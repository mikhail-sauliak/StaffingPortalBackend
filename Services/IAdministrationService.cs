using StaffingPortalBackend.DTO;

namespace StaffingPortalBackend.Services
{
    public interface IAdministrationService
    {
        Task<IEnumerable<PersonAdminDto>> GetPeopleAsync();
        Task<PersonReadDto> CreatePersonAsync(PersonCreateDto personCreateDto);
        Task UpdatePersonAsync(int id, PersonUpdateDto personUpdateDto);
        Task DeletePersonAsync(int id);
        Task<PersonAdminDto> GetPersonAdminDetailsAsync(int personId);
        Task<IEnumerable<PersonAdminDto>> GetSubordinatesAsync(int managerId);
        Task<IEnumerable<PersonAdminDto>> GetPeopleByStreamAsync(string stream);
        Task<IEnumerable<UserReadDto>> GetDivisionManagersAsync();
        Task<IEnumerable<UserReadDto>> GetTalentManagersByDivisionManagerAsync(int divisionManagerId);
        Task<IEnumerable<UserReadDto>> GetAllTalentManagersAsync();
    }
}