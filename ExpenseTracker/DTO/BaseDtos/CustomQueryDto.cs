using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;

namespace ExpenseTracker.DTO
{
    public class CustomQueryDto 
    {   
        [AllowNull]
        public String? SearchValue { get; set; }
        [DefaultValue("ASC")]
        public String? SortOrder { get; set; }
        [DefaultValue(10)]
        public required int PageSize {get;set;}
        [DefaultValue(1)]
        public required int PageNumber {get;set;}
    }
}