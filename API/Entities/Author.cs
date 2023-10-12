namespace API.Entities
{
    public class Author
    {
        public int Id { get; set; }
        public string NameOfAuthor { get; set; }
        public string LastNameOfAuthor { get; set; }
        public DateOnly DateOfBirth{ get; set; }
        public ICollection<Book> Books { get; set; }
    }
}
