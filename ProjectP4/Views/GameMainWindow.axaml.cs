using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using ProjectP4.ViewModels;

namespace ProjectP4.Views;

public partial class GameMainWindow : Window
{
    public GameMainWindow()
    {
        InitializeComponent();
        DataContext = new GameWindowViewModel();
#if DEBUG
        this.AttachDevTools();
#endif
    }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }
}