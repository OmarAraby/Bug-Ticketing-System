

# 🐞 Bug Ticketing System API

A powerful and extensible RESTful API for managing software bug tracking across projects and teams. Built with **ASP.NET Core**, it follows N-Tiers Architecture principles to ensure maintainability, testability, and scalability.

---

## 🚀 Features

- ✅ **User Authentication** – Secure registration and login via JWT.
    
- 🧑‍💻 **Project Management** – Create and manage software projects.
    
- 🐛 **Bug Tracking** – Report, assign, and manage bugs.
    
- 📎 **Attachment Handling** – Upload, retrieve, and delete bug-related files.
    
- 🔐 **JWT Authorization** – Token-based authentication for secure endpoints.
    
- 🧱 **N-tier Architecture** – Proper separation of concerns: API, Business Logic, and Data Layer.
    

---

## 🏗️ Tech Stack

- ASP.NET Core 9
    
- Entity Framework Core
    
- SQL Server
    
- JWT Authentication
    
- FluentValidation
    
- N-Tier Architecture
    

---

## 📁 Folder Structure

```
BugTicketingSystem/
├── BugTicketingSystem.APIs/
│   ├── Controllers/
│   │   ├── AttachmentsController.cs
│   │   ├── BugsController.cs
│   │   ├── ProjectsController.cs
│   │   └── UserController.cs
│   ├── HandleFiles/
│   │   ├── FileService.cs
│   │   ├── FileUploadRequest.cs
│   │   ├── FileUploadResult.cs
│   │   └── IFileService.cs
│   ├── Middleware/
│   │   └── ExceptionHandlingMiddleware.cs
│   ├── Upload/ (directory)
│   ├── appsettings.json
│   ├── BugTicketingSystem.APIs.http
│   └── Program.cs
│
├── BugTicketingSystem.BL/
│   ├── Dtos/
│   │   ├── AttachmentDtos/
│   │   │   ├── AttachmentCreateDto.cs
│   │   │   ├── AttachmentDto.cs
│   │   │   └── Validator/
│   │   │       └── AttachmentCreateValidator.cs
│   │   ├── BugDtos/
│   │   │   ├── AssignUserToBugDto.cs
│   │   │   ├── BugCreateDto.cs
│   │   │   ├── BugDetailsDto.cs
│   │   │   ├── BugListDto.cs
│   │   │   └── Validator/
│   │   │       ├── AssignUserToBugDtoValidator.cs
│   │   │       └── BugValidator.cs
│   │   ├── ProjectDtos/
│   │   │   ├── ProjectCreateDto.cs
│   │   │   ├── ProjectDetailsDto.cs
│   │   │   └── Validator
│   │   └── UserDtos/
│   │       ├── AuthResponseDto.cs
│   │       ├── UserDetailsDto.cs
│   │       ├── UserLoginDto.cs
│   │       ├── UserRegisterDto.cs
│   │       └── Validator
│   ├── Exceptions/
│   │   └── BLValidationException.cs
│   ├── Managers/
│   │   ├── Attachment/
│   │   │   ├── AttachmentManager.cs
│   │   │   └── IAttachmentManager.cs
│   │   ├── Bug/
│   │   │   ├── BugManager.cs
│   │   │   └── IBugManager.cs
│   │   ├── Project/
│   │   │   ├── IProjectManager.cs
│   │   │   └── ProjectManager.cs
│   │   └── User/
│   │       ├── IUserManager.cs
│   │       └── UserManager.cs
│   ├── Services/
│   │   └── JWTService.cs
│   └── Utils/
│       ├── Error/
│       │   ├── APIError.cs
│       │   └── APIResult.cs
│       └── BusinessExtensions.cs
│
└── BugTicketingSystem.DL/
    ├── Context/
    │   └── BugTicketingSystemDbContext.cs
    ├── EntitiesConfiguration/
    │   ├── AttachmentConfiguration.cs
    │   ├── BugConfiguration.cs
    │   ├── BugUserConfiguration.cs
    │   ├── ProjectConfiguration.cs
    │   ├── UserConfiguration.cs
    │   └── UserRoleConfiguration.cs
    ├── Migrations/
    │   ├── 20250430021210_InitialCreate.cs
    │   └── BugTicketingSystemDbContextModelSnapshot.cs
    ├── Models/
    │   ├── Attachment.cs
    │   ├── Bug.cs
    │   ├── BugUser.cs
    │   ├── Project.cs
    │   ├── User.cs
    │   └── UserRole.cs
    ├── Repository/
    │   ├── AttachmentRepository/
    │   │   └── AttachmentRepository.cs
    │   ├── BugRepository/
    │   │   └── BugRepository.cs
    │   ├── BugUserRepository/
    │   │   └── BugUserRepository.cs
    │   ├── ProjectRepository/
    │   │   └── ProjectRepository.cs
    │   ├── UserRepository/
    │   │   └── UserRepository.cs
    │   └── UserRoleRepository/
    │       └── UserRoleRepository.cs
    └── UnitOfWork/
        ├── UnitOfWork.cs
        └── DataAccessExtensions.cs
```

