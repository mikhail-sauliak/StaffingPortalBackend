// using AutoMapper;
// using Microsoft.EntityFrameworkCore;
// using StaffingPortalBackend.DTO;
// using StaffingPortalBackend.Models;

// namespace StaffingPortalBackend.Services
// {
//     public class SalaryReviewHistoryService : ISalaryReviewHistoryService
//     {
//         private readonly AppDbContext _context;
//         private readonly IMapper _mapper;
//         private readonly AuthorizationHelper _authorizationHelper;

//         public SalaryReviewHistoryService(AppDbContext context, IMapper mapper, AuthorizationHelper authorizationHelper)
//         {
//             _context = context;
//             _mapper = mapper;
//             _authorizationHelper = authorizationHelper;
//         }

//         public async Task<IEnumerable<SalaryReviewHistoryReadDto>> GetAllHistoryAsync()
//         {
//             var history = await _context.SalaryReviewHistories.ToListAsync();
//             return _mapper.Map<IEnumerable<SalaryReviewHistoryReadDto>>(history);
//         }

//         public async Task<IEnumerable<SalaryReviewHistoryReadDto>> GetHistoryByPersonIdAsync(int personId)
//         {
//             var history = await _context.SalaryReviewHistories
//                 .Where(srh => srh.PersonId == personId)
//                 .ToListAsync();

//             return _mapper.Map<IEnumerable<SalaryReviewHistoryReadDto>>(history);
//         }

//         public async Task<SalaryReviewHistoryReadDto> GetHistoryByIdAsync(int id)
//         {
//             var history = await _context.SalaryReviewHistories.FindAsync(id);
//             return _mapper.Map<SalaryReviewHistoryReadDto>(history);
//         }

//         public async Task CreateHistoryAsync(SalaryReviewHistoryCreateDto historyDto)
//         {
//             if (string.IsNullOrWhiteSpace(historyDto.ReviewResult))
//             {
//                 historyDto.ReviewResult = "Approved"; // значение по умолчанию
//             }

//             Console.WriteLine($"Creating history with data: " +
//                       $"\nReviewResult: {historyDto.ReviewResult}" +
//                       $"\nPersonId: {historyDto.PersonId}" +
//                       $"\nReviewDate: {historyDto.ReviewDate}" +
//                       $"\nJustification: {historyDto.Justification}" +
//                       $"\nOldSalary: {historyDto.OldSalary}" +
//                       $"\nProposedSalary: {historyDto.ProposedSalary}" +
//                       $"\nIsApproved: {historyDto.IsApproved}" +
//                       $"\nCurrency: {historyDto.Currency}" +
//                       $"\nContractType: {historyDto.ContractType}" +
//                       $"\nNewSalaryStartDate: {historyDto.NewSalaryStartDate}" +
//                       $"\nSalaryReceivedDate: {historyDto.SalaryReceivedDate}" +
//                       $"\nDivisionManagerComments: {historyDto.DivisionManagerComments}" +
//                       $"\nTalentManagerComments: {historyDto.TalentManagerComments}");

//             var history = _mapper.Map<SalaryReviewHistory>(historyDto);
//             _context.SalaryReviewHistories.Add(history);
//             try
//             {
//                 await _context.SaveChangesAsync();
//             }
//             catch (Exception e)
//             {
//                 Console.WriteLine($"Error saving history: {e.Message}");
//                 throw;
//             }
//         }

//         public async Task UpdateHistoryAsync(SalaryReviewHistoryUpdateDto historyDto)
//         {
//             var history = await _context.SalaryReviewHistories.FindAsync(historyDto.HistoryId);
//             _mapper.Map(historyDto, history);
//             await _context.SaveChangesAsync();
//         }

//         public async Task DeleteHistoryAsync(int id)
//         {
//             var history = await _context.SalaryReviewHistories.FindAsync(id);
//             _context.SalaryReviewHistories.Remove(history);
//             await _context.SaveChangesAsync();
//         }

//         public async Task ArchiveSalaryReview(int salaryReviewId)
//         {
//             var salaryReview = await _context.SalaryReviews.FindAsync(salaryReviewId);
//             if (salaryReview != null)
//             {
//                 var historyRecord = _mapper.Map<SalaryReviewHistory>(salaryReview);
//                 _context.SalaryReviewHistories.Add(historyRecord);
//                 await _context.SaveChangesAsync();
//             }
//         }

//         public async Task<bool> ArchivePreviousSalaryReview(int personId)
//         {
//             var salaryReviews = await _context.SalaryReviews
//                 .Where(sr => sr.PersonId == personId)
//                 .OrderByDescending(sr => sr.ReviewDate)
//                 .ToListAsync();

