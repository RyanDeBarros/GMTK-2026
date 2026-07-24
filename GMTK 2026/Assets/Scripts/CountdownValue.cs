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
            if (value == CountdownValue.Ten)
                return value;
            else if (value < CountdownValue.Zero)
                return value - 1;
            else
            {
                if (fractions && value > CountdownValue.OneHalf)
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
                    return CountdownValue.OneHalf;
                else
                    return CountdownValue.Zero;
            }
            else if (value > CountdownValue.Zero)
            {
                if (fractions && value < CountdownValue.OneFourth)
                    return value + 1;
                else
                    return CountdownValue.Zero;
            }
            else
                return value;
        }
    }
}
