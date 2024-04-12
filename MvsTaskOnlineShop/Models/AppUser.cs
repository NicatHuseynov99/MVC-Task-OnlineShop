using Microsoft.AspNetCore.Identity;

namespace MvsTaskOnlineShop.Models
{
    public class AppUser : IdentityUser
    {
        public string FullName { get; set; }
    }
}
