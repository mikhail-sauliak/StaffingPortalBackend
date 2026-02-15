using StaffingPortalBackend.DTO;
using System;

public class PersonCreateDtoBuilder
{
    private readonly PersonCreateDto _personCreateDto = new PersonCreateDto();

    public PersonCreateDtoBuilder()
    {
        WithDefaults();
    }

    public PersonCreateDtoBuilder WithDefaults()
    {
        _personCreateDto.FirstName = "John";
        _personCreateDto.LastName = "Doe";
        _personCreateDto.Location = "New York";
        _personCreateDto.DivisionManagerId = 1;
        _personCreateDto.TalentManagerId = 2;
        _personCreateDto.AvailableFrom = DateTime.UtcNow;
        _personCreateDto.TechStack = ".NET";
        _personCreateDto.Stream = "Backend";
        _personCreateDto.TMAware = "Aware";
        _personCreateDto.Level = "Senior";
        _personCreateDto.IsDismissed = false;
        _personCreateDto.AssignmentExistsInGCP = true;
        _personCreateDto.UserId = 100;
        _personCreateDto.HeadId = 3;
        _personCreateDto.DeputyHeadId = 4;
        _personCreateDto.Comments = "No comments";
        return this;
    }

    public PersonCreateDtoBuilder WithFirstName(string firstName)
    {
        _personCreateDto.FirstName = firstName;
        return this;
    }

    public PersonCreateDtoBuilder WithLastName(string lastName)
    {
        _personCreateDto.LastName = lastName;
        return this;
    }

    public PersonCreateDtoBuilder WithLocation(string location)
    {
        _personCreateDto.Location = location;
        return this;
    }

    public PersonCreateDtoBuilder WithDivisionManagerId(int? divisionManagerId)
    {
        _personCreateDto.DivisionManagerId = divisionManagerId;
        return this;
    }

    public PersonCreateDtoBuilder WithTalentManagerId(int? talentManagerId)
    {
        _personCreateDto.TalentManagerId = talentManagerId;
        return this;
    }

    public PersonCreateDtoBuilder WithAvailableFrom(DateTime availableFrom)
    {
        _personCreateDto.AvailableFrom = availableFrom;
        return this;
    }

    public PersonCreateDtoBuilder WithTechStack(string techStack)
    {
        _personCreateDto.TechStack = techStack;
        return this;
    }

    public PersonCreateDtoBuilder WithStream(string stream)
    {
        _personCreateDto.Stream = stream;
        return this;
    }

    public PersonCreateDtoBuilder WithTMAware(string tmAware)
    {
        _personCreateDto.TMAware = tmAware;
        return this;
    }

    public PersonCreateDtoBuilder WithLevel(string level)
    {
        _personCreateDto.Level = level;
        return this;
    }

    public PersonCreateDtoBuilder WithIsDismissed(bool isDismissed)
    {
        _personCreateDto.IsDismissed = isDismissed;
        return this;
    }

    public PersonCreateDtoBuilder WithAssignmentExistsInGCP(bool assignmentExistsInGCP)
    {
        _personCreateDto.AssignmentExistsInGCP = assignmentExistsInGCP;
        return this;
    }

    public PersonCreateDtoBuilder WithUserId(int? userId)
    {
        _personCreateDto.UserId = userId;
        return this;
    }

    public PersonCreateDtoBuilder WithHeadId(int? headId)
    {
        _personCreateDto.HeadId = headId;
        return this;
    }

    public PersonCreateDtoBuilder WithDeputyHeadId(int? deputyHeadId)
    {
        _personCreateDto.DeputyHeadId = deputyHeadId;
        return this;
    }

    public PersonCreateDtoBuilder WithComments(string comments)
    {
        _personCreateDto.Comments = comments;
        return this;
    }

    public PersonCreateDto Build()
    {
        return _personCreateDto;
    }
}
