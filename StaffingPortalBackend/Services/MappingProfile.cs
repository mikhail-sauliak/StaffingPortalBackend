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
               .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.UserId ?? 0))
               .ForMember(dest => dest.Username, opt => opt.MapFrom(src => src.User != null ? src.User.Username : string.Empty))
               .ForMember(dest => dest.Role, opt => opt.MapFrom(src => src.User != null ? src.User.Role : string.Empty))
               .ForMember(dest => dest.Stream, opt => opt.MapFrom(src => src.User != null ? src.User.Stream : string.Empty))
               .ForMember(dest => dest.PersonId, opt => opt.MapFrom(src => src.Id))
               .ForMember(dest => dest.ManagerId, opt => opt.MapFrom(src => src.DivisionManagerId ?? src.TalentManagerId ?? 0))
               .ForMember(dest => dest.ManagedBy, opt => opt.Ignore())
               .ForMember(dest => dest.Division, opt => opt.Ignore());

            // User to UserReadDto mapping
            CreateMap<User, UserReadDto>()
                .ForMember(dest => dest.Username, opt => opt.MapFrom(src => src.Username))
                .ForMember(dest => dest.Role, opt => opt.MapFrom(src => src.Role))
                .ForMember(dest => dest.Stream, opt => opt.MapFrom(src => src.Stream))
                .ForMember(dest => dest.PersonId, opt => opt.MapFrom(src => src.PersonId))
                .ForMember(dest => dest.ManagerId, opt => opt.MapFrom(src => src.Person != null ? (src.Person.DivisionManagerId ?? src.Person.TalentManagerId ?? 0) : 0))
                .ForMember(dest => dest.ManagedBy, opt => opt.MapFrom(src =>
                    src.Person != null
                        ? (src.Person.DivisionManager != null
                            ? src.Person.DivisionManager.FirstName + " " + src.Person.DivisionManager.LastName
                            : src.Person.TalentManager != null
                                ? src.Person.TalentManager.FirstName + " " + src.Person.TalentManager.LastName
                                : "N/A")
                        : "N/A"))
                .ForMember(dest => dest.Division, opt => opt.MapFrom(src =>
                    src.Person != null && src.Person.DivisionManager != null ? "Division" : "N/A"));

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
                .ForMember(dest => dest.FirstName, opt => opt.MapFrom(src => src.FirstName))
                .ForMember(dest => dest.LastName, opt => opt.MapFrom(src => src.LastName))
                .ForMember(dest => dest.FullName, opt => opt.MapFrom(src => src.FirstName + " " + src.LastName))
                .ForMember(dest => dest.Role, opt => opt.MapFrom(src => src.User != null ? src.User.Role : string.Empty))
                .ForMember(dest => dest.Stream, opt => opt.MapFrom(src => src.Stream))
                .ForMember(dest => dest.DivisionManager, opt => opt.MapFrom(src =>
                    src.User != null && (src.User.Role == "Head" || src.User.Role == "DeputyHead" || src.User.Role == "DivisionManager")
                        ? new DivisionManagerDto { Id = src.Id, FullName = src.FirstName + " " + src.LastName }
                        : src.DivisionManager != null
                            ? new DivisionManagerDto
                            {
                                Id = src.DivisionManager.Id,
                                FullName = src.DivisionManager.FirstName + " " + src.DivisionManager.LastName
                            }
                            : new DivisionManagerDto { Id = 0, FullName = "N/A" }))
                .ForMember(dest => dest.TalentManager, opt => opt.MapFrom(src =>
                    src.User != null && src.User.Role == "Head"
                        ? new TalentManagerDto { Id = src.Id, FullName = src.FirstName + " " + src.LastName }
                        : src.TalentManager != null
                            ? new TalentManagerDto
                            {
                                Id = src.TalentManager.Id,
                                FullName = src.TalentManager.FirstName + " " + src.TalentManager.LastName
                            }
                            : new TalentManagerDto { Id = 0, FullName = "N/A" }))
                .ForMember(dest => dest.PlannedAssignment, opt => opt.MapFrom(src => src.PlannedAssignment))
                .ForMember(dest => dest.DivisionManagerId, opt => opt.MapFrom(src => src.DivisionManagerId))
                .ForMember(dest => dest.TalentManagerId, opt => opt.MapFrom(src => src.TalentManagerId))
                .ForMember(dest => dest.TechStack, opt => opt.MapFrom(src => src.TechStack))
                .ForMember(dest => dest.Level, opt => opt.MapFrom(src => src.Level))
                .ForMember(dest => dest.Location, opt => opt.MapFrom(src => src.Location))
                .ForMember(dest => dest.Comments, opt => opt.MapFrom(src => src.Comments))
                .ForMember(dest => dest.AvailableFrom, opt => opt.MapFrom(src => src.AvailableFrom))
                .ForMember(dest => dest.TMAware, opt => opt.MapFrom(src => src.TMAware))
                .ForMember(dest => dest.IsDismissed, opt => opt.MapFrom(src => src.IsDismissed))
                .ForMember(dest => dest.EndDate, opt => opt.MapFrom(src => src.EndDate))
                .ForMember(dest => dest.AssignmentExistsInGCP, opt => opt.MapFrom(src => src.AssignmentExistsInGCP))
                .ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.UserId))
                .ForMember(dest => dest.HeadId, opt => opt.MapFrom(src => src.HeadId))
                .ForMember(dest => dest.HeadFullName, opt => opt.MapFrom(src => src.Head != null ? src.Head.FirstName + " " + src.Head.LastName : null))
                .ForMember(dest => dest.DeputyHeadId, opt => opt.MapFrom(src => src.DeputyHeadId))
                .ForMember(dest => dest.DeputyHeadFullName, opt => opt.MapFrom(src => src.DeputyHead != null ? src.DeputyHead.FirstName + " " + src.DeputyHead.LastName : null))
                .ForMember(dest => dest.ManagerPersonId, opt => opt.MapFrom(src =>
                    src.DivisionManager != null && src.DivisionManager.Id != src.Id
                        ? (int?)src.DivisionManager.Id
                        : src.TalentManager != null && src.TalentManager.Id != src.Id
                            ? (int?)src.TalentManager.Id
                            : src.Head != null && src.Head.Id != src.Id
                                ? (int?)src.Head.Id
                                : src.DeputyHead != null && src.DeputyHead.Id != src.Id
                                    ? (int?)src.DeputyHead.Id
                                    : null))
                .ForMember(dest => dest.ManagerFullName, opt => opt.MapFrom(src =>
                    src.DivisionManager != null && src.DivisionManager.Id != src.Id
                        ? src.DivisionManager.FirstName + " " + src.DivisionManager.LastName
                        : src.TalentManager != null && src.TalentManager.Id != src.Id
                            ? src.TalentManager.FirstName + " " + src.TalentManager.LastName
                            : src.Head != null && src.Head.Id != src.Id
                                ? src.Head.FirstName + " " + src.Head.LastName
                                : src.DeputyHead != null && src.DeputyHead.Id != src.Id
                                    ? src.DeputyHead.FirstName + " " + src.DeputyHead.LastName
                                    : string.Empty));

            // User mappings
            CreateMap<User, UserReadDto>();
            CreateMap<UserCreateDto, User>();
            CreateMap<UserUpdateDto, User>();

            // SalaryReview mappings
            CreateMap<SalaryReview, SalaryReviewReadDto>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.PersonId, opt => opt.MapFrom(src => src.PersonId))
                .ForMember(dest => dest.ReviewDate, opt => opt.MapFrom(src => src.ReviewDate))
                .ForMember(dest => dest.Justification, opt => opt.MapFrom(src => src.Justification))
                .ForMember(dest => dest.OldSalary, opt => opt.MapFrom(src => src.OldSalary))
                .ForMember(dest => dest.ProposedSalary, opt => opt.MapFrom(src => src.ProposedSalary))
                .ForMember(dest => dest.IsApproved, opt => opt.MapFrom(src => src.IsApproved))
                .ForMember(dest => dest.Currency, opt => opt.MapFrom(src => src.Currency))
                .ForMember(dest => dest.ContractType, opt => opt.MapFrom(src => src.ContractType))
                .ForMember(dest => dest.NewSalaryStartDate, opt => opt.MapFrom(src => src.NewSalaryStartDate))
                .ForMember(dest => dest.SalaryReceivedDate, opt => opt.MapFrom(src => src.SalaryReceivedDate))
                .ForMember(dest => dest.DivisionManagerComments, opt => opt.MapFrom(src => src.DivisionManagerComments))
                .ForMember(dest => dest.TalentManagerComments, opt => opt.MapFrom(src => src.TalentManagerComments))
                .ForMember(dest => dest.ReviewResult, opt => opt.MapFrom(src => src.ReviewResult)).ReverseMap();

            // Mapping from SalaryReviewCreateDto to SalaryReview
            CreateMap<SalaryReviewCreateDto, SalaryReview>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.PersonId, opt => opt.MapFrom(src => src.PersonId))
                .ForMember(dest => dest.ReviewDate, opt => opt.MapFrom(src => src.ReviewDate))
                .ForMember(dest => dest.Justification, opt => opt.MapFrom(src => src.Justification))
                .ForMember(dest => dest.OldSalary, opt => opt.MapFrom(src => src.OldSalary))
                .ForMember(dest => dest.ProposedSalary, opt => opt.MapFrom(src => src.ProposedSalary))
                .ForMember(dest => dest.IsApproved, opt => opt.MapFrom(src => src.IsApproved))
                .ForMember(dest => dest.Currency, opt => opt.MapFrom(src => src.Currency))
                .ForMember(dest => dest.ContractType, opt => opt.MapFrom(src => src.ContractType))
                .ForMember(dest => dest.NewSalaryStartDate, opt => opt.MapFrom(src => src.NewSalaryStartDate))
                .ForMember(dest => dest.SalaryReceivedDate, opt => opt.MapFrom(src => src.SalaryReceivedDate))
                .ForMember(dest => dest.DivisionManagerComments, opt => opt.MapFrom(src => src.DivisionManagerComments))
                .ForMember(dest => dest.TalentManagerComments, opt => opt.MapFrom(src => src.TalentManagerComments))
                .ForMember(dest => dest.ReviewResult, opt => opt.MapFrom(src => src.ReviewResult));

            CreateMap<SalaryReviewUpdateDto, SalaryReview>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.PersonId, opt => opt.MapFrom(src => src.PersonId))
                .ForMember(dest => dest.ReviewDate, opt => opt.MapFrom(src => src.ReviewDate))
                .ForMember(dest => dest.Justification, opt => opt.MapFrom(src => src.Justification))
                .ForMember(dest => dest.OldSalary, opt => opt.MapFrom(src => src.OldSalary))
                .ForMember(dest => dest.ProposedSalary, opt => opt.MapFrom(src => src.ProposedSalary))
                .ForMember(dest => dest.IsApproved, opt => opt.MapFrom(src => src.IsApproved))
                .ForMember(dest => dest.Currency, opt => opt.MapFrom(src => src.Currency))
                .ForMember(dest => dest.ContractType, opt => opt.MapFrom(src => src.ContractType))
                .ForMember(dest => dest.NewSalaryStartDate, opt => opt.MapFrom(src => src.NewSalaryStartDate))
                .ForMember(dest => dest.SalaryReceivedDate, opt => opt.MapFrom(src => src.SalaryReceivedDate))
                .ForMember(dest => dest.DivisionManagerComments, opt => opt.MapFrom(src => src.DivisionManagerComments))
                .ForMember(dest => dest.TalentManagerComments, opt => opt.MapFrom(src => src.TalentManagerComments))
                .ForMember(dest => dest.ReviewResult, opt => opt.MapFrom(src => src.ReviewResult))
                .ForMember(dest => dest.ApprovedByDM, opt => opt.MapFrom(src => src.ApprovedByDM));

            // Person mappings
            CreateMap<Person, PersonReadDto>();
            CreateMap<Person, PersonBaseDto>()
                .ForMember(dest => dest.HeadId, opt => opt.MapFrom(src => src.HeadId))
                .ForMember(dest => dest.HeadName, opt => opt.MapFrom(src => src.Head != null ? src.Head.FirstName + " " + src.Head.LastName : null))
                .ForMember(dest => dest.DeputyHeadId, opt => opt.MapFrom(src => src.DeputyHeadId))
                .ForMember(dest => dest.DeputyHeadName, opt => opt.MapFrom(src => src.DeputyHead != null ? src.DeputyHead.FirstName + " " + src.DeputyHead.LastName : null));

            // SalaryReviewHistory mappings
            CreateMap<SalaryReviewHistory, SalaryReviewHistoryReadDto>();
            CreateMap<SalaryReviewCreateDto, SalaryReviewHistory>();
            CreateMap<SalaryReview, SalaryReviewHistory>()
                .ForMember(dest => dest.HistoryId, opt => opt.Ignore())
                .ForMember(dest => dest.SalaryReviewId, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.ArchivedDate, opt => opt.MapFrom(src => DateTime.Now));

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
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status));

            CreateMap<SalaryReviewRequest, SalaryReview>()
                .ForMember(dest => dest.ReviewDate, opt => opt.MapFrom(src => DateTime.Now))
                .ForMember(dest => dest.Justification, opt => opt.MapFrom(src => src.Justification))
                .ForMember(dest => dest.OldSalary, opt => opt.MapFrom(src => src.OldSalary))
                .ForMember(dest => dest.ProposedSalary, opt => opt.MapFrom(src => src.NewSalary))
                .ForMember(dest => dest.Currency, opt => opt.MapFrom(src => src.Currency))
                .ForMember(dest => dest.ContractType, opt => opt.MapFrom(src => src.ContractType))
                .ForMember(dest => dest.NewSalaryStartDate, opt => opt.MapFrom(src => src.NewSalaryStartDate))
                .ForMember(dest => dest.SalaryReceivedDate, opt => opt.MapFrom(src => src.SalaryReceivedDate))
                .ForMember(dest => dest.DivisionManagerComments, opt => opt.MapFrom(src => src.DivisionManagerComments))
                .ForMember(dest => dest.TalentManagerComments, opt => opt.MapFrom(src => src.TalentManagerComments))
                .ForMember(dest => dest.IsApproved, opt => opt.MapFrom(src => true))
                .ForMember(dest => dest.ApprovedByDM, opt => opt.MapFrom(src => true))
                .ForMember(dest => dest.ReviewResult, opt => opt.MapFrom(src => "Approved"));

            CreateMap<SalaryReviewRequestDto, SalaryReview>()
                .ForMember(dest => dest.ReviewDate, opt => opt.MapFrom(src => DateTime.Now)) 
                .ForMember(dest => dest.Justification, opt => opt.MapFrom(src => src.Justification))
                .ForMember(dest => dest.OldSalary, opt => opt.MapFrom(src => src.OldSalary)) 
                .ForMember(dest => dest.ProposedSalary, opt => opt.MapFrom(src => src.NewSalary)) 
                .ForMember(dest => dest.Currency, opt => opt.MapFrom(src => src.Currency))
                .ForMember(dest => dest.ContractType, opt => opt.MapFrom(src => src.ContractType))
                .ForMember(dest => dest.NewSalaryStartDate, opt => opt.MapFrom(src => src.NewSalaryStartDate))
                .ForMember(dest => dest.SalaryReceivedDate, opt => opt.MapFrom(src => src.SalaryReceivedDate))
                .ForMember(dest => dest.DivisionManagerComments, opt => opt.MapFrom(src => src.DivisionManagerComments))
                .ForMember(dest => dest.TalentManagerComments, opt => opt.MapFrom(src => src.TalentManagerComments))
                .ForMember(dest => dest.IsApproved, opt => opt.MapFrom(src => true)) 
                .ForMember(dest => dest.ApprovedByDM, opt => opt.MapFrom(src => src.ApprovedByDM.HasValue && src.ApprovedByDM.Value)) 
                .ForMember(dest => dest.ReviewResult, opt => opt.MapFrom(src => "Approved")) 
                .ForMember(dest => dest.Id, opt => opt.Ignore()); 

            CreateMap<SalaryReviewPatchDto, SalaryReview>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.ReviewDate, opt => opt.Condition(src => src.ReviewDate.HasValue))
                .ForMember(dest => dest.Justification, opt => opt.Condition(src => !string.IsNullOrEmpty(src.Justification)))
                .ForMember(dest => dest.OldSalary, opt => opt.Condition(src => src.OldSalary.HasValue))
                .ForMember(dest => dest.ProposedSalary, opt => opt.Condition(src => src.ProposedSalary.HasValue))
                .ForMember(dest => dest.Currency, opt => opt.Condition(src => !string.IsNullOrEmpty(src.Currency)))
                .ForMember(dest => dest.ContractType, opt => opt.Condition(src => !string.IsNullOrEmpty(src.ContractType)))
                .ForMember(dest => dest.NewSalaryStartDate, opt => opt.Condition(src => src.NewSalaryStartDate.HasValue))
                .ForMember(dest => dest.SalaryReceivedDate, opt => opt.Condition(src => src.SalaryReceivedDate.HasValue))
                .ForMember(dest => dest.DivisionManagerComments, opt => opt.Condition(src => !string.IsNullOrEmpty(src.DivisionManagerComments)))
                .ForMember(dest => dest.TalentManagerComments, opt => opt.Condition(src => !string.IsNullOrEmpty(src.TalentManagerComments)))
                .ForMember(dest => dest.IsApproved, opt => opt.Condition(src => src.IsApproved.HasValue))
                .ForMember(dest => dest.PersonId, opt => opt.Ignore())
                .ForMember(dest => dest.ApprovedByDM, opt => opt.Ignore());

            CreateMap<SalaryReviewHistoryCreateDto, SalaryReviewHistory>()
                .ForMember(dest => dest.HistoryId, opt => opt.Ignore())
                .ForMember(dest => dest.SalaryReviewId, opt => opt.MapFrom(src => src.SalaryReviewId))
                .ForMember(dest => dest.PersonId, opt => opt.MapFrom(src => src.PersonId))
                .ForMember(dest => dest.ReviewDate, opt => opt.MapFrom(src => src.ReviewDate))
                .ForMember(dest => dest.Justification, opt => opt.MapFrom(src => src.Justification))
                .ForMember(dest => dest.OldSalary, opt => opt.MapFrom(src => src.OldSalary))
                .ForMember(dest => dest.ProposedSalary, opt => opt.MapFrom(src => src.ProposedSalary))
                .ForMember(dest => dest.IsApproved, opt => opt.MapFrom(src => src.IsApproved))
                .ForMember(dest => dest.Currency, opt => opt.MapFrom(src => src.Currency))
                .ForMember(dest => dest.ContractType, opt => opt.MapFrom(src => src.ContractType))
                .ForMember(dest => dest.NewSalaryStartDate, opt => opt.MapFrom(src => src.NewSalaryStartDate))
                .ForMember(dest => dest.SalaryReceivedDate, opt => opt.MapFrom(src => src.SalaryReceivedDate))
                .ForMember(dest => dest.DivisionManagerComments, opt => opt.MapFrom(src => src.DivisionManagerComments))
                .ForMember(dest => dest.TalentManagerComments, opt => opt.MapFrom(src => src.TalentManagerComments))
                .ForMember(dest => dest.Person, opt => opt.Ignore())
                .ForMember(dest => dest.ReviewResult, opt => opt.MapFrom(src => src.ReviewResult))
                .ForMember(dest => dest.ArchivedDate, opt => opt.Ignore());

            CreateMap<PersonCreateDto, Person>();
            CreateMap<PersonUpdateDto, Person>()
                .ForMember(dest => dest.Id, opt => opt.Ignore());

            CreateMap<QuarterlyDepartmentSalaryChange, QuarterlyDepartmentSalaryChangeDto>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.Year, opt => opt.MapFrom(src => src.Year))
                .ForMember(dest => dest.Q1, opt => opt.MapFrom(src => src.Q1))
                .ForMember(dest => dest.Q2, opt => opt.MapFrom(src => src.Q2))
                .ForMember(dest => dest.Q3, opt => opt.MapFrom(src => src.Q3))
                .ForMember(dest => dest.Q4, opt => opt.MapFrom(src => src.Q4))
                .ForMember(dest => dest.Total, opt => opt.MapFrom(src => src.Total))
                .ForMember(dest => dest.DepartmentId, opt => opt.MapFrom(src => src.DepartmentId));

            // Mapping from QuarterlyDepartmentSalaryChangeDto to QuarterlyDepartmentSalaryChange
            CreateMap<QuarterlyDepartmentSalaryChangeDto, QuarterlyDepartmentSalaryChange>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.Year, opt => opt.MapFrom(src => src.Year))
                .ForMember(dest => dest.Q1, opt => opt.MapFrom(src => src.Q1))
                .ForMember(dest => dest.Q2, opt => opt.MapFrom(src => src.Q2))
                .ForMember(dest => dest.Q3, opt => opt.MapFrom(src => src.Q3))
                .ForMember(dest => dest.Q4, opt => opt.MapFrom(src => src.Q4))
                .ForMember(dest => dest.Total, opt => opt.MapFrom(src => src.Total))
                .ForMember(dest => dest.DepartmentId, opt => opt.MapFrom(src => src.DepartmentId));

            CreateMap<UpdateFieldsDto, SalaryReviewRequest>()
                .ForMember(dest => dest.NewSalary, opt => opt.MapFrom(src => src.ProposedSalary))
                .ForMember(dest => dest.OldSalary, opt => opt.MapFrom(src => src.OldSalary))
                .ForMember(dest => dest.Currency, opt => opt.MapFrom(src => src.Currency))
                .ForMember(dest => dest.ContractType, opt => opt.MapFrom(src => src.ContractType))
                .ForMember(dest => dest.NewSalaryStartDate, opt => opt.MapFrom(src => src.NewSalaryStartDate))
                .ForMember(dest => dest.SalaryReceivedDate, opt => opt.MapFrom(src => src.SalaryReceivedDate))
                .ForMember(dest => dest.DivisionManagerComments, opt => opt.MapFrom(src => src.DivisionManagerComments))
                .ForMember(dest => dest.IsApproved, opt => opt.MapFrom(src => src.IsApproved))
                .ForMember(dest => dest.ApprovedByDM, opt => opt.MapFrom(src => src.ApprovedByDM));

            CreateMap<TitleHistory, TitleHistoryDto>().ReverseMap();            
        }
    }
}