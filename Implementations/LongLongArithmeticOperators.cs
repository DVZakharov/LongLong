using CompositeLong.Contracts;

namespace CompositeLong;

public partial class LongLong : IArithmeticOperations<LongLong>
{
    public static LongLong operator +(LongLong left, LongLong right)
    {
        if (left.IsNegative == right.IsNegative)
        {
            ulong low = left.LowCoef + right.LowCoef;
            long carry = (low < left.LowCoef) ? 1 : 0;

            long high;
            try
            {
                high = left.HighCoef + right.HighCoef + carry;
            } catch (OverflowException)
            {
                throw new OverflowException($"Переполнение CompositeLong");
            }

            return new LongLong
            {
                HighCoef = high,
                LowCoef = low,
                IsNegative = left.IsNegative
            };
        }
        else
        {
            var absLeft = new LongLong
            {
                HighCoef = left.HighCoef,
                LowCoef = left.LowCoef,
                IsNegative = false
            };
            var absRight = new LongLong
            {
                HighCoef = right.HighCoef,
                LowCoef = right.LowCoef,
                IsNegative = false
            };

            if (absLeft < absRight)
            {
                var (high, low) = SubstractAbs(
                    right.HighCoef, right.LowCoef,
                    left.HighCoef, left.LowCoef);
                return new LongLong
                {
                    HighCoef = high,
                    LowCoef = low,
                    IsNegative = right.IsNegative
                };
            }
            else if (absRight < absLeft)
            {
                var (high, low) = SubstractAbs(
                    left.HighCoef, left.LowCoef,
                    right.HighCoef, right.LowCoef);
                return new LongLong
                {
                    HighCoef = high,
                    LowCoef = low,
                    IsNegative = left.IsNegative
                };
            }
            else
            {
                return new LongLong();
            }
        }
    }

    private static (long high, ulong low) SubstractAbs(
        long highLarger, ulong lowLarger,
        long highSmaller, ulong lowSmaller)
    {
        ulong low = lowLarger - lowSmaller;
        long borrow = (lowLarger < lowSmaller) ? 1 : 0;
        long high = highLarger - highSmaller - borrow;
        return (high, low);
    }

    public static LongLong operator -(LongLong value)
    {
        bool isZero = value.HighCoef == 0 && value.LowCoef == 0;
        return new LongLong
        {
            HighCoef = value.HighCoef,
            LowCoef = value.LowCoef,
            IsNegative = isZero ? false : !value.IsNegative
        };
    }

    public static LongLong operator -(LongLong left, LongLong right) => left + (-right);

    public static LongLong operator *(LongLong left, LongLong right)
    {


        ulong a_high = (ulong)left.HighCoef;
        ulong a_low = left.LowCoef;
        ulong b_high = (ulong)right.HighCoef;
        ulong b_low = right.LowCoef;

        ulong h_ll = Math.BigMul(a_low, b_low, out ulong l_ll);
        ulong h_hl = Math.BigMul(a_high, b_low, out ulong l_hl);
        ulong h_lh = Math.BigMul(a_low, b_high, out ulong l_lh);
        ulong h_hh = Math.BigMul(a_high, b_high, out ulong l_hh);

        ulong w0 = l_ll;

        ulong sum1 = l_hl + l_lh;
        ulong carry1 = (sum1 < l_hl) ? 1UL : 0;
        ulong w1 = h_ll + sum1;
        carry1 += (w1 < h_ll) ? 1UL : 0;

        ulong temp = l_hh + h_hl;
        ulong carry2 = (temp < l_hh) ? 1UL : 0;
        temp += h_lh;
        if (temp < h_lh)
            carry2++;
        temp += carry1;
        if (temp < carry1)
            carry2++;
        ulong w2 = temp;
        ulong w3 = h_hh + carry2;

        long resultHigh;
        try
        {
            resultHigh = checked((long)w1);
        } catch (OverflowException)
        {
            throw new OverflowException("Переполнение CompositeLong");
        }

        if (w2 != 0 || w3 != 0)
            throw new OverflowException("Переполнение CompositeLong");

        return new LongLong
        {
            HighCoef = resultHigh,
            LowCoef = w0,
            IsNegative = left.IsNegative ^ right.IsNegative
        };
    }

