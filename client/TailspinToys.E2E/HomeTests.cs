using Microsoft.Playwright;

namespace TailspinToys.E2E;

public class HomeTests : PlaywrightTestBase
{
    [Fact]
    public async Task ShouldDisplayTheCorrectTitle()
    {
        await Page.GotoAsync("/");

        // Check that the page title is correct
        await Expect(Page).ToHaveTitleAsync("Tailspin Toys - Crowdfunding your new favorite game!");
    }

    [Fact]
    public async Task ShouldDisplayTheMainHeading()
    {
        await Page.GotoAsync("/");

        // Check that the main page heading is present
        await Expect(Page.GetByRole(AriaRole.Heading, new() { Name = "Welcome to Tailspin Toys", Exact = true })).ToBeVisibleAsync();
    }

    [Fact]
    public async Task ShouldDisplayTheSiteBrandingInHeader()
    {
        await Page.GotoAsync("/");

        // Check that the site branding is present in the header
        await Expect(Page.GetByText("Tailspin Toys").First).ToBeVisibleAsync();
    }

    [Fact]
    public async Task ShouldDisplayTheWelcomeMessage()
    {
        await Page.GotoAsync("/");

        // Check that the welcome message is present
        await Expect(Page.GetByText("Find your next game! And maybe even back one! Explore our collection!")).ToBeVisibleAsync();
    }

    private static ILocatorAssertions Expect(ILocator locator) => Assertions.Expect(locator);
    private static IPageAssertions Expect(IPage page) => Assertions.Expect(page);
}
