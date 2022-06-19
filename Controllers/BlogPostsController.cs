using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ASPLiteBlog.Data;
using ASPLiteBlog.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;

namespace ASPLiteBlog.Controllers
{
    public class BlogPostsController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ApplicationDbContext _context;

        public BlogPostsController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: BlogPosts
        [Route("")]
        [Route("Home")]
        [Route("Home/Index")]
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = await _context.BlogPost.Include(b => b.user).ToListAsync();
            return View(applicationDbContext);
        }

        // GET: BlogPosts/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.BlogPost == null)
            {
                return NotFound();
            }

            var blogPost = await _context.BlogPost
                .Include(b => b.user)
                .FirstOrDefaultAsync(m => m.ID == id);
            if (blogPost == null)
            {
                return NotFound();
            }

            return View(blogPost);
        }

        // GET: BlogPosts/Create
        public IActionResult Create()
        {
            //ViewData["userID"] = new SelectList(_context.Users, "Id", "Id");
            return View();
        }

        // POST: BlogPosts/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "admin,writer")]
        public async Task<IActionResult> Create([Bind("ID,title,body,draft")] BlogPost blogPost, IFormFile formFile)
        {
            //var cultureInvariant = CultureInfo.InvariantCulture;

            blogPost.userID = _userManager.GetUserId(User);
            //use this to get the userID and link the user to the model

            ModelState.Remove("user");
            ModelState.Remove("userID"); 
            ModelState.Remove("previewPicLocation");
            //ModelState validation complains when user or userID are not passed into the function, we do this here
            //these are not errors, delete and ignore
            if (ModelState.IsValid)
            {
                if (!blogPost.draft)
                {
                    blogPost.timePublished = DateTime.UtcNow;
                }

                blogPost.previewPicLocation = UploadFile(formFile);
                _context.Add(blogPost);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            //ViewData["userID"] = new SelectList(_context.Users, "Id", "Id", blogPost.userID);
            return View(blogPost);
        }

        [Authorize(Roles = "admin,writer")]
        public IActionResult UploadFileTinyMCE()
        {
            IFormFile ufile = Request.Form.Files[0];
            if (ufile != null && ufile.Length > 0)
            {
                //makes sure the file has a safe name to store
                var fileName = Guid.NewGuid().ToString();
                fileName += Path.GetExtension(ufile.FileName);

                //gets file path to wwwroot media 
                //NEEDS app.UseStaticFiles() in Startup.cs to work
                var filePath = Path.Combine(Directory.GetCurrentDirectory(), @"wwwroot/media");

                //create if not there
                if (!Directory.Exists(filePath))
                {
                    Directory.CreateDirectory(filePath);
                }

                //combine both
                filePath = Path.Combine(filePath, fileName);

                //write to folder
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    ufile.CopyTo(fileStream);
                }

                //wwwroot is used for static content like this
                //return json that tinymce can understand and load
                return Json(new { location = "/media/" + fileName });
            }
            return Json(new { location = "Not Found" });
        }
        /*
         * The TinyMCE NuGet packages are broken since an update to the store?
         * By calling it in js (the official way) and giving a public function
         * that returns Json is the way I got it working
         * I was told to use HttpPostFile or something like that as an argument,
         * that was depricated
         */

        [HttpPost]
        [Authorize(Roles = "admin,writer")]
        public string UploadFile(IFormFile ufile)
        {
            //makes sure the file has a safe name to store
            var fileName = Guid.NewGuid().ToString();
            fileName += Path.GetExtension(ufile.FileName);

            //gets file path to wwwroot media 
            //NEEDS app.UseStaticFiles() in Startup.cs to work
            var filePath = Path.Combine(Directory.GetCurrentDirectory(), @"wwwroot/media");

            //create if not there
            if (!Directory.Exists(filePath))
            {
                Directory.CreateDirectory(filePath);
            }

            //combine both
            filePath = Path.Combine(filePath, fileName);

            //write to folder
            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                ufile.CopyTo(fileStream);
            }

            //wwwroot is used for static content like this
            return "media/" + fileName; 
        }

        // GET: BlogPosts/Edit/5
        [Authorize(Roles = "admin,writer")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.BlogPost == null)
            {
                return NotFound();
            }

            var blogPost = await _context.BlogPost.FindAsync(id);
            if (blogPost == null)
            {
                return NotFound();
            }
            //ViewData["userID"] = new SelectList(_context.Users, "Id", "Id", blogPost.userID);
            return View(blogPost);
        }

        // POST: BlogPosts/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "admin,writer")]
        public async Task<IActionResult> Edit(int id, [Bind("ID," +
            "title," +
            "body," +
            "draft," +
            "timeCreated," +
            "timeLastEdited," +
            "timePublished," +
            "timeLastEditedPublished," +
            "userID")] BlogPost blogPost, IFormFile? formFile, string oldPreviewPicLoc)
        {
            if (id != blogPost.ID)
            {
                return NotFound();
            }

            ModelState.Remove("user");
            ModelState.Remove("userID");
            ModelState.Remove("previewPicLocation");
            if (ModelState.IsValid)
            {
                try
                {
                    if (blogPost.draft)
                    {
                        blogPost.timeLastEdited = DateTime.UtcNow;
                    }
                    else
                    {
                        if(blogPost.timePublished == null)
                        {
                            blogPost.timePublished = DateTime.UtcNow;
                        }
                        else
                        {
                            blogPost.timeLastEditedPublished = DateTime.UtcNow;
                        }
                    }

                    if (formFile != null)
                    {
                        var filePath = Path.Combine(Directory.GetCurrentDirectory(), @"wwwroot", oldPreviewPicLoc);

                        //this check shouldn't be necessary 
                        if (System.IO.File.Exists(filePath))
                        {
                            System.IO.File.Delete(filePath);
                        }

                        blogPost.previewPicLocation = UploadFile(formFile);
                    }
                    else
                    {
                        blogPost.previewPicLocation = oldPreviewPicLoc;
                    }

                    _context.Update(blogPost);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BlogPostExists(blogPost.ID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            //ViewData["userID"] = new SelectList(_context.Users, "Id", "Id", blogPost.userID);
            return View(blogPost);
        }

        // GET: BlogPosts/Delete/5
        [Authorize(Roles = "admin,writer")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.BlogPost == null)
            {
                return NotFound();
            }

            var blogPost = await _context.BlogPost
                .Include(b => b.user)
                .FirstOrDefaultAsync(m => m.ID == id);
            if (blogPost == null)
            {
                return NotFound();
            }

            return View(blogPost);
        }

        // POST: BlogPosts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "admin,writer")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.BlogPost == null)
            {
                return Problem("Entity set 'ApplicationDbContext.BlogPost'  is null.");
            }
            var blogPost = await _context.BlogPost.FindAsync(id);
            if (blogPost != null)
            {
                _context.BlogPost.Remove(blogPost);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool BlogPostExists(int id)
        {
          return (_context.BlogPost?.Any(e => e.ID == id)).GetValueOrDefault();
        }
    }
}
