using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BlindMatchPAS_Final.Data;
using BlindMatchPAS_Final.Models;
using System.Security.Claims;

namespace BlindMatchPAS_Final.Controllers
{
    [Authorize(Roles = "Student")]
    public class ProjectsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ProjectsController(ApplicationDbContext context)
        {
            _context = context;
        }

       
        //DASHBOARD
        
        public async Task<IActionResult> Index()
        {
            var email = User.Identity.Name;

            var projects = await _context.Projects
                .Where(p => p.StudentEmail == email)
                .OrderByDescending(p => p.CreatedAt)
                .ToListAsync();

            return View(projects);
        }

        
        //CREATE
        
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Project project)
        {
            if (ModelState.IsValid)
            {
                project.StudentEmail = User.Identity.Name;
                project.StudentId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                project.Status = "Pending";

                _context.Add(project);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }

            return View(project);
        }

        
        //EDIT
        
        public async Task<IActionResult> Edit(int id)
        {
            var project = await _context.Projects.FindAsync(id);

            if (project == null)
                return NotFound();

            
            if (project.Status != "Rejected")
                return RedirectToAction(nameof(Index));

            return View(project);
        }

        
        //RESUBMIT
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Project project)
        {
            var existing = await _context.Projects.FindAsync(project.Id);

            if (existing == null)
                return NotFound();

            
            existing.Title = project.Title;
            existing.Abstract = project.Abstract;
            existing.TechStack = project.TechStack;
            existing.ResearchArea = project.ResearchArea;

            //RESET AFTER RESUBMIT
            existing.Status = "Pending";
            existing.Feedback = null;
            existing.Score = 0;
            existing.IsMatched = false;
            existing.SupervisorEmail = null;
            existing.SupervisorId = null;

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        
        //DELETE
        
        public async Task<IActionResult> Delete(int id)
        {
            var project = await _context.Projects.FindAsync(id);

            if (project != null)
            {
                _context.Remove(project);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }
    }
}