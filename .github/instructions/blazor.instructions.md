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

### SSR + Interactive Dual-Render Behaviour

Components with `@rendermode InteractiveServer` render **twice**:

1. **SSR pass** — Blazor runs `OnInitializedAsync` on the server and sends the full HTML to the browser (including dropdown options, lists, etc.). `OnAfterRenderAsync` does **NOT** run during SSR.
2. **Interactive pass** — After the page loads, Blazor's SignalR circuit connects in the browser and the component becomes interactive. `OnInitializedAsync` runs again, and `OnAfterRenderAsync(firstRender: true)` runs for the first time.

This has two important implications:

**`@onchange` / event handlers only work after the interactive pass.** During SSR the HTML looks complete but no event handlers are wired. Blazor's JavaScript intercepts DOM events and sends them to the server via SignalR — if the circuit isn't connected yet, events fire but nothing handles them.

**`OnAfterRenderAsync(firstRender: true)` is the only reliable signal that the circuit is connected.** Use it to set a `data-interactive` attribute so tests and other code can detect readiness:

```razor
@* Container div signals circuit readiness for tests *@
<div data-testid="my-interactive-panel" data-interactive="@(_isInteractive ? "true" : null)">
    @* content *@
</div>

@code {
    private bool _isInteractive = false;

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender)
        {
            _isInteractive = true;
            StateHasChanged();
        }
    }
}
```

Any E2E test that interacts with an `@rendermode InteractiveServer` component **must** wait for `data-interactive="true"` before firing events. See `playwright.instructions.md` for the `WaitForInteractiveAsync()` helper pattern.

### Client Models Must Mirror the API JSON Shape

Client-side models in `client/TailspinToys.Web/Models/` must exactly match the JSON returned by the API. Before writing a client model, check the server model's `ToDict()` method to understand the JSON structure.

- **Nested API objects must be nested client classes.** The API returns `publisher: { id, name }` — the client model needs a `Publisher` class, not a flat `[JsonPropertyName("publisher_name")] string PublisherName`.
- **Never add flat `[JsonPropertyName]` properties for relationship data** — they will always be `null` because the JSON key doesn't exist.

```csharp
// ✅ Correct — matches nested JSON: { "publisher": { "id": 1, "name": "Acme" } }
public class Game
{
    public int Id { get; set; }
    public string Title { get; set; } = "";
    public Publisher? Publisher { get; set; }   // populated from nested JSON
    public Category? Category { get; set; }      // populated from nested JSON
}

public class Publisher { public int Id { get; set; } public string Name { get; set; } = ""; }

// ❌ Wrong — "publisher_name" key does not exist in the API response; always null
[JsonPropertyName("publisher_name")]
public string? PublisherName { get; set; }
```

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
