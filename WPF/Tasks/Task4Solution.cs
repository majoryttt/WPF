using System;
using System.Text;

namespace WPF
{
    public static class Task4Solution
    {
        public static string GetSolution(string firstNumber, string secondNumber)
        {
            StringBuilder result = new StringBuilder();

            try
            {
              int num1 = Convert.ToInt32(firstNumber);
              int num2 = Convert.ToInt32(secondNumber);

              int divisionResult = num1 / num2;
              result.AppendLine($"Результат деления: {divisionResult}");

              int[] array = new int[3];
              array[5] = 10;
            }
            catch (DivideByZeroException exception)
            {
              result.AppendLine("Ошибка: Деление на ноль невозможно.");
              PrintExceptionDetails(exception, result);
            }
            catch (IndexOutOfRangeException exception)
            {
              result.AppendLine("Ошибка: Индекс массива выходит за пределы диапазона.");
              PrintExceptionDetails(exception, result);
            }
            catch (FormatException exception)
            {
              result.AppendLine("Ошибка: Неверный формат входных данных.");
              PrintExceptionDetails(exception, result);
            }
            finally
            {
                result.AppendLine("Программа завершена.");
            }

            return result.ToString();
        }

        private static void PrintExceptionDetails(Exception exception, StringBuilder result)
        {
            result.AppendLine($"Сообщение об ошибке: {exception.Message}");
            result.AppendLine($"Источник ошибки: {exception.Source}");
            result.AppendLine($"Метод, вызвавший исключение: {exception.TargetSite}");
            result.AppendLine($"Стек вызовов: {exception.StackTrace}");
            if (exception.InnerException != null)
            {
                result.AppendLine($"Внутреннее исключение: {exception.InnerException.Message}");
            }
            result.AppendLine($"Код ошибки (HResult): {exception.HResult}");
            if (!string.IsNullOrEmpty(exception.HelpLink))
            {
                result.AppendLine($"Ссылка на справку: {exception.HelpLink}");
            }
        }
    }
}
