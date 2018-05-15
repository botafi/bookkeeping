using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Bookkeeping.Data;
using Bookkeeping.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;

namespace Bookkeeping.Controllers
{
    [Authorize]
    [Route("[controller]/[action]")]
    public class InvoicesController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ApplicationDbContext _context;

        public InvoicesController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: Invoices
        public async Task<IActionResult> Index()
        {
            var user = await _userManager.GetUserAsync(User);
            var applicationDbContext = _context.Invoices.Where(i => i.ApplicationUserId == user.Id)
                .Include(i => i.Contact)
                .Include(i => i.InvoiceItems);
            var list = await applicationDbContext.ToListAsync();
            ViewData["Sum"] = list.SelectMany(i => i.InvoiceItems.Select(ii => (i.InvoiceType == InvoiceType.Expense ? -1 : 1) * ii.Price * ii.Amount)).Sum();
            ViewData["Income"] = list.Where(i => i.InvoiceType == InvoiceType.Income).SelectMany(i => i.InvoiceItems.Select(ii => ii.Price * ii.Amount)).Sum();
            ViewData["Expense"] = list.Where(i => i.InvoiceType == InvoiceType.Expense).SelectMany(i => i.InvoiceItems.Select(ii => -1 * ii.Price * ii.Amount)).Sum();
            return View(list);
        }

        // GET: Invoices/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var user = await _userManager.GetUserAsync(User);
            var invoice = await _context.Invoices
                .Include(i => i.Contact)
                .SingleOrDefaultAsync(m => m.InvoiceId == id && m.ApplicationUserId == user.Id);
            if (invoice == null)
            {
                return NotFound();
            }

            return View(invoice);
        }

        // GET: Invoices/Create
        public async Task<IActionResult> Create()
        {
            var user = await _userManager.GetUserAsync(User);
            ViewData["ContactId"] = new SelectList(_context.Contacts.Where(c => c.ApplicationUserId == user.Id), "ContactId", "Name");
            return View(new Invoice() { DueTime = DateTime.Now.AddDays(14), DateOfIssue = DateTime.Now, Title = DateTime.Now.Year.ToString() + "-" + await _context.Invoices.Where(i => i.ApplicationUserId == user.Id).CountAsync()});
        }

        // POST: Invoices/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("InvoiceId,ContactId,DueTime,DateOfIssue,Title,InvoiceType")] Invoice invoice, InvoiceItem[] InvoiceItems)
        {
            var user = await _userManager.GetUserAsync(User);
            ViewData["ContactId"] = new SelectList(_context.Contacts.Where(c => c.ApplicationUserId == user.Id), "ContactId", "Name", invoice.ContactId);
            ModelState.Clear();
            invoice.ApplicationUserId = user.Id;
            invoice.ApplicationUser = user;
            if (TryValidateModel(invoice))
            {
                _context.Add(invoice);
                await _context.SaveChangesAsync();
                invoice.InvoiceItems = InvoiceItems;
                foreach (InvoiceItem ii in InvoiceItems)
                {
                    ii.Invoice = invoice;
                    ii.InvoiceId = invoice.InvoiceId;
                    if (TryValidateModel(ii))
                    {
                        _context.Add(ii);
                    }
                    else
                    {
                        return View(invoice);
                    }
                }
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            else
            {
                return View(invoice);
            }
        }

        // GET: Invoices/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            var user = await _userManager.GetUserAsync(User);
            if (id == null)
            {
                return NotFound();
            }

            var invoice = await _context.Invoices
                .Include(i => i.Contact)
                .Include(i => i.InvoiceItems)
                .SingleOrDefaultAsync(m => m.InvoiceId == id && m.ApplicationUserId == user.Id)
                ;
            if (invoice == null)
            {
                return NotFound();
            }
            ViewData["ContactId"] = new SelectList(_context.Contacts.Where(c => c.ApplicationUserId == user.Id), "ContactId", "Name", invoice.ContactId);
            return View(invoice);
        }

        // POST: Invoices/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("InvoiceId,ContactId,DueTime,DateOfIssue,Title,InvoiceType")] Invoice invoice, List<InvoiceItem> InvoiceItems, List<int> DeleteInvoiceItems)
        {
            var user = await _userManager.GetUserAsync(User);

            if (!HasInvoice(invoice.InvoiceId, user.Id))
            {
                return NotFound();
            }
            invoice.ApplicationUserId = user.Id;
            invoice.ApplicationUser = user;
            ModelState.Clear();
            if (TryValidateModel(invoice))
            {
                try
                {
                    _context.Update(invoice);
                    await _context.SaveChangesAsync();
                    invoice.InvoiceItems = InvoiceItems;
                    foreach (InvoiceItem ii in InvoiceItems)
                    {
                        ii.Invoice = invoice;
                        ii.InvoiceId = invoice.InvoiceId;
                        if (TryValidateModel(ii))
                        {
                            if (ii.InvoiceItemId == null)
                                _context.Add(ii);
                            else
                                _context.Update(ii);
                        }
                        else
                        {
                            return View(invoice);
                        }
                    }
                    await _context.SaveChangesAsync();
                    _context.RemoveRange(
                        _context.InvoiceItems.Where(
                            ii => ii.InvoiceId == invoice.InvoiceId && DeleteInvoiceItems.Contains(ii.InvoiceItemId != null ? (int)ii.InvoiceItemId : -1)
                        )
                        .ToList()
                    );
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!InvoiceExists(invoice.InvoiceId))
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
            ViewData["ContactId"] = new SelectList(_context.Contacts.Where(c => c.ApplicationUserId == user.Id), "ContactId", "Name", invoice.ContactId);
            return View(invoice);
        }

        // GET: Invoices/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            var user = await _userManager.GetUserAsync(User);
            if (id == null)
            {
                return NotFound();
            }

            var invoice = await _context.Invoices
                .Include(i => i.Contact)
                .SingleOrDefaultAsync(m => m.InvoiceId == id && m.ApplicationUserId == user.Id);
            if (invoice == null)
            {
                return NotFound();
            }

            return View(invoice);
        }

        // POST: Invoices/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var user = await _userManager.GetUserAsync(User);
            var invoice = await _context.Invoices.SingleOrDefaultAsync(m => m.InvoiceId == id && m.ApplicationUserId == user.Id);
            if (invoice == null)
            {
                return NotFound();
            }
            _context.Invoices.Remove(invoice);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool InvoiceExists(int id)
        {
            return _context.Invoices.Any(e => e.InvoiceId == id);
        }
        private bool HasInvoice(int id, string userId)
        {
            Console.WriteLine("HasInvoice(int id {0}, string userId {1})", id, userId);
            return _context.Invoices.Any(e => e.InvoiceId == id && e.ApplicationUserId == userId);
        }
    }
}
