using Microsoft.Extensions.Options;
using Phonebook.KamilKolanowski.Models;
using Phonebook.KamilKolanowski.Services;
using Phonebook.KamilKolanowski.Views;
using Spectre.Console;

namespace Phonebook.KamilKolanowski.Controllers;

internal class PhoneBookController
{
    PhoneBookService _phoneBookService = new PhoneBookService();
    ViewPhoneBook _viewPhoneBook = new ViewPhoneBook();

    internal void ContactsOperation()
    {
        while (true)
        {
            Console.Clear();

            var selectOperation = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title("What would you like to do?")
                    .AddChoices(PhoneBookMenu.Menu.Values)
            );

            var selectedOperation = PhoneBookMenu
                .Menu.FirstOrDefault(v => v.Value == selectOperation)
                .Key;

            switch (selectedOperation)
            {
                case PhoneBookMenu.PhoneBookMenuType.AddContact:
                    AddContact();
                    break;
                case PhoneBookMenu.PhoneBookMenuType.DeleteContact:
                    DeleteContact();
                    break;
                case PhoneBookMenu.PhoneBookMenuType.EditContact:
                    EditContact();
                    break;
                case PhoneBookMenu.PhoneBookMenuType.ShowContact:
                    ViewContacts();
                    break;
                case PhoneBookMenu.PhoneBookMenuType.Exit:
                    Environment.Exit(0);
                    break;
            }
        }
    }

    private void AddContact()
    {
        _phoneBookService.AddContact();
    }

    private void DeleteContact()
    {
        var contacts = _phoneBookService.GetContacts().ToList();

        _viewPhoneBook.ViewContacts();

        var rowIndexToDelete = AnsiConsole.Prompt(
            new TextPrompt<int>("Select the contact number to delete: ").Validate(index =>
                index > 0 && index <= contacts.Count
                    ? ValidationResult.Success()
                    : ValidationResult.Error("Invalid contact number.")
            )
        );

        var contactId = contacts[rowIndexToDelete - 1].Id;

        _phoneBookService.DeleteContact(contactId);
    }

    private void EditContact()
    {
        var contacts = _phoneBookService.GetContacts().ToList();
        _viewPhoneBook.ViewContacts();

        var rowIndexToUpdate = AnsiConsole.Prompt(
            new TextPrompt<int>("Select the contact number to edit: ").Validate(index =>
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
                    $"Provide new value for {selectColumnToUpdate}: "
                );
                var result = _phoneBookService.ValidateEmail(newValue);
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

    private void ViewContacts()
    {
        _viewPhoneBook.ViewContacts();
        AnsiConsole.MarkupLine("Press any key to continue...");
        Console.ReadKey();
    }
}
