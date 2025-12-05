public class BookLoan
{
   
    public int BookLoanId { get; set; }
    

    public int MemberId { get; set; }
    public int BookId { get; set; }
    public int IssuedByStaffId { get; set; }
    public int? ReturnedByStaffId { get; set; }
    

    public DateTime IssueDate { get; set; }
    public DateTime DueDate { get; set; }
    public DateTime? ReturnDate { get; set; }
    public string Status { get; set; }  
    public int RenewalCount { get; set; }
    public string Notes { get; set; }

    public Member Member { get; set; }
    public Book Book { get; set; }
    public Staff IssuedByStaff { get; set; }
    public Staff ReturnedByStaff { get; set; }
    public Fine Fine { get; set; }  
}
