using Microsoft.EntityFrameworkCore;
using Library.Core.Models;
using Library.Core.Interfaces;
namespace Library.Data.Repositories
{
    public class BookLoanRepository : BaseRepository<BookLoan>, IBookLoanRepository
    {
        public BookLoanRepository(LibraryContext context) : base(context)
        {
        }

        public async Task<IEnumerable<BookLoan>> GetActiveLoansByMemberAsync(int memberId)
        {
            return await _context.BookLoans
                .Include(bl => bl.Book)
                    .ThenInclude(b => b.Category)
                .Include(bl => bl.Member)
                .Include(bl => bl.IssuedByStaff)
                .Where(bl => bl.MemberId == memberId && 
                           bl.Status == "Issued" && 
                           bl.ReturnDate == null)
                .OrderByDescending(bl => bl.IssueDate)
                .ToListAsync();
        }

        public async Task<IEnumerable<BookLoan>> GetOverdueLoansAsync()
        {
            var today = DateTime.Now.Date;
            return await _context.BookLoans
                .Include(bl => bl.Book)
                .Include(bl => bl.Member)
                .Where(bl => bl.Status == "Issued" && 
                           bl.DueDate < today && 
                           bl.ReturnDate == null)
                .OrderBy(bl => bl.DueDate)
                .ToListAsync();
        }

        public async Task<int> GetOverdueDaysAsync(int loanId)
        {
            var loan = await GetByIdAsync(loanId);
            if (loan == null || loan.ReturnDate.HasValue || loan.Status != "Issued")
                return 0;

            var today = DateTime.Now.Date;
            var dueDate = loan.DueDate.Date;
            
            if (today <= dueDate)
                return 0;

            var overdueDays = (today - dueDate).Days;
            
        
            var member = await _context.Members
                .Include(m => m.MembershipType)
                .FirstOrDefaultAsync(m => m.MemberId == loan.MemberId);
            
            if (member?.MembershipType?.GracePeriodDays >= overdueDays)
                return 0;

            return overdueDays - (member?.MembershipType?.GracePeriodDays ?? 0);
        }

        public async Task<IEnumerable<BookLoan>> GetDueSoonLoansAsync(int days)
        {
            var today = DateTime.Now.Date;
            var warningDate = today.AddDays(days);
            
            return await _context.BookLoans
                .Include(bl => bl.Book)
                .Include(bl => bl.Member)
                .Where(bl => bl.Status == "Issued" && 
                           bl.ReturnDate == null &&
                           bl.DueDate >= today && 
                           bl.DueDate <= warningDate)
                .OrderBy(bl => bl.DueDate)
                .ToListAsync();
        }
    }
}