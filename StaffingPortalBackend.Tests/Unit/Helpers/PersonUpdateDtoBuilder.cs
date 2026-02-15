using StaffingPortalBackend.DTO;
using System;
using System.Collections.Generic;

public class PersonUpdateDtoBuilder
{
    private readonly PersonUpdateDto _personUpdateDto = new PersonUpdateDto();

    public PersonUpdateDtoBuilder WithDefaults()
    {
        _personUpdateDto.FirstName = "DefaultFirstName";
        _personUpdateDto.LastName = "DefaultLastName";
        _personUpdateDto.Location = "DefaultLocation";
        _personUpdateDto.DivisionManagerId = 1;
        _personUpdateDto.TalentManagerId = 2;
        _personUpdateDto.AvailableFrom = DateTime.UtcNow;
        _personUpdateDto.TechStack = "DefaultTechStack";
        _personUpdateDto.Stream = "DefaultStream";
        _personUpdateDto.TMAware = "DefaultTMAware";
        _personUpdateDto.Level = "DefaultLevel";
        _personUpdateDto.IsDismissed = false;
        _personUpdateDto.AssignmentExistsInGCP = true;
        _personUpdateDto.UserId = 100;
        _personUpdateDto.HeadId = 3;
        _personUpdateDto.DeputyHeadId = 4;
        _personUpdateDto.Comments = "Default Comments";
        _personUpdateDto.ProjectCandidateIds = new List<int> { 1, 2, 3 };
        return this;
    }

    public PersonUpdateDtoBuilder WithFirstName(string firstName)
    {
        _personUpdateDto.FirstName = firstName;
        return this;
    }

    public PersonUpdateDtoBuilder WithLastName(string lastName)
    {
        _personUpdateDto.LastName = lastName;
        return this;
    }

    public PersonUpdateDtoBuilder WithLocation(string location)
    {
        _personUpdateDto.Location = location;
        return this;
    }

    public PersonUpdateDtoBuilder WithDivisionManagerId(int? divisionManagerId)
    {
        _personUpdateDto.DivisionManagerId = divisionManagerId;
        return this;
    }

    public PersonUpdateDtoBuilder WithTalentManagerId(int? talentManagerId)
    {
        _personUpdateDto.TalentManagerId = talentManagerId;
        return this;
    }

    public PersonUpdateDtoBuilder WithAvailableFrom(DateTime availableFrom)
    {
        _personUpdateDto.AvailableFrom = availableFrom;
        return this;
    }

    public PersonUpdateDtoBuilder WithTechStack(string techStack)
    {
        _personUpdateDto.TechStack = techStack;
        return this;
    }

    public PersonUpdateDtoBuilder WithStream(string stream)
    {
        _personUpdateDto.Stream = stream;
        return this;
    }

    public PersonUpdateDtoBuilder WithTMAware(string tmAware)
    {
        _personUpdateDto.TMAware = tmAware;
        return this;
    }

    public PersonUpdateDtoBuilder WithLevel(string level)
    {
        _personUpdateDto.Level = level;
        return this;
    }

    public PersonUpdateDtoBuilder WithIsDismissed(bool isDismissed)
    {
        _personUpdateDto.IsDismissed = isDismissed;
        return this;
    }

    public PersonUpdateDtoBuilder WithAssignmentExistsInGCP(bool assignmentExistsInGCP)
    {
        _personUpdateDto.AssignmentExistsInGCP = assignmentExistsInGCP;
        return this;
    }

    public PersonUpdateDtoBuilder WithUserId(int? userId)
    {
        _personUpdateDto.UserId = userId;
        return this;
    }

    public PersonUpdateDtoBuilder WithHeadId(int? headId)
    {
        _personUpdateDto.HeadId = headId;
        return this;
    }

    public PersonUpdateDtoBuilder WithDeputyHeadId(int? deputyHeadId)
    {
        _personUpdateDto.DeputyHeadId = deputyHeadId;
        return this;
    }

    public PersonUpdateDtoBuilder WithComments(string comments)
    {
        _personUpdateDto.Comments = comments;
        return this;
    }

    public PersonUpdateDtoBuilder WithProjectCandidateIds(ICollection<int> projectCandidateIds)
    {
        _personUpdateDto.ProjectCandidateIds = projectCandidateIds;
        return this;
    }

    public PersonUpdateDto Build()
    {
        return _personUpdateDto;
    }
}
