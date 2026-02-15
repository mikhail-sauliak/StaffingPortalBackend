using StaffingPortalBackend.Models;
using System;

public class SalaryReviewRequestBuilder
{
    private SalaryReviewRequest _request;

    public SalaryReviewRequestBuilder()
    {
        _request = new SalaryReviewRequest();
    }

    public SalaryReviewRequestBuilder WithDefaults()
    {
        _request = new SalaryReviewRequest
        {
            PersonId = 1,
            Justification = "Performance improvement",
            ReviewDate = DateTime.UtcNow,
            NewSalaryStartDate = DateTime.UtcNow.AddDays(30),
            SalaryReceivedDate = DateTime.UtcNow.AddDays(60),
            TalentManagerComments = "Excellent progress",
            DivisionManagerComments = null,
            Status = "Pending",
            Currency = "USD",
            ContractType = "Full-time",
            OldSalary = 50000,
            NewSalary = 55000
        };

        return this;
    }

    public SalaryReviewRequestBuilder WithPersonId(int personId)
    {
        _request.PersonId = personId;
        return this;
    }

    public SalaryReviewRequestBuilder WithJustification(string justification)
    {
        _request.Justification = justification;
        return this;
    }

    public SalaryReviewRequestBuilder WithReviewDate(DateTime reviewDate)
    {
        _request.ReviewDate = reviewDate;
        return this;
    }

    public SalaryReviewRequestBuilder WithNewSalaryStartDate(DateTime newSalaryStartDate)
    {
        _request.NewSalaryStartDate = newSalaryStartDate;
        return this;
    }

    public SalaryReviewRequestBuilder WithSalaryReceivedDate(DateTime salaryReceivedDate)
    {
        _request.SalaryReceivedDate = salaryReceivedDate;
        return this;
    }

    public SalaryReviewRequestBuilder WithTalentManagerComments(string comments)
    {
        _request.TalentManagerComments = comments;
        return this;
    }

    public SalaryReviewRequestBuilder WithDivisionManagerComments(string comments)
    {
        _request.DivisionManagerComments = comments;
        return this;
    }

    public SalaryReviewRequestBuilder WithStatus(string status)
    {
        _request.Status = status;
        return this;
    }

    public SalaryReviewRequestBuilder WithCurrency(string currency)
    {
        _request.Currency = currency;
        return this;
    }

    public SalaryReviewRequestBuilder WithContractType(string contractType)
    {
        _request.ContractType = contractType;
        return this;
    }

    public SalaryReviewRequestBuilder WithOldSalary(decimal oldSalary)
    {
        _request.OldSalary = oldSalary;
        return this;
    }

    public SalaryReviewRequestBuilder WithNewSalary(decimal newSalary)
    {
        _request.NewSalary = newSalary;
        return this;
    }

    public SalaryReviewRequest Build()
    {
        return _request;
    }
}
