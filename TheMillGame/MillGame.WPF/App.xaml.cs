using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Threading;
using MillGame.Model;
using MillGame.Persistence;
using MillGame.WPF.View;
using MillGame.WPF.ViewModel;
using Microsoft.Win32;
using TheMillGame.Persistence;

namespace MillGame.WPF
{
    public partial class App : Application
    {
        #region Fields

        private MillGameModel _model = null!;
        private MillGameViewModel _viewModel = null!;
        private MainWindow _view = null!;

        #endregion

        #region Constructors

        public App()
        {
            Startup += new StartupEventHandler(App_Startup);
        }

        #endregion

        #region Application event handlers

        private void App_Startup(object? sender, StartupEventArgs e)
        {
            // modell letrehozasa
            _model = new MillGameModel(new MillGameFileDataAccess());
            _model.GameOver += new EventHandler<MillEventArgs>(Model_GameOver);
            _model.NewGame();

            // nezetmodell letrehozasa
            _viewModel = new MillGameViewModel(_model);
            _viewModel.NewGame += new EventHandler(ViewModel_NewGame);
            _viewModel.ExitGame += new EventHandler(ViewModel_ExitGame);
            _viewModel.LoadGame += new EventHandler(ViewModel_LoadGame);
            _viewModel.SaveGame += new EventHandler(ViewModel_SaveGame);

            // nezet letrehozasa
            _view = new MainWindow();
            _view.DataContext = _viewModel;
            _view.Closing += new System.ComponentModel.CancelEventHandler(View_Closing);
            _view.Show();
        }

        #endregion

        #region View event handlers

        private void View_Closing(object? sender, CancelEventArgs e)
        {
            if (MessageBox.Show("Biztos, hogy ki akar lépni?", "Malom játék", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.No)
            {
                e.Cancel = true;
            }
        }

        #endregion

        #region ViewModel event handlers

        private void ViewModel_NewGame(object? sender, EventArgs e)
        {
            _model.NewGame();
        }

        private async void ViewModel_LoadGame(object? sender, System.EventArgs e)
        {
            try
            {
                OpenFileDialog openFileDialog = new OpenFileDialog();
                openFileDialog.Title = "Malom játék tábla betöltése";
                openFileDialog.Filter = "Malom játék tábla|*.mtl";
                if (openFileDialog.ShowDialog() == true)
                {
                    await _model.LoadGameAsync(openFileDialog.FileName);
                }
            }
            catch (MillGameDataException)
            {
                MessageBox.Show("A fájl betöltése sikertelen!", "Malom játék", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private async void ViewModel_SaveGame(object? sender, EventArgs e)
        {
            try
            {
                SaveFileDialog saveFileDialog = new SaveFileDialog();
                saveFileDialog.Title = "Malom játék tábla betöltése";
                saveFileDialog.Filter = "Malom játék tábla|*.mtl";
                if (saveFileDialog.ShowDialog() == true)
                {
                    try
                    {
                        await _model.SaveGameAsync(saveFileDialog.FileName);
                    }
                    catch (MillGameDataException)
                    {
                        MessageBox.Show("Játék mentése sikertelen!" + Environment.NewLine + "Hibás az elérési út, vagy a könyvtár nem írható.", "Hiba!", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
            catch
            {
                MessageBox.Show("A fájl mentése sikertelen!", "Malom játék", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void ViewModel_ExitGame(object? sender, System.EventArgs e)
        {
            _view.Close();
        }

        #endregion

        #region Model event handlers

        private void Model_GameOver(object? sender, MillEventArgs e)
        {
            if (e.Winner == 0) 
            {
                MessageBox.Show("A piros játékos nyert!", "Malom játék", MessageBoxButton.OK, MessageBoxImage.Asterisk);
            }
            else
            {
                MessageBox.Show("A kék játékos nyert!", "Malom játék", MessageBoxButton.OK, MessageBoxImage.Asterisk);
            }
        }

        #endregion
    }
}