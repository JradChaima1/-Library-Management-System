public class Author
{

    public int AuthorId { get; set; }
    

    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Biography { get; set; }
    public string Country { get; set; }
    public DateTime? BirthDate { get; set; }
    public string Email { get; set; }
    public string Website { get; set; }
    
   
    public ICollection<BookAuthor> BookAuthors { get; set; }  
}
