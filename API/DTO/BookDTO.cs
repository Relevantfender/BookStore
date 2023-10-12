using API.Entities;

namespace API.DTO
{
    public class BookDTO
    {
       
        public int Isbn { get; set; }
        public string Title { get; set; }
        public ICollection<AuthorDTO> Authors { get; set; }
        public int NumberOfPages { get; set; }
        public int YearOfPublishing { get; set; }
        public int Quantity { get; set; }
        public string CoverPhoto { get; set; }
        

    }
}
