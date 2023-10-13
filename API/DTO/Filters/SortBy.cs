using System.Runtime.Serialization;


namespace API.DTO.Filters
{
    public enum SortBy
    {
        [EnumMember(Value = "ASC")]
        ASC,
        [EnumMember(Value = "DESC")]
        DESC
    }
}
