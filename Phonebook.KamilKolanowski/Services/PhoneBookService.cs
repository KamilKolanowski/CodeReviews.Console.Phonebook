using System.Net.Mail;
using System.Text.RegularExpressions;
using Phonebook.KamilKolanowski.Models;
using Spectre.Console;

namespace Phonebook.KamilKolanowski.Services;

internal class PhoneBookService
{
    internal void AddContact(PhoneBookMenu.ContactCategoryMenuType type)
    {
        using (var context = new AppDb.AppDbContext())
        {
            var newContact = CreateNewContact(type);
            context.Contacts.Add(newContact);
            context.SaveChanges();
        }

        ReturnStatusMessage("added");
    }

    internal void DeleteContact(int id)
    {
        using (var context = new AppDb.AppDbContext())
        {
            var contact = context.Contacts.Find(id);
            context.Contacts.Remove(contact);
            context.SaveChanges();
        }

        ReturnStatusMessage("deleted");
    }

    internal void EditContact(int id, string columnToUpdate, string newValue)
    {
        using (var context = new AppDb.AppDbContext())
        {
            var contact = context.Contacts.Find(id);
            if (contact == null)
                return;

            switch (columnToUpdate)
            {
                case "First Name":
                    contact.FirstName = newValue;
                    break;
                case "Last Name":
                    contact.LastName = newValue;
                    break;
                case "Email Address":
                    contact.Email = newValue;
                    break;
                case "Phone Number":
                    contact.Phone = newValue;
                    break;
            }
            context.SaveChanges();
        }
        ReturnStatusMessage("updated");
    }

    internal List<Contact> GetContacts(PhoneBookMenu.ContactCategoryMenuType category)
    {
        using (var context = new AppDb.AppDbContext())
        {
            return context.Contacts.Where(c => c.Category == category.ToString()).ToList();
        }
    }

    internal List<Contact> GetAllContacts()
    {
        using (var context = new AppDb.AppDbContext())
        {
            return context.Contacts.ToList();
        }
    }

    internal ValidationResult ValidateEmail(string emailAddress)
    {
        emailAddress = emailAddress.Trim();
        try
        {
            var addr = new MailAddress(emailAddress);
            if (!addr.Host.Contains('.'))
                return ValidationResult.Error("[red]Email domain must contain a dot.[/]");
            if (!Regex.IsMatch(addr.Host, @"\.[a-zA-Z]{2,}$"))
                return ValidationResult.Error("[red]Email must have a valid domain suffix.[/]");
            return ValidationResult.Success();
        }
        catch
        {
            return ValidationResult.Error("[red]Provided Email is not valid, please try again.[/]");
        }
    }

    internal ValidationResult ValidatePhone(string phoneNumber)
    {
        phoneNumber = phoneNumber.Trim();
        try
        {
            if (!Regex.IsMatch(phoneNumber, @"^\+\d{9}$"))
            {
                return ValidationResult.Error(
                    "[red]Provided Phone Number is not valid, please try again.[/]"
                );
            }

            return ValidationResult.Success();
        }
        catch
        {
            return ValidationResult.Error(
                "[red]Provided Phone Number is not valid, please try again.[/]"
            );
        }
    }
    
    private Contact CreateNewContact(PhoneBookMenu.ContactCategoryMenuType category)
    {
        var firstName = AnsiConsole.Ask<string>("Enter first name: ");
        var lastName = AnsiConsole.Ask<string>("Enter last name: ");
        var email = AnsiConsole.Prompt(
            new TextPrompt<string>(
                "Enter email address [yellow](e.g. john.doe@gmail.com)[/]: "
            ).Validate(input => ValidateEmail(input))
        );
        var phone = AnsiConsole.Prompt(
            new TextPrompt<string>("Enter phone number [yellow](e.g. +123456789)[/]: ").Validate(
                input => ValidatePhone(input)
            )
        );

        return new Contact
        {
            FirstName = firstName,
            LastName = lastName,
            Email = email,
            Phone = phone,
            Category = category.ToString()
        };
    }

    private void ReturnStatusMessage(string operation)
    {
        AnsiConsole.MarkupLine($"[green]Successfully {operation} contact![/]");
        AnsiConsole.MarkupLine("Press any key to continue...");
        Console.ReadKey();
    }
}
