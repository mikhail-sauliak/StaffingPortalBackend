using System.Threading.Tasks;
using StaffingPortalBackend.DTO.Onboarding;

namespace StaffingPortalBackend.Services
{
    public interface IOnboardingService
    {
        Task<PersonOnboardingResponseDto> OnboardPersonAsync(PersonOnboardingRequestDto request);
    }
}
