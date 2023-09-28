namespace StaffingPortalBackend.DTO
{
    public class ProjectCreateDto
    {
        public string Name { get; set; }
        public string TechStack { get; set; }
        public string StartDate { get; set; }
        public string EndDate { get; set; }
        public string Comments { get; set; }
        // Если вы хотите позволить клиенту устанавливать связанных кандидатов при создании проекта,
        // то вы можете добавить список идентификаторов кандидатов
        public List<int> CandidateIds { get; set; }
    }
}