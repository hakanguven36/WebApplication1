using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Araclar;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    [Yetki("admin")]
    public class ProjectsController : Controller
    {
        private readonly MyContext _context;

        public ProjectsController(MyContext context)
        {
            _context = context;
        }

        // GET: Projects
        public async Task<IActionResult> Index()
        {
            return View(await _context.Proje.ToListAsync());
        }

        // GET: Projects/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var proje = await _context.Proje
                .FirstOrDefaultAsync(m => m.ID == id);
            if (proje == null)
            {
                return NotFound();
            }

            return View(proje);
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
        public async Task<IActionResult> Create([Bind("ID,name")] Proje proje)
        {
            if (ModelState.IsValid)
            {
                _context.Add(proje);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(proje);
        }

        // GET: Projects/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var proje = await _context.Proje.FindAsync(id);
            if (proje == null)
            {
                return NotFound();
            }
            return View(proje);
        }

        // POST: Projects/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID,name")] Proje proje)
        {
            if (id != proje.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(proje);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProjeExists(proje.ID))
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
            return View(proje);
        }

        // GET: Projects/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var proje = await _context.Proje
                .FirstOrDefaultAsync(m => m.ID == id);
            if (proje == null)
            {
                return NotFound();
            }

            return View(proje);
        }

        // POST: Projects/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var proje = await _context.Proje.FindAsync(id);
            _context.Proje.Remove(proje);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ProjeExists(int id)
        {
            return _context.Proje.Any(e => e.ID == id);
        }
    }
}
