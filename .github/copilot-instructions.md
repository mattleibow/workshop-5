# Tailspin Toys Crowd Funding Development Guidelines

This is a crowdfunding platform for games with a developer theme. The application uses an ASP.NET Core Minimal API backend with Entity Framework Core for database interactions, and a Blazor frontend with Tailwind CSS for styling. Please follow these guidelines when contributing:

## Agent notes

- Explore the project before beginning code generation
- Create todo lists for long operations
  - Before each step in a todo list, reread the instructions to ensure you always have the right directions
- Always use instructions files when available, reviewing before generating code
- Do not generate summary markdown files upon completion of a task
- Always use absolute paths when running scripts and BASH commands
- **Do NOT commit or push changes unless explicitly instructed to do so by the user**

## Code standards

### Required Before Each Commit

#### Testing guidelines

- Run .NET tests to ensure backend functionality, and Playwright tests to ensure e2e and frontend functionality
- Review the existing tests to ensure we're not duplicating efforts
- Test code should be of the same quality as the rest of the project, and follow DRY principles
- For frontend changes, run builds in the client directory to verify build success and the end-to-end tests, to ensure everything works correctly
- When making API changes, update and run the corresponding tests to ensure everything works correctly

#### Project guidelines

- When updating models, ensure database migrations are included if needed
- When adding new functionality, make sure you update the README
- Make sure all guidance in the Copilot Instructions file is updated with any relevant changes, including to project structure and scripts, and programming guidance

### Code formatting requirements

- When writing C#, you must use type annotations for return values and method parameters.
- Use nullable reference types when appropriate so generated code clearly communicates nullability expectations.
- Follow C# naming conventions (PascalCase for public members, camelCase for locals)

### C# and ASP.NET Core Patterns

- Use Entity Framework Core models for database interactions
- Use Minimal API route groups for organizing routes
- Follow RESTful API design principles

### Blazor Patterns

- **Blazor Components**: Use interactive server rendering for dynamic components
- **Pages**: Use Blazor page routing with `@page` directive
- Create reusable Razor components when functionality is used in multiple places
- Use `@rendermode InteractiveServer` for interactive components

### Styling

- Use Tailwind CSS utility classes exclusively - see `tailwindcss.instructions.md`
- Dark theme colors: slate palette (`bg-slate-800`, `text-slate-100`, etc.)
- Rounded corners and modern UI patterns
- Follow modern UI/UX principles with clean, accessible interfaces

### GitHub Actions workflows

- Follow good security practices
- Make sure to explicitly set the workflow permissions
- Add comments to document what tasks are being performed

## Scripts

- Several scripts exist in the `scripts` folder
- Always use available scripts to perform tasks rather than performing operations manually
- Existing scripts (use `.sh` on Linux/macOS/Codespaces, `.ps1` on Windows):
    - `scripts/setup-env`: Performs restoration of all .NET dependencies
    - `scripts/run-server-tests`: Runs all .NET backend tests
    - `scripts/run-e2e-tests`: Runs Playwright E2E tests for frontend
    - `scripts/start-app`: Calls setup-env, then starts both backend and frontend servers

## Repository Structure

- `server/`: ASP.NET Core backend code
  - `TailspinToys.Api/`: API project
    - `Models/`: Entity Framework Core models
    - `Routes/`: API endpoints organized by resource
    - `Utils/`: Utility functions and helpers
    - `Program.cs`: Application initialization
  - `TailspinToys.Api.Tests/`: xUnit tests for the API
- `client/`: Blazor frontend code
  - `TailspinToys.Web/`: Blazor web project
    - `Components/`: Reusable Razor components
    - `Components/Layout/`: Layout components
    - `Components/Pages/`: Page components with routing
    - `wwwroot/`: Static files
  - `TailspinToys.E2E/`: Playwright E2E tests in C# (home, games, accessibility)
- `scripts/`: Development and deployment scripts
- `data/`: Database files
- `README.md`: Project documentation
