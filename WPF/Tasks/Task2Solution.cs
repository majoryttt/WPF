using System.Text;

namespace WPF
{
    public static class Task2Solution
    {
        public static string GetSolution()
        {
            StringBuilder result = new StringBuilder();

            result.AppendLine($"sizeof(bool) = {sizeof(bool)}");
            result.AppendLine($"sizeof(char) = {sizeof(char)}");
            result.AppendLine($"sizeof(sbyte) = {sizeof(sbyte)}");
            result.AppendLine($"sizeof(byte) = {sizeof(byte)}");
            result.AppendLine($"sizeof(int) = {sizeof(int)}");
            result.AppendLine($"sizeof(uint) = {sizeof(uint)}");
            result.AppendLine($"sizeof(short) = {sizeof(short)}");
            result.AppendLine($"sizeof(ushort) = {sizeof(ushort)}");
            result.AppendLine($"sizeof(long) = {sizeof(long)}");
            result.AppendLine($"sizeof(ulong) = {sizeof(ulong)}");
            result.AppendLine($"sizeof(float) = {sizeof(float)}");
            result.AppendLine($"sizeof(double) = {sizeof(double)}");
            result.AppendLine($"sizeof(decimal) = {sizeof(decimal)}");
            result.AppendLine($"sizeof(string) = {IntPtr.Size}");
            result.AppendLine($"sizeof(object) = {IntPtr.Size}");

            int a = 10;
            int b = 3;
            double divResult = (double)a / b;
            result.AppendLine($"\nРезультат деления {a} на {b} равен {divResult}");

            char symbol = 'A';
            int asciiCode = (int)symbol;
            char previousSymbol = (char)(asciiCode - 1);
            char nextSymbol = (char)(asciiCode + 1);
            result.AppendLine($"\nСимвол: {symbol}");
            result.AppendLine($"ASCII-код: {asciiCode}");
            result.AppendLine($"Предыдущий символ: {previousSymbol}");
            result.AppendLine($"Следующий символ: {nextSymbol}");

            return result.ToString();
        }
    }
}
