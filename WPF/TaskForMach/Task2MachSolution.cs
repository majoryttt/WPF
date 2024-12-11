using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WPF.TaskForMach;

public class Task2MachSolution
{
    public class Athlete
    {
        public string Name { get; set; }
        public string Sport { get; set; }
        public string Country { get; set; }
        public int BirthYear { get; set; }
        public string Position { get; set; }

        public override string ToString()
        {
            return $"{Name} ({Sport}, {Country}, {BirthYear}, {Position})";
        }
    }

    private static List<Athlete> athletes = new List<Athlete>
    {
        new Athlete { Name = "John Smith", Sport = "Basketball", Country = "USA", BirthYear = 1995, Position = "Forward" },
        new Athlete { Name = "Ivan Petrov", Sport = "Basketball", Country = "Russia", BirthYear = 1995, Position = "Guard" },
        new Athlete { Name = "Luis Garcia", Sport = "Football", Country = "Spain", BirthYear = 1998, Position = "Forward" },
        new Athlete { Name = "Mario Rossi", Sport = "Volleyball", Country = "Italy", BirthYear = 1996, Position = "Setter" },
        new Athlete { Name = "Yuki Tanaka", Sport = "Basketball", Country = "Japan", BirthYear = 1995, Position = "Center" },
        new Athlete { Name = "Carlos Silva", Sport = "Football", Country = "Brazil", BirthYear = 1997, Position = "Midfielder" },
        new Athlete { Name = "Pierre Dubois", Sport = "Basketball", Country = "France", BirthYear = 1996, Position = "Guard" },
        new Athlete { Name = "Alex Johnson", Sport = "Volleyball", Country = "USA", BirthYear = 1995, Position = "Libero" },
        new Athlete { Name = "Hans Weber", Sport = "Football", Country = "Germany", BirthYear = 1998, Position = "Defender" },
        new Athlete { Name = "Marco Rossi", Sport = "Volleyball", Country = "Italy", BirthYear = 1997, Position = "Spiker" }
    };

    private static bool[,] CreateRelationMatrix(Func<Athlete, Athlete, bool> relation)
    {
        int n = athletes.Count;
        bool[,] matrix = new bool[n, n];

        for (int i = 0; i < n; i++)
        {
            for (int j = 0; j < n; j++)
            {
                matrix[i, j] = relation(athletes[i], athletes[j]);
            }
        }

        return matrix;
    }

    private static bool IsReflexive(bool[,] matrix)
    {
        int n = matrix.GetLength(0);
        for (int i = 0; i < n; i++)
        {
            if (!matrix[i, i]) return false;
        }
        return true;
    }

    private static bool IsIrreflexive(bool[,] matrix)
    {
        int n = matrix.GetLength(0);
        for (int i = 0; i < n; i++)
        {
            if (matrix[i, i]) return false;
        }
        return true;
    }

    private static bool IsSymmetric(bool[,] matrix)
    {
        int n = matrix.GetLength(0);
        for (int i = 0; i < n; i++)
        {
            for (int j = 0; j < n; j++)
            {
                if (matrix[i, j] != matrix[j, i]) return false;
            }
        }
        return true;
    }

    private static bool IsAntisymmetric(bool[,] matrix)
    {
        int n = matrix.GetLength(0);
        for (int i = 0; i < n; i++)
        {
            for (int j = 0; j < n; j++)
            {
                if (i != j && matrix[i, j] && matrix[j, i]) return false;
            }
        }
        return true;
    }

    private static bool IsTransitive(bool[,] matrix)
    {
        int n = matrix.GetLength(0);
        for (int i = 0; i < n; i++)
        {
            for (int j = 0; j < n; j++)
            {
                for (int k = 0; k < n; k++)
                {
                    if (matrix[i, j] && matrix[j, k] && !matrix[i, k]) return false;
                }
            }
        }
        return true;
    }

    private static List<List<Athlete>> GetEquivalenceClasses(Func<Athlete, Athlete, bool> relation)
    {
        var classes = new List<List<Athlete>>();
        var processed = new HashSet<Athlete>();

        foreach (var athlete in athletes)
        {
            if (processed.Contains(athlete)) continue;

            var equivalenceClass = athletes.Where(a => relation(athlete, a)).ToList();
            classes.Add(equivalenceClass);
            processed.UnionWith(equivalenceClass);
        }

        return classes;
    }
    public static string GetSolution(bool[,] inputMatrix)
    {
        var sb = new StringBuilder();
    
        // Analyze properties of the input binary relation
        sb.AppendLine("Свойства введенного бинарного отношения:");
        sb.AppendLine($"Рефлексивность: {IsReflexive(inputMatrix)}");
        sb.AppendLine($"Иррефлексивность: {IsIrreflexive(inputMatrix)}");
        sb.AppendLine($"Симметричность: {IsSymmetric(inputMatrix)}");
        sb.AppendLine($"Антисимметричность: {IsAntisymmetric(inputMatrix)}");
        sb.AppendLine($"Транзитивность: {IsTransitive(inputMatrix)}");

        // Check if it's an equivalence or order relation
        bool isEquivalence = IsReflexive(inputMatrix) && IsSymmetric(inputMatrix) && IsTransitive(inputMatrix);
        bool isOrder = IsReflexive(inputMatrix) && IsAntisymmetric(inputMatrix) && IsTransitive(inputMatrix);
    
        sb.AppendLine($"\nЯвляется отношением эквивалентности: {isEquivalence}");
        sb.AppendLine($"Является отношением порядка: {isOrder}");

        return sb.ToString();
    }


private static void AnalyzeRelation(StringBuilder sb, string relationName, bool[,] matrix, Func<Athlete, Athlete, bool> relation)
{
    sb.AppendLine($"Свойства отношения \"{relationName}\":");
    sb.AppendLine($"Рефлексивность: {IsReflexive(matrix)}");
    sb.AppendLine($"Иррефлексивность: {IsIrreflexive(matrix)}");
    sb.AppendLine($"Симметричность: {IsSymmetric(matrix)}");
    sb.AppendLine($"Антисимметричность: {IsAntisymmetric(matrix)}");
    sb.AppendLine($"Транзитивность: {IsTransitive(matrix)}");

    bool isEquivalence = IsReflexive(matrix) && IsSymmetric(matrix) && IsTransitive(matrix);
    bool isOrder = IsReflexive(matrix) && IsAntisymmetric(matrix) && IsTransitive(matrix);
    
    sb.AppendLine($"\nЯвляется отношением эквивалентности: {isEquivalence}");
    sb.AppendLine($"Является отношением порядка: {isOrder}");

    if (isEquivalence)
    {
        sb.AppendLine($"\nКлассы эквивалентности по {relationName}:");
        var classes = GetEquivalenceClasses(relation);
        foreach (var group in classes)
        {
            sb.AppendLine($"\nГруппа {GetGroupValue(group[0], relationName)}:");
            foreach (var athlete in group)
            {
                sb.AppendLine($"- {athlete.Name}");
            }
        }
    }

    sb.AppendLine();
}

private static string GetGroupValue(Athlete athlete, string relation) => relation switch
{
    "Sport" => athlete.Sport,
    "Country" => athlete.Country,
    "BirthYear" => athlete.BirthYear.ToString(),
    _ => throw new ArgumentException("Invalid relation type")
};
}

