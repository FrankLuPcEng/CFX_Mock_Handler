using System.Windows;
using CfxTestTool.Wpf.ViewModels;

namespace CfxTestTool.Wpf;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
        DataContext = new MainWindowViewModel();
    }
}
