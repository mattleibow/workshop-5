---
description: 'Playwright E2E test patterns and locator conventions'
applyTo: '**/TailspinToys.E2E/**/*.cs'
---

# Playwright E2E Test Instructions

## Test Patterns

### Test Structure

```csharp
using Microsoft.Playwright;

public class FeatureTests : PlaywrightTestBase
{
    [Fact]
    public async Task ShouldDoSomethingSpecific()
    {
        // Navigate to page
        await Page.GotoAsync("/");

        // Verify content
        var element = Page.GetByTestId("element-id");
        await Expect(element).ToBeVisibleAsync();
    }

    private static ILocatorAssertions Expect(ILocator locator) => Assertions.Expect(locator);
    private static IPageAssertions Expect(IPage page) => Assertions.Expect(page);
}
```

### Locator Strategies (Priority Order)

1. **`GetByTestId`** - For elements with `data-testid`
2. **`GetByRole`** - For semantic HTML elements
3. **`GetByText`** - For text content
4. **`GetByLabel`** - For form elements

### Auto-Retrying Assertions

Always use `await Expect()` for assertions:
```csharp
await Expect(Page.GetByTestId("games-grid")).ToBeVisibleAsync();
await Expect(Page.GetByTestId("game-title")).Not.ToBeEmptyAsync();
await Expect(Page).ToHaveURLAsync("/game/1");
```

### Important Rules

- **NEVER** use `Task.Delay` or hard-coded waits
- **NEVER** use `WaitForSelectorAsync` to wait after an interaction — if the element is already in the DOM it returns immediately, causing flaky tests. Use `Expect()` assertions instead (see below).
- Use descriptive test method names
- Take screenshots only on failure
- Use `data-testid` for all interactive elements
- All test classes should extend `PlaywrightTestBase`

### Waiting After Interactions

After clicking, selecting, or any user interaction, **always wait for the semantic consequence** using auto-retrying `Expect()` assertions. Never use `WaitForSelectorAsync` or `WaitForLoadStateAsync` as a proxy for "loading finished" — if the element is already present they return immediately.

```csharp
// ✅ Correct — waits for the filter to take effect (auto-retries until true)
await categorySelect.SelectOptionAsync(new SelectOptionValue { Value = "1" });
await Expect(Page.GetByTestId("game-category").First).ToContainTextAsync("Strategy");

// ✅ Correct — waits for count to match (auto-retries)
await Page.GetByTestId("filter-clear-button").ClickAsync();
await Expect(Page.GetByTestId("game-card")).ToHaveCountAsync(initialCount);

// ❌ Wrong — returns immediately if games-grid is already in the DOM
await Page.WaitForSelectorAsync("[data-testid='games-grid']");
```

### Blazor Interactive Server Testing

Components with `@rendermode InteractiveServer` are rendered twice: once by SSR (initial HTML, no event handlers) and again when Blazor's SignalR circuit connects. DOM elements can be present in the page before the circuit is ready, so interacting with them immediately after navigation may have no effect.

**Always wait for the component's `data-interactive="true"` attribute before interacting:**

```csharp
/// <summary>Waits for the filter panel's Blazor circuit to be connected.</summary>
private async Task WaitForInteractiveAsync(string testId = "game-filter-panel")
{
    await Page.Locator($"[data-testid='{testId}'][data-interactive='true']")
              .WaitForAsync(new() { Timeout = 15000 });
}
```

Call this before any `SelectOptionAsync`, `ClickAsync`, or other interactions on Blazor interactive components:

```csharp
await Page.GotoAsync("/");
await WaitForInteractiveAsync("game-filter-panel");
// Now safe to interact with the filter dropdowns
await Page.GetByTestId("filter-category").SelectOptionAsync(...);
```

### Available Test IDs

- `games-grid` - Games grid container
- `game-card` - Individual game card
- `game-title` - Game title in card
- `game-category` - Category badge (conditional when data is present)
- `game-publisher` - Publisher badge (conditional when data is present)
- `game-description` - Game description
- `game-details` - Game details container
- `game-details-title` - Game details title
- `game-details-description` - Game details description
- `game-details-category` - Category in details
- `game-details-publisher` - Publisher in details
- `game-rating` - Star rating display
- `back-game-button` - Support This Game button
- `about-section` - About page section
- `about-heading` - About page heading
- `game-filter-panel` - Filter panel container on the games list page
- `filter-category` - Category filter dropdown (select element)
- `filter-publisher` - Publisher filter dropdown (select element)
- `filter-clear-button` - Clear all filters button (visible only when a filter is active)
