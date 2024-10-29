using System;
using System.Collections.Generic;
using System.Text;

namespace WPF
{
  public static class Task8Solution
  {
    // Основной метод решения задачи
    public static string GetSolution(string text, string word)
    {
      StringBuilder result = new StringBuilder();

      // Выводим введенные данные
      result.AppendLine($"Введенная строка: {text}");
      result.AppendLine($"Введенное слово: {word}\n");

      // Находим слова, начинающиеся с букв заданного слова
      List<string> matchingWords = FindWordsStartingWith(text, word);

      // Выводим найденные слова
      result.AppendLine("Слова, начинающиеся с букв заданного слова:");
      foreach (var matchedWord in matchingWords)
      {
        result.AppendLine(matchedWord);
      }

      return result.ToString();
    }

    // Метод для поиска слов, начинающихся с букв заданного слова
    private static List<string> FindWordsStartingWith(string text, string word)
    {
      List<string> result = new List<string>();
      // Разбиваем текст на слова, удаляя пустые элементы
      string[] words = text.Split(new char[] { ' ', ',', '.', '!', '?' }, StringSplitOptions.RemoveEmptyEntries);

      foreach (string w in words)
      {
        bool matches = true;
        // Проверяем, совпадают ли буквы слова с буквами заданного слова
        for (int i = 0; i < Math.Min(w.Length, word.Length); i++)
        {
          if (char.ToLower(w[i]) != char.ToLower(word[i]))
          {
            matches = false;
            break;
          }
        }
        // Если слово подходит, добавляем его в результат
        if (matches)
        {
          result.Add(w);
        }
      }

      return result;
    }
  }
}
