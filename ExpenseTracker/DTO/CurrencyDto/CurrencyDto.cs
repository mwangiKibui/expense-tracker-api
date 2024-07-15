namespace ExpenseTracker.DTO
{
    public class CurrencyDto {
        public Guid CurrencyID {get;set;}
        public required String Name {get;set;}
        public required String Code {get;set;}
        public bool IsDeleted {get;set;}
    }
}