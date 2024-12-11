using System;
using System.Text;

namespace WPF.TaskForMach;

public class Task4MachSolution
{
    public static string GetSolution(long number)
    {
        var sb = new StringBuilder();
        
        // Test using all three methods
        sb.AppendLine($"Тестируемое число: {number}\n");
        
        // Trial Division
        bool isTrialDivisionPrime = IsTrialDivisionPrime(number);
        sb.AppendLine($"Метод перебора делителей: {(isTrialDivisionPrime ? "Простое" : "Составное")}");
        
        // Fermat Test
        bool isFermatPrime = IsFermatPrime(number, 5); // 5 iterations
        sb.AppendLine($"Тест Ферма: {(isFermatPrime ? "Вероятно простое" : "Составное")}");
        
        // Miller-Rabin Test
        bool isMillerRabinPrime = IsMillerRabinPrime(number, 5); // 5 iterations
        sb.AppendLine($"Тест Миллера-Рабина: {(isMillerRabinPrime ? "Вероятно простое" : "Составное")}");
        
        return sb.ToString();
    }
    

    // Trial Division Method
    private static bool IsTrialDivisionPrime(long n)
    {
        if (n <= 1) return false;
        if (n <= 3) return true;
        if (n % 2 == 0) return false;

        for (long i = 3; i <= Math.Sqrt(n); i += 2)
        {
            if (n % i == 0) return false;
        }
        return true;
    }

    // Fermat Primality Test
    private static bool IsFermatPrime(long n, int iterations)
    {
        if (n <= 1) return false;
        if (n <= 3) return true;
        
        Random rand = new Random();
        
        for (int i = 0; i < iterations; i++)
        {
            long a = rand.Next(2, (int)Math.Min(n - 1, int.MaxValue));
            if (ModPow(a, n - 1, n) != 1)
                return false;
        }
        return true;
    }

    // Miller-Rabin Primality Test
    private static bool IsMillerRabinPrime(long n, int iterations)
    {
        if (n <= 1) return false;
        if (n <= 3) return true;
        if (n % 2 == 0) return false;

        long d = n - 1;
        int s = 0;
        while (d % 2 == 0)
        {
            d /= 2;
            s++;
        }

        Random rand = new Random();
        for (int i = 0; i < iterations; i++)
        {
            long a = rand.Next(2, (int)Math.Min(n - 2, int.MaxValue)) + 1;
            long x = ModPow(a, d, n);
            
            if (x == 1 || x == n - 1) continue;

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

    // Helper method for modular exponentiation
    private static long ModPow(long baseNum, long exponent, long modulus)
    {
        if (modulus == 1) return 0;
        
        long result = 1;
        baseNum = baseNum % modulus;
        
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
