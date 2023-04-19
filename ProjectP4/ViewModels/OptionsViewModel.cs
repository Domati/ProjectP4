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
    
    private int _amountMoves;
    private bool _forward = true;
    private int _currentMove;
    public void ControlsViewModel(BoardViewModel boardViewModel, Global global)
    {
        Global = global;
        Board = boardViewModel;
        OnLoadBoardClicked = ReactiveCommand.Create(LoadBoard);
        OnStartClicked = ReactiveCommand.Create(StartOrReset);
        _timer.Elapsed += TimerTick;
    }
    public Global Global { get; set; }
    private BoardViewModel Board { get; set; }
    public ReactiveCommand<Unit, Unit> OnLoadBoardClicked { get; }
    public ReactiveCommand<Unit, Unit> OnStartClicked { get; }
    
    public int CurrentMove
    {
        get => _currentMove;
        set
        {
            this.RaiseAndSetIfChanged(ref _currentMove, value);
            CurrentMoveHasChanged();
        }
    }
    public bool Forward
    {
        get => _forward;
        set => this.RaiseAndSetIfChanged(ref _forward, value);
    }

    public int AmountMoves
    {
        get => _amountMoves;
        set => this.RaiseAndSetIfChanged(ref _amountMoves, value);
    }
    
    private void CurrentMoveHasChanged()
    {
        if (Global.GameRunning) Board.ExecuteMove(Moves[CurrentMove]);
    }
    private void AutomaticHasChanged()
    {
        if (_automatic)
            _timer.Start();
        else
            _timer.Stop();
    }

    private void TimerTick(object sender, ElapsedEventArgs e)
    {
        Board.ExecuteMove(Moves[_currentMove]);

        if (Forward)
        {
            if (CurrentMove < AmountMoves) CurrentMove++;
        }
        else
        {
            if (CurrentMove > 0) CurrentMove--;
        }
    }

    private void Reset()
    {
        Automatic = false;
        Forward = true;
        Moves.Clear();
        AmountMoves = 0;
        CurrentMove = 0;
    }

    private void LoadBoard()
    {
        if (Global.GameRunning || Global.NeedsReset) return;
        Global.CurrentAmountBombs = Global.AmountBombs;
        Global.CurrentRowsAndColumns = Global.RowsAndColumns;
        Board.CreateBoard();
    }

    private void StartOrReset()
    {
        Global.SolverActive = _solverActive;

        if (Global.MainButtonStatus == MainButtonStatuses.Start)
            Start();
        else if (Global.MainButtonStatus == MainButtonStatuses.Reset)
            Stop();
        else if (Global.MainButtonStatus == MainButtonStatuses.Cancel) CancelSolver();
    }

    private void Start()
    {
        Board.Start();
        if (_solverActive) StartSolver();
        else
            Global.MainButtonStatus = MainButtonStatuses.Reset;

        AmountMoves--;
    }

    private void Stop()
    {
        _timer.Stop();
        Board.Stop();
        Reset();
        Global.MainButtonStatus = MainButtonStatuses.Start;
    }
    private CreationField[,] CloneCreationFields(CreationField[,] creationFields)
    {
        CreationField[,] fields = new CreationField[Global.CurrentRowsAndColumns, Global.CurrentRowsAndColumns];
        for (int y = 0; y < Global.CurrentRowsAndColumns; y++)
        for (int x = 0; x < Global.CurrentRowsAndColumns; x++)
        {
            CreationField creationField = new()
            {
                Position = new Point(y, x),
                Value = creationFields[y, x].Value,
                HasBomb = creationFields[y, x].HasBomb,
                IsCovered = creationFields[y, x].IsCovered,
                IsFlagged = creationFields[y, x].IsFlagged
            };
            fields[y, x] = creationField;
        }

        return fields;
    }
    
}