    public static LongLong operator /(LongLong left, uint right)
    {
        if (right == 0)
            throw new DivideByZeroException("Деление на ноль.");

        ulong high = (ulong)left.HighCoef;
        ulong low = left.LowCoef;

        uint a3 = (uint)(high >> 32);
        uint a2 = (uint)(high & 0xFFFFFFFF);
        uint a1 = (uint)(low >> 32);
        uint a0 = (uint)(low & 0xFFFFFFFF);

        uint q3 = 0, q2 = 0, q1 = 0, q0 = 0;
        ulong rem = 0;


        ulong temp = a3;
        q3 = (uint)(temp / right);
        rem = temp % right;

        temp = rem << 32 | a2;
        q2 = (uint)(temp / right);
        rem = temp % right;

        temp = rem << 32 | a1;
        q1 = (uint)(temp / right);
        rem = temp % right;

        temp = rem << 32 | a0;
        q0 = (uint)(temp / right);
        rem = temp % right;

        ulong lowQuot = ((ulong)q1 << 32) | q0;
        ulong highQuot = ((ulong)q3 << 32) | q2;

        bool isNegative = left.IsNegative;
        bool isZero = highQuot == 0 && lowQuot == 0;

        return new LongLong
        {
            HighCoef = (long)highQuot,
            LowCoef = lowQuot,
            IsNegative = isNegative && !isZero
        };
    }


    public static LongLong operator %(LongLong left, uint right)
    {
        if (right == 0)
            throw new DivideByZeroException("Деление на ноль.");

        ulong high = (ulong)left.HighCoef;
        ulong low = left.LowCoef;

        uint a3 = (uint)(high >> 32);
        uint a2 = (uint)(high & 0xFFFFFFFF);
        uint a1 = (uint)(low >> 32);
        uint a0 = (uint)(low & 0xFFFFFFFF);

        ulong remainder = 0;

        remainder = (remainder << 32 | a3) % right;
        remainder = (remainder << 32 | a2) % right;
        remainder = (remainder << 32 | a1) % right;
        remainder = (remainder << 32 | a0) % right;

        uint rem = (uint)remainder;

        bool isNegative = left.IsNegative && rem != 0;

        return new LongLong
        {
            HighCoef = 0,
            LowCoef = rem,
            IsNegative = isNegative
        };
    }

    public static LongLong operator /(LongLong left, LongLong right)
    {
        var zero = new LongLong();
        var one = new LongLong("1");
        zero = right.IsNegative ? -zero : zero;
        if (right == zero)
            throw new DivideByZeroException();

        bool resultNegative = left.IsNegative ^ right.IsNegative;

        LongLong dividend = left.IsNegative ? -left : left;
        LongLong divisor = right.IsNegative ? -right : right;

        if (dividend < divisor)
            return new LongLong();

        LongLong lo = new LongLong("0");
        LongLong hi = dividend;
        LongLong quotient = lo;

        while (lo <= hi)
        {
            LongLong mid = (lo + hi) / 2u;
            LongLong product = mid * divisor;

            if (product <= dividend)
            {
                quotient = mid;
                lo = mid + one;
            }
            else
            {
                hi = mid - one;
            }
        }
        return resultNegative ? -quotient : quotient;
    }

    public static LongLong operator %(LongLong left, LongLong right)
    {
        var zero = new LongLong();

        LongLong quotient = left / right;
        LongLong remainder = left - quotient * right;

        if (remainder.IsNegative != left.IsNegative && remainder != zero)
            remainder = -remainder;

        return remainder;
    }
}
