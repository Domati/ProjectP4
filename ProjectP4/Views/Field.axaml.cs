using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Markup.Xaml;
using ProjectP4.ViewModels;
namespace ProjectP4.Views;

public partial class Field : UserControl
{
    public Field()
    {
        InitializeComponent();
    }
    private void FieldPressed(object? sender, PointerPressedEventArgs e)
    {
        FieldViewModel fieldViewModel = (FieldViewModel) DataContext!;
        if (e.GetCurrentPoint(null).Properties.IsRightButtonPressed) fieldViewModel.FieldRightClicked();
        if (e.GetCurrentPoint(null).Properties.IsLeftButtonPressed) fieldViewModel.FieldLeftClicked();
    }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }
}