using Microsoft.EntityFrameworkCore;
using Library.Core.Models;
using Library.Core.Interfaces;
namespace Library.Data.Repositories
{
    public interface IStaffRepository : IRepository<Staff>
    {
        Task<IEnumerable<Staff>> GetActiveStaffAsync();
        Task<IEnumerable<Staff>> GetStaffByPositionAsync(string position);
        Task<Staff> GetStaffWithDetailsAsync(int staffId);
    }

    public class StaffRepository : BaseRepository<Staff>, IStaffRepository
    {
        public StaffRepository(LibraryContext context) : base(context)
        {
        }

        public async Task<IEnumerable<Staff>> GetActiveStaffAsync()
        {
            return await _context.Staff
                .Where(s => s.IsActive)
                .OrderBy(s => s.LastName)
                .ThenBy(s => s.FirstName)
                .ToListAsync();
        }

        public async Task<IEnumerable<Staff>> GetStaffByPositionAsync(string position)
        {
            return await _context.Staff
                .Where(s => s.IsActive && s.Position == position)
                .OrderBy(s => s.LastName)
                .ThenBy(s => s.FirstName)
                .ToListAsync();
        }

        public async Task<Staff> GetStaffWithDetailsAsync(int staffId)
        {
            return await _context.Staff
                .Include(s => s.IssuedLoans)
                .Include(s => s.ReturnedLoans)
                .Include(s => s.FulfilledReservations)
                .Include(s => s.WaivedFines)
                .FirstOrDefaultAsync(s => s.StaffId == staffId);
        }
    }
}