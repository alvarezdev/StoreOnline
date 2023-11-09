using System.Runtime.Serialization;
namespace StoreOnline.Domain.Exceptions;

[Serializable]
public class MinimumModelException : CoreBusinessException
{
    public MinimumModelException()
    {
    }

    public MinimumModelException(string message) : base(message)
    {
    }

    public MinimumModelException(string message, Exception inner) : base(message, inner)
    {
    }

    protected MinimumModelException(SerializationInfo info, StreamingContext context) : base(info, context)
    {
    }
}
