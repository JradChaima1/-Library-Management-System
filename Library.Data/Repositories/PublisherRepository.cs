using Microsoft.EntityFrameworkCore;
using Library.Core.Models;
using Library.Core.Interfaces;
namespace Library.Data.Repositories
{
    public class PublisherRepository : BaseRepository<Publisher>, IPublisherRepository
    {
        public PublisherRepository(LibraryContext context) : base(context)
        {
        }

        public async Task<IEnumerable<Publisher>> SearchPublishersAsync(string searchTerm)
        {
            return await _context.Publishers
                .Where(p => 
                    p.Name.Contains(searchTerm) ||
                    p.City.Contains(searchTerm) ||
                    p.Country.Contains(searchTerm))
                .OrderBy(p => p.Name)
                .ToListAsync();
        }

        public async Task<Publisher> GetPublisherWithBooksAsync(int publisherId)
        {
            return await _context.Publishers
                .Include(p => p.Books.Where(b => b.IsActive))
                .FirstOrDefaultAsync(p => p.PublisherId == publisherId);
        }

        public async Task<IEnumerable<Publisher>> GetPublishersByCountryAsync(string country)
        {
            return await _context.Publishers
                .Where(p => p.Country == country)
                .OrderBy(p => p.Name)
                .ToListAsync();
        }
    }
}