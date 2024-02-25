namespace MillGame.Model
{
    public class MillEventArgs : EventArgs
    {
        private Int32 _winner;      // -1, ha meg nincs nyertes
        private Int32 _x;           // -1, ha nem a megvaltozo pozicio az erdekes, egyebkent a megvaltozo mezo x koordinataja
        private Int32 _y;           // -1, ha nem a megvaltozo pozicio az erdekes, egyebkent a megvaltozo mezo y koordinataja
        private Int32 _player;      // -1, ha nem a mezon allo jatekos jatekos az erdekes, egyebkent 0 (RED) vagy 1 (KEK)

        public Int32 Winner { get { return _winner; } }
        public Int32 X { get { return _x; } }
        public Int32 Y { get { return _y; } }
        public Int32 Player { get { return _player; } }

        public MillEventArgs(Int32 winner, Int32 x, Int32 y, Int32 player)
        {
            _winner = winner;
            _x = x;
            _y = y;
            _player = player;
        }
    }
}