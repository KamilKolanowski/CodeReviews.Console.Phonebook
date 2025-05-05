namespace Phonebook.KamilKolanowski.Models;

internal class Mail
{
    public string Recipient { get; set; }
    public string Subject { get; set; }
    public MimeKit.TextPart Body { get; set; }
}