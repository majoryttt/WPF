using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Runtime.CompilerServices;

namespace WPF.Tasks
{
    // Статический класс для работы с историческими событиями
    public static class Task9Solution
    {
        // Коллекция для хранения исторических событий
        // ObservableCollection - это коллекция, которая реализует интерфейс INotifyPropertyChanged
        private static ObservableCollection<HistoricalEvent> events = [];

        // Метод для добавления нового события и возврата результата операции
        public static string GetSolution(int day, int month, int year, string eventName)
        {
            try
            {
                var newEvent = new HistoricalEvent(day, month, year, eventName);
                events.Add(newEvent);
                return $"Событие успешно добавлено: {newEvent}";
            }
            catch (Exception ex)
            {
                return $"Ошибка: {ex.Message}";
            }
        }

        // Метод для сохранения событий в файл
        public static void SaveEventsToFile(List<HistoricalEvent> events, string filePath)
        {
          var lines = events.Select(e => $"{e.Day},{e.Month},{e.Year},{e.EventName}");
          File.WriteAllLines(filePath, lines);
        }

        // Метод для загрузки событий из файла
        public static List<HistoricalEvent> LoadEventsFromFile(string filePath)
        {
          var events = new List<HistoricalEvent>();
          var lines = File.ReadAllLines(filePath);

          foreach (var line in lines)
          {
            var parts = line.Split(',');
            if (parts.Length == 4)
            {
              events.Add(new HistoricalEvent(
                int.Parse(parts[0]),
                int.Parse(parts[1]),
                int.Parse(parts[2]),
                parts[3]
              ));
            }
          }

          return events;
        }
    }

    // Класс для представления исторического события
    // INotifyPropertyChanged - это интерфейс, который позволяет объектам уведомлять систему об изменениях своих свойств
    public class HistoricalEvent : INotifyPropertyChanged, IComparable<HistoricalEvent>, IComparer<HistoricalEvent>
    {
        // Приватные поля для хранения данных события
        private int day;
        private int month;
        private int year;
        private string eventName;

        // Реализация интерфейса INotifyPropertyChanged
        public event PropertyChangedEventHandler? PropertyChanged; // ? - это оператор null-объединения

        // Метод для уведомления об изменении свойства
        private void OnPropertyChanged([CallerMemberName] string propertyName = null) // Метод для уведомления об изменении свойства
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        // Конструктор с проверкой корректности даты
        public HistoricalEvent(int day, int month, int year, string eventName)
        {
            if (!IsValidDate(day, month, year))
                throw new ArgumentException("Некорректная дата");

            this.day = day;
            this.month = month;
            this.year = year;
            this.eventName = eventName ?? throw new ArgumentNullException(nameof(eventName));
        }

        // Свойства с проверкой корректности значений
        public int Day
        {
            get => day;
            set
            {
                if (!IsValidDate(value, month, year))
                    throw new ArgumentException("Некорректный день");
                day = value;
                OnPropertyChanged();
            }
        }

        public int Month
        {
            get => month;
            set
            {
                if (!IsValidDate(day, value, year))
                    throw new ArgumentException("Некорректный месяц");
                month = value;
                OnPropertyChanged();
            }
        }

        public int Year
        {
            get => year;
            set
            {
                if (!IsValidDate(day, month, value))
                    throw new ArgumentException("Некорректный год");
                year = value;
                OnPropertyChanged();
            }
        }

        public string EventName
        {
            get => eventName;
            set
            {
                eventName = value ?? throw new ArgumentNullException(nameof(value));
                OnPropertyChanged();
            }
        }

        public static int operator -(HistoricalEvent event1, HistoricalEvent event2)
        {
            DateTime date1 = new DateTime(event1.year, event1.month, event1.day);
            DateTime date2 = new DateTime(event2.year, event2.month, event2.day);
            return Math.Abs((date1 - date2).Days);
        }

        // Метод для поиска самого позднего события
        public static HistoricalEvent FindLatestEvent(ObservableCollection<HistoricalEvent> events)
        {
            if (events == null || events.Count == 0)
                throw new ArgumentException("Коллекция событий пуста");

            var latestEvent = events[0];
            for (int i = 1; i < events.Count; i++)
            {
                if (events[i] > latestEvent)
                    latestEvent = events[i];
            }
            return latestEvent;
        }

        // Метод проверки корректности даты
        private bool IsValidDate(int day, int month, int year)
        {
            if (year < 1 || month < 1 || month > 12 || day < 1)
                return false;

            return day <= DateTime.DaysInMonth(year, month);
        }

        // Переопределение метода ToString для удобного отображения события
        public override string ToString()
        {
            return $"{eventName} ({day}/{month}/{year})";
        }

        // Реализация IComparable для сравнения по умолчанию по дате
        // Разница между IComparable и IComparer в том, что IComparable позволяет сравнивать объекты одного типа, а IComparer позволяет сравнивать объекты разных типов.
        public int CompareTo(HistoricalEvent? other)
        {
            if (other == null) return 1;
            DateTime thisDate = new DateTime(year, month, day);
            DateTime otherDate = new DateTime(other.year, other.month, other.day);
            return thisDate.CompareTo(otherDate);
        }

        // Реализация IComparer для сравнения по имени события
        public int Compare(HistoricalEvent? x, HistoricalEvent? y)
        {
            if (x == null && y == null) return 0;
            if (x == null) return -1;
            if (y == null) return 1;
            return string.Compare(x.EventName, y.EventName, StringComparison.OrdinalIgnoreCase);
        }

        // Перегрузка операторов для сравнения
        public static bool operator >(HistoricalEvent left, HistoricalEvent right)
        {
            if (left == null || right == null)
                throw new ArgumentNullException();
            return left.CompareTo(right) > 0;
        }

        public static bool operator <(HistoricalEvent left, HistoricalEvent right)
        {
            if (left == null || right == null)
                throw new ArgumentNullException();
            return left.CompareTo(right) < 0;
        }
    }
}

