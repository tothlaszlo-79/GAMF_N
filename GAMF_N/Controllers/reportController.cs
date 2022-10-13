using GAMF_N.Data;
using GAMF_N.Models;
using Microsoft.AspNetCore.Mvc;

namespace GAMF_N.Controllers
{
    public class reportController : Controller
    {
        private readonly GAMFDbContext _Context;

        public reportController(GAMFDbContext context)
        {
            _Context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public IActionResult EnrollmentDateReport()
        {
            var result = _Context.Students.GroupBy(s => s.EnrollmentDate)
                .Select(s => new EnrollmentDateVM
                {
                    EnrollmentDate = s.Key,
                    StudentCount = s.Count()
                });


            return View(result.ToList());
        }
    }
}
