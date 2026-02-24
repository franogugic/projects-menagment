namespace projects_menagment.Application.Exceptions;

public sealed class ValidationException(string message)
    : AppException("VALIDATION_ERROR", message)
{
}
