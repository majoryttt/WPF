using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;
using WPF.TaskForMach;
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

  private void
    SidebarSplitter_DragDelta(object sender,
      System.Windows.Controls.Primitives.DragDeltaEventArgs e) // Обработчик события перетаскивания
  {
    double newWidth = Sidebar.Width + e.HorizontalChange;
    Sidebar.Width = Math.Max(150, Math.Min(400, newWidth));
  }

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
    OOPSidebar.Visibility = Visibility.Visible;
    PinSidebar();
  }

  private void MenuItem2_Click(object sender, RoutedEventArgs e)
  {
    DiscreteMathSidebar.Visibility = Visibility.Visible;
    PinSidebar();
  }

  private void CloseSecondarySidebar_Click(object sender, RoutedEventArgs e)
  {
    DiscreteMathSidebar.Visibility = Visibility.Collapsed;
    OOPSidebar.Visibility = Visibility.Collapsed;
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
          result.AppendLine(
            $"Между событиями '{selectedEvents[i].EventName}' и '{selectedEvents[j].EventName}': {days} дней");
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
    Storyboard.SetTargetProperty(enterAnimation,
      new PropertyPath("(UIElement.RenderTransform).(TranslateTransform.X)"));
    enterStoryboard.Children.Add(enterAnimation);
    enterTrigger.Actions.Add(new BeginStoryboard { Storyboard = enterStoryboard });

    var leaveTrigger = new EventTrigger(MouseLeaveEvent);
    var leaveStoryboard = new Storyboard();
    var leaveAnimation = new DoubleAnimation(-140, TimeSpan.FromSeconds(0.2));
    Storyboard.SetTarget(leaveAnimation, Sidebar);
    Storyboard.SetTargetProperty(leaveAnimation,
      new PropertyPath("(UIElement.RenderTransform).(TranslateTransform.X)"));
    leaveStoryboard.Children.Add(leaveAnimation);
    leaveTrigger.Actions.Add(new BeginStoryboard { Storyboard = leaveStoryboard });

    Sidebar.Triggers.Add(enterTrigger);
    Sidebar.Triggers.Add(leaveTrigger);
  }

  private void RunPrimalityTest_Click(object sender, RoutedEventArgs e)
  {
    if (long.TryParse(PrimalityTestInput.Text, out long number))
    {
      ResultTextBlock.Text = Task4MachSolution.GetSolution(number);
      ResultTextBlock.Visibility = Visibility.Visible;
    }
    else
    {
      ResultTextBlock.Text = "Пожалуйста, введите корректное число";
      ResultTextBlock.Visibility = Visibility.Visible;
    }
  }

  private void RunSetOperations_Click(object sender, RoutedEventArgs e)
  {
    try
    {
      bool[,] relation1 = ParseRelationMatrix(Relation1Input.Text);
      bool[,] relation2 = ParseRelationMatrix(Relation2Input.Text);

      Task1ResultBlock.Text = Task1MachSolution.GetSolution(
        Set1Input.Text,
        Set2Input.Text,
        UniversalSetInput.Text,
        relation1,
        relation2
      );
    }
    catch (Exception ex)
    {
      Task1ResultBlock.Text = $"Ошибка: {ex.Message}";
    }
  }

  private bool[,] ParseRelationMatrix(string input)
  {
    var lines = input.Split('\n');
    int n = lines.Length;
    var matrix = new bool[n, n];

    for (int i = 0; i < n; i++)
    {
      var values = lines[i].Trim().Split(' ');
      for (int j = 0; j < n; j++)
      {
        matrix[i, j] = values[j] == "1";
      }
    }

    return matrix;
  }

  private void LoadEventsFromFile_Click(object sender, RoutedEventArgs e)
  {
    var openFileDialog = new Microsoft.Win32.OpenFileDialog
    {
      Filter = "Text files (*.txt)|*.txt|All files (*.*)|*.*"
    };

    if (openFileDialog.ShowDialog() == true)
    {
      try
      {
        var events = Task9Solution.LoadEventsFromFile(openFileDialog.FileName);
        EventsList.Items.Clear();
        foreach (var historicalEvent in events)
        {
          EventsList.Items.Add(historicalEvent);
        }

        ResultTextBlock.Text = "События успешно загружены из файла";
      }
      catch (Exception ex)
      {
        ResultTextBlock.Text = $"Ошибка при загрузке файла: {ex.Message}";
      }

      ResultTextBlock.Visibility = Visibility.Visible;
    }
  }

  private void SaveEventsToFile_Click(object sender, RoutedEventArgs e)
  {
    var saveFileDialog = new Microsoft.Win32.SaveFileDialog
    {
      Filter = "Text files (*.txt)|*.txt|All files (*.*)|*.*"
    };

    if (saveFileDialog.ShowDialog() == true)
    {
      try
      {
        var events = EventsList.Items.Cast<HistoricalEvent>().ToList();
        Task9Solution.SaveEventsToFile(events, saveFileDialog.FileName);
        ResultTextBlock.Text = "События успешно сохранены в файл";
      }
      catch (Exception ex)
      {
        ResultTextBlock.Text = $"Ошибка при сохранении файла: {ex.Message}";
      }

      ResultTextBlock.Visibility = Visibility.Visible;
    }
  }


  private void ReturnButton_Click(object sender, RoutedEventArgs e)
  {
    GetSolutionButton.Visibility = Visibility.Collapsed;
    ResultTextBlock.Visibility = Visibility.Collapsed;
    ReturnButton.Visibility = Visibility.Collapsed;
    InputPanel.Visibility = Visibility.Collapsed;
    Task8InputPanel.Visibility = Visibility.Collapsed;
    Task9InputPanel.Visibility = Visibility.Collapsed;
    Task4MachPanel.Visibility = Visibility.Collapsed;
    Task1MachPanel.Visibility = Visibility.Collapsed;
    Set1Input.Clear();
    Set2Input.Clear();
    UniversalSetInput.Clear();
    ResultTextBlock.Text = string.Empty;
    Relation1Input.Clear();
    Relation2Input.Clear();
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
      10 => Task10Solution.GetSolution(),
      15 => Task5MachSolution.GetSolution(), // Add this line
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

  private void Task10_Click(object sender, RoutedEventArgs e)
  {
    currentTask = 10;
    GetSolutionButton.Visibility = Visibility.Visible;
    ResultTextBlock.Visibility = Visibility.Collapsed;
    ReturnButton.Visibility = Visibility.Visible;
  }

  private void Task1MachSolution_Click(object sender, RoutedEventArgs e)
  {
    Task1MachPanel.Visibility = Visibility.Visible;
    ResultTextBlock.Visibility = Visibility.Collapsed;
    ReturnButton.Visibility = Visibility.Visible;
  }

  private void Task2MachSolution_Click(object sender, RoutedEventArgs e)
  {
    ResultTextBlock.Text = new Task2MachSolution().Solve();
    ResultTextBlock.Visibility = Visibility.Visible;
    ReturnButton.Visibility = Visibility.Visible;
  }

  private void Task3MachSolution_Click(object sender, RoutedEventArgs e)
  {
    ResultTextBlock.Text = Task3MachSolution.GetSolution();
    ResultTextBlock.Visibility = Visibility.Visible;
    ReturnButton.Visibility = Visibility.Visible;
  }

  private void Task4MachSolution_Click(object sender, RoutedEventArgs e)
  {
    Task4MachPanel.Visibility = Visibility.Visible;
    ResultTextBlock.Visibility = Visibility.Collapsed;
    ReturnButton.Visibility = Visibility.Visible;
  }

  private void Task5MachSolution_Click(object sender, RoutedEventArgs e)
  {
    currentTask = 15;
    GetSolutionButton.Visibility = Visibility.Visible;
    ResultTextBlock.Visibility = Visibility.Collapsed;
    ReturnButton.Visibility = Visibility.Visible;
  }
}
