using StaffingPortalBackend.DTO;
using StaffingPortalBackend.Models;
using StaffingPortalBackend.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StaffingPortalBackend.Tests.Unit.Mocks
{
    public class MockSalaryReviewHistoryService : ISalaryReviewHistoryService
    {
        private readonly List<SalaryReviewHistoryReadDto> _mockHistory;
        private readonly Dictionary<int, SalaryReviewReadDto> _mockSalaryReviews; 
        private int _nextHistoryId = 1;

        public MockSalaryReviewHistoryService()
        {
            _mockHistory = new List<SalaryReviewHistoryReadDto>();
            _mockSalaryReviews = new Dictionary<int, SalaryReviewReadDto>();
        }

        public Task<IEnumerable<SalaryReviewHistoryReadDto>> GetAllHistoryAsync()
        {
            return Task.FromResult<IEnumerable<SalaryReviewHistoryReadDto>>(_mockHistory.ToList());
        }

        public Task<IEnumerable<SalaryReviewHistoryReadDto>> GetHistoryByPersonIdAsync(int personId)
        {
            var history = _mockHistory.Where(h => h.PersonId == personId).ToList();
            return Task.FromResult<IEnumerable<SalaryReviewHistoryReadDto>>(history);
        }

        public Task<SalaryReviewHistoryReadDto> GetHistoryByIdAsync(int id)
        {
            var history = _mockHistory.FirstOrDefault(h => h.HistoryId == id);
            return Task.FromResult(history ?? new SalaryReviewHistoryReadDto());
        }

        public Task<IEnumerable<SalaryReviewHistoryReadDto>> GetSalaryReviewHistoryByPersonId(int personId)
        {
            return GetHistoryByPersonIdAsync(personId);
        }

        public Task CreateHistoryAsync(SalaryReviewHistoryCreateDto historyDto)
        {
            var newHistory = MapCreateDtoToReadDto(historyDto);
            newHistory.HistoryId = _nextHistoryId++;
            newHistory.ArchivedDate = DateTime.UtcNow;
            _mockHistory.Add(newHistory);
            return Task.CompletedTask;
        }

        public Task UpdateHistoryAsync(SalaryReviewHistoryUpdateDto historyDto)
        {
            var existing = _mockHistory.FirstOrDefault(h => h.HistoryId == historyDto.HistoryId);
            if (existing != null)
            {
                MergeHistory(existing, historyDto);
                existing.ArchivedDate = DateTime.UtcNow;
            }

            return Task.CompletedTask;
        }

        public Task DeleteHistoryAsync(int id)
        {
            var existing = _mockHistory.FirstOrDefault(h => h.HistoryId == id);
            if (existing != null)
            {
                _mockHistory.Remove(existing);
            }

            return Task.CompletedTask;
        }

        public Task ArchiveSalaryReview(int salaryReviewId)
        {
            if (_mockSalaryReviews.TryGetValue(salaryReviewId, out var salaryReview))
            {
                var newHistoryRecord = new SalaryReviewHistoryReadDto
                {
                    HistoryId = _nextHistoryId++,
                    PersonId = salaryReview.PersonId,
                    ReviewDate = salaryReview.ReviewDate,
                    Justification = "Archived",
                    OldSalary = salaryReview.OldSalary,
                    ProposedSalary = salaryReview.ProposedSalary,
                    IsApproved = salaryReview.IsApproved,
                    Currency = salaryReview.Currency,
                    ContractType = salaryReview.ContractType,
                    NewSalaryStartDate = salaryReview.NewSalaryStartDate,
                    SalaryReceivedDate = salaryReview.SalaryReceivedDate,
                    DivisionManagerComments = salaryReview.DivisionManagerComments,
                    TalentManagerComments = salaryReview.TalentManagerComments,
                    ReviewResult = salaryReview.ReviewResult,
                    ArchivedDate = DateTime.UtcNow
                };

                _mockHistory.Add(newHistoryRecord);
            }

            return Task.CompletedTask;
        }

        public Task<bool> ArchivePreviousSalaryReview(int personId)
        {
            var previousSalaryReview = _mockSalaryReviews.Values
                .Where(sr => sr.PersonId == personId)
                .OrderByDescending(sr => sr.ReviewDate)
                .FirstOrDefault();

            if (previousSalaryReview != null)
            {
                var newHistoryRecord = new SalaryReviewHistoryReadDto
                {
                    HistoryId = _nextHistoryId++,
                    PersonId = previousSalaryReview.PersonId,
                    ReviewDate = previousSalaryReview.ReviewDate,
                    Justification = previousSalaryReview.Justification,
                    OldSalary = previousSalaryReview.OldSalary,
                    ProposedSalary = previousSalaryReview.ProposedSalary,
                    IsApproved = previousSalaryReview.IsApproved,
                    Currency = previousSalaryReview.Currency,
                    ContractType = previousSalaryReview.ContractType,
                    NewSalaryStartDate = previousSalaryReview.NewSalaryStartDate,
                    SalaryReceivedDate = previousSalaryReview.SalaryReceivedDate,
                    DivisionManagerComments = previousSalaryReview.DivisionManagerComments,
                    TalentManagerComments = previousSalaryReview.TalentManagerComments,
                    ReviewResult = "Archived",
                    ArchivedDate = DateTime.UtcNow
                };

                _mockHistory.Add(newHistoryRecord);
                return Task.FromResult(true);
            }

            return Task.FromResult(false);
        }

        public Task<List<MonthlySalaryAggregationDto>> GetAggregatedMonthlySalariesByYear(int year, User currentUser)
        {
            var result = _mockHistory
                .Where(h => h.ReviewDate.Year == year)
                .GroupBy(h => h.PersonId)
                .Select(group => new MonthlySalaryAggregationDto
                {
                    PersonId = group.Key,
                    FullName = $"Person {group.Key}",
                    Salaries = group
                        .GroupBy(item => item.ReviewDate.Month)
                        .Select(monthGroup => new SalaryMonthPeriodDto
                        {
                            Month = monthGroup.Key,
                            Salary = monthGroup.Last().ProposedSalary,
                            Currency = monthGroup.Last().Currency
                        })
                        .ToList(),
                    Currency = group.Last().Currency
                })
                .ToList();

            return Task.FromResult(result);
        }

        public Task<IEnumerable<AggregatedSalaryDto>> GetAggregatedSalariesForAllTime(User currentUser)
        {
            var result = _mockHistory
                .GroupBy(h => h.PersonId)
                .Select(group => new AggregatedSalaryDto
                {
                    PersonId = group.Key,
                    FullName = $"Person {group.Key}",
                    Stream = currentUser?.Stream ?? "N/A",
                    Currency = group.Last().Currency,
                    Salaries = group
                        .Select(item => new SalaryPeriodDto
                        {
                            Salary = item.ProposedSalary,
                            StartDate = item.NewSalaryStartDate
                        })
                        .ToList()
                });

            return Task.FromResult(result);
        }

        public Task<IEnumerable<AggregatedSalaryDto>> GetAggregatedSalariesByYear(int year)
        {
            var result = _mockHistory
                .Where(h => h.ReviewDate.Year == year)
                .GroupBy(h => h.PersonId)
                .Select(group => new AggregatedSalaryDto
                {
                    PersonId = group.Key,
                    FullName = $"Person {group.Key}",
                    Stream = "N/A",
                    Currency = group.Last().Currency,
                    Salaries = group
                        .Select(item => new SalaryPeriodDto
                        {
                            Salary = item.ProposedSalary,
                            StartDate = item.NewSalaryStartDate
                        })
                        .ToList()
                });

            return Task.FromResult(result);
        }

        public void AddToHistory(SalaryReviewHistoryReadDto historyRecord)
        {
            _mockHistory.Add(historyRecord);
        }

        public void AddSalaryReview(SalaryReviewReadDto salaryReview)
        {
            _mockSalaryReviews[salaryReview.Id] = salaryReview;
        }

        private static SalaryReviewHistoryReadDto MapCreateDtoToReadDto(SalaryReviewHistoryCreateDto createDto)
        {
            return new SalaryReviewHistoryReadDto
            {
                PersonId = createDto.PersonId,
                SalaryReviewId = createDto.SalaryReviewId,
                ReviewDate = createDto.ReviewDate,
                Justification = createDto.Justification,
                OldSalary = createDto.OldSalary,
                ProposedSalary = createDto.ProposedSalary,
                IsApproved = createDto.IsApproved,
                Currency = createDto.Currency,
                ContractType = createDto.ContractType,
                NewSalaryStartDate = createDto.NewSalaryStartDate,
                SalaryReceivedDate = createDto.SalaryReceivedDate,
                DivisionManagerComments = createDto.DivisionManagerComments,
                TalentManagerComments = createDto.TalentManagerComments,
                ReviewResult = createDto.ReviewResult
            };
        }

        private static void MergeHistory(SalaryReviewHistoryReadDto target, SalaryReviewHistoryCreateDto source)
        {
            target.PersonId = source.PersonId;
            target.SalaryReviewId = source.SalaryReviewId;
            target.ReviewDate = source.ReviewDate;
            target.Justification = source.Justification;
            target.OldSalary = source.OldSalary;
            target.ProposedSalary = source.ProposedSalary;
            target.IsApproved = source.IsApproved;
            target.Currency = source.Currency;
            target.ContractType = source.ContractType;
            target.NewSalaryStartDate = source.NewSalaryStartDate;
            target.SalaryReceivedDate = source.SalaryReceivedDate;
            target.DivisionManagerComments = source.DivisionManagerComments;
            target.TalentManagerComments = source.TalentManagerComments;
            target.ReviewResult = source.ReviewResult;
        }
    }
}
