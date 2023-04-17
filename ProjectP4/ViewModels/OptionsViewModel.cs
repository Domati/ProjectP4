using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive;
using System.Threading;
using System.Timers;
using ProjectP4.Models;
using ReactiveUI;
using Timer = System.Timers.Timer;
namespace ProjectP4.ViewModels;

public class OptionsViewModel : ViewModelBase
{
    private readonly Timer _timer = new();
    public void ControlsViewModel(BoardViewModel boardViewModel, Global global)
    {
        Global = global;
        Board = boardViewModel;
        
    }
    public Global Global { get; set; }
    private BoardViewModel Board { get; set; }
    public ReactiveCommand<Unit, Unit> OnLoadBoardClicked { get; }
    public ReactiveCommand<Unit, Unit> OnStartClicked { get; }
}


