using System.Runtime.Serialization;


namespace API.DTO.Filters
{
    public enum SortBy
    {
        
        [EnumMember(Value = "asc")]
        ASC,
        [EnumMember(Value = "desc")]
        DESC
    }
}
