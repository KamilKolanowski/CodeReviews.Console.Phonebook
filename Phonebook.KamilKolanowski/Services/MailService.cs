using Phonebook.KamilKolanowski.Models;
using Spectre.Console;
using MailKit.Net.Smtp;
using MimeKit;
using MailKit;
namespace Phonebook.KamilKolanowski.Services;

internal class MailService
{
    internal void SendMail()
    {
        using (var client = new SmtpClient ()) {
            client.Connect ("smtp.gmail.com", 587, false);
            client.Authenticate ("", "");
            client.Send (CreateMail());
            client.Disconnect (true);
        }
    }
    
    private MimeMessage CreateMail()
    {
        var message = new MimeMessage ();
        var newMail = CreateMessage();
        message.From.Add (new MailboxAddress ("Test", "@gmail.com"));
        message.To.Add (new MailboxAddress ("Test", newMail.Recipient));
        message.Subject = newMail.Subject;
        message.Body = newMail.Body;
        
        return message;
    }
    
    private Mail CreateMessage()
    {
        var recipient = AnsiConsole.Ask<string>("Specify recipient: ");
        var subject = AnsiConsole.Ask<string>("Specify title: ");
        var body = new TextPart ("plain") {
            Text = AnsiConsole.Ask<string>("Write your message: ")};
    
        var mail = new Mail
        {
            Recipient = recipient,
            Subject = subject,
            Body = body
        };
        
        return mail;
    }
}

