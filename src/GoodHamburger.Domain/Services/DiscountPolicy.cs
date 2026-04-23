namespace GoodHamburger.Domain.Services;

public static class DiscountPolicy
{
    public static decimal CalculateRate(bool hasSandwich, bool hasFrenchFries, bool hasSoftDrink)
    {
        if (hasSandwich && hasFrenchFries && hasSoftDrink)
        {
            return 0.20m;
        }

        if (hasSandwich && hasSoftDrink)
        {
            return 0.15m;
        }

        if (hasSandwich && hasFrenchFries)
        {
            return 0.10m;
        }

        return 0m;
    }
}