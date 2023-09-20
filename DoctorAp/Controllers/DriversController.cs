using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using DoctorAp.Data;
using DoctorAp.Models;

namespace DoctorAp.Controllers
{
    public class DriversController : Controller
    {
        private readonly ApplicationDbContext _context;

        public DriversController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Drivers
        public async Task<IActionResult> Index()
        {
              return _context.Drivers != null ? 
                          View(await _context.Drivers.ToListAsync()) :
                          Problem("Entity set 'ApplicationDbContext.Drivers'  is null.");
        }

        // GET: Drivers/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Drivers == null)
            {
                return NotFound();
            }

            var driver = await _context.Drivers
                .FirstOrDefaultAsync(m => m.Id == id);
            if (driver == null)
            {
                return NotFound();
            }

            return View(driver);
        }

        // GET: Drivers/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Drivers/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,OrderNumber,DriverName")] Driver driver)
        {
            if (ModelState.IsValid)
            {
                // Set Delivered to a default value, for example, false (not delivered)
                driver.Delivered = false;

                _context.Add(driver);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            // Debugging: Check ModelState errors
            var errors = ModelState.Values.SelectMany(v => v.Errors);
            Console.WriteLine(errors);

            return View(driver);
        }


        // GET: Drivers/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Drivers == null)
            {
                return NotFound();
            }

            var driver = await _context.Drivers.FindAsync(id);
            if (driver == null)
            {
                return NotFound();
            }
            return View(driver);
        }

        // POST: Drivers/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,OrderNumber,DriverName,Delivered")] Driver driver)
        {
            if (id != driver.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(driver);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DriverExists(driver.Id))
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
            return View(driver);
        }

        // GET: Drivers/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Drivers == null)
            {
                return NotFound();
            }

            var driver = await _context.Drivers
                .FirstOrDefaultAsync(m => m.Id == id);
            if (driver == null)
            {
                return NotFound();
            }

            return View(driver);
        }

        // POST: Drivers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Drivers == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Drivers'  is null.");
            }
            var driver = await _context.Drivers.FindAsync(id);
            if (driver != null)
            {
                _context.Drivers.Remove(driver);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool DriverExists(int id)
        {
          return (_context.Drivers?.Any(e => e.Id == id)).GetValueOrDefault();
        }


        [HttpPost]
        public IActionResult UpdateDeliveredStatus(int id, bool delivered)
        {
            var driver = _context.Drivers.Find(id);
            if (driver != null)
            {
                driver.Delivered = delivered;
                _context.SaveChanges();
                return Json(new { success = true });
            }

            return Json(new { success = false });
        }
        public IActionResult DeleteSelected(List<int> selectedDrivers)
        {
            if (selectedDrivers == null || selectedDrivers.Count == 0)
            {
                return Json(new { success = false });
            }

            var deletedDrivers = new List<int>();

            foreach (var driverId in selectedDrivers)
            {
                var driver = _context.Drivers.Find(driverId);

                if (driver != null)
                {
                    _context.Drivers.Remove(driver);
                    deletedDrivers.Add(driverId);
                }
            }

            _context.SaveChanges();

            return Json(new { success = true, deletedDrivers = deletedDrivers });
        }
    }
}

