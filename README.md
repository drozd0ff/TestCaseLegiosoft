# TestCaseLegiosoft
ASP.NET Core Web Api for simple transaction management, available functions:
- Import .csv, 
- Export .xlsx (filtered by status/type),
- Get transactions (filtered by status/type), Update transaction status by Id, Delete transaction by Id

## Getting Started
Use these instructions to get the project up and running.

### Prerequisites
You will need the following tools:

- [Visual Studio Code or Visual Studio 2019](https://visualstudio.microsoft.com/vs/)
- [.NET Core SDK 3.1](https://dotnet.microsoft.com/download/dotnet-core/3.1)
- [SQL Server Express LocalDB](https://www.microsoft.com/en-us/sql-server/sql-server-downloads) (also can be installed via VS Installer)
  *(or you can change connection string to connect to your DB (connectionString is in appsettings.json file), migrations should be applied automatically)*
  
### Setup
Follow these steps to get your development environment set up:

  1. Clone the repository
  2. At the root directory, restore required packages by running:
      ```
     dotnet restore
     ```
  3. Next, build the solution by running:
     ```
     dotnet build
     ```
  4. Launch project by running:
     ```
     dotnet run
     ```
  5. Launch [https://localhost:5001/swagger](https://localhost:5001/swagger) in your browser to view Swagger UI

## Technologies
* .NET Core 3
* ASP.NET Core 3
* Entity Framework Core 3
* Swagger
