# Exercise 0: Prerequisites

Before you get started on the lab, there's a few tasks you need to complete to get everything ready. You should first create your own copy of the repository, then choose whether to work in a [codespace][codespaces] or by cloning your own repository locally.

## Setting up the Lab Repository

To create a copy of the repository for the code you'll create an instance from the [template][template-repository]. The new instance will contain all of the necessary files for the lab, and you'll use it as you work through the exercises. 

1. In a new browser window, navigate to the GitHub repository for this lab: `https://github.com/mattleibow/tailspin-toys-workshop-dotnet`.
2. Create your own copy of the repository by selecting the **Use this template** button on the lab repository page. Then select **Create a new repository**.

    ![Use this template button](./images/0-use-template.png)

3. If you are completing the workshop as part of an event being led by GitHub or Microsoft, follow the instructions provided by the mentors. Otherwise, you can create the new repository in an organization where you have access to Copilot.

    ![Input the repository template settings](./images/0-repository-settings.png)

4. Make a note of the repository path you created (**organization-or-user-name/repository-name**), as you will be referring to this later in the lab.

## Choose Your Development Environment

Now that you've created your own repository, choose one of the following options:

### Option A: Create a Codespace for your Repository (recommended)

[GitHub Codespaces][codespaces] are a cloud-based development environment that allows you to write, run, and debug code directly in your browser. It provides a fully-featured IDE with support for multiple programming languages, extensions, and tools.

1. Navigate to your newly created repository.
2. Select the green **Code** button.

    ![Select the Code button](./images/0-code-button.png)

3. Select the **Codespaces** tab and select the **+** button to create a new Codespace.

    ![Create a new codespace](./images/0-create-codespace.png)

The creation of the codespace will take several minutes, although it's still far quicker than having to manually install all the services! That said, you can use this time to explore other features of GitHub Copilot, which we'll turn your attention to next!

### Option B: Clone your Repository locally

If you prefer to work locally rather than in a codespace, clone your own repository and open it in your preferred editor or terminal:

```bash
git clone https://github.com/<organization-or-user-name>/<repository-name>.git
cd <repository-name>
```

> [!TIP]
> You can open the cloned folder in any editor you prefer — VS Code, Vim, or any other editor. The workshop exercises use the terminal exclusively.

> [!IMPORTANT]
> If you chose Option A, you'll return to the codespace in a future exercise. For the time being, leave it open in a tab in your browser.

> [!NOTE]
> This workshop is built to run inside of a codespace or container. This ensures the environment you're working in has all of the necessary prerequisites installed and you'll have a smooth experience. If you wish to run the workshop locally on your system, you will need the .NET 10 SDK, Git, and a terminal. **Node.js 22 or higher is only needed for the workshop tooling** - specifically installing GitHub Copilot CLI with npm and running the Playwright MCP server in Exercise 3. The Tailspin Toys app itself, plus the Playwright E2E tests in this repo, are C# / xUnit based.

## Quick checklist for the rest of the workshop

Before moving on, make sure you have:

- your **own copy of the template repository** and write access to it
- a browser available for GitHub authentication, Codespaces, and reviewing issues / PRs
- internet access to GitHub and the workshop services used later, such as npm and Microsoft Learn
- if working locally: **.NET 10 SDK**, **Git**, **Node.js 22+**, and **GitHub CLI (`gh`)**

> [!TIP]
> **GitHub CLI** is used in Part 2 of the workshop for Agentic Workflows. If you're in a Codespace, it's already installed. If working locally, install it from [cli.github.com](https://cli.github.com/) and authenticate with `gh auth login`. You can also do this later when you reach Exercise 8.

> [!IMPORTANT]
> Later exercises create **GitHub issues**, **branches**, **pull requests**, and optionally a **gist**. These steps work best when you are in your own repository created from the template and your GitHub account or organization allows those operations.

## Summary

Congratulations, you have created a copy of the lab repository and selected your development environment.

## Resources

- [GitHub Codespaces overview][codespaces]
- [Creating a repository from a template][template-repository]
- [Getting started with Codespaces][codespaces-quickstart]

---

[codespaces]: https://github.com/features/codespaces
[template-repository]: https://docs.github.com/repositories/creating-and-managing-repositories/creating-a-repository-from-a-template
[codespaces-quickstart]: https://docs.github.com/codespaces/getting-started/quickstart
