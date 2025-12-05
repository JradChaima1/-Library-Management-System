namespace Library.Core.Models{
public class Role: BaseEntity
{
  
    public int RoleId { get; set; }
    

    public string Name { get; set; }  
    public string Description { get; set; }
    
  
    public ICollection<User> Users { get; set; }
}
}