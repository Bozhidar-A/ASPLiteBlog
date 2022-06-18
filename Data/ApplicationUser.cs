using Microsoft.AspNetCore.Identity;

namespace ASPLiteBlog.Data
{
    public class ApplicationUser : IdentityUser
    {
        [PersonalData]
        public string firstName { get; set; }
        [PersonalData]
        public string lastName { get; set; }
    }
}
