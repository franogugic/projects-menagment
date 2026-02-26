namespace projects_menagment.Application.Exceptions;

public sealed class NotFoundException(string message)
    : AppException("NOT_FOUND", message)
{
}
