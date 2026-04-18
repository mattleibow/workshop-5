# AI Agent Guide for Tailspin Toys

This document provides essential information to help AI agents understand and work effectively with the Tailspin Toys codebase.

## Project Overview

Tailspin Toys is a crowdfunding platform for games with a developer theme. The application consists of:

- **Backend**: ASP.NET Core Minimal API with Entity Framework Core
- **Frontend**: Blazor Interactive Server
- **Styling**: Tailwind CSS v4 (dark theme throughout)
- **Database**: SQLite
- **Testing**: xUnit (backend), Playwright (frontend)

## Quick Start

### Running the Application

```bash
# Linux/macOS/Codespaces
./scripts/start-app.sh

# Windows (PowerShell)
.\scripts\start-app.ps1

# Access at http://localhost:4321
```

### Running Tests

```bash
# Linux/macOS/Codespaces
./scripts/run-server-tests.sh    # Backend tests
./scripts/run-e2e-tests.sh       # Frontend E2E tests

# Windows (PowerShell)
.\scripts\run-server-tests.ps1   # Backend tests
.\scripts\run-e2e-tests.ps1      # Frontend E2E tests
```

## Repository Structure

```
tailspin-toys/
├── server/                         # ASP.NET Core backend
│   ├── TailspinToys.Api/          # API project
│   │   ├── Models/                # Entity Framework Core models
│   │   ├── Routes/                # API endpoints (Minimal API route groups)
│   │   ├── Utils/                 # Helper functions
│   │   └── Program.cs            # Application initialization
│   └── TailspinToys.Api.Tests/   # xUnit tests
├── client/                        # Blazor frontend
│   ├── TailspinToys.Web/         # Blazor web project
│   │   ├── Components/
│   │   │   ├── Layout/           # Layout components
│   │   │   ├── Pages/            # Page components (file-based routing)
│   │   │   └── Shared/           # Reusable components
│   │   ├── Models/               # Client-side data models
│   │   └── wwwroot/              # Static files
│   └── TailspinToys.E2E/         # Playwright E2E tests (C#/xUnit)
├── scripts/                       # Development automation scripts
├── data/                          # SQLite database files
└── .github/
    └── instructions/              # Technology-specific guidelines
```

## Technology-Specific Instructions

Before working with any technology, **read the corresponding instruction file**:

| Technology | Instruction File | Purpose |
|------------|-----------------|---------|
| Blazor components | `.github/instructions/blazor.instructions.md` | Component patterns, routing, state |
| Tailwind CSS | `.github/instructions/tailwindcss.instructions.md` | Dark theme patterns, utility classes |
| ASP.NET Core endpoints | `.github/instructions/aspnetcore-endpoint.instructions.md` | Route groups, REST conventions |
| .NET tests | `.github/instructions/dotnet-tests.instructions.md` | Test structure, xUnit patterns |
| Playwright tests | `.github/instructions/playwright.instructions.md` | E2E test patterns, locators |

## Critical Patterns

### Backend (ASP.NET Core Minimal API + Entity Framework Core)

**Models** (`server/TailspinToys.Api/Models/`)
- Use Entity Framework Core with data annotations
- Include `ToDict()` method for JSON serialization
- Define relationships clearly with navigation properties

**Routes** (`server/TailspinToys.Api/Routes/`)
- Create Minimal API route groups per resource
- Use extension methods: `app.MapGamesRoutes()`
- Pattern: `group.MapGet("/", async (TailspinToysContext db) => ...)`
- Register routes in `Program.cs`

**API Response Format**
```csharp
// Success (200)
return Results.Ok(data);

// Not Found (404)
return Results.NotFound(new { error = "Resource not found" });

// Bad Request (400)
return Results.BadRequest(new { error = "Invalid input" });
```

### Frontend (Blazor Interactive Server)

**Component Pattern**
```razor
@page "/example"
@inject HttpClient Http

<div class="bg-slate-800 rounded-xl p-6">
    @if (loading)
    {
        <LoadingSkeleton />
    }
    else
    {
        <h2 class="text-slate-100">@title</h2>
    }
</div>

@code {
    private bool loading = true;
    private string title = "";

    protected override async Task OnInitializedAsync()
    {
        // Fetch data from API
        loading = false;
    }
}
```

**Tailwind Dark Theme**
```html
<!-- Standard card pattern -->
<div class="bg-slate-800 rounded-xl p-6 shadow-lg border border-slate-700">
  <h2 class="text-slate-100 text-2xl font-bold">Title</h2>
  <p class="text-slate-300">Description</p>
</div>
```

## Testing Requirements

### Backend Tests

**Structure** (`server/TailspinToys.Api.Tests/`)
- Use xUnit with `WebApplicationFactory<Program>`
- Use InMemoryDatabase for test isolation
- Define test data as class constants
- Implement `IDisposable` for cleanup

### Frontend Tests

**Existing E2E Tests** (`client/TailspinToys.E2E/`)

The project includes comprehensive Playwright E2E tests written in C# with xUnit:
- `HomeTests.cs` - Homepage display and content
- `GamesTests.cs` - Game listing, navigation, and details pages
- `AccessibilityTests.cs` - Accessibility compliance tests

