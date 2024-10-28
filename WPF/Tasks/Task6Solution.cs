using System;
using System.Text;

namespace WPF
{
  public static class Task6Solution
  {
    public static string GetSolution()
    {
      StringBuilder result = new StringBuilder();

      result.AppendLine("В целочисленном массиве, сгенерированном случайным образом, определить количество перемен знака.\nИспользовать в программе оператор foreach.\n\n");

      int[] array = GenerateRandomArray(10);
      int signChanges = CountSignChanges(array);

      result.AppendLine("Сгенерированный массив: " + string.Join(", ", array));
      result.AppendLine("Количество перемен знака: " + signChanges);

      return result.ToString();
    }

    private static int[] GenerateRandomArray(int length)
    {
      Random random = new Random();
      int[] array = new int[length];
      for (int i = 0; i < length; i++)
      {
        array[i] = random.Next(-100, 100);
      }
      return array;
    }

    private static int CountSignChanges(int[] array)
    {
      int signChanges = 0;
      int? previousSign = null;

      foreach (int number in array)
      {
        int currentSign = Math.Sign(number);

        if (previousSign.HasValue && currentSign != 0 && currentSign != previousSign)
        {
          signChanges++;
        }

        previousSign = currentSign;
      }

      return signChanges;
    }
  }
}
