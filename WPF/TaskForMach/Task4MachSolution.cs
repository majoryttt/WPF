using System;
using System.Text;

namespace WPF.TaskForMach;

public class Task4MachSolution
{
    // Основной метод решения, принимающий число для проверки на простоту
    public static string GetSolution(long number)
    {
        var sb = new StringBuilder();

        // Вывод тестируемого числа
        sb.AppendLine($"Тестируемое число: {number}\n");

        // Проверка числа методом перебора делителей
        bool isTrialDivisionPrime = IsTrialDivisionPrime(number);
        sb.AppendLine($"Метод перебора делителей: {(isTrialDivisionPrime ? "Простое" : "Составное")}");

        // Проверка числа тестом Ферма (5 итераций)
        bool isFermatPrime = IsFermatPrime(number, 5);
        sb.AppendLine($"Тест Ферма: {(isFermatPrime ? "Вероятно простое" : "Составное")}");

        // Проверка числа тестом Миллера-Рабина (5 итераций)
        bool isMillerRabinPrime = IsMillerRabinPrime(number, 5);
        sb.AppendLine($"Тест Миллера-Рабина: {(isMillerRabinPrime ? "Вероятно простое" : "Составное")}");

        return sb.ToString();
    }

    // Метод проверки числа на простоту перебором делителей
    // Метод перебора делителей - это метод, который проверяет, делится ли число на какое-либо другое число, кроме 1 и самого себя.
    private static bool IsTrialDivisionPrime(long n)
    {
        if (n <= 1) return false;
        if (n <= 3) return true;
        if (n % 2 == 0) return false;

        // Проверяем делители до корня из числа
        for (long i = 3; i <= Math.Sqrt(n); i += 2)
        {
            if (n % i == 0) return false;
        }
        return true;
    }

    // Тест Ферма на простоту числа
    // Тест простоты Ферма - это тест простоты, который используется для определения, является ли число простым или составным.
    private static bool IsFermatPrime(long n, int iterations)
    {
        if (n <= 1) return false;
        if (n <= 3) return true;

        Random rand = new Random();

        // Выполняем заданное количество итераций теста
        for (int i = 0; i < iterations; i++)
        {
            // Выбираем случайное число a в диапазоне [2, n-1]
            long a = rand.Next(2, (int)Math.Min(n - 1, int.MaxValue));
            // Проверяем условие теста Ферма: a^(n-1) ≡ 1 (mod n)
            if (ModPow(a, n - 1, n) != 1)
                return false;
        }
        return true;
    }

    // Тест Миллера-Рабина на простоту числа
    // Алгоритм Миллера — Рабина - это вероятностный тест простоты, который используется для определения, является ли число простым или составным.
    private static bool IsMillerRabinPrime(long n, int iterations)
    {
        if (n <= 1) return false;
        if (n <= 3) return true;
        if (n % 2 == 0) return false;

        // Представляем n - 1 в виде d * 2^s
        long d = n - 1;
        int s = 0;
        while (d % 2 == 0)
        {
            d /= 2;
            s++;
        }

        Random rand = new Random();
        // Выполняем заданное количество итераций теста
        for (int i = 0; i < iterations; i++)
        {
            // Выбираем случайное число a в диапазоне [2, n-2]
            long a = rand.Next(2, (int)Math.Min(n - 2, int.MaxValue)) + 1;
            long x = ModPow(a, d, n);

            if (x == 1 || x == n - 1) continue;

            // Проверяем последовательность квадратов
            for (int r = 1; r < s; r++)
            {
                x = (x * x) % n;
                if (x == 1) return false;
                if (x == n - 1) break;
            }

            if (x != n - 1) return false;
        }
        return true;
    }

    // Вспомогательный метод для быстрого возведения в степень по модулю
    private static long ModPow(long baseNum, long exponent, long modulus)
    {
        if (modulus == 1) return 0;

        long result = 1;
        baseNum = baseNum % modulus;

        // Алгоритм быстрого возведения в степень
        while (exponent > 0)
        {
            if ((exponent & 1) == 1)
                result = (result * baseNum) % modulus;

            baseNum = (baseNum * baseNum) % modulus;
            exponent >>= 1;
        }
        return result;
    }
}
