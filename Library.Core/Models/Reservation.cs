namespace Library.Core.Models{
public class Reservation: BaseEntity
{
   
    public int ReservationId { get; set; }
    
   
    public int MemberId { get; set; }
    public int BookId { get; set; }
    

    public DateTime ReservationDate { get; set; }
    public DateTime ExpiryDate { get; set; }
    public string Status { get; set; }  // Active, Fulfilled, Cancelled, Expired
    public DateTime? FulfilledDate { get; set; }
    public int? FulfilledByStaffId { get; set; }
    public string CancellationReason { get; set; }
    

    public Member Member { get; set; }
    public Book Book { get; set; }
    public Staff FulfilledByStaff { get; set; }
}
}