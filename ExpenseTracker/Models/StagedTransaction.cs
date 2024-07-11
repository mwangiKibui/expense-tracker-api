namespace ExpenseTracker.Models
{
    public class StagedTransaction 
    {
        public int Id { get; set; }
        public Guid StagedTransactionID { get; set; }
        public required int userId {get;set;}
        public String? TransactionCode {get;set;}
        public required DateTime TransactionDate {get;set;}
        public required decimal TransactionAmount {get;set;}
        public required String TransactionCurrency {get;set;}
        public bool? isReconciled {get;set;} = false;
        public required String Channel {get;set;}
        public DateTime? DeletedAt {get;set;}
        public Guid? DeletedBy { get; set; }
        public DateTime CreatedAt {get;set;} = DateTime.Now;
        public Guid CreatedBy {get;set;}
        public User? User { get; set; } = null;
        public ICollection<Transaction>? Transactions {get; set;} = null;
    }
}