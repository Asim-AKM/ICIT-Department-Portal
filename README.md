# ICIT Department Portal – Backend  
**ASP.NET Core Web API | Final Year Project (FYP)**

---

## 🚀 Project Overview

The **ICIT Department Portal Backend** is a scalable and secure ASP.NET Core Web API developed as part of a **Final Year Project (FYP)**.  
It provides RESTful APIs to manage academic and administrative workflows for the ICIT Department.

This backend supports a **multi-role portal system**, enabling seamless interaction between:

- 🎓 Students  
- 👨‍🏫 Faculty Members  
- 🧾 Clerks  
- 🛠 Administrators  

The system ensures **data integrity, security, and efficient workflow management** across student records, FYP proposals, supervision processes, and administrative controls.

---

## 🧠 System Architecture

The project follows **Clean Architecture principles**, ensuring separation of concerns, maintainability, and scalability.

```
/src
│
├── Application
│   ├── Application_Service
│   │   ├── DTOs
│   │   ├── Interfaces
│   │   └── UseCases
│
├── Core
│   ├── Domain_Service
│   │   ├── Entities
│   │   ├── ValueObjects
│   │   └── BusinessRules
│
├── Infrastructure
│   ├── Infrastructure_Service
│   │   ├── Data
│   │   ├── Repositories
│   │   └── ExternalServices
│
├── Presentation
│   ├── APIGateway_Service
│   │   ├── Controllers
│   │   ├── Middleware
│   │   └── Program.cs
│
└── Solution Items
    └── Directory.Packages.props
```

---

## 🛠 Technology Stack

- **Framework:** ASP.NET Core Web API (C#)  
- **Architecture:** Clean Architecture (Layered)  
- **Database:** Azure SQL Server  
- **ORM:** Entity Framework Core  
- **Authentication:** JWT (JSON Web Token)  
- **Security:** Password Hashing  
- **Login Method:** CNIC + Roll Number  

---

## ⚙️ Core Features

### 🔐 Authentication & Authorization
- Secure CNIC + Roll Number login
- JWT-based authentication
- Role-based access control

### 👥 Role-Based Modules
- **Student:** Proposal submission, transcripts, announcements  
- **Faculty:** Supervision, proposal review  
- **Clerk:** Bulk student entry, fee & transcript management  
- **Admin:** User & role management, system control  

---

## 📖 How to Run the Project

1. Clone the repository:
```bash
git clone https://github.com/Asim-AKM/ICIT-Department-Portal.git
```

2. Navigate to API Gateway:
```bash
cd src/Presentation/APIGateway_Service
```

3. Configure database in `appsettings.json`

4. Apply migrations:
```bash
dotnet ef database update
```

5. Run the project:
```bash
dotnet run
```

API will be available at:
```
https://localhost:5001
```

---

## 📌 API Documentation

Swagger UI:
```
https://localhost:5001/swagger
```

---

## 👨‍💻 Team Members

- Asim Khan  
- Amsa Mansoor  
- Nizam Ullah  
- Ayesha Mahsood  

---

## 📜 License

This project is developed **for academic purposes only (FYP)** and is **not intended for commercial use**.
