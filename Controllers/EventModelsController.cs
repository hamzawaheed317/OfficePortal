using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using OfficePortal.Data;
using OfficePortal.Models;

namespace OfficePortal.Controllers
{
    public class EventModelsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public EventModelsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: EventModels
        public async Task<IActionResult> Index()
        {
              return _context.EventModel != null ? 
                          View(await _context.EventModel.ToListAsync()) :
                          Problem("Entity set 'ApplicationDbContext.EventModel'  is null.");
        }

        // GET: EventModels/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.EventModel == null)
            {
                return NotFound();
            }

            var eventModel = await _context.EventModel
                .FirstOrDefaultAsync(m => m.Id == id);
            if (eventModel == null)
            {
                return NotFound();
            }

            return View(eventModel);
        }

        // GET: EventModels/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: EventModels/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(EventModel eventModel)
        {
            if (ModelState.IsValid)
            {
                _context.Add(eventModel);
                await _context.SaveChangesAsync();

                return RedirectToAction("Privacy", "Home");
            }
            return View(eventModel);
        }

        // GET: EventModels/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.EventModel == null)
            {
                return NotFound();
            }

            var eventModel = await _context.EventModel.FindAsync(id);
            if (eventModel == null)
            {
                return NotFound();
            }
            return View(eventModel);
        }

        // POST: EventModels/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Title,StartDate,EndDate,Repeat,Location,Attendees,Description,Reminder,Label")] EventModel eventModel)
        {
            if (id != eventModel.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(eventModel);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!EventModelExists(eventModel.Id))
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
            return View(eventModel);
        }
        [HttpGet]
        [HttpGet]
        public JsonResult GetEvents()
        {
            var events = _context.EventModel.Select(e => new
            {
                title = e.Title,
                start = e.StartDate.ToString("yyyy-MM-ddTHH:mm:ss"),  // Format the date correctly
                end = e.EndDate.ToString("yyyy-MM-ddTHH:mm:ss"),
                label = e.Label
            }).ToList();

            return Json(events);
        }


        // Action to get event labels
        [HttpGet]
        public JsonResult GetEventLabels()
        {
            var labels = _context.EventModel.Select(e => e.Label).Distinct().ToList();
            return Json(labels);
        }

    

    // GET: EventModels/Delete/5
    public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.EventModel == null)
            {
                return NotFound();
            }

            var eventModel = await _context.EventModel
                .FirstOrDefaultAsync(m => m.Id == id);
            if (eventModel == null)
            {
                return NotFound();
            }

            return View(eventModel);
        }

        // POST: EventModels/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.EventModel == null)
            {
                return Problem("Entity set 'ApplicationDbContext.EventModel'  is null.");
            }
            var eventModel = await _context.EventModel.FindAsync(id);
            if (eventModel != null)
            {
                _context.EventModel.Remove(eventModel);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool EventModelExists(int id)
        {
          return (_context.EventModel?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
