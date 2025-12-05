using Microsoft.EntityFrameworkCore;
using Library.Core.Models;
using Library.Core.Interfaces;
namespace Library.Data.Repositories
{

    public class AuthorRepository : BaseRepository<Author>, IAuthorRepository
    {
        public AuthorRepository(LibraryContext context) : base(context)
        {
        }

        public async Task<IEnumerable<Author>> SearchAuthorsAsync(string searchTerm)
        {
            return await _context.Authors
                .Where(a => 
                    a.FirstName.Contains(searchTerm) ||
                    a.LastName.Contains(searchTerm) ||
                    a.Country.Contains(searchTerm) ||
                    (a.FirstName + " " + a.LastName).Contains(searchTerm))
                .OrderBy(a => a.LastName)
                .ThenBy(a => a.FirstName)
                .ToListAsync();
        }

        public async Task<IEnumerable<Author>> GetAuthorsByCountryAsync(string country)
        {
            return await _context.Authors
                .Where(a => a.Country == country)
                .OrderBy(a => a.LastName)
                .ThenBy(a => a.FirstName)
                .ToListAsync();
        }

        public async Task<Author> GetAuthorWithBooksAsync(int authorId)
        {
            return await _context.Authors
                .Include(a => a.BookAuthors)
                    .ThenInclude(ba => ba.Book)
                        .ThenInclude(b => b.Category)
                .FirstOrDefaultAsync(a => a.AuthorId == authorId);
        }

        public async Task<IEnumerable<Author>> GetPopularAuthorsAsync(int count)
        {
            return await _context.Authors
                .Select(a => new
                {
                    Author = a,
                    BookCount = a.BookAuthors.Count
                })
                .OrderByDescending(x => x.BookCount)
                .Take(count)
                .Select(x => x.Author)
                .ToListAsync();
        }
    }
}