using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public class Task2MachSolution
{
    // Основной метод решения задачи
    public string Solve()
    {
        StringBuilder result = new StringBuilder();

        // Создание бинарного отношения в виде пар чисел
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

        // Создание объекта для анализа бинарного отношения
        var br = new BinaryRelation(relation);
        result.AppendLine(br.GetRelationString());
        result.AppendLine(br.GetRelationTypeString());

        // Создание и анализ классов эквивалентности для списка фильмов
        var movies = GetMoviesList();
        var ec = new EquivalenceClasses(movies);
        result.AppendLine(ec.GetEquivalenceClassesString());

        return result.ToString();
    }

    // Класс для работы с бинарными отношениями
    private class BinaryRelation
    {
        private List<Tuple<int, int>> relation;
        private HashSet<int> elements; // Множество уникальных элементов отношения

        // Конструктор класса
        public BinaryRelation(List<Tuple<int, int>> relation)
        {
            this.relation = relation;
            this.elements = new HashSet<int>();

            // Заполнение множества уникальных элементов
            foreach (var pair in relation)
            {
                elements.Add(pair.Item1);
                elements.Add(pair.Item2);
            }
        }

        // Метод для вывода отношения в строковом виде
        public string GetRelationString()
        {
            StringBuilder sb = new StringBuilder("Отношение:\n");
            foreach (var pair in relation)
            {
                sb.AppendLine($"({pair.Item1}, {pair.Item2})");
            }
            return sb.ToString();
        }

        // Проверка рефлексивности отношения
        // Рефлексивность - это свойство, при котором каждый элемент находится в отношении с самим собой.
        private bool IsReflexive()
        {
            foreach (var element in elements)
            {
                if (!relation.Contains(Tuple.Create(element, element)))
                    return false;
            }
            return true;
        }

        // Проверка антирефлексивности отношения
        // Антирефлексивность - это свойство, при котором ни один элемент не находится в отношении с самим собой.
        private bool IsIrreflexive()
        {
            foreach (var element in elements)
            {
                if (relation.Contains(Tuple.Create(element, element)))
                    return false;
            }
            return true;
        }

        // Проверка симметричности отношения
        // Симметричность - это свойство, при котором если элемент a находится в отношении с элементом b, то элемент b находится в отношении с элементом a.
        private bool IsSymmetric()
        {
            foreach (var pair in relation)
            {
                if (!relation.Contains(Tuple.Create(pair.Item2, pair.Item1)))
                    return false;
            }
            return true;
        }

        // Проверка антисимметричности отношения
        // Антисимметричность - это свойство, при котором если элемент a находится в отношении с элементом b, то элемент b не может находиться в отношении с элементом a, если a не равно b.
        private bool IsAntisymmetric()
        {
            foreach (var pair in relation)
            {
                if (pair.Item1 != pair.Item2 && relation.Contains(Tuple.Create(pair.Item2, pair.Item1)))
                    return false;
            }
            return true;
        }

        // Проверка транзитивности отношения
        // Транзитивность - это свойство, при котором если элемент a находится в отношении с элементом b, а элемент b находится в отношении с элементом c, то элемент a находится в отношении с элементом c.
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

        // Метод для определения типа отношения и вывода результатов проверки свойств
        public string GetRelationTypeString()
        {
            StringBuilder sb = new StringBuilder("\nПроверка свойств отношения:\n\n");
            sb.AppendLine($"Рефлексивность: {IsReflexive()}");
            sb.AppendLine($"Антирефлексивность: {IsIrreflexive()}");
            sb.AppendLine($"Симметричность: {IsSymmetric()}");
            sb.AppendLine($"Антисимметричность: {IsAntisymmetric()}");
            sb.AppendLine($"Транзитивность: {IsTransitive()}");

            // Определение типа отношения на основе свойств
            if (IsReflexive() && IsTransitive() && IsAntisymmetric())
                sb.AppendLine("\nОтношение является отношением частичного порядка.");
            else if (IsReflexive() && IsSymmetric() && IsTransitive())
                sb.AppendLine("\nОтношение является отношением эквивалентности.");
            else
                sb.AppendLine("\nОтношение не является ни отношением частичного порядка, ни отношением эквивалентности.");

            return sb.ToString();
        }
    }

    // Класс для представления фильма
    private class Movie
    {
        public string Title { get; set; }
        public int Year { get; set; }
        public string Genre { get; set; }
        public string Type { get; set; }

        // Конструктор класса Movie
        public Movie(string title, int year, string genre, string type)
        {
            Title = title;
            Year = year;
            Genre = genre;
            Type = type;
        }

        // Переопределение метода ToString для удобного вывода информации о фильме
        public override string ToString()
        {
            return $"{Title} ({Year}), {Genre}, {Type}";
        }
    }

    // Класс для работы с классами эквивалентности
    private class EquivalenceClasses
    {
        private List<Movie> movies;
        private Dictionary<string, List<int>> equivalenceClassesByGenre;

        // Конструктор класса
        public EquivalenceClasses(List<Movie> movies)
        {
            this.movies = movies;
            this.equivalenceClassesByGenre = new Dictionary<string, List<int>>();
            GenerateEquivalenceClasses();
        }

        // Метод для генерации классов эквивалентности по жанрам
        private void GenerateEquivalenceClasses()
        {
            for (int i = 0; i < movies.Count; i++)
            {
                if (!equivalenceClassesByGenre.ContainsKey(movies[i].Genre))
                    equivalenceClassesByGenre[movies[i].Genre] = new List<int>();

                equivalenceClassesByGenre[movies[i].Genre].Add(i);
            }
        }

        // Метод для вывода классов эквивалентности в строковом виде
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

    // Метод для создания тестового списка фильмов
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
