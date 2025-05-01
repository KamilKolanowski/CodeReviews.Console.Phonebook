using System.Net.Mail;
using System.Text.RegularExpressions;
using Microsoft.EntityFrameworkCore;
using Phonebook.KamilKolanowski.Models;
using Spectre.Console;

namespace Phonebook.KamilKolanowski.Services;

internal class PhoneBookService
{
    internal class AppDbContext : DbContext
    {
        internal DbSet<Contact> Contacts { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(
                @"Server=localhost;
                                Database=PhoneBook;
                                User Id=sa;
                                Password=dockerStrongPwd123;
                                Encrypt=False;
                                TrustServerCertificate=True;"
            );
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasDefaultSchema("TCSA");
            base.OnModelCreating(modelBuilder);
        }
    }

    internal void AddContact()
    {
        using (var context = new AppDbContext())
        {
            var newContact = CreateNewContact();
            context.Contacts.Add(newContact);
            context.SaveChanges();
        }

        ReturnStatusMessage("added");
    }

    internal void DeleteContact(int id)
    {
        using (var context = new AppDbContext())
        {
            var contact = context.Contacts.Find(id);
            context.Contacts.Remove(contact);
            context.SaveChanges();
        }

        ReturnStatusMessage("deleted");
    }

    internal void EditContact(int id, string columnToUpdate, string newValue)
    {
        using (var context = new AppDbContext())
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

    internal List<Contact> GetContacts()
    {
        using (var context = new AppDbContext())
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
                return ValidationResult.Error("Email domain must contain a dot.");
            if (!Regex.IsMatch(addr.Host, @"\.[a-zA-Z]{2,}$"))
                return ValidationResult.Error("Email must have a valid domain suffix.");
            return ValidationResult.Success();
        }
        catch
        {
            return ValidationResult.Error("Provided Email is not valid, please try again.");
        }
    }

    private Contact CreateNewContact()
    {
        var firstName = AnsiConsole.Ask<string>("Enter first name: ");
        var lastName = AnsiConsole.Ask<string>("Enter last name: ");
        var email = AnsiConsole.Prompt(
            new TextPrompt<string>("Enter email address: ").Validate(input => ValidateEmail(input))
        );
        var phone = AnsiConsole.Ask<string>("Enter phone number: ");

        return new Contact
        {
            FirstName = firstName,
            LastName = lastName,
            Email = email,
            Phone = phone,
        };
    }

    private void ReturnStatusMessage(string operation)
    {
        AnsiConsole.MarkupLine($"[green]Successfully {operation} contact![/]");
        AnsiConsole.MarkupLine("Press any key to continue...");
        Console.ReadKey();
    }
}
