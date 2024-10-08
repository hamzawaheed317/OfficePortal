using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using OfficePortal.Data;
using OfficePortal.Models;
using OfficePortal.Services;

namespace OfficePortal.Controllers
{
    public class TrainingRequestViewModelsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private LanguageService _localization;

        public TrainingRequestViewModelsController(ApplicationDbContext context, LanguageService localization)
        {
            _context = context;
            _localization = localization;

        }

        // GET: TrainingRequestViewModels
        public async Task<IActionResult> Index()
        {
            return View(await _context.TrainingRequestViewModel.ToListAsync());
        }

        // GET: TrainingRequestViewModels/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var trainingRequestViewModel = await _context.TrainingRequestViewModel
                .FirstOrDefaultAsync(m => m.id == id);
            if (trainingRequestViewModel == null)
            {
                return NotFound();
            }

            return PartialView("_TrainingRequestDetails", trainingRequestViewModel);
        }

        // GET: TrainingRequestViewModels/Create
        public IActionResult Create()
        {
            UserInfo userInfo = GetUserInfo(); // Replace this with actual fetching logic

            TrainingRequestViewModel model = new TrainingRequestViewModel
            {
                EmployeeName = userInfo.Employee_Name,
                JobTitle = userInfo.Job_Title,
                Grade = userInfo.Grade,
                Number = userInfo.Employee_Number.ToString(),
                Department = userInfo.Department,
                LineManager = userInfo.role
            };

            // Assuming this method populates all the necessary employee details into the model

            return View(model);
        }

        // POST: TrainingRequestViewModels/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(TrainingRequestViewModel trainingRequestViewModel)
        {
            if (ModelState.IsValid)
            {  
                _context.TrainingRequestViewModel.Add(trainingRequestViewModel); 
                await _context.SaveChangesAsync(); 

                return RedirectToAction(nameof(Index)); 
            }
            return View(trainingRequestViewModel);
        }

        private UserInfo GetUserInfo()
        {
            return new UserInfo
            {
                Employee_Name = "John Doe",
                Job_Title = "Software Engineer",
                Grade = "A1",
                Employee_Number = "12345",
                Department = "IT",
                role = "Line Manager"
            };
        }
        // GET: TrainingRequestViewModels/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var trainingRequestViewModel = await _context.TrainingRequestViewModel.FindAsync(id);
            if (trainingRequestViewModel == null)
            {
                return NotFound();
            }
            return View(trainingRequestViewModel);
        }

        // POST: TrainingRequestViewModels/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("id,EmployeeName,JobTitle,Grade,Number,Department,LineManager,Type,TrainingEntity,DateFrom,DateTo,Location,Amount,form_type,status")] TrainingRequestViewModel trainingRequestViewModel)
        {
            if (id != trainingRequestViewModel.id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(trainingRequestViewModel);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TrainingRequestViewModelExists(trainingRequestViewModel.id))
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
            return View(trainingRequestViewModel);
        }

        // GET: TrainingRequestViewModels/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var trainingRequestViewModel = await _context.TrainingRequestViewModel
                .FirstOrDefaultAsync(m => m.id == id);
            if (trainingRequestViewModel == null)
            {
                return NotFound();
            }

            return View(trainingRequestViewModel);
        }

        // POST: TrainingRequestViewModels/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var trainingRequestViewModel = await _context.TrainingRequestViewModel.FindAsync(id);
            if (trainingRequestViewModel != null)
            {
                _context.TrainingRequestViewModel.Remove(trainingRequestViewModel);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TrainingRequestViewModelExists(int id)
        {
            return _context.TrainingRequestViewModel.Any(e => e.id == id);
        }
    }
}
