# Exercise 12 - Review and Next Steps

Congratulations on completing the workshop! 🎉🚀

Over the course of this workshop, you've gone from learning about Copilot CLI to building features for a real application, and then automating your development workflow with agentic workflows.

## What you learned

### Part 1: Building with Copilot CLI

1. **Installing Copilot CLI** — Set up and authenticated GitHub Copilot in the terminal.
2. **Custom Instructions** — Provided Copilot with project-specific context using `.github/copilot-instructions.md` and `.instructions` files.
3. **MCP Servers** — Connected to GitHub, Playwright, and Microsoft Learn via Model Context Protocol to extend Copilot's capabilities.
4. **Generating Code** — Used plan mode to design features and generated code to implement filtering in Tailspin Toys.
5. **Agent Skills** — Leveraged reusable skills to create pull requests that follow team guidelines.
6. **Custom Agents** — Used a specialized accessibility agent to review and improve the site.
7. **Slash Commands** — Explored `/share`, `/context`, `/model`, and other commands for managing sessions and configuration.

### Part 2: Automating with Agentic Workflows

8. **Agentic Workflows Setup** — Authenticated GitHub CLI, installed `gh aw`, and configured the repository secret needed for Actions runs.
9. **First Agentic Workflow** — Initialised workflows in your repo and created a daily issue/PR digest.
10. **ChatOps Game Lookup** — Created a `/game-lookup` slash command that lets team members look up game details directly from a GitHub issue comment.
11. **Trending Games Digest** — Built a scheduled workflow that pulls popular free-to-play games from the FreeToGame API and posts them as a daily GitHub issue.

## Key takeaways

- **Context is king** — Instructions files, clear prompts, and structured guidance produce dramatically better results from AI tools. The investment in `.github/copilot-instructions.md` and `.instructions` files pays dividends.
- **Skills and agents are reusable** — Once you define a skill or agent, it works across Copilot CLI, Copilot Chat, and Copilot Coding Agent. Build them once, use them everywhere.
- **Prompts are code** — Agentic workflow prompts live in version control, can be reviewed in PRs, and evolve with your project. Treat them with the same care as source code.
- **ChatOps keeps context in GitHub** — Slash commands triggered by issue comments mean your team never has to leave GitHub to run an agent or look up information.
- **Iterate quickly** — Whether you're refining a Copilot CLI prompt or an agentic workflow, the fastest way to improve is to try, review, and refine.

## Best practices

### For Copilot CLI
- Invest in robust instructions files — they're the single most impactful thing you can do for code quality.
- Use plan mode for complex features before jumping into implementation.
- Clear context with `/clear` when switching tasks; use `/compact` when context is getting large.
- Share sessions with `/share` to help your team learn from your prompts and workflows.

### For Agentic Workflows
- Start simple — a daily digest is a great first workflow for any repository.
- Always check the network allowlist — if your workflow calls an external API, the domain must be listed.
- Edit prompts directly for quick iterations; recompile only when changing frontmatter.
- Use `workflow_dispatch` alongside scheduled triggers so you can test immediately.

## Ideas for extending what you built

- **Enhance the trending games digest** — Add game ratings, player counts, or filter by category (strategy, party, etc.).
- **Add more slash commands** — Try `/game-compare <game1> vs <game2>` to compare two games side by side.
- **Connect to more APIs** — Create a workflow that monitors a game crowdfunding platform (like Kickstarter's RSS feeds) for new game projects.
- **PR automation** — Build a workflow that automatically labels PRs based on the files changed, or posts a summary comment on every new PR.
- **Team notifications** — Create a workflow that posts a weekly roundup of merged PRs and closed issues.

## Continue your learning

- 📖 [About Copilot CLI][about-copilot-cli] — official documentation
- 🔧 [Using Copilot CLI][using-copilot-cli] — how-to guides
- ⚡ [Agentic Workflows Quick Start][aw-quickstart] — the official getting-started guide
- 🔌 [ChatOps Pattern Reference][chatops-pattern] — full documentation for slash commands
- 🌟 [Awesome Copilot][awesome-copilot] — community resources, instructions files, skills, and agents
- 🤖 [Agent Skills Specification][agent-skills-spec] — the open standard for skills
- 📝 [Custom Instructions Guide][repo-instructions] — best practices for instruction files
- 🔗 [MCP Specification][mcp-spec] — the Model Context Protocol standard
- 💬 [GitHub Community Discussions][github-community] — ask questions and share ideas

## Feedback

We'd love to hear your feedback! Please share your thoughts with the workshop facilitators or open an issue in this repository.

---

*Thank you for participating! Happy coding and automating with GitHub Copilot! 🚀🤖*

[about-copilot-cli]: https://docs.github.com/copilot/concepts/agents/about-copilot-cli
[using-copilot-cli]: https://docs.github.com/copilot/how-tos/use-copilot-agents/use-copilot-cli
[aw-quickstart]: https://github.github.com/gh-aw/setup/quick-start/
[chatops-pattern]: https://github.github.com/gh-aw/patterns/chat-ops/
[awesome-copilot]: https://github.com/github/awesome-copilot
[agent-skills-spec]: https://agentskills.io/specification
[repo-instructions]: https://docs.github.com/copilot/how-tos/configure-custom-instructions/add-repository-instructions
[mcp-spec]: https://modelcontextprotocol.io/
[github-community]: https://github.com/orgs/community/discussions
