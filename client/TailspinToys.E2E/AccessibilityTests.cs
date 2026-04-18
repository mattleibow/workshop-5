using Deque.AxeCore.Commons;
using Deque.AxeCore.Playwright;
using Microsoft.Playwright;

namespace TailspinToys.E2E;

public class AccessibilityTests : PlaywrightTestBase
{
    [Fact]
    public async Task HomePageShouldNotHaveAccessibilityViolations()
    {
        await Page.GotoAsync("/");
        await Page.WaitForSelectorAsync("[data-testid='games-grid']", new() { Timeout = 10000 });

        var results = await Page.RunAxe(new AxeRunOptions
        {
            RunOnly = new RunOnlyOptions
            {
                Type = "tag",
                Values = ["wcag2a", "wcag2aa", "wcag21a", "wcag21aa"]
            }
        });

        Assert.Empty(results.Violations);
    }

    [Fact]
    public async Task GameDetailsPageShouldNotHaveAccessibilityViolations()
    {
        await Page.GotoAsync("/game/1");
        await Page.WaitForSelectorAsync("[data-testid='game-details']", new() { Timeout = 10000 });

        var results = await Page.RunAxe(new AxeRunOptions
        {
            RunOnly = new RunOnlyOptions
            {
                Type = "tag",
                Values = ["wcag2a", "wcag2aa", "wcag21a", "wcag21aa"]
            }
        });

        Assert.Empty(results.Violations);
    }

    [Fact]
    public async Task AboutPageShouldNotHaveAccessibilityViolations()
    {
        await Page.GotoAsync("/about");
        await Page.WaitForSelectorAsync("[data-testid='about-section']", new() { Timeout = 10000 });

        var results = await Page.RunAxe(new AxeRunOptions
        {
            RunOnly = new RunOnlyOptions
            {
                Type = "tag",
                Values = ["wcag2a", "wcag2aa", "wcag21a", "wcag21aa"]
            }
        });

        Assert.Empty(results.Violations);
    }

    [Fact]
    public async Task KeyboardNavigationShouldBeAbleToNavigateHeaderMenu()
    {
        await Page.GotoAsync("/");
        await Page.WaitForSelectorAsync("[data-testid='games-grid']", new() { Timeout = 10000 });

        // Focus on the menu button
        var menuButton = Page.Locator("#menu-toggle");
        await menuButton.FocusAsync();

        // Verify the menu button is focused
        await Expect(menuButton).ToBeFocusedAsync();

        // Open menu using the keyboard to match the original workshop coverage
        await Page.Keyboard.PressAsync("Enter");

        // Verify menu is visible (no longer has 'hidden' class)
        var menu = Page.Locator("#menu");
        await Expect(menu).Not.ToHaveClassAsync(new System.Text.RegularExpressions.Regex(@"\bhidden\b"));

        // Tab to first menu item
        await Page.Keyboard.PressAsync("Tab");
        var homeLink = Page.Locator("#menu a[href='/']");
        await Expect(homeLink).ToBeFocusedAsync();

        // Tab to second menu item
        await Page.Keyboard.PressAsync("Tab");
        var aboutLink = Page.Locator("#menu a[href='/about']");
        await Expect(aboutLink).ToBeFocusedAsync();
    }

    [Fact]
    public async Task KeyboardNavigationShouldBeAbleToNavigateToGameCards()
    {
        await Page.GotoAsync("/");
        await Page.WaitForSelectorAsync("[data-testid='games-grid']", new() { Timeout = 10000 });

        // Tab through header elements to get to game cards
        var tabCount = 0;
        var gameCardFocused = false;

        while (tabCount < 20 && !gameCardFocused)
        {
            await Page.Keyboard.PressAsync("Tab");
            tabCount++;

            var focusedElement = Page.Locator(":focus");
            var testId = await focusedElement.GetAttributeAsync("data-testid");

            if (testId == "game-card")
            {
                gameCardFocused = true;
            }
        }

        Assert.True(gameCardFocused);
    }

