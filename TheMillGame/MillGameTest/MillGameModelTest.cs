using System;
using Moq;
using MillGame.Model;
using MillGame.Persistence;

namespace TheMillGameTest
{
    [TestClass]
    public class TheMillGameModelTest
    {
        private MillGameModel _model = null!;
        private Board _mockedBoard = null!;
        private Mock<IMillGameDataAccess> _mock = null!;

        [TestInitialize]
        public void Initialize()
        {
            Int32 cp = 1;
            Actions action = Actions.REMOVE;
            Int32 lastClicked = -1;
            Int32[] red = new Int32[] { 0, 3, 6 };
            Int32[] blue = new Int32[] { 0, 4, 5 };
            Int32[] values = new Int32[] { -1, 0, -1, 0, 0, -1, -1, -1, -1, -1, -1, 1, -1, 1, 1, -1, -1, -1, -1, -1, 1, -1, -1, -1, };

            _mockedBoard = new Board(red, blue, values);

            _mock = new Mock<IMillGameDataAccess>();
            _mock.Setup(mock => mock.LoadAsync(It.IsAny<String>()))
                .Returns(() => Task.FromResult((cp, action, lastClicked, _mockedBoard)));

            _model = new MillGameModel(_mock.Object);

            _model.GameOver += new EventHandler<MillEventArgs>(Model_GameOver);
        }

        [TestMethod]
        public async Task MillGameModelLoadTest()
        {
            _model.NewGame();
            await _model.LoadGameAsync(String.Empty);

            for (Int32 i = 0; i < _mockedBoard.FieldNum; i++)
            {
                Assert.AreEqual(_mockedBoard.FieldValues[i], _model.Board.FieldValues[i]);
            }

            _mock.Verify(dataAccess => dataAccess.LoadAsync(String.Empty), Times.Once());

            // a moclkolt tablan a kek jatekos epp egy malom kirakasa utan lep
            // kivalasztja a piros egyik babuja, amivel piros babunak a szama 2-re csokken
            _model.Action(0, 3);

        }

        private void Model_GameOver(Object? sender, MillEventArgs e)
        {
            Assert.IsTrue(_model.IsGameOver);   // a jateknak vege
            Assert.AreEqual(1, e.Winner);   // a jatekot a kek jatekos nyerte meg
        }
    }
}