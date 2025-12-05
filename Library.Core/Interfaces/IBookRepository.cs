using Libray.Core.Models;
namespace Libray.Core.Interfaces 
{
    public interface IBookRepository: IRepository<Book> {
Task<IEnumerable<Book>> SearchBooksAsync(string searchTerm);
Task<IEnumerable<Book>> GetBooksByCategoryAsync(int categoryId);
Task<IEnumerable<Book>> GetBooksByAuthorAsync(int authorId);
Task<IEnumerable<Book>> GetBooksByPublisherAsync(int publisherId);
Task<IEnumerable<Book>> GetAvailableBooksAsync();
Task<Book> GetBookByISBNAsync(string isbn);
Task<Book> GetBookWithDetailsAsync(int bookId); 
Task<IEnumerable<Book>> GetPopularBooksAsync(int count);
Task<IEnumerable<Book>> GetNewArrivalsAsync(int days);
Task<bool> IsBookAvailableAsync(int bookId);
Task UpdateAvailableCopiesAsync(int bookId, int change);

    }
}