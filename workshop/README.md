# GitHub Copilot CLI & Agentic Workflows Workshop

Welcome to this hands-on workshop on **GitHub Copilot CLI** and **Agentic Workflows**! In this session, you'll build real features for an application using AI, then automate your development lifecycle with agentic workflows — all from the command line.

## Workshop Overview

This workshop is split into two parts:

- **Part 1: Building with Copilot CLI** — Learn to use GitHub Copilot as a powerful agentic coding assistant directly from your terminal. You'll configure custom instructions, connect MCP servers, generate code, use agent skills, and work with custom agents.
- **Part 2: Automating with Agentic Workflows** — Take things further by creating agentic workflows that run on schedules and respond to events in your repository. You'll build automated digests and ChatOps slash commands.

### Learning Objectives

By the end of this workshop, you will be able to:

1. **Install and configure GitHub Copilot CLI** — Set up Copilot CLI in a codespace and authenticate with GitHub
2. **Understand custom instructions** — Provide context and guidelines to improve code generation quality
3. **Work with MCP servers** — Extend Copilot CLI's capabilities by connecting to external services
4. **Generate and modify code** — Leverage AI to create new features from the terminal
5. **Use agent skills** — Enhance Copilot's capabilities with specialized skills
6. **Create custom agents** — Build specialized agents for specific tasks
7. **Build agentic workflows** — Create natural-language workflows that run as GitHub Actions
8. **Use the ChatOps pattern** — Trigger agentic workflows from issue comments with slash commands

## Prerequisites

Before attending this workshop, please ensure you have:

- [ ] A GitHub account with an active **Copilot Pro, Pro+, Business, or Enterprise** subscription
- [ ] Permission to create and work in **your own copy of the template repository** in a user or organization where you can create issues, branches, commits, and pull requests
- [ ] A web browser and internet access to GitHub and the external services used in the workshop
- [ ] Basic familiarity with terminal/command line operations
- [ ] Either:
  - **GitHub Codespaces access** (recommended), or
  - a local environment with **.NET 10 SDK**, **Git**, **Node.js 22+**, and **GitHub CLI (`gh`)**

> [!NOTE]
> If you are using Copilot Business or Copilot Enterprise, ensure your admin has enabled Copilot CLI and Copilot Extensions (including Agentic Workflows) for use.

> [!TIP]
> Node.js is only needed for the **Copilot CLI / MCP tooling** flow in this workshop — specifically installing Copilot CLI with npm and running the Playwright MCP server. The Tailspin Toys app and the repository's Playwright E2E tests are still **.NET / C# / xUnit** based.

## The Scenario: Tailspin Toys

Throughout this workshop, you'll be working with **Tailspin Toys**, a crowdfunding platform for games with a developer theme. The application features:

- **Backend**: ASP.NET Core Minimal API with Entity Framework Core for database interactions
- **Frontend**: Blazor with Tailwind CSS for styling
- **Database**: SQLite for local development

This full-stack application provides an excellent playground for exploring Copilot CLI's capabilities across different file types, languages, and development tasks. In Part 2, you'll automate aspects of this project's development lifecycle with agentic workflows.

## Workshop Exercises

### Part 1: Building with Copilot CLI (~80 min)

| Exercise | Topic | Description |
|----------|-------|-------------|
| [0. Prerequisites][ex0] | Setup | Create your repository and codespace |
| [1. Installing Copilot CLI][ex1] | Installation | Install and authenticate Copilot CLI |
| [2. Custom Instructions][ex2] | Context | Learn how instruction files guide Copilot |
| [3. MCP Servers][ex3] | External Tools | Connect to GitHub and other services via MCP |
| [4. Generating Code][ex4] | Code Generation | Use plan mode and generate features |
| [5. Agent Skills][ex5] | Skills | Enhance Copilot with specialized skills |
| [6. Custom Agents][ex6] | Agents | Create and use custom agents |
| [7. Slash Commands][ex7] | CLI Features | Explore context, models, and sharing |

### Part 2: Automating with Agentic Workflows (~75 min)

| Exercise | Topic | Description |
|----------|-------|-------------|
| [8. Agentic Workflows Setup][ex8] | Setup | Authenticate GitHub, install gh aw, and configure repo secrets |
| [9. First Agentic Workflow][ex9] | Automation | Initialise gh aw, create a daily digest, open a PR, and run it |
| [10. ChatOps: Game Lookup][ex10] | ChatOps | Create a /game-lookup slash command |
| [11. Trending Games Digest][ex11] | External APIs | Fetch popular FreeToGame titles on a schedule |
| [12. Review & Next Steps][ex12] | Summary | Recap both parts and continue learning |

## Tips for Success

1. **Be specific** — The more context you provide, the better the results
2. **Start with exploration** — Ask Copilot to explain the codebase before making changes
3. **Iterate** — Refine your prompts based on initial results
4. **Trust but verify** — Always review generated code before committing
5. **Use instruction files** — Leverage `.github/copilot-instructions.md` for project-wide guidance
6. **Read the generated workflow** — Understanding what the agent writes helps you tune it

## Support

- **During the workshop**: Raise your hand or use the chat to ask questions
- **After the workshop**: Open an issue in this repository

---

*Happy coding and automating with GitHub Copilot! 🚀🤖*

[ex0]: ./00-prereqs.md
[ex1]: ./01-install-copilot-cli.md
[ex2]: ./02-custom-instructions.md
[ex3]: ./03-mcp.md
[ex4]: ./04-generating-code.md
[ex5]: ./05-agent-skills.md
[ex6]: ./06-custom-agents.md
[ex7]: ./07-slash-commands.md
[ex8]: ./08-agentic-workflows.md
[ex9]: ./09-first-agentic-workflow.md
[ex10]: ./10-chatops-game-lookup.md
[ex11]: ./11-trending-games-digest.md
[ex12]: ./12-review.md
