# Exercise 2 - Providing context to Copilot with instruction files

Context is key across many aspects of life, and when working with generative AI. If you're performing a task which needs to be completed a particular way, or if a piece of background information is important, you want to ensure Copilot has access to that information. You can use [instruction files][instruction-files] to provide guidance so that Copilot not only understands what you want it to do but also how you want it to be done.

In this exercise, you will learn how to:

- provide Copilot with project-specific context, coding guidelines and documentation standards using [repository custom instructions][repository-custom-instructions] **.github/copilot-instructions.md**.
- provide path instruction files to guide Copilot for repetitive or templated tasks on specific types of files.
- implement both repository-wide instructions and task-specific instructions.

> [!IMPORTANT]
> Note that the code generated may diverge from some of the standards you set. AI tools like Copilot are non-deterministic, and may not always provide the same result. The other files in the codebase do not contain XML doc comments or comment headers, which could lead Copilot in another direction. Consistency is key, so making sure that your code follows the established patterns is important. You can always follow-up and ask Copilot to follow your coding standards, which will help guide it in the right direction.

## Scenario

As any good dev shop, Tailspin Toys has a set of guidelines and requirements for development practices. These include:

- API always needs unit tests.
- UI should be in dark mode and have a modern feel.
- Documentation should be added to code in the form of XML doc comments.
- A block of comments should be added to the head of each file describing what the file does.

Through the use of instruction files you'll ensure Copilot has the right information to perform the tasks in alignment with the practices highlighted.

## Custom instructions

Custom instructions allow you to provide context and preferences to Copilot, so that it can better understand your coding style and requirements. This is a powerful feature that can help you steer Copilot to get more relevant suggestions and code snippets. You can specify your preferred coding conventions, libraries, and even the types of comments you like to include in your code. You can create instructions for your entire repository, or for specific types of files for task-level context.

There are two types of instructions files:

