using Microsoft.Extensions.Logging.Abstractions;
using Moq;
using projects_menagment.Application.Dtos.Auth;
using projects_menagment.Application.Exceptions;
using projects_menagment.Application.Interfaces.Repositories;
using projects_menagment.Application.Interfaces.Security;
using projects_menagment.Application.Services;
using projects_menagment.Domain.Entities;
using Xunit;

namespace projects_menagment.Application.Tests.Services;

public sealed class AuthServiceTests
{
    [Fact]
    public async Task SignupAsync_WhenRequestIsValid_CreatesUser()
    {
        var userRepository = new Mock<IUserRepository>();
        var refreshTokenRepository = new Mock<IRefreshTokenRepository>();
        var passwordHasher = new Mock<IPasswordHasher>();
        var authTokenService = new Mock<IAuthTokenService>();

        userRepository
            .Setup(x => x.ExistsByEmailAsync("john.doe@test.com", It.IsAny<CancellationToken>()))
            .ReturnsAsync(false);
        passwordHasher.Setup(x => x.HashPassword("Valid1!Pass")).Returns("hashed-password");

        User? createdUser = null;
        userRepository
            .Setup(x => x.AddAsync(It.IsAny<User>(), It.IsAny<CancellationToken>()))
            .Callback<User, CancellationToken>((u, _) => createdUser = u)
            .Returns(Task.CompletedTask);

        var sut = CreateSut(userRepository, refreshTokenRepository, passwordHasher, authTokenService);
        var request = new SignupRequestDto("John", "Doe", "  JOHN.DOE@TEST.COM ", "Valid1!Pass");

        var response = await sut.SignupAsync(request, CancellationToken.None);

        Assert.NotNull(createdUser);
        Assert.Equal(createdUser!.Id, response.UserId);
        Assert.Equal("john.doe@test.com", createdUser.Email);
        Assert.Equal("User created successfully.", response.Message);
        userRepository.Verify(x => x.AddAsync(It.IsAny<User>(), It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task SignupAsync_WhenEmailAlreadyExists_ThrowsConflictException()
    {
        var userRepository = new Mock<IUserRepository>();
        var refreshTokenRepository = new Mock<IRefreshTokenRepository>();
        var passwordHasher = new Mock<IPasswordHasher>();
        var authTokenService = new Mock<IAuthTokenService>();

        userRepository
            .Setup(x => x.ExistsByEmailAsync("john.doe@test.com", It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);

        var sut = CreateSut(userRepository, refreshTokenRepository, passwordHasher, authTokenService);
        var request = new SignupRequestDto("John", "Doe", "john.doe@test.com", "Valid1!Pass");

        await Assert.ThrowsAsync<ConflictException>(() => sut.SignupAsync(request, CancellationToken.None));
        userRepository.Verify(x => x.AddAsync(It.IsAny<User>(), It.IsAny<CancellationToken>()), Times.Never);
    }

    [Fact]
    public async Task SignupAsync_WhenPasswordIsNotComplex_ThrowsValidationException()
    {
        var sut = CreateSut(
            new Mock<IUserRepository>(),
            new Mock<IRefreshTokenRepository>(),
            new Mock<IPasswordHasher>(),
            new Mock<IAuthTokenService>());

        var request = new SignupRequestDto("John", "Doe", "john.doe@test.com", "weakpass");

        var ex = await Assert.ThrowsAsync<ValidationException>(() => sut.SignupAsync(request, CancellationToken.None));
        Assert.Equal("Password must contain at least one uppercase letter.", ex.Message);
    }

    [Fact]
    public async Task LoginAsync_WhenCredentialsAreValid_ReturnsTokens()
    {
        var userRepository = new Mock<IUserRepository>();
        var refreshTokenRepository = new Mock<IRefreshTokenRepository>();
        var passwordHasher = new Mock<IPasswordHasher>();
        var authTokenService = new Mock<IAuthTokenService>();

        var user = User.Create("John", "Doe", "john.doe@test.com", "hash");
        var now = DateTime.UtcNow;
        var generatedTokens = new AuthTokensDto("access", "refresh", now.AddMinutes(15), now.AddDays(7));

        userRepository
            .Setup(x => x.GetByEmailAsync("john.doe@test.com", It.IsAny<CancellationToken>()))
            .ReturnsAsync(user);
        passwordHasher.Setup(x => x.VerifyPassword("Valid1!Pass", user.PasswordHash)).Returns(true);
        authTokenService.Setup(x => x.GenerateTokens(user)).Returns(generatedTokens);

        var sut = CreateSut(userRepository, refreshTokenRepository, passwordHasher, authTokenService);
        var request = new LoginRequestDto(" john.doe@test.com ", "Valid1!Pass");

        var response = await sut.LoginAsync(request, CancellationToken.None);

        Assert.Equal("access", response.AccessToken);
        Assert.Equal("refresh", response.RefreshToken);
        refreshTokenRepository.Verify(x => x.AddAsync(It.IsAny<RefreshToken>(), It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task LoginAsync_WhenUserIsInactive_ThrowsForbiddenException()
    {
        var userRepository = new Mock<IUserRepository>();
        var refreshTokenRepository = new Mock<IRefreshTokenRepository>();
        var passwordHasher = new Mock<IPasswordHasher>();
        var authTokenService = new Mock<IAuthTokenService>();

        var user = User.Create("John", "Doe", "john.doe@test.com", "hash");
        SetUserIsActive(user, false);

        userRepository
            .Setup(x => x.GetByEmailAsync("john.doe@test.com", It.IsAny<CancellationToken>()))
            .ReturnsAsync(user);

        var sut = CreateSut(userRepository, refreshTokenRepository, passwordHasher, authTokenService);

        await Assert.ThrowsAsync<ForbiddenException>(() => sut.LoginAsync(
            new LoginRequestDto("john.doe@test.com", "Valid1!Pass"),
            CancellationToken.None));
    }

    [Fact]
    public async Task RefreshTokenAsync_WhenTokenIsValid_RotatesRefreshToken()
    {
        var userRepository = new Mock<IUserRepository>();
        var refreshTokenRepository = new Mock<IRefreshTokenRepository>();
        var passwordHasher = new Mock<IPasswordHasher>();
        var authTokenService = new Mock<IAuthTokenService>();

        var user = User.Create("John", "Doe", "john.doe@test.com", "hash");
        var existingRefresh = RefreshToken.Create(user.Id, "old-refresh", DateTime.UtcNow.AddHours(1));
        var now = DateTime.UtcNow;
        var generatedTokens = new AuthTokensDto("new-access", "new-refresh", now.AddMinutes(15), now.AddDays(7));

        refreshTokenRepository
            .Setup(x => x.GetByTokenAsync("old-refresh", It.IsAny<CancellationToken>()))
            .ReturnsAsync(existingRefresh);
        userRepository
            .Setup(x => x.GetByIdAsync(user.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(user);
        authTokenService.Setup(x => x.GenerateTokens(user)).Returns(generatedTokens);

        var sut = CreateSut(userRepository, refreshTokenRepository, passwordHasher, authTokenService);

        var response = await sut.RefreshTokenAsync(new RefreshTokenRequestDto("old-refresh"), CancellationToken.None);

        Assert.Equal("new-access", response.AccessToken);
        Assert.True(existingRefresh.IsRevoked);
        refreshTokenRepository.Verify(x => x.UpdateAsync(existingRefresh, It.IsAny<CancellationToken>()), Times.Once);
        refreshTokenRepository.Verify(x => x.AddAsync(It.IsAny<RefreshToken>(), It.IsAny<CancellationToken>()), Times.Once);
    }

    private static AuthService CreateSut(
        Mock<IUserRepository> userRepository,
        Mock<IRefreshTokenRepository> refreshTokenRepository,
        Mock<IPasswordHasher> passwordHasher,
        Mock<IAuthTokenService> authTokenService)
    {
        return new AuthService(
            userRepository.Object,
            refreshTokenRepository.Object,
            passwordHasher.Object,
            authTokenService.Object,
            NullLogger<AuthService>.Instance);
    }

    private static void SetUserIsActive(User user, bool value)
    {
        var field = typeof(User).GetField("<IsActive>k__BackingField", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic);
        field!.SetValue(user, value);
    }
}
