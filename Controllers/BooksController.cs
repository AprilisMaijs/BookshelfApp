namespace BookshelfApp.Controllers
{
    using BookshelfApp.Data;
    using BookshelfApp.Services;
    using Microsoft.AspNetCore.Mvc;
    using System.Security.Claims;
    using System.Threading.Tasks;
    using BookshelfApp.Models;

    public class BooksController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly BookService _bookService;

        public BooksController(ApplicationDbContext context, BookService bookService)
        {
            _context = context;
            _bookService = bookService;
        }

        // GET: Books
        public IActionResult Index()
        {
            // Your logic to get the list of books
            return View();
        }

        // GET: Books/LookupByISBN
        public IActionResult LookupByISBN()
        {
            return View();
        }

        // POST: Books/LookupByISBN
        [HttpPost]
        public async Task<IActionResult> LookupByISBN(string isbn)
        {
            if (string.IsNullOrWhiteSpace(isbn))
            {
                ModelState.AddModelError("ISBN", "Please enter a valid ISBN.");
                return View();
            }

            var book = await _bookService.GetBookByISBNAsync(isbn);
            if (book == null)
            {
                ModelState.AddModelError("ISBN", "No book found with the given ISBN.");
                return View();
            }

            return View(book); // Pass the book information to the view
        }

        // GET: Books/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Books/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Title,Author,ISBN,Description,PublishedDate")] Book book)
        {
            if (ModelState.IsValid)
            {
                // Associate the book with the current user
                book.UserId = User.FindFirstValue(ClaimTypes.NameIdentifier);

                _context.Add(book);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(book);
        }

        // GET: Books/AddByISBN
        public IActionResult AddByISBN()
        {
            return View();
        }

        // POST: Books/AddByISBN
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddByISBN(string isbn)
        {
            if (string.IsNullOrWhiteSpace(isbn))
            {
                ModelState.AddModelError("", "ISBN cannot be empty.");
                return View();
            }

            var book = await _bookService.GetBookByISBNAsync(isbn);

            if (book != null)
            {
                // Associate the book with the current user
                book.UserId = User.FindFirstValue(ClaimTypes.NameIdentifier);

                // Save the book to the database
                _context.Books.Add(book);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }
            else
            {
                ModelState.AddModelError("", "No book found with the provided ISBN.");
                return View();
            }
        }


    }

}
