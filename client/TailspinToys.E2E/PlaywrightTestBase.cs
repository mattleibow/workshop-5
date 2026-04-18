using Microsoft.Playwright;

namespace TailspinToys.E2E;

/// <summary>
/// Base class for Playwright E2E tests that handles browser lifecycle.
/// </summary>
public class PlaywrightTestBase : IAsyncLifetime
{
    protected IPlaywright PlaywrightInstance { get; private set; } = null!;
    protected IBrowser Browser { get; private set; } = null!;
    protected IPage Page { get; private set; } = null!;

    private const string BaseUrl = "http://localhost:4321";

    public async Task InitializeAsync()
    {
        PlaywrightInstance = await Playwright.CreateAsync();
        Browser = await PlaywrightInstance.Chromium.LaunchAsync(new BrowserTypeLaunchOptions
        {
            Headless = true
        });
        var context = await Browser.NewContextAsync(new BrowserNewContextOptions
        {
            BaseURL = BaseUrl
        });
        Page = await context.NewPageAsync();
        Page.SetDefaultTimeout(15000);
        Page.SetDefaultNavigationTimeout(30000);
        Assertions.SetDefaultExpectTimeout(15000);
    }

    public async Task DisposeAsync()
    {
        await Page.CloseAsync();
        await Browser.CloseAsync();
        PlaywrightInstance.Dispose();
    }
}
