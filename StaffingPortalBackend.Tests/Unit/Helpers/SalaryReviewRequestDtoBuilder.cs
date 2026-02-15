using StaffingPortalBackend.DTO;
using System;

public class SalaryReviewRequestDtoBuilder
{
    private SalaryReviewRequestDto _requestDto;

    public SalaryReviewRequestDtoBuilder()
    {
        _requestDto = new SalaryReviewRequestDto();
    }

    public SalaryReviewRequestDtoBuilder WithDefaults()
    {
        _requestDto = new SalaryReviewRequestDto
        {
            PersonId = 1,
            Justification = "Performance improvement",
            ReviewDate = DateTime.UtcNow,
            NewSalaryStartDate = DateTime.UtcNow.AddDays(30),
            SalaryReceivedDate = DateTime.UtcNow.AddDays(60),
            TalentManagerComments = "Excellent progress",
            DivisionManagerComments = "Aprooved",
            ContractType = "B2B",
            Currency = "USD",
            OldSalary = 50000,
            NewSalary = 55000
        };

        return this;
    }

    public SalaryReviewRequestDtoBuilder WithPersonId(int personId)
    {
        _requestDto.PersonId = personId;
        return this;
    }

    public SalaryReviewRequestDtoBuilder WithJustification(string justification)
    {
        _requestDto.Justification = justification;
        return this;
    }

    public SalaryReviewRequestDtoBuilder WithReviewDate(DateTime reviewDate)
    {
        _requestDto.ReviewDate = reviewDate;
        return this;
    }

    public SalaryReviewRequestDtoBuilder WithNewSalaryStartDate(DateTime newSalaryStartDate)
    {
        _requestDto.NewSalaryStartDate = newSalaryStartDate;
        return this;
    }

    public SalaryReviewRequestDtoBuilder WithSalaryReceivedDate(DateTime salaryReceivedDate)
    {
        _requestDto.SalaryReceivedDate = salaryReceivedDate;
        return this;
    }

    public SalaryReviewRequestDtoBuilder WithTalentManagerComments(string comments)
    {
        _requestDto.TalentManagerComments = comments;
        return this;
    }

    public SalaryReviewRequestDtoBuilder WithDivisionManagerComments(string comments)
    {
        _requestDto.DivisionManagerComments = comments;
        return this;
    }

    public SalaryReviewRequestDtoBuilder WithStatus(string status)
    {
        _requestDto.Status = status;
        return this;
    }

    public SalaryReviewRequestDtoBuilder WithCurrency(string currency)
    {
        _requestDto.Currency = currency;
        return this;
    }

    public SalaryReviewRequestDtoBuilder WithContractType(string contractType)
    {
        _requestDto.ContractType = contractType;
        return this;
    }

    public SalaryReviewRequestDtoBuilder WithOldSalary(decimal oldSalary)
    {
        _requestDto.OldSalary = oldSalary;
        return this;
    }

    public SalaryReviewRequestDtoBuilder WithNewSalary(decimal newSalary)
    {
        _requestDto.NewSalary = newSalary;
        return this;
    }

    public SalaryReviewRequestDto Build()
    {
        return _requestDto;
    }
}
