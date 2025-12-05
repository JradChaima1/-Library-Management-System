using Library.Core.Models;

namespace Library.Core.Interfaces;
{
 
public Interface IReservationRepository: IRepository <Reservation>{  
Task<IEnumerable<Reservation>> GetActiveReservationsAsync();
Task<IEnumerable<Reservation>> GetReservationsByMemberAsync(int memberId);
Task<IEnumerable<Reservation>> GetReservationsByBookAsync(int bookId);
Task<IEnumerable<Reservation>> GetExpiredReservationsAsync();
Task<Reservation> GetOldestActiveReservationForBookAsync(int bookId);
Task<int> GetReservationQueuePositionAsync(int reservationId);
Task<bool> HasActiveReservationAsync(int memberId, int bookId);
}
}