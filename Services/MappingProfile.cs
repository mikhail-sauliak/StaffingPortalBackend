using AutoMapper;
using StaffingPortalBackend.DTO;
using StaffingPortalBackend.Models;

namespace StaffingPortalBackend.Services
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
             CreateMap<Person, UserReadDto>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.Username, opt => opt.MapFrom(src => src.User.Username))
                .ForMember(dest => dest.Role, opt => opt.MapFrom(src => src.User.Role))
                .ForMember(dest => dest.Stream, opt => opt.MapFrom(src => src.User.Stream))
                .ForMember(dest => dest.PersonId, opt => opt.MapFrom(src => src.UserId))
                .ForMember(dest => dest.ManagerId, opt => opt.MapFrom(src => src.DivisionManagerId ?? src.TalentManagerId))
                // Add other properties as needed
                .ForMember(dest => dest.ManagedBy, opt => opt.Ignore()) // Ignore or map accordingly
                .ForMember(dest => dest.Division, opt => opt.Ignore()); // Ignore or map accordingly   

            // User to UserReadDto mapping
            CreateMap<User, UserReadDto>()
                .ForMember(dest => dest.Username, opt => opt.MapFrom(src => src.Username))
                .ForMember(dest => dest.Role, opt => opt.MapFrom(src => src.Role))
                .ForMember(dest => dest.Stream, opt => opt.MapFrom(src => src.Stream))
                .ForMember(dest => dest.PersonId, opt => opt.MapFrom(src => src.PersonId))
                .ForMember(dest => dest.ManagerId, opt => opt.MapFrom(src => src.Person.DivisionManagerId ?? src.Person.TalentManagerId))
                .ForMember(dest => dest.ManagedBy, opt => opt.MapFrom(src => 
                    src.Person.DivisionManager != null ? src.Person.DivisionManager.FirstName + " " + src.Person.DivisionManager.LastName :
                    src.Person.TalentManager != null ? src.Person.TalentManager.FirstName + " " + src.Person.TalentManager.LastName :
                    "N/A"))
                .ForMember(dest => dest.Division, opt => opt.MapFrom(src => src.Person.DivisionManager != null ? "Division" : "N/A")); // This is a placeholder, adjust it according to your actual logic

            // Person to PersonReadDto mapping
            CreateMap<Person, PersonReadDto>()
                .ForMember(dest => dest.FirstName, opt => opt.MapFrom(src => src.FirstName))
                .ForMember(dest => dest.LastName, opt => opt.MapFrom(src => src.LastName))
                .ForMember(dest => dest.Location, opt => opt.MapFrom(src => src.Location))
                .ForMember(dest => dest.DivisionManagerId, opt => opt.MapFrom(src => src.DivisionManagerId))
                .ForMember(dest => dest.TalentManagerId, opt => opt.MapFrom(src => src.TalentManagerId))
                .ForMember(dest => dest.AvailableFrom, opt => opt.MapFrom(src => src.AvailableFrom))
                .ForMember(dest => dest.TechStack, opt => opt.MapFrom(src => src.TechStack))
                .ForMember(dest => dest.Comments, opt => opt.MapFrom(src => src.Comments))
                .ForMember(dest => dest.Stream, opt => opt.MapFrom(src => src.Stream))
                .ForMember(dest => dest.TMAware, opt => opt.MapFrom(src => src.TMAware))
                .ForMember(dest => dest.Level, opt => opt.MapFrom(src => src.Level))
                .ForMember(dest => dest.AssignmentExistsInGCP, opt => opt.MapFrom(src => src.AssignmentExistsInGCP))
                .ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.UserId))
                .ForMember(dest => dest.PlannedAssignment, opt => opt.MapFrom(src => src.PlannedAssignment));

            // Person to PersonAdminDto mapping
            CreateMap<Person, PersonAdminDto>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.FullName, opt => opt.MapFrom(src => src.FirstName + " " + src.LastName))
                .ForMember(dest => dest.Role, opt => opt.MapFrom(src => src.User.Role))
                .ForMember(dest => dest.DivisionManager, opt => opt.MapFrom(src =>
                    src.User.Role == "Head" || src.User.Role == "DeputyHead" || src.User.Role == "DivisionManager"
                    ? new DivisionManagerDto { Id = src.Id, FullName = src.FirstName + " " + src.LastName }
                    : new DivisionManagerDto 
                    {
                        Id = src.DivisionManager.Id, 
                        FullName = src.DivisionManager.FirstName + " " + src.DivisionManager.LastName
                    }))
                .ForMember(dest => dest.TalentManager, opt => opt.MapFrom(src =>
                    src.User.Role == "Head"
                    ? new TalentManagerDto { Id = src.Id, FullName = src.FirstName + " " + src.LastName }
                    : new TalentManagerDto 
                    {
                        Id = src.TalentManager.Id,
                        FullName = src.TalentManager.FirstName + " " + src.TalentManager.LastName
                    }))
                .ForMember(dest => dest.ProjectAssignment, opt => opt.MapFrom(src => src.PlannedAssignment))
                .ForMember(dest => dest.TechStack, opt => opt.MapFrom(src => src.TechStack))
                .ForMember(dest => dest.Level, opt => opt.MapFrom(src => src.Level))
                .ForMember(dest => dest.Location, opt => opt.MapFrom(src => src.Location))
                .ForMember(dest => dest.AvailableFrom, opt => opt.MapFrom(src => src.AvailableFrom))
                .ForMember(dest => dest.TMAware, opt => opt.MapFrom(src => src.TMAware)); 

            // User mappings
            CreateMap<User, UserReadDto>();
            CreateMap<UserCreateDto, User>();
            CreateMap<UserUpdateDto, User>();

            // SalaryReview mappings
            CreateMap<SalaryReview, SalaryReviewReadDto>();
            CreateMap<SalaryReviewCreateDto, SalaryReview>();
            CreateMap<SalaryReviewUpdateDto, SalaryReview>();

            // Person mappings
            CreateMap<Person, PersonReadDto>();
            CreateMap<Person, PersonBaseDto>()
                .ForMember(dest => dest.HeadId, opt => opt.MapFrom(src => src.HeadId))
                .ForMember(dest => dest.HeadName, opt => opt.MapFrom(src => src.Head != null ? src.Head.FirstName + " " + src.Head.LastName : null))
                .ForMember(dest => dest.DeputyHeadId, opt => opt.MapFrom(src => src.DeputyHeadId))
                .ForMember(dest => dest.DeputyHeadName, opt => opt.MapFrom(src => src.DeputyHead != null ? src.DeputyHead.FirstName + " " + src.DeputyHead.LastName : null));

            // SalaryReviewHistory mappings
            CreateMap<SalaryReviewHistory, SalaryReviewHistoryReadDto>();
            CreateMap<SalaryReviewCreateDto, SalaryReviewHistory>();  // Reusing SalaryReviewCreateDto
            CreateMap<SalaryReview, SalaryReviewHistory>()  // Mapping from SalaryReview to SalaryReviewHistory
                .ForMember(dest => dest.SalaryReviewId, opt => opt.MapFrom(src => src.Id));

            // Mapping from SalaryReviewRequest to SalaryReviewRequestDto
            CreateMap<SalaryReviewRequest, SalaryReviewRequestDto>() 
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.PersonId, opt => opt.MapFrom(src => src.PersonId))
                .ForMember(dest => dest.ReviewDate, opt => opt.MapFrom(src => src.ReviewDate))
                .ForMember(dest => dest.Justification, opt => opt.MapFrom(src => src.Justification))
                .ForMember(dest => dest.NewSalaryStartDate, opt => opt.MapFrom(src => src.NewSalaryStartDate))
                .ForMember(dest => dest.SalaryReceivedDate, opt => opt.MapFrom(src => src.SalaryReceivedDate))
                .ForMember(dest => dest.TalentManagerComments, opt => opt.MapFrom(src => src.TalentManagerComments));

            // Mapping from SalaryReviewRequestDto to SalaryReviewRequest
            CreateMap<SalaryReviewRequestDto, SalaryReviewRequest>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.PersonId, opt => opt.MapFrom(src => src.PersonId))
                .ForMember(dest => dest.Justification, opt => opt.MapFrom(src => src.Justification))
                .ForMember(dest => dest.NewSalaryStartDate, opt => opt.MapFrom(src => src.NewSalaryStartDate))
                .ForMember(dest => dest.SalaryReceivedDate, opt => opt.MapFrom(src => src.SalaryReceivedDate))
                .ForMember(dest => dest.TalentManagerComments, opt => opt.MapFrom(src => src.TalentManagerComments))
                .ForMember(dest => dest.DivisionManagerComments, opt => opt.Ignore())
                .ForMember(dest => dest.Status, opt => opt.Ignore());

            CreateMap<PersonCreateDto, Person>();
            CreateMap<PersonUpdateDto, Person>();
        }
    }
}