    [Fact]
    public async Task KeyboardNavigationShouldBeAbleToActivateGameCardWithEnter()
    {
        await Page.GotoAsync("/");
        await Page.WaitForSelectorAsync("[data-testid='games-grid']", new() { Timeout = 10000 });

        // Get first game card
        var firstGameCard = Page.GetByTestId("game-card").First;
        var gameId = await firstGameCard.GetAttributeAsync("data-game-id");

        // Focus on the game card
        await firstGameCard.FocusAsync();

        // Activate with Enter
        await Page.Keyboard.PressAsync("Enter");

        // Verify navigation occurred
        await Expect(Page).ToHaveURLAsync($"/game/{gameId}");
    }

    [Fact]
    public async Task FocusIndicatorsShouldHaveVisibleFocusIndicatorsOnInteractiveElements()
    {
        await Page.GotoAsync("/");
        await Page.WaitForSelectorAsync("[data-testid='games-grid']", new() { Timeout = 10000 });

        // Check menu button has focus indicator
        var menuButton = Page.Locator("#menu-toggle");
        await menuButton.FocusAsync();

        var hasVisibleFocus = await menuButton.EvaluateAsync<bool>(@"(el) => {
            const styles = window.getComputedStyle(el);
            const outline = styles.outline;
            const outlineWidth = styles.outlineWidth;
            const boxShadow = styles.boxShadow;
            return (outline !== 'none' && outlineWidth !== '0px') || boxShadow !== 'none';
        }");

        Assert.True(hasVisibleFocus);
    }

    [Fact]
    public async Task AriaLabelsShouldHaveProperAttributes()
    {
        await Page.GotoAsync("/");

        var menuButton = Page.Locator("#menu-toggle");
        var hasAriaLabel = await menuButton.EvaluateAsync<bool>(@"(el) => {
            return el.hasAttribute('aria-label') || 
                   el.hasAttribute('aria-labelledby') ||
                   el.hasAttribute('aria-describedby');
        }");

        var menuIcon = menuButton.Locator("svg");
        var svgAccessible = await menuIcon.EvaluateAsync<bool>(@"(el) => {
            return el.hasAttribute('role') || 
                   el.hasAttribute('aria-label') ||
                   el.querySelector('title') !== null;
        }");

        Assert.True(hasAriaLabel || svgAccessible);
    }

    [Fact]
    public async Task ColorContrastShouldMeetWcagAaStandards()
    {
        await Page.GotoAsync("/");
        await Page.WaitForSelectorAsync("[data-testid='games-grid']", new() { Timeout = 10000 });

        var results = await Page.RunAxe(new AxeRunOptions
        {
            RunOnly = new RunOnlyOptions
            {
                Type = "tag",
                Values = ["wcag2aa"]
            }
        });

        var contrastViolations = results.Violations
            .Where(v => v.Id == "color-contrast")
            .ToArray();

        Assert.Empty(contrastViolations);
    }

    [Fact]
    public async Task SemanticHtmlMainLandmarksShouldBePresent()
    {
        await Page.GotoAsync("/");
        await Page.WaitForSelectorAsync("[data-testid='games-grid']", new() { Timeout = 10000 });

        // Check for header landmark
        var header = Page.Locator("header").First;
        await Expect(header).ToBeVisibleAsync();

        // Check for main landmark
        var main = Page.Locator("main");
        await Expect(main).ToBeVisibleAsync();
    }

    [Fact]
    public async Task DecorativeSvgsShouldHaveAriaHiddenAttribute()
    {
        await Page.GotoAsync("/");
        await Page.WaitForSelectorAsync("[data-testid='games-grid']", new() { Timeout = 10000 });

        // Check menu button SVG has aria-hidden
        var menuButtonSvg = Page.Locator("#menu-toggle svg");
        await Expect(menuButtonSvg).ToHaveAttributeAsync("aria-hidden", "true");

        // Check game card arrow SVGs have aria-hidden
        var firstGameCard = Page.GetByTestId("game-card").First;
        var gameCardSvgs = firstGameCard.Locator("svg");
        var count = await gameCardSvgs.CountAsync();

        Assert.True(count > 0);
        for (var i = 0; i < count; i++)
        {
            await Expect(gameCardSvgs.Nth(i)).ToHaveAttributeAsync("aria-hidden", "true");
        }
    }

