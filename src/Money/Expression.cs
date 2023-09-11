namespace Money;

public interface Expression
{
    public Money Reduce(Bank bank, string to);
    public Expression Plus(Expression addend);
    public Expression Times(int multiplier);
}