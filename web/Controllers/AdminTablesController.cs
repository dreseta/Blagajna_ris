using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;
using web.Data;

namespace web.Controllers
{
    [Authorize(Roles = "Administrator")]
    public class AdminTablesController : Controller
    {
        private readonly BlagajnaContext _context;

        public AdminTablesController(BlagajnaContext context)
        {
            _context = context;
        }

        public IActionResult Index(string tableName)
        {
            ViewBag.Tables = new List<string>
            {
                "Transactions",
                "Categories",
                "ApplicationUsers",
                "SavedMoney",
                "Budgets",
                "Incomes"
            };

            if (string.IsNullOrEmpty(tableName))
            {
                return View("SelectTable"); // Pokaže stran za izbiro tabele.
            }

            dynamic data;
            switch (tableName)
            {
                case "Transactions":
                    data = _context.Transactions.Include(t => t.Category).Include(t => t.User).Select(t => new
                    {
                        t.Id,
                        t.Amount,
                        Category = t.Category != null ? t.Category.Name : "Ni kategorije",
                        User = t.User != null ? t.User.UserName : "Ni uporabnika",
                        t.Date
                    }).ToList();
                    break;
                case "Categories":
                    data = _context.Categories.Include(t => t.User).ToList();
                    break;
                case "ApplicationUsers":
                    data = _context.Users.ToList();
                    break;
                case "SavedMoney":
                    data = _context.SavedMoney.Include(t => t.User).ToList();
                    break;
                case "Budgets":
                    data = _context.Budgets.Include(t => t.User).ToList();
                    break;
                case "Incomes":
                    data = _context.Incomes.Include(t => t.User).ToList();
                    break;    
                default:
                    return NotFound();
            }

            ViewBag.TableName = tableName;
            return View("TableView", data); // Prikaz izbrane tabele.
        }
    }
}
