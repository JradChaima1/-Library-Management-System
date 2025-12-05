using Microsoft.EntityFrameworkCore;
using Library.Core.Models;
using Library.Core.Interfaces;
namespace Library.Data.Repositories
{
    public class MemberRepository : BaseRepository<Member>, IMemberRepository
    {
        public MemberRepository(LibraryContext context) : base(context)
        {
        }

        public async Task<IEnumerable<Member>> GetActiveMembersAsync()
        {
            return await _context.Members
                .Include(m => m.MembershipType)
                .Where(m => m.IsActive && m.ExpiryDate >= DateTime.Now)
                .OrderBy(m => m.LastName)
                .ThenBy(m => m.FirstName)
                .ToListAsync();
        }

        public async Task<IEnumerable<Member>> GetExpiredMembershipsAsync()
        {
            return await _context.Members
                .Include(m => m.MembershipType)
                .Where(m => m.IsActive && m.ExpiryDate < DateTime.Now)
                .OrderByDescending(m => m.ExpiryDate)
                .ToListAsync();
        }

        public async Task<Member> GetMemberByNumberAsync(string membershipNumber)
        {
            return await _context.Members
                .Include(m => m.MembershipType)
                .FirstOrDefaultAsync(m => m.MembershipNumber == membershipNumber);
        }

        public async Task<Member> GetMemberWithLoansAsync(int memberId)
        {
            return await _context.Members
                .Include(m => m.MembershipType)
                .Include(m => m.BookLoans)
                    .ThenInclude(bl => bl.Book)
                .Include(m => m.BookLoans)
                    .ThenInclude(bl => bl.Fine)
                .FirstOrDefaultAsync(m => m.MemberId == memberId);
        }

        public async Task<Member> GetMemberWithFinesAsync(int memberId)
        {
            return await _context.Members
                .Include(m => m.MembershipType)
                .Include(m => m.Fines)
                .FirstOrDefaultAsync(m => m.MemberId == memberId);
        }

        public async Task<IEnumerable<Member>> SearchMembersAsync(string searchTerm)
        {
            return await _context.Members
                .Include(m => m.MembershipType)
                .Where(m => 
                    m.FirstName.Contains(searchTerm) ||
                    m.LastName.Contains(searchTerm) ||
                    m.MembershipNumber.Contains(searchTerm) ||
                    m.Email.Contains(searchTerm) ||
                    m.Phone.Contains(searchTerm))
                .OrderBy(m => m.LastName)
                .ThenBy(m => m.FirstName)
                .ToListAsync();
        }

        public async Task<bool> HasOverdueBooksAsync(int memberId)
        {
            var today = DateTime.Now.Date;
            return await _context.BookLoans
                .AnyAsync(bl => bl.MemberId == memberId && 
                              bl.Status == "Issued" && 
                              bl.DueDate < today && 
                              bl.ReturnDate == null);
        }

        public async Task<bool> HasUnpaidFinesAsync(int memberId)
        {
            return await _context.Fines
                .AnyAsync(f => f.MemberId == memberId && 
                             f.Status == "Pending");
        }

        public async Task<int> GetActiveLoanCountAsync(int memberId)
        {
            return await _context.BookLoans
                .CountAsync(bl => bl.MemberId == memberId && 
                                bl.Status == "Issued" && 
                                bl.ReturnDate == null);
        }
    }
}