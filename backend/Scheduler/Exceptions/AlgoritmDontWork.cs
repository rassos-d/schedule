namespace Scheduler.Exceptions;

public class AlgorithmDontWorkException : BaseException
{
    private readonly string message;

    public AlgorithmDontWorkException(string message)
    {
        this.message = message;
    }
    
    public override string GetErrorData()
    {
        return message;
    }
}