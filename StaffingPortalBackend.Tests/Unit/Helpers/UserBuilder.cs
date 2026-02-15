using StaffingPortalBackend.Models;
using System;
using System.Collections.Generic;

public class UserBuilder
{
    private readonly User _user = new User();

    public UserBuilder()
    {
        WithDefaults();
    }

    public UserBuilder WithDefaults()
    {
        _user.Username = "DefaultUsername";
        _user.PasswordHash = GeneratePasswordHash("defaultPassword");
        _user.PasswordSalt = GenerateSalt();
        _user.Role = "TalentManager";
        _user.Stream = "DefaultStream";
        _user.PersonId = null; 
        return this;
    }

    public UserBuilder WithId(int id)
    {
        _user.Id = id;
        return this;
    }

    public UserBuilder WithUsername(string username)
    {
        _user.Username = username;
        return this;
    }

    public UserBuilder WithPassword(string password)
    {
        _user.PasswordSalt = GenerateSalt();
        _user.PasswordHash = GeneratePasswordHash(password);
        return this;
    }

    public UserBuilder WithRole(string role)
    {
        _user.Role = role;
        return this;
    }

    public UserBuilder WithPersonId(int? personId)
    {
        _user.PersonId = personId;
        return this;
    }

    public UserBuilder WithStream(string stream)
    {
        _user.Stream = stream;
        return this;
    }

    private byte[] GenerateSalt()
    {
        using var rng = new System.Security.Cryptography.RNGCryptoServiceProvider();
        var salt = new byte[16];
        rng.GetBytes(salt);
        return salt;
    }

    private byte[] GeneratePasswordHash(string password)
    {
        using var algorithm = new System.Security.Cryptography.Rfc2898DeriveBytes(password, GenerateSalt(), 10000);
        return algorithm.GetBytes(32); // 256-bit hash
    }

    public User Build()
    {
        return _user;
    }
}
