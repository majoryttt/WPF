using System.Text;

namespace WPF.Tasks
{
  public static class Task6Solution
  {
    // Основной метод решения задачи
    public static string GetSolution()
    {
      StringBuilder result = new StringBuilder();

      // Генерируем случайный массив из 10 элементов
      int[] array = GenerateRandomArray(10);
      // Подсчитываем количество перемен знака в массиве
      int signChanges = CountSignChanges(array);

      // Добавляем информацию о сгенерированном массиве и количестве перемен знака в результат
      result.AppendLine("Сгенерированный массив: " + string.Join(", ", array));
      result.AppendLine("Количество перемен знака: " + signChanges);

      return result.ToString();
    }

    // Метод для генерации случайного массива заданной длины
    private static int[] GenerateRandomArray(int length)
    {
      Random random = new Random();
      int[] array = new int[length];
      for (int i = 0; i < length; i++)
      {
        // Генерируем случайное число от -100 до 99
        array[i] = random.Next(-100, 100);
      }
      return array;
    }

    // Метод для подсчета количества перемен знака в массиве
    private static int CountSignChanges(int[] array)
    {
      int signChanges = 0;
      int? previousSign = null;

      foreach (int number in array)
      {
        // Определяем знак текущего числа
        int currentSign = Math.Sign(number);

        // Если предыдущий знак существует, текущее число не ноль и знаки отличаются, увеличиваем счетчик перемен знака
        if (previousSign.HasValue && currentSign != 0 && currentSign != previousSign)
        {
          signChanges++;
        }

        // Обновляем предыдущий знак
        previousSign = currentSign;
      }

      return signChanges;
    }
  }
}
