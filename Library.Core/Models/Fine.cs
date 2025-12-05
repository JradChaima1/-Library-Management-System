namespace Library.Core.Models{
public class Fine: BaseEntity
{

    public int FineId { get; set; }
    
 
    public int MemberId { get; set; }
    public int BookLoanId { get; set; }
    
 
    public decimal Amount { get; set; }
    public string Reason { get; set; } 
    public DateTime IssueDate { get; set; }
    public DateTime? PaidDate { get; set; }
    public string Status { get; set; }  /
    public decimal PaidAmount { get; set; }
    public int? WaivedByStaffId { get; set; }
    public string WaiverReason { get; set; }
    public string PaymentMethod { get; set; }  
    public string TransactionId { get; set; }
    
 
    public Member Member { get; set; }
    public BookLoan BookLoan { get; set; }
    public Staff WaivedByStaff { get; set; }
}
}