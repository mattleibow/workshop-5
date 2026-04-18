using Microsoft.Playwright;

namespace TailspinToys.E2E;

public class GamesTests : PlaywrightTestBase
{
    [Fact]
    public async Task ShouldDisplayGamesWithTitlesOnIndexPage()
    {
        // Navigate to homepage
        await Page.GotoAsync("/");

        // Wait for games to load (games-grid only renders when data is available)
        await Page.WaitForSelectorAsync("[data-testid='games-grid']", new() { Timeout = 15000 });

        // Verify game cards are displayed
        var gameCards = Page.GetByTestId("game-card");
        await Expect(gameCards.First).ToBeVisibleAsync();
        Assert.True(await gameCards.CountAsync() > 0);

        // Verify game cards have titles with content
        await Expect(gameCards.First.GetByTestId("game-title")).ToBeVisibleAsync();
        await Expect(gameCards.First.GetByTestId("game-title")).Not.ToBeEmptyAsync();
    }

    [Fact]
    public async Task ShouldNavigateToCorrectGameDetailsPage()
    {
        // Navigate to homepage and wait for games to load
        await Page.GotoAsync("/");
        await Page.WaitForSelectorAsync("[data-testid='games-grid']", new() { Timeout = 15000 });

        // Get first game information and click it
        var firstGameCard = Page.GetByTestId("game-card").First;
        var gameId = await firstGameCard.GetAttributeAsync("data-game-id");
        var gameTitle = await firstGameCard.GetAttributeAsync("data-game-title");
        await firstGameCard.ClickAsync();

        // Verify navigation to game details page
        await Expect(Page).ToHaveURLAsync($"/game/{gameId}");
        await Expect(Page.GetByTestId("game-details")).ToBeVisibleAsync();

        // Verify game title matches clicked game
        if (gameTitle is not null)
        {
            await Expect(Page.GetByTestId("game-details-title")).ToHaveTextAsync(gameTitle);
        }
    }

    [Fact]
    public async Task ShouldDisplayGameDetailsWithAllRequiredInformation()
    {
        // Navigate to specific game details page
        await Page.GotoAsync("/game/1");
        await Expect(Page.GetByTestId("game-details")).ToBeVisibleAsync();

        // Verify game title is displayed
        var gameTitle = Page.GetByTestId("game-details-title");
        await Expect(gameTitle).ToBeVisibleAsync();
        await Expect(gameTitle).Not.ToBeEmptyAsync();

        // Verify game description is displayed
        var gameDescription = Page.GetByTestId("game-details-description");
        await Expect(gameDescription).ToBeVisibleAsync();
        await Expect(gameDescription).Not.ToBeEmptyAsync();

        // Verify publisher or category information is present
        var publisherExists = await Page.GetByTestId("game-details-publisher").IsVisibleAsync();
        var categoryExists = await Page.GetByTestId("game-details-category").IsVisibleAsync();
        Assert.True(publisherExists || categoryExists);

        if (publisherExists)
        {
            await Expect(Page.GetByTestId("game-details-publisher")).Not.ToBeEmptyAsync();
        }

        if (categoryExists)
        {
            await Expect(Page.GetByTestId("game-details-category")).Not.ToBeEmptyAsync();
        }
    }

    [Fact]
    public async Task ShouldDisplayAButtonToBackTheGame()
    {
        // Navigate to game details page
        await Page.GotoAsync("/game/1");
        await Expect(Page.GetByTestId("game-details")).ToBeVisibleAsync();

        // Verify back game button is visible and enabled
        var backButton = Page.GetByTestId("back-game-button");
        await Expect(backButton).ToBeVisibleAsync();
        await Expect(backButton).ToContainTextAsync("Support This Game");
        await Expect(backButton).ToBeEnabledAsync();
    }

    [Fact]
    public async Task ShouldBeAbleToNavigateBackToHomeFromGameDetails()
    {
        // Navigate to game details page
        await Page.GotoAsync("/game/1");
        await Expect(Page.GetByTestId("game-details")).ToBeVisibleAsync();

        // Click back to all games link
        var backLink = Page.GetByRole(AriaRole.Link, new() { NameRegex = new System.Text.RegularExpressions.Regex("back to all games", System.Text.RegularExpressions.RegexOptions.IgnoreCase) });
        await Expect(backLink).ToBeVisibleAsync();
        await backLink.ClickAsync();

        // Verify navigation back to homepage
        await Expect(Page).ToHaveURLAsync("/");
        await Expect(Page.GetByTestId("games-grid")).ToBeVisibleAsync();
    }

    [Fact]
    public async Task ShouldHandleNavigationToNonExistentGameGracefully()
    {
        // Navigate to non-existent game
        var response = await Page.GotoAsync("/game/99999");

        // Verify page handles error gracefully
        Assert.NotNull(response);
        Assert.True(response.Status < 500);
        await Expect(Page).ToHaveTitleAsync(new System.Text.RegularExpressions.Regex("Game Details - Tailspin Toys"));
    }

    private static ILocatorAssertions Expect(ILocator locator) => Assertions.Expect(locator);
    private static IPageAssertions Expect(IPage page) => Assertions.Expect(page);
}
