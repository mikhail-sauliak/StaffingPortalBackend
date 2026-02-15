using StaffingPortalBackend.Models;
using System;

public class PersonBuilder
{
    private readonly Person _person = new Person();

    public PersonBuilder()
    {
        WithDefaults();
    }

    public PersonBuilder WithDefaults()
    {
        _person.FirstName = "John";
        _person.LastName = "Doe";
        _person.Location = "New York";
        _person.AvailableFrom = DateTime.UtcNow;
        _person.TechStack = ".NET";
        _person.Stream = "Backend";
        _person.TMAware = "Aware";
        _person.Level = "Senior";
        _person.AssignmentExistsInGCP = true;
        _person.IsDismissed = false;
        _person.UserId = null;
        _person.DivisionManagerId = null;
        _person.TalentManagerId = null;
        _person.HeadId = null;
        _person.DeputyHeadId = null;
        _person.Comments = "No comments";
        _person.PlannedAssignment = "Planned Project";
        return this;
    }

    public PersonBuilder WithId(int id)
    {
        _person.Id = id;
        return this;
    }
    
    public PersonBuilder WithFirstName(string firstName)
    {
        _person.FirstName = firstName;
        return this;
    }

    public PersonBuilder WithLastName(string lastName)
    {
        _person.LastName = lastName;
        return this;
    }

    public PersonBuilder WithLocation(string location)
    {
        _person.Location = location;
        return this;
    }

    public PersonBuilder WithAvailableFrom(DateTime availableFrom)
    {
        _person.AvailableFrom = availableFrom;
        return this;
    }

    public PersonBuilder WithTechStack(string techStack)
    {
        _person.TechStack = techStack;
        return this;
    }

    public PersonBuilder WithStream(string stream)
    {
        _person.Stream = stream;
        return this;
    }

    public PersonBuilder WithTMAware(string tmAware)
    {
        _person.TMAware = tmAware;
        return this;
    }

    public PersonBuilder WithLevel(string level)
    {
        _person.Level = level;
        return this;
    }

    public PersonBuilder WithAssignmentExistsInGCP(bool existsInGcp)
    {
        _person.AssignmentExistsInGCP = existsInGcp;
        return this;
    }

    public PersonBuilder WithIsDismissed(bool isDismissed)
    {
        _person.IsDismissed = isDismissed;
        return this;
    }

    public PersonBuilder WithUserId(int? userId)
    {
        _person.UserId = userId;
        return this;
    }

    public PersonBuilder WithDivisionManagerId(int? divisionManagerId)
    {
        _person.DivisionManagerId = divisionManagerId;
        return this;
    }

    public PersonBuilder WithTalentManagerId(int? talentManagerId)
    {
        _person.TalentManagerId = talentManagerId;
        return this;
    }

    public PersonBuilder WithHeadId(int? headId)
    {
        _person.HeadId = headId;
        return this;
    }

    public PersonBuilder WithDeputyHeadId(int? deputyHeadId)
    {
        _person.DeputyHeadId = deputyHeadId;
        return this;
    }

    public PersonBuilder WithComments(string comments)
    {
        _person.Comments = comments;
        return this;
    }

    public PersonBuilder WithPlannedAssignment(string plannedAssignment)
    {
        _person.PlannedAssignment = plannedAssignment;
        return this;
    }

    public Person Build()
    {
        return _person;
    }
}
