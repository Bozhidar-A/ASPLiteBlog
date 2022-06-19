using ASPLiteBlog.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ASPLiteBlog.Areas.Identity.Pages.Account.Manage
{
    public class PrivacyModel : PageModel
    {
        private readonly UserManager<ApplicationUser> _userManager;
        //private readonly ILogger<PersonalDataModel> _logger;

        public PrivacyModel(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPostAsync(string displayChoice)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            if(displayChoice == "username")
            {
                user.displayUsername = true;
            }
            else
            {
                user.displayUsername = false;
            }

            await _userManager.UpdateAsync(user);

            return Page();
        }
    }
}
