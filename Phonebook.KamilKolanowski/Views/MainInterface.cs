using Phonebook.KamilKolanowski.Controllers;
using Phonebook.KamilKolanowski.Services;
using Spectre.Console;
using static Phonebook.KamilKolanowski.Models.PhoneBookMenu;

namespace Phonebook.KamilKolanowski.Views;

internal class MainInterface
{
    private readonly PhoneBookController _phoneBookController = new();
    private readonly MessageController _messageController = new();

    internal void Run()
    {
        while (true)
        {
            var selectMenuOption = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title("Choose option from the menu")
                    .AddChoices(MainMenuOptions.Values)
            );

            var selectedMenuOption = MainMenuOptions
                .FirstOrDefault(v => v.Value == selectMenuOption)
                .Key;

            switch (selectedMenuOption)
            {
                case MainMenu.ManageContacts:
                    _phoneBookController.ContactsOperation();
                    break;
                case MainMenu.SendMessages:
                    _messageController.SendMessage();
                    break;
                case MainMenu.Exit:
                    Environment.Exit(0);
                    break;
            }
        }
    }
}
