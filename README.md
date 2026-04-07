# Acme Global College – MVC Application

## Overview
ASP.NET Core MVC application using Entity Framework Core and Identity.

The system manages students, courses, enrolments and faculty with role-based access.

---

## Roles

### Admin
- Full access
- Manage courses, students, enrolments and faculty
- View system dashboard

### Faculty
- View assigned courses
- View students enrolled in those courses
- Access student details (restricted to their courses)

### Student
- View own profile
- View own enrolments

---

## Core Features

- Role-based dashboards (Admin, Faculty, Student)
- Course details with:
  - Branch
  - Modules
  - Enrolled students
- Student details with enrolments
- Enrolment creation and linking
- Data filtering based on user role

---

## Entity Relationships

- Branch → Courses
- Course → Modules
- Course → Enrolments
- Student → Enrolments
- Faculty → Course assignments

---

## Authentication

- ASP.NET Core Identity
- Role-based authorization

---

## Seeded Accounts

### Admin
admin@acmeglobal.ie / AcmeVGC!26

### Faculty
albus.dumbledore@acmeglobal.ie / Dumbledore!26  
severus.snape@acmeglobal.ie / Snape!26  
minerva.mcgonagall@acmeglobal.ie / McGonagall!26  

### Students
hermione.granger@acmeglobal.ie / Hermione!26  
harry.potter@acmeglobal.ie / Harry!26  
luna.lovegood@acmeglobal.ie / LunaLove!26  

---

## Run

```bash
cd src/AcmeGlobalCollege.Web
dotnet run

Open:
http://localhost:5205

Notes
All features are integrated across entities
Access is restricted based on user role
Designed to meet assessment requirements for workflows and integration

EOF


---