using Phonebook.KamilKolanowski.Services;
using Spectre.Console;

namespace Phonebook.KamilKolanowski.Views;

internal class ViewPhoneBook
{
    private readonly PhoneBookService _phoneBookService = new();

    internal void ViewContacts()
    {
        var contacts = _phoneBookService.GetContacts();
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
}
