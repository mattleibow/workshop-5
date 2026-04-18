# Tailspin Toys

This repository contains the project for a guided workshop to explore GitHub Copilot CLI and Agentic Workflows. The project is a website for a fictional game crowd-funding company, with an [ASP.NET Core](https://learn.microsoft.com/aspnet/core/) backend using [Entity Framework Core](https://learn.microsoft.com/ef/core/) and [Blazor](https://learn.microsoft.com/aspnet/core/blazor/) frontend with [Tailwind CSS](https://tailwindcss.com/) for styling.

The workshop is split into two parts:

- **Part 1: Building with Copilot CLI** — Learn to use custom instructions, MCP servers, code generation, agent skills, and custom agents to build features for the Tailspin Toys app.
- **Part 2: Automating with Agentic Workflows** — Create scheduled workflows and ChatOps slash commands using `gh aw` to automate your development lifecycle.

## Repository setup

Create your own repository from this template first, then either:

- create a Codespace from your repository, or
- clone your repository locally.

Start with [workshop/00-prereqs.md](./workshop/00-prereqs.md) for the full setup flow.

## Start the workshop

**To begin the workshop, start at [workshop/README.md](./workshop/README.md)**

Or, if just want to run the app...

## Launch the site

A script file has been created to launch the site. You can run it by:

```bash
# Linux/macOS/Codespaces
./scripts/start-app.sh

# Windows (PowerShell)
.\scripts\start-app.ps1
```

Then navigate to the [website](http://localhost:4321) to see the site!

## License 

This project is licensed under the terms of the MIT open source license. Please refer to the [LICENSE](./LICENSE) for the full terms.

## Support

This project is provided as-is, and may be updated over time. If you have questions, please open an issue.
