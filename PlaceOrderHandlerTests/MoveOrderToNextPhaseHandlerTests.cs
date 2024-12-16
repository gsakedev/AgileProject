using Moq;
using OrderManager.Application.Commands;
using OrderManager.Application.Handlers;
using OrderManager.Domain.Entities;
using OrderManager.Domain.Enums;
using OrderManager.Domain.Exceptions;
using OrderManager.Domain.Factories;
using OrderManager.Domain.Interfaces;
using OrderManager.Domain.States;

public class MoveOrderToNextPhaseHandlerTests
{
    private readonly Mock<IOrderRepository> _orderRepositoryMock;
    private readonly Mock<IDomainEventPublisher> _domainEventPublisherMock;
    private readonly MoveOrderToNextPhaseHandler _handler;
    private readonly CancellationToken cancellationToken = CancellationToken.None;
    

    public MoveOrderToNextPhaseHandlerTests()
    {
        _orderRepositoryMock = new Mock<IOrderRepository>();
        _domainEventPublisherMock = new Mock<IDomainEventPublisher>();
        _handler = new MoveOrderToNextPhaseHandler(_orderRepositoryMock.Object, _domainEventPublisherMock.Object);
    }

    [Fact]
    public async Task Handle_ValidOrder_MovesToNextState()
    {
        // Arrange
        var orderId = Guid.NewGuid();
        var staffId = Guid.NewGuid();

        var order = new Order
        {
            Id = orderId,
            State = OrderState.Pending,
        };

        _orderRepositoryMock.Setup(r => r.GetByIdAsync(orderId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(order);

        // Act
        await _handler.Handle(new MoveOrderToNextPhaseCommand { OrderId = orderId, StaffId = staffId }, CancellationToken.None);

        // Assert
        Assert.Equal(OrderState.Preparing, order.State);
        _orderRepositoryMock.Verify(r => r.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
        _domainEventPublisherMock.Verify(p => p.Publish(It.IsAny<IDomainEvent>(), It.IsAny<CancellationToken>()), Times.AtLeastOnce);
    }

    [Fact]
    public async Task Handle_OrderNotFound_ThrowsKeyNotFoundException()
    {
        // Arrange
        var orderId = Guid.NewGuid();

        _orderRepositoryMock.Setup(r => r.GetByIdAsync(orderId, It.IsAny<CancellationToken>()))
            .ReturnsAsync((Order)null);

        // Act & Assert
        await Assert.ThrowsAsync<KeyNotFoundException>(() =>
            _handler.Handle(new MoveOrderToNextPhaseCommand { OrderId = orderId, StaffId = Guid.NewGuid() }, CancellationToken.None));
    }

    [Fact]
    public async Task Handle_InvalidOrderState_ThrowsOrderStateException()
    {
        // Arrange
        var orderId = Guid.NewGuid();
        var staffId = Guid.NewGuid();

        var order = new Order
        {
            Id = orderId,
            State = OrderState.Delivered, // Final state
            
        };
        order.OrderStatesB = OrderStateFactory.CreateState(order);
        _orderRepositoryMock.Setup(r => r.GetByIdAsync(orderId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(order);

        // Act & Assert
        await Assert.ThrowsAsync<OrderStateException>(() =>
            _handler.Handle(new MoveOrderToNextPhaseCommand { OrderId = orderId, StaffId = staffId }, CancellationToken.None));
    }

    [Fact]
    public async Task Handle_PublishesDomainEvents()
    {
        // Arrange
        var orderId = Guid.NewGuid();
        var staffId = Guid.NewGuid();

        var order = new Order
        {
            Id = orderId,
            State = OrderState.Pending,
        };

        _orderRepositoryMock.Setup(r => r.GetByIdAsync(orderId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(order);

        // Act
        await _handler.Handle(new MoveOrderToNextPhaseCommand { OrderId = orderId, StaffId = staffId }, CancellationToken.None);

        // Assert
        _domainEventPublisherMock.Verify(p => p.Publish(It.IsAny<IDomainEvent>(), It.IsAny<CancellationToken>()), Times.AtLeastOnce);
    }

    [Fact]
    public async Task Handle_SavesChanges()
    {
        // Arrange
        var orderId = Guid.NewGuid();
        var staffId = Guid.NewGuid();

        var order = new Order
        {
            Id = orderId,
            State = OrderState.Pending,
        };

        _orderRepositoryMock.Setup(r => r.GetByIdAsync(orderId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(order);

        // Act
        var command = new MoveOrderToNextPhaseCommand { OrderId = orderId, StaffId = staffId };
        var handler = new MoveOrderToNextPhaseHandler(_orderRepositoryMock.Object, _domainEventPublisherMock.Object);
        await handler.Handle(command, cancellationToken);
        // Assert
        _orderRepositoryMock.Verify(r => r.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }
}
