namespace OrderManager.Domain.Enums
{
    public enum OrderState
    {
        Pending,
        Preparing,
        ReadyForPickup,
        ReadyForDelivery,
        OutForDelivery,
        Delivered,
        UnableToDeliver,
        Cancelled
    }
}
