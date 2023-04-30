using ReactiveUI;

namespace ProjectP4.ViewModels
{
    public class InfoTextViewModel : ViewModelBase
    {
        private int _amountBombs;

        private int _flagsSet;
        private string _infoText = "";

        public string InfoText
        {
            get => _infoText;
            set => this.RaiseAndSetIfChanged(ref _infoText, value);
        }

        public int FlagsSet
        {
            get => _flagsSet;
            set => this.RaiseAndSetIfChanged(ref _flagsSet, value);
        }

        public int AmountBombs
        {
            get => _amountBombs;
            set => this.RaiseAndSetIfChanged(ref _amountBombs, value);
        }

        public void Reset() //funkcja resetu opcji
        {
            InfoText = "";
            AmountBombs = 0;
            FlagsSet = 0;
        }
    }
}