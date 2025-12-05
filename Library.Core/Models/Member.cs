public class Member
{
 
    public int MemberId { get; set; }
    
   
    public string MembershipNumber { get; set; }  
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }
    public string Phone { get; set; }
    public string Address { get; set; }
    public DateTime DateOfBirth { get; set; }
    public string IdCardNumber { get; set; }
    

    public int MembershipTypeId { get; set; }
    public DateTime JoinDate { get; set; }
    public DateTime ExpiryDate { get; set; }
    public bool IsActive { get; set; }
    public string PhotoPath { get; set; }
    
   
    public MembershipType MembershipType { get; set; }
    public ICollection<BookLoan> BookLoans { get; set; }
    public ICollection<Reservation> Reservations { get; set; }
    public ICollection<Fine> Fines { get; set; }
}
