namespace ProjectP4.ViewModels;

public class GameWindowViewModel : ViewModelBase
{
    public GameWindowViewModel() //tworzenie widoku okna gry
    {
        InfoTextViewModel = new InfoTextViewModel();
        ControlsViewModel = new ControlsViewModel();
        BoardViewModel = new BoardViewModel(InfoTextViewModel, ControlsViewModel);
        ControlsViewModel.Board = BoardViewModel;
    }
    
    public BoardViewModel BoardViewModel { get; set; }
    public InfoTextViewModel InfoTextViewModel { get; set; }
    public ControlsViewModel ControlsViewModel { get; set; }
}