using System.Numerics;

namespace CompositeLong;

public partial class LongLong : IComparisonOperators<LongLong, LongLong, bool>
{
    public override bool Equals(object? obj)
    {
        if (base.Equals(obj))
            return true;

        if (obj is LongLong comLong)
        {
            return
                LowCoef == comLong.LowCoef
                && HighCoef == comLong.HighCoef
                && IsNegative == comLong.IsNegative;
        }
        return false;
    }

    public static bool operator <(LongLong left, LongLong right)
    {
        if (left.IsNegative != right.IsNegative)
            return left.IsNegative;

        if (left.IsNegative)
        {
            if (left.HighCoef != right.HighCoef)
                return left.HighCoef > right.HighCoef;
            return left.LowCoef > right.LowCoef;
        }
        else
        {
            if (left.HighCoef != right.HighCoef)
                return left.HighCoef < right.HighCoef;
            return left.LowCoef < right.LowCoef;
        }
    }

    public static bool operator >(LongLong left, LongLong right)
    {
        return right < left;
    }

    public static bool operator >=(LongLong left, LongLong right)
    {
        return (left > right) || left.Equals(right);
    }

    public static bool operator <=(LongLong left, LongLong right)
    {
        return right >= left;
    }

    public static bool operator ==(LongLong? left, LongLong? right)
    {
        return left!.Equals(right);
    }

    public static bool operator !=(LongLong? left, LongLong? right)
    {
        return !left!.Equals(right);
    }
}
