using biblioteka.Data;
using biblioteka.Models.DBEntities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
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
        public IActionResult Index(int? publishedYear, string bookTitle, string authorName)
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
            if (!string.IsNullOrEmpty(bookTitle))
            {
                books = books.Where(b => b.Title.Contains(bookTitle));
            }
            if (!string.IsNullOrEmpty(authorName))
            {
                books = books.Where(b => b.Author.Contains(authorName));
            }

            ViewBag.AuthorName = authorName; // Preserve the input value
            ViewBag.BookTitle = bookTitle; // Preserve the input value

            return View(books.ToList());
        }

        // other actions remain the same...
    }
}
