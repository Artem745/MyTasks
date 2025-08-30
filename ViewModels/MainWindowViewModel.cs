using System;
using System.Collections.ObjectModel;
using Avalonia.Controls;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace MyTasks.ViewModels;

public partial class MainWindowViewModel : ViewModelBase
{
    public MainWindowViewModel()
    {
        ObservableCollection<MyTask> myTasks = DBQueries.SelectDB(CurrentTab);
        LoadTasks(myTasks);
        LoadProgress(myTasks);
    }
    
    [ObservableProperty]
    private string _currentTab = "day";

    [ObservableProperty]
    private string _themeColor1 = "#404040";

    [ObservableProperty]
    private string _themeColor2 = "#4f4f4f";

    [ObservableProperty]
    private string _name = "";
    partial void OnNameChanged(string value)
    {
        AddTaskCommand.NotifyCanExecuteChanged();
    }

    [ObservableProperty]
    private string _description = "";

    [ObservableProperty]
    private ComboBoxItem? _difficulty = null;

    [ObservableProperty]
    private int _selectedIndexDifficulty = 0;

    [ObservableProperty]
    private ObservableCollection<MyTask> _tasksList = new();

    [ObservableProperty]
    private string _progressText = "Progress 0%";

    [ObservableProperty]
    private string _progressWidth = "0";

    private void LoadTasks(ObservableCollection<MyTask> myTasks)
    {
        TasksList.Clear();
        foreach (var task in myTasks)
        {
            TasksList.Add(task);
        }
    }

    private void LoadProgress(ObservableCollection<MyTask> items)
    {
        int TaskDone = 0;
        int TaskAll = items.Count;

        if (TaskAll == 0)
        {
            ProgressText = "Progress 0%";
            ProgressWidth = "0";
        }
        else
        {
            foreach (var item in items)
            {
                if (item.IsDone == "Undone")
                {
                    TaskDone += 1;
                }
            }
            int progress = TaskDone * 100 / items.Count;
            int progressWidth = progress * 660 / 100;
            ProgressText = $"Progress {progress}%";
            ProgressWidth = Convert.ToString(progressWidth);
        }
    }

    [RelayCommand]
    private void ChangeTheme(string themeColors)
    {
        string[] colors = themeColors.Split(",");
        ThemeColor1 = colors[0];
        ThemeColor2 = colors[1];
    }

    [RelayCommand]
    private void ChangeTab(string Tab)
    {
        CurrentTab = Tab;
        ObservableCollection<MyTask> myTasks = DBQueries.SelectDB(CurrentTab);
        LoadTasks(myTasks);
        LoadProgress(myTasks);
    }

    [RelayCommand(CanExecute = nameof(CanAdd))]
    private void AddTask()
    {
        string? Diff = Difficulty?.Content?.ToString();
        if (Diff != null && Diff == "Avalonia.Controls.TextBlock")
            Diff = "";
        DBQueries.InsertDB(Name, Description, Diff ?? "", CurrentTab);
        ObservableCollection<MyTask> myTasks = DBQueries.SelectDB(CurrentTab);
        Name = "";
        Description = "";
        SelectedIndexDifficulty = 0;
        LoadTasks(myTasks);
        LoadProgress(myTasks);
    }

    private bool CanAdd => !string.IsNullOrEmpty(Name);

    [RelayCommand]
    private void RemoveTask(string taskId)
    {
        DBQueries.DeleteDB(Convert.ToInt32(taskId));
        foreach (var item in TasksList)
        {
            if (item.Id == taskId)
            {
                TasksList.Remove(item);

                break;
            }
        }
        LoadProgress(DBQueries.SelectDB(CurrentTab));
    }

    [RelayCommand]
    private void DoneTask(string taskId)
    {
        DBQueries.SetDoneDB(Convert.ToInt32(taskId));
        ObservableCollection<MyTask> myTasks = DBQueries.SelectDB(CurrentTab);
        LoadTasks(myTasks);
        LoadProgress(myTasks);

        // foreach (var item in TasksList)
        // {
        //     if (item.Id == taskId)
        //     {
        // }
    }
}
