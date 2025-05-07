using Phonebook.KamilKolanowski.Helpers;
using Phonebook.KamilKolanowski.Models;
using Phonebook.KamilKolanowski.Services;
using Phonebook.KamilKolanowski.Views;
using Spectre.Console;

namespace Phonebook.KamilKolanowski.Controllers;

internal class MessageController
{
    private readonly ViewPhoneBook _viewPhoneBook = new();
    private readonly PromptHelper _promptHelper = new();
    private readonly PhoneBookService _phoneBookService = new();
    private readonly MailService _mailService = new();
    private readonly SmsService _smsService = new();

    internal void SendMessage()
    {
        var messageType = AnsiConsole.Prompt(
            new SelectionPrompt<MessageType.Message>()
                .Title("What do you want to send?")
                .AddChoices(Enum.GetValues<MessageType.Message>())
        );

        switch (messageType)
        {
            case MessageType.Message.Mail:
                MailOperation();
                break;
            case MessageType.Message.Sms:
                SmsOperation();
                break;
        }
    }

    private void MailOperation()
    {
        var contacts = _phoneBookService.GetAllContacts();
        _viewPhoneBook.ViewAllContacts();

        AnsiConsole.MarkupLine("You're sending an email.");
        var contactId = _promptHelper.PromptForContactId(contacts);

        var contactMail = contacts.FirstOrDefault(m => m.Id == contactId);
        _mailService.SendMail(contactMail.Email);
    }

    private void SmsOperation()
    {
        var contacts = _phoneBookService.GetAllContacts();
        _viewPhoneBook.ViewAllContacts();

        AnsiConsole.MarkupLine("You're sending an SMS.");
        var contactId = _promptHelper.PromptForContactId(contacts);
        var contactSms = contacts.FirstOrDefault(m => m.Id == contactId);

        _smsService.SendSms(contactSms.Phone.Replace("+", "+48"));
    }
}
