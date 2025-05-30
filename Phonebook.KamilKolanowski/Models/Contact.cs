using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace Phonebook.KamilKolanowski.Models;

public class Contact
{
    [Key]
    public int Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }
    public string Phone { get; set; }
    public string Category { get; set; }
}