- **.github/copilot-instructions.md**, a single instruction file sent to Copilot for **every** prompt for the repository. This file should contain project-level information, context which is relevant for most requests sent to Copilot. This could include the tech stack being used, an overview of what's being built and best practices, and other global guidance for Copilot.
- **\*.instructions.md** files can be created for specific tasks or file types. You can use **\*.instructions.md** files to provide guidelines for particular languages (like C# or TypeScript), or for tasks like creating a Razor component or a new set of unit tests.

## Best practices for managing instructions files

A full conversation about creating instructions files is beyond the scope of the workshop. However, the examples provided in the sample project provide a representative example of how to approach their management. At a high level:

- Keep instructions in **copilot-instructions.md** focused on project-level guidance, such as a description of what's being built, the structure of the project, and global coding standards.
- Use **\*.instructions.md** files to provide specific instructions for file types (unit tests, Razor components, API endpoints), or for specific tasks.
- Use natural language in your instructions files. Keep guidance clear. Provide examples of how code should (and shouldn't) look.

There isn't one specific way to create instructions files, just as there isn't one specific way to use AI. You will find through experimentation what works best for your project. The guidance provided here and the [resources](#resources) below should help you get started.

> [!TIP]
> Every project using GitHub Copilot should have a robust collection of instructions files to provide context and best guide code generation. As you explore the instructions files in the project, you may notice there are ones for numerous types of files and tasks, including [UI updates][ui-instructions] and [Blazor][blazor-instructions]. The investment made in instructions files will greatly enhance the quality of code suggestion from Copilot, ensuring it better matches the style and requirements your organization has.
>
> You can even have Copilot CLI generate instruction files for you as a starting point by running `/init` inside Copilot CLI.
>
> And, if you're looking for templates or a starting point for instructions files, you can explore [awesome-copilot][awesome-copilot], a repository full of instructions files, custom agents, and other resources to help you out!

## Explore the custom instructions files

Let's start by exploring the instructions files created for this project. You'll notice there's one core **copilot-instructions.md** file, and a collection of **.instructions** files for various tasks.

1. Return to your codespace.
2. Open **.github/copilot-instructions.md**.
3. Explore the file, noting the brief description of the project and sections for **Code standards**, **Scripts** and **GitHub Actions Workflows**. These are applicable to any interactions you'd have with Copilot, are robust, and provide clear guidance on what you're doing and how you want to accomplish it.
4. Open **.github/instructions**, and explore the files contained inside it. Note there are instructions for Blazor files, ASP.NET Core endpoints, the various tests, and others.
5. Open **.github/instructions/dotnet-tests.instructions.md**. Make note of the `applyTo` section. This sets the path, relative to the root of the project, which determines which files the instructions apply to. In this case, the pattern matches C# files in test folders such as **server/TailspinToys.Api.Tests**.
6. Note the instructions specific to creating xUnit tests for this project.
7. Finally, open **.github/instructions/aspnetcore-endpoint.instructions.md**, and scroll to the bottom of the file. Note the links to other instructions files and existing files in the project. This allows you to both breakdown larger instruction sets into smaller, reusable files, and to point to examples Copilot should consider when generating code. Note these paths are relative to the instructions file rather than the root of the project.

## Examine the impact of custom instructions

To see the impact of custom instructions, you'll send a prompt with the current version of the files and see how Copilot uses them. Then you'll make some updates, send the same prompt again, and note the difference.

1. Return to your codespace.
2. If not already open, open a terminal window by pressing <kbd>Ctrl</kbd>+<kbd>\`</kbd>.
3. If not already running, start Copilot CLI by issuing the following command in the terminal window:

    ```bash
    copilot
    ```

4. Send the following prompt to create a new endpoint to return all publishers:

    ```plaintext
    Create a new endpoint to return a list of all publishers. It should return the name and id for all publishers.
    ```

5. Copilot explores the project to learn how best to implement the code, and generates suggestions which may include code for `PublishersRoutes.cs`, `Program.cs`, and tests.
6. **Don't accept the suggested changes** — this was just to see what Copilot generates with the current instructions.

> [!TIP]
> Copilot CLI automatically loads **copilot-instructions.md** as well as any matching **.instructions** files when it reads or generates files in your project. You don't need to do anything special — the instructions are picked up automatically.

6. Explore the code, noticing the generated code includes [nullable reference types][csharp-nullable-reference-types] because, as you'll see, the custom instructions includes the directive to use them.

> [!TIP]
> You can use the `/diff` command in Copilot CLI to view the generated or modified files and see exactly what changes were made.

7. Notice the generated code **is missing** either an XML doc comment or a comment header — or both!

> [!IMPORTANT]
> As highlighted previously, GitHub Copilot and LLM tools are probabilistic, not deterministic. As a result, the exact code generated may vary, and there's even a chance it'll abide by your rules without you spelling it out! But to aid consistency in code you should always document anything you want to ensure Copilot should understand about how you want your code generated.

## Add new repository standards to copilot-instructions.md

As highlighted previously, `copilot-instructions.md` is designed to provide project-level information to Copilot. Let's ensure repository coding standards are documented to improve code suggestions.

1. Return to your codespace.
2. Open `.github/copilot-instructions.md` in your editor.
3. Locate the **Code formatting requirements** section, which should be near line 35. Note how it contains a note to use nullable reference types. That's why you saw those in the code generated previously.
4. Add the following lines of markdown right below the note about nullable reference types to instruct Copilot to add comment headers to files and XML doc comments (which should be near line 35):

   ```markdown
   - Every public method should have XML doc comments or the language equivalent.
   - Before the namespace declaration or any code, add a comment block to the file that explains its purpose.
   ```

5. Save and close **copilot-instructions.md**.
6. Return to the terminal, clear the conversation context and restart Copilot CLI so it picks up the updated instructions:

    ```
    /clear
    ```

    ```
    /restart
    ```

7. Send the same prompt as before to create the endpoint:

   ```plaintext
   Create a new endpoint to return a list of all publishers. It should return the name and id for all publishers.
   ```

8. Use the `/diff` command to view the generated files and see the changes Copilot made.

9.  Notice how the newly generated code includes a comment header at the top of the file which resembles the following:

   ```csharp
   // Publishers routes - exposes endpoints for listing and retrieving publisher data.
   ```

10. Notice how the newly generated code includes XML doc comments on the class and method which resemble the following:

   ```csharp
   /// <summary>
   /// Minimal API route group for publisher-related endpoints.
   /// </summary>
   public static class PublishersRoutes
   {
       /// <summary>
       /// Registers all publisher routes on the application.
       /// </summary>
       /// <param name="app">The <see cref="WebApplication"/> to register routes on.</param>
       public static void MapPublishersRoutes(this WebApplication app)
   ```

11. Notice the generated code now includes an XML doc comment as well as a comment block at the top!
12. Also note how the existing code isn't updated, but of course you could ask Copilot to perform that operation if you so desired!
13. **Don't accept the suggested changes**, as you'll be implementing features in a later exercise.

> [!NOTE]
> If you accepted the changes, you can always use `git checkout .` to discard uncommitted file changes.

From this section, you explored how the custom instructions file has provided Copilot with the context it needs to generate code that follows the established guidelines.

## Explore the impact of a .instructions file

Our focus in the last two sets of steps was on **copilot-instructions.md**, the global instructions file used for all chat requests for Copilot Chat, Copilot Coding Agent (CCA), and Copilot CLI. Now let's explore the impact of a **.instructions** file.

**.instructions** files can contain an `applyTo` setting in its frontmatter, which allows you to specify a slug or path. Copilot will utilize these instructions whenever it works on a file which matches the slug. In our case, we have an instructions file for .NET tests defined at **.github/instructions/dotnet-tests.instructions.md**, which will be used by Copilot for any files which match the pattern **`**/*Tests*/**/*.cs`**.

> [!NOTE]
> There's a chance Copilot already generated test code in the prior step, so you might be looking at the same code again. To ensure we can see the behavior, we're going to clear context, be a bit more specific with the prompt, and see the tests Copilot generates based on the instructions.

1. Return to the terminal with Copilot CLI running.
2. Clear the conversation context to start fresh:

    ```
    /clear
    ```

3. Send the following prompt to ensure tests are generated:

    ```
    Create a new endpoint to return a list of all publishers. It should return the name and id for all publishers. Also generate the tests for the newly generated endpoint.
    ```

> [!TIP]
> Use the `/diff` command to view the generated files and review what Copilot created.

4. Explore the generated code for the test. Based on the instructions, it should:
    - utilize xUnit with the `[Fact]` or `[Theory]` attribute.
    - use `WebApplicationFactory<Program>` for integration testing.
    - contain both setup and teardown via constructor and `IDisposable`.

5. **Keep the generated code** — we'll be using it in later exercises.

## Commit and push your changes

Now that you have generated the publishers endpoint with the correct coding standards, let's commit this to the `main` branch so it's available for the rest of the workshop.

1. Return to the terminal. If Copilot CLI is running, you can run shell commands directly by prefixing with `!`:

    ```
    !git add -A && git commit -m "Add publishers endpoint with tests" && git push
    ```

    Or if you prefer, exit Copilot CLI and run:

    ```bash
    git add -A
    git commit -m "Add publishers endpoint with tests"
    git push
    ```

> [!NOTE]
> If you accepted changes from an earlier step by mistake, you can always reset to a clean state with `git checkout .` and then re-run only the final prompt from the previous section.

## Summary and next steps

Congratulations! You explored how to ensure Copilot has the right context to generate code following the practices your organization has set forth. This can be done at a repository level with the **.github/copilot-instructions.md** file, or on a task basis with instruction files. You explored how to:

- provide Copilot with project-specific context, coding guidelines and documentation standards using custom instructions (.github/copilot-instructions.md).
- use instruction files to guide Copilot for repetitive or templated tasks.
- implement both repository-wide instructions and task-specific instructions.

## Resources

- [Instruction files for GitHub Copilot customization][instruction-files]
- [5 tips for writing better custom instructions for Copilot][copilot-instructions-five-tips]
- [Best practices for creating custom instructions][instructions-best-practices]
- [Personal custom instructions for GitHub Copilot][personal-instructions]
- [Awesome Copilot - a collection of instructions files and other resources][awesome-copilot]

---

[instruction-files]: https://code.visualstudio.com/docs/copilot/copilot-customization
[repository-custom-instructions]: https://docs.github.com/copilot/how-tos/configure-custom-instructions/add-repository-instructions
[csharp-nullable-reference-types]: https://learn.microsoft.com/dotnet/csharp/nullable-references
[instructions-best-practices]: https://docs.github.com/enterprise-cloud@latest/copilot/using-github-copilot/coding-agent/best-practices-for-using-copilot-to-work-on-tasks#adding-custom-instructions-to-your-repository
[personal-instructions]: https://docs.github.com/copilot/customizing-copilot/adding-personal-custom-instructions-for-github-copilot
[copilot-instructions-five-tips]: https://github.blog/ai-and-ml/github-copilot/5-tips-for-writing-better-custom-instructions-for-copilot/
[awesome-copilot]: https://github.com/github/awesome-copilot
[ui-instructions]: https://github.com/mattleibow/tailspin-toys-workshop-dotnet/blob/main/.github/instructions/tailwindcss.instructions.md
[blazor-instructions]: https://github.com/mattleibow/tailspin-toys-workshop-dotnet/blob/main/.github/instructions/blazor.instructions.md
