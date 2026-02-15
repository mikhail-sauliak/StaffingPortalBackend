using StaffingPortalBackend.DTO;
using System;

public class SalaryReviewHistoryReadDtoBuilder
{
    private readonly SalaryReviewHistoryReadDto _historyReadDto = new SalaryReviewHistoryReadDto();

    public SalaryReviewHistoryReadDtoBuilder WithDefaults()
    {
        _historyReadDto.HistoryId = 1;
        _historyReadDto.PersonId = 1; 
        _historyReadDto.ReviewDate = DateTime.UtcNow;
        _historyReadDto.Justification = "Annual review";
        _historyReadDto.OldSalary = 50000m;
        _historyReadDto.ProposedSalary = 55000m;
        _historyReadDto.IsApproved = true;
        _historyReadDto.Currency = "USD";
        _historyReadDto.ContractType = "Full-time";
        _historyReadDto.NewSalaryStartDate = DateTime.UtcNow.AddDays(30);
        _historyReadDto.SalaryReceivedDate = DateTime.UtcNow.AddDays(60);
        _historyReadDto.DivisionManagerComments = "Outstanding performance";
        _historyReadDto.TalentManagerComments = "Agree with the assessment";
        _historyReadDto.ReviewResult = "Positive";
        _historyReadDto.ArchivedDate = DateTime.UtcNow;
        return this;
    }

    public SalaryReviewHistoryReadDtoBuilder WithHistoryId(int historyId)
    {
        _historyReadDto.HistoryId = historyId;
        return this;
    }

    public SalaryReviewHistoryReadDtoBuilder WithPersonId(int personId)
    {
        _historyReadDto.PersonId = personId;
        return this;
    }

    public SalaryReviewHistoryReadDtoBuilder WithReviewDate(DateTime reviewDate)
    {
        _historyReadDto.ReviewDate = reviewDate;
        return this;
    }

    public SalaryReviewHistoryReadDtoBuilder WithJustification(string justification)
    {
        _historyReadDto.Justification = justification;
        return this;
    }

    public SalaryReviewHistoryReadDtoBuilder WithOldSalary(decimal oldSalary)
    {
        _historyReadDto.OldSalary = oldSalary;
        return this;
    }

    public SalaryReviewHistoryReadDtoBuilder WithProposedSalary(decimal proposedSalary)
    {
        _historyReadDto.ProposedSalary = proposedSalary;
        return this;
    }

    public SalaryReviewHistoryReadDtoBuilder WithIsApproved(bool isApproved)
    {
        _historyReadDto.IsApproved = isApproved;
        return this;
    }

    public SalaryReviewHistoryReadDtoBuilder WithCurrency(string currency)
    {
        _historyReadDto.Currency = currency;
        return this;
    }

    public SalaryReviewHistoryReadDtoBuilder WithContractType(string contractType)
    {
        _historyReadDto.ContractType = contractType;
        return this;
    }

    public SalaryReviewHistoryReadDtoBuilder WithNewSalaryStartDate(DateTime? newSalaryStartDate)
    {
        _historyReadDto.NewSalaryStartDate = newSalaryStartDate;
        return this;
    }

    public SalaryReviewHistoryReadDtoBuilder WithSalaryReceivedDate(DateTime? salaryReceivedDate)
    {
        _historyReadDto.SalaryReceivedDate = salaryReceivedDate;
        return this;
    }

    public SalaryReviewHistoryReadDtoBuilder WithDivisionManagerComments(string comments)
    {
        _historyReadDto.DivisionManagerComments = comments;
        return this;
    }

    public SalaryReviewHistoryReadDtoBuilder WithTalentManagerComments(string comments)
    {
        _historyReadDto.TalentManagerComments = comments;
        return this;
    }

    public SalaryReviewHistoryReadDtoBuilder WithReviewResult(string reviewResult)
    {
        _historyReadDto.ReviewResult = reviewResult;
        return this;
    }

    public SalaryReviewHistoryReadDtoBuilder WithArchivedDate(DateTime archivedDate)
    {
        _historyReadDto.ArchivedDate = archivedDate;
        return this;
    }

    public SalaryReviewHistoryReadDto Build()
    {
        return _historyReadDto;
    }
}
