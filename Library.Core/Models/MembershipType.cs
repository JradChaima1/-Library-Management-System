public class MembershipType
{
 
    public int MembershipTypeId { get; set; }
    
 
    public string Name { get; set; }  
    public string Description { get; set; }
    
   
    public int MaxBooksAllowed { get; set; }
    public int LoanDurationDays { get; set; }
    public int MaxRenewals { get; set; }
    public decimal AnnualFee { get; set; }
    public decimal FinePerDay { get; set; }
    public decimal MaxFineAmount { get; set; }
    public int GracePeriodDays { get; set; }
    
  
    public ICollection<Member> Members { get; set; }
}
