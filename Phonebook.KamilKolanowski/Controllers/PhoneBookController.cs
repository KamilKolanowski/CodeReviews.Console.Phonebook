using Phonebook.KamilKolanowski.Helpers;
using Phonebook.KamilKolanowski.Models;
using Phonebook.KamilKolanowski.Services;
using Phonebook.KamilKolanowski.Views;
using Spectre.Console;
using static Phonebook.KamilKolanowski.Models.PhoneBookMenu;

namespace Phonebook.KamilKolanowski.Controllers;

internal class PhoneBookController
{
    private readonly PhoneBookService _phoneBookService = new();
    private readonly ViewPhoneBook _viewPhoneBook = new();
    private readonly PromptHelper _promptHelper = new();

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

            var selectedOperation = 
                Menu.FirstOrDefault(v => v.Value == selectOperation).Key;

            switch (selectedOperation)
            {
                case PhoneBookMenuType.AddContact:
                case PhoneBookMenuType.DeleteContact:
                case PhoneBookMenuType.EditContact:
                case PhoneBookMenuType.ShowContact:
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
                        
                    }
                    break;

                case PhoneBookMenuType.ShowAllContacts:
                    ViewAllContacts();
                    break;
                case PhoneBookMenuType.GoBack:
                    return;
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

            var contactId = _promptHelper.PromptForContactId(contacts);
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
                AnsiConsole.MarkupLine("Press any key to continue...");
                Console.ReadKey();
                return;
            }

            _viewPhoneBook.ViewContacts(type);
            var contactId = _promptHelper.PromptForContactId(contacts);
            var column = _promptHelper.PromptForColumnToEdit();
            var newValue = _promptHelper.PromptForNewValue(column);

            _phoneBookService.EditContact(contactId, column, newValue);
        
            var editAnother = AnsiConsole.Confirm("Do you want to edit another contact?");
            Console.Clear();
            if (!editAnother)
                return;
        }
    }
    
    private void ViewContacts(ContactCategoryMenuType type)
    {
        _viewPhoneBook.ViewContacts(type);
        AnsiConsole.MarkupLine("Press any key to continue...");
        Console.ReadKey();
    }

    private void ViewAllContacts()
    {
        _viewPhoneBook.ViewAllContacts();
        AnsiConsole.MarkupLine("Press any key to continue...");
        Console.ReadKey();
    }
    
    private bool ValidateIfTableIsEmpty(List<Contact> contacts)
    {
        return contacts.Any();
    }
}
