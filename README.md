# 🔧 Job Portal — Backend API

A RESTful Web API built with **ASP.NET Core** that powers the Job Portal application. Handles authentication, job management, and application tracking with role-based authorization.

---

## 🛠️ Tech Stack

| Technology | Purpose |
| ASP.NET Core | Web API Framework |
| Entity Framework Core | ORM / Database Access |
| SQL Server | Database |
| JWT Bearer | Authentication & Authorization |
| BCrypt.Net | Password Hashing |

---

## ✨ Features

- JWT-based authentication (Register / Login)
- Role-based authorization (`JobSeeker`, `Employer`)
- Full CRUD for Job listings
- Job Application system
- Employer can view applicants per job
- Job Seeker can view their own applications

---

## 📁 Project Structure

```
JobPortal.API/
├── Controllers/
│   ├── AuthController.cs           # Register & Login endpoints
│   ├── JobsController.cs           # Job listing endpoints
│   └── JobApplicationController.cs # Application endpoints
├── Data/
│   └── ApplicationDbContext.cs     # EF Core DB Context
├── DTOs/
│   ├── RegisterDto.cs
│   ├── LoginDto.cs
│   ├── ApplyJobDto.cs
│   └── UpdateApplicationStatusDto.cs
├── Helpers/
│   └── JwtHelper.cs                # JWT token generation
├── Models/
│   ├── User.cs
│   ├── Job.cs
│   └── JobApplication.cs
├── Migrations/                     # EF Core migrations
├── appsettings.example.json        # Config template (copy & fill in)
└── Program.cs                      # App startup & middleware
```

---

## 📡 API Endpoints

### Auth
| Method | Endpoint | Description |
|--------|----------|-------------|
| POST | `/api/Auth/register` | Register new user |
| POST | `/api/Auth/login` | Login and receive JWT token |

### Jobs
| Method | Endpoint | Auth Required | Description |
|--------|----------|---------------|-------------|
| GET | `/api/Jobs/all` | ✅ | Get all job listings |
| POST | `/api/Jobs` | ✅ Employer | Create a new job |
| GET | `/api/Jobs/my-jobs` | ✅ Employer | Get jobs posted by logged-in employer |

### Applications
| Method | Endpoint | Auth Required | Description |
|--------|----------|---------------|-------------|
| POST | `/api/JobApplication/apply` | ✅ JobSeeker | Apply for a job |
| GET | `/api/JobApplication/my-applications` | ✅ JobSeeker | Get my applications |
| GET | `/api/JobApplication/job/{jobId}/applications` | ✅ Employer | Get applicants for a job |

---

## 🚀 Getting Started

### Prerequisites
- Visual Studio 2022
- .NET 8 SDK
- SQL Server (LocalDB or full)

### Setup

```bash
# 1. Clone the repository
git clone https://github.com/YOUR_USERNAME/job-portal-api.git

# 2. Open JobPortalSystem_API.sln in Visual Studio 2022
```

**3. Configure the database:**

Copy `appsettings.example.json` → rename it to `appsettings.json`, then fill in your connection string:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=YOUR_SERVER;Database=JobPortalDB;Trusted_Connection=True;"
  },
  "JwtSettings": {
    "SecretKey": "YOUR_SECRET_KEY_HERE",
    "Issuer": "JobPortalAPI",
    "Audience": "JobPortalClient"
  }
}
```

**4. Apply database migrations:**

Open the Package Manager Console and run:
```
Update-Database
```

**5. Run the project** — press `F5` or click the Run button in Visual Studio.

The API will start at `http://localhost:5027`

---

## 🔗 Related Repository

- 🎨 **Frontend:** [job-portal-frontend](https://github.com/YOUR_USERNAME/job-portal-frontend) — React + Vite

---

## 🔐 Authentication

JWT tokens are issued on login and must be sent as a Bearer token in the `Authorization` header for protected endpoints:

```
Authorization: Bearer <your_token>
```
