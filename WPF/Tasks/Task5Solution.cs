using System;
using System.Text;

namespace WPF
{
    public class ArrayHandler
    {
        public void GenerateException()
        {
            int[] array = new int[3];
            array[5] = 10;
        }
    }

    public static class Task5Solution
    {
        public static string GetSolution()
        {
            StringBuilder result = new StringBuilder();

            try
            {
                ArrayHandler handler = new ArrayHandler();
                handler.GenerateException();
            }
            catch (IndexOutOfRangeException exception)
            {
                result.AppendLine("Ошибка: Индекс массива выходит за пределы диапазона.");
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
