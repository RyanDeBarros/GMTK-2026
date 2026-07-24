public enum CountdownValue
{
    Ten,
    Nine,
    Eight,
    Seven,
    Six,
    Five,
    Four,
    Three,
    Two,
    One,
    Zero,

    OneHalf,
    OneThird,
    OneFourth
}

public class CountdownValueUtil
{
    public static CountdownValue Next(CountdownValue value, bool fractions, bool reverse)
    {
        if (reverse)
        {
            if (value == Max())
                return value;
            else if (value < CountdownValue.Zero)
                return value - 1;
            else
            {
                if (fractions && value > LargestFraction())
                    return value - 1;
                else
                    return CountdownValue.One;
            }
        }
        else
        {
            if (value < CountdownValue.One)
                return value + 1;
            else if (value == CountdownValue.One)
            {
                if (fractions)
                    return LargestFraction();
                else
                    return CountdownValue.Zero;
            }
            else if (value > CountdownValue.Zero)
            {
                if (fractions && value < SmallestFraction())
                    return value + 1;
                else
                    return CountdownValue.Zero;
            }
            else
                return value;
        }
    }

    public static CountdownValue Max()
    {
        return CountdownValue.Ten;
    }

    public static CountdownValue LargestFraction()
    {
        return CountdownValue.OneHalf;
    }

    public static CountdownValue SmallestFraction()
    {
        return CountdownValue.OneFourth;
    }

    public static bool AtLeast(CountdownValue lhs, CountdownValue rhs)
    {
        if (lhs == CountdownValue.Zero)
            return rhs == CountdownValue.Zero;
        else if (lhs < CountdownValue.Zero)
        {
            if (rhs >= CountdownValue.Zero)
                return true;
            else
                return lhs <= rhs;
        }
        else
        {
            if (rhs == CountdownValue.Zero)
                return true;
            else if (rhs < CountdownValue.Zero)
                return false;
            else
                return lhs <= rhs;
        }
    }

    public static bool AtMost(CountdownValue lhs, CountdownValue rhs)
    {
        return AtLeast(rhs, lhs);
    }

    public static bool GreaterThan(CountdownValue lhs, CountdownValue rhs)
    {
        return !AtMost(lhs, rhs);
    }

    public static bool LessThan(CountdownValue lhs, CountdownValue rhs)
    {
        return !AtLeast(lhs, rhs);
    }
}
