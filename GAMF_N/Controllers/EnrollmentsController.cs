using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using GAMF_N.Data;
using GAMF_N.Models;

namespace GAMF_N.Controllers
{
    public class EnrollmentsController : Controller
    {
        private readonly GAMFDbContext _context;

        public EnrollmentsController(GAMFDbContext context)
        {
            _context = context;
        }

        // GET: Enrollments
        public async Task<IActionResult> Index(string sortOrder, string searchString)
        {

            ViewBag.NameSortParam = string.IsNullOrEmpty(sortOrder) ? "NameDesc" : string.Empty;
            ViewBag.CourseSortParam = sortOrder == "Course" ? "CourseDesc" : "Course";
            var enrollments = _context.Enrollments
                .Include(e => e.Course)
                .Include(e => e.Students)
                .AsQueryable();

            if(!string.IsNullOrEmpty(searchString))
            {
                enrollments = enrollments.Where(
                    s => s.Students.LastName.ToUpper().Contains(searchString.ToUpper())
                    ||
                    s.Course.Title.ToUpper().Contains(searchString.ToUpper())
                    );
            }


            switch (sortOrder)
            {
                case "NameDesc":
                     enrollments = enrollments.OrderByDescending(e => e.Students.LastName);
                    break;
                case "Course":
                    enrollments = enrollments.OrderBy(e => e.Course.Title);
                    break;
                case "CourseDesc":
                    enrollments = enrollments.OrderByDescending(e => e.Course.Title);
                    break;
                default:
                    enrollments = enrollments.OrderBy(e => e.Students.LastName);
                    break;
            }
            
            // var gAMFDbContext = _context.Enrollments.Include(e => e.Course).Include(e => e.Students);
            return View(await enrollments.ToListAsync());
        }

        public IActionResult Index2() => View();

        public JsonResult GetEnrollments()
        {
            var enrollments = _context.Enrollments
                .Include(e => e.Course)
                .Include(e => e.Students)
                .Select(e => new EnrollmentListVM
                {
                    CourseTitle = e.Course.Title,
                    StudentFullname = $"{e.Students.LastName} {e.Students.FirstMidName}",
                    Grade = e.Grade.ToString()
                });

            return Json(enrollments.ToList());
        }





        // GET: Enrollments/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Enrollments == null)
            {
                return NotFound();
            }

            var enrollment = await _context.Enrollments
                .Include(e => e.Course)
                .Include(e => e.Students)
                .FirstOrDefaultAsync(m => m.EnrollmentId == id);
            if (enrollment == null)
            {
                return NotFound();
            }

            return View(enrollment);
        }

        // GET: Enrollments/Create
        public IActionResult Create()
        {
            ViewData["CourseId"] = new SelectList(_context.Courses, "CourseId", "CourseId");
            ViewData["StudentId"] = new SelectList(_context.Students, "Id", "FirstMidName");
            return View();
        }

        // POST: Enrollments/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("EnrollmentId,CourseId,StudentId,Grade")] Enrollment enrollment)
        {
            if (ModelState.IsValid)
            {
                _context.Add(enrollment);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CourseId"] = new SelectList(_context.Courses, "CourseId", "CourseId", enrollment.CourseId);
            ViewData["StudentId"] = new SelectList(_context.Students, "Id", "FirstMidName", enrollment.StudentId);
            return View(enrollment);
        }

        // GET: Enrollments/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Enrollments == null)
            {
                return NotFound();
            }

            var enrollment = await _context.Enrollments.FindAsync(id);
            if (enrollment == null)
            {
                return NotFound();
            }
            ViewData["CourseId"] = new SelectList(_context.Courses, "CourseId", "CourseId", enrollment.CourseId);
            ViewData["StudentId"] = new SelectList(_context.Students, "Id", "FirstMidName", enrollment.StudentId);
            return View(enrollment);
        }

        // POST: Enrollments/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("EnrollmentId,CourseId,StudentId,Grade")] Enrollment enrollment)
        {
            if (id != enrollment.EnrollmentId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(enrollment);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!EnrollmentExists(enrollment.EnrollmentId))
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
            ViewData["CourseId"] = new SelectList(_context.Courses, "CourseId", "CourseId", enrollment.CourseId);
            ViewData["StudentId"] = new SelectList(_context.Students, "Id", "FirstMidName", enrollment.StudentId);
            return View(enrollment);
        }

        // GET: Enrollments/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Enrollments == null)
            {
                return NotFound();
            }

            var enrollment = await _context.Enrollments
                .Include(e => e.Course)
                .Include(e => e.Students)
                .FirstOrDefaultAsync(m => m.EnrollmentId == id);
            if (enrollment == null)
            {
                return NotFound();
            }

            return View(enrollment);
        }

        // POST: Enrollments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Enrollments == null)
            {
                return Problem("Entity set 'GAMFDbContext.Enrollments'  is null.");
            }
            var enrollment = await _context.Enrollments.FindAsync(id);
            if (enrollment != null)
            {
                _context.Enrollments.Remove(enrollment);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool EnrollmentExists(int id)
        {
          return _context.Enrollments.Any(e => e.EnrollmentId == id);
        }
    }
}
