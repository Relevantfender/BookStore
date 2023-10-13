using System.ComponentModel.DataAnnotations;

namespace API.DTO.Filters
{
    public class PageRequest
    {
        public string Title { get; set; }
        public string Author { get; set; }
        
        public SortBy SortByTitle { get; set; }
        
        public SortBy SortByAuthor { get; set; }
        public int StartPage { get; set; } = 1;
        public int LimitPage { get; set; } = 30;

        //public static readonly List<string> SortByValues = new List<string> { "asc", "desc" };

    }
}
