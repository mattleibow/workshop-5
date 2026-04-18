// E2E tests for the game filtering feature.
// Verifies filter panel visibility, filtering by category and publisher,
// combined filters, and clearing filters to restore the full game list.

using Microsoft.Playwright;

namespace TailspinToys.E2E;

public class FilterTests : PlaywrightTestBase
{
    [Fact]
    public async Task ShouldDisplayFilterPanelOnHomePage()
    {
        // Navigate to homepage and wait for the page to load
        await Page.GotoAsync("/");
        await Page.WaitForSelectorAsync("[data-testid='games-grid']", new() { Timeout = 15000 });

        // Verify filter panel and its controls are visible
        await Expect(Page.GetByTestId("game-filter-panel")).ToBeVisibleAsync();
        await Expect(Page.GetByTestId("filter-category")).ToBeVisibleAsync();
        await Expect(Page.GetByTestId("filter-publisher")).ToBeVisibleAsync();
    }

    [Fact]
    public async Task ShouldFilterGamesByCategory()
    {
        // Navigate to homepage and wait for games and filter options to load
        await Page.GotoAsync("/");
        await Page.WaitForSelectorAsync("[data-testid='games-grid']", new() { Timeout = 15000 });
        await WaitForFilterOptionsAsync();

        // Verify categories dropdown loaded real options
        var categorySelect = Page.GetByTestId("filter-category");
        var categoryOptions = await categorySelect.Locator("option").AllAsync();
        Assert.True(categoryOptions.Count > 1, "Category dropdown must have options beyond 'All Categories'");

        // Collect game titles before filtering
        var titlesBefore = await Page.GetByTestId("game-title").AllInnerTextsAsync();
        Assert.NotEmpty(titlesBefore);

        // Select the first real category
        var firstCategoryText = (await categoryOptions[1].InnerTextAsync()).Trim();
        var firstCategoryValue = await categoryOptions[1].GetAttributeAsync("value");
        await categorySelect.SelectOptionAsync(new SelectOptionValue { Value = firstCategoryValue });

        // Wait for the grid to refresh
        await Page.WaitForSelectorAsync("[data-testid='games-grid']", new() { Timeout = 15000 });

        // Every visible category badge must match the selected category
        var categoryBadges = Page.GetByTestId("game-category");
        var badgeCount = await categoryBadges.CountAsync();
        Assert.True(badgeCount > 0, "Filtered results must include at least one game with a category badge");

        for (var i = 0; i < badgeCount; i++)
        {
            await Expect(categoryBadges.Nth(i)).ToContainTextAsync(firstCategoryText);
        }
    }

    [Fact]
    public async Task ShouldFilterGamesByPublisher()
    {
        // Navigate to homepage and wait for games and filter options to load
        await Page.GotoAsync("/");
        await Page.WaitForSelectorAsync("[data-testid='games-grid']", new() { Timeout = 15000 });
        await WaitForFilterOptionsAsync();

        // Verify publishers dropdown loaded real options
        var publisherSelect = Page.GetByTestId("filter-publisher");
        var publisherOptions = await publisherSelect.Locator("option").AllAsync();
        Assert.True(publisherOptions.Count > 1, "Publisher dropdown must have options beyond 'All Publishers'");

        // Select the first real publisher
        var firstPublisherText = (await publisherOptions[1].InnerTextAsync()).Trim();
        var firstPublisherValue = await publisherOptions[1].GetAttributeAsync("value");
        await publisherSelect.SelectOptionAsync(new SelectOptionValue { Value = firstPublisherValue });

        // Wait for the grid to refresh
        await Page.WaitForSelectorAsync("[data-testid='games-grid']", new() { Timeout = 15000 });

        // Every visible publisher badge must match the selected publisher
        var publisherBadges = Page.GetByTestId("game-publisher");
        var badgeCount = await publisherBadges.CountAsync();
        Assert.True(badgeCount > 0, "Filtered results must include at least one game with a publisher badge");

        for (var i = 0; i < badgeCount; i++)
        {
            await Expect(publisherBadges.Nth(i)).ToContainTextAsync(firstPublisherText);
        }
    }

