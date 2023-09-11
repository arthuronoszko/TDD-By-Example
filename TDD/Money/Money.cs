namespace Money;

public class Money : Expression
{
    public int Amount;
    public string currency;

    public Money(int amount, string currency)
    {
        Amount = amount;
        this.currency = currency;
    }
    public string Currency()
    {
        return currency;
    }
    public override bool Equals(object? obj)
    {
        Money money = (Money)obj;
        return Amount == money.Amount
            && Currency().Equals(money.Currency());
    }

    public static Money Dollar(int amount)
    {
        return new Money(amount, "USD");
    }

    public Expression Times(int multiplier)
    {
        return new Money(Amount * multiplier, currency);
    }

    public static Money Franc(int amount)
    {
        return new Money(amount, "CHF");
    }

    public override string ToString()
    {
        return Amount + " " + currency;
    }
    
    public Expression Plus(Expression addend)
    {
        return new Sum(this, addend);
    }

    public Money Reduce(Bank bank, string to)
    {
        var rate = bank.Rate(currency, to);
        return new Money(Amount / rate, to);
    }
}