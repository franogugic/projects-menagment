using projects_menagment.Domain.Entities;
using Xunit;

namespace projects_menagment.Application.Tests.Domain;

public sealed class UserTests
{
    [Fact]
    public void Create_WhenValidInput_SetsRequiredFields()
    {
        var user = User.Create("John", "Smith", "john@test.com", "hash");

        Assert.NotEqual(Guid.Empty, user.Id);
        Assert.Equal("John", user.FirstName);
        Assert.Equal("Smith", user.LastName);
        Assert.Equal("john@test.com", user.Email);
        Assert.Equal("hash", user.PasswordHash);
        Assert.True(user.IsActive);
        Assert.True(user.CreatedAt <= DateTime.UtcNow);
    }

    [Fact]
    public void Create_WhenFirstNameIsMissing_ThrowsArgumentException()
    {
        var ex = Assert.Throws<ArgumentException>(() => User.Create("", "Smith", "john@test.com", "hash"));
        Assert.Equal("firstName", ex.ParamName);
    }

    [Fact]
    public void Create_WhenPasswordHashIsMissing_ThrowsArgumentException()
    {
        var ex = Assert.Throws<ArgumentException>(() => User.Create("John", "Smith", "john@test.com", ""));
        Assert.Equal("passwordHash", ex.ParamName);
    }
}
