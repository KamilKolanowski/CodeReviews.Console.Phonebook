namespace Phonebook.KamilKolanowski.Models;

internal class PhoneBookMenu
{
    internal enum PhoneBookMenuType
    {
        AddContact,
        DeleteContact,
        EditContact,
        ShowContact,
        ShowAllContacts,
        GoBack
    }

    internal static Dictionary<PhoneBookMenuType, string> Menu { get; } =
        new()
        {
            { PhoneBookMenuType.AddContact, "Add Contact" },
            { PhoneBookMenuType.DeleteContact, "Delete Contact" },
            { PhoneBookMenuType.EditContact, "Edit Contact" },
            { PhoneBookMenuType.ShowContact, "Show Contact(s)" },
            { PhoneBookMenuType.ShowAllContacts, "Show All Contacts" },
            { PhoneBookMenuType.GoBack, "Back" },
        };

    internal enum ContactCategoryMenuType
    {
        Family,
        Friends,
        Work,
        Other,
    }

    internal enum MainMenu
    {
        ManageContacts,
        SendMessages,
        Exit,
    }

    internal static Dictionary<MainMenu, string> MainMenuOptions { get; } =
        new()
        {
            { MainMenu.ManageContacts, "Manage Contacts" },
            { MainMenu.SendMessages, "Send Message" },
            { MainMenu.Exit, "Exit" },
        };
}
