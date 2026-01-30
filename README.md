ICIT Department Portal – Backend (ASP.NET Core Web API)

Overview

This repository contains the backend solution for the ICIT Department Portal FYP. It provides RESTful APIs to manage authentication, role-based access, student records, faculty supervision, clerk workflows, admin controls, and FYP proposal management.

The backend is designed to support a multi-module portal system that facilitates seamless interaction between students, faculty, clerks, and administrators. It ensures data integrity, security, and efficient workflow management across various academic and administrative processes.

Architecture

Follows Clean Architecture principles with layered separation:

/src
├── Application          # Application logic (DTOs, interfaces)
│   └── Application_Service
├── Core                 # Domain models & business rules
│   └── Domain_Service
├── Infrastructure       # Database, external services
│   └── Infrastructure_Service
├── Presentation         # API Gateway & Controllers
│   └── APIGateway_Service
└── Solution Items       # Global configs (Directory.Packages.Props)

Tech Stack

Framework: ASP.NET Core Web API (C#)

Architecture: Clean Architecture (Layered)

Database: Azure SQL Server

Authentication: JWT + Password Hashing + CNIC/RollNo based login

Features

Secure authentication with CNIC + RollNo

Role-based access control (Student, Faculty, Clerk, Admin)

Bulk student entry via Clerk module

Proposal lock system for FYP integrity

Transcript and fee record management

Announcements and downloads APIs

Modular API Gateway for routing

How to Run

Clone the repository:

git clone https://github.com/Asim-AKM/ICIT-Department-Portal.git

Navigate to API Gateway:

cd src/Presentation/APIGateway_Service
dotnet run

Configure database in appsettings.json

Apply EF Core migrations:

dotnet ef database update

API will run locally on https://localhost:5001

Team Members

Asim Khan

Amsa Mansoor

Nizam Ullah

Ayesha Mahsood

License

This project is for academic purposes (FYP). Not intended for commercial use.