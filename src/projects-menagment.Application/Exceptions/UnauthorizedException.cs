namespace projects_menagment.Application.Exceptions;

public sealed class UnauthorizedException(string message)
    : AppException("UNAUTHORIZED", message)
{
}
