using StaffingPortalBackend.DTO;
using System;

public class SalaryReviewCreateDtoBuilder
{
    private readonly SalaryReviewCreateDto _dto = new SalaryReviewCreateDto();

    public SalaryReviewCreateDtoBuilder WithDefaults()
    {
        _dto.PersonId = 1;
        _dto.ReviewDate = DateTime.UtcNow;
        _dto.Justification = "Excellent Performance";
        _dto.OldSalary = 50000m;
        _dto.ProposedSalary = 55000m;
        _dto.IsApproved = false;
        _dto.Currency = "USD";
        _dto.ContractType = "Full-time";
        _dto.NewSalaryStartDate = DateTime.UtcNow.AddDays(30);
        _dto.SalaryReceivedDate = null;
        _dto.DivisionManagerComments = "Well deserved";
        _dto.TalentManagerComments = "Support this decision";
        _dto.ReviewResult = "Approved";
        return this;
    }

    public SalaryReviewCreateDtoBuilder WithPersonId(int personId)
    {
        _dto.PersonId = personId;
        return this;
    }

    public SalaryReviewCreateDtoBuilder WithReviewDate(DateTime reviewDate)
    {
        _dto.ReviewDate = reviewDate;
        return this;
    }

    public SalaryReviewCreateDtoBuilder WithJustification(string justification)
    {
        _dto.Justification = justification;
        return this;
    }

    public SalaryReviewCreateDtoBuilder WithOldSalary(decimal oldSalary)
    {
        _dto.OldSalary = oldSalary;
        return this;
    }

    public SalaryReviewCreateDtoBuilder WithProposedSalary(decimal proposedSalary)
    {
        _dto.ProposedSalary = proposedSalary;
        return this;
    }

    public SalaryReviewCreateDtoBuilder WithIsApproved(bool isApproved)
    {
        _dto.IsApproved = isApproved;
        return this;
    }

    public SalaryReviewCreateDtoBuilder WithCurrency(string currency)
    {
        _dto.Currency = currency;
        return this;
    }

    public SalaryReviewCreateDtoBuilder WithContractType(string contractType)
    {
        _dto.ContractType = contractType;
        return this;
    }

    public SalaryReviewCreateDtoBuilder WithNewSalaryStartDate(DateTime? newSalaryStartDate)
    {
        _dto.NewSalaryStartDate = newSalaryStartDate;
        return this;
    }

    public SalaryReviewCreateDtoBuilder WithSalaryReceivedDate(DateTime? salaryReceivedDate)
    {
        _dto.SalaryReceivedDate = salaryReceivedDate;
        return this;
    }

    public SalaryReviewCreateDtoBuilder WithDivisionManagerComments(string comments)
    {
        _dto.DivisionManagerComments = comments;
        return this;
    }

    public SalaryReviewCreateDtoBuilder WithTalentManagerComments(string comments)
    {
        _dto.TalentManagerComments = comments;
        return this;
    }

    public SalaryReviewCreateDtoBuilder WithReviewResult(string reviewResult)
    {
        _dto.ReviewResult = reviewResult;
        return this;
    }

    public SalaryReviewCreateDto Build()
    {
        return _dto;
    }
}
