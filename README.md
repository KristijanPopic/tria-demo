# Tria Demo

### Prerequisites

- Docker Desktop installed
- Alternatively, you will need PostgreSQL installed on your machine

### Setup

1. clone project, checkout `main` branch
2. in the root folder, run `docker compose up -d`. It needs ports 5000 (for the app) and port 5432 (for PostgreSQL) so make sure those ports are not already occupied. If the ports are already occupied, change them in `compose.yaml` according to your needs before running the command.
3. open `http://localhost:5000/swagger` in your browser

### Startup
When starting with `docker compose up -d`, on rare occasions the database doesn't get available before the app starts. The app needs connection to seed the database on startup. It will retry 10 times to connect, but if the database doesn't get available until then, it will crash. In that case, check your containers, make sure that PostgreSQL container is running and healthy and simply start the app container again.

### Notes

* I'm assuming that notifications are completely custom (not predefined in the system). For simplicity I'm also assuming that they are in-app notifications so they're immediately stored to the database. In case of emails or SMS messages this would be done asynchronously, meaning there would be a message queue in between (e.g. RabbitMQ), message would first be published to the queue and then sent when consumed from the queue
* This demo uses a simple layered architecture, suitable for small to medium-size monolithic CRUD projects. An alternative would be vertical-slice architecture when the complexity is high. 
* The dependency direction between Service and Repository layer is inverted (dependency inversion principle), i.e. instead of Service having a reference to Repository, the Repository has a reference to Service. The Service defines repository contracts based on it's needs, the Repository layer implements them using Entity Framework with PostgreSQL. That way the Service layer is decoupled from infrastructure as it doesn't depend on EF nor database
* There are various ways of transporting data from client to database across layers, depending on the project complexity and it's needs. There can be separate data classes for each layer, BLL layer may perhaps accept `Command` objects and return `CommandResult<T>` with `T` being another kind of DTO to avoid leaking BLL models to the API, Repository layer may have it's own DTOs for EF/DB interaction to avoid mixing properties with BLL models and so on. For this demo, I kept it simple and used only two types of classes:
  * Domain models. They are anemic in this case, containing only simple data properties, but they could also be real OOP models, if e.g. it's a DDD style project. In this demo they are also used for EF/DB interaction directly. They do leak out to the Rest API Controllers.
  * API request DTOs. They just accept HTTP payload from the client and return response to the client. The API layer maps them to/from domain models
* Error handling / API responses can also be done in many different ways. For this demo I throw an exception from Services when a business rule is violated and use the `GlobalExceptionHandler` to return proper problem details response. An alternative is to return `CommandResult` objects with errors from services and then the API can decide what response to return.
* In a realworlds project, `Get` endpoints would have paging and a default page size defined to avoid fetching possibly huge amount of data
