using Microsoft.AspNetCore.Identity;

namespace CarRenting.Data.Models
{
    public class User : IdentityUser
    {
        public string FullName { get; set; }
    }
}
