namespace StaffingPortalBackend.DTO
{
    public class UserCreateDto : UserBaseDto
    {
        public string Password { get; set; } // necessary to create a user
    }
}