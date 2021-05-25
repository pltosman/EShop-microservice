# Microservice EShop

This project is a simple .Net 5.0 code for microservice architecture pattern using MediatR, CQRS, and Docker.

## Services

By now, the functional services are still decomposed into four core services. Each of them can be tested, built, and deployed independently.

![Infrastructure plan](add link)

### Core service

Provides several API for user authentication and authorization with OAuth 2.0.

| Method | Path              | Description                                   | Scope |
|--------|-------------------|-----------------------------------------------|-------|
| POST   | /api/v1/Auth/createtoken  | Get new access token | ui    |
| POST   | /api/v1/Auth/loginwithemail | Login with email address and password         | ui    |
| POST   | /api/v1/Auth/refreshtoken  | Get refresh access token | ui    |
| POST   | /api/v1/Register/register  | Create new customer  | ui    |
| POST   | /api/v1/Confirmation/emailconfirmation  | Get refresh access token | ui    |
| POST   | /api/v1/Confirmation/emailconfirm  | Get refresh access token | ui    |
| POST   | /api/v1/Password/resetpassword  | Update customer's password | ui    |


### Product service

Provides several API for product with OAuth 2.0.

| Method | Path                                     | Description                                   | Scope  | Privilege              |
| ------ | ---------------------------------------- | --------------------------------------------- | ------ | ---------------------- |
| POST   | /api/v1/product                          | Create new product                            | ui     | ALL_ACCESS             |
| GET    | /api/v1/product                          | Get All product information                  | ui     | READ_BASIC_INFORMATION |
| GET    | /api/v1/productsByMerchant{merchantName} | Get All product information by Merchant Name | ui     | READ_BASIC_INFORMATION |
| PUT    | /api/v1/product/                         | Update product with id                        | server | ALL_ACCESS             |
| GET    | /api/v1/product/{id}                     | Get current product with id                   | server | ALL_ACCESS             |
| DELETE | /api/v1/product/{id}                     | Delete current product with id                | server | ALL_ACCESS             |

### Order service

| Method | Path              | Description                                   | Scope |
|--------|-------------------|-----------------------------------------------|-------|
| POST   | /api/v1/Order | Create new Order | ui    |
| POST   | /api/v1/Order/ChangeOrderStatus | Change order status with order id and status        | ui    |
| GET   | /api/v1/Order/GetOrdersByMerchantNameQuery/{merchantName}  | Get all order indormation by Merchant name | ui    |

### Payment service

### APIGateway

### RabbitMQ

### MongoDb
docker pull mongo

docker run -d -p 27017:27017 --name EShopMongoDb mongo
### Serilog
docker run -d --restart unless-stopped --name seq -e ACCEPT_EULA=Y -v 'PATH':/data -p 8081:80 datalust/seq:latest
#### Notes

- Each microservice has it's own database and there is no way to access the database directly from other services.
- The services in this project are using MySQL, PostgreSQL, MongoDB for the persistent storage. In other case, it is also possible for one service
  to use any type of database (SQL or NoSQL).
- Service-to-service communication is done by using RabbitMQ.

### Important Endpoint \*

- [http://localhost:80](http://localhost:80) - Gateway
- [http://localhost:15672](http://localhost:15672) - RabbitMq management (default login/password: guest/guest)
- [http://localhost:8081](http://localhost:8081) - Serilog UI
- [http://localhost:6379](http://localhost:6379) - Redis
- [http://localhost:27017](http://localhost:27017) - MongoDb
