using biblioteka.Data;
using biblioteka.Models.DBEntities;
using biblioteka.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Linq;
using System.Threading.Tasks;

namespace biblioteka.Controllers
{
    public class ReservationsController : Controller
    {
        private readonly ReservationsDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly BooksDbContext _booksContext;

        public ReservationsController(ReservationsDbContext context, UserManager<IdentityUser> userManager, BooksDbContext booksContext)
        {
            _context = context;
            _userManager = userManager;
            _booksContext = booksContext;
        }

public async Task<IActionResult> Index()
{
    var currentUser = await _userManager.GetUserAsync(User);
    if (currentUser == null)
    {
        return Challenge();
    }

    // Get the current date
    var currentDate = DateTime.Now;

    // Retrieve expired reservations for the current user
    var expiredReservations = await _context.Reservations
        .Where(r => r.UserID == currentUser.Id && r.DueDate < currentDate)
        .ToListAsync();

    // Delete expired reservations
    if (expiredReservations.Any())
    {
        _context.Reservations.RemoveRange(expiredReservations);
        await _context.SaveChangesAsync();
    }

    // Retrieve active reservations for the current user
    var reservations = await _context.Reservations
        .Where(r => r.UserID == currentUser.Id && r.DueDate >= currentDate)
        .Include(r => r.Book)
        .ToListAsync();

    return View(reservations);
}

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var currentUser = await _userManager.GetUserAsync(User);
            if (currentUser == null)
            {
                return Challenge();
            }

            ViewBag.Users = await _userManager.Users.Select(u => new SelectListItem 
            { 
                Value = u.Id, 
                Text = u.UserName 
            }).ToListAsync();

            ViewBag.Books = await _booksContext.Books.Select(b => new SelectListItem 
            { 
                Value = b.BookID.ToString(), 
                Text = b.Title 
            }).ToListAsync();

            ViewBag.CurrentUserID = currentUser.Id;

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(ReservationsViewModel model)
        {
            Console.WriteLine($"-----CREATE-------: {model.BookID}, {model.UserID}");
            if (!ModelState.IsValid)
            {
                foreach (var error in ModelState.Values.SelectMany(v => v.Errors))
                {
                    Console.WriteLine(error.ErrorMessage);
                }

                var users = await _userManager.Users.ToListAsync();
                var books = await _booksContext.Books.ToListAsync();

                return View(model);
            }

            try
            {
                var reservation = new Reservations
                {
                    UserID = model.UserID,
                    BookID = model.BookID,
                    ReservationDate = model.ReservationDate,
                    DueDate = model.DueDate
                };

                _context.Reservations.Add(reservation);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                ModelState.AddModelError(string.Empty, "An error occurred while saving the reservation.");

                var users = await _userManager.Users.ToListAsync();
                var books = await _booksContext.Books.ToListAsync();

                return View(model);
            }
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var reservation = await _context.Reservations.FindAsync(id);
            if (reservation == null)
            {
                return NotFound();
            }

            var users = await _userManager.Users.ToListAsync();
            var books = await _booksContext.Books.ToListAsync();
            ViewBag.Books = await _booksContext.Books.Select(b => new SelectListItem 
            { 
                Value = b.BookID.ToString(), 
                Text = b.Title 
            }).ToListAsync();

            var model = new ReservationsViewModel
            {
                ReservationID = reservation.ReservationID,
                UserID = reservation.UserID,
                BookID = reservation.BookID,
                ReservationDate = reservation.ReservationDate,
                DueDate = reservation.DueDate,
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(ReservationsViewModel model)
        {
            if (ModelState.IsValid)
            {
                var reservation = await _context.Reservations.FindAsync(model.ReservationID);
                if (reservation == null)
                {
                    return NotFound();
                }

                reservation.UserID = model.UserID;
                reservation.BookID = model.BookID;
                reservation.ReservationDate = model.ReservationDate;
                reservation.DueDate = model.DueDate;

                _context.Reservations.Update(reservation);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            var users = await _userManager.Users.ToListAsync();
            var books = await _booksContext.Books.ToListAsync();

            return View(model);
        }

    [HttpGet]
    public async Task<IActionResult> Delete(int id)
    {
        var reservation = await _context.Reservations.FindAsync(id);
        if (reservation == null)
        {
            return NotFound();
        }

        return View(reservation);
    }

[HttpPost, ActionName("Delete")]
public async Task<IActionResult> DeleteConfirmed(int id)
{
    var reservation = await _context.Reservations.FindAsync(id);
    if (reservation == null)
    {
        return NotFound();
    }

    _context.Reservations.Remove(reservation);
    await _context.SaveChangesAsync();
    return RedirectToAction(nameof(Index));
}
    }
}
