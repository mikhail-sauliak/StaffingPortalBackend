using AutoMapper;
using StaffingPortalBackend.DTO;
using StaffingPortalBackend.Models;

namespace StaffingPortalBackend.Services
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
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

            // SalaryReviewHistory mappings
            CreateMap<SalaryReviewHistory, SalaryReviewHistoryReadDto>();
            CreateMap<SalaryReviewCreateDto, SalaryReviewHistory>();  // Reusing SalaryReviewCreateDto
            CreateMap<SalaryReview, SalaryReviewHistory>()  // Mapping from SalaryReview to SalaryReviewHistory
                .ForMember(dest => dest.SalaryReviewId, opt => opt.MapFrom(src => src.Id));

            // Mapping between SalaryReviewRequest and SalaryReviewRequestDto
            CreateMap<SalaryReviewRequest, SalaryReviewRequestDto>();
            CreateMap<SalaryReviewRequest, SalaryReviewRequestDto>()
                .ForMember(
                    dest => dest.ContractType,
                    opt => opt.MapFrom(src => src.Person.SalaryReviews.FirstOrDefault().ContractType))
                .ForMember(
                    dest => dest.Currency,
                    opt => opt.MapFrom(src => src.Person.SalaryReviews.FirstOrDefault().Currency))
                .ForMember(
                    dest => dest.OldSalary,
                    opt => opt.MapFrom(src => src.Person.SalaryReviews.FirstOrDefault().OldSalary))
                .ForMember(
                    dest => dest.ReviewDate,
                    opt => opt.MapFrom(src => src.Person.SalaryReviews.FirstOrDefault().ReviewDate));

            CreateMap<PersonCreateDto, Person>();
            CreateMap<Person, PersonAdminDto>()
                .ForMember(dest => dest.FullName, opt => opt.MapFrom(src => src.FirstName + " " + src.LastName));
            CreateMap<PersonUpdateDto, Person>();
        }
    }
}