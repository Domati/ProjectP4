using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace ProjectP4.Views;

public partial class Board : UserControl
{
    public Board()
    {
        InitializeComponent();

    }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }
}