using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;

namespace ExpenseTracker.DTO
{
    public class ExpenseQueryDto 
    {   
        [AllowNull]
        public string? SearchValue { get; set; }
        [DefaultValue("ASC")]
        public string? SortOrder { get; set; }
        [DefaultValue(10)]
        public required int PageSize {get;set;}
        [DefaultValue(1)]
        public required int PageNumber {get;set;}
        public string? UserId {get;set;}
    }
}