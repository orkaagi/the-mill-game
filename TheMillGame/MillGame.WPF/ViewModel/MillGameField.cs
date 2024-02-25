using System;

namespace MillGame.WPF.ViewModel
{
    public class MillGameField : ViewModelBase
    {
        private Int32 _player;

        private Int32[][] areCoordinatesVisible = new Int32[][] {  
            new Int32[] { 1, 0, 0, 1, 0, 0, 1 },
            new Int32[] { 0, 1, 0, 1, 0, 1, 0 },
            new Int32[] { 0, 0, 1, 1, 1, 0, 0 },
            new Int32[] { 1, 1, 1, 0, 1, 1, 1 },
            new Int32[] { 0, 0, 1, 1, 1, 0, 0 },
            new Int32[] { 0, 1, 0, 1, 0, 1, 0 },
            new Int32[] { 1, 0, 0, 1, 0, 0, 1 }
        };  // 0, ha nem lathato, 1, ha lathato

        public Boolean IsButtonVisible
        {
            get { return areCoordinatesVisible[X][Y] == 1; }
        }

        public Int32 Player
        {
            get { return _player; }
            set 
            {
                if (_player != value) 
                {
                    _player = value;
                    OnPropertyChanged();
                }
            }
        }

        public Int32 X { get; set; }
        public Int32 Y { get; set; }
        public Tuple<Int32, Int32> XY { get { return new(X, Y); } }

        public DelegateCommand? StepCommand { get; set; }
    }
}