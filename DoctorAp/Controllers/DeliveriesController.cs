using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using DoctorAp.Data;
using DoctorAp.Models;
using Microsoft.AspNetCore.Authorization;
using System.Data;

namespace DoctorAp.Controllers
{
   
    public class DeliveriesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public DeliveriesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Deliveries
        public async Task<IActionResult> Index()
        {
              return _context.Delivery != null ? 
                          View(await _context.Delivery.ToListAsync()) :
                          Problem("Entity set 'ApplicationDbContext.Delivery'  is null.");
        }

        // GET: Deliveries/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Delivery == null)
            {
                return NotFound();
            }

            var delivery = await _context.Delivery
                .FirstOrDefaultAsync(m => m.DeliveryId == id);
            if (delivery == null)
            {
                return NotFound();
            }

            return View(delivery);
        }

        // GET: Deliveries/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Deliveries/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("DeliveryId,OrderNumber,Name,Email,Address,City,State,PostalCode,Country,PhoneNumber,DeliveryDate,Progress")] Delivery delivery)
        {
            string address = delivery.Address;
            ViewBag.Address = address;

            if (ModelState.IsValid)
            {
                _context.Add(delivery);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index","ItemsLeads"); //hhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhh
            }
            return View(delivery);
        }

        // GET: Deliveries/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Delivery == null)
            {
                return NotFound();
            }

            var delivery = await _context.Delivery.FindAsync(id);
            if (delivery == null)
            {
                return NotFound();
            }
            return View(delivery);
        }

        // POST: Deliveries/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("DeliveryId,OrderNumber,Name,Email,Address,City,State,PostalCode,Country,PhoneNumber,DeliveryDate,Progress")] Delivery delivery)
        {
            if (id != delivery.DeliveryId)
            {
                return NotFound();
            }
            string address = delivery.Address;
            

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(delivery);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DeliveryExists(delivery.DeliveryId))
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
            return View(delivery);
        }

        // GET: Deliveries/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Delivery == null)
            {
                return NotFound();
            }

            var delivery = await _context.Delivery
                .FirstOrDefaultAsync(m => m.DeliveryId == id);
            if (delivery == null)
            {
                return NotFound();
            }

            return View(delivery);
        }

        // POST: Deliveries/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Delivery == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Delivery'  is null.");
            }
            var delivery = await _context.Delivery.FindAsync(id);
            if (delivery != null)
            {
                _context.Delivery.Remove(delivery);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool DeliveryExists(int id)
        {
          return (_context.Delivery?.Any(e => e.DeliveryId == id)).GetValueOrDefault();
        }
    }
}
