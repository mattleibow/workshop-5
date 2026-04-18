---
name: Code review
description: Guidelines for reviewing code in this project. Use this when asked to review code, audit the codebase, check for best practices, or validate code quality.
---

# Code Review

## Overview

You are performing a code review of the application or the subsection as indicated by the user. The goal here is to provide just the review and feedback, with suggestions on what should be updated. The generated result should follow this pattern:

```markdown
- **Overall notes**
    - item
    - item
    - item
- **Test status**
    - item
    - item
- **High impact fixes**
    - item
    - item
    - item
- **Medium impact fixes**
    - item
    - item
    - item
- **Low impact fixes**
    - item
    - item
    - item
```

Each piece of feedback should be a single sentence. If more details are needed, create a sub-bullet with that information. Don't perform any actions unless the user tells you to actually implement them.

## Review Philosophy

### Focus on What Matters
- Prioritize correctness, security, and maintainability over style nitpicks
- Identify bugs, logic errors, and potential runtime issues first
- Check for security vulnerabilities and data handling issues
- Ensure code follows established project patterns

### Use Available Tools
Before reviewing, leverage MCP tools to validate code:

**For Blazor Components:**
- Use the Microsoft Learn MCP to verify current Blazor and ASP.NET Core guidance
- Check render-mode usage, component parameters, and accessibility patterns

**For Browser and E2E Validation:**
- Use the Playwright MCP to inspect UI behavior and verify participant-visible flows
- Confirm `data-testid` usage, navigation behavior, and accessible interactions

### Run tests

Before performing the review, run both the unit and e2e tests. Add those results to the output following the pattern indicated above.

## Code Quality Checklist

### All Code
- [ ] Follows existing patterns in the codebase
- [ ] Has appropriate error handling
- [ ] Is readable and self-documenting
- [ ] Avoids unnecessary complexity

### C# (Backend)
- [ ] Type annotations on all method parameters and return values
- [ ] Entity Framework Core models have `ToDict()` methods
- [ ] Minimal API route groups used for organizing endpoints
- [ ] RESTful conventions followed
- [ ] Proper status codes returned (200, 404, 400, etc.)

### Blazor Components
- [ ] Uses `@rendermode InteractiveServer` for interactive components
- [ ] Interactive elements have `data-testid` attributes
- [ ] Parameters have proper types with `[Parameter]` attribute
- [ ] Loading/error/empty states handled properly

### Styling (Tailwind CSS)
- [ ] Dark theme colors used (slate palette)
- [ ] Utility classes only - no custom CSS unless necessary
- [ ] Focus states included for accessibility
- [ ] Responsive design considered

### Accessibility
- [ ] Semantic HTML elements used
- [ ] ARIA labels where needed
- [ ] `aria-hidden="true"` on decorative elements (icons, SVGs)
- [ ] Keyboard navigation works
- [ ] Sufficient color contrast

## Common Issues to Flag

### Critical (Must Fix)
- Security vulnerabilities
- Missing error handling that could crash the app
- Broken functionality
- Missing type annotations in C#
- Null reference issues

### Important (Should Fix)
- Missing `data-testid` on interactive elements
- Accessibility violations
- Inconsistent patterns with rest of codebase
- Missing or incorrect types

### Minor (Consider Fixing)
- Code could be simplified
- Better variable names possible
- Documentation could be clearer

## Review Process

1. **Understand the context** - What is this code trying to do?
2. **Check patterns** - Does it follow project conventions?
3. **Validate with tools** - Use MCP tools to catch issues
4. **Test coverage** - Are there tests? Should there be?
5. **Run tests** - Use `./scripts/run-server-tests.sh` and `./scripts/run-e2e-tests.sh` (or `.ps1` equivalents on Windows)

## Project-Specific Patterns

### Technology Separation
- **Blazor Pages**: Page routing, layouts, interactive components
- **Razor Components**: Reusable UI components
- **Tailwind**: All styling via utility classes
- **ASP.NET Core**: API endpoints via Minimal API route groups

### File Locations
- Blazor components: `client/TailspinToys.Web/Components/Shared/`
- Blazor pages: `client/TailspinToys.Web/Components/Pages/`
- API routes: `server/TailspinToys.Api/Routes/`
- Backend tests: `server/TailspinToys.Api.Tests/`
- E2E tests: `client/TailspinToys.E2E/`

### Required Before Merge
- All tests pass
- No regressions in existing functionality
- New features have appropriate test coverage
- Documentation updated if needed
