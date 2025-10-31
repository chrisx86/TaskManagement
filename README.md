# TaskManagment App - Collaborative To-Do List

A modern, high-performance, and collaborative desktop to-do list application for small to medium-sized teams. Built with C# on the .NET 8 platform using Windows Forms.

---

## ✨ Features

This application goes beyond a simple to-do list, offering a suite of features designed for team collaboration and efficient task management.

-   **👤 User Authentication & Roles**: Secure login with hashed passwords. Two roles: **Admin** and **User**.
-   **📝 Rich Task Management**:
    -   Create, edit, and delete tasks with titles, priorities, due dates, and detailed notes.
    -   **Rich Text Notes**: Utilize a full-featured rich text editor for comments, supporting styles (bold, italic), colors, highlights, bullet points, and code snippets.
    -   Assign tasks to any team member.
-   **🚀 High-Performance UI**:
    -   **Virtual Mode DataGridView**: Effortlessly handles tens of thousands of tasks with buttery-smooth scrolling and instant filtering, thanks to an advanced data caching and prefetching mechanism.
    -   **Asynchronous Operations**: All database and network operations are fully asynchronous, ensuring the UI never freezes.
-   **🔍 Powerful Filtering & Sorting**:
    -   Instantly filter tasks by status, user relationship (assigned to me, created by me), assigned user, or search keywords.
    -   Sort by any column with a single click.
-   **👑 Admin Dashboard**:
    -   A comprehensive overview of all tasks across the entire team.
    -   At-a-glance statistics cards for key metrics (total tasks, overdue, unassigned, etc.).
    -   Interactive tree view to explore tasks grouped by user.
    -   Global search and filtering capabilities.
-   **🔐 Secure & Robust**:
    *   **"Remember Me"**: Secure auto-login functionality using tokens stored safely with Windows Data Protection API (DPAPI).
    *   **Graceful Shutdown**: Ensures all background tasks (like sending emails or logging) are completed before the application closes, preventing data loss and exceptions.
    *   **Database Integrity**: Optimized for shared network drive usage with robust transaction handling.
-   **📜 Audit Trail**:
    *   Complete history tracking for every task. All creations, updates, and deletions are logged with user and timestamp information.

---

## 🛠️ Tech Stack & Architecture

This project is built following modern software engineering principles to ensure it is maintainable, scalable, and robust.

-   **Platform**: .NET 8
-   **Language**: C# 12
-   **UI Framework**: Windows Forms (WinForms)
-   **Database**: SQLite
-   **ORM**: Entity Framework Core 8
-   **Architecture**: Classic 3-Tier Architecture
    -   **`TodoApp.WinForms` (Presentation Layer)**: The main UI project, containing all forms and user controls.
    -   **`TodoApp.Core` (Core Layer)**: Defines the "contracts" of the application, including data models (Entities, DTOs) and service interfaces. It has zero dependencies on other layers.
    -   **`TodoApp.Infrastructure` (Infrastructure Layer)**: Contains the concrete implementation of services, data access logic (`DbContext`), and interaction with external systems.
-   **Testing**: Unit tests are written using NUnit and Moq to ensure the reliability of the business logic.

---

## 🚀 Getting Started

### Prerequisites

-   .NET 8 SDK
-   Visual Studio 2022 or later

### Installation & Running

1.  **Clone the repository**:
    ```bash
    git clone https://github.com/chrisx86/TaskManagmentApp.git
    ```
2.  **Open the solution**:
    -   Navigate to the cloned directory and open `TodoApp.sln` in Visual Studio.
3.  **Configure the database path**:
    -   Open the `appsettings.json` file located in the `TodoApp.WinForms` project.
    -   Modify the `DefaultConnection` string to point to your desired database location (can be a local path or a network share path).
    ```json
    "ConnectionStrings": {
      "DefaultConnection": "Data Source=C:\\Path\\To\\Your\\Database\\TaskManagment.db;Cache=Shared"
    }
    ```
4.  **Run the application**:
    -   Set `TodoApp.WinForms` as the startup project.
    -   Press `F5` to build and run the application in debug mode, or `Ctrl+F5` to run without debugging.

### First-Time Login

-   The database is automatically created on the first run.
-   An initial administrator account is seeded into the database:
    -   **Username**: `admin`
    -   **Password**: `admin`
-   It is highly recommended to change the admin password immediately after the first login.