//             var previousSalaryReview = salaryReviews.LastOrDefault();

//             if (previousSalaryReview != null)
//             {
//                 Console.WriteLine($"Archiving salary review for PersonID: {personId}");
//                 Console.WriteLine($"Original salary review: OldSalary = {previousSalaryReview.OldSalary}, ProposedSalary = {previousSalaryReview.ProposedSalary}");

//                 var historyRecord = _mapper.Map<SalaryReviewHistory>(previousSalaryReview);
//                 Console.WriteLine($"Mapped to history: OldSalary = {historyRecord.OldSalary}, ProposedSalary = {historyRecord.ProposedSalary}");
//                 _context.SalaryReviewHistories.Add(historyRecord);
//                 _context.SalaryReviews.Remove(previousSalaryReview);
//                 await _context.SaveChangesAsync();
//                 Console.WriteLine($"Archived successfully for PersonID: {personId}");
//                 return true;
//             }
//             Console.WriteLine($"No salary review found to archive for PersonID: {personId}");
//             return false;
//         }

//         public async Task<IEnumerable<MonthlySalaryDto>> GetAggregatedMonthlySalariesByYear(int personId, int year)
//         {
//             var salaryReviews = await _context.SalaryReviews
//                 .Where(sr => sr.PersonId == personId && sr.NewSalaryStartDate.HasValue && sr.NewSalaryStartDate.Value.Year == year)
//                 .ToListAsync();

//             var salaryReviewHistories = await _context.SalaryReviewHistories
//                 .Where(srh => srh.PersonId == personId && srh.NewSalaryStartDate.HasValue && srh.NewSalaryStartDate.Value.Year == year)
//                 .ToListAsync();

//             var combinedSalaries = salaryReviews.Concat(salaryReviewHistories.Select(srh => new SalaryReview
//             {
//                 NewSalaryStartDate = srh.NewSalaryStartDate,
//                 ProposedSalary = srh.ProposedSalary,
//                 Currency = srh.Currency
//             }));

//             var aggregatedData = combinedSalaries
//                 .GroupBy(sr => new { sr.NewSalaryStartDate.Value.Month, sr.Currency })
//                 .Select(g => new MonthlySalaryDto
//                 {
//                     Year = year,
//                     Month = g.Key.Month,
//                     Currency = g.Key.Currency,
//                     TotalSalary = g.Sum(sr => sr.ProposedSalary)
//                 })
//                 .ToList();

//             return aggregatedData;
//         }

//         public async Task<IEnumerable<AggregatedSalaryDto>> GetAggregatedSalariesForAllTime(User currentUser)
//         {
//             var salaryReviews = await _context.SalaryReviews.Include(sr => sr.Person).ToListAsync();
//             var salaryReviewHistories = await _context.SalaryReviewHistories.Include(srh => srh.Person).ToListAsync();

//             var allSalaries = salaryReviews.Select(sr => new
//             {
//                 sr.Person.Id,
//                 sr.Person.FirstName,
//                 sr.Person.LastName,
//                 sr.Person.Stream,
//                 sr.ProposedSalary,
//                 sr.Currency,
//                 sr.NewSalaryStartDate
//             }).Concat(salaryReviewHistories.Select(srh => new
//             {
//                 srh.Person.Id,
//                 srh.Person.FirstName,
//                 srh.Person.LastName,
//                 srh.Person.Stream,
//                 ProposedSalary = srh.ProposedSalary,
//                 srh.Currency,
//                 srh.NewSalaryStartDate
//             }));

//             var aggregatedData = allSalaries
//                 .GroupBy(s => new { s.Id, s.Currency })
//                 .Select(g => new AggregatedSalaryDto
//                 {
//                     PersonId = g.Key.Id,
//                     FullName = $"{g.First().FirstName} {g.First().LastName}",
//                     Stream = g.First().Stream,
//                     Currency = g.Key.Currency,
//                     Salaries = g.Select(s => new SalaryPeriodDto { Salary = s.ProposedSalary, StartDate = s.NewSalaryStartDate }).ToList()
//                 }).ToList();

//             return aggregatedData.Where(dto => _authorizationHelper.CanUserAccessData(currentUser, dto.PersonId)).ToList();
//         }
//         public async Task<IEnumerable<AggregatedSalaryDataDto>> GetAggregatedSalariesByYear(int year)
// {
//     // Соединяем данные из SalaryReviews и SalaryReviewHistories
//     var salaries = await _context.SalaryReviews
//         .Where(sr => sr.NewSalaryStartDate.HasValue && sr.NewSalaryStartDate.Value.Year == year)
//         .ToListAsync();
//     var historySalaries = await _context.SalaryReviewHistories
//         .Where(sh => sh.NewSalaryStartDate.HasValue && sh.NewSalaryStartDate.Value.Year == year)
//         .ToListAsync();

