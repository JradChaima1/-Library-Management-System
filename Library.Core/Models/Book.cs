public class Book
{
 
    public int BookId { get; set; }
    

    public string ISBN { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public int PublicationYear { get; set; }
    public string Edition { get; set; }
    public string Language { get; set; }
    public int TotalPages { get; set; }
   
    public int TotalCopies { get; set; }
    public int AvailableCopies { get; set; }
    public decimal Price { get; set; }
    

    public int CategoryId { get; set; }
    public int PublisherId { get; set; }
    
 
    public DateTime AddedDate { get; set; }
    public bool IsActive { get; set; }
    

    public Category Category { get; set; }
    public Publisher Publisher { get; set; }
    public ICollection<BookAuthor> BookAuthors { get; set; }  // Many-to-Many
    public ICollection<BookLoan> BookLoans { get; set; }
    public ICollection<Reservation> Reservations { get; set; }
}
