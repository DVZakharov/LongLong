namespace CompositeLong.Contracts;

public interface IArithmeticOperations<T> where T : IArithmeticOperations<T>
{
    static abstract T operator +(T left, T right);
    static abstract T operator -(T left, T right);
    static abstract T operator *(T left, T right);
    static abstract T operator /(T left, T right);
    static abstract T operator %(T left, T right);
    static abstract T operator -(T value);
}
