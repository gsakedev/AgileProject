using Moq;
using OrderManager.Application.Commands;
using OrderManager.Application.Handlers;
using OrderManager.Domain.Entities;
using OrderManager.Domain.Enums;
using OrderManager.Domain.Interfaces;

public class PlaceOrderHandlerTests
{
    [Fact]
    public async Task PlaceOrder_ShouldCreateOrder_WhenValidItemsProvided()
    {
        // Arrange
        var menuItemRepositoryMock = new Mock<IMenuItemRepository>();
        var orderRepositoryMock = new Mock<IRepository<Order>>();
        var orderFactoryMock = new Mock<IOrderFactory>();
        var userContextServiceMock = new Mock<IUserContextService>();
        var cancellationToken = CancellationToken.None;

        // Mock dependencies
        var menuItems = new List<MenuItem>
        {
            new MenuItem { Id = Guid.NewGuid(), Name = "Burger", Price = 5.99M, IsAvailable = true },
            new MenuItem { Id = Guid.NewGuid(), Name = "Pizza", Price = 12.99M, IsAvailable = true }
        };

        menuItemRepositoryMock
            .Setup(repo => repo.GetAvailableMenuItemsAsync(It.IsAny<IEnumerable<Guid>>()))
            .ReturnsAsync(menuItems);

        var userId = Guid.NewGuid();
        userContextServiceMock
            .Setup(service => service.GetCurrentUserId())
            .Returns(userId);

        var createdOrder = new Order
        {
            Id = Guid.NewGuid(),
            CustomerId = userId,
            Items = menuItems.Select(mi => new OrderItem
            {
                MenuItemId = mi.Id,
                Quantity = 2,
                Price = mi.Price,
                Name = mi.Name
            }).ToList()
        };

        orderFactoryMock
            .Setup(factory => factory.CreateOrder(
                It.IsAny<Guid>(),
                It.IsAny<List<OrderItem>>(),
                It.IsAny<DeliveryOption>(),
                It.IsAny<string>()
            ))
            .Returns(createdOrder);

        orderRepositoryMock
            .Setup(repo => repo.AddAsync(It.IsAny<Order>(), cancellationToken))
            .Returns(Task.CompletedTask);

        orderRepositoryMock
            .Setup(repo => repo.SaveChangesAsync(cancellationToken))
            .Returns(Task.CompletedTask);

        var command = new PlaceOrderCommand
        {
            DeliveryOption = DeliveryOption.Delivery,
            SpecialInstructions = "Extra napkins",
            Items = menuItems.Select(mi => new OrderItemCommand
            {
                MenuItemId = mi.Id,
                Quantity = 2
            }).ToList()
        };

        var handler = new PlaceOrderHandler(
            orderRepositoryMock.Object,
            menuItemRepositoryMock.Object,
            orderFactoryMock.Object,
            userContextServiceMock.Object
        );

        var result = await handler.Handle(command, cancellationToken);

        Assert.Equal(createdOrder.Id, result);
        orderRepositoryMock.Verify(repo => repo.AddAsync(It.IsAny<Order>(), cancellationToken), Times.Once);
        orderRepositoryMock.Verify(repo => repo.SaveChangesAsync(cancellationToken), Times.Once);
        menuItemRepositoryMock.Verify(repo => repo.GetAvailableMenuItemsAsync(It.IsAny<IEnumerable<Guid>>()), Times.Once);
        userContextServiceMock.Verify(service => service.GetCurrentUserId(), Times.Once);
    }

    [Fact]
    public async Task PlaceOrder_ShouldThrowException_WhenMenuItemNotAvailable()
    {
        var menuItemRepositoryMock = new Mock<IMenuItemRepository>();
        var orderRepositoryMock = new Mock<IRepository<Order>>();
        var orderFactoryMock = new Mock<IOrderFactory>();
        var userContextServiceMock = new Mock<IUserContextService>();
        var cancellationToken = CancellationToken.None;

        menuItemRepositoryMock
            .Setup(repo => repo.GetAvailableMenuItemsAsync(It.IsAny<IEnumerable<Guid>>()))
            .ReturnsAsync(new List<MenuItem>()); 

        var command = new PlaceOrderCommand
        {
            DeliveryOption = DeliveryOption.Delivery,
            Items = new List<OrderItemCommand>
            {
                new OrderItemCommand { MenuItemId = Guid.NewGuid(), Quantity = 1 }
            }
        };

        var handler = new PlaceOrderHandler(
            orderRepositoryMock.Object,
            menuItemRepositoryMock.Object,
            orderFactoryMock.Object,
            userContextServiceMock.Object
        );

        // Act & Assert
        await Assert.ThrowsAsync<ArgumentException>(() => handler.Handle(command, cancellationToken));
        orderRepositoryMock.Verify(repo => repo.AddAsync(It.IsAny<Order>(), cancellationToken), Times.Never);
        orderRepositoryMock.Verify(repo => repo.SaveChangesAsync(cancellationToken), Times.Never);
    }

    [Fact]
    public async Task PlaceOrder_ShouldThrowException_WhenQuantityIsZeroOrNegative()
    {
        var menuItemRepositoryMock = new Mock<IMenuItemRepository>();
        var orderRepositoryMock = new Mock<IRepository<Order>>();
        var orderFactoryMock = new Mock<IOrderFactory>();
        var userContextServiceMock = new Mock<IUserContextService>();
        var cancellationToken = CancellationToken.None;

        var command = new PlaceOrderCommand
        {
            DeliveryOption = DeliveryOption.Delivery,
            Items = new List<OrderItemCommand>
            {
                new OrderItemCommand { MenuItemId = Guid.NewGuid(), Quantity = 0 }
            }
        };

        var handler = new PlaceOrderHandler(
            orderRepositoryMock.Object,
            menuItemRepositoryMock.Object,
            orderFactoryMock.Object,
            userContextServiceMock.Object
        );

        // Act & Assert
        await Assert.ThrowsAsync<ArgumentException>(() => handler.Handle(command, cancellationToken));
        orderRepositoryMock.Verify(repo => repo.AddAsync(It.IsAny<Order>(), cancellationToken), Times.Never);
        orderRepositoryMock.Verify(repo => repo.SaveChangesAsync(cancellationToken), Times.Never);
    }
}
