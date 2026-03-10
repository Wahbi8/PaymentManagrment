# Payment Management API

A production-ready RESTful API for managing invoices, payments, and customers. Built with ASP.NET Core 8 following Clean Architecture principles. This repository contains the API presentation project, application services, domain entities, and infrastructure (EF Core) plus unit tests and CI/Docker workflows.

## Features

### Core Features
- **Invoice Management**
  - Create invoices with multiple line items
  - Auto-calculated totals (subtotal, tax, total)
  - Partial payments support
  - Invoice status workflow (draft → sent → partially paid → paid/overdue → cancelled)
  
- **Payment Processing**
  - Support for multiple payment methods
  - Full and partial refunds
  - Payment history tracking

- **Customer & Company Management**
  - Full CRUD operations
  - Company-based data isolation

- **Audit Trail**
  - Track all create/update/delete operations
  - User action logging with timestamps

### Technical Features
- **Authentication & Authorization**
  - JWT-based authentication
  - Refresh token support
  - Role-based access control

- **Data Validation**
  - FluentValidation for input validation
  - Consistent error responses

- **API Features**
  - Pagination for list endpoints
  - Global exception handling
  - RESTful API design

## Architecture

```
PaymentManagement/
├── Domain/                 # Business entities & interfaces
│   ├── Entities/
│   ├── Interfaces/
│   └── Exception/
├── Application/            # Business logic
│   ├── Services/
│   ├── DTO/
│   ├── Validators/
│   └── Mappings/
├── Infrastructure/          # Data access
│   └── Repositories/
└── Presentation/           # API endpoints
    ├── Controllers/
    ├── Middlewares/
    └── Common/
```

## Technology Stack

- **.NET 8** - Framework
- **ASP.NET Core Web API** - API framework
- **Entity Framework Core** - ORM (AppDbContext in `PaymentManagement.Infrastructure`)
- **SQL Server** - Database
- **JWT** - Authentication
- **AutoMapper** - Object mapping
- **FluentValidation** - Validation
- **xUnit + Moq** - Testing (tests in `PaymentManagement.Tests`)
- **Docker** - Containerization (Dockerfile in `PaymentManagement.Presentation`)
- **GitHub Actions** - CI and Docker workflows (.github/workflows)

## Getting Started

### Prerequisites
- Visual Studio 2022 (or newer) with .NET 8 workloads
- .NET 8 SDK
- SQL Server instance accessible to the connection string in `PaymentManagement.Presentation\appsettings.json`
- (Optional) Docker
- (Optional) `dotnet-ef` tool for running migrations

### Configuration

Update `PaymentManagement.Presentation\appsettings.json` with valid values:

{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost;Database=PaymentDB;Trusted_Connection=True;TrustServerCertificate=True"
  },
  "JwtSettings": {
    "SecretKey": "YourSecretKeyHere",
    "Issuer": "YourIssuer",
    "Audience": "YourAudience",
    "AccessToken": "60",  // Token expiry in minutes
    "RefreshToken": "30"   // Token expiry in days
  }
}

Important settings:
- `ConnectionStrings:DefaultConnection` — SQL Server connection string.
- `JwtSettings:SecretKey` — keep secret; for production use environment variables or a secrets store.

Recommended environment variable names (for local/production overrides):
- `ConnectionStrings__DefaultConnection`
- `JwtSettings__SecretKey`
- `JwtSettings__Issuer`
- `JwtSettings__Audience`

To avoid committing secrets, replace sensitive values with environment variables or use the Secret Manager in development.

### Database Migrations

Install EF tool (if not installed):

dotnet tool install --global dotnet-ef

Run migrations and update the database (run from repo root):

