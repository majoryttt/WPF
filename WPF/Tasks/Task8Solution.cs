using System;
using System.Collections.Generic;
using System.Text;

namespace WPF
{
  public static class Task8Solution
  {
    public static string GetSolution(string text, string word)
    {
      StringBuilder result = new StringBuilder();

      result.AppendLine("Даны текстовая строка и слово (например, ba). Напечатать все слова, входящие в эту текстовую строку, начинающиеся с букв заданного слова (например, bak, barber, baab, baalam), используя методы класса String\nили StringBuilder\n\n");

      result.AppendLine($"Введенная строка: {text}");
      result.AppendLine($"Введенное слово: {word}\n");

      List<string> matchingWords = FindWordsStartingWith(text, word);

      result.AppendLine("Слова, начинающиеся с букв заданного слова:");
      foreach (var matchedWord in matchingWords)
      {
        result.AppendLine(matchedWord);
      }

      return result.ToString();
    }

    private static List<string> FindWordsStartingWith(string text, string word)
    {
      List<string> result = new List<string>();
      string[] words = text.Split(new char[] { ' ', ',', '.', '!', '?' }, StringSplitOptions.RemoveEmptyEntries);

      foreach (string w in words)
      {
        bool matches = true;
        for (int i = 0; i < Math.Min(w.Length, word.Length); i++)
        {
          if (char.ToLower(w[i]) != char.ToLower(word[i]))
          {
            matches = false;
            break;
          }
        }
        if (matches)
        {
          result.Add(w);
        }
      }

      return result;
    }
  }
}
