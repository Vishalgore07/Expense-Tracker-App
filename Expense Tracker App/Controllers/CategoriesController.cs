using Expense_Tracker_App.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Expense_Tracker_App.Controllers
{
    public class CategoriesController : Controller
    {
        private readonly ApplicationDbContext _db;

        public CategoriesController(ApplicationDbContext db)
        {
            _db = db;
        }

        public async Task<IActionResult> Index()
        {
            return _db.Categories != null ?
                                    View(await _db.Categories.ToListAsync()) :
                                    Problem("Entity set 'ApplicationDbContext.Categories' is null.");
        }

    }
}
