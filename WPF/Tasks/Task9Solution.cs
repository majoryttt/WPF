using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;

namespace WPF.Tasks
{
    public static class Task9Solution
    {
        private static ObservableCollection<HistoricalEvent> events = new ObservableCollection<HistoricalEvent>();

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

        public static void AddEvent(HistoricalEvent newEvent)
        {
            events.Add(newEvent);
        }

        public static string CompareEvents(List<HistoricalEvent> selectedEvents)
        {
            if (selectedEvents.Count < 2)
                return "Выберите минимум два события для сравнения";

            var result = new System.Text.StringBuilder();
            var latestEvent = HistoricalEvent.FindLatestEvent(new ObservableCollection<HistoricalEvent>(selectedEvents));
            result.AppendLine($"Самое позднее событие: {latestEvent}");

            for (int i = 0; i < selectedEvents.Count; i++)
            {
                for (int j = i + 1; j < selectedEvents.Count; j++)
                {
                    var days = selectedEvents[i] - selectedEvents[j];
                    result.AppendLine($"Между событиями '{selectedEvents[i].EventName}' и '{selectedEvents[j].EventName}': {days} дней");
                }
            }

            return result.ToString();
        }
    }

    public class HistoricalEvent : INotifyPropertyChanged
    {
        private int day;
        private int month;
        private int year;
        private string eventName;

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public HistoricalEvent(int day, int month, int year, string eventName)
        {
            if (!IsValidDate(day, month, year))
                throw new ArgumentException("Некорректная дата");

            this.day = day;
            this.month = month;
            this.year = year;
            this.eventName = eventName ?? throw new ArgumentNullException(nameof(eventName));
        }

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

        public static HistoricalEvent FindLatestEvent(ObservableCollection<HistoricalEvent> events)
        {
            if (events == null || events.Count == 0)
                throw new ArgumentException("Коллекция событий пуста");

            return events.OrderByDescending(e => new DateTime(e.year, e.month, e.day)).First();
        }

        private bool IsValidDate(int day, int month, int year)
        {
            if (year < 1 || month < 1 || month > 12 || day < 1)
                return false;

            return day <= DateTime.DaysInMonth(year, month);
        }

        public override string ToString()
        {
            return $"{eventName} ({day}/{month}/{year})";
        }
    }
}
