using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BlindMatchPAS_Final.Data;
using BlindMatchPAS_Final.Models;
using System.Security.Claims;

namespace BlindMatchPAS_Final.Controllers
{
    [Authorize(Roles = "Supervisor")]
    public class SupervisorController : Controller
    {
        private readonly ApplicationDbContext _context;

        public SupervisorController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index(string search, string status, string area)
        {
            var query = _context.Projects.AsQueryable();

            if (!string.IsNullOrEmpty(search))
                query = query.Where(p => p.Title.Contains(search));

            if (!string.IsNullOrEmpty(status))
                query = query.Where(p => p.Status == status);

            if (!string.IsNullOrEmpty(area))
                query = query.Where(p => p.ResearchArea == area);

            return View(await query.ToListAsync());
        }

        //INTERESTED
        public async Task<IActionResult> Interested(int id)
        {
            var project = await _context.Projects.FindAsync(id);

            project.Status = "UnderReview";

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        //REVIEW
        public async Task<IActionResult> Review(int id)
        {
            var project = await _context.Projects.FindAsync(id);
            return View(project);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Review(int id, Project model)
        {
            var project = await _context.Projects.FindAsync(id);

            project.Feedback = model.Feedback;
            project.Score = model.Score;
            project.Status = "Reviewed";

            //NOTIFICATION
            _context.Notifications.Add(new Notification
            {
                UserEmail = project.StudentEmail,
                Message = $"Your project '{project.Title}' has been reviewed."
            });

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        //REJECT
        public async Task<IActionResult> Reject(int id)
        {
            var project = await _context.Projects.FindAsync(id);
            return View(project);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Reject(int id, string reason)
        {
            var project = await _context.Projects.FindAsync(id);

            project.Status = "Rejected";
            project.Feedback = reason;

            //NOTIFICATION
            _context.Notifications.Add(new Notification
            {
                UserEmail = project.StudentEmail,
                Message = $"Your project '{project.Title}' was rejected."
            });

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        //MATCH
        public async Task<IActionResult> ConfirmMatch(int id)
        {
            var project = await _context.Projects.FindAsync(id);

            project.Status = "Matched";
            project.IsMatched = true;
            project.SupervisorEmail = User.Identity.Name;

            //NOTIFICATION
            _context.Notifications.Add(new Notification
            {
                UserEmail = project.StudentEmail,
                Message = $"Your project '{project.Title}' has been matched with a supervisor."
            });

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
    }
}