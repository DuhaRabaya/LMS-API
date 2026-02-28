# 🎓 LMS – Learning Management System API

A full-featured **ASP.NET Core Web API** for managing online courses, instructors, students, enrollments, tasks, grading, and payments.

This project follows **N-Tier Architecture** with clean separation of concerns, secure authentication, and scalable backend best practices.

---

## 📌 Project Overview

The LMS API allows:

- Students to enroll in courses, track progress, submit tasks, and view final marks.
- Instructors to create courses, manage content, create tasks, and grade submissions.
- Admins to manage users and approve instructor registrations.

---

## 🏗️ Architecture & Design

- N-Tier Architecture
- Generic Repository Pattern
- Global Exception Handling
- Audit Logging
- Localization Support
- Clean Separation of Concerns

---

## 🔐 Authentication & Authorization

- Register & Login
- Email Confirmation
- JWT Access Tokens
- Refresh Tokens
- Update Password
- Role-Based Authorization (Admin / Instructor / Student)
- CORS Policy Configuration

---

## 👥 User Management

- Managing user accounts
- Instructor registration (Admin approval required)
- Identity integration
- Audit tracking system

---

## 📚 Course Management

- Create Course
- Update Course
- Delete Course
- Publish / Unpublish Course
- Course Thumbnail Upload (File Service)
- Course Translations (Localization)
- Get All Courses
- Get Courses by Instructor ID
- Pagination
- Search
- Filtering
- Sorting
- Discount on Course Price

---

## 🎟️ Enrollment System

- Student Enrollment in Courses
- Get Student Enrollments
- Stripe Checkout Integration
- Enrollment Cancellation (3-Day Refund Policy)
- Top Student Reward (Full Course Price Refund)

---

## 📝 Tasks & Submissions

- Implement Tasks
- Task Submission Workflow
- Grading Submission System
- Tasks Progress Tracking

---

## 📖 Course Content Management

- Add Content
- Update Content
- Delete Content
- Navigate Content (Next / Previous)
- Course Content Progress Tracking

---

## 📊 Dashboards & Performance

- Instructor Dashboard (Overall Progress)
- Student Dashboard (Overall Progress)
- Final Mark Endpoint
- Passing Percentage Logic

---

## 💳 Payment Integration

- Stripe Payment Integration for secure course enrollment
- Refund logic implementation

---

## 🌍 Localization

- Multi-language course translations
- Localized API responses

---

## 🧪 Seed Data

- Initial roles
- Default users
- Sample data for testing

---

## 🛠️ Tech Stack

- ASP.NET Core Web API
- Entity Framework Core
- SQL Server
- ASP.NET Identity
- JWT Authentication
- Stripe API

