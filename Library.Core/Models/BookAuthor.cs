namespace Library.Core.Models
{
public class BookAuthor: BaseEntity
{
  
    public int BookId { get; set; }
    public int AuthorId { get; set; }
    
 
    public int AuthorOrder { get; set; }  
    
   
    public Book Book { get; set; }
    public Author Author { get; set; }
}
}