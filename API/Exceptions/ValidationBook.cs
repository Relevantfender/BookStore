namespace API.Exceptions
{
    public class ValidationBook
    {
        public int StatusCode { get; set; }
        public List<string> ResponseMessage { get; set; } = new List<string>();
    }
}