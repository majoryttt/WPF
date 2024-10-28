using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace WPF;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
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
    }
    private int currentTask = 0;

    private void GetSolutionButton_Click(object sender, RoutedEventArgs e) // Обработчик события нажатия на кнопку "Получить решение"
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
            _ => string.Empty
        };

        GetSolutionButton.Visibility = Visibility.Collapsed; // Скрываем кнопку "Получить решение"
        ResultTextBlock.Visibility = Visibility.Visible; // Показываем блок с результатом
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
}