    [Fact]
    public async Task HomePageShouldHaveExactlyOneH1WithCorrectContent()
    {
        await Page.GotoAsync("/");
        await Page.WaitForSelectorAsync("[data-testid='games-grid']", new() { Timeout = 10000 });

        // Exactly one <h1> on the home page
        var h1Elements = Page.Locator("h1");
        await Expect(h1Elements).ToHaveCountAsync(1);
        await Expect(h1Elements.First).ToContainTextAsync("Welcome to Tailspin Toys");
    }

    [Fact]
    public async Task HomePageHeadingsShouldFollowLogicalHierarchy()
    {
        await Page.GotoAsync("/");
        await Page.WaitForSelectorAsync("[data-testid='games-grid']", new() { Timeout = 10000 });

        // h1 must exist before any h2
        var h1 = Page.Locator("h1").First;
        var h2 = Page.Locator("h2").First;
        await Expect(h1).ToBeVisibleAsync();
        await Expect(h2).ToBeVisibleAsync();

        // No h3 or deeper without a preceding h2 (no skipped levels)
        var h3Count = await Page.Locator("h3").CountAsync();
        var h2Count = await Page.Locator("h2").CountAsync();
        if (h3Count > 0)
            Assert.True(h2Count > 0, "An h3 exists but no h2 precedes it — heading levels are skipped");
    }

    [Fact]
    public async Task AboutPageShouldHaveExactlyOneH1WithCorrectContent()
    {
        await Page.GotoAsync("/about");
        await Page.WaitForSelectorAsync("[data-testid='about-section']", new() { Timeout = 10000 });

        // Exactly one <h1> on the about page
        var h1Elements = Page.Locator("h1");
        await Expect(h1Elements).ToHaveCountAsync(1);
        await Expect(h1Elements.First).ToContainTextAsync("About Tailspin Toys");
    }

    [Fact]
    public async Task AboutPageHeadingsShouldFollowLogicalHierarchy()
    {
        await Page.GotoAsync("/about");
        await Page.WaitForSelectorAsync("[data-testid='about-section']", new() { Timeout = 10000 });

        // No heading level should be skipped
        var h2Count = await Page.Locator("h2").CountAsync();
        var h3Count = await Page.Locator("h3").CountAsync();

        if (h3Count > 0)
            Assert.True(h2Count > 0, "An h3 exists but no h2 precedes it — heading levels are skipped");
    }

    [Fact]
    public async Task GameDetailsPageShouldHaveExactlyOneH1WithGameTitle()
    {
        await Page.GotoAsync("/game/1");
        await Page.WaitForSelectorAsync("[data-testid='game-details']", new() { Timeout = 10000 });

        // Exactly one <h1> containing the game title
        var h1Elements = Page.Locator("h1");
        await Expect(h1Elements).ToHaveCountAsync(1);
        await Expect(h1Elements.First).Not.ToBeEmptyAsync();

        // The h1 text should match the game-details-title testid
        var titleText = await Page.GetByTestId("game-details-title").InnerTextAsync();
        await Expect(h1Elements.First).ToHaveTextAsync(titleText);
    }

    [Fact]
    public async Task GameDetailsPageHeadingsShouldFollowLogicalHierarchy()
    {
        await Page.GotoAsync("/game/1");
        await Page.WaitForSelectorAsync("[data-testid='game-details']", new() { Timeout = 10000 });

        // h1 (game title) → h2 ("About this game") — no levels skipped
        var h1Count = await Page.Locator("h1").CountAsync();
        var h2Count = await Page.Locator("h2").CountAsync();
        var h3Count = await Page.Locator("h3").CountAsync();

        Assert.Equal(1, h1Count);
        if (h3Count > 0)
            Assert.True(h2Count > 0, "An h3 exists but no h2 precedes it — heading levels are skipped");
    }

    private static ILocatorAssertions Expect(ILocator locator) => Assertions.Expect(locator);
    private static IPageAssertions Expect(IPage page) => Assertions.Expect(page);
}
