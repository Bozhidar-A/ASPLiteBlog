using ASPLiteBlog.Data;
using ASPLiteBlog.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace ASPLiteBlog.Areas.Identity.Pages.Account.Manage
{
    public class DraftsModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public DraftsModel(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public IList<BlogPost> drafts { get; set; }

        [Authorize(Roles = "admin,writer")]
        public void OnGet()
        {
            var user = _userManager.FindByNameAsync(User.Identity.Name).GetAwaiter().GetResult();
            drafts = _context.BlogPost.Include(m => m.user).Where(x => x.draft && x.userID == user.Id).ToArray();
        }
    }
}