dotnet ef database update --project PaymentManagement.Infrastructure --startup-project PaymentManagement.Presentation
```

Adjust commands if you use a different startup or migrations project.

### Run Locally

Using Visual Studio 2022:
1. Open the solution in Visual Studio.
2. Set `PaymentManagement.Presentation` as the startup project.
3. Ensure `PaymentManagement.Presentation\appsettings.json` (or environment variables) contain a valid connection string and JWT settings.
4. Start debugging using __Debug > Start Debugging__.

Using CLI:

dotnet restore
dotnet build
dotnet run --project PaymentManagement.Presentation

Run tests:

dotnet test PaymentManagement.Tests

### Run with Docker

A Dockerfile exists in `PaymentManagement.Presentation`. To build and run the container locally:

docker build -f PaymentManagement.Presentation\Dockerfile -t paymentmanagement .
docker run -p 5000:80 -e ConnectionStrings__DefaultConnection="<your-connection>" -e JwtSettings__SecretKey="<your-secret>" paymentmanagement

CI workflows for building, testing, and publishing container images are under `.github/workflows` (see `ci.yml` and `docker.yml`).

## API Endpoints

### Authentication
| Method | Endpoint | Description |
|--------|----------|-------------|
| POST | `/api/auth/register` | Register new user |
| POST | `/api/auth/login` | Login and get tokens |
| POST | `/api/auth/refresh` | Refresh access token |

### Invoices
| Method | Endpoint | Description |
|--------|----------|-------------|
| GET | `/api/invoice` | Get paginated invoices |
| GET | `/api/invoice/{id}` | Get invoice by ID |
| POST | `/api/invoice` | Create invoice with line items |
| PUT | `/api/invoice/{id}` | Update invoice |
| DELETE | `/api/invoice/{id}` | Delete invoice |
| POST | `/api/invoice/{id}/send` | Send invoice |
| POST | `/api/invoice/{id}/cancel` | Cancel invoice |

### Payments
| Method | Endpoint | Description |
|--------|----------|-------------|
| GET | `/api/payment` | Get all payments |
| GET | `/api/payment/{id}` | Get payment by ID |
| POST | `/api/payment` | Create payment |

### Customers
| Method | Endpoint | Description |
|--------|----------|-------------|
| GET | `/api/customer` | Get all customers |
| GET | `/api/customer/{id}` | Get customer by ID |
| POST | `/api/customer` | Create customer |
| PUT | `/api/customer/{id}` | Update customer |
| DELETE | `/api/customer/{id}` | Delete customer |

### Companies
| Method | Endpoint | Description |
|--------|----------|-------------|
| GET | `/api/company` | Get all companies |
| GET | `/api/company/{id}` | Get company by ID |
| POST | `/api/company` | Create company |
| PUT | `/api/company/{id}` | Update company |
| DELETE | `/api/company/{id}` | Delete company |

## API Surface

Controllers exposed in the Presentation project include:
- `AuthController` — authentication endpoints (login/refresh).
- `CustomerController` — customer CRUD operations.
- `CompanyController` — company CRUD operations.
- `InvoiceController` — invoice operations and invoice line items.

Use Swagger if enabled, or refer to controller route attributes to see exact endpoints.

## Running Tests

dotnet test

## CI/CD

The project includes GitHub Actions workflows:
- **CI** - Builds and runs tests on every push/PR
- **Docker** - Builds and pushes Docker image on the main branch

## Project Structure Details

### Domain Layer
- `Entities/` - Business objects (Invoice, Payment, Customer, etc.)
- `Interfaces/` - Repository interfaces
- `Exception/` - Custom exception classes

### Application Layer
- `Services/` - Business logic services
- `DTO/` - Data transfer objects
- `Validators/` - FluentValidation validators
- `Mappings/` - AutoMapper profiles

### Infrastructure Layer
- `Repositories/` - Database operations
- `AppDbContext.cs` - EF Core context

### Presentation Layer
- `Controllers/` - API endpoints
- `Middlewares/` - Custom middleware (Exception handling, JWT)
- `Common/Extensions/` - Helper extensions

## Contributing

Read `CONTRIBUTING.md` and follow the repository conventions. This project references an `.editorconfig` to enforce code style — please adhere to it.

Common workflow:
1. Create a feature branch from `master`.
2. Run tests locally (`dotnet test`).
3. Open a pull request and include a clear description of changes.

## Troubleshooting

- If migrations fail, check the connection string and ensure the target database server is reachable.
- If authentication fails, ensure the `JwtSettings__SecretKey` matches between clients and the API.

