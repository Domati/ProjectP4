using System;
using System.Reactive;
using ReactiveUI;

namespace ProjectP4.ViewModels
{
    public class ControlsViewModel : ViewModelBase
    {
        public BoardViewModel Board { get; set; }
        private int _amountBombs = 25;

        private int _maxBombs = 112;

        private int _rowsAndColumns = 15;

        private string _startOrResetButtonText = "Start"; //text przycisku start
        public bool NeedsReset { get; set; }

        public ControlsViewModel()
        {
            OnLoadBoardClicked = ReactiveCommand.Create(LoadBoard);
            OnStartClicked = ReactiveCommand.Create(StartOrReset);
        }

        public int AmountBombs //liczba bomb podana przez użytkownika
        {
            get => _amountBombs;
            set => this.RaiseAndSetIfChanged(ref _amountBombs, value);
        }

        public int RowsAndColumns //funkcja do odczytywania liczby kolumn i obliczanie maksymalnej liczby bomb
        {
            get => _rowsAndColumns;
            set
            {
                this.RaiseAndSetIfChanged(ref _rowsAndColumns, value);
                MaxBombs = (int) (Math.Pow(value, 2) / 2); //maksymalna liczba bomb
            }
        }

        public int MaxBombs
        {
            get => _maxBombs;
            set => this.RaiseAndSetIfChanged(ref _maxBombs, value);
        }

        public string StartOrResetButtonText //przycisk startowania gry
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