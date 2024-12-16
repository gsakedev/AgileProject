# **Order Manager API**

Order Manager API is a robust, scalable solution for managing restaurant orders, delivery staff, and menu items. It features advanced design patterns, clean architecture principles, and modern practices like the **Specification Pattern**, **Domain Events**, and **CQRS (Command Query Responsibility Segregation)**. Built with **.NET 8**, it ensures high performance and maintainability.

---

## **Features**

### **Order Management**
- **Create Orders**: Allows customers to create orders with delivery or pickup options.  
- **Track Order States**: Tracks the state of orders (e.g., Pending, Preparing, Out for Delivery).  
- **State Transitions**: Enforces business rules when moving orders through various states using the State Pattern.  
- **Audit Logging**: Tracks state changes for orders via `OrderStateAudits`.  

### **Menu Management**
- **CRUD Operations**: Provides endpoints for creating, updating, soft deleting, and retrieving menu items.  
- **Filters & Sorting**: Customers can view only available menu items, while admin roles can view all items.  
- **Advanced Querying**: Supports dynamic filtering, sorting, and pagination using a Query Builder.  

### **Delivery Staff Management**
- **Assignment Logic**: Restaurant staff can assign orders to available delivery staff.  
- **Tracking**: Keeps track of delivery staff availability, assigned orders, and completed deliveries.  
- **Delivery Statistics**: Tracks delivery performance, such as completed deliveries and average delivery time.  

---

### **Admin Dashboard** *(Planned)*
- **Statistics**:
  - Total orders delivered within a specific time frame.
  - Orders grouped by state.
  - Delivery stats per delivery staff.
  - Average delivery times.
- **Advanced Filters**: Allows admins to filter data by delivery staff, time range, and more.

---

### **Security**
- **Role-Based Access Control**: Different roles (Customer, RestaurantStaff, DeliveryStaff, Admin) with specific permissions.  
- **JWT Authentication**: Secures endpoints with Bearer tokens.  
- **Custom Policies**: Enforces role-specific access policies.  

---

### **Event-Driven Architecture**
- **Domain Events**: Handles side effects (e.g., updating delivery staff availability) with a Domain Event system.  
- **Mediator Integration**: Decouples event handlers using MediatR for commands and notifications.  

---

## **Technologies Used**
- **.NET 8**: Framework for building high-performance web applications.  
- **Entity Framework Core**: ORM for database interactions.  
- **MediatR**: Simplifies CQRS and event-driven architecture.  
- **FluentValidation**: Enforces input validation rules.  
- **AutoMapper**: Simplifies mapping between DTOs and domain models.  
- **PostgreSQL**: Relational database backend.  
- **Swagger/OpenAPI**: Provides API documentation and testing capabilities.  
- **RabbitMQ**: Message broker for asynchronous messaging and notifications.  

---

## **DB USERS**

```json
[
  { "username": "customer1@example.com", "password": "Admin@123" },
  { "username": "customer2@example.com", "password": "Admin@123" },
  { "username": "customer3@example.com", "password": "Admin@123" },
  { "username": "deliverystaff1@example.com", "password": "Admin@123" },
  { "username": "deliverystaff2@example.com", "password": "Admin@123" },
  { "username": "deliverystaff3@example.com", "password": "Admin@123" },
  { "username": "restaurantstaff@example.com", "password": "Admin@123" },
  { "username": "admin@ordermanager.com", "password": "Admin@123" }
]
```
---

## **Future Implementations**

### **1. PostgreSQL `ltree` for Hierarchical Filters**

- **Purpose**:  
   Use the `ltree` PostgreSQL extension to represent hierarchical relationships, such as stores managed by managers and regions. This will allow for advanced querying and filtering capabilities based on hierarchy.

- **Example**:  
   A manager may oversee multiple stores in a hierarchy:  
   - `store.north.east.branch1`  
   - `store.north.east.branch2`  
   - `store.south.west.branch1`

- **Advantages**:  
   - Enables querying based on tree-like structures.  
   - Supports filters like "find all branches under `store.north`".

---

### **2. RabbitMQ for Distance-Based Auto-Assignment**

- **Purpose**:  
   Use RabbitMQ to notify and auto-assign delivery staff based on the distance to the order location.

- **Steps**:  
   1. Publish an event (e.g., `OrderReadyForDeliveryEvent`) when an order is marked as **"Ready for Delivery"**.  
   2. A consumer listens to this event and calculates the distance between the delivery staff's location and the order location.  
   3. The closest delivery staff is automatically assigned to the order.

- **Technologies**:  
   - **RabbitMQ**: For event publishing and subscribing.  
   - **Google Maps API**: For geolocation and distance calculation.  
   - **Background Workers**: For asynchronous processing of assignment logic.

---
