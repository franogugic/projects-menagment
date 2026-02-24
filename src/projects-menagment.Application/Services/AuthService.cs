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
    IPasswordHasher passwordHasher,
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

        var validationError = ValidateRequest(firstName, lastName, email, password);
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

    private static string? ValidateRequest(string firstName, string lastName, string email, string password)
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
