using MailKit.Net.Smtp;
using Microsoft.Extensions.Configuration;
using MimeKit;
using Phonebook.KamilKolanowski.Models;
using Spectre.Console;

namespace Phonebook.KamilKolanowski.Services;

internal class MailService
{
    internal void SendMail(string recipient)
    {
        var smtp = CreateSmtpSettings();

        var message = CreateMail(smtp.Username, recipient);

        using (var client = new SmtpClient())
        {
            client.Connect(smtp.Host, smtp.Port, smtp.UseSsl);
            client.Authenticate(smtp.Username, smtp.Password);
            client.Send(message);
            client.Disconnect(true);
        }

        AnsiConsole.MarkupLine("[green]Message Sent![/]");
        AnsiConsole.MarkupLine("Press any key to continue...");
        Console.ReadKey();
        Console.Clear();
    }

    private SmtpSettings CreateSmtpSettings()
    {
        var configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json")
            .Build();

        var smtpSettings = configuration.GetSection("Smtp").Get<SmtpSettings>();
        return smtpSettings;
    }

    private MimeMessage CreateMail(string sender, string recipient)
    {
        var message = new MimeMessage();
        var newMail = CreateMessage();
        message.From.Add(new MailboxAddress("PhonebookApp", sender));
        message.To.Add(new MailboxAddress("Receiver", recipient));
        message.Subject = newMail.Subject;
        message.Body = newMail.Body;

        return message;
    }

    private Mail CreateMessage()
    {
        var subject = AnsiConsole.Ask<string>("Specify title: ");
        var body = new TextPart("plain") { Text = AnsiConsole.Ask<string>("Write your message: ") };

        var mail = new Mail { Subject = subject, Body = body };

        return mail;
    }
}
