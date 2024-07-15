using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;

namespace ExpenseTracker.DTO
{
    public class ExpenseQueryDto : CustomQueryDto
    {  
        public string? UserId {get;set;}
    }
}