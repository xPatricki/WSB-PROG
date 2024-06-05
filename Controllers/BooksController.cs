using biblioteka.Data;
using biblioteka.Models.DBEntities;
using biblioteka.ViewModels;
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
        public IActionResult Index(int? publishedYear, string bookTitle, string authorName, int page = 1, int pageSize = 10)
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

            var totalItems = books.Count();
            var pagerOptions = new PagerOptions
            {
                CurrentPage = page,
                PageSize = pageSize,
                TotalItems = totalItems
            };

            var paginatedBooks = books.Skip((page - 1) * pageSize).Take(pageSize).ToList();

            var pagerViewModel = new PagerViewModel(page, pageSize, totalItems, new[] { 5, 10, 20 });

            ViewData["Pager"] = pagerViewModel;
            ViewBag.AuthorName = authorName;
            ViewBag.BookTitle = bookTitle;

            return View(paginatedBooks);
        }

        // other actions remain the same...
    }
}
