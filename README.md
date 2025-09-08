# 🎓 Course and Academic Management System

A structured **ASP.NET Core MVC** web application for managing **Courses, Sessions, Users, and Grades**, built with **Three-Tier Architecture** (Presentation Layer, Business Logic Layer, Data Access Layer) and implemented using **Repository Pattern, Generic Repository, and Unit of Work**.

---

## 📌 Project Overview

The system allows administrators, instructors, and trainees to manage academic processes in a clean, maintainable, and scalable way.  

### Core features

- **Course Management** (create, edit, delete, assign instructor, validations, search, pagination)
- **Session Management** (schedule sessions, capacity, enrollment rules, validation on dates)
- **User Management** (roles: Admin, Instructor, Trainee, user CRUD, unique email validation)
- **Grades Management** (record grades, multiple attempts, final grade enforcement, trainee reports)
- **Reusable UI Components** (partial views for forms)
- **Validation & Feedback** (Data Annotations, Custom Validation, Remote Validation, Bootstrap alerts)
- **Audit & Security** (RowVersion concurrency, soft delete, role-based authorization)

---

## 🏗️ Architecture

The project follows **3-Tier Architecture**:

1. **Presentation Layer (UI)**
   - ASP.NET MVC Controllers
   - Razor Views with Tag Helpers
   - Bootstrap-based UI for responsive design

2. **Business Logic Layer (BLL)**
   - Service classes for rules and validation
   - Custom validation (e.g., NoNumberAttribute)
   - Unit of Work to coordinate transactions

3. **Data Access Layer (DAL)**
   - Entity Framework Core
   - Repository Pattern & Generic Repository
   - Separation of concerns for clean persistence logic

---

## ⚙️ Technologies

- **.NET 9** (ASP.NET Core MVC)
- **Entity Framework Core**
- **SQL Server** (default database, can be replaced)
- **Bootstrap 5**
- **JavaScript / jQuery** (for remote validation & client-side scripts)
- **Git & GitHub** (version control)
- **Jira** (project management) → [View Jira Board](https://eslamwkhalifa.atlassian.net/jira/software/projects/OPS/summary?atlOrigin=eyJpIjoiOGI3ODNlM2JlZjk5NDkzYjlkYmJmYWVkZjg3YWNkZDIiLCJwIjoiaiJ9)
- **Docker** (planned for containerization at final stage)
- **Monster .NET** (deployment target)

---

## 🚀 Getting Started

### Prerequisites

- .NET 9 SDK
- SQL Server or SQL Express
- Visual Studio 2022 (recommended)
- Git

### Clone Repository

```bash
git clone https://github.com/your-username/course-and-academic-management-system.git
cd course-and-academic-management-system
````

### Database Setup

1. Update `appsettings.json` with your SQL Server connection string.
2. Apply migrations:

   ```bash
   dotnet ef database update
   ```

### Run the Project

```bash
dotnet run
```

Then visit [http://localhost:5000](http://localhost:5000).

---

## 📖 Project Structure

```txt
CourseAndAcademicManagementSystem/
│── src/
│   ├── Presentation/        # MVC controllers & views
│   ├── Business/            # Services, rules, validators
│   ├── DataAccess/          # Repositories, EF Core, Unit of Work
│   └── Core/                # Entities, DTOs, Interfaces
│── tests/                   # Unit tests
│── docs/                    # Requirements, ERD, diagrams
│── README.md
```

---

## 🛠️ Development Workflow

We use **Git Flow**:

* Default branch: `dev`
* Production branch: `main`
* Feature branches: `feature/*`

### Example

```bash
git checkout dev
git checkout -b feature/course-crud
# work, commit, push, and create a PR into dev
```

---

## 🤝 Contribution Guide

1. Fork the repository
2. Create a new feature branch (`feature/xyz`)
3. Commit your changes (`git commit -m "feat: add course search with pagination"`)
4. Push to the branch
5. Open a Pull Request to `dev`

---

## 👥 Contributors

* [Eslam Khalifa](https://github.com/eslam-khalifa)
* [Waled Maher]()
* [Omar Mohammed](https://github.com/Omar-Mohameed)

---

## 📜 License

This project is for academic purposes and internal university use. Licensing can be adapted later if extended.
