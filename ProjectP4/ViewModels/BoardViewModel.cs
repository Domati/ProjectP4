using System;
using System.Collections.Generic;
using System.Linq;
using Avalonia.Media;
using DynamicData.Binding;
using ProjectP4.Models;
using ReactiveUI;

namespace ProjectP4.ViewModels
{
    public class BoardViewModel : ViewModelBase
    {
        private readonly List<FieldViewModel> _bombs = new(); // Performance
        private readonly InfoTextViewModel _infoTextViewModel;
        private readonly ControlsViewModel _controlsViewModel;
        private readonly Random _rnd = new();
        private int _amountBombs = 50;

        private int _rowsColumns = 15;
        private DateTime _startTime;
        public bool BombsArePlaced;
        private bool _gameRunning;
        public bool GameRunning
        {
            get => _gameRunning;
            private set
            {
                _gameRunning = value;
                _controlsViewModel.StartOrResetButtonText = value ? "Reset" : "Start";
            }
        }

        public BoardViewModel(InfoTextViewModel infoTextViewModel, ControlsViewModel controlsViewModel)
        {
            _controlsViewModel = controlsViewModel;
            _infoTextViewModel = infoTextViewModel;
            CreateBoard();
        }

        public int FlagsSet
        {
            get => _infoTextViewModel.FlagsSet;
            set => _infoTextViewModel.FlagsSet = value;
        }

        public int AmountBombs
        {
            get => _amountBombs;
            set => this.RaiseAndSetIfChanged(ref _amountBombs, value);
        }

        public int RowsColumns
        {
            get => _rowsColumns;
            set => this.RaiseAndSetIfChanged(ref _rowsColumns, value);
        }

        public ObservableCollectionExtended<FieldViewModel> Fields { get; set; } = new();

        private FieldViewModel? GetField(int x, int y)
        {
            if (x < 0 || y < 0 || x >= _rowsColumns || y >= _rowsColumns)
                return null;

            int index = RowsColumns * y + x;

            if (index >= Fields.Count || index < 0)
                return null;

            FieldViewModel field = Fields[index];
            return field;
        }

        private FieldViewModel? GetField(Point position)
        {
            return GetField(position.X, position.Y);
        }

        public void CreateBoard()
        {
            if (_amountBombs > (Math.Pow(_rowsColumns, 2) / 2))
            {
                AmountBombs = (int) (Math.Pow(_rowsColumns, 2) / 4);
                _controlsViewModel.AmountBombs = AmountBombs;
            }
            using (Fields.SuspendNotifications())
            {
                Fields.Clear();

                bool dark = false;
                for (int i = 0; i < _rowsColumns; i++)
                {
                    if (RowsColumns % 2 == 0) dark = !dark;

                    for (int j = 0; j < _rowsColumns; j++)
                    {
                        FieldViewModel field = new(0, new Point(i, j), this);
                        dark = !dark;
                        field.Background = dark ? Brushes.Beige : Brushes.LightGray;
                        field.CoverColor = dark ? Brushes.Green : Brushes.LightGreen;
                        Fields.Add(field);
                    }
                }
            }
        }

        public void FillInBombs(Point firstFieldPosition)
        {
            _controlsViewModel.NeedsReset = true;
            _startTime = DateTime.Now;
            BombsArePlaced = true;
            _infoTextViewModel.AmountBombs = _amountBombs;
            AddBombs(_amountBombs, firstFieldPosition);
            GenerateValue();
        }

        private void GenerateValue()
        {
            foreach (FieldViewModel field in Fields)
            {
                IEnumerable<FieldViewModel> surroundingFields = GetSurroundingFields(field.Position);
                int value = surroundingFields.Count(f => f.HasBomb);
                field.Value = field.HasBomb ? 0 : value;
                field.HasNumber = field.Value > 0;
            }
        }

        private void AddBombs(int amount, Point firstFieldPosition)
        {
            List<Point> availablePoints = new();
            int removedPoints = 0;
            for (int i = 0; i < RowsColumns; i++)
            for (int j = 0; j < RowsColumns; j++)
            {
                if (j == firstFieldPosition.X &&
                    i == firstFieldPosition.Y) // It is impossible to hit a bomb on the first click
                    continue;

                availablePoints.Add(new Point(i, j));
            }

            for (int i = 0; i < amount; i++)
            {
                int pointIndex = _rnd.Next(RowsColumns * RowsColumns - removedPoints);
                Point point = availablePoints[pointIndex];
                FieldViewModel bomb = GetField(point)!;
                bomb.HasBomb = true;
                _bombs.Add(bomb);
                availablePoints.Remove(point);
                removedPoints++;
            }
        }

