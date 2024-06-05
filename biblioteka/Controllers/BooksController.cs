using biblioteka.Data;
using biblioteka.Models.DBEntities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Linq;

namespace biblioteka.Controllers
{
    public class BooksController : Controller
    {
        private readonly BooksDbContext _context;

        public BooksController(BooksDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult Index(int? publishedYear)
        {
            var books = _context.Books.AsQueryable();
            var years = _context.Books
                                .Select(b => b.PublishedYear)
                                .Distinct()
                                .OrderBy(y => y)
                                .ToList();

            if (publishedYear.HasValue)
            {
                books = books.Where(b => b.PublishedYear == publishedYear.Value);
            }

            ViewBag.Years = new SelectList(years);

            return View(books.ToList());

        }

        [HttpGet]
        public IActionResult Details(int id)
        {
            var book = _context.Books.Find(id);
            if (book == null)
            {
                TempData["errorMessage"] = "Book not found.";
                return RedirectToAction("Index");
            }
            return View(book);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(Books book)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    _context.Books.Add(book);
                    _context.SaveChanges();

                    TempData["successMessage"] = "Book created successfully.";
                    return RedirectToAction("Index");
                }
                else
                {
                    TempData["errorMessage"] = "Model data is not valid.";
                    return View(book);
                }
            }
            catch (Exception ex)
            {
                TempData["errorMessage"] = ex.Message;
                return View(book);
            }
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            var book = _context.Books.Find(id);
            if (book == null)
            {
                TempData["errorMessage"] = "Book not found.";
                return RedirectToAction("Index");
            }
            return View(book);
        }

        [HttpPost]
        public IActionResult Edit(Books book)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    _context.Books.Update(book);
                    _context.SaveChanges();

                    TempData["successMessage"] = "Book updated successfully.";
                    return RedirectToAction("Index");
                }
                else
                {
                    TempData["errorMessage"] = "Model data is not valid.";
                    return View(book);
                }
            }
            catch (Exception ex)
            {
                TempData["errorMessage"] = ex.Message;
                return View(book);
            }
        }

        [HttpPost]
        public IActionResult DeleteConfirmed(int id)
        {
            try
            {
                var book = _context.Books.Find(id);
                if (book == null)
                {
                    TempData["errorMessage"] = "Book not found.";
                    return RedirectToAction("Index");
                }

                _context.Books.Remove(book);
                _context.SaveChanges();

                TempData["successMessage"] = "Book deleted successfully.";
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
