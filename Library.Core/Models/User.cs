namespace Library.Core.Models{

public class User: BaseEntity
{
 
    public int UserId { get; set; }
    
  
    public string Username { get; set; }
    public string PasswordHash { get; set; }
    public string Email { get; set; }
    

    public int RoleId { get; set; }
    public int? StaffId { get; set; }
    public int? MemberId { get; set; }
    
   
    public bool IsActive { get; set; }
    public DateTime CreatedDate { get; set; }
    public DateTime? LastLoginDate { get; set; }
    
    public Role Role { get; set; }
    public Staff Staff { get; set; }
    public Member Member { get; set; }
}
}