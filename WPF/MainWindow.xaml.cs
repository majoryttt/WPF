using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;
using WPF.Tasks;

namespace WPF;

public partial class MainWindow : Window
{
    private bool isPinned = false;

    public MainWindow()
    {
        InitializeComponent();
        SidebarSplitter.DragDelta += SidebarSplitter_DragDelta; // Добавляем обработчик события перетаскивания
        AddHoverTriggers(); // Добавляем триггеры при запуске
    }

    private void MaximizeButton_Click(object sender, RoutedEventArgs e)
    {
        WindowState = WindowState == WindowState.Maximized ? WindowState.Normal : WindowState.Maximized;
    }

    private void CloseButton_Click(object sender, RoutedEventArgs e)
    {
        Close();
    }

    private void DragZone_MouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
    {
      DragMove();
    }

    private void SidebarSplitter_DragDelta(object sender, System.Windows.Controls.Primitives.DragDeltaEventArgs e) // Обработчик события перетаскивания
    {
      double newWidth = Sidebar.Width + e.HorizontalChange;
      Sidebar.Width = Math.Max(150, Math.Min(400, newWidth));    }

    private void PinButton_Click(object sender, RoutedEventArgs e) // Добавляем обработчик события нажатия на кнопку
    {
        isPinned = !isPinned;
        var transform = (TranslateTransform)Sidebar.RenderTransform;

        if (isPinned)
        {
            transform.X = 0;
            PinIcon.Data = Geometry.Parse("M2,2 L6,6 M2,6 L6,2");
            Sidebar.Triggers.Clear(); // Убираем триггеры при закреплении
        }
        else
        {
            transform.X = -190;
            PinIcon.Data = Geometry.Parse("M4,2 L4,8 M2,4 L6,4");
            AddHoverTriggers(); // Восстанавливаем триггеры при открепленни
        }
    }

  private void MenuItem1_Click(object sender, RoutedEventArgs e)
  {
      SecondarySidebar.Visibility = Visibility.Visible;
      PinSidebar();
  }

  private void MenuItem2_Click(object sender, RoutedEventArgs e)
  {
      SecondarySidebar.Visibility = Visibility.Visible;
      PinSidebar();
  }

  private void CloseSecondarySidebar_Click(object sender, RoutedEventArgs e)
  {
      SecondarySidebar.Visibility = Visibility.Collapsed;
      UnpinSidebar();
  }

  private void PinSidebar()
  {
      isPinned = true;
      var transform = (TranslateTransform)Sidebar.RenderTransform;
      transform.X = 0;
      PinIcon.Data = Geometry.Parse("M2,2 L6,6 M2,6 L6,2");
      Sidebar.Triggers.Clear();
  }

  private void UnpinSidebar()
  {
      isPinned = false;
      var transform = (TranslateTransform)Sidebar.RenderTransform;
      transform.X = -140;
      PinIcon.Data = Geometry.Parse("M4,2 L4,8 M2,4 L6,4");
      AddHoverTriggers();
  }

  private void AddEvent_Click(object sender, RoutedEventArgs e)
  {
    try
    {
      var newEvent = new HistoricalEvent(
        int.Parse(Task9DayInput.Text),
        int.Parse(Task9MonthInput.Text),
        int.Parse(Task9YearInput.Text),
        Task9EventInput.Text
      );

      EventsList.Items.Add(newEvent);

      Task9DayInput.Clear();
      Task9MonthInput.Clear();
      Task9YearInput.Clear();
      Task9EventInput.Clear();

      ResultTextBlock.Text = $"Событие успешно добавлено: {newEvent}";
      ResultTextBlock.Visibility = Visibility.Visible;
    }
    catch (Exception ex)
    {
      ResultTextBlock.Text = $"Ошибка: {ex.Message}";
      ResultTextBlock.Visibility = Visibility.Visible;
    }
  }

  private void CompareEvents_Click(object sender, RoutedEventArgs e)
  {
    var selectedEvents = EventsList.SelectedItems.Cast<HistoricalEvent>().ToList();

    if (selectedEvents.Count < 2)
    {
      ResultTextBlock.Text = "Выберите минимум два события для сравнения";
    }
    else
    {
      var latestEvent = HistoricalEvent.FindLatestEvent(new ObservableCollection<HistoricalEvent>(selectedEvents));
      var result = new System.Text.StringBuilder();
      result.AppendLine($"Самое позднее событие: {latestEvent}");

      for (int i = 0; i < selectedEvents.Count; i++)
      {
        for (int j = i + 1; j < selectedEvents.Count; j++)
        {
          var days = selectedEvents[i] - selectedEvents[j];
          result.AppendLine($"Между событиями '{selectedEvents[i].EventName}' и '{selectedEvents[j].EventName}': {days} дней");
        }
      }

      ResultTextBlock.Text = result.ToString();
    }

    ResultTextBlock.Visibility = Visibility.Visible;
  }

  private void DeleteEvents_Click(object sender, RoutedEventArgs e)
  {
    var selectedEvents = EventsList.SelectedItems.Cast<HistoricalEvent>().ToList();

    foreach (var eventToDelete in selectedEvents)
    {
      EventsList.Items.Remove(eventToDelete);
    }

    ResultTextBlock.Text = $"Удалено событий: {selectedEvents.Count}";
    ResultTextBlock.Visibility = Visibility.Visible;
  }

