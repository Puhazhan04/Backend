Sakila REST API
This project is a RESTful API for the Sakila database built with C# and ASP.NET Core. It allows interaction with the following entities:

Actor
Address
City
Customer
Film
Rental
Features
CRUD support (GET, POST, PUT, DELETE)
DTOs for clean API contracts
Entity Framework Core for database access
Logging with ILogger
Error handling with custom exceptions
SQL Injection protection
Clear separation of concerns (Controller, Service, DTO, Entity)
Technologies Used
.NET 8
Entity Framework Core (MySQL via Pomelo)
Mapster (for DTO mapping)
Getting Started
Prerequisites
.NET 8 SDK
MySQL or XAMPP with Sakila database imported
Configuration
Set the connection string in appsettings.json:

"ConnectionStrings": {
"DefaultConnection": "server=localhost;user=root;password=yourPassword;database=sakila"
}
Run the API
dotnet build
dotnet run
API Endpoints
Actor
GET     /actor          -> Get all actors
GET     /actor/{id}     -> Get actor by ID
POST    /actor          -> Create new actor
PUT     /actor/{id}     -> Update actor
DELETE  /actor/{id}     -> Delete actor
Customer
GET     /customer
GET     /customer/{id}
POST    /customer
PUT     /customer/{id}
Rental
GET     /rental
GET     /rental/{id}
POST    /rental          -> Create rental
PUT     /rental/{id}     -> Close rental (set return date)
(similar for City, Address, Film)

Example: Create a Rental
POST /rental

{
"inventoryId": 1,
"customerId": 2,
"staffId": 1
}
Response

{
"rentalId": 10005,
"rentalDate": "2025-05-06T10:22:13",
"returnDate": null,
"customerId": 2,
"inventoryId": 1,
"staffId": 1
}
License
MIT License

Author
Developed by Puhazhan . Part of module M295 final project.