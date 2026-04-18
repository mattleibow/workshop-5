# Exercise 11 - Trending Games Digest

In the previous exercise you used the ChatOps pattern to trigger a workflow from a GitHub comment. Now you'll build something more powerful: a workflow that reaches out to an **external API** and brings data into your GitHub repository automatically.

**Estimated time:** 20 minutes

## Objectives

- Write a targeted natural-language prompt for an agentic workflow that calls an external API
- Understand frontmatter configuration: schedules, network allowlists, and safe outputs
- Inspect and refine the generated workflow markdown
- Validate that the output issue is useful and well-structured

## Scenario

Tailspin Toys is a crowdfunding platform for games. The team wants to stay on top of what's trending in the free-to-play game world so they can spot opportunities — popular titles, genres, and release patterns that could inspire new features. You'll create a workflow that automatically fetches a **FreeToGame popularity feed** every morning and posts it as a GitHub issue.

## Background: The FreeToGame API

[FreeToGame](https://www.freetogame.com/api-doc) provides a public JSON API for free-to-play games and does not require authentication for demo use:

- `GET https://www.freetogame.com/api/games?sort-by=popularity` — returns games sorted by popularity
- `GET https://www.freetogame.com/api/game?id=<id>` — returns details for a specific game

Your agentic workflow will call these endpoints, parse the results, and format them into a readable GitHub issue.

## Part 1 — Create the workflow

Make sure you are still in the `agentic-workflows` agent. If not, type `/agent` and select it again.

Then provide the following description:

```
Create a daily digest workflow for the Tailspin Toys team that tracks
popular free-to-play games. Every weekday, fetch the games list from the
FreeToGame API sorted by popularity
(https://www.freetogame.com/api/games?sort-by=popularity). For the top 15
games, include: the game name, genre, platform, publisher, release date,
and a link to its FreeToGame page from the `freetogame_profile_url`
field. Create a GitHub issue titled "🎮 Trending Free Games – <date>"
with the results formatted as a Markdown table. Add a brief intro
paragraph explaining these are currently popular free-to-play games
according to FreeToGame.

Name the workflow file trending-games.
Allow for manual runs.
```

The agent will confirm the weekday schedule, issue-creation safeguards, timeout, and the FreeToGame network allowlist, then generate the workflow files.

## Part 2 — Review and refine the workflow

Open the generated workflow file:

```bash
cat .github/workflows/trending-games.md
```

The file has two parts:

**YAML frontmatter** (between `---` markers) — configuration that requires recompilation:

```yaml
---
name: Trending Free Games
description: >
 Every weekday, fetches the top 15 most popular free-to-play games from the
 FreeToGame API and creates a GitHub issue with a Markdown table summary for
 the Tailspin Toys team.
on:
 schedule: daily on weekdays
 workflow_dispatch:
permissions:
 issues: read
network:
 allowed:
 - defaults
 - www.freetogame.com
checkout: false
timeout-minutes: 10
safe-outputs:
 mentions: false
 allowed-github-references: []
 create-issue:
  title-prefix: "🎮 Trending Free Games –"
  labels: [digest, games]
  close-older-issues: true
  expires: 14
---
```

**Markdown body** (after the frontmatter) — the plain-English instructions. You can edit the body directly and your changes will take effect on the next run, **without recompiling**.

> [!NOTE]
> If you want to change the trigger, permissions, network rules, or issue output settings in the frontmatter, you need to recompile: `gh aw compile trending-games`.

### Things to check

1. **Network allowlist** — does the frontmatter include `www.freetogame.com`? The agent needs network access to call the FreeToGame API.
2. **Schedule** — does it use fuzzy scheduling (`daily on weekdays`) rather than a fixed cron? Fuzzy scheduling is preferred because it distributes load and automatically adds `workflow_dispatch` for manual runs.
3. **Prompt body** — does the body clearly describe the filtering criteria and the desired output format?

If you want to adjust the prompt, simply edit the markdown body and the change takes effect on the next run. To update the frontmatter (e.g., add a network domain), edit the frontmatter and recompile:

```bash
gh aw compile trending-games
```

## Part 3 — Push to main and run the workflow

Commit and push the workflow files directly to `main`:

```bash
git add -A
git commit -m "Add trending games digest workflow"
git push
```

Then trigger a manual run:

```bash
gh aw run trending-games
```

Wait for the run to complete, then open GitHub and check the **Issues** tab.

### What to check

- The issue title contains today's date and the 🎮 emoji.
- The issue body is a Markdown table with game names, genres, platforms, release dates, and FreeToGame links.
- The games are real, currently popular free-to-play games from FreeToGame.

> [!TIP]
> If the output looks good but the formatting is off, edit the markdown body of `.github/workflows/trending-games.md` to tweak the output template, then push and re-run — no recompilation needed.

## Success criteria

- [ ] `.github/workflows/trending-games.md` exists in your repository
- [ ] `.github/workflows/trending-games.lock.yml` exists in your repository
- [ ] The workflow frontmatter includes `www.freetogame.com` in the network allowlist
- [ ] The workflow uses fuzzy scheduling (`daily on weekdays`)
- [ ] A GitHub issue titled **🎮 Trending Free Games – \<today's date\>** was created with a table of popular games

## Summary and next steps

You've created a workflow that automatically monitors popular game trends for Tailspin Toys. You learned how to:

- write a targeted prompt for an agentic workflow that calls an external API.
- configure network allowlists, tools, and safe outputs in the frontmatter.
- review, refine, compile, and trigger a workflow.
- validate the output issue.

This optional extension shows how Agentic Workflows can also pull live data from external services and turn it into useful updates for your team.

## Resources

- [FreeToGame API Docs][freetogame-api]
- [Agentic Workflows Reference][aw-reference]

---

[freetogame-api]: https://www.freetogame.com/api-doc
[aw-reference]: https://github.github.com/gh-aw/
