using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;

namespace WPF.Tasks
{
    public static class Task9Solution
    {
        public static string GetSolution(int day, int month, int year, string eventName)
        {
            try
            {
                var events = new ObservableCollection<HistoricalEvent>
                {
                    new HistoricalEvent(day, month, year, eventName),
                    new HistoricalEvent(1, 1, 2000, "Миллениум"),
                    new HistoricalEvent(12, 4, 1961, "Полет Гагарина в космос"),
                    new HistoricalEvent(9, 5, 1945, "День Победы")
                };

                var latestEvent = HistoricalEvent.FindLatestEvent(events);
                var searchedEvent = HistoricalEvent.FindEventByName(events, eventName);
                var daysFromVictory = searchedEvent - events.First(e => e.EventName == "День Победы");

                return $"Введенное событие: {searchedEvent}\n" +
                       $"Самое позднее событие: {latestEvent}\n" +
                       $"Дней между введенным событием и Днем Победы: {daysFromVictory}";
            }
            catch (ArgumentException ex)
            {
                return $"Ошибка: {ex.Message}";
            }
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

        public HistoricalEvent()
        {
            day = 1;
            month = 1;
            year = 2000;
            eventName = "Unknown Event";
        }

        public HistoricalEvent(int day, int month, int year, string eventName)
        {
            if (!IsValidDate(day, month, year))
                throw new ArgumentException("Invalid date");

            this.day = day;
            this.month = month;
            this.year = year;
            this.eventName = eventName ?? throw new ArgumentNullException(nameof(eventName));
        }

        public HistoricalEvent(string eventName)
        {
            this.day = 1;
            this.month = 1;
            this.year = 2000;
            this.eventName = eventName ?? throw new ArgumentNullException(nameof(eventName));
        }

        public int Day
        {
            get => day;
            set
            {
                if (!IsValidDate(value, month, year))
                    throw new ArgumentException("Invalid day");
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
                    throw new ArgumentException("Invalid month");
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
                    throw new ArgumentException("Invalid year");
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
                throw new ArgumentException("Events collection is empty or null");

            return events.OrderByDescending(e => new DateTime(e.year, e.month, e.day)).First();
        }

        private bool IsValidDate(int day, int month, int year)
        {
            if (year < 1 || month < 1 || month > 12 || day < 1)
                return false;

            return day <= DateTime.DaysInMonth(year, month);
        }

        public static HistoricalEvent FindEventByName(ObservableCollection<HistoricalEvent> events, string eventName)
        {
            if (string.IsNullOrEmpty(eventName))
                throw new ArgumentException("Event name cannot be empty");

            return events.FirstOrDefault(e => e.eventName.Equals(eventName, StringComparison.OrdinalIgnoreCase));
        }

        public override string ToString()
        {
            return $"{eventName} ({day}/{month}/{year})";
        }
    }
}


