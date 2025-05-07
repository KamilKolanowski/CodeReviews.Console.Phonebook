using Phonebook.KamilKolanowski.Controllers;
using Phonebook.KamilKolanowski.Services;
using Phonebook.KamilKolanowski.Views;

namespace Phonebook.KamilKolanowski;

class Program
{
    static void Main(string[] args)
    {
        MainInterface mainInterface = new();
        mainInterface.Run();
    }
}
