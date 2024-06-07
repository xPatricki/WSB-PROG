using biblioteka.Data;
using biblioteka.Models.DBEntities;
using biblioteka.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Rendering;  // Add this using directive
using System.Linq;
using System.Threading.Tasks;

namespace biblioteka.Controllers
{
    // [Authorize(Roles = "Admin")]
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

        public IActionResult Index()
        {
            var reservations = _context.Reservations
                .Include(r => r.User)
                .Include(r => r.Book)
                .ToList();
            return View(reservations);
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var users = await _userManager.Users.ToListAsync();
            var books = await _booksContext.Books.ToListAsync();

            var model = new ReservationViewModel
            {
                Users = users.Select(u => new SelectListItem { Value = u.Id, Text = u.UserName }).ToList(),
                Books = books.Select(b => new SelectListItem { Value = b.BookID.ToString(), Text = b.Title }).ToList()
            };

            return View(model);
        }

[HttpPost]
public async Task<IActionResult> Create(ReservationViewModel model)
{
    if (!ModelState.IsValid)
    {
        // Log the validation errors
        foreach (var error in ModelState.Values.SelectMany(v => v.Errors))
        {
            Console.WriteLine(error.ErrorMessage);
        }

        var users = await _userManager.Users.ToListAsync();
        var books = await _booksContext.Books.ToListAsync();

        model.Users = users.Select(u => new SelectListItem { Value = u.Id, Text = u.UserName }).ToList();
        model.Books = books.Select(b => new SelectListItem { Value = b.BookID.ToString(), Text = b.Title }).ToList();

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
        // Log the exception
        Console.WriteLine(ex.Message);
        ModelState.AddModelError(string.Empty, "An error occurred while saving the reservation.");
        
        var users = await _userManager.Users.ToListAsync();
        var books = await _booksContext.Books.ToListAsync();

        model.Users = users.Select(u => new SelectListItem { Value = u.Id, Text = u.UserName }).ToList();
        model.Books = books.Select(b => new SelectListItem { Value = b.BookID.ToString(), Text = b.Title }).ToList();

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

            var model = new ReservationViewModel
            {
                ReservationID = reservation.ReservationID,
                UserID = reservation.UserID,
                BookID = reservation.BookID,
                ReservationDate = reservation.ReservationDate,
                DueDate = reservation.DueDate,
                Users = users.Select(u => new SelectListItem { Value = u.Id, Text = u.UserName }).ToList(),
                Books = books.Select(b => new SelectListItem { Value = b.BookID.ToString(), Text = b.Title }).ToList()
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(ReservationViewModel model)
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

            model.Users = users.Select(u => new SelectListItem { Value = u.Id, Text = u.UserName }).ToList();
            model.Books = books.Select(b => new SelectListItem { Value = b.BookID.ToString(), Text = b.Title }).ToList();

            return View(model);
        }
    }
}
