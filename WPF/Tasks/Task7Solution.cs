﻿using System;
using System.Text;

namespace WPF
{
    public static class Task7Solution
    {
        public static string GetSolution()
        {
            StringBuilder result = new StringBuilder();

            result.AppendLine("Написать и протестировать метод, находящий сумму элементов\nзаданного целочисленного ступенчатого массива, расположенных на первой и последней позиции каждой строки. Метод должен генерировать хотя\nбы одно исключение. Ступенчатый массив должен генерироваться случайным образом и выводиться на экран в методе Main.\n\n");

            try
            {
                int[][] jaggedArray = GenerateRandomJaggedArray(5, 1, 10);
                PrintJaggedArray(jaggedArray, result);
                int sum = SumFirstAndLastElements(jaggedArray);
                result.AppendLine($"Сумма элементов на первой и последней позиции каждой строки: {sum}");
            }
            catch (Exception exception)
            {
                result.AppendLine("Произошла ошибка:");
                PrintExceptionDetails(exception, result);
            }

            return result.ToString();
        }

        private static int[][] GenerateRandomJaggedArray(int rows, int minColumns, int maxColumns)
        {
            Random random = new Random();
            int[][] jaggedArray = new int[rows][];
            for (int i = 0; i < rows; i++)
            {
                int columns = random.Next(minColumns, maxColumns + 1);
                jaggedArray[i] = new int[columns];
                for (int j = 0; j < columns; j++)
                {
                    jaggedArray[i][j] = random.Next(-100, 100);
                }
            }

            return jaggedArray;
        }

        private static void PrintJaggedArray(int[][] jaggedArray, StringBuilder result)
        {
            result.AppendLine("Сгенерированный ступенчатый массив:");
            foreach (var row in jaggedArray)
            {
                result.AppendLine(string.Join(", ", row));
            }
        }

        private static int SumFirstAndLastElements(int[][] jaggedArray)
        {
            int sum = 0;
            foreach (var row in jaggedArray)
            {
                if (row.Length == 0)
                {
                    throw new InvalidOperationException("Одна из строк массива пуста.");
                }

                sum += row[0];
                if (row.Length > 1)
                {
                    sum += row[^1];
                }
            }
            return sum;
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