namespace projects_menagment.Application.Exceptions;

public sealed class ConflictException(string message)
    : AppException("EMAIL_ALREADY_EXISTS", message)
{
}