    private void AddHoverTriggers() // Настройка скрытия и раскрытия Sidebar при наведении
    {
        Sidebar.Triggers.Clear();

        var enterTrigger = new EventTrigger(MouseEnterEvent);
        var enterStoryboard = new Storyboard();
        var enterAnimation = new DoubleAnimation(0, TimeSpan.FromSeconds(0.2));
        Storyboard.SetTarget(enterAnimation, Sidebar);
        Storyboard.SetTargetProperty(enterAnimation, new PropertyPath("(UIElement.RenderTransform).(TranslateTransform.X)"));
        enterStoryboard.Children.Add(enterAnimation);
        enterTrigger.Actions.Add(new BeginStoryboard { Storyboard = enterStoryboard });

        var leaveTrigger = new EventTrigger(MouseLeaveEvent);
        var leaveStoryboard = new Storyboard();
        var leaveAnimation = new DoubleAnimation(-140, TimeSpan.FromSeconds(0.2));
        Storyboard.SetTarget(leaveAnimation, Sidebar);
        Storyboard.SetTargetProperty(leaveAnimation, new PropertyPath("(UIElement.RenderTransform).(TranslateTransform.X)"));
        leaveStoryboard.Children.Add(leaveAnimation);
        leaveTrigger.Actions.Add(new BeginStoryboard { Storyboard = leaveStoryboard });

        Sidebar.Triggers.Add(enterTrigger);
        Sidebar.Triggers.Add(leaveTrigger);
    }
    private void ReturnButton_Click(object sender, RoutedEventArgs e)
    {
      GetSolutionButton.Visibility = Visibility.Collapsed;
      ResultTextBlock.Visibility = Visibility.Collapsed;
      ReturnButton.Visibility = Visibility.Collapsed;
      InputPanel.Visibility = Visibility.Collapsed;
      Task8InputPanel.Visibility = Visibility.Collapsed;
      Task9InputPanel.Visibility = Visibility.Collapsed;  // Add this line
    }

    private int currentTask = 0;

    private void GetSolutionButton_Click(object sender, RoutedEventArgs e)
    {
      ResultTextBlock.Text = currentTask switch
      {
        1 => Task1Solution.GetSolution(),
        2 => Task2Solution.GetSolution(),
        4 => Task4Solution.GetSolution(FirstNumberInput.Text, SecondNumberInput.Text),
        5 => Task5Solution.GetSolution(),
        6 => Task6Solution.GetSolution(),
        7 => Task7Solution.GetSolution(),
        8 => Task8Solution.GetSolution(TextInput.Text, WordInput.Text),
        9 => Task9Solution.GetSolution(
          int.Parse(Task9DayInput.Text),
          int.Parse(Task9MonthInput.Text),
          int.Parse(Task9YearInput.Text),
          Task9EventInput.Text),
        _ => string.Empty
      };

      GetSolutionButton.Visibility = Visibility.Collapsed;
      ResultTextBlock.Visibility = Visibility.Visible;
    }

    private void Task1_Click(object sender, RoutedEventArgs e)
    {
        currentTask = 1;
        GetSolutionButton.Visibility = Visibility.Visible;
        ResultTextBlock.Visibility = Visibility.Collapsed;
        ReturnButton.Visibility = Visibility.Visible;
    }

    private void Task2_Click(object sender, RoutedEventArgs e)
    {
        currentTask = 2;
        GetSolutionButton.Visibility = Visibility.Visible;
        ResultTextBlock.Visibility = Visibility.Collapsed;
        ReturnButton.Visibility = Visibility.Visible;
    }

    private void Task4_Click(object sender, RoutedEventArgs e)
    {
        currentTask = 4;
        InputPanel.Visibility = Visibility.Visible;
        GetSolutionButton.Visibility = Visibility.Visible;
        ResultTextBlock.Visibility = Visibility.Collapsed;
        ReturnButton.Visibility = Visibility.Visible;
    }

    private void Task5_Click(object sender, RoutedEventArgs e)
    {
        currentTask = 5;
        GetSolutionButton.Visibility = Visibility.Visible;
        ResultTextBlock.Visibility = Visibility.Collapsed;
        ReturnButton.Visibility = Visibility.Visible;
    }

    private void Task6_Click(object sender, RoutedEventArgs e)
    {
        currentTask = 6;
        GetSolutionButton.Visibility = Visibility.Visible;
        ResultTextBlock.Visibility = Visibility.Collapsed;
        ReturnButton.Visibility = Visibility.Visible;
    }

    private void Task7_Click(object sender, RoutedEventArgs e)
    {
        currentTask = 7;
        GetSolutionButton.Visibility = Visibility.Visible;
        ResultTextBlock.Visibility = Visibility.Collapsed;
        ReturnButton.Visibility = Visibility.Visible;
    }

    private void Task8_Click(object sender, RoutedEventArgs e)
    {
        currentTask = 8;
        Task8InputPanel.Visibility = Visibility.Visible;
        GetSolutionButton.Visibility = Visibility.Visible;
        ResultTextBlock.Visibility = Visibility.Collapsed;
        ReturnButton.Visibility = Visibility.Visible;
    }

    private void Task9_Click(object sender, RoutedEventArgs e)
    {
      currentTask = 9;
      Task9InputPanel.Visibility = Visibility.Visible;
      ResultTextBlock.Visibility = Visibility.Collapsed;
      ReturnButton.Visibility = Visibility.Visible;
    }
}
