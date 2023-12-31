using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;

namespace Money.Tests;

public class MoneyTests
{
    [Fact]
    public void Multiplication()
    {
        Money five = Money.Dollar(5);
        Assert.Equal(Money.Dollar(10), five.Times(2));
        Assert.Equal(Money.Dollar(15), five.Times(3));
    }
    
    [Fact]
    public void Equality()
    {
        Assert.True(Money.Dollar(5).Equals(Money.Dollar(5)));
        Assert.False(Money.Dollar(5).Equals(Money.Dollar(6)));
        Assert.False(Money.Franc(5).Equals(Money.Dollar(5)));
    }

    [Fact]
    public void Currency()
    {
        Assert.Equal("USD", Money.Dollar(1).Currency());
        Assert.Equal("CHF", Money.Franc(1).Currency());
    }

    [Fact]
    public void DifferentClassEquality()
    {
        Assert.True(new Money(10, "CHF").Equals(new Money(10, "CHF")));
    }

    [Fact]
    public void SimpleAddition()
    {
        Money five = Money.Dollar(5);
        Expression sum = five.Plus(five);
        Bank bank = new Bank();
        Money reduced = bank.Reduce(sum, "USD");
        Assert.Equal(Money.Dollar(10), reduced);
    }

    [Fact]
    public void PlusReturnsSum()
    {
        Money five = Money.Dollar(5);
        Expression result = five.Plus(five);
        Sum sum = (Sum)result;
        Assert.Equal(five, sum.Augend);
        Assert.Equal(five, sum.Addend);
    }

    [Fact]
    public void ReduceSum()
    {
        Expression sum = new Sum(Money.Dollar(3), Money.Dollar(4));
        Bank bank = new Bank();
        Money result = bank.Reduce(sum, "USD");
        Assert.Equal(Money.Dollar(7), result);
    }

    [Fact]
    public void ReduceMoney()
    {
        Bank bank = new Bank();
        Money result = bank.Reduce(Money.Dollar(1), "USD");
        Assert.Equal(Money.Dollar(1), result);
    }

    [Fact]
    public void ReduceMoneyDifferentCurrency()
    {
        Bank bank = new Bank();
        bank.AddRate("CHF", "USD", 2);
        Money result = bank.Reduce(Money.Franc(2), "USD");
        Assert.Equal(Money.Dollar(1), result);
    }

    [Fact]
    public void IdentityRate()
    {
        Assert.Equal(1, new Bank().Rate("USD", "USD"));
    }

    [Fact]
    public void MixedAddition()
    {
        Expression fiveBucks = Money.Dollar(5);
        Expression tenFrancs = Money.Franc(10);
        var bank = new Bank();
        bank.AddRate("CHF", "USD", 2);
        Money result = bank.Reduce(fiveBucks.Plus(tenFrancs), "USD");
        Assert.Equal(Money.Dollar(10), result);
    }

    [Fact]
    public void SumPlusMoney()
    {
        Expression fiveBucks = Money.Dollar(5);
        Expression tenFrancs = Money.Franc(10);
        Bank bank = new Bank();
        bank.AddRate("CHF", "USD", 2);
        Expression sum = new Sum(fiveBucks, tenFrancs).Plus(fiveBucks);
        Money result = bank.Reduce(sum, "USD");
        Assert.Equal(Money.Dollar(15), result);
    }

    [Fact]
    public void SumTimes()
    {
        Expression fiveBucks = Money.Dollar(5);
        Expression tenFrancs = Money.Franc(10);
        Bank bank = new Bank();
        bank.AddRate("CHF", "USD", 2);
        Expression sum = new Sum(fiveBucks, tenFrancs).Times(2);
        Money result = bank.Reduce(sum, "USD");
        Assert.Equal(Money.Dollar(20), result);
    }
}