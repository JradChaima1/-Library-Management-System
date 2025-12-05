using Microsoft.EntityFrameworkCore;
using Library.Core.Models;
using Library.Core.Interfaces;
namespace Library.Data.Repositories
{

    public class CategoryRepository : BaseRepository<Category>, ICategoryRepository
    {
        public CategoryRepository(LibraryContext context) : base(context)
        {
        }

        public async Task<IEnumerable<Category>> GetMainCategoriesAsync()
        {
            return await _context.Categories
                .Where(c => c.ParentCategoryId == null)
                .OrderBy(c => c.Name)
                .ToListAsync();
        }

        public async Task<IEnumerable<Category>> GetSubCategoriesAsync(int parentCategoryId)
        {
            return await _context.Categories
                .Where(c => c.ParentCategoryId == parentCategoryId)
                .OrderBy(c => c.Name)
                .ToListAsync();
        }

        public async Task<Category> GetCategoryWithBooksAsync(int categoryId)
        {
            return await _context.Categories
                .Include(c => c.Books.Where(b => b.IsActive))
                .Include(c => c.SubCategories)
                .FirstOrDefaultAsync(c => c.CategoryId == categoryId);
        }

        public async Task<bool> HasBooksAsync(int categoryId)
        {
            return await _context.Books
                .AnyAsync(b => b.CategoryId == categoryId && b.IsActive);
        }
    }
}