using ASPLiteBlog.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;

namespace ASPLiteBlog.Areas.Identity.Pages.Account.Manage
{
    public class RolesStorage
    {
        public bool admin { get; set; }
        public bool writer { get; set; }

        public RolesStorage()
        {
            admin = false;
            writer = false;
        }
    }

    public class ManageUsersModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        const string adminRoleName = "admin";
        const string writerRoleName = "writer";

        public ManageUsersModel(ApplicationDbContext context, UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _context = context;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public class InputModel
        {
            public Dictionary<string, RolesStorage> roles { get; set; }

            public IList<ApplicationUser> users { get; set; }
        }

        /// <summary>
        /// Reloads data into Input otherwise we get nulls.
        /// There should be a better way, but this works.
        /// </summary>
        void SetupModel()
        {
            Input = new InputModel();

            Input.roles = new Dictionary<string, RolesStorage>();

            Input.users = _context.Users.ToList();

            foreach (var user in Input.users)
            {
                Input.roles[user.Id] = new RolesStorage();

                var rolesUser = _userManager.GetRolesAsync(user).GetAwaiter().GetResult();

                foreach (var role in rolesUser)
                {
                    switch (role)
                    {
                        case "admin":
                            Input.roles[user.Id].admin = true;
                            break;
                        case "writer":
                            Input.roles[user.Id].writer = true;
                            break;
                        default:
                            break;
                    }
                }
            }
        }

        public void OnGet()
        {
            SetupModel();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            foreach (KeyValuePair<string, RolesStorage> entry in Input.roles)
            {
                var user = await _userManager.FindByIdAsync(entry.Key);

                if (user == null)
                {
                    continue;
                }

                if (entry.Value.admin)
                {
                    await _userManager.AddToRoleAsync(user, adminRoleName);
                }
                else
                {
                    await _userManager.RemoveFromRoleAsync(user, adminRoleName);
                }

                if (entry.Value.writer)
                {
                    await _userManager.AddToRoleAsync(user, writerRoleName);
                }
                else
                {
                    await _userManager.RemoveFromRoleAsync(user, writerRoleName);
                }
            }

            SetupModel();

            return Page();
        }

        public async Task<PageResult> OnPostBAN(string userID)
        {
            //get user
            var user = await _userManager.FindByIdAsync(userID);

            //use built int method to ban them
            user.LockoutEnabled = true;
            user.LockoutEnd = DateTime.UtcNow.AddYears(2000);

            //updated
            await _userManager.UpdateAsync(user);

            //reload data
            SetupModel();

            return Page();
        }

        public async Task<PageResult> OnPostUNBAN(string userID)
        {
            var user = await _userManager.FindByIdAsync(userID);

            user.LockoutEnabled = false;
            //instead of having subtract methods we just use this
            //https://stackoverflow.com/questions/2359029/calculating-past-datetime-in-c-sharp
            user.LockoutEnd = DateTime.UtcNow.AddHours(-12);

            await _userManager.UpdateAsync(user);

            SetupModel();

            return Page();
        }
    }
}
