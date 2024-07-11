using System.Linq.Expressions;

namespace ExpenseTracker.Helpers;

public class OrderBy<TOrder,TBy> : IOrderBy
{
    private readonly Expression<Func<TOrder,TBy>> _expression;
    public OrderBy(Expression<Func<TOrder,TBy>> expression){
        _expression = expression;
    }

    public dynamic Expression => _expression;
}