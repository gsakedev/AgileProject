using OrderManager.Domain.Enums;
using OrderManager.Domain.Events;
using OrderManager.Domain.Exceptions;
using OrderManager.Domain.Interfaces;
using OrderManager.Domain.States;
using System.ComponentModel.DataAnnotations;

namespace OrderManager.Domain.Entities
{

    public class Order
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();
        public Guid CustomerId { get; set; }
        public List<OrderItem> Items { get; set; }
        public DeliveryOption DeliveryOption { get; set; }
        public string? SpecialInstructions { get; set; }
        public OrderState State { get; set; } = OrderState.Pending;
        public OrderStateBase OrderStatesB { get; set; }
        public bool IsCompleted { get; set; } = false;
        public DateTime OrderDate { get; set; } = DateTime.UtcNow;
        public DateTime? CompletedDate { get; set; }
        public decimal OrderTotalPrice { get; set; } 
        public string? OrderLocation { get; set; }

        private readonly List<IDomainEvent> _domainEvents = new();
        public IReadOnlyList<IDomainEvent> DomainEvents => _domainEvents.AsReadOnly();
        public void ClearDomainEvents() => _domainEvents.Clear();
        public void CalculateTotalPrice()
        {
            OrderTotalPrice = Items.Sum(i => i.Price * i.Quantity);
        }
        public void MoveToSpecificState(Guid staffId, bool delivered)
        {
            if (!DeliveryOption.Equals(DeliveryOption.Pickup))
            {
                if (OrderStatesB is OutForDeliveryState outForDeliveryState)
                {
                    if (delivered)
                    {
                        outForDeliveryState.MarkAsDelivered();
                    }
                    else if (!delivered)
                    {
                        outForDeliveryState.MarkAsUnableToDeliver();
                    }
                    else
                    {
                        throw new OrderStateException("No delivery status was given.");
                    }

                    _domainEvents.Add(new OrderStateChangedEvent
                    {
                        OrderId = Id,
                        FromState = OrderState.OutForDelivery.ToString(),
                        ToState = delivered ? OrderState.Delivered.ToString() : OrderState.UnableToDeliver.ToString(),
                        StaffId = staffId,
                        Timestamp = DateTime.UtcNow
                    });
                    _domainEvents.Add(new DeliveryStaffStatusChangeEvent
                    {
                        DeliveryStaffId = staffId.ToString(),
                        OrderId =  null , 
                        IsAvailable = true, 
                        Timestamp = DateTime.UtcNow
                    });
                }
                else
                {
                    throw new OrderStateException("This transition is only allowed from OutForDelivery state.");
                }
            }
            else
            {
                throw new OrderStateException("This transition is only allowed from OutForDelivery state.");
            }
        }

        public void MoveToNextState(Guid staffId)
        {
            var previousState = State;
            OrderStatesB.MoveToNext();

            if (State == OrderState.Delivered || State == OrderState.ReadyForPickup)
            {
                IsCompleted = true;
                CompletedDate = DateTime.UtcNow;
            }
            //TODO: for future assigmeng by distance
            //if (State == OrderState.ReadyForDelivery)
            //{
            //    _domainEvents.Add(new OrderReadyForDeliveryEvent(Id, DeliveryOption));
            //}
            _domainEvents.Add(new OrderStateChangedEvent
            {
                OrderId = Id,
                FromState = previousState.ToString(),
                ToState = State.ToString(),
                StaffId = staffId,
                Timestamp = DateTime.UtcNow
            });
        }
        public void CancelOrder(Guid staffId)
        {
            var previousState = State;
            OrderStatesB.Cancel();
            IsCompleted = false;
            CompletedDate = null;

            _domainEvents.Add(new OrderStateChangedEvent
            {
                OrderId = Id,
                FromState = previousState.ToString(),
                ToState = State.ToString(),
                StaffId = staffId,
                Timestamp = DateTime.UtcNow
            });
        }
    }

    public class OrderItem
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();
        public Guid MenuItemId { get; set; } // Reference to the menu item
        public MenuItem MenuItem { get; set; }
        public int Quantity { get; set; } = 1;
        public decimal Price { get; set; } = 0;
        public string? Name { get; set; }

        public decimal TotalPrice => Quantity * Price;

    }

}

