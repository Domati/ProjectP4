using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices.JavaScript;
using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using ProjectP4.Views;


namespace ProjectP4.ViewModels;



public class MainWindowViewModel : ViewModelBase
{
    public string Greeting => "TESTOWY";

    public void Newgame()
    {
        var newWindow = new GameMainWindow();
        newWindow.Title = "GameMainWindow";
        newWindow.Show();
    }
   
    public void Exit()
    {
        if(Application.Current?.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime lifetime) lifetime.Shutdown(); 
    }
}
