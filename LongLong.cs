namespace CompositeLong;

public partial class LongLong
{
    public LongLong()
    {
        LowCoef = 0;
        HighCoef = 0;
        IsNegative = false;
    }

    public LongLong(string value)
    {
        ArgumentNullException.ThrowIfNull(value);
        if (value.Length == 0)
            throw new ArgumentException("Строка не может быть пустой.", nameof(value));

        int end = value.Length - 1;
        while (end >= 0 && char.IsWhiteSpace(value[end]))
            end--;
        int start = 0;
        while (start <= end && char.IsWhiteSpace(value[start]))
            start++;

        if (start > end)
            throw new FormatException("Строка содержит только пробельные символы.");

        if (value[start] == '-')
        {
            IsNegative = true;
            start++;
        }
        else if (value[start] == '+')
        {
            start++;
        }

        while (start <= end && value[start] == '0')
            start++;

        if (start > end)
        {
            HighCoef = 0;
            LowCoef = 0;
            IsNegative = false;
            return;
        }

        UInt128 result = 0;

        for (int i = start; i <= end; i++)
        {
            char c = value[i];
            if (c < '0' || c > '9')
                throw new FormatException($"Некорректный символ '{c}' в позиции {i}.");

            uint digit = (uint)(c - '0');

            if (result > (UInt128.MaxValue - digit) / 10)
                throw new OverflowException("Число превышает диапазон 128-битного целого.");

            result = result * 10 + digit;
        }

        LowCoef = (ulong)result;
        HighCoef = (long)(result >> 64);

        if (result == 0)
            IsNegative = false;
    }

    public long HighCoef { get; init; }
    public ulong LowCoef { get; init; }
    public bool IsNegative { get; init; }


    public override int GetHashCode()
    {
        return HashCode.Combine(HighCoef, LowCoef, IsNegative);
    }


    public override string ToString()
    {
        if (HighCoef == 0 && LowCoef == 0)
            return "0";

        LongLong absValue = this.IsNegative ? -this : this;

        var digits = new List<char>();

        while (!(absValue.HighCoef == 0 && absValue.LowCoef == 0))
        {
            LongLong remainder = absValue % 10;
            char digitChar = (char)('0' + remainder.LowCoef);
            digits.Add(digitChar);

            absValue /= 10;
        }
        digits.Reverse();

        string result = new([.. digits]);
        return this.IsNegative ? "-" + result : result;
    }
}