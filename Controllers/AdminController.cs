using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BlindMatchPAS_Final.Data;
using BlindMatchPAS_Final.Models;

namespace BlindMatchPAS_Final.Controllers
{
    [Authorize(Roles = "Admin")] 
    public class AdminController : Controller
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly ApplicationDbContext _context;

        public AdminController(UserManager<IdentityUser> userManager,
                               ApplicationDbContext context)
        {
            _userManager = userManager;
            _context = context;
        }

        // ADMIN DASHBOARD
        public async Task<IActionResult> Index()
        {
            
            ViewBag.TotalUsers = _userManager.Users.Count();
            ViewBag.TotalProjects = await _context.Projects.CountAsync();
            ViewBag.MatchedProjects = await _context.Projects.CountAsync(p => p.Status == "Matched");
            ViewBag.PendingProjects = await _context.Projects.CountAsync(p => p.Status == "Pending");

            return View(await _userManager.Users.ToListAsync());
        }

        
        public async Task<IActionResult> Users()
        {
            var users = await _userManager.Users.ToListAsync();
            return View(users);
        }

        
        public async Task<IActionResult> MakeSupervisor(string id)
        {
            var user = await _userManager.FindByIdAsync(id);

            if (user != null)
            {
                await _userManager.RemoveFromRoleAsync(user, "Student");
                await _userManager.AddToRoleAsync(user, "Supervisor");

                TempData["Success"] = "User promoted to Supervisor!";
            }

            return RedirectToAction(nameof(Users));
        }

        //MAKE STUDENT
        public async Task<IActionResult> MakeStudent(string id)
        {
            var user = await _userManager.FindByIdAsync(id);

            if (user != null)
            {
                await _userManager.RemoveFromRoleAsync(user, "Supervisor");
                await _userManager.AddToRoleAsync(user, "Student");

                TempData["Success"] = "User changed to Student!";
            }

            return RedirectToAction(nameof(Users));
        }

        //DELETE USER
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteUser(string id)
        {
            var user = await _userManager.FindByIdAsync(id);

            if (user != null)
            {
                
                if (user.Email == User.Identity.Name)
                {
                    TempData["Error"] = "You cannot delete your own account!";
                    return RedirectToAction(nameof(Users));
                }

                var result = await _userManager.DeleteAsync(user);

                if (result.Succeeded)
                {
                    TempData["Success"] = "User deleted successfully!";
                }
                else
                {
                    TempData["Error"] = "Failed to delete user!";
                }
            }

            return RedirectToAction(nameof(Users));
        }

        
        public async Task<IActionResult> Projects()
        {
            var projects = await _context.Projects.ToListAsync();
            return View(projects);
        }

        //RESEARCH AREAS
        public async Task<IActionResult> Areas()
        {
            return View(await _context.ResearchAreas.ToListAsync());
        }

        
        public IActionResult CreateArea()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateArea(ResearchArea area)
        {
            if (ModelState.IsValid)
            {
                _context.ResearchAreas.Add(area);
                await _context.SaveChangesAsync();

                TempData["Success"] = "Research area added!";
                return RedirectToAction(nameof(Areas));
            }

            return View(area);
        }

        //DELETE AREA
        public async Task<IActionResult> DeleteArea(int id)
        {
            var area = await _context.ResearchAreas.FindAsync(id);

            if (area != null)
            {
                _context.ResearchAreas.Remove(area);
                await _context.SaveChangesAsync();

                TempData["Success"] = "Research area deleted!";
            }

            return RedirectToAction(nameof(Areas));
        }
    }
}