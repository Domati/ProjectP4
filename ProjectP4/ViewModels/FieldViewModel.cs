using System;
using System.Collections.Generic;
using System.Reactive;
using Avalonia.Media;
using ProjectP4.Models;
using ReactiveUI;

namespace ProjectP4.ViewModels
{
    public class FieldViewModel : ViewModelBase, IEquatable<FieldViewModel>
    {
        private readonly BoardViewModel _board;

        private IBrush _background;

        private bool _hasBomb;
        private bool _number;

        private bool _isCovered = true;

        private bool _isFlagged;

        private int _value;

        private IBrush _valueColor = Brushes.Black;

        public FieldViewModel(int value, Point position, BoardViewModel board)
        {
            OnFieldLeftClicked = ReactiveCommand.Create(FieldLeftClicked);
            Position = position;
            Value = value;
            _board = board;
        }

        public int Value
        {
            get => _value;
            set
            {
                this.RaiseAndSetIfChanged(ref _value, value);
                UpdateFontColor();
            }
        }

        public bool IsCovered
        {
            get => _isCovered;
            set => this.RaiseAndSetIfChanged(ref _isCovered, value);
        }

        public IBrush ValueColor
        {
            get => _valueColor;
            set => this.RaiseAndSetIfChanged(ref _valueColor, value);
        }

        public bool HasBomb
        {
            get => _hasBomb;
            set => this.RaiseAndSetIfChanged(ref _hasBomb, value);
        }

        public ReactiveCommand<Unit, Unit> OnFieldLeftClicked { get; }

        public Point Position { get; }

        public IBrush Background
        {
            get => _background;
            set => this.RaiseAndSetIfChanged(ref _background, value);
        }

        public IBrush CoverColor { get; set; }

        public bool Number
        {
            get => _number;
            set => this.RaiseAndSetIfChanged(ref _number, value);
        }

        public bool IsFlagged
        {
            get => _isFlagged;
            set => this.RaiseAndSetIfChanged(ref _isFlagged, value);
        }

        public bool Equals(FieldViewModel? other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return _board.Equals(other._board) && Position.Equals(other.Position);
        }

        public void FieldLeftClicked()
        {
            if (IsFlagged || !_board.GameRunning) return;
            Console.WriteLine($"Field Left clicked X: {Position.X} Y: {Position.Y}");


            if (!_board.BombsArePlaced) _board.FillInBombs(Position);

            if (!IsCovered)
            {
                if (Number) _board.UncoverEveryFieldSurroundingIfValueMatchesFlags(this);
                if (_board.HasWon()) _board.Win();
            }
            else
            {
                _board.Uncover(this);
                if (HasBomb)
                {
                    _board.GameOver();
                }
                else
                {
                    _board.UncoverSurroundingZeros(this);
                    if (_board.HasWon()) _board.Win();
                }
            }
        }

        public void FieldRightClicked()
        {
            if (IsCovered && _board.GameRunning)
            {
                Console.WriteLine($"Field Right clicked X: {Position.X} Y: {Position.Y}");
                if (!IsFlagged)
                {
                    IsFlagged = true;
                    _board.FlagsSet++;
                }
                else
                {
                    _board.FlagsSet--;
                    IsFlagged = false;
                }

                if (_board.HasWon()) _board.Win();
            }
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(_board, Position);
        }

        public static bool operator ==(FieldViewModel? left, FieldViewModel? right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(FieldViewModel? left, FieldViewModel? right)
        {
            return !Equals(left, right);
        }


        private void UpdateFontColor()
        {
            Number = true;

            switch (Value)
            {
                case 0:
                    Number = false;
                    break;
                case 1:
                    ValueColor = Brushes.Blue;
                    Number = true;
                    break;
                case 2:
                    ValueColor = Brushes.Green;
                    Number = true;
                    break;
                case 3:
                    ValueColor = Brushes.Red;
                    Number = true;
                    break;
                case 4:
                    ValueColor = Brushes.Purple;
                    Number = true;
                    break;
                case 5:
                    ValueColor = Brushes.Orange;
                    Number = true;
                    break;
                case 6:
                    ValueColor = Brushes.Yellow;
                    Number = true;
                    break;
                case 7:
                    ValueColor = Brushes.Pink;
                    Number = true;
                    break;
                case 8:
                    ValueColor = Brushes.Black;
                    Number = true;
                    break;
            }
        }

        public override bool Equals(object? obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != GetType()) return false;
            return Equals((FieldViewModel)obj);
        }


    }
}