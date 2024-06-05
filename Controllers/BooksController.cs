using biblioteka.Data;
using biblioteka.Models.DBEntities;
using biblioteka.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

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
        public async Task<IActionResult> Index(int? publishedYear, string bookTitle, string authorName, int page = 1, int pageSize = 10)
        {
            var books = _context.Books.AsQueryable();

            if (publishedYear.HasValue)
            {
                books = books.Where(b => b.PublishedYear == publishedYear.Value);
            }
            if (!string.IsNullOrEmpty(bookTitle))
            {
                books = books.Where(b => b.Title.Contains(bookTitle));
            }
            if (!string.IsNullOrEmpty(authorName))
            {
                books = books.Where(b => b.Author.Contains(authorName));
            }

            var totalItems = await books.CountAsync();
            var pagerOptions = new PagerOptions
            {
                CurrentPage = page,
                PageSize = pageSize,
                TotalItems = totalItems
            };

            var paginatedBooks = await books.Skip((page - 1) * pageSize).Take(pageSize).ToListAsync();

            var pagerViewModel = new PagerViewModel(page, pageSize, totalItems, new[] { 5, 10, 20 });

            ViewData["Pager"] = pagerViewModel;
            ViewBag.AuthorName = authorName;
            ViewBag.BookTitle = bookTitle;

            return View(paginatedBooks);
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
            if(!ModelState.IsValid)
            {
                TempData["errorMessage"] = "Model data is not valid.";
                return View(book);
            }
            try
            {
                _context.Books.Add(book);
                _context.SaveChanges();
                TempData["successMessage"] = "Employee created successfully.";
                return RedirectToAction("Index");
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
        [ValidateAntiForgeryToken]
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

        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            var book = await _context.Books.FirstOrDefaultAsync(b => b.BookID == id);
            if (book == null)
            {
                return NotFound();
            }
            return View(book);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var book = await _context.Books.FindAsync(id);
            if (book != null)
            {
                _context.Books.Remove(book);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }


        private bool BookExists(int id)
        {
            return _context.Books.Any(e => e.BookID == id);
        }
    }
}
