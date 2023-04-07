using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Markup.Xaml;

namespace ProjectP4.Views;

public partial class Options : Window
{
    
    public Options()
    {
        InitializeComponent();
#if DEBUG
        this.AttachDevTools();
#endif
    }

    private void InitializeComponent()
    {
        
        AvaloniaXamlLoader.Load(this);
    }
}