using StaffingPortalBackend.Models;
using System;

public class SalaryReviewBuilder
{
    private readonly SalaryReview _review = new SalaryReview();
    private static int _nextId = 1;

    public SalaryReviewBuilder WithDefaults()
    {
        _review.Id = _nextId++;
        _review.PersonId = 1; // Default person ID
        _review.ReviewDate = DateTime.UtcNow;
        _review.Justification = "Performance";
        _review.OldSalary = 50000m;
        _review.ProposedSalary = 55000m;
        _review.IsApproved = false;
        _review.Currency = "USD";
        _review.ContractType = "Full-time";
        _review.NewSalaryStartDate = null;
        _review.SalaryReceivedDate = null;
        _review.DivisionManagerComments = "Good performance";
        _review.TalentManagerComments = "Agree with the review";
        _review.ReviewResult = "Positive";
        _review.ApprovedByDM = false;
        return this;
    }

    public SalaryReviewBuilder WithId(int Id)
    {
        _review.Id = Id;
        return this;
    }

    public SalaryReviewBuilder WithPersonId(int personId)
    {
        _review.PersonId = personId;
        return this;
    }

    public SalaryReviewBuilder WithReviewDate(DateTime reviewDate)
    {
        _review.ReviewDate = reviewDate;
        return this;
    }

    public SalaryReviewBuilder WithJustification(string justification)
    {
        _review.Justification = justification;
        return this;
    }

    public SalaryReviewBuilder WithOldSalary(decimal oldSalary)
    {
        _review.OldSalary = oldSalary;
        return this;
    }

    public SalaryReviewBuilder WithProposedSalary(decimal proposedSalary)
    {
        _review.ProposedSalary = proposedSalary;
        return this;
    }

    public SalaryReviewBuilder WithIsApproved(bool isApproved)
    {
        _review.IsApproved = isApproved;
        return this;
    }

    public SalaryReviewBuilder WithCurrency(string currency)
    {
        _review.Currency = currency;
        return this;
    }

    public SalaryReviewBuilder WithContractType(string contractType)
    {
        _review.ContractType = contractType;
        return this;
    }

    public SalaryReviewBuilder WithNewSalaryStartDate(DateTime? newSalaryStartDate)
    {
        _review.NewSalaryStartDate = newSalaryStartDate;
        return this;
    }

    public SalaryReviewBuilder WithSalaryReceivedDate(DateTime? salaryReceivedDate)
    {
        _review.SalaryReceivedDate = salaryReceivedDate;
        return this;
    }

    public SalaryReviewBuilder WithDivisionManagerComments(string comments)
    {
        _review.DivisionManagerComments = comments;
        return this;
    }

    public SalaryReviewBuilder WithTalentManagerComments(string comments)
    {
        _review.TalentManagerComments = comments;
        return this;
    }

    public SalaryReviewBuilder WithReviewResult(string reviewResult)
    {
        _review.ReviewResult = reviewResult;
        return this;
    }

    public SalaryReviewBuilder WithApprovedByDM(bool approvedByDM)
    {
        _review.ApprovedByDM = approvedByDM;
        return this;
    }

    public static void ResetCounter()
    {
        _nextId = 1;
    }

    public SalaryReview Build()
    {
        return _review;
    }
}
