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
- Use descriptive test method names
- Take screenshots only on failure
- Use `data-testid` for all interactive elements
- All test classes should extend `PlaywrightTestBase`

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
