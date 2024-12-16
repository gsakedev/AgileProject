namespace OrderManager.Domain.Exceptions
{

    public class OrderStateException : Exception
    {
        public OrderStateException(string message) : base(message) { }
    }

    public class ValidationException : Exception
    {
        public ValidationException(string message) : base(message) { }

    }
    public class DeliverStaffOrderException : Exception
    {
        public DeliverStaffOrderException(string message) : base(message) { }

    }
}
