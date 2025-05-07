using Microsoft.Extensions.Configuration;
using Spectre.Console;
using Twilio;
using Twilio.Rest.Api.V2010.Account;
using Twilio.Types;

namespace Phonebook.KamilKolanowski.Services;

internal class SmsService
{
    internal void SendSms(string phoneNumber)
    {
        var builder = GetConfigurationBuilder().Build();

        var accountSid = builder["SmsService:Sid"];
        var authToken = builder["SmsService:Token"];
        TwilioClient.Init(accountSid, authToken);

        try
        {
            var message = MessageResource.Create(
                body: CreateMessage(),
                from: new PhoneNumber(builder["SmsService:VirtualNumber"]),
                to: new PhoneNumber(phoneNumber)
            );

            AnsiConsole.MarkupLine($"[yellow]Message to {message.To} has been {message.Status}[/]");
        }
        catch (Exception ex)
        {
            AnsiConsole.MarkupLine(
                $"[yellow]Error: You can only send message to verified numbers in Twilio App : {ex.Message}[/]"
            );
        }

        AnsiConsole.MarkupLine("Press any key to continue...");
        Console.ReadKey();
        Console.Clear();
    }

    private IConfigurationBuilder GetConfigurationBuilder()
    {
        return new ConfigurationBuilder().AddJsonFile("appsettings.json", false, true);
    }

    private string CreateMessage()
    {
        var body = AnsiConsole.Ask<string>("Write your message: ");
        return body;
    }
}