    [Fact]
    public async Task ShouldFilterGamesByCategoryAndPublisherCombined()
    {
        // Navigate to homepage and wait for games and filter options to load
        await Page.GotoAsync("/");
        await Page.WaitForSelectorAsync("[data-testid='games-grid']", new() { Timeout = 15000 });
        await WaitForFilterOptionsAsync();

        var categorySelect = Page.GetByTestId("filter-category");
        var publisherSelect = Page.GetByTestId("filter-publisher");

        var categoryOptions = await categorySelect.Locator("option").AllAsync();
        var publisherOptions = await publisherSelect.Locator("option").AllAsync();

        Assert.True(categoryOptions.Count > 1, "Category dropdown must have options beyond 'All Categories'");
        Assert.True(publisherOptions.Count > 1, "Publisher dropdown must have options beyond 'All Publishers'");

        // Apply both filters
        var selectedCategoryText = (await categoryOptions[1].InnerTextAsync()).Trim();
        var selectedPublisherText = (await publisherOptions[1].InnerTextAsync()).Trim();

        await categorySelect.SelectOptionAsync(new SelectOptionValue { Value = await categoryOptions[1].GetAttributeAsync("value") });
        await Page.WaitForSelectorAsync("[data-testid='games-grid']", new() { Timeout = 15000 });

        await publisherSelect.SelectOptionAsync(new SelectOptionValue { Value = await publisherOptions[1].GetAttributeAsync("value") });
        await Page.WaitForSelectorAsync("[data-testid='games-grid']", new() { Timeout = 15000 });

        // Results should either be an empty state or cards that match both filters
        var gameCards = Page.GetByTestId("game-card");
        var cardCount = await gameCards.CountAsync();

        for (var i = 0; i < cardCount; i++)
        {
            var card = gameCards.Nth(i);
            var categoryBadge = card.GetByTestId("game-category");
            var publisherBadge = card.GetByTestId("game-publisher");

            if (await categoryBadge.IsVisibleAsync())
                await Expect(categoryBadge).ToContainTextAsync(selectedCategoryText);

            if (await publisherBadge.IsVisibleAsync())
                await Expect(publisherBadge).ToContainTextAsync(selectedPublisherText);
        }
    }

    [Fact]
    public async Task ShouldShowClearButtonOnlyWhenFilterIsActive()
    {
        // Navigate to homepage and wait for games and filter options to load
        await Page.GotoAsync("/");
        await Page.WaitForSelectorAsync("[data-testid='games-grid']", new() { Timeout = 15000 });
        await WaitForFilterOptionsAsync();

        // Clear button should not be visible with no filters
        await Expect(Page.GetByTestId("filter-clear-button")).Not.ToBeVisibleAsync();

        // Select a category — clear button must appear
        var categorySelect = Page.GetByTestId("filter-category");
        var categoryOptions = await categorySelect.Locator("option").AllAsync();
        Assert.True(categoryOptions.Count > 1, "Category dropdown must have options beyond 'All Categories'");

        await categorySelect.SelectOptionAsync(new SelectOptionValue { Value = await categoryOptions[1].GetAttributeAsync("value") });
        await Expect(Page.GetByTestId("filter-clear-button")).ToBeVisibleAsync();
    }

    [Fact]
    public async Task ShouldClearFiltersAndRestoreAllGames()
    {
        // Navigate to homepage and wait for games and filter options to load
        await Page.GotoAsync("/");
        await Page.WaitForSelectorAsync("[data-testid='games-grid']", new() { Timeout = 15000 });
        await WaitForFilterOptionsAsync();

        // Record initial full count
        var initialCount = await Page.GetByTestId("game-card").CountAsync();
        Assert.True(initialCount > 0, "There must be at least one game to test filtering");

        // Apply a category filter
        var categorySelect = Page.GetByTestId("filter-category");
        var categoryOptions = await categorySelect.Locator("option").AllAsync();
        Assert.True(categoryOptions.Count > 1, "Category dropdown must have options beyond 'All Categories'");

        await categorySelect.SelectOptionAsync(new SelectOptionValue { Value = await categoryOptions[1].GetAttributeAsync("value") });
        await Page.WaitForSelectorAsync("[data-testid='games-grid']", new() { Timeout = 15000 });

        // Click the clear button (single click — triggers single load)
        await Page.GetByTestId("filter-clear-button").ClickAsync();

        // Wait for the full list to be restored
        await Page.WaitForSelectorAsync("[data-testid='games-grid']", new() { Timeout = 15000 });

        // Full count should be restored and clear button hidden
        var restoredCount = await Page.GetByTestId("game-card").CountAsync();
        Assert.Equal(initialCount, restoredCount);
        await Expect(Page.GetByTestId("filter-clear-button")).Not.ToBeVisibleAsync();
    }

    [Fact]
    public async Task FilterDropdownsShouldBeKeyboardAccessible()
    {
        // Navigate to homepage
        await Page.GotoAsync("/");
        await Page.WaitForSelectorAsync("[data-testid='games-grid']", new() { Timeout = 15000 });

        // Both dropdowns should have accessible labels
        var categorySelect = Page.GetByLabel("Filter games by category");
        var publisherSelect = Page.GetByLabel("Filter games by publisher");

        await Expect(categorySelect).ToBeVisibleAsync();
        await Expect(publisherSelect).ToBeVisibleAsync();

        // Both should be focusable and interactive via keyboard
        await categorySelect.FocusAsync();
        await Expect(categorySelect).ToBeFocusedAsync();

        await publisherSelect.FocusAsync();
        await Expect(publisherSelect).ToBeFocusedAsync();
    }

    /// <summary>
    /// Waits for the Blazor interactive circuit to connect and populate filter dropdown options.
    /// The filter panel uses @rendermode InteractiveServer, so options are loaded asynchronously
    /// after the SignalR circuit connects — waiting for games-grid alone is not sufficient.
    /// </summary>
    private async Task WaitForFilterOptionsAsync()
    {
        await Expect(Page.GetByTestId("filter-category").Locator("option").Nth(1))
            .ToBeAttachedAsync(new() { Timeout = 15000 });
    }

    private static ILocatorAssertions Expect(ILocator locator) => Assertions.Expect(locator);
    private static IPageAssertions Expect(IPage page) => Assertions.Expect(page);
}
