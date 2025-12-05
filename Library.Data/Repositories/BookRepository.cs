using Microsoft.EntityFrameworkCore;
using Library.Core.Models;
using Library.Core.Interfaces;
namespace Library.Data.Repositories
{
    public class BookRepository : BaseRepository<Book>, IBookRepository
    {
        public BookRepository(LibraryContext context) : base(context)
        {
        }

        public async Task<IEnumerable<Book>> SearchBooksAsync(string searchTerm)
        {
            return await _context.Books
                .Include(b => b.Category)
                .Include(b => b.Publisher)
                .Include(b => b.BookAuthors)
                    .ThenInclude(ba => ba.Author)
                .Where(b => b.IsActive && (
                    b.Title.Contains(searchTerm) ||
                    b.ISBN.Contains(searchTerm) ||
                    b.Description.Contains(searchTerm) ||
                    b.BookAuthors.Any(ba => ba.Author.FirstName.Contains(searchTerm) ||
                                           ba.Author.LastName.Contains(searchTerm))))
                .ToListAsync();
        }

        public async Task<IEnumerable<Book>> GetBooksByCategoryAsync(int categoryId)
        {
            return await _context.Books
                .Include(b => b.Category)
                .Include(b => b.Publisher)
                .Where(b => b.IsActive && b.CategoryId == categoryId)
                .ToListAsync();
        }

        public async Task<IEnumerable<Book>> GetAvailableBooksAsync()
        {
            return await _context.Books
                .Include(b => b.Category)
                .Where(b => b.IsActive && b.AvailableCopies > 0)
                .ToListAsync();
        }

        public async Task<Book> GetBookByISBNAsync(string isbn)
        {
            return await _context.Books
                .Include(b => b.Category)
                .Include(b => b.Publisher)
                .Include(b => b.BookAuthors)
                    .ThenInclude(ba => ba.Author)
                .FirstOrDefaultAsync(b => b.ISBN == isbn);
        }

        public async Task<Book> GetBookWithDetailsAsync(int bookId)
        {
            return await _context.Books
                .Include(b => b.Category)
                .Include(b => b.Publisher)
                .Include(b => b.BookAuthors)
                    .ThenInclude(ba => ba.Author)
                .FirstOrDefaultAsync(b => b.BookId == bookId);
        }

        public async Task<bool> IsBookAvailableAsync(int bookId)
        {
            var book = await _context.Books
                .FirstOrDefaultAsync(b => b.BookId == bookId);
            return book?.AvailableCopies > 0;
        }

        public async Task UpdateAvailableCopiesAsync(int bookId, int change)
        {
            var book = await GetByIdAsync(bookId);
            if (book != null)
            {
                book.AvailableCopies += change;
                if (book.AvailableCopies < 0)
                    book.AvailableCopies = 0;
                if (book.AvailableCopies > book.TotalCopies)
                    book.AvailableCopies = book.TotalCopies;
                
                await UpdateAsync(book);
            }
        }

        public async Task<IEnumerable<Book>> GetPopularBooksAsync(int count)
        {
            return await _context.Books
                .Include(b => b.Category)
                .Where(b => b.IsActive)
                .OrderByDescending(b => b.BookLoans.Count)
                .Take(count)
                .ToListAsync();
        }

        public async Task<IEnumerable<Book>> GetNewArrivalsAsync(int days)
        {
            var dateThreshold = DateTime.Now.AddDays(-days);
            return await _context.Books
                .Include(b => b.Category)
                .Where(b => b.IsActive && b.AddedDate >= dateThreshold)
                .OrderByDescending(b => b.AddedDate)
                .ToListAsync();
        }
    }


}