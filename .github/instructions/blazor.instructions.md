---
description: 'Blazor component patterns for pages, layouts, and routing'
applyTo: '**/*.razor'
---

# Blazor Component Instructions

## Blazor Component Patterns

Blazor is used for page routing, layouts, and interactive components. All UI is built with Razor components.

### Component Structure

```razor
@page "/example"
@using TailspinToys.Web.Models
@inject HttpClient Http

<PageTitle>Example - Tailspin Toys</PageTitle>

<div class="py-8">
    @if (loading)
    {
        <LoadingSkeleton />
    }
    else if (error is not null)
    {
        <ErrorMessage Error="@error" />
    }
    else
    {
        <!-- Content here -->
    }
</div>

@code {
    private bool loading = true;
    private string? error;

    protected override async Task OnInitializedAsync()
    {
        try
        {
            // Fetch data
        }
        catch (Exception ex)
        {
            error = ex.Message;
        }
        finally
        {
            loading = false;
        }
    }
}
```

### Page Routing

- Use `@page "/path"` directive for routes
- Use `@page "/game/{Id:int}"` for parameterized routes
- Set page title with `<PageTitle>` component

### Layout Pattern

```razor
@inherits LayoutComponentBase

<Header />
<main class="container mx-auto py-6 h-full max-w-7xl">
    <div class="px-4 sm:px-6 lg:px-8">
        @Body
    </div>
</main>
```

### Component Parameters

```razor
<!-- Parent -->
<GameCard Game="@game" />

<!-- Child (GameCard.razor) -->
@code {
    [Parameter]
    public Game Game { get; set; } = default!;
}
```

### Interactive Server Rendering

- Use `@rendermode InteractiveServer` for components that need interactivity
- Use `@inject HttpClient Http` for API calls
- Handle loading states and errors gracefully

### Data Fetching

```razor
@code {
    private List<Game> games = [];
    private bool loading = true;

    protected override async Task OnInitializedAsync()
    {
        games = await Http.GetFromJsonAsync<List<Game>>("/api/games") ?? [];
        loading = false;
    }
}
```