        public void UncoverSurroundingZeros(FieldViewModel field)
        {
            if (field.HasNumber) return;

            HashSet<FieldViewModel> fieldsToUncoverSurroundingFields = new() {field};
            int lastAmountFields;
            HashSet<FieldViewModel> fieldsToUncover = new();
            do
            {
                lastAmountFields = fieldsToUncoverSurroundingFields.Count;
                foreach (FieldViewModel fieldViewModel in GetSurroundingUncoveredFields(
                    fieldsToUncoverSurroundingFields)) fieldsToUncover.Add(fieldViewModel);

                foreach (FieldViewModel fieldToUncover in fieldsToUncover.Where(fieldToUncover =>
                    !fieldToUncover.HasNumber)) fieldsToUncoverSurroundingFields.Add(fieldToUncover);
            } while (fieldsToUncoverSurroundingFields.Count != lastAmountFields);

            foreach (FieldViewModel fieldToUncover in fieldsToUncover) Uncover(fieldToUncover);
        }

        public void Uncover(FieldViewModel field)
        {
            field.IsCovered = false;
        }

        private IEnumerable<FieldViewModel> GetSurroundingUncoveredFields(IEnumerable<FieldViewModel> fields)
        {
            HashSet<FieldViewModel> fieldViewModels = new();
            foreach (FieldViewModel field in fields)
            foreach (FieldViewModel fieldToAdd in GetSurroundingFields(field.Position))
                fieldViewModels.Add(fieldToAdd);

            return fieldViewModels;
        }

        public void UncoverEveryFieldSurroundingIfValueMatchesFlags(FieldViewModel field)
        {
            IEnumerable<FieldViewModel> surroundingFields = GetSurroundingFields(field.Position);
            if (field.Value != surroundingFields.Count(f => f.IsFlagged))
                return;

            foreach (FieldViewModel fieldViewModel in surroundingFields.Where(f => !f.IsFlagged))
            {
                if (fieldViewModel.IsCovered) // should improve performance
                    Uncover(fieldViewModel);

                if (fieldViewModel.Value == 0) UncoverSurroundingZeros(fieldViewModel);

                if (fieldViewModel.HasBomb) GameOver();
            }
        }

        private IEnumerable<FieldViewModel> GetSurroundingFields(Point position)
        {
            FieldViewModel? topLeft = GetField(position.X - 1, position.Y - 1);
            if (topLeft != null)
                yield return topLeft;

            FieldViewModel? top = GetField(position.X, position.Y - 1);
            if (top != null)
                yield return top;

            FieldViewModel? topRight = GetField(position.X + 1, position.Y - 1);
            if (topRight != null)
                yield return topRight;

            FieldViewModel? right = GetField(position.X + 1, position.Y);
            if (right != null)
                yield return right;

            FieldViewModel? bottomRight = GetField(position.X + 1, position.Y + 1);
            if (bottomRight != null)
                yield return bottomRight;

            FieldViewModel? bottom = GetField(position.X, position.Y + 1);
            if (bottom != null)
                yield return bottom;

            FieldViewModel? bottomLeft = GetField(position.X - 1, position.Y + 1);
            if (bottomLeft != null)
                yield return bottomLeft;

            FieldViewModel? left = GetField(position.X - 1, position.Y);
            if (left != null)
                yield return left;
        }

        public void Reset()
        {
            BombsArePlaced = false;
            foreach (FieldViewModel field in Fields)
            {
                field.HasBomb = false;
                field.IsCovered = true;
                field.Value = 0;
                field.IsFlagged = false;
                field.HasNumber = false;
                _bombs.Clear();
            }

            _infoTextViewModel.Reset();
        }

        public bool HasWon()
        {
            return !Fields.Any(f => !f.HasBomb && f.IsCovered);
        }

        public void GameOver()
        {
            foreach (FieldViewModel bomb in _bombs) bomb.IsCovered = false;

            GameRunning = false;
            _infoTextViewModel.InfoText = "You Lost!";
        }

        public void Win()
        {
            GameRunning = false;
            _infoTextViewModel.InfoText =
                $"You Won! \n You needed \n {(DateTime.Now - _startTime).TotalMilliseconds}ms";
        }

        public void Start()
        {
            GameRunning = true;
            _infoTextViewModel.InfoText = "Game running";
        }

        public void Stop()
        {
            GameRunning = false;
            Reset();
            _infoTextViewModel.InfoText = "Game stopped";
        }
    }
}