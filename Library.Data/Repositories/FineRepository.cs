using Microsoft.EntityFrameworkCore;
using Library.Core.Models;
using Library.Core.Interfaces;
namespace Library.Data.Repositories
{
    public class FineRepository : BaseRepository<Fine>, IFineRepository
    {
        public FineRepository(LibraryContext context) : base(context)
        {
        }

        public async Task<IEnumerable<Fine>> GetUnpaidFinesAsync()
        {
            return await _context.Fines
                .Include(f => f.Member)
                .Include(f => f.BookLoan)
                    .ThenInclude(bl => bl.Book)
                .Where(f => f.Status == "Pending")
                .OrderBy(f => f.IssueDate)
                .ToListAsync();
        }

        public async Task<IEnumerable<Fine>> GetFinesByMemberAsync(int memberId)
        {
            return await _context.Fines
                .Include(f => f.BookLoan)
                    .ThenInclude(bl => bl.Book)
                .Where(f => f.MemberId == memberId)
                .OrderByDescending(f => f.IssueDate)
                .ToListAsync();
        }

        public async Task<IEnumerable<Fine>> GetFinesByStatusAsync(string status)
        {
            return await _context.Fines
                .Include(f => f.Member)
                .Include(f => f.BookLoan)
                .Where(f => f.Status == status)
                .OrderByDescending(f => f.IssueDate)
                .ToListAsync();
        }

        public async Task<decimal> GetTotalUnpaidFinesByMemberAsync(int memberId)
        {
            return await _context.Fines
                .Where(f => f.MemberId == memberId && f.Status == "Pending")
                .SumAsync(f => f.Amount - f.PaidAmount);
        }

        public async Task<IEnumerable<Fine>> GetFinesByDateRangeAsync(DateTime startDate, DateTime endDate)
        {
            return await _context.Fines
                .Include(f => f.Member)
                .Include(f => f.BookLoan)
                .Where(f => f.IssueDate >= startDate && f.IssueDate <= endDate)
                .OrderBy(f => f.IssueDate)
                .ToListAsync();
        }

        public async Task<decimal> GetTotalCollectedFinesAsync(DateTime startDate, DateTime endDate)
        {
            return await _context.Fines
                .Where(f => f.PaidDate.HasValue && 
                          f.PaidDate >= startDate && 
                          f.PaidDate <= endDate && 
                          f.Status == "Paid")
                .SumAsync(f => f.PaidAmount);
        }
    }
}