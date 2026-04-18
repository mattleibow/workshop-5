---
on:
  slash_command: game-lookup
  workflow_dispatch:
permissions:
  contents: read
  issues: read
network:
  allowed:
    - defaults
    - www.freetogame.com
rate-limit:
  max: 5
  window: 60
safe-outputs:
  mentions: false
  add-comment:
    max: 1
    hide-older-comments: true
    discussions: false
    pull-requests: false
---

# Game Lookup

A ChatOps command that looks up a game by name using the FreeToGame API and replies
with details directly on the issue.

## Trigger

This workflow runs when a user posts a comment on a GitHub issue containing:

```
/game-lookup <game name>
```

## Instructions

### 1. Extract the game name

Read the body of the comment that triggered this workflow (issue comment on issue
`${{ github.event.issue.number }}`). The comment body starts with `/game-lookup`.
Everything after `/game-lookup` (trimmed) is the **game name input**.

If the game name input is empty or blank, reply with:

> ❌ **No game name provided.**
> Usage: `/game-lookup <game title>`
> Example: `/game-lookup Fortnite`

Then stop.

### 2. Fetch the full game list

Make an HTTP GET request to:

```
https://www.freetogame.com/api/games
```

The response is a JSON array. Each element has at minimum: `id`, `title`, `genre`,
`platform`, `publisher`, `developer`, `release_date`, `short_description`,
`freetogame_profile_url`.

### 3. Find the best match

Search the game list for the best match against the extracted game name:

1. **Exact match** (case-insensitive): `title.toLowerCase() === input.trim().toLowerCase()`
2. **Contains match** (case-insensitive): `title.toLowerCase().includes(input.trim().toLowerCase())`

Use the first result from step 1 if found; otherwise the first result from step 2.

If no match is found in either pass, reply with:

> ❌ **No game found matching that title.**
> Try a different spelling or a partial title (e.g. `/game-lookup Forge` to find "Forge of Empires").

Then stop.

### 4. Fetch the game details

Use the `id` from the matched game to fetch full details:

```
https://www.freetogame.com/api/game?id=<id>
```

### 5. Extract the following fields

| Field | JSON key |
|---|---|
| Name | `title` |
| Genre | `genre` |
| Platform | `platform` |
| Publisher | `publisher` |
| Developer | `developer` |
| Release date | `release_date` |
| Short description | First 200 characters of `short_description` (append `…` if truncated) |
| Profile URL | `freetogame_profile_url` |

### 6. Reply with the formatted result

Post a comment on the triggering issue using the following Markdown template:

```markdown
### 🎮 <title>

| | |
|---|---|
| **Genre** | <genre> |
| **Platform** | <platform> |
| **Publisher** | <publisher> |
| **Developer** | <developer> |
| **Release date** | <release_date> |

> <short_description (up to 200 chars, with … if truncated)>

🔗 [View on FreeToGame](<freetogame_profile_url>)
```

Do not add any extra commentary outside this template.
