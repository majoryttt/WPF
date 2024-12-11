using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WPF.TaskForMach;

public class Task1MachSolution
{
    public static string GetSolution(string set1Input, string set2Input, string universalSetInput, 
        bool[,] relation1 = null, bool[,] relation2 = null)
    {
        var sb = new StringBuilder();
        
        // Разбирать входные строки в HashSets
        var set1 = ParseSet(set1Input);
        var set2 = ParseSet(set2Input);
        var universalSet = ParseSet(universalSetInput);

        // Установить операции
        sb.AppendLine("Операции над множествами:");
        sb.AppendLine($"Множество A: {{{string.Join(", ", set1)}}}");
        sb.AppendLine($"Множество B: {{{string.Join(", ", set2)}}}");
        sb.AppendLine($"Универсальное множество U: {{{string.Join(", ", universalSet)}}}");
        
        sb.AppendLine($"\nПересечение A ∩ B: {{{string.Join(", ", Intersection(set1, set2))}}}");
        sb.AppendLine($"Объединение A ∪ B: {{{string.Join(", ", Union(set1, set2))}}}");
        sb.AppendLine($"Разность A \\ B: {{{string.Join(", ", Difference(set1, set2))}}}");
        sb.AppendLine($"Симметрическая разность A △ B: {{{string.Join(", ", SymmetricDifference(set1, set2))}}}");
        sb.AppendLine($"Дополнение A̅: {{{string.Join(", ", Complement(set1, universalSet))}}}");
        sb.AppendLine($"Дополнение B̅: {{{string.Join(", ", Complement(set2, universalSet))}}}");
        
        sb.AppendLine("\nДекартово произведение A × B:");
        foreach (var pair in CartesianProduct(set1, set2))
        {
            sb.AppendLine($"({pair.Item1}, {pair.Item2})");
        }

        // Операции с двоичными отношениями (если предусмотрены)
        if (relation1 != null && relation2 != null)
        {
            sb.AppendLine("\nОперации над бинарными отношениями:");
            sb.AppendLine("Обращение отношения R1:");
            sb.AppendLine(MatrixToString(InverseRelation(relation1)));
            
            sb.AppendLine("\nКомпозиция отношений R1 ∘ R2:");
            sb.AppendLine(MatrixToString(ComposeRelations(relation1, relation2)));
        }

        return sb.ToString();
    }

    private static HashSet<int> ParseSet(string input)
    {
        return new HashSet<int>(input.Split(new[] { ' ', ',', '{', '}' }, 
            StringSplitOptions.RemoveEmptyEntries).Select(int.Parse));
    }

    private static HashSet<int> Intersection(HashSet<int> set1, HashSet<int> set2) 
        => new(set1.Intersect(set2));
    
    private static HashSet<int> Union(HashSet<int> set1, HashSet<int> set2) 
        => new(set1.Union(set2));
    
    private static HashSet<int> Difference(HashSet<int> set1, HashSet<int> set2) 
        => new(set1.Except(set2));
    
    private static HashSet<int> SymmetricDifference(HashSet<int> set1, HashSet<int> set2) 
        => new(set1.Except(set2).Union(set2.Except(set1)));
    
    private static HashSet<int> Complement(HashSet<int> set, HashSet<int> universalSet) 
        => new(universalSet.Except(set));
    
    private static List<(int, int)> CartesianProduct(HashSet<int> set1, HashSet<int> set2)
    {
        var result = new List<(int, int)>();
        foreach (var x in set1)
            foreach (var y in set2)
                result.Add((x, y));
        return result;
    }

    private static bool[,] InverseRelation(bool[,] relation)
    {
        int n = relation.GetLength(0);
        var result = new bool[n, n];
        for (int i = 0; i < n; i++)
            for (int j = 0; j < n; j++)
                result[i, j] = relation[j, i];
        return result;
    }

    private static bool[,] ComposeRelations(bool[,] r1, bool[,] r2)
    {
        int n = r1.GetLength(0);
        var result = new bool[n, n];
        for (int i = 0; i < n; i++)
            for (int j = 0; j < n; j++)
                for (int k = 0; k < n; k++)
                    if (r1[i, k] && r2[k, j])
                    {
                        result[i, j] = true;
                        break;
                    }
        return result;
    }

    private static string MatrixToString(bool[,] matrix)
    {
        var sb = new StringBuilder();
        int n = matrix.GetLength(0);
        
        for (int i = 0; i < n; i++)
        {
            for (int j = 0; j < n; j++)
            {
                sb.Append(matrix[i, j] ? "1 " : "0 ");
            }
            sb.AppendLine();
        }
        
        return sb.ToString();
    }
}
