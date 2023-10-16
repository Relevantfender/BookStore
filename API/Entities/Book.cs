namespace API.Entities
{
    public class Book
    {
        public int Id { get; set; }
        public int Isbn { get; set; }
        public string Title { get; set; }
        public ICollection<Author> Authors { get; set; }

        public int NumberOfPages { get; set; }
        public int YearOfPublishing { get; set; }
        public int Quantity { get; set; }
        public string CoverPhoto { get; set; }
     }
}
