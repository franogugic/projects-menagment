using System.Net.Mail;
using System.Text.RegularExpressions;
using Microsoft.Extensions.Logging;
using projects_menagment.Application.Dtos.Auth;
using projects_menagment.Application.Exceptions;
using projects_menagment.Application.Interfaces.Repositories;
using projects_menagment.Application.Interfaces.Security;
using projects_menagment.Application.Interfaces.Services;
using projects_menagment.Domain.Entities;

namespace projects_menagment.Application.Services;

public sealed class AuthService(
    IUserRepository userRepository,
    IRefreshTokenRepository refreshTokenRepository,
    IPasswordHasher passwordHasher,
    IAuthTokenService authTokenService,
    ILogger<AuthService> logger) : IAuthService
{
    private static readonly Regex UppercaseRegex = new("[A-Z]");
    private static readonly Regex LowercaseRegex = new("[a-z]");
    private static readonly Regex DigitRegex = new("[0-9]");
    private static readonly Regex SpecialCharacterRegex = new("[^a-zA-Z0-9]");

    public async Task<SignupResponseDto> SignupAsync(SignupRequestDto request, CancellationToken cancellationToken)
    {
        var firstName = request.FirstName?.Trim() ?? string.Empty;
        var lastName = request.LastName?.Trim() ?? string.Empty;
        var email = request.Email?.Trim() ?? string.Empty;
        var password = request.Password ?? string.Empty;

        var validationError = ValidateSignupRequest(firstName, lastName, email, password);
        if (validationError is not null)
        {
            logger.LogWarning("Signup validation failed for email {Email}. Reason: {Reason}", email, validationError);
            throw new ValidationException(validationError);
        }

        var normalizedEmail = email.ToLowerInvariant();
        var userAlreadyExists = await userRepository.ExistsByEmailAsync(normalizedEmail, cancellationToken);
        if (userAlreadyExists)
        {
            logger.LogInformation("Signup rejected because email {Email} already exists", normalizedEmail);
            throw new ConflictException("A user with this email already exists.");
        }

        var user = User.Create(
            firstName,
            lastName,
            normalizedEmail,
            passwordHasher.HashPassword(password));

        await userRepository.AddAsync(user, cancellationToken);
        logger.LogInformation("User {UserId} created successfully for email {Email}", user.Id, normalizedEmail);

        return new SignupResponseDto(user.Id, "User created successfully.");
    }

    public async Task<LoginResponseDto> LoginAsync(LoginRequestDto request, CancellationToken cancellationToken)
    {
        var email = request.Email?.Trim() ?? string.Empty;
        var password = request.Password ?? string.Empty;

        if (string.IsNullOrWhiteSpace(email))
        {
            throw new ValidationException("Email is required.");
        }

        if (string.IsNullOrWhiteSpace(password))
        {
            throw new ValidationException("Password is required.");
        }

        var normalizedEmail = email.ToLowerInvariant();
        var user = await userRepository.GetByEmailAsync(normalizedEmail, cancellationToken);
        if (user is null)
        {
            logger.LogWarning("Login failed because user was not found for email {Email}", normalizedEmail);
            throw new UnauthorizedException("Invalid email or password.");
        }

        if (!user.IsActive)
        {
            logger.LogWarning("Login failed because user {UserId} is inactive", user.Id);
            throw new ForbiddenException("User account is inactive.");
        }

        if (!passwordHasher.VerifyPassword(password, user.PasswordHash))
        {
            logger.LogWarning("Login failed due to invalid credentials for user {UserId}", user.Id);
            throw new UnauthorizedException("Invalid email or password.");
        }

        var tokens = authTokenService.GenerateTokens(user);
        var refreshToken = RefreshToken.Create(user.Id, tokens.RefreshToken, tokens.RefreshTokenExpiresAt);
        await refreshTokenRepository.AddAsync(refreshToken, cancellationToken);

        logger.LogInformation("User {UserId} logged in successfully", user.Id);

        return new LoginResponseDto(
            tokens.AccessToken,
            tokens.RefreshToken,
            tokens.AccessTokenExpiresAt,
            tokens.RefreshTokenExpiresAt);
    }

    public async Task<LoginResponseDto> RefreshTokenAsync(RefreshTokenRequestDto request, CancellationToken cancellationToken)
    {
        var providedToken = request.RefreshToken?.Trim() ?? string.Empty;
        if (string.IsNullOrWhiteSpace(providedToken))
        {
            throw new ValidationException("Refresh token is required.");
        }

        var refreshToken = await refreshTokenRepository.GetByTokenAsync(providedToken, cancellationToken);
        if (refreshToken is null)
        {
            throw new UnauthorizedException("Invalid refresh token.");
        }

        if (refreshToken.IsRevoked || refreshToken.IsExpired(DateTime.UtcNow))
        {
            throw new UnauthorizedException("Refresh token is no longer valid.");
        }

        var user = await userRepository.GetByIdAsync(refreshToken.UserId, cancellationToken);
        if (user is null)
        {
            throw new UnauthorizedException("Refresh token user is invalid.");
        }

        if (!user.IsActive)
        {
            throw new ForbiddenException("User account is inactive.");
        }

        refreshToken.Revoke(DateTime.UtcNow);
        await refreshTokenRepository.UpdateAsync(refreshToken, cancellationToken);

        var tokens = authTokenService.GenerateTokens(user);
        var newRefreshToken = RefreshToken.Create(user.Id, tokens.RefreshToken, tokens.RefreshTokenExpiresAt);
        await refreshTokenRepository.AddAsync(newRefreshToken, cancellationToken);

        logger.LogInformation("Refresh token rotated successfully for user {UserId}", user.Id);

        return new LoginResponseDto(
            tokens.AccessToken,
            tokens.RefreshToken,
            tokens.AccessTokenExpiresAt,
            tokens.RefreshTokenExpiresAt);
    }

    private static string? ValidateSignupRequest(string firstName, string lastName, string email, string password)
    {
        if (string.IsNullOrWhiteSpace(firstName))
        {
            return "First name is required.";
        }

        if (firstName.Length > 100)
        {
            return "First name must not exceed 100 characters.";
        }

        if (string.IsNullOrWhiteSpace(lastName))
        {
            return "Last name is required.";
        }

        if (lastName.Length > 100)
        {
            return "Last name must not exceed 100 characters.";
        }

        if (string.IsNullOrWhiteSpace(email))
        {
            return "Email is required.";
        }

        if (email.Length > 256 || !MailAddress.TryCreate(email, out _))
        {
            return "Email format is invalid.";
        }

        if (string.IsNullOrWhiteSpace(password))
        {
            return "Password is required.";
        }

        if (password.Length < 8)
        {
            return "Password must be at least 8 characters long.";
        }

        if (!UppercaseRegex.IsMatch(password))
        {
            return "Password must contain at least one uppercase letter.";
        }

        if (!LowercaseRegex.IsMatch(password))
        {
            return "Password must contain at least one lowercase letter.";
        }

        if (!DigitRegex.IsMatch(password))
        {
            return "Password must contain at least one number.";
        }

        if (!SpecialCharacterRegex.IsMatch(password))
        {
            return "Password must contain at least one special character.";
        }

        return null;
    }
}
