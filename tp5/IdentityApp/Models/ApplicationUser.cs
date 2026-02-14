using Microsoft.AspNetCore.Identity;

namespace IdentityApp.Models;

public class ApplicationUser : IdentityUser
{
    public string? City { get; set; }
}
