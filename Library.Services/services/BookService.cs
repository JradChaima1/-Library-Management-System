using Library.Core.Models;
using Library.Core.Interfaces;
using Library.Core.Exceptions;
using Library.Data;
using Microsoft.EntityFrameworkCore;

namespace Library.Services.Services
{
    public class BookService : IBookService
    {
        private readonly IBookRepository _bookRepository;
        private readonly LibraryContext _context;

        public BookService(IBookRepository bookRepository, LibraryContext context)
        {
            _bookRepository = bookRepository;
            _context = context;
        }

        public async Task<IEnumerable<Book>> GetAllBooksAsync()
        {
            return await _bookRepository.GetAllAsync();
        }

        public async Task<Book> GetBookByIdAsync(int id)
        {
            var book = await _bookRepository.GetByIdAsync(id);
            if (book == null)
                throw new NotFoundException($"Book with ID {id} not found.");
            
            return book;
        }

        public async Task<Book> CreateBookAsync(Book book, List<int> authorIds)
        {
            if (book == null)
                throw new ValidationException("Book cannot be null.");
            
            if (string.IsNullOrWhiteSpace(book.Title))
                throw new ValidationException("Book title is required.");
            
            if (string.IsNullOrWhiteSpace(book.ISBN))
                throw new ValidationException("Book ISBN is required.");
            
            if (book.TotalCopies < 0)
                throw new ValidationException("Total copies cannot be negative.");
            
            if (authorIds == null || !authorIds.Any())
                throw new ValidationException("At least one author is required.");

            var existingBook = await _context.Books.FirstOrDefaultAsync(b => b.ISBN == book.ISBN);
            if (existingBook != null)
                throw new ValidationException($"Book with ISBN {book.ISBN} already exists.");

            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                book.AddedDate = DateTime.Now;
                book.IsActive = true;
                book.AvailableCopies = book.TotalCopies;
                
                var addedBook = await _bookRepository.AddAsync(book);
                await _context.SaveChangesAsync();

                foreach (var authorId in authorIds)
                {
                    var author = await _context.Authors.FindAsync(authorId);
                    if (author == null)
                        throw new NotFoundException($"Author with ID {authorId} not found.");
                    
                    var bookAuthor = new BookAuthor
                    {
                        BookId = addedBook.BookId,
                        AuthorId = authorId,
                        AuthorOrder = authorIds.IndexOf(authorId) + 1
                    };
                    _context.BookAuthors.Add(bookAuthor);
                }

                await _context.SaveChangesAsync();
                await transaction.CommitAsync();
                
                return addedBook;
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        }

        public async Task UpdateBookAsync(Book book, List<int> authorIds)
        {
            if (book == null)
                throw new ValidationException("Book cannot be null.");
            
            if (string.IsNullOrWhiteSpace(book.Title))
                throw new ValidationException("Book title is required.");
            
            if (book.TotalCopies < 0)
                throw new ValidationException("Total copies cannot be negative.");

            var existingBook = await _bookRepository.GetByIdAsync(book.BookId);
            if (existingBook == null)
                throw new NotFoundException($"Book with ID {book.BookId} not found.");

            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                await _bookRepository.UpdateAsync(book);

                var existingBookAuthors = _context.BookAuthors.Where(ba => ba.BookId == book.BookId);
                _context.BookAuthors.RemoveRange(existingBookAuthors);

                foreach (var authorId in authorIds)
                {
                    var author = await _context.Authors.FindAsync(authorId);
                    if (author == null)
                        throw new NotFoundException($"Author with ID {authorId} not found.");
                    
                    var bookAuthor = new BookAuthor
                    {
                        BookId = book.BookId,
                        AuthorId = authorId,
                        AuthorOrder = authorIds.IndexOf(authorId) + 1
                    };
                    _context.BookAuthors.Add(bookAuthor);
                }

                await _context.SaveChangesAsync();
                await transaction.CommitAsync();
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        }

        public async Task DeleteBookAsync(int id)
        {
            var book = await _bookRepository.GetByIdAsync(id);
            if (book == null)
                throw new NotFoundException($"Book with ID {id} not found.");

            var hasActiveLoans = await _context.BookLoans
                .AnyAsync(bl => bl.BookId == id && bl.Status == "Active");
            
            if (hasActiveLoans)
                throw new ValidationException("Cannot delete book with active loans.");

            await _bookRepository.DeleteAsync(id);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<Book>> SearchBooksAsync(string searchTerm)
        {
            if (string.IsNullOrWhiteSpace(searchTerm))
                throw new ValidationException("Search term cannot be empty.");

            return await _bookRepository.SearchBooksAsync(searchTerm);
        }

        public async Task<IEnumerable<Book>> GetBooksByCategoryAsync(int categoryId)
        {
            var category = await _context.Categories.FindAsync(categoryId);
            if (category == null)
                throw new NotFoundException($"Category with ID {categoryId} not found.");

            return await _bookRepository.GetBooksByCategoryAsync(categoryId);
        }

        public async Task<IEnumerable<Book>> GetAvailableBooksAsync()
        {
            return await _bookRepository.GetAvailableBooksAsync();
        }

        public async Task<bool> CheckAvailabilityAsync(int bookId)
        {
            var book = await _bookRepository.GetByIdAsync(bookId);
            if (book == null)
                throw new NotFoundException($"Book with ID {bookId} not found.");

            return await _bookRepository.IsBookAvailableAsync(bookId);
        }

        public async Task<Book> GetBookDetailsAsync(int bookId)
        {
            var book = await _bookRepository.GetBookWithDetailsAsync(bookId);
            if (book == null)
                throw new NotFoundException($"Book with ID {bookId} not found.");

            return book;
        }

        public async Task<IEnumerable<Book>> GetPopularBooksAsync(int count)
        {
            if (count <= 0)
                throw new ValidationException("Count must be greater than zero.");

            return await _bookRepository.GetPopularBooksAsync(count);
        }

        public async Task<IEnumerable<Book>> GetNewArrivalsAsync(int days)
        {
            if (days <= 0)
                throw new ValidationException("Days must be greater than zero.");

            return await _bookRepository.GetNewArrivalsAsync(days);
        }
    }
}


