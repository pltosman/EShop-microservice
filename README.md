# Microservice EShop

This project is a simple .Net 5.0 code for microservice architecture pattern using MediatR,CQRS, and Docker.
 
## Services

By now, the functional services are still decomposed into four core services. Each of them can be tested, built, and deployed independently.

![Infrastructure plan](add  link)

### Core service
### Product service
Provides several API for user authentication and authorization with OAuth 2.0.

| Method | Path              | Description                                   | Scope |  Privilege |
|--------|-------------------|-----------------------------------------------|-------|------------|
| POST   | /api/v1/product  | Create new product | ui    | ALL_ACCESS |
| GET    | /api/v1/product  | Get All product informations                | ui    | READ_BASIC_INFORMATION |
| PUT    | /api/v1/product/ |  | server | ALL_ACCESS |
| GET    | /api/v1/product/{id} | Get current product with id  | server | ALL_ACCESS |
| DELETE    | /api/v1/product/{id}  | Delete current product with id | server | ALL_ACCESS |
### Order service
### Payment service
### APIGateway
### RabbitMQ



#### Notes
* Each microservice has it's own database and there is no way to access the database directly from other services.
* The services in this project are using MySQL, PostgreSQL, MOngoDB for the persistent storage. In other case, it is also possible for one service 
to use any type of database (SQL or NoSQL).
* Service-to-service communiation is done by using RabbitMQ.


### Important Endpoint *
* [http://localhost:80](http://localhost:80) - Gateway
* [http://localhost:15672](http://localhost:15672) - RabbitMq management (default login/password: guest/guest)
