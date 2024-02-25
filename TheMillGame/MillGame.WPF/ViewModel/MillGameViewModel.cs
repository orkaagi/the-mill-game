using System;
using System.Collections.ObjectModel;
using System.Linq;
using MillGame.Model;

namespace MillGame.WPF.ViewModel
{
    public class MillGameViewModel : ViewModelBase
    {
        #region Fields and properties

        private MillGameModel _model;

        public DelegateCommand NewGameCommand { get; private set; }
        public DelegateCommand LoadGameCommand { get; private set; }
        public DelegateCommand SaveGameCommand { get; private set; }
        public DelegateCommand ExitCommand { get; private set; }
        public ObservableCollection<MillGameField> Fields { get; set; }

        #endregion

        #region Events

        public event EventHandler? NewGame;
        public event EventHandler? LoadGame;
        public event EventHandler? SaveGame;
        public event EventHandler? ExitGame;

        #endregion

        #region Constructor

        public MillGameViewModel(MillGameModel model)
        {
            // malom jatek modell
            _model = model;
            _model.FieldChanged += new EventHandler<MillEventArgs>(Model_FieldChanged);
            _model.GameCreated += new EventHandler<MillEventArgs>(Model_GameCreated);

            // parancsok
            NewGameCommand = new DelegateCommand(param => OnNewGame());
            LoadGameCommand = new DelegateCommand(param => OnLoadGame());
            SaveGameCommand = new DelegateCommand(param => OnSaveGame());
            ExitCommand = new DelegateCommand(param => OnExitGame());

            // gombokbol allo jatektabla letrehozasa
            Fields = new ObservableCollection<MillGameField>();
            for (Int32 i = 0; i < 7; i++)
            {
                for (Int32 j = 0; j < 7; j++)
                {
                    Fields.Add(new MillGameField
                    {
                        X = i,
                        Y = j,
                        Player = -1,
                        StepCommand = new DelegateCommand(param =>
                        {
                            if (param is Tuple<Int32, Int32> position)
                                StepGame(position.Item1, position.Item2);
                        })
                    });
                }
            }
            
            RefreshTable();
        }

        #endregion

        #region Private methods

        private void RefreshTable()
        {
            foreach (MillGameField field in Fields)
            {
                if (field.IsButtonVisible == true) 
                {
                    field.Player = _model.Board.FieldPlayer(field.X, field.Y);
                }                
            }
        }

        private void StepGame(Int32 x, Int32 y)
        {
            _model.Action(x, y);
        }

        #endregion

        #region Game event handlers

        private void Model_FieldChanged(object? sender, MillEventArgs e)
        {
            MillGameField field = Fields.Single(f => (f.X == e.X && f.Y == e.Y));
            field.Player = e.Player;
        }

        private void Model_GameCreated(object? sender, MillEventArgs e)
        {
            RefreshTable();
        }

        #endregion

        #region Event methods

        private void OnNewGame()
        {
            NewGame?.Invoke(this, EventArgs.Empty);
        }

        private void OnLoadGame()
        {
            LoadGame?.Invoke(this, EventArgs.Empty);
        }

        private void OnSaveGame()
        {
            SaveGame?.Invoke(this, EventArgs.Empty);
        }

        private void OnExitGame()
        {
            ExitGame?.Invoke(this, EventArgs.Empty);
        }

        #endregion
    }
}