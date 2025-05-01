using Phonebook.KamilKolanowski.Controllers;

namespace Phonebook.KamilKolanowski;

class Program
{
    static void Main(string[] args)
    {
        PhoneBookController phoneBookController = new PhoneBookController();
        phoneBookController.ContactsOperation();
    }
}
