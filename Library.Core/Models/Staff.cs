namespace Library.Core.Models{
public class Staff: BaseEntity
{

    public int StaffId { get; set; }
    
  
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }
    public string Phone { get; set; }
    public string Position { get; set; }  
    public string Department { get; set; }
    public DateTime HireDate { get; set; }
    public decimal Salary { get; set; }
    public bool IsActive { get; set; }
    

    public ICollection<BookLoan> IssuedLoans { get; set; }
    public ICollection<BookLoan> ReturnedLoans { get; set; }
    public ICollection<Reservation> FulfilledReservations { get; set; }
    public ICollection<Fine> WaivedFines { get; set; }
}
}