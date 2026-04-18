# Exercise 1 - Installing GitHub Copilot CLI

[GitHub Copilot CLI][about-copilot-cli] is a powerful agentic coding assistant that runs in your terminal, enabling you to explore codebases, generate code, run commands, and interact with external tools - all from the command line.

## Scenario

Tailspin Toys is a nascent organization with a website that's lacking in many features. Their backlog is continuing to grow, and there's a strong demand to grow. To aid the developers, they want to begin utilizing AI agents through Copilot CLI. This will allow developers to be more productive, as they can focus on the bigger picture while moving faster. The first step to doing this is, of course, to install Copilot CLI!

In this exercise, you will learn how to:

- ensure your codespace is ready for the workshop.
- install GitHub Copilot CLI using one of the supported installation methods.
- authenticate with your GitHub account.
- verify the installation.

## Ensure your codespace is ready

In a [prior exercise][prereqs-lesson] you launched the codespace you'll use for the remainder of the coding exercises in this lab. Let's put the final touches on it before you begin.

1. Return to the tab where you started your codespace. If you closed the tab, return to your repository, select **Code** > **Codespaces** and then the name of the codespace.
2. Select **Extensions** on the workbench on the left side of your codespace.

    ![Screenshot of the extensions window with multiple extensions showing either Update or Reload Window buttons][img-extensions-updates]

3. Select **Update** on any extensions with an **Update** button. Repeat as necessary.
4. Select **Reload Window** on any extensions with a **Reload Window** button to reload the codespace.
5. When prompted by a dialog, select **Reload** to reload the window. This will ensure the latest version is being used.

## Open a terminal

Before installing Copilot CLI, you need to open a terminal window.

1. Return to your codespace or local clone.
2. Open a terminal window. In a codespace, press <kbd>Ctrl</kbd>+<kbd>\`</kbd>. Locally, use your preferred terminal application.
3. You should see a terminal prompt ready for commands.

## Install Copilot CLI

GitHub Copilot CLI can be installed in several ways. If you are in Codespaces, npm is convenient because Node.js is typically already installed. If you are working locally and prefer not to install Node.js just for Copilot CLI, use Homebrew on macOS/Linux or WinGet on Windows instead.

> [!NOTE]
> In this workshop, **Node.js is used for Copilot tooling**, not for the Tailspin Toys application itself. We use npm here because it works consistently in Codespaces, and Exercise 3 later uses the Playwright MCP server via `npx @playwright/mcp@latest`. The web app and the repo's Playwright E2E tests remain **C# / xUnit** based.

1. Choose the install method that matches your environment:

   **macOS / Linux with Homebrew**

   ```bash
   brew install copilot-cli
   ```

   **Windows with WinGet**

   ```powershell
   winget install GitHub.Copilot
   ```

   **Codespaces or any environment with Node.js / npm**

   ```bash
   npm install -g @github/copilot
   ```

2. If you are installing Copilot CLI via npm, verify Node.js is installed and meets the version requirement:

   ```bash
   node --version
   ```

   You should see version 22 or higher (e.g., `v22.x.x`).

3. Verify the installation by checking the version:

   ```bash
   copilot --version
   ```

   You should see the version number displayed (e.g., `v0.0.393`).

> [!TIP]
> If you prefer, you can also use the official Copilot CLI install script. If you encounter permission errors with npm, you may need to use `sudo npm install -g @github/copilot` on some systems. However, this shouldn't be necessary in GitHub Codespaces.

## Authenticate with GitHub

On first launch, Copilot CLI will prompt you to authenticate with your GitHub account.

1. Start Copilot CLI:

   ```bash
   copilot
   ```

2. If you're not currently logged in, you'll see a prompt to authenticate. Copilot CLI will display a device code and ask you to visit a URL.
3. Follow the on-screen instructions:
   - Open the provided URL in your browser
   - Enter the device code when prompted
   - Authorize Copilot CLI to access your GitHub account
4. Once authenticated, you'll see the Copilot CLI prompt, ready to accept your questions and commands.

> [!NOTE]
> In a codespace, you may already be authenticated through your GitHub session. If Copilot CLI starts without prompting for authentication, you're good to go!

## Trust the directory

When you first use Copilot CLI in a directory, it will ask you to confirm that you trust the files in that folder. This is a security feature to prevent Copilot from accidentally working with untrusted code.

1. When prompted, you'll see three options:
   - **Yes, proceed**: Trust for this session only
   - **Yes, and remember this folder for future sessions**: Trust permanently
   - **No, exit (Esc)**: Don't allow file access
2. For this workshop, select **Yes, and remember this folder for future sessions** since you'll be working in this repository throughout.

## Verify everything is working

Let's make sure Copilot CLI is properly installed and connected.

1. If you exited Copilot CLI, start it again:

   ```bash
   copilot
   ```

2. Ask Copilot a simple question to verify it's working:

   ```
   What files are in this project?
   ```

3. Copilot should explore the repository and provide a summary of the project structure.
4. Try the `/help` command to see available slash commands:

   ```
   /help
   ```

## Summary and next steps

Congratulations! You've successfully installed and authenticated GitHub Copilot CLI. You learned how to:

- install Copilot CLI using one of the supported installation methods.
- authenticate with your GitHub account.
- trust a directory for Copilot CLI to work with.
- verify the installation is working correctly.

## Resources

- [Installing GitHub Copilot CLI][install-copilot-cli]
- [About Copilot CLI][about-copilot-cli]
- [Using Copilot CLI][using-copilot-cli]

---

[install-copilot-cli]: https://docs.github.com/copilot/how-tos/set-up/install-copilot-cli
[about-copilot-cli]: https://docs.github.com/copilot/concepts/agents/about-copilot-cli
[using-copilot-cli]: https://docs.github.com/copilot/how-tos/use-copilot-agents/use-copilot-cli
[prereqs-lesson]: ./00-prereqs.md
[img-extensions-updates]: ./images/1-extensions-updates.png
