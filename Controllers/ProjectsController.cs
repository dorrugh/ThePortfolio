using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ThePortfolio.Data;
using ThePortfolio.Models;
using System.IO;
using Microsoft.AspNetCore.Hosting;
using ThePortfolio.ViewModels;


namespace ThePortfolio.Controllers
{
    public class ProjectsController : Controller
    {
        private readonly ProjectContext _context;
        private readonly IWebHostEnvironment webHostEnvironment;

        public ProjectsController(ProjectContext context, IWebHostEnvironment hostEnvironment)
        {
            _context = context;
            webHostEnvironment = hostEnvironment;
        }

        // GET: Projects
        public async Task<IActionResult> Index()
        {
            return View(await _context.ProjectTbl.ToListAsync());
        }

        // GET: Projects/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var project = await _context.ProjectTbl
                .FirstOrDefaultAsync(m => m.Id == id);

            var projectViewModel = new ProjectViewModel()
            {
                Id = project.Id,
                ProjectName = project.ProjectName,
                Description = project.Description,
                TechnologiesUsed = project.TechnologiesUsed,
                ExistingImage = project.ProjectPicture,
            };

            if (project == null)
            {
                return NotFound();
            }

            return View(project);
        }

        // GET: Projects/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Projects/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,ProjectName,TechnologiesUsed,Description,ProjectPicture")] ProjectViewModel model)
        {
            if (ModelState.IsValid)
            {
                string uniqueFileName = ProcessUploadedFile(model);
                Project project = new Project
                {
                    Id = model.Id,
                    ProjectName = model.ProjectName,
                    Description = model.Description,
                    TechnologiesUsed = model.TechnologiesUsed,
                    ProjectPicture = uniqueFileName,
                };

                _context.Add(project);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(model);
        }

        // GET: Projects/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var project = await _context.ProjectTbl.FindAsync(id);
            var projectViewModel = new ProjectViewModel()
            {
                Id = project.Id,
                ProjectName = project.ProjectName,
                Description = project.Description,
                TechnologiesUsed = project.TechnologiesUsed,
                ExistingImage = project.ProjectPicture,
            };
            if (project == null)
            {
                return NotFound();
            }
            return View(projectViewModel);
        }

        // POST: Projects/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,ProjectName,TechnologiesUsed,Description,ProjectPicture")] ProjectViewModel model)
        {
            if (ModelState.IsValid)
            {
                var project = await _context.ProjectTbl.FindAsync(model.Id);
                project.ProjectName = model.ProjectName;
                project.Description = model.Description;
                project.TechnologiesUsed = model.TechnologiesUsed;

                if (model.ProjectPicture != null)
                {
                    if (model.ExistingImage != null)
                    {
                        string filePath = Path.Combine(webHostEnvironment.WebRootPath, FileLocation.FileUploadFolder, model.ExistingImage);
                        System.IO.File.Delete(filePath);
                    }

                    project.ProjectPicture = ProcessUploadedFile(model);
                }
                _context.Update(project);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View();
        }

        // GET: Projects/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var project = await _context.ProjectTbl
                .FirstOrDefaultAsync(m => m.Id == id);

            var projectViewModel = new ProjectViewModel()
            {
                Id = project.Id,
                ProjectName = project.ProjectName,
                Description = project.Description,
                TechnologiesUsed = project.TechnologiesUsed,
                ExistingImage = project.ProjectPicture
            };
            if (project == null)
            {
                return NotFound();
            }

            return View(projectViewModel);
        }

        // POST: Projects/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var project = await _context.ProjectTbl.FindAsync(id);
            var CurrentImage = Path.Combine(Directory.GetCurrentDirectory(), FileLocation.DeleteFileFromFolder, project.ProjectPicture);
            _context.ProjectTbl.Remove(project);
            if (await _context.SaveChangesAsync() > 0)
            {
                if (System.IO.File.Exists(CurrentImage))
                {
                    System.IO.File.Delete(CurrentImage);
                }
            }
            return RedirectToAction(nameof(Index));
        }

        private bool ProjectExists(int id)
        {
            return _context.ProjectTbl.Any(e => e.Id == id);
        }
        private string ProcessUploadedFile(ProjectViewModel model)
        {
            string uniqueFileName = null;

            if (model.ProjectPicture != null)
            {
                string uploadsFolder = Path.Combine(webHostEnvironment.WebRootPath, FileLocation.FileUploadFolder);
                uniqueFileName = Guid.NewGuid().ToString() + "_" + model.ProjectPicture.FileName;
                string filePath = Path.Combine(uploadsFolder, uniqueFileName);
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    model.ProjectPicture.CopyTo(fileStream);
                }
            }

            return uniqueFileName;
        }
    }
}
