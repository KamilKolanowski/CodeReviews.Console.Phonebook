using Phonebook.KamilKolanowski.Helpers;
using Phonebook.KamilKolanowski.Services;
using Phonebook.KamilKolanowski.Views;
using Spectre.Console;

namespace Phonebook.KamilKolanowski.Controllers;

internal class MailController
{
    private readonly ViewPhoneBook _viewPhoneBook = new();
    private readonly PromptHelper _promptHelper = new();
    private readonly PhoneBookService _phoneBookService = new();
    private readonly MailService _mailService = new();

    internal void MailOperation()
    {
        var contacts = _phoneBookService.GetAllContacts();
        _viewPhoneBook.ViewAllContacts();
        
        AnsiConsole.MarkupLine("You're sending an email.");
        var contactId = _promptHelper.PromptForContactId(contacts);
        
        var contactMail = contacts.FirstOrDefault(m => m.Id == contactId);
        _mailService.SendMail(contactMail.Email);
    }
}