//     // Объединяем текущие зарплаты и исторические данные
//     var combinedSalaries = salaries.Concat(historySalaries);

//     // Агрегируем данные по каждому месяцу
//     var aggregatedData = combinedSalaries
//         .GroupBy(s => new { s.PersonId, Month = s.NewSalaryStartDate.Value.Month })
//         .Select(g => new AggregatedSalaryDataDto
//         {
//             PersonId = g.Key.PersonId,
//             Month = g.Key.Month,
//             TotalSalary = g.Sum(s => s.ProposedSalary)
//         })
//         .OrderBy(a => a.PersonId).ThenBy(a => a.Month)
//         .ToList();

//     return aggregatedData;
// }
//     }
// }

using AutoMapper;
using Microsoft.EntityFrameworkCore;
using StaffingPortalBackend.DTO;
using StaffingPortalBackend.Models;
using System.Linq;

namespace StaffingPortalBackend.Services
{
    public class SalaryReviewHistoryService : ISalaryReviewHistoryService
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;
        private readonly AuthorizationHelper _authorizationHelper;

        public SalaryReviewHistoryService(AppDbContext context, IMapper mapper, AuthorizationHelper authorizationHelper)
        {
            _context = context;
            _mapper = mapper;
            _authorizationHelper = authorizationHelper;
        }


        public async Task<IEnumerable<SalaryReviewHistoryReadDto>> GetAllHistoryAsync()
        {
            var history = await _context.SalaryReviewHistories.ToListAsync();
            return _mapper.Map<IEnumerable<SalaryReviewHistoryReadDto>>(history);
        }

        public async Task<IEnumerable<SalaryReviewHistoryReadDto>> GetHistoryByPersonIdAsync(int personId)
        {
            var history = await _context.SalaryReviewHistories
                .Where(srh => srh.PersonId == personId)
                .ToListAsync();

            return _mapper.Map<IEnumerable<SalaryReviewHistoryReadDto>>(history);
        }

        public async Task<IEnumerable<SalaryReviewHistoryReadDto>> GetSalaryReviewHistoryByPersonId(int personId)
        {
            return await GetHistoryByPersonIdAsync(personId);
        }

        public async Task<SalaryReviewHistoryReadDto> GetHistoryByIdAsync(int id)
        {
            var history = await _context.SalaryReviewHistories.FindAsync(id);
            return _mapper.Map<SalaryReviewHistoryReadDto>(history);
        }

