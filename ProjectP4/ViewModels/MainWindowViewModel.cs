using System.Runtime.InteropServices.JavaScript;
using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;



namespace ProjectP4.ViewModels;

public class MainWindowViewModel : ViewModelBase
{
    public string Greeting => "TESTOWY";

    public void Newgame()
    {
        
    }
    public void Options()
    {
        
    }
    public void Exit()
    {
        if(Application.Current?.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime lifetime) lifetime.Shutdown(); 
    }
}