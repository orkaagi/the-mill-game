using System;
using MillGame.Persistence;

namespace MillGame.Model
{
    public enum Actions { NEXT, MOVE, REMOVE}

    public class MillGameModel
    {
        #region Fields and properties

        private IMillGameDataAccess _dataAccess;
        private Board _board;
        private Int32 _currentPlayer;   // az aktualis jatekos RED=0 vagy BLUE=1
        private Actions _actionState;
        private Int32 _lastClicked;

        public Board Board { get { return _board; } }
        public Int32 CurrentPlayer { get { return _currentPlayer; } }
        public Actions ActionState { get { return _actionState; } }
        public Int32 LastClicked { get { return _lastClicked; } }
        public Boolean IsGameOver { get { return _board.GameOver(); } }

        #endregion

        #region Constructor

        public MillGameModel(IMillGameDataAccess dataAccess)
        {
            _dataAccess = dataAccess;
            _board = new Board();
            _currentPlayer = 0;
            _actionState = Actions.NEXT;
            _lastClicked = -1;
        }

        #endregion

        #region Events and event methods

        public event EventHandler<MillEventArgs>? FieldChanged;
        public event EventHandler<MillEventArgs>? GameOver;
        public event EventHandler<MillEventArgs>? GameCreated;

        private void OnFieldChanged(Int32 x, Int32 y, Int32 player)
        {
            FieldChanged?.Invoke(this, new MillEventArgs(-1, x, y, player));
        }

        private void OnGameOver(Int32 winner)
        {
            GameOver?.Invoke(this, new MillEventArgs(winner, -1, -1, -1));
        }

        private void OnGameCreated()
        {
            GameCreated?.Invoke(this, new MillEventArgs(-1, -1, -1, -1));
        }

        #endregion

        #region Game methods

        public void NewGame()
        {
            _board = new Board();
            _currentPlayer = 0;
            _actionState = Actions.NEXT;
            _lastClicked = -1;
            OnGameCreated();
        }

        public void Action(Int32 x, Int32 y)
        {
            Int32 pos = _board.CoordinatesToPosition[x][y];

            if (IsGameOver)
            {
                return;
            }
            Int32 cp = _currentPlayer;
            Boolean validMove;

            switch (_actionState)
            {
                case Actions.NEXT:
                    // ha epp lepakolasi fazis van (vagyis vannak meg inaktiv jatekosok)
                    if (_board.MenActivity[_currentPlayer][0] > 0)
                    {
                        validMove = _board.Put(_currentPlayer, pos);
                        // ha a lerakas sikeres volt es ezzel olyan malom alakult ki, ami utan az ellenfel egyik jatekosa eltavolithato
                        if (validMove && _board.InMill(pos) && _board.canRemove(cp))
                        {
                            _lastClicked = -1;
                            _actionState = Actions.REMOVE;
                        }
                        // ha a lerakas sikeres volt, de nem alakult ki malom,
                        // vagy olyan malom alakult ki, ami utan az ellenfel egyik jatekosa sem eltavolithato
                        // vege a muveletnek, johet a masik jatekos
                        else if (validMove)
                        {
                            _lastClicked = -1;
                            _actionState = Actions.NEXT;
                            _currentPlayer = 1 - cp;
                        }
                    }
                    // egyebkent mozgatasi fazis van
                    else
                    {
                        // ha a soron kovetkezo jatekos tudja mozgatni legalabb egy babujat
                        if (_board.canMove(cp))
                        {
                            // ha az aktualis jatekos a sajat babujara kattintott elokeszitve annak mozgatasat
                            if (_board.FieldValues[pos] == cp)
                            {
                                _lastClicked = pos;
                                _actionState = Actions.MOVE;
                            }
                        }
                        // ha a soron kovetkezo jatekos nem tud mozgatni, passzol a masik jatekosnak
                        else
                        {
                            // ha az ellenfel kijelolte a babujat, amit mozgatni fog
                            if (_board.FieldValues[pos] == (1 - cp))
                            {
                                _lastClicked = pos;
                                _actionState = Actions.MOVE;
                            }
                            else 
                            {
                                _lastClicked = -1;
                                _actionState = Actions.NEXT;
                            }                            
                            _currentPlayer = 1 - cp;
                        }
                    }
                    break;

                case Actions.MOVE:
                    validMove = _board.Move(_currentPlayer, _lastClicked, pos);
                    // ha a mozgatas sikeres volt es ezzel olyan malom alakult ki ami utan az ellenfel egyik jatekosa eltavolithato
                    if (validMove && _board.InMill(pos) && _board.canRemove(cp))
                    {
                        Int32 lastX = _board.PositionToCoordinates[_lastClicked].Item2;
                        Int32 lastY = _board.PositionToCoordinates[_lastClicked].Item1;
                        OnFieldChanged(lastX, lastY, _board.FieldValues[_lastClicked]);
                        _lastClicked = -1;
                        _actionState = Actions.REMOVE;
                    }
                    // ha a mozgatas sikeres volt, de nem alakult ki malom,
                    // vagy ha olyan malom alakult ki, ami utan az ellenfel egyik babuja sem eltavolithato (mert mind malomban van)
                    // vege a muveletnek, johet a masik jatekos
                    else if (validMove)
                    {
                        Int32 lastX = _board.PositionToCoordinates[_lastClicked].Item2;
                        Int32 lastY = _board.PositionToCoordinates[_lastClicked].Item1;
                        OnFieldChanged(lastX, lastY, _board.FieldValues[_lastClicked]);
                        _lastClicked = -1;
                        _actionState = Actions.NEXT;
                        _currentPlayer = 1 - cp;
                    }
                    break;

                case Actions.REMOVE:
                    validMove = _board.RemoveOpponent(_currentPlayer, pos);
                    // ha az ellenfel eltavolitasa sikeres volt, vege a muveletnek, johet a masik jatekos
                    if (validMove)
                    {
                        _lastClicked = -1;
                        _actionState = Actions.NEXT;
                        _currentPlayer = 1 - cp;
                    }
                    break;

                default:
                    break;
            }
            // valtozasok megjelenitese a nezetben
            OnFieldChanged(x, y, _board.FieldValues[pos]);

            if (_board.GameOver())
            {
                OnGameOver(cp);
            }
        }

        #endregion

        #region Methods for loading and saving game

        public async Task LoadGameAsync(String path)
        {
            if (_dataAccess == null)
            {
                throw new InvalidOperationException("No data access is provided.");
            }            
            (_currentPlayer, _actionState, _lastClicked, _board) = await _dataAccess.LoadAsync(path);
            OnGameCreated();
        }

        public async Task SaveGameAsync(String path)
        {
            if (_dataAccess == null)
            {
                throw new InvalidOperationException("No data access is provided.");
            }
            await _dataAccess.SaveAsync(path, _board, _currentPlayer, _actionState, _lastClicked);
        }

        #endregion
    }
}