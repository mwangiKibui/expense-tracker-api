namespace ExpenseTracker.DTO
{
    public class ResponseDto 
    {   
        public bool? Success { get; set; } = true;
        public String? Message { get; set; }
        public System.Net.HttpStatusCode StatusCode { get; set; }
    }
}