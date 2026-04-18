---
on:
  schedule:
    - cron: "0 8 * * 1-5"
  workflow_dispatch:
permissions:
  contents: read
  issues: read
  pull-requests: read
safe-outputs:
  mentions: false
  allowed-github-references: []
  create-issue:
    title-prefix: "Daily Digest –"
    close-older-issues: true
    expires: 7
---

# Daily Digest

Every weekday, create a GitHub issue summarising all currently open issues and pull requests in this repository.

## Title

Use this exact title format:

```
Daily Digest – <YYYY-MM-DD>
```

Replace `<YYYY-MM-DD>` with today's date (UTC).

## Report Structure

Follow the structure below. Use `###` and lower for all headings.

### Overview

Begin with a one-paragraph summary:

- Total open issues
- Total open pull requests
- Number of distinct labels represented

### Open Issues

List **all** open issues grouped by label. Items with no label appear under **Unlabelled**.

Within each label group, sort items from oldest to newest (longest open first).

For each item include:

| Field | Format |
|---|---|
| Title (linked) | `[#123 Title](url)` |
| Author | `@author` (will be escaped automatically) |
| Age | Human-readable, e.g. `3 days`, `2 weeks`, `1 month` |

Show the count for each group as a heading, e.g. `### 🏷️ bug (4 issues)`.

### Open Pull Requests

Same structure as Open Issues — grouped by label, sorted oldest first, with count per group.

### Summary Table

End the report with a compact table:

| Label | Issues | Pull Requests | Total |
|---|---|---|---|
| bug | N | N | N |
| ... | | | |
| **Total** | **N** | **N** | **N** |

## Style Notes

- Use `<details><summary>View all items</summary>…</details>` to collapse any group that has more than 10 items, keeping the issue readable at a glance.
- Use emoji to make the report scannable: ✅ for nothing open, ⚠️ for many items open (>20 total), 🔴 for very many (>50 total).
- Do **not** add a footer or attribution — it is appended automatically.
