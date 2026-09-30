# Used Books API

An ASP.NET Core learning project for a used-book catalogue, using Entity Framework Core, SQLite and ASP.NET Core Identity.

## Code guide

| Path | Purpose |
| --- | --- |
| `UsedBooks/Controllers/BooksController.cs` | Book endpoints |
| `UsedBooks/Features/Book/` | Book, faculty and department models and repository |
| `UsedBooks/Data/` | EF context, seed logic and unit of work |
| `UsedBooks/HostingExtensions.cs` | Dependency injection, authentication and middleware |

## Local setup

The original project targets .NET 6. Use a compatible development SDK and an isolated local SQLite database. Set `ConnectionStrings__dbConnection` to a local SQLite connection string and set a new random `Jwt__SigningKey` in the environment. `.env.example` is a reference, not an automatically loaded file.

```bash
dotnet restore UsedBooks.sln
dotnet build UsedBooks.sln
dotnet run --project UsedBooks/UsedBooks.csproj
```

Review `UsedBooks/Data/ApplicationDbInitializer.cs` before starting: startup invokes the seed routine. Use sample data only. Swagger configuration is in the startup code.

## Status

Historical learning project, not a production marketplace. Generated build output, editor state and the local database are not source files. Production authentication, dependency migration and automated integration tests remain separate work. Replace any signing key previously taken from this source before using the application.
