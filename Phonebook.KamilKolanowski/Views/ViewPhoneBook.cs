using Phonebook.KamilKolanowski.Models;
using Phonebook.KamilKolanowski.Services;
using Spectre.Console;

namespace Phonebook.KamilKolanowski.Views;

internal class ViewPhoneBook
{
    private readonly PhoneBookService _phoneBookService = new();

    internal void ViewContacts(PhoneBookMenu.ContactCategoryMenuType type)
    {
        var contacts = _phoneBookService.GetContacts(type);
        var contactsTable = new Table();

        contactsTable.Title("[yellow]Phone Book[/]");
        contactsTable.Border = TableBorder.MinimalHeavyHead;

        contactsTable.AddColumn("[red]Id[/]");
        contactsTable.AddColumn("[red]First Name[/]");
        contactsTable.AddColumn("[red]Last Name[/]");
        contactsTable.AddColumn("[red]Email Address[/]");
        contactsTable.AddColumn("[red]Phone Number[/]");

        var idx = 1;

        foreach (var contact in contacts)
        {
            contactsTable.AddRow(
                $"{idx}",
                $"{contact.FirstName}",
                $"{contact.LastName}",
                $"{contact.Email}",
                $"{contact.Phone}"
            );

            idx++;
        }

        AnsiConsole.Write(contactsTable);
    }

    internal void ViewAllContacts()
    {
        var contacts = _phoneBookService.GetAllContacts();
        var contactsTable = new Table();

        contactsTable.Title("[yellow]Phone Book[/]");
        contactsTable.Border = TableBorder.MinimalHeavyHead;

        contactsTable.AddColumn("[red]Id[/]");
        contactsTable.AddColumn("[red]First Name[/]");
        contactsTable.AddColumn("[red]Last Name[/]");
        contactsTable.AddColumn("[red]Email Address[/]");
        contactsTable.AddColumn("[red]Phone Number[/]");
        contactsTable.AddColumn("[red]Category[/]");

        var idx = 1;

        foreach (var contact in contacts)
        {
            contactsTable.AddRow(
                $"{idx}",
                $"{contact.FirstName}",
                $"{contact.LastName}",
                $"{contact.Email}",
                $"{contact.Phone}",
                $"{contact.Category}"
            );

            idx++;
        }

        AnsiConsole.Write(contactsTable);
    }
}