**Before creating new E2E tests**, check existing coverage in `client/TailspinToys.E2E/`.

**Playwright Patterns**
- Use role-based locators: `getByRole`, `getByLabel`, `getByText`
- Use `test.step()` for grouping
- Auto-retrying assertions: `await expect(locator).toHaveText()`
- **NEVER** use `waitForTimeout` or hard-coded waits

**Testability Requirement**
- **ALL** interactive elements MUST have `data-testid` attributes
- Use descriptive IDs: `data-testid="game-card"`

## Development Workflow

### Before Starting Work
1. **Explore the project** - understand existing patterns
2. **Read relevant instruction files** - follow established conventions
3. **Create TODO list** for complex tasks

### Making Changes
1. **Make minimal changes** - surgical, precise edits only
2. **Follow existing patterns** - consistency is critical
3. **Use absolute paths** - for scripts and file operations
4. **Add type annotations** - required for all C# code

### Before Committing
1. **Run backend tests**: `./scripts/run-server-tests.sh` (or `.\scripts\run-server-tests.ps1` on Windows)
2. **Run frontend tests** (if UI changed): `./scripts/run-e2e-tests.sh` (or `.\scripts\run-e2e-tests.ps1` on Windows)
3. **Update tests** - if adding/modifying API endpoints or UI
4. **Update README** - if adding new functionality
5. **Update instructions** - if patterns change

## Common Tasks

### Adding a New API Endpoint

1. Read `.github/instructions/aspnetcore-endpoint.instructions.md`
2. Create/update route group in `server/TailspinToys.Api/Routes/`
3. Register routes in `Program.cs`
4. Add tests in `server/TailspinToys.Api.Tests/`
5. Run `./scripts/run-server-tests.sh` (or `.\scripts\run-server-tests.ps1` on Windows)

### Adding a New Page

1. Read `.github/instructions/blazor.instructions.md`
2. Create `.razor` file in `client/TailspinToys.Web/Components/Pages/`
3. Use existing layout with `@layout MainLayout`
4. Add interactive components with `@rendermode InteractiveServer`

### Creating a Component

1. Read `.github/instructions/blazor.instructions.md`
2. Create `.razor` file in `Components/Shared/`
3. Use `[Parameter]` for component inputs
4. Add `data-testid` to interactive elements
5. Apply Tailwind dark theme classes

## Available Scripts

| Script (Bash) | Script (PowerShell) | Purpose |
|---------------|---------------------|---------|
| `./scripts/setup-env.sh` | `.\scripts\setup-env.ps1` | Restore .NET dependencies |
| `./scripts/start-app.sh` | `.\scripts\start-app.ps1` | Start both backend and frontend servers |
| `./scripts/run-server-tests.sh` | `.\scripts\run-server-tests.ps1` | Run xUnit tests |
| `./scripts/run-e2e-tests.sh` | `.\scripts\run-e2e-tests.ps1` | Run Playwright E2E tests |

**Always use existing scripts** rather than performing operations manually. Use `.sh` scripts on Linux/macOS/Codespaces and `.ps1` scripts on Windows.

## Key Principles

### Agent Behavior
- ✅ Explore before generating code
- ✅ Read instruction files before working with technology
- ✅ Create TODO lists for multi-step tasks
- ✅ Use absolute paths for scripts
- ❌ Don't generate summary markdown files
- ❌ Don't ignore existing patterns

### Code Quality
- ✅ Type annotations for all C# code
- ✅ Descriptive variable/function names
- ✅ Follow REST conventions for APIs
- ✅ Dark theme throughout UI
- ✅ Accessibility (semantic HTML, ARIA labels)

### Testing
- ✅ Test all API endpoints
- ✅ Add `data-testid` to interactive elements
- ✅ Use web-first assertions in Playwright
- ✅ Run tests before committing
- ❌ Don't skip test coverage for new features

## Database Schema

Key models in `server/TailspinToys.Api/Models/`:
- **Game**: Crowdfunding game projects (title, description, star rating)
- **Category**: Game categories (Strategy, Puzzle, etc.)
- **Publisher**: Game publishers

Relationships use Entity Framework Core with Include for eager loading.

## API Conventions

- **Base URL**: `/api/<resource>`
- **GET** `/api/games` - List all games
- **GET** `/api/games/<id>` - Get single game

All endpoints return JSON with appropriate status codes.

## Accessibility Requirements

- Use semantic HTML elements (`<nav>`, `<main>`, `<article>`, `<button>`)
- Provide ARIA labels where needed
- Ensure keyboard navigation works
- Visible focus states: `focus:ring-2 focus:ring-blue-500`
- Sufficient color contrast in dark theme

## GitHub Actions

When creating workflows (`.github/workflows/`):
- Set explicit permissions
- Follow security best practices
- Add comments documenting tasks
- Test thoroughly before committing

## Questions?

1. Check the relevant instruction file in `.github/instructions/`
2. Review existing code for patterns
3. Examine test files for usage examples
4. Refer to `copilot-instructions.md` for general guidelines
