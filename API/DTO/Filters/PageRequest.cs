using System.ComponentModel.DataAnnotations;

namespace API.DTO.Filters
{
    public class PageRequest
    {
        public string Title { get; set; }
        public string Author { get; set; }

        public SortBy? SortByTitle { get; set; }

        public SortBy? SortByAuthor { get; set; }
        public short StartPage { get; set; } = 1;
        public short LimitPage { get; set; } = 30;

        public override bool Equals(object obj)
        {
            return obj is PageRequest request &&
                   Title == request.Title &&
                   Author == request.Author &&
                   SortByTitle == request.SortByTitle &&
                   SortByAuthor == request.SortByAuthor &&
                   StartPage == request.StartPage &&
                   LimitPage == request.LimitPage;
        }

        public override int GetHashCode()
        {
            throw new NotImplementedException();
        }
    }
}
