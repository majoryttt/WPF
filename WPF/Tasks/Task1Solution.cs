using System.Text;

namespace WPF.Tasks
{
  public static class Task1Solution
  {
    public static string GetSolution()
    {
      StringBuilder result = new StringBuilder(); // Создаем объект StringBuilder для построения строки результата.

      // Добавляем строки с различными вариантами вывода текста.
      result.AppendLine("Это строка,");
      result.AppendLine("        иначе – \"стринг\",");
      result.AppendLine("                иначе – \"строковый литерал\"\n");

      result.AppendLine("Это звуковой");
      result.AppendLine("сигнал!\n");

      result.AppendLine("Это строка?");
      result.AppendLine("\"Да!\"\n");

      result.AppendLine("\" – это двойные кавычки");
      result.AppendLine("\' – это апостроф\n");

      result.AppendLine("Это \"строка\"?");
      result.AppendLine("Это \"строка\"!\b \n");

      result.AppendLine("Это строка?");
      result.AppendLine("Это\rДа строка!\n");

      result.AppendLine("\\Это комментарий?");
      result.AppendLine("//Нет, это комментарий\n");

      result.AppendLine("C:\\Program Files\\Far");
      result.AppendLine("D\rF:\\Program Files\\Far\n");


      result.AppendLine("\x43\x65\x6E\x61\x3D\x0A\x3D\x31\x30\x30\x24\n"); // Добавляем строки с использованием шестнадцатеричных и Unicode-кодов символов.

      result.AppendLine("\u0426\u0435\u043d\u0430\u003D");
      result.AppendLine("\u003D\u0031\u0030\u0030\u0024\n");

      return result.ToString(); // Возвращаем полученную строку результата.
    }
  }
}
