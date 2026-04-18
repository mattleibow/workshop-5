---
on:
  schedule:
    - cron: "0 9 * * 1-5"
  workflow_dispatch:
permissions:
  contents: read
  issues: read
network:
  allowed:
    - defaults
    - www.freetogame.com
safe-outputs:
  mentions: false
  allowed-github-references: []
  create-issue:
    title-prefix: "🎮 Trending Free Games –"
    close-older-issues: true
    expires: 7
---

# Trending Free Games Digest

Every weekday, fetch the current list of popular free-to-play games from
the FreeToGame API and create a GitHub issue summarising the top 15.

## Data Source

Fetch games sorted by popularity from:

```
https://www.freetogame.com/api/games?sort-by=popularity
```

Take the **first 15 items** from the response array.

## Title

Use this exact title format:

```
🎮 Trending Free Games – <YYYY-MM-DD>
```

Replace `<YYYY-MM-DD>` with today's date (UTC).

## Report Structure

Use `###` and lower for all headings. Never use `##` or `#` in the issue body.

### Intro Paragraph

Begin with a short paragraph explaining that these are currently popular
free-to-play games according to FreeToGame, refreshed daily on weekdays.

### Games Table

Present the 15 games as a Markdown table with these columns, in this order:

| Column | Source field |
|---|---|
| # | Position (1–15) |
| Game | `title` linked to `freetogame_profile_url` |
| Genre | `genre` |
| Platform | `platform` |
| Publisher | `publisher` |
| Release Date | `release_date` |

Example row:

```
| 1 | [Game Title](https://www.freetogame.com/game/123) | Strategy | PC (Windows) | Acme Corp | 2023-04-12 |
```

If the API request fails or returns fewer than 15 games, note this in the
issue body and include however many games were returned.
