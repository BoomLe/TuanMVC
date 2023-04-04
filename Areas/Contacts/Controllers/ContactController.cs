using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using App.Areas.Contacts.Models;
using App.Models;
using Microsoft.AspNetCore.Authorization;

namespace App.Areas.Contacts.Controllers
{
    [Area("Contacts")]
    
    public class ContactController : Controller
    {
        private readonly MyDbContext _context;
        private readonly ContactModel _contact;

        [TempData]
        public string AlertStatusMessage {set;get;}
      

        public ContactController(MyDbContext context, ContactModel contact)
        {
            _context = context;
            _contact = contact;
        }

          
      

        // GET: Contact
        [HttpGet("/admin/contact")]
        public async Task<IActionResult> Index()
        {
              return _context.contactModel != null ? 
                          View(await _context.contactModel.ToListAsync()) :
                          Problem("Entity set 'MyDbContext.contactModel'  is null.");
        }

        // GET: Contact/Details/5
        [HttpGet("/admin/contact/details/{id?}")]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.contactModel == null)
            {
                return NotFound();
            }

            var contactModel = await _context.contactModel
                .FirstOrDefaultAsync(m => m.Id == id);
            if (contactModel == null)
            {
                return NotFound();
            }

            return View(contactModel);
        }

        // GET: Contact/Create
        [HttpGet("/contact/")]
        [AllowAnonymous]
        public IActionResult SendContact()
        {
            return View();
        }

      

        // POST: Contact/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost("/contact/")]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SendContact([Bind("FullName,Email,PhoneNumber,Message")] ContactModel contactModel)
        {
            if (ModelState.IsValid)
            {
                _contact.SendDate = DateTime.Now;
                _context.Add(contactModel);
                await _context.SaveChangesAsync();

                AlertStatusMessage = "Cảm ơn bạn đã góp ý !";

                return RedirectToAction("Index", "Home");
            }
            return View(contactModel);
        }



        // GET: Contact/Delete/5
        [HttpGet("/admin/contact/delete/{id?}")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.contactModel == null)
            {
                return NotFound();
            }

            var contactModel = await _context.contactModel
                .FirstOrDefaultAsync(m => m.Id == id);
            if (contactModel == null)
            {
                return NotFound();
            }

            return View(contactModel);
        }

        // POST: Contact/Delete/5
        [HttpPost("/admin/contact/delete/{id?}"), ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.contactModel == null)
            {
                return Problem("Entity set 'MyDbContext.contactModel'  is null.");
            }
            var contactModel = await _context.contactModel.FindAsync(id);
            if (contactModel != null)
            {
                _context.contactModel.Remove(contactModel);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

     
    }
}
