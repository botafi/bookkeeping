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
    public class ContactsController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ApplicationDbContext _context;

        public ContactsController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: Contacts
        public async Task<IActionResult> Index()
        {
            var user = await _userManager.GetUserAsync(User);
            return View(await _context.Contacts.Where(c => c.ApplicationUserId == user.Id).ToListAsync());
        }

        // GET: Contacts/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            var user = await _userManager.GetUserAsync(User);
            if (id == null)
            {
                return NotFound();
            }

            var contact = await _context.Contacts
                .SingleOrDefaultAsync(m => m.ContactId == id && m.ApplicationUserId == user.Id);
            if (contact == null)
            {
                return NotFound();
            }

            return View(contact);
        }

        // GET: Contacts/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Contacts/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ContactId,Name,Address,Type,ICO,DIC")] Contact contact)
        {
            var user = await _userManager.GetUserAsync(User);
            contact.ApplicationUserId = user.Id;
            contact.ApplicationUser = user;
            ModelState.Clear();
            if (TryValidateModel(contact))
            {
                _context.Add(contact);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(contact);
        }

        // GET: Contacts/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            var user = await _userManager.GetUserAsync(User);
            if (id == null)
            {
                return NotFound();
            }

            var contact = await _context.Contacts.SingleOrDefaultAsync(m => m.ContactId == id && m.ApplicationUserId == user.Id);
            if (contact == null)
            {
                return NotFound();
            }
            return View(contact);
        }

        // POST: Contacts/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ContactId,Name,Address,Type,ICO,DIC")] Contact contact)
        {
            var user = await _userManager.GetUserAsync(User);
            if (id != contact.ContactId)
            {
                return NotFound();
            }

            if (!HasContact(id, user.Id))
            {
                return NotFound();
            }

            contact.ApplicationUserId = user.Id;
            contact.ApplicationUser = user;
            ModelState.Clear();
            if (TryValidateModel(contact))
            {
                try
                {
                    _context.Update(contact);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ContactExists(contact.ContactId))
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
            return View(contact);
        }

        // GET: Contacts/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            var user = await _userManager.GetUserAsync(User);
            if (id == null)
            {
                return NotFound();
            }

            var contact = await _context.Contacts
                .SingleOrDefaultAsync(m => m.ContactId == id && m.ApplicationUserId == user.Id);
            if (contact == null)
            {
                return NotFound();
            }

            return View(contact);
        }

        // POST: Contacts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var user = await _userManager.GetUserAsync(User);
            var contact = await _context.Contacts.SingleOrDefaultAsync(m => m.ContactId == id && m.ApplicationUserId == user.Id);
            _context.Contacts.Remove(contact);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ContactExists(int id)
        {
            return _context.Contacts.Any(e => e.ContactId == id);
        }
        private bool HasContact(int id, string userId)
        {
            return _context.Contacts.Any(e => e.ContactId == id && e.ApplicationUserId == userId);
        }
    }
}
