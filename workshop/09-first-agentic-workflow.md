# Exercise 9 - Create Your First Agentic Workflow

In the previous exercise, you prepared your environment and repository for Agentic Workflows. Now you're ready to initialise the repo, generate your first workflow, open a pull request, merge it, and run it.

**Estimated time:** 15 minutes

## Objectives

- initialise agentic workflows in your repository
- inspect the installed workflow agent
- create a daily digest workflow with Copilot
- open a PR, merge it, and trigger the workflow manually

## Initialise Agentic Workflows in your repository

From the root of your Tailspin Toys repository, run:

```bash
gh aw init
```

This sets up your repository for agentic workflows. It creates several files, including:

- `.gitattributes` — marks compiled lock files as generated
- `.github/agents/agentic-workflows.agent.md` — an AI assistant for creating and editing workflows
- `.vscode/settings.json` and `.vscode/mcp.json` — editor configuration

After `gh aw init`, you can open the installed agent file to see the metadata that powers the workflow authoring experience:

```bash
cat .github/agents/agentic-workflows.agent.md
```

It includes frontmatter like:

```yaml
---
description: GitHub Agentic Workflows (gh-aw) - Create, debug, and upgrade AI-powered workflows with intelligent prompt routing
disable-model-invocation: true
---
```

## Create a daily digest workflow

Now create your first workflow — a daily digest of all open issues and pull requests in the Tailspin Toys repository.

1. Start Copilot CLI:

   ```bash
   copilot
   ```

2. Type `/agent` and press Enter.

3. Select the `agentic-workflows` agent.

4. Paste in this prompt:

   ```
   Every weekday, create a GitHub issue that summarises all open issues
   and pull requests in this repository. Group them by label. Include the
   total count, the title, the author, and how long each item has been
   open. Title the issue "Daily Digest – <date>".

   Name the workflow file daily-digest.
   Allow for manual runs.
   ```

5. The agent will ask clarifying questions and then generate the workflow files for you.

6. If Copilot asks you to create a `digest` label for the generated issue, do this:

   1. Open your repository on GitHub and go to **Issues** > **Labels**.
   2. Click **New label**.
   3. Enter `digest`, choose any colour you like, create the label, and then return to Copilot.

### What gets created

After the agent finishes, you will have:

- `.github/workflows/daily-digest.md` — the human-readable workflow
- `.github/workflows/daily-digest.lock.yml` — the compiled file for GitHub Actions

Open the markdown file to see what the agent wrote:

```bash
cat .github/workflows/daily-digest.md
```

The frontmatter will look similar to:

```yaml
---
on:
  schedule:
    - cron: "0 8 * * 1-5"
  workflow_dispatch:
permissions:
  issues: read
  pull-requests: read
safe-outputs:
  mentions: false
  allowed-github-references: []
  create-issue:
    title-prefix: "Daily Digest –"
    labels: [digest]
    close-older-issues: true
    expires: 7
---
```

And the body is a plain-English description of what the agent should do.

> [!NOTE]
> Agentic workflow files are regular markdown — commit them to version control just like any other code. The `.lock.yml` is auto-generated and should not be edited by hand.

## Open a PR and run the workflow

Stay in the `agentic-workflows` agent and use it to package the generated workflow into a pull request.

1. Make sure you are still in the `agentic-workflows` agent. If not, type `/agent` and select it again.
2. Enter this prompt:

   ```
   Can you please create a pull request for me!
   ```

This reuses the same PR skill from earlier in the workshop to commit, push, and open a PR for your workflow files.

3. Open the PR on GitHub and merge it.

4. Return to your terminal and switch back to `main`:

```bash
git checkout main
git pull
```

Once the workflow is on `main`, trigger a manual run using either of these options:

### Option 1: Run it from the CLI

```bash
gh aw run daily-digest
```

### Option 2: Run it from the GitHub UI

1. Open your repository on GitHub and go to the **Actions** tab.
2. Select the `daily-digest` workflow from the list on the left.
3. Click **Run workflow**.
4. Choose the `main` branch and confirm the run.

After the run completes, open GitHub and check the **Issues** tab. You should see a new issue titled **Daily Digest – <today's date>** summarising your open issues and PRs.

> [!NOTE]
> If the repository has few open issues or PRs, the digest will reflect that. The workflow is still working correctly.

## Summary and next steps

In this exercise you:

- initialised agentic workflows in your repository.
- created your first workflow using the `agentic-workflows` agent.
- opened and merged a PR for the generated files.
- triggered the workflow manually and verified the output.

In the next exercise, you'll build a workflow that reaches out to an external API.
