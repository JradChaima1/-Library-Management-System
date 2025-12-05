using Microsoft.EntityFrameworkCore;
using Library.Core.Models;
using Library.Core.Interfaces;
namespace Library.Data.Repositories
{
    public class ReservationRepository : BaseRepository<Reservation>, IReservationRepository
    {
        public ReservationRepository(LibraryContext context) : base(context)
        {
        }

        public async Task<IEnumerable<Reservation>> GetActiveReservationsAsync()
        {
            return await _context.Reservations
                .Include(r => r.Member)
                .Include(r => r.Book)
                .Where(r => r.Status == "Active" && r.ExpiryDate >= DateTime.Now)
                .OrderBy(r => r.ReservationDate)
                .ToListAsync();
        }

        public async Task<IEnumerable<Reservation>> GetReservationsByMemberAsync(int memberId)
        {
            return await _context.Reservations
                .Include(r => r.Book)
                    .ThenInclude(b => b.Category)
                .Where(r => r.MemberId == memberId)
                .OrderByDescending(r => r.ReservationDate)
                .ToListAsync();
        }

        public async Task<IEnumerable<Reservation>> GetReservationsByBookAsync(int bookId)
        {
            return await _context.Reservations
                .Include(r => r.Member)
                .Where(r => r.BookId == bookId && 
                          r.Status == "Active" && 
                          r.ExpiryDate >= DateTime.Now)
                .OrderBy(r => r.ReservationDate)
                .ToListAsync();
        }

        public async Task<IEnumerable<Reservation>> GetExpiredReservationsAsync()
        {
            return await _context.Reservations
                .Include(r => r.Member)
                .Include(r => r.Book)
                .Where(r => r.Status == "Active" && r.ExpiryDate < DateTime.Now)
                .ToListAsync();
        }

        public async Task<Reservation> GetOldestActiveReservationForBookAsync(int bookId)
        {
            return await _context.Reservations
                .Include(r => r.Member)
                .Where(r => r.BookId == bookId && 
                          r.Status == "Active" && 
                          r.ExpiryDate >= DateTime.Now)
                .OrderBy(r => r.ReservationDate)
                .FirstOrDefaultAsync();
        }

        public async Task<int> GetReservationQueuePositionAsync(int reservationId)
        {
            var reservation = await GetByIdAsync(reservationId);
            if (reservation == null || reservation.Status != "Active")
                return -1;

            var position = await _context.Reservations
                .CountAsync(r => r.BookId == reservation.BookId && 
                               r.Status == "Active" && 
                               r.ReservationDate < reservation.ReservationDate && 
                               r.ExpiryDate >= DateTime.Now);

            return position + 1; 
        }

        public async Task<bool> HasActiveReservationAsync(int memberId, int bookId)
        {
            return await _context.Reservations
                .AnyAsync(r => r.MemberId == memberId && 
                             r.BookId == bookId && 
                             r.Status == "Active" && 
                             r.ExpiryDate >= DateTime.Now);
        }
    }
}