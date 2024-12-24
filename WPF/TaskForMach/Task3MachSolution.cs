using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WPF.TaskForMach
{
    public static class Task3MachSolution
    {
        public static string GetSolution()
        {
            StringBuilder result = new StringBuilder();
            try
            {
                result.AppendLine("Логическая функция:");
                result.AppendLine("F(x,y,z) = !x | (y & !z) | (!y & z)");
                result.AppendLine();

                bool[] values = { false, true };
                string[] variables = { "x", "y", "z" };

                var table = new List<(bool[] Values, bool Result)>();
                foreach (var x in values)
                {
                    foreach (var y in values)
                    {
                        foreach (var z in values)
                        {
                            bool[] val = { x, y, z };
                            bool expressionResult = EvaluateExpression(x, y, z);
                            table.Add((val, expressionResult));
                        }
                    }
                }

                var sdnfTerms = table.Where(row => row.Result)
                    .Select(row => row.Values.Select(b => b ? 1 : 0).ToArray())
                    .ToList();
                var sknfTerms = table.Where(row => !row.Result)
                    .Select(row => row.Values.Select(b => b ? 0 : 1).ToArray())
                    .ToList();

                string sdnf = string.Join(" | ", sdnfTerms.Select(term => $"({Term(term, variables)})"));
                string sknf = string.Join(" & ", sknfTerms.Select(term => $"({Term(term, variables)})"));

                var minSdnfTerms = QuineMcCluskey(sdnfTerms, variables);
                string minSdnf = string.Join(" | ", minSdnfTerms.Select(term => $"({term})"));

                result.AppendLine("Таблица истинности:");
                result.AppendLine(" x | y | z | Результат ");
                result.AppendLine("-----------------------");
                foreach (var row in table)
                {
                    result.AppendLine($" {Convert.ToInt32(row.Values[0])} | {Convert.ToInt32(row.Values[1])} | {Convert.ToInt32(row.Values[2])} |    {Convert.ToInt32(row.Result)}");
                }

                result.AppendLine($"\nСДНФ: {sdnf}");
                result.AppendLine($"СКНФ: {sknf}");
                result.AppendLine($"МДНФ: {minSdnf}");
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

        private static bool EvaluateExpression(bool x, bool y, bool z)
        {
            return !x || (y && !z) || (!y && z);
        }

        private static string Term(int[] values, string[] variables)
        {
            var terms = new List<string>();
            for (int i = 0; i < values.Length; i++)
            {
                if (values[i] == 1)
                    terms.Add(variables[i]);
                else if (values[i] == 0)
                    terms.Add($"!{variables[i]}");
            }
            return string.Join(" & ", terms);
        }

        private static List<string> QuineMcCluskey(List<int[]> minterms, string[] variables)
        {
            var primeImplicants = new List<int[]>();
            var newMinterms = new List<int[]>();

            while (minterms.Count > 0)
            {
                newMinterms.Clear();
                bool[] combined = new bool[minterms.Count];
                for (int i = 0; i < minterms.Count; i++)
                {
                    for (int j = i + 1; j < minterms.Count; j++)
                    {
                        int diffCount = 0;
                        int[] combinedMinterm = new int[minterms[i].Length];
                        for (int k = 0; k < minterms[i].Length; k++)
                        {
                            if (minterms[i][k] != minterms[j][k])
                            {
                                combinedMinterm[k] = -1;
                                diffCount++;
                            }
                            else
                            {
                                combinedMinterm[k] = minterms[i][k];
                            }
                        }
                        if (diffCount == 1)
                        {
                            combined[i] = true;
                            combined[j] = true;
                            newMinterms.Add(combinedMinterm);
                        }
                    }
                }
                for (int i = 0; i < minterms.Count; i++)
                {
                    if (!combined[i])
                        primeImplicants.Add(minterms[i]);
                }
                minterms = newMinterms.Distinct().ToList();
            }

            var terms = new List<string>();
            foreach (var minterm in primeImplicants)
            {
                var termParts = new List<string>();
                for (int i = 0; i < minterm.Length; i++)
                {
                    if (minterm[i] == 1)
                        termParts.Add(variables[i]);
                    else if (minterm[i] == 0)
                        termParts.Add($"!{variables[i]}");
                }
                terms.Add(string.Join(" & ", termParts));
            }

            return terms;
        }
    }
}
