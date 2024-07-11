namespace ExpenseTracker.DTO
{
    public class ExpenseTypeDto 
    {
        public int Id { get; set; }
        public Guid ExpenseTypeid { get; set; }
        public required String Name {get;set;}
        public String? Description {get;set;}

    }
}