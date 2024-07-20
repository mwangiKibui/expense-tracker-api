namespace ExpenseTracker.Data
{
    public class TransactionDefaults
    {
        public enum AllowedTransactionTypes
        {
            CREDIT = 1,
            DEBIT = 2
        }
        public enum AllowedTransactionNatures
        {
            INCOME = 1,
            EXPENSE = 2,
            BUDGETPLAN = 3
        }
        public readonly int CREDIT = 1;
        public readonly int DEBIT = 2;
        public readonly int INCOMETRANSACTIONNATURE = 1;
        public readonly int EXPENSETRANSACTIONNATURE = 2;
        public readonly int BUDGETPLANTRANSACTIONNATURE = 3;
    }
}