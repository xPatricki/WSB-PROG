using biblioteka.Data;
using biblioteka.Models.DBEntities;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;

namespace biblioteka.Controllers
{
    public class ReservationsController : Controller
    {
        private readonly ReservationsDbContext _context;

        public ReservationsController(ReservationsDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult Index()
        {
            var reservations = _context.Reservations.ToList();
            return View(reservations);
        }

        [HttpGet]
        public IActionResult Details(int id)
        {
            var reservation = _context.Reservations.Find(id);
            if (reservation == null)
            {
                TempData["errorMessage"] = "Reservation not found.";
                return RedirectToAction("Index");
            }
            return View(reservation);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(Reservations reservation)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    _context.Reservations.Add(reservation);
                    _context.SaveChanges();

                    TempData["successMessage"] = "Reservation created successfully.";
                    return RedirectToAction("Index");
                }
                else
                {
                    TempData["errorMessage"] = "Model data is not valid.";
                    return View(reservation);
                }
            }
            catch (Exception ex)
            {
                TempData["errorMessage"] = ex.Message;
                return View(reservation);
            }
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            var reservation = _context.Reservations.Find(id);
            if (reservation == null)
            {
                TempData["errorMessage"] = "Reservation not found.";
                return RedirectToAction("Index");
            }
            return View(reservation);
        }

        [HttpPost]
        public IActionResult Edit(Reservations reservation)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    _context.Reservations.Update(reservation);
                    _context.SaveChanges();

                    TempData["successMessage"] = "Reservation updated successfully.";
                    return RedirectToAction("Index");
                }
                else
                {
                    TempData["errorMessage"] = "Model data is not valid.";
                    return View(reservation);
                }
            }
            catch (Exception ex)
            {
                TempData["errorMessage"] = ex.Message;
                return View(reservation);
            }
        }

        [HttpPost]
        public IActionResult DeleteConfirmed(int id)
        {
            try
            {
                var reservation = _context.Reservations.Find(id);
                if (reservation == null)
                {
                    TempData["errorMessage"] = "Reservation not found.";
                    return RedirectToAction("Index");
                }

                _context.Reservations.Remove(reservation);
                _context.SaveChanges();

                TempData["successMessage"] = "Reservation deleted successfully.";
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                TempData["errorMessage"] = ex.Message;
                return RedirectToAction("Index");
            }
        }
    }
}
