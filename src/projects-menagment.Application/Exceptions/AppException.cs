namespace projects_menagment.Application.Exceptions;

public abstract class AppException(string code, string message) : Exception(message)
{
    public string Code { get; } = code;
}
