namespace CQRS.Core.Exceptions
{
    public class EventProducerException : Exception
    {
        public EventProducerException(string message) : base(message)
        {

        }
    }
}
