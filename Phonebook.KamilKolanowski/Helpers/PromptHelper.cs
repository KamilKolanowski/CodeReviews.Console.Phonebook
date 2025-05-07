using Phonebook.KamilKolanowski.Models;
using Phonebook.KamilKolanowski.Services;
using Spectre.Console;

namespace Phonebook.KamilKolanowski.Helpers;

internal class PromptHelper
{
    private readonly PhoneBookService _phoneBookService = new();

    internal int PromptForContactId(List<Contact> contacts)
    {
        var index = AnsiConsole.Prompt(
            new TextPrompt<int>("Select the contact Id: ").Validate(i =>
                i > 0 && i <= contacts.Count
                    ? ValidationResult.Success()
                    : ValidationResult.Error("[red]Invalid contact Id.[/]")
            )
        );

        return contacts[index - 1].Id;
    }

    internal string PromptForColumnToEdit()
    {
        AnsiConsole.MarkupLine("\nSelect the column to edit");
        return AnsiConsole.Prompt(
            new SelectionPrompt<string>().AddChoices(
                "First Name",
                "Last Name",
                "Email Address",
                "Phone Number"
            )
        );
    }

    internal string PromptForNewValue(string column)
    {
        while (true)
        {
            var input = AnsiConsole.Ask<string>($"Provide new value for {column}: ");

            if (column == "Email Address")
            {
                var result = _phoneBookService.ValidateEmail(input);
                if (result.Successful)
                    return input;
                AnsiConsole.MarkupLine($"[red]{result.Message}[/]");
            }
            else if (column == "Phone Number")
            {
                var result = _phoneBookService.ValidatePhone(input);
                if (result.Successful)
                    return input;
                AnsiConsole.MarkupLine($"[red]{result.Message}[/]");
            }
            else
            {
                return input;
            }
        }
    }
}
