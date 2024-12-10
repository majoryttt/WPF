using System;
using System.Text;
using System.Linq;

namespace WPF.Tasks
{
    // Базовый класс для строки
    public class CStr
    {
        protected string Value { get; set; }

        public CStr()
        {
            Value = string.Empty;
        }

        public CStr(string value)
        {
            Value = value;
        }

        public override string ToString()
        {
            return Value;
        }
    }

    // Производный класс для шестнадцатеричной строки
    public class CSStr : CStr
    {
        private static readonly char[] ValidHexChars = "0123456789ABCDEFabcdef".ToCharArray();

        public CSStr() : base()
        {
        }

        public CSStr(string value) : base(ValidateHexString(value))
        {
        }

        private static string ValidateHexString(string value)
        {
            if (string.IsNullOrEmpty(value) || !value.All(c => ValidHexChars.Contains(c)))
            {
                return "0";
            }
            return value.ToUpper();
        }

        public int ToDecimal()
        {
            return Convert.ToInt32(Value, 16);
        }

        public void InvertSign()
        {
            int decimalValue = ToDecimal();
            decimalValue = -decimalValue;
            Value = decimalValue.ToString("X");
        }

        public static CSStr operator +(CSStr str1, CSStr str2)
        {
            int sum = str1.ToDecimal() + str2.ToDecimal();
            return new CSStr(sum.ToString("X"));
        }

        public static bool operator ==(CSStr str1, CSStr str2)
        {
            if (ReferenceEquals(str1, null))
                return ReferenceEquals(str2, null);
            return str1.ToDecimal() == str2.ToDecimal();
        }

        public static bool operator !=(CSStr str1, CSStr str2)
        {
            return !(str1 == str2);
        }

        public override bool Equals(object obj)
        {
            if (obj is CSStr other)
                return this == other;
            return false;
        }

        public override int GetHashCode()
        {
            return Value?.GetHashCode() ?? 0;
        }
    }

    public static class Task10Solution
    {
        public static string GetSolution()
        {
            StringBuilder result = new StringBuilder();

            try
            {
                result.AppendLine("Демонстрация работы с шестнадцатеричными строками:");

                CSStr str1 = new CSStr("1A");
                CSStr str2 = new CSStr("2B");
                CSStr invalidStr = new CSStr("GZ");

                result.AppendLine($"Строка 1: {str1}");
                result.AppendLine($"Строка 2: {str2}");
                result.AppendLine($"Недопустимая строка: {invalidStr}");

                result.AppendLine($"\nСтрока 1 в десятичном виде: {str1.ToDecimal()}");
                result.AppendLine($"Строка 2 в десятичном виде: {str2.ToDecimal()}");

                CSStr sum = str1 + str2;
                result.AppendLine($"\nСумма строк: {sum}");
                result.AppendLine($"Сумма в десятичном виде: {sum.ToDecimal()}");

                result.AppendLine($"\nСтроки равны: {str1 == str2}");

                str1.InvertSign();
                result.AppendLine($"\nСтрока 1 после инверсии знака: {str1}");
                result.AppendLine($"В десятичном виде: {str1.ToDecimal()}");
            }
            catch (Exception ex)
            {
                result.AppendLine($"Произошла ошибка: {ex.Message}");
            }
            finally
            {
                result.AppendLine("\nПрограмма завершена.");
            }

            return result.ToString();
        }
    }
}
