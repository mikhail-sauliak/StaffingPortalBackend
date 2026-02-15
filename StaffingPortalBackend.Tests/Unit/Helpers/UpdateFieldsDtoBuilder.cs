using StaffingPortalBackend.DTO;
using System;

public class UpdateFieldsDtoBuilder
{
    private UpdateFieldsDto _dto;

    public UpdateFieldsDtoBuilder()
    {
        _dto = new UpdateFieldsDto();
    }

    public UpdateFieldsDtoBuilder WithDefaults()
    {
        _dto = new UpdateFieldsDto
        {
            ProposedSalary = 60000m,
            Currency = "USD",
            ContractType = "Full-time",
            NewSalaryStartDate = DateTime.Now.AddDays(30),
            SalaryReceivedDate = DateTime.Now.AddDays(60),
            DivisionManagerComments = "Reviewed",
            IsApproved = true,
            ApprovedByDM = true
        };
        return this;
    }

    public UpdateFieldsDtoBuilder WithProposedSalary(decimal proposedSalary)
    {
        _dto.ProposedSalary = proposedSalary;
        return this;
    }

    public UpdateFieldsDtoBuilder WithCurrency(string currency)
    {
        _dto.Currency = currency;
        return this;
    }

    public UpdateFieldsDtoBuilder WithContractType(string contractType)
    {
        _dto.ContractType = contractType;
        return this;
    }

    public UpdateFieldsDtoBuilder WithNewSalaryStartDate(DateTime? newSalaryStartDate)
    {
        if (newSalaryStartDate.HasValue)
        {
            _dto.NewSalaryStartDate = newSalaryStartDate.Value;
        }
        return this;
    }

    public UpdateFieldsDtoBuilder WithSalaryReceivedDate(DateTime? salaryReceivedDate)
    {
        if (salaryReceivedDate.HasValue)
        {
            _dto.SalaryReceivedDate = salaryReceivedDate.Value;
        }
        return this;
    }

    public UpdateFieldsDtoBuilder WithDivisionManagerComments(string comments)
    {
        _dto.DivisionManagerComments = comments;
        return this;
    }

    public UpdateFieldsDtoBuilder WithIsApproved(bool? isApproved)
    {
        _dto.IsApproved = isApproved;
        return this;
    }

    public UpdateFieldsDtoBuilder WithApprovedByDM(bool? approvedByDM)
    {
        _dto.ApprovedByDM = approvedByDM;
        return this;
    }

    public UpdateFieldsDto Build()
    {
        return _dto;
    }
}
