# Mio E-Learning

## Overview
Mio E-Learning is a web-based learning management system developed as part of an internship at Saigon Mio Technology JSC during the third week (June 2025). The project focuses on implementing a layered architecture to enhance modularity, maintainability, and scalability. It provides a platform for managing educational content, user interactions, and course-related functionalities.

![home](https://github.com/user-attachments/assets/27287c34-f3ed-45e0-baa6-421def986e1f)

- **GitHub Repository**: [github.com/dinhduc0794/mio-e-learning](https://github.com/dinhduc0794/mio-e-learning)
- **Duration**: Week 3 of Internship (Semester 243, 2024-2025)
- **Role**: Backend Developer
- **Mentor**: Ngô Đặng Anh Khoa

## Objectives
- Design and implement a layered architecture (Core, Data, Service, AdminSite) to ensure clear separation of concerns.
- Develop a robust backend system using ASP.NET Core MVC to handle course management and user interactions.
- Enhance user experience with a responsive and interactive frontend.

## Technologies
- **Backend**: C#, ASP.NET Core MVC 8.0, Entity Framework Core, MySQL
- **Frontend**: HTML5, CSS3, Bootstrap 5, AJAX, JQuery
- **Tools**: Git, GitHub, Visual Studio

## System Architecture
The project follows a **Layered Architecture** to ensure modularity and maintainability:
- **Core Layer**: Defines core entities (e.g., Course, User) and business logic interfaces.
- **Data Layer**: Manages database interactions using Entity Framework Core, with models like `Course.cs` and `User.cs`, and a `MioDbContext.cs` for database context.
- **Service Layer**: Implements business logic in services like `ICourseService.cs` and `IUserService.cs`, ensuring separation from data access and presentation.
- **AdminSite Layer**: Handles presentation logic, including controllers (e.g., `CourseController.cs`, `HomeController.cs`) and views (`.cshtml` files) for the admin interface.

This structure adheres to SOLID principles, enabling easy maintenance and scalability.

## Key Features
- **Course Management**: Create, update, and delete courses with detailed attributes.
- **User Management**: Handle user profiles, roles, and authentication.
- **Responsive UI**: Interactive admin interface for managing educational content, built with Bootstrap 5, AJAX, and JQuery for dynamic updates.
- **Data Access**: Efficient database operations using Entity Framework Core with MySQL.

## Project Structure
```
mio-e-learning/
├── src/
│   ├── Core/
│   │   ├── Entities/          # Course.cs, User.cs, etc.
│   │   ├── Interfaces/       # IService interfaces
│   ├── Data/
│   │   ├── MioDbContext.cs   # Database context
│   │   ├── Repositories/     # IRepository and implementations
│   ├── Service/
│   │   ├── CourseService.cs  # Business logic for courses
│   │   ├── UserService.cs    # Business logic for users
│   ├── AdminSite/
│   │   ├── Controllers/      # CourseController.cs, HomeController.cs
│   │   ├── Views/            # .cshtml files for UI
│   │   ├── wwwroot/          # Static files (CSS, JS)
├── migrations/               # EF Core migrations
├── README.md
```
## Demonstration
![course_all](https://github.com/user-attachments/assets/01787123-25d6-4e3a-afb0-d353b710b696)
![course_add_1](https://github.com/user-attachments/assets/2a2c2d8a-5b55-468e-af2f-075546bc7224)
![course_add_2](https://github.com/user-attachments/assets/70d589f1-2387-4b1b-9490-52a00886a072)
![course_add_3](https://github.com/user-attachments/assets/ee971377-8aee-4b92-bd5e-170fb1eec4fb)
![course_add_res](https://github.com/user-attachments/assets/1d72246c-83ad-46b0-8883-f94ed0fec9db)
![course_edit](https://github.com/user-attachments/assets/94a46c99-43dd-41aa-91c8-a3873eede9ab)
![course_edit_res](https://github.com/user-attachments/assets/119c3363-6a75-4775-8be2-2676f6972ba1)

## Achievements
- Successfully implemented a layered architecture, improving system modularity and maintainability.
- Developed core functionalities for course and user management, integrated with a MySQL database.
- Gained proficiency in ASP.NET Core MVC, Entity Framework Core, and modern frontend technologies.
- Learned effective team collaboration and professional software development practices in a real-world environment.

## Getting Started
1. **Clone the repository**:
   ```bash
   git clone https://github.com/dinhduc0794/mio-e-learning.git
   ```
2. **Set up the environment**:
   - Install .NET SDK 8.0 and MySQL.
   - Configure the database connection in `appsettings.json`.
3. **Run migrations**:
   ```bash
   dotnet ef migrations add InitialCreate
   dotnet ef database update
   ```
4. **Run the application**:
   ```bash
   dotnet run
   ```

## Future Improvements
- Add advanced features like course progress tracking and quiz management.
- Implement role-based access control for enhanced security.
- Optimize frontend performance with advanced JavaScript frameworks.

## Acknowledgments
Special thanks to my mentor, Ngô Đăng Anh Khoa, and the team at Saigon Mio Technology JSC for their guidance and support during the internship.
