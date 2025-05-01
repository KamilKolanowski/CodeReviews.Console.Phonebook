namespace Phonebook.KamilKolanowski.Models;

internal class PhoneBookMenu
{
    internal enum PhoneBookMenuType
    {
        AddContact,
        DeleteContact,
        EditContact,
        ShowContact,
        Exit,
    }

    internal static Dictionary<PhoneBookMenuType, string> Menu { get; } =
        new()
        {
            { PhoneBookMenuType.AddContact, "Add Contact" },
            { PhoneBookMenuType.DeleteContact, "Delete Contact" },
            { PhoneBookMenuType.EditContact, "Edit Contact" },
            { PhoneBookMenuType.ShowContact, "Show Contact(s)" },
            { PhoneBookMenuType.Exit, "Exit" },
        };
}