        public async Task CreateHistoryAsync(SalaryReviewHistoryCreateDto historyDto)
        {
            if (string.IsNullOrWhiteSpace(historyDto.ReviewResult))
            {
                historyDto.ReviewResult = "Approved"; // default value
            }

            Console.WriteLine($"Creating history with data: {historyDto}");

            var history = _mapper.Map<SalaryReviewHistory>(historyDto);
            _context.SalaryReviewHistories.Add(history);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateHistoryAsync(SalaryReviewHistoryUpdateDto historyDto)
        {
            var history = await _context.SalaryReviewHistories.FindAsync(historyDto.HistoryId);
            _mapper.Map(historyDto, history);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteHistoryAsync(int id)
        {
            var history = await _context.SalaryReviewHistories.FindAsync(id);
            if (history != null)
            {
                _context.SalaryReviewHistories.Remove(history);
                await _context.SaveChangesAsync();
            }
        }

        public async Task ArchiveSalaryReview(int salaryReviewId)
        {
            var salaryReview = await _context.SalaryReviews.FindAsync(salaryReviewId);
            if (salaryReview != null)
            {
                var historyRecord = _mapper.Map<SalaryReviewHistory>(salaryReview);
                _context.SalaryReviewHistories.Add(historyRecord);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<bool> ArchivePreviousSalaryReview(int personId)
        {
            var salaryReviews = await _context.SalaryReviews
                .Where(sr => sr.PersonId == personId)
                .OrderByDescending(sr => sr.ReviewDate)
                .ToListAsync();

            var latestSalaryReview = salaryReviews.FirstOrDefault();

            if (latestSalaryReview != null)
            {
                Console.WriteLine($"Archiving salary review for PersonID: {personId}");
                Console.WriteLine($"Original salary review: {latestSalaryReview}");

                var historyRecord = _mapper.Map<SalaryReviewHistory>(latestSalaryReview);
                Console.WriteLine($"Mapped to history: {historyRecord}");
                _context.SalaryReviewHistories.Add(historyRecord);
                _context.SalaryReviews.Remove(latestSalaryReview);
                await _context.SaveChangesAsync();
                Console.WriteLine($"Archived successfully for PersonID: {personId}");
                return true;
            }
            Console.WriteLine($"No salary review found to archive for PersonID: {personId}");
            return false;
        }

        public async Task<List<MonthlySalaryAggregationDto>> GetAggregatedMonthlySalariesByYear(int year, User currentUser)
        {
            var people = await _context.People.ToListAsync();
            var allSalaries = await _context.SalaryReviews
                .Where(sr => sr.SalaryReceivedDate.HasValue && sr.SalaryReceivedDate.Value.Year <= year)
                .Select(sr => new
                {
                    sr.PersonId,
                    sr.SalaryReceivedDate,
                    sr.ProposedSalary,
                    sr.Currency,
                    sr.OldSalary
                }).ToListAsync();

            var salaryHistories = await _context.SalaryReviewHistories
                .Where(sh => sh.SalaryReceivedDate.HasValue && sh.SalaryReceivedDate.Value.Year <= year)
                .Select(sh => new
                {
                    sh.PersonId,
                    sh.SalaryReceivedDate,
                    sh.ProposedSalary,
                    sh.Currency,
                    sh.OldSalary
                }).ToListAsync();

            var combinedSalaries = allSalaries.Concat(salaryHistories)
                                              .OrderByDescending(sr => sr.SalaryReceivedDate)
                                              .ToList();

            var results = new List<MonthlySalaryAggregationDto>();

            Dictionary<int, (decimal OldSalary, string Currency)> lastKnownBeforeYear = new Dictionary<int, (decimal, string)>();

            foreach (var person in people)
            {
                if (!await _authorizationHelper.CanUserAccessDataAsync(currentUser, person.Id))
                {
                    continue;
                }

                var personSalaries = combinedSalaries.Where(sr => sr.PersonId == person.Id).ToList();
                var monthlySalaries = new SalaryMonthPeriodDto[12];

                decimal lastKnownSalary = 0;
                string lastKnownCurrency = "PLN";

                var firstSalaryReview = personSalaries
                    .OrderBy(sr => sr.SalaryReceivedDate)
                    .FirstOrDefault();

                var lastReviewBeforeYear = combinedSalaries
                    .Where(sr => sr.PersonId == person.Id && sr.SalaryReceivedDate < new DateTime(year, 1, 1))
                    .OrderByDescending(sr => sr.SalaryReceivedDate)
                    .FirstOrDefault();
                if (lastReviewBeforeYear != null)
                {
                    lastKnownBeforeYear[person.Id] = (lastReviewBeforeYear.OldSalary, lastReviewBeforeYear.Currency);
                }

                for (int month = 1; month <= 12; month++)
                {
                    var monthStart = new DateTime(year, month, 1);
                    var applicableSalary = personSalaries
                        .Where(sr => sr.SalaryReceivedDate <= monthStart)
                        .OrderByDescending(sr => sr.SalaryReceivedDate)
                        .FirstOrDefault();

                    if (person.IsDismissed && person.EndDate.HasValue)
                    {
                        if (year > person.EndDate.Value.Year || (year == person.EndDate.Value.Year && month > person.EndDate.Value.Month))
                        {
                            lastKnownSalary = 0;
                        }
                        else if (applicableSalary != null)
                        {
                            lastKnownSalary = applicableSalary.ProposedSalary;
                            lastKnownCurrency = applicableSalary.Currency;
                        }
                    }
                    else if (applicableSalary != null)
                    {
                        lastKnownSalary = applicableSalary.ProposedSalary;
                        lastKnownCurrency = applicableSalary.Currency;
                    }
                    else
                    {
                        DateTime referenceDate = new DateTime(2022, 4, 1);
                        if (firstSalaryReview != null && firstSalaryReview.SalaryReceivedDate.Value > monthStart)
                        {
                            lastKnownSalary = (monthStart >= referenceDate) ? firstSalaryReview.OldSalary : 0;
                            lastKnownCurrency = firstSalaryReview.Currency;
                        }
                        else if (firstSalaryReview == null || monthStart < firstSalaryReview.SalaryReceivedDate)
                        {
                            lastKnownSalary = (monthStart >= referenceDate) ? 0 : lastKnownSalary;
                            lastKnownCurrency = "PLN";
                        }
                    }

                    monthlySalaries[month - 1] = new SalaryMonthPeriodDto
                    {
                        Month = month,
                        Salary = lastKnownSalary,
                        Currency = lastKnownCurrency
                    };
                }

                results.Add(new MonthlySalaryAggregationDto
                {
                    PersonId = person.Id,
                    FullName = $"{person.FirstName} {person.LastName}",
                    Salaries = monthlySalaries.ToList(),
                    IsDismissed = person.IsDismissed,
                    EndDate = person.EndDate,
                    AvailableFrom = person.AvailableFrom
                });
            }

            return results;
        }

        public async Task<IEnumerable<AggregatedSalaryDto>> GetAggregatedSalariesByYear(int year)
        {
            var people = await _context.People.ToListAsync();

            var currentYearSalaries = await _context.SalaryReviews
                .Where(sr => sr.SalaryReceivedDate.HasValue && sr.SalaryReceivedDate.Value.Year == year)
                .ToListAsync();

            var previousSalaries = await _context.SalaryReviewHistories
                .OrderByDescending(sh => sh.SalaryReceivedDate)
                .ToListAsync();

            var results = new List<AggregatedSalaryDto>();

            foreach (var person in people)
            {
                var salaries = new Dictionary<int, decimal>();

                foreach (var salary in currentYearSalaries.Where(sr => sr.PersonId == person.Id))
                {
                    var startMonth = salary.SalaryReceivedDate.Value.Month;
                    for (int month = startMonth; month <= 12; month++)
                    {
                        salaries[month] = salary.ProposedSalary;
                    }
                }

                var lastHistoricalSalary = previousSalaries.FirstOrDefault(sh => sh.PersonId == person.Id && sh.SalaryReceivedDate.Value.Year < year);
                if (lastHistoricalSalary != null)
                {
                    for (int month = 1; month < (currentYearSalaries.FirstOrDefault()?.SalaryReceivedDate?.Month ?? 1); month++)
                    {
                        salaries[month] = lastHistoricalSalary.ProposedSalary;
                    }
                }

                // Добавление данных в результат
                results.Add(new AggregatedSalaryDto
                {
                    PersonId = person.Id,
                    FullName = $"{person.FirstName} {person.LastName}",
                    Salaries = Enumerable.Range(1, 12).Select(month => new SalaryPeriodDto
                    {
                        StartDate = new DateTime(year, month, 1),
                        Salary = salaries.ContainsKey(month) ? salaries[month] : 0
                    }).ToList()
                });
            }

            return results;
        }

        public async Task<IEnumerable<AggregatedSalaryDto>> GetAggregatedSalariesForAllTime(User currentUser)
        {
            var salaryReviews = await _context.SalaryReviews.Include(sr => sr.Person).ToListAsync();
            var salaryReviewHistories = await _context.SalaryReviewHistories.Include(srh => srh.Person).ToListAsync();

            var allSalaries = salaryReviews.Select(sr => new
            {
                sr.Person.Id,
                sr.Person.FirstName,
                sr.Person.LastName,
                sr.Person.Stream,
                sr.ProposedSalary,
                sr.Currency,
                sr.NewSalaryStartDate
            }).Concat(salaryReviewHistories.Select(srh => new
            {
                srh.Person.Id,
                srh.Person.FirstName,
                srh.Person.LastName,
                srh.Person.Stream,
                ProposedSalary = srh.ProposedSalary,
                srh.Currency,
                srh.NewSalaryStartDate
            }));

            var aggregatedData = allSalaries
                .GroupBy(s => new { s.Id, s.Currency })
                .Select(g => new AggregatedSalaryDto
                {
                    PersonId = g.Key.Id,
                    FullName = $"{g.First().FirstName} {g.First().LastName}",
                    Stream = g.First().Stream,
                    Currency = g.Key.Currency,
                    Salaries = g.Select(s => new SalaryPeriodDto { Salary = s.ProposedSalary, StartDate = s.NewSalaryStartDate }).ToList()
                }).ToList();

            var filtered = new List<AggregatedSalaryDto>();
            foreach (var dto in aggregatedData)
            {
                if (await _authorizationHelper.CanUserAccessDataAsync(currentUser, dto.PersonId))
                {
                    filtered.Add(dto);
                }
            }

            return filtered;
        }

        private decimal CalculateProRatedSalary(DateTime endDate, decimal salary)
        {
            int daysInMonth = DateTime.DaysInMonth(endDate.Year, endDate.Month);
            decimal dailyRate = salary / daysInMonth;
            return dailyRate * endDate.Day;
        }
    }
}