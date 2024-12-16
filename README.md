# Order Manager API

Order Manager API is a robust, scalable solution for managing restaurant orders, delivery staff, and menu items. It features advanced design patterns, clean architecture principles, and modern practices like the Specification Pattern, Domain Events, and CQRS (Command Query Responsibility Segregation). Built with .NET, it ensures high performance and maintainability.

---

## **Features**

### **Order Management**
- **Create Orders:** Allows customers to create orders with delivery or pickup options.
- **Track Order States:** Tracks the state of orders (e.g., Pending, Preparing, Out for Delivery).
- **State Transitions:** Enforces business rules when moving orders through various states using a State Pattern.
- **Audit Logging:** Tracks state changes for orders via `OrderStateAudits`.

### **Menu Management**
- **CRUD Operations:** Provides endpoints for creating, updating, deleting (soft delete), and retrieving menu items.
- **Filters & Sorting:** Customers can view only available menu items, while admin roles can view all.
- **Advanced Querying:** Supports dynamic filtering, sorting, and pagination using a Query Builder.

### **Delivery Staff Management**
- **Assignment Logic:** Restaurant staff can assign orders to available delivery staff.
- **Tracking:** Keeps track of delivery staff availability, assigned orders, and completed deliveries.
- **Delivery Statistics:** Tracks delivery performance, such as completed deliveries and average delivery time.

###not implemented, intended
### **Admin Dashboard**
- **Statistics:**
  - Total orders delivered within a specific time frame.
  - Orders grouped by state.
  - Delivery stats per delivery staff.
  - Average delivery times.
- **Advanced Filters:** Allows admin to filter data by delivery staff, time range, and more.

### **Security**
- **Role-Based Access Control:** Different roles (Customer, RestaurantStaff, DeliveryStaff, Admin) with specific permissions.
- **JWT Authentication:** Secures endpoints with Bearer tokens.
- **Custom Policies:** Enforces role-specific access policies.

### **Event-Driven Architecture**
- **Domain Events:** Handles side effects (e.g., updating delivery staff availability) with a Domain Event system.
- **Mediator Integration:** Decouples event handlers using MediatR for commands and notifications.


---

## **Technologies Used**
- **.NET 8**: Framework for building high-performance web applications.
- **Entity Framework Core**: ORM for database interactions.
- **MediatR**: Simplifies CQRS and event-driven architecture.
- **FluentValidation**: Enforces input validation rules.
- **AutoMapper**: Simplifies mapping between DTOs and domain models.
- **PostgreSQL**: Relational database backend.
- **Swagger/OpenAPI**: Provides API documentation and testing capabilities.

---
##**Prerequisites**
- **Visual Studio 2022 with Docker support
- **Docker Desktop (latest version)
- **.NET SDK 8.0
- **PostgreSQL (included in Docker Compose)
- **RabbitMQ (included in Docker Compose)

##***DB USERS*
{
  "username": "customer1@example.com",
  "password": "Admin@123"
}
{
  "username": "deliverystaff1@example.com",
  "password": "Admin@123"
}
{
  "username": "restaurantstaff@example.com",
  "password": "Admin@123"
}
{
  "username": "admin@ordermanager.com",
  "password": "Admin@123"
}

## **Future Implementetions**
- **Use the PostgreSQL ltrre for hierarchy in the filters with mangers that manage many stores.
- **Use RabbitMQ for messaging and notification for the autoasigment depending the distance of the delivery staff from the Order