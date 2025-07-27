namespace Scheduler.Dto.Base;

public class SimpleDto<T>
{
    public T Data { get; init; }

    public SimpleDto(T data)
    {
        Data = data;
    }
}