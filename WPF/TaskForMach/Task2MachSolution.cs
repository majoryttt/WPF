using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public class Task2MachSolution
{
    public string Solve()
    {
        StringBuilder result = new StringBuilder();

        var relation = new List<Tuple<int, int>>()
        {
            Tuple.Create(1, 1),
            Tuple.Create(2, 2),
            Tuple.Create(3, 3),
            Tuple.Create(1, 2),
            Tuple.Create(2, 1),
            Tuple.Create(2, 3),
            Tuple.Create(3, 2),
            Tuple.Create(1, 3),
            Tuple.Create(3, 1)

        };

        var br = new BinaryRelation(relation);
        result.AppendLine(br.GetRelationString());
        result.AppendLine(br.GetRelationTypeString());

        var movies = GetMoviesList();
        var ec = new EquivalenceClasses(movies);
        result.AppendLine(ec.GetEquivalenceClassesString());

        return result.ToString();
    }

    private class BinaryRelation
    {
        private List<Tuple<int, int>> relation;
        private HashSet<int> elements;

        public BinaryRelation(List<Tuple<int, int>> relation)
        {
            this.relation = relation;
            this.elements = new HashSet<int>();

            foreach (var pair in relation)
            {
                elements.Add(pair.Item1);
                elements.Add(pair.Item2);
            }
        }

        public string GetRelationString()
        {
            StringBuilder sb = new StringBuilder("Отношение:\n");
            foreach (var pair in relation)
            {
                sb.AppendLine($"({pair.Item1}, {pair.Item2})");
            }
            return sb.ToString();
        }

        private bool IsReflexive()
        {
            foreach (var element in elements)
            {
                if (!relation.Contains(Tuple.Create(element, element)))
                    return false;
            }
            return true;
        }

        private bool IsIrreflexive()
        {
            foreach (var element in elements)
            {
                if (relation.Contains(Tuple.Create(element, element)))
                    return false;
            }
            return true;
        }

        private bool IsSymmetric()
        {
            foreach (var pair in relation)
            {
                if (!relation.Contains(Tuple.Create(pair.Item2, pair.Item1)))
                    return false;
            }
            return true;
        }

        private bool IsAntisymmetric()
        {
            foreach (var pair in relation)
            {
                if (pair.Item1 != pair.Item2 && relation.Contains(Tuple.Create(pair.Item2, pair.Item1)))
                    return false;
            }
            return true;
        }

        private bool IsTransitive()
        {
            foreach (var pair1 in relation)
            {
                foreach (var pair2 in relation)
                {
                    if (pair1.Item2 == pair2.Item1)
                    {
                        if (!relation.Contains(Tuple.Create(pair1.Item1, pair2.Item2)))
                            return false;
                    }
                }
            }
            return true;
        }

        public string GetRelationTypeString()
        {
            StringBuilder sb = new StringBuilder("\nПроверка свойств отношения:\n\n");
            sb.AppendLine($"Рефлексивность: {IsReflexive()}");
            sb.AppendLine($"Антирефлексивность: {IsIrreflexive()}");
            sb.AppendLine($"Симметричность: {IsSymmetric()}");
            sb.AppendLine($"Антисимметричность: {IsAntisymmetric()}");
            sb.AppendLine($"Транзитивность: {IsTransitive()}");

            if (IsReflexive() && IsTransitive() && IsAntisymmetric())
                sb.AppendLine("\nОтношение является отношением частичного порядка.");
            else if (IsReflexive() && IsSymmetric() && IsTransitive())
                sb.AppendLine("\nОтношение является отношением эквивалентности.");
            else
                sb.AppendLine("\nОтношение не является ни отношением частичного порядка, ни отношением эквивалентности.");

            return sb.ToString();
        }
    }

    private class Movie
    {
        public string Title { get; set; }
        public int Year { get; set; }
        public string Genre { get; set; }
        public string Type { get; set; }

        public Movie(string title, int year, string genre, string type)
        {
            Title = title;
            Year = year;
            Genre = genre;
            Type = type;
        }

        public override string ToString()
        {
            return $"{Title} ({Year}), {Genre}, {Type}";
        }
    }

    private class EquivalenceClasses
    {
        private List<Movie> movies;
        private Dictionary<string, List<int>> equivalenceClassesByGenre;

        public EquivalenceClasses(List<Movie> movies)
        {
            this.movies = movies;
            this.equivalenceClassesByGenre = new Dictionary<string, List<int>>();
            GenerateEquivalenceClasses();
        }

        private void GenerateEquivalenceClasses()
        {
            for (int i = 0; i < movies.Count; i++)
            {
                if (!equivalenceClassesByGenre.ContainsKey(movies[i].Genre))
                    equivalenceClassesByGenre[movies[i].Genre] = new List<int>();

                equivalenceClassesByGenre[movies[i].Genre].Add(i);
            }
        }

        public string GetEquivalenceClassesString()
        {
            StringBuilder sb = new StringBuilder("\nКлассы эквивалентности:\n");
            int classIndex = 0;
            foreach (var genreClass in equivalenceClassesByGenre)
            {
                sb.AppendLine($"Класс {classIndex}:");
                foreach (var movieIndex in genreClass.Value)
                {
                    sb.AppendLine($"  - {movies[movieIndex]}");
                }
                classIndex++;
            }
            return sb.ToString();
        }
    }

    private List<Movie> GetMoviesList()
    {
        return new List<Movie>()
        {
            new Movie("The Shawshank Redemption", 1994, "Drama", "Film"),
            new Movie("The Godfather", 1972, "Crime", "Film"),
            new Movie("Toy Story", 1995, "Family", "Animation"),
            new Movie("The Dark Knight", 2008, "Action", "Film"),
            new Movie("Pulp Fiction", 1994, "Crime", "Film"),
            new Movie("Forrest Gump", 1994, "Drama", "Film"),
            new Movie("The Lion King", 1994, "Family", "Animation"),
            new Movie("Inception", 2010, "Sci-Fi", "Film"),
            new Movie("The Matrix", 1999, "Sci-Fi", "Film"),
            new Movie("Spirited Away", 2001, "Fantasy", "Animation"),
            new Movie("Goodfellas", 1990, "Crime", "Film"),
            new Movie("The Silence of the Lambs", 1991, "Thriller", "Film"),
            new Movie("Finding Nemo", 2003, "Family", "Animation"),
            new Movie("Saving Private Ryan", 1998, "War", "Film"),
            new Movie("Shrek", 2001, "Family", "Animation")
        };
    }
}
