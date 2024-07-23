namespace ExpenseTracker.DTO
{
    public class CurrencyDto {
        public int Id {get;set;}
        public Guid CurrencyID {get;set;}
        public required String Name {get;set;}
        public required String Code {get;set;}
        public required bool isDefault {get;set;}
        // public bool IsDeleted {get;set;}
    }
}