namespace projects_menagment.Application.Exceptions;

public sealed class ForbiddenException(string message)
    : AppException("FORBIDDEN", message)
{
}
