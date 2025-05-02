using Phonebook.KamilKolanowski.Models;
using Phonebook.KamilKolanowski.Services;
using Phonebook.KamilKolanowski.Views;
using Spectre.Console;
using static Phonebook.KamilKolanowski.Models.PhoneBookMenu;

namespace Phonebook.KamilKolanowski.Controllers;

internal class PhoneBookController
{
    PhoneBookService _phoneBookService = new();
    ViewPhoneBook _viewPhoneBook = new();

    internal void ContactsOperation()
    {
        while (true)
        {
            Console.Clear();

            var selectOperation = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title("What would you like to do?")
                    .AddChoices(Menu.Values)
            );

            var selectedOperation = PhoneBookMenu
                .Menu.FirstOrDefault(v => v.Value == selectOperation)
                .Key;

            var selectedContactType = AnsiConsole.Prompt(
                new SelectionPrompt<ContactCategoryMenuType>()
                    .Title("Choose contact category")
                    .AddChoices(Enum.GetValues<ContactCategoryMenuType>())
            );

            switch (selectedOperation)
            {
                case PhoneBookMenuType.AddContact:
                    AddContact(selectedContactType);
                    break;
                case PhoneBookMenuType.DeleteContact:
                    DeleteContact(selectedContactType);
                    break;
                case PhoneBookMenuType.EditContact:
                    EditContact(selectedContactType);
                    break;
                case PhoneBookMenuType.ShowContact:
                    ViewContacts(selectedContactType);
                    break;
                case PhoneBookMenuType.Exit:
                    Environment.Exit(0);
                    break;
            }
        }
    }

    private void AddContact(ContactCategoryMenuType type)
    {
        _phoneBookService.AddContact(type);
    }

    private void DeleteContact(ContactCategoryMenuType type)
    {
        var contacts = _phoneBookService.GetContacts().ToList();

        _viewPhoneBook.ViewContacts();

        var rowIndexToDelete = AnsiConsole.Prompt(
            new TextPrompt<int>("Select the contact Id to delete: ").Validate(index =>
                index > 0 && index <= contacts.Count
                    ? ValidationResult.Success()
                    : ValidationResult.Error("Invalid contact number.")
            )
        );

        var contactId = contacts[rowIndexToDelete - 1].Id;

        _phoneBookService.DeleteContact(contactId);
    }

    private void EditContact(ContactCategoryMenuType type)
    {
        var contacts = _phoneBookService.GetContacts().ToList();
        _viewPhoneBook.ViewContacts();

        var rowIndexToUpdate = AnsiConsole.Prompt(
            new TextPrompt<int>("Select the contact Id to edit: ").Validate(index =>
                index > 0 && index <= contacts.Count
                    ? ValidationResult.Success()
                    : ValidationResult.Error("Invalid contact number.")
            )
        );

        var contactId = contacts[rowIndexToUpdate - 1].Id;

        AnsiConsole.MarkupLine("\nSelect the column to edit");
        var selectColumnToUpdate = AnsiConsole.Prompt(
            new SelectionPrompt<string>().AddChoices(
                "First Name",
                "Last Name",
                "Email Address",
                "Phone Number"
            )
        );

        string newValue;

        if (selectColumnToUpdate == "Email Address")
        {
            while (true)
            {
                newValue = AnsiConsole.Ask<string>(
                    $"Provide new value for {selectColumnToUpdate} [yellow](e.g. john.doe@gmail.com)[/]: "
                );
                var result = _phoneBookService.ValidateEmail(newValue);
                if (result.Successful)
                    break;
                AnsiConsole.MarkupLine($"[red]{result.Message}[/]");
            }
        }
        else if (selectColumnToUpdate == "Phone Number")
        {
            while (true)
            {
                newValue = AnsiConsole.Ask<string>(
                    $"Provide new value for {selectColumnToUpdate} [yellow](e.g. +123456789)[/]: "
                );
                var result = _phoneBookService.ValidatePhone(newValue);
                if (result.Successful)
                    break;
                AnsiConsole.MarkupLine($"[red]{result.Message}[/]");
            }
        }
        else
        {
            newValue = AnsiConsole.Ask<string>($"Provide new value for {selectColumnToUpdate}: ");
        }

        _phoneBookService.EditContact(contactId, selectColumnToUpdate, newValue);
    }

    private void ViewContacts(ContactCategoryMenuType type)
    {
        _viewPhoneBook.ViewContacts();
        AnsiConsole.MarkupLine("Press any key to continue...");
        Console.ReadKey();
    }
}
