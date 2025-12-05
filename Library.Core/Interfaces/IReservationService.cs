
namespace Library.Core.Interfaces
{
public interface IReservationService
{

Task<Reservation> CreateReservationAsync(int memberId, int bookId);
Task CancelReservationAsync(int reservationId, string reason);
Task<Reservation> FulfillReservationAsync(int reservationId, int staffId);
Task<IEnumerable<Reservation>> GetMemberReservationsAsync(int memberId);
Task<IEnumerable<Reservation>> GetActiveReservationsAsync();
Task ProcessExpiredReservationsAsync();
Task<bool> CanReserveBookAsync(int memberId, int bookId);
Task<int> GetQueuePositionAsync(int reservationId);
Task NotifyReservationReadyAsync(int reservationId);
}
}