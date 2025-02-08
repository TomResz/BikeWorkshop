# Bike Workshop API

BikeWorkshopAPI is an application built using clean architecture and CQRS pattern. It allows employees to place order for specific services of bike workshop, generate summaries and provide essential information to end customers by SMTP service as tracking numbers.

# Database

![Database](https://github.com/TomResz/BikeWorkshop/blob/master/Doc/db.jpg?raw=true)

# Stack

- C# 12 .NET 8.0
- Entity Framework Core
- Fluent Validation
- JWT .NET
- MediatR
- MS SQL Server

# Setup

### Setup via Docker:
Configure <a href="https://learn.microsoft.com/en-us/aspnet/core/security/app-secrets?view=aspnetcore-9.0&tabs=windows">secret file</a>.
   ```json
   {
     "SMPT:Port": "587",
     "SMPT:Password": "password",
     "SMPT:Host": "host.com",
     "SMPT:Email": "email@email.com",
     "Kestrel:Certificates:Development:Password": "58b61312-24a7-4aeb-9a07-3a9af9f16f10",
     "Authentication:JwtKey": "extrastronpassword123!@#$%^&*()_+)()*@!@",
     "Authentication:JwtIssuer": "issuer",
     "Authentication:JwtExpireDays": "7"
   }
   ```
1. **Run Docker Container:**
   Run the following command to start the container on the command line in the solution folder:
     ``` bash
     docker-compose up
     ```
2. **Open Browser:**

   Type the following url to open <b>Swagger UI</b>:

   ``` url
   http://localhost:5000/swagger/index.html
   ```

   or

   ``` url
   https://localhost:5001/swagger/index.html

   ```

### Setup via Visual Studio:

1. **Open BikeWorkshop.API project:**
   - Then open <b>appsetting.json</b> file,
   - Change the following line to your own local MS SQL Server connection string:
   ```
       "LocalDb": "Server=TOMEK;Database=BikeWorkshopDb;Trusted_Connection=True;MultipleActiveResultSets=true;TrustServerCertificate=True;",
   ```
2. **Run application**.

# Swagger

![Swagger](https://github.com/TomResz/BikeWorkshop/blob/master/Doc/swagger.png?raw=true)

# Testing API

1. **Authentication:**
   Almost all endpoints require authentication from the side from a user with a role : **manager** or **employee**.
   For this, use the POST method found after the link :
   `    ../api/Employee/login
   `
2. **Built-in accounts**
   Then you can use two accounts:
   `    Email: admin@admin.com
    Password: adminadmin
    Role: Manager
   `
   or
   `    Email: worker@worker.com
    Password: workerworker
    Role: Worker
   `
3. **Swagger authorization**
   After executing this method, we get JWT authorization token.
   Then we need to enter this token in the Swagger token holder that it's located at the top of page:
   ![Swagger-token-holder](https://github.com/TomResz/BikeWorkshop/blob/master/Doc/swagger-token-holder.png?raw=true)
4. **Test endpoints**
   If the authentication was successful you should not get a <b>401</b> status after testing any endpoint.