Key features of this structure:

1. **Clear Separation of Concerns**:
   - APIs: Contains all web-related components
   - BL: Business logic layer with DTOs, validators, and managers
   - DL: Data access layer with models, repositories, and configurations

2. **Consistent Naming**:
   - All folders use PascalCase
   - Files maintain their original names from your project
   - Related files are grouped together (e.g., Manager interfaces with implementations)

3. **Logical Grouping**:
   - DTOs and their validators are colocated
   - Each entity has its own configuration in DL
   - Repository pattern implemented with separate folders per entity

4. **Standard .NET Conventions**:
   - Controllers in API project
   - Migrations in dedicated folder
   - Middleware in separate folder


---

## ⚙️ Getting Started

### 📋 Prerequisites

- [.NET 9 SDK](https://dotnet.microsoft.com/download/dotnet/9.0)
    
- SQL Server 
    
- [Postman](https://www.postman.com/) for API testing
    

### 🛠️ Installation

1. **Clone the Repository**
    
    ```bash
    git clone https://github.com/your-username/bug-ticketing-system.git
    cd bug-ticketing-system
    ```
    
2. **Configure the Database**
    
    - Update your connection string in `appsettings.json`
        
    - Run migrations:
        
        ```bash
        dotnet ef migrations add InitialCreate
        dotnet ef database update
        ```
        
3. **Restore Dependencies**
    
    ```bash
    dotnet restore
    ```
    
4. **Run the Application**
    
    ```bash
    dotnet run
    ```
    
5. API will be live at: `http://localhost:5285`
    

---

## 🔐 Authentication

This API uses **JWT Tokens** for securing endpoints.

> 🔒 _Enable JWT authorization in the code by uncommenting the `[Authorize]` attributes._

### Sample Login Response:

```json
{
  "userId": "guid",
  "userName": "JohnDoe",
  "email": "john@example.com",
  "token": "<JWT_TOKEN>",
  "expireDate": "2025-05-02T03:46:30Z"
}
```

Use the token in the `Authorization` header:

```
Authorization: Bearer <JWT_TOKEN>
```


## 📌 API Endpoints


### 👤 User Management

|Method|Endpoint|Description|
|---|---|---|
|POST|`/api/users/register`|Register a new user|
|POST|`/api/users/login`|Login and receive JWT token|

---

### 📂 Project Management

|Method|Endpoint|Description|
|---|---|---|
|POST|`/api/projects`|Create a new project|
|GET|`/api/projects`|List all projects|
|GET|`/api/projects/:id`|Get specific project info + related bugs|

---

### 🐞 Bug Management

|Method|Endpoint|Description|
|---|---|---|
|POST|`/api/bugs`|Create (report) a new bug|
|GET|`/api/bugs`|List all bugs|
|GET|`/api/bugs/:id`|Get detailed info for a specific bug|

---

### 👥 User-Bug Relationships

| Method | Endpoint                         | Description                     |
| ------ | -------------------------------- | ------------------------------- |
| POST   | `/api/bugs/:id/assign`           | Assign a user to a bug          |
| DELETE | `/api/bugs/:id/unassign/:userId` | Remove user assignment from bug |
|        |                                  |                                 |

---

### 📎 File Management

|Method|Endpoint|Description|
|---|---|---|
|POST|`/api/bugs/:id/attachments`|Upload attachment to bug|
|GET|`/api/bugs/:id/attachments`|Get all attachments for a bug|
|DELETE|`/api/bugs/:id/attachments/:attachmentId`|Delete a specific attachment|

---

## 🧪 Testing

You can use **Postman** or Swagger (if enabled) to test all endpoints. Make sure to include the Bearer token for protected routes.

---

## 💡 Future Enhancements

- 📝 Comment system for bugs
    
- 📊 Bug statistics and analytics
    
- 📬 Email notifications
    
- 🔍 Advanced search and filtering
    

---

## 🧑‍💻 Contributing

1. Fork the repo
    
2. Create a new branch: `feature/your-feature-name`
    
3. Commit your changes
    
4. Push and open a PR
    

---

## 📃 License

MIT License. See `LICENSE` file for details.

---

## 🙌 Acknowledgements

Built with ❤️ using ASP.NET Core by ***Omar Araby***.

