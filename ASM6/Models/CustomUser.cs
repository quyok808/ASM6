using Microsoft.AspNetCore.Identity;

namespace ASM6.Models
{
    public class CustomUser : IdentityUser
    {
        public string? FullName { get; set; }
        public string? Address { get; set; }
    }
}
