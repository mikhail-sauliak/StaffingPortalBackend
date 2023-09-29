namespace StaffingPortalBackend.DTO
{
    public class ProjectReadDto : ProjectUpdateDto
    {
        public int Id { get; set; }
        public List<PersonReadDto> Candidates { get; set; }
    }
}