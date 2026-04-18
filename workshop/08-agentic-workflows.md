# Exercise 8 - Prepare Your Repo for Agentic Workflows

Over the previous exercises you've explored GitHub Copilot CLI from multiple angles — custom instructions, MCP servers, code generation, agent skills, custom agents, and slash commands. You've built real features for Tailspin Toys, created pull requests, and learned how to get the most out of AI-assisted development.

Now let's take things further. Instead of running Copilot from your terminal one prompt at a time, what if you could describe an automation in plain English and have it run on a schedule or in response to events — right inside GitHub Actions? That's exactly what **Agentic Workflows** enable.

In this exercise, you will:

- learn what agentic workflows are and why they complement the skills you've already built.
- prepare your local environment and GitHub repository for agentic workflows.
- install the `gh aw` CLI extension and verify that it is ready to use.
- configure the repository secret required for workflow runs.

## What are Agentic Workflows?

Agentic Workflows let you describe *what* you want done in plain English. Copilot figures out *how* to do it, runs the necessary tools, and posts the results back to GitHub. Each workflow is a **markdown file** with a small YAML frontmatter block at the top. The frontmatter declares things like the trigger, required tools, and permissions. The markdown body is a plain-English prompt.

Before a workflow can run in GitHub Actions, it must be **compiled** into a lock file. You commit both the `.md` (human-readable) and the `.lock.yml` (machine-readable) files.

```
.github/workflows/
├── daily-digest.md          ← you write this (plain English)
└── daily-digest.lock.yml    ← compiled (auto-generated)
```

## Scenario

You've been building features for Tailspin Toys across the previous exercises. Your repository now has issues, branches, and pull requests. The team wants a way to stay on top of all this activity without manually checking GitHub every morning.

Before you create that automation, you need to prepare both your terminal environment and your GitHub repository so agentic workflows can run successfully.

## Authenticate with GitHub

If you haven't already authenticated the GitHub CLI, run:

```bash
gh auth login
```

Follow the prompts and choose **GitHub.com** -> **HTTPS** -> **Login with a web browser**.

After logging in, verify it worked:

```bash
gh auth status
```

You should see `Logged in to github.com`.

## Install the Agentic Workflows extension

The `gh aw` CLI extension installs through the standard GitHub CLI extension mechanism, which works across platforms.

```bash
gh extension install github/gh-aw
gh aw version
```

> [!TIP]
> If `gh aw version` returns "unknown command", verify GitHub CLI is installed with `gh --version`, then re-run `gh extension install github/gh-aw`.

## Add the Copilot token secret

Before your workflows can run in GitHub Actions, your repository needs a secret named `COPILOT_GITHUB_TOKEN`. Without it, workflow runs will fail with an error like:

```text
None of the following secrets are set: COPILOT_GITHUB_TOKEN
```

If you're using Copilot as the AI engine, this secret must contain a **fine-grained GitHub Personal Access Token (PAT)**.

### Create the token

1. Create a new fine-grained PAT from your **own GitHub user account**.
2. Under **Permissions** -> **Account permissions**, set **Copilot Requests** to **Read**.
3. Generate the token and copy the value somewhere safe.

### Add it to the repository

You can add the secret in the GitHub UI:

1. Open your repository on GitHub.
2. Go to **Settings** -> **Secrets and variables** -> **Actions**.
3. Click **New repository secret**.
4. Name it `COPILOT_GITHUB_TOKEN` and paste in your token.

Or set it from the CLI:

```bash
gh aw secrets set COPILOT_GITHUB_TOKEN --value "<your-github-pat>"
```

> [!TIP]
> If you believe the secret is already configured but the workflow still fails, check that the name matches exactly, and if it is an organization secret, make sure this repository has been granted access.

## Summary and next steps

Your environment and repository are now ready for agentic workflows. In the next exercise, you'll start from `gh aw init`, generate your first workflow, open a PR for it, merge it, and run it.

## Resources

- [Agentic Workflows Quick Start][aw-quickstart]
- [GitHub CLI][gh-cli]
- [About GitHub Actions][github-actions]

---

[aw-quickstart]: https://github.github.com/gh-aw/setup/quick-start/
[gh-cli]: https://cli.github.com/
[github-actions]: https://docs.github.com/actions
