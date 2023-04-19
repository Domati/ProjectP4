using System;
using System.Reactive;
using ReactiveUI;

namespace ProjectP4.ViewModels
{
    public class ControlsViewModel : ViewModelBase
    {
        public BoardViewModel Board { get; set; }
        private int _amountBombs = 50;

        private int _maxBombs = 112;

        private int _rowsAndColumns = 15;

        private string _startOrResetButtonText = "Start";
        public bool NeedsReset { get; set; }

        public ControlsViewModel()
        {
            OnLoadBoardClicked = ReactiveCommand.Create(LoadBoard);
            OnStartClicked = ReactiveCommand.Create(StartOrReset);
        }

        public int AmountBombs
        {
            get => _amountBombs;
            set => this.RaiseAndSetIfChanged(ref _amountBombs, value);
        }

        public int RowsAndColumns
        {
            get => _rowsAndColumns;
            set
            {
                this.RaiseAndSetIfChanged(ref _rowsAndColumns, value);
                MaxBombs = (int) (Math.Pow(value, 2) / 2);
            }
        }

        public int MaxBombs
        {
            get => _maxBombs;
            set => this.RaiseAndSetIfChanged(ref _maxBombs, value);
        }

        public string StartOrResetButtonText
        {
            get => _startOrResetButtonText;
            set => this.RaiseAndSetIfChanged(ref _startOrResetButtonText, value);
        }

        public ReactiveCommand<Unit, Unit> OnLoadBoardClicked { get; }
        public ReactiveCommand<Unit, Unit> OnStartClicked { get; }


        private void LoadBoard()
        {
            if (Board.GameRunning) return;
            Board.RowsColumns = _rowsAndColumns;
            Board.AmountBombs = _amountBombs;
            Board.CreateBoard();
        }

        private void StartOrReset()
        {
            if (NeedsReset)
            {
                Board.Reset();
                NeedsReset = false;
            }
            if (StartOrResetButtonText == "Start")
            {
                Board.Start();
                StartOrResetButtonText = "Reset";
            }
            else
            {
                Board.Stop();
                StartOrResetButtonText = "Start";
            }
        }
    }
}