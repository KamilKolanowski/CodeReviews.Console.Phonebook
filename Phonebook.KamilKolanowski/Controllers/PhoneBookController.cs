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
        while (true)
        {
            _phoneBookService.AddContact(type);
            
            var addAnother = AnsiConsole.Confirm($"Do you want to add another contact to {type} category?");
            Console.Clear();
            if (!addAnother) 
                return;
        }
        
    }

    private void DeleteContact(ContactCategoryMenuType type)
    {
        while (true)
        {
            var contacts = _phoneBookService.GetContacts(type).ToList();

            if (!ValidateIfTableIsEmpty(contacts))
            {
                AnsiConsole.MarkupLine("[bold red]There's no contact to delete.[/]");
                AnsiConsole.MarkupLine("Press any key to continue...");
                Console.ReadKey();
                return;
            }

            _viewPhoneBook.ViewContacts(type);

            var contactId = PromptForContactId(contacts);
            _phoneBookService.DeleteContact(contactId);

            var deleteAnother = AnsiConsole.Confirm("Do you want to delete another contact?");
            Console.Clear();
            if (!deleteAnother)
                return;
        }
    }

    private void EditContact(ContactCategoryMenuType type)
    {
        while (true)
        {
            var contacts = _phoneBookService.GetContacts(type).ToList();
            if (!ValidateIfTableIsEmpty(contacts))
            {
                AnsiConsole.MarkupLine("[bold red]There's no contact to edit.[/]");
                return;
            }

            _viewPhoneBook.ViewContacts(type);
            var contactId = PromptForContactId(contacts);
            var column = PromptForColumnToEdit();
            var newValue = PromptForNewValue(column);

            _phoneBookService.EditContact(contactId, column, newValue);
        
            var editAnother = AnsiConsole.Confirm("Do you want to edit another contact?");
            Console.Clear();
            if (!editAnother)
                return;
        }
    }

    private int PromptForContactId(List<Contact> contacts)
    {
        var index = AnsiConsole.Prompt(
            new TextPrompt<int>("Select the contact Id to edit: ").Validate(i =>
                i > 0 && i <= contacts.Count
                    ? ValidationResult.Success()
                    : ValidationResult.Error("Invalid contact number.")
            )
        );

        return contacts[index - 1].Id;
    }

    private string PromptForColumnToEdit()
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

    private string PromptForNewValue(string column)
    {
        while (true)
        {
            var input = AnsiConsole.Ask<string>($"Provide new value for {column}: ");

            if (column == "Email Address")
            {
                var result = _phoneBookService.ValidateEmail(input);
                if (result.Successful) return input;
                AnsiConsole.MarkupLine($"[red]{result.Message}[/]");
            }
            else if (column == "Phone Number")
            {
                var result = _phoneBookService.ValidatePhone(input);
                if (result.Successful) return input;
                AnsiConsole.MarkupLine($"[red]{result.Message}[/]");
            }
            else
            {
                return input;
            }
        }
    }
    private void ViewContacts(ContactCategoryMenuType type)
    {
        _viewPhoneBook.ViewContacts(type);
        AnsiConsole.MarkupLine("Press any key to continue...");
        Console.ReadKey();
    }
    
    private bool ValidateIfTableIsEmpty(List<Contact> contacts)
    {
        return contacts.Any();
    }
}
