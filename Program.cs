using System.Text.RegularExpressions;
using CompositeLong;
public class Program
{
    public static void Main(string[] args)
    {               
        Console.WriteLine("================= Программа для работы с 128 битными числами ================");
        LongLong a = GetLongLongFromCMD("Введите первое число: ");
        LongLong b = GetLongLongFromCMD("Введите второе число: ");
        while (true)
        {
            Console.WriteLine("Выберите из списка действие: \n" +
                "1. Вывести первое число\n" +
                "2. Вывести второе число\n" +
                "3. 1 + 2\n" +
                "4. 1 - 2\n" +
                "5. 2 - 1\n" +
                "6. 1 * 2\n" +
                "7. 1 / 2\n" +
                "8. 2 / 1\n" +
                "9. 1 > 2\n" +
                "10. 1 >= 2\n" +
                "11. 1 < 2\n" +
                "12. 1 <= 2\n" +
                "13. 1 == 2\n" +
                "14. 1 != 2\n" +
                "Для завершения работы нажмите любую кнопку кроме представленных.....");
            string choice = Console.ReadLine();
            if (int.TryParse(choice, out int parsed) && (parsed < 0 || parsed > 14))
                break;

            switch (parsed)
            {
                case 1:
                    Console.WriteLine(a);
                    break;
                case 2:
                    Console.WriteLine(b);
                    break;
                case 3:
                    Console.WriteLine(a + b);
                    break;
                case 4:
                    Console.WriteLine(a - b);
                    break;
                case 5:
                    Console.WriteLine(b - a);
                    break;
                case 6:
                    Console.WriteLine(a * b);
                    break;
                case 7:
                    Console.WriteLine(a / b);
                    break;
                case 8:
                    Console.WriteLine(b / a);
                    break;
                case 9:
                    Console.WriteLine(a > b);
                    break;
                case 10:
                    Console.WriteLine(a >= b);
                    break;
                case 11:
                    Console.WriteLine(a < b);
                    break;
                case 12:
                    Console.WriteLine(a <= b);
                    break;
                case 13:
                    Console.WriteLine(a == b);
                    break;
                case 14:
                    Console.WriteLine(a != b);
                    break;
            }
        }
    }

    private static LongLong GetLongLongFromCMD(string promt)
    {
        Console.Write(promt);
        string input = Console.ReadLine();
        Regex regex = new("^[+-]\\d{1-25}");
        while (string.IsNullOrEmpty(input) || regex.IsMatch(input))
        {
            Console.WriteLine("Неверный ввод!!!");
            Console.Write(promt);
            input = Console.ReadLine();
        }
        LongLong result = new(input);
        return result;
    }
}