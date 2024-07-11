namespace ExpenseTracker.DTO
{
    public class ListResponseDto 
    {   
        public bool? Success { get; set; } = true;
        public string? Message { get; set; }
        public System.Net.HttpStatusCode StatusCode { get; set;}
        public int TotalCount { get; set; } = 0;
        public int CurrentPage { get; set; } = 0;
        public int TotalPages { get; set; } = 0;
    }
}