# UserManagementAPI

A simple ASP.NET Core Web API for managing users with token-based authentication and Swagger UI developed to complete `Back-End Development with .NET` course by Microsoft.

## Prerequisites
- [.NET 9 SDK](https://dotnet.microsoft.com/download)
- Git

## Getting Started

1. **Clone the repository:**
   ```bash
   git clone https://github.com/masumbillah21/UserManagementAPI.git
   cd UserManagementAPI
   ```

2. **Restore dependencies:**
   ```bash
   dotnet restore
   ```

3. **Run the API:**
   ```bash
   dotnet watch run
   ```

4. **Open Swagger UI:**
   - Navigate to `https://localhost:5001/swagger` (or the URL shown in your terminal)
   - Use the "Authorize" button to enter your Bearer token (default: `secret`)

## API Endpoints
- `GET /api/users` - List users (requires token)
- `GET /api/users/{id}` - Get user by ID (requires token)
- `POST /api/users` - Create user (requires token, do not include `id` in request body)
- `PUT /api/users/{id}` - Update user (requires token)
- `DELETE /api/users/{id}` - Delete user (requires token)

## Authentication
- All `/api/users` endpoints require a Bearer token in the `Authorization` header.
- Default token: `secret`

## Customization
- Update the token in `Middleware/TokenValidationMiddleware.cs` as needed.
- Adjust user model and validation in `Model/User.cs`.

## Development
- All requests and responses are logged.
- Unhandled exceptions return a consistent JSON error response.

---

Feel free to contribute or open issues!