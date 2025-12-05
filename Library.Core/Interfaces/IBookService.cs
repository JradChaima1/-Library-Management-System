using Library.Core.Models;

namespace Library.Core.Interfaces
{
public interface IBookService
{
Task<IEnumerable<Book>> GetAllBooksAsync();
Task<Book> GetBookByIdAsync(int id);
Task<Book> CreateBookAsync(Book book, List<int> authorIds);
Task UpdateBookAsync(Book book, List<int> authorIds);
Task DeleteBookAsync(int id);
Task<IEnumerable<Book>> SearchBooksAsync(string searchTerm);
Task<IEnumerable<Book>> GetBooksByCategoryAsync(int categoryId);
Task<IEnumerable<Book>> GetAvailableBooksAsync();
Task<bool> CheckAvailabilityAsync(int bookId);
Task<Book> GetBookDetailsAsync(int bookId);
Task<IEnumerable<Book>> GetPopularBooksAsync(int count);
Task<IEnumerable<Book>> GetNewArrivalsAsync(int days);

    }
    }