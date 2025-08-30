using Avalonia.Controls;
using MyTasks.ViewModels;

namespace MyTasks.Views;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
        // DataContext = new MainWindowViewModel();
    }
}