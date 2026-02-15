using StaffingPortalBackend.DTO;
using System;

public class SalaryReviewUpdateDtoBuilder
{
    private readonly SalaryReviewUpdateDto _dto = new SalaryReviewUpdateDto();

    public SalaryReviewUpdateDtoBuilder WithDefaults()
    {
        _dto.PersonId = 1;
        _dto.ReviewDate = DateTime.UtcNow;
        _dto.Justification = "Updated Performance";
        _dto.OldSalary = 55000m;
        _dto.ProposedSalary = 60000m;
        _dto.IsApproved = true;
        _dto.Currency = "USD";
        _dto.ContractType = "Full-time";
        _dto.NewSalaryStartDate = DateTime.UtcNow.AddDays(30);
        _dto.SalaryReceivedDate = DateTime.UtcNow.AddDays(60);
        _dto.DivisionManagerComments = "Updated comments";
        _dto.TalentManagerComments = "Updated approval";
        _dto.ReviewResult = "Approved";
        _dto.ApprovedByDM = true;
        return this;
    }

    public SalaryReviewUpdateDtoBuilder WithPersonId(int personId)
    {
        _dto.PersonId = personId;
        return this;
    }

    public SalaryReviewUpdateDtoBuilder WithReviewDate(DateTime reviewDate)
    {
        _dto.ReviewDate = reviewDate;
        return this;
    }

    public SalaryReviewUpdateDtoBuilder WithJustification(string justification)
    {
        _dto.Justification = justification;
        return this;
    }

    public SalaryReviewUpdateDtoBuilder WithOldSalary(decimal oldSalary)
    {
        _dto.OldSalary = oldSalary;
        return this;
    }

    public SalaryReviewUpdateDtoBuilder WithProposedSalary(decimal proposedSalary)
    {
        _dto.ProposedSalary = proposedSalary;
        return this;
    }

    public SalaryReviewUpdateDtoBuilder WithIsApproved(bool isApproved)
    {
        _dto.IsApproved = isApproved;
        return this;
    }

    public SalaryReviewUpdateDtoBuilder WithCurrency(string currency)
    {
        _dto.Currency = currency;
        return this;
    }

    public SalaryReviewUpdateDtoBuilder WithContractType(string contractType)
    {
        _dto.ContractType = contractType;
        return this;
    }

    public SalaryReviewUpdateDtoBuilder WithNewSalaryStartDate(DateTime? newSalaryStartDate)
    {
        _dto.NewSalaryStartDate = newSalaryStartDate;
        return this;
    }

    public SalaryReviewUpdateDtoBuilder WithSalaryReceivedDate(DateTime? salaryReceivedDate)
    {
        _dto.SalaryReceivedDate = salaryReceivedDate;
        return this;
    }

    public SalaryReviewUpdateDtoBuilder WithDivisionManagerComments(string comments)
    {
        _dto.DivisionManagerComments = comments;
        return this;
    }

    public SalaryReviewUpdateDtoBuilder WithTalentManagerComments(string comments)
    {
        _dto.TalentManagerComments = comments;
        return this;
    }

    public SalaryReviewUpdateDtoBuilder WithReviewResult(string reviewResult)
    {
        _dto.ReviewResult = reviewResult;
        return this;
    }

    public SalaryReviewUpdateDtoBuilder WithApprovedByDM(bool approvedByDM)
    {
        _dto.ApprovedByDM = approvedByDM;
        return this;
    }

    public SalaryReviewUpdateDto Build()
    {
        return _dto;
    }
}
