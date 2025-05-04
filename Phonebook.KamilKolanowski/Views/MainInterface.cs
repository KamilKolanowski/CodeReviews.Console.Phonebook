using Phonebook.KamilKolanowski.Controllers;
using static Phonebook.KamilKolanowski.Models.PhoneBookMenu;
using Spectre.Console;

namespace Phonebook.KamilKolanowski.Views;

internal class MainInterface
{
    private PhoneBookController _phoneBookController = new();
    
    internal void Run()
    {
        var selectMenuOption = AnsiConsole.Prompt(
            new SelectionPrompt<string>()
                .Title("Choose option from the menu")
                .AddChoices(MainMenuOptions.Values));
        
        var selectedMenuOption = MainMenuOptions.FirstOrDefault(v => v.Value == selectMenuOption).Key;

        switch (selectedMenuOption)
        {
            case MainMenu.ManageContacts:
                _phoneBookController.ContactsOperation();
                break;
            case MainMenu.SendMessages:
                Console.WriteLine("sending messages");
                break;
            case MainMenu.Exit:
                Environment.Exit(0);
                break;
        }
    }
}