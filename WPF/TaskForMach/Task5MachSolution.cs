using System;
using System.Collections.Generic;
using System.Text;

namespace WPF.TaskForMach
{
  public class Task5MachSolution
  {
    private static Dictionary<(int, int), long> stirlingCache = new Dictionary<(int, int), long>();

    public static string GetSolution()
    {
      StringBuilder result = new StringBuilder();

      for (int n = 0; n <= 9; n++)
      {
        result.AppendLine($"n = {n}:");
        result.AppendLine($"Bell number: {BellNumber(n)}");
        result.AppendLine("Stirling numbers of the 2nd kind:");
        for (int k = 0; k <= n; k++)
        {
          result.AppendLine($"S({n},{k}) = {StirlingNumber2(n, k)}");
        }
        result.AppendLine();
      }

      return result.ToString();
    }

    private static long StirlingNumber2(int n, int k)
    {
      if (k == 0 || n == 0)
        return (n == k) ? 1 : 0;

      if (k > n)
        return 0;

      if (k == 1 || k == n)
        return 1;

      var key = (n, k);
      if (stirlingCache.ContainsKey(key))
        return stirlingCache[key];

      long result = k * StirlingNumber2(n - 1, k) + StirlingNumber2(n - 1, k - 1);
      stirlingCache[key] = result;
      return result;
    }

    private static long BellNumber(int n)
    {
      long sum = 0;
      for (int k = 0; k <= n; k++)
      {
        sum += StirlingNumber2(n, k);
      }
      return sum;
    }
  }
}
