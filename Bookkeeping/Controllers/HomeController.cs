using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Bookkeeping.Models;
using Bookkeeping.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Bookkeeping.Controllers
{
    public class HomeController : Controller
    {
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public HomeController(ApplicationDbContext context, UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager)
        {
            _context = context;
            _signInManager = signInManager;
            _userManager = userManager;
        }
        public async Task<IActionResult> Index()
        {
            if(_signInManager.IsSignedIn(User))
            {
                var user = await _userManager.GetUserAsync(User);
                var applicationDbContext = _context.Invoices.Where(i => i.ApplicationUserId == user.Id)
                    .Include(i => i.Contact)
                    .Include(i => i.InvoiceItems);
                var list = await applicationDbContext.ToListAsync();
                ViewData["Sum"] = list.SelectMany(i => i.InvoiceItems.Select(ii => (i.InvoiceType == InvoiceType.Expense ? -1 : 1) * ii.Price * ii.Amount)).Sum();
                ViewData["Income"] = list.Where(i => i.InvoiceType == InvoiceType.Income).SelectMany(i => i.InvoiceItems.Select(ii => ii.Price * ii.Amount)).Sum();
                ViewData["Expense"] = list.Where(i => i.InvoiceType == InvoiceType.Expense).SelectMany(i => i.InvoiceItems.Select(ii => -1 * ii.Price * ii.Amount)).Sum();
            }
            return View();
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
