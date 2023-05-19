using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using ProjectP4.Views;

namespace ProjectP4.ViewModels;

public class GameWindowViewModel : ViewModelBase
{
    public GameWindowViewModel(GameMainWindow w) //tworzenie widoku okna gry
    {
        Window = w;
        InfoTextViewModel = new InfoTextViewModel();
        ControlsViewModel = new ControlsViewModel();
        BoardViewModel = new BoardViewModel(InfoTextViewModel, ControlsViewModel);
        ControlsViewModel.Board = BoardViewModel;
        
    }
    
    
    public BoardViewModel BoardViewModel { get; set; }
    public InfoTextViewModel InfoTextViewModel { get; set; }
    public ControlsViewModel ControlsViewModel { get; set; }
   

    public GameMainWindow Window;

    public void Exit()
    {
        Window.Close();
    }

}