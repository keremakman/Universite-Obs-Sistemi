using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Universite.Models;

namespace Universite.Controllers
{
    public class HomeController : Controller
    {
        private readonly UniversiteContext _context = new UniversiteContext();
        
        public IActionResult Index(int? id)
        {
            var fakulteler = _context.Fakultelers.ToList();
            var bolumler = _context.Bolumlers.Include(x => x.Fakulte).ToList();

            var allData = (fakulteler: fakulteler, bolumler: bolumler);

            if (!id.HasValue || id == 0)
            {
                return View(allData);
            }
            else
            {
                var filtreBolumler = _context.Bolumlers.Include(x => x.Fakulte).Where(x => x.FakulteId == id).ToList();
                allData.bolumler = filtreBolumler; // Tuple içerisindeki bolumler özelliğine filtreBolumler'i atadık.
                return View(allData);
            }
        }

        public IActionResult FakulteEkle()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Search(string searchString)
        {
            var fakulteler = _context.Fakultelers.ToList();
            var bolumler = _context.Bolumlers.Include(x => x.Fakulte).ToList();

            if (!String.IsNullOrEmpty(searchString))
            {
                // Fakülte adı veya bölüm adı araması yapılacak
                fakulteler = fakulteler.Where(f => f.FakulteAd.Contains(searchString)).ToList();
                bolumler = bolumler.Where(b => b.BolumAd.Contains(searchString)).ToList();
            }

            var allData = (fakulteler: fakulteler, bolumler: bolumler);
            return View("Index", allData);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult FakulteEkle(Fakulteler fakulte)
        {
            _context.Fakultelers.Add(fakulte);
            _context.SaveChanges();
            return RedirectToAction("FakulteEkle");
        }

        public IActionResult BolumEkle()
        {
            var data = _context.Fakultelers.ToList();
            ViewBag.Fakulteler = new SelectList(data, "FakulteId", "FakulteAd");
            return View();
        }

        [HttpPost]
        public IActionResult BolumEkle(Bolumler bolum)
        {
            _context.Bolumlers.Add(bolum);
            _context.SaveChanges();
            return RedirectToAction("BolumEkle");
        }

        [HttpGet]
        public IActionResult EditBolum(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var bolum = _context.Bolumlers
                .Include(b => b.Fakulte)
                .FirstOrDefault(m => m.BolumId == id);
            if (bolum == null)
            {
                return NotFound();
            }

            ViewData["FakulteId"] = new SelectList(_context.Fakultelers, "FakulteId", "FakulteAd", bolum.FakulteId);
            return View(bolum);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult EditBolum(int id, [Bind("BolumId,FakulteId,BolumAd")] Bolumler bolum)
        {
            if (id != bolum.BolumId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(bolum);
                    _context.SaveChanges();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BolumExists(bolum.BolumId))
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
            ViewData["FakulteId"] = new SelectList(_context.Fakultelers, "FakulteId", "FakulteAd", bolum.FakulteId);
            return View(bolum);
        }

        public IActionResult DeleteBolum(int id)
        {
            var data = _context.Bolumlers.FirstOrDefault(x => x.BolumId == id);
            _context.Bolumlers.Remove(data!);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }

        private bool BolumExists(int id)
        {
            return _context.Bolumlers.Any(e => e.BolumId == id);
        }
    }
}
