using Moq;
using System;
using MillGame.Model;

namespace TheMillGameTest
{
    [TestClass]
    public class TheMillGameBoardTest
    {
        [TestMethod]
        public void BoardPutTest()
        {
            Int32[] red = new Int32[] { 6, 3, 0 };
            Int32[] blue = new Int32[] { 7, 2, 0 };
            Int32[] values = new Int32[] { 0, 0, -1, -1, -1, -1, -1, -1, 1, -1, -1, -1, 0, -1, -1, 1, -1, -1, -1, -1, -1, -1, -1, -1 };

            Board board = new Board(red, blue, values);

            Assert.AreEqual(false, board.Put(1, 0));   // ellenfell all ott
            Assert.AreEqual(false, board.Put(1, 15));  // sajat babu all ott
            Assert.AreEqual(true, board.Put(1, 14));   // ures hely
        }

        [TestMethod]
        public void CanMoveTest()
        {
            Int32[] red = new Int32[] { 0, 4, 5 };
            Int32[] blue = new Int32[] { 0, 4, 5 };
            Int32[] values = new Int32[] { -1, -1, -1, -1, -1, -1, -1, -1, -1, 1, 0, 1, -1, -1, -1, -1, 1, 0, 0, 0, 1, -1, -1, -1 };

            Board board = new Board(red, blue, values);

            Assert.AreEqual(false, board.canMove(0));    // a piros jatekos minden babuja be van keritve, nem tud lepni
            Assert.AreEqual(true, board.canMove(1));     // a kek jatekosnak van olyan babuja, amivel tud lepni
        }

        [TestMethod]
        public void BoardMoveTest()
        {
            Int32[] red = new Int32[] { 3, 5, 1 };
            Int32[] blue = new Int32[] { 3, 5, 1 };
            Int32[] values = new Int32[] { 1, -1, 0, 0, 0, -1, -1, 0, -1, -1, -1, 0, 1, 1, 1, -1, -1, -1, -1, -1, -1, 1, -1, -1 };

            Board board = new Board(red, blue, values);

            Assert.AreEqual(false, board.Move(1, 14, 13));  // a kek jatekos nem tudja abba a szomszedba mozgatni babujat, ahol egy masik kek all
            Assert.AreEqual(false, board.Move(1, 12, 11));  // a kek jatekos nem tudja abba a szomszedba mozgatni babujat, ahol egy piros all
            Assert.AreEqual(false, board.Move(1, 12, 6));   // a kek jatekos nem tudja egy nem szomszedos ures mezobe mozgatni a babujat
            Assert.AreEqual(false, board.Move(1, 0, 7));    // a kek jatekos nem tudja egy nem szomszedos foglalt mezobe mozgatni a babujat
            Assert.AreEqual(true, board.Move(1, 0, 1));     // a kek jatekos egy szomszedos szabad mezobe mozgathatja a babujat

            Assert.AreEqual(false, board.Move(0, 4, 3));    // a piros jatekos nem tudja abba a szomszedba mozgatni babujat, ahol egy masik piros all
            Assert.AreEqual(false, board.Move(0, 11, 12));  // a piros jatekos nem tudja abba a szomszedba mozgatni babujat, ahol egy kek all
            Assert.AreEqual(false, board.Move(0, 7, 23));   // a piros jatekos nem tudja egy nem szomszedos ures mezobe mozgatni a babujat
            Assert.AreEqual(false, board.Move(0, 3, 13));   // a piros jatekos nem tudja egy nem szomszedos foglalt mezobe mozgatni a babujat
            Assert.AreEqual(true, board.Move(0, 4, 5));     // a piros jatekos egy szomszedos szabad mezobe mozgathatja a babujat

        }

        [TestMethod]
        public void BoardInMillTest()
        {
            Int32[] red = new Int32[] { 3, 5, 1 };
            Int32[] blue = new Int32[] { 3, 5, 1 };
            Int32[] values = new Int32[] { 1, -1, 0, 0, 0, -1, -1, 0, -1, -1, -1, 0, 1, 1, 1, -1, -1, -1, -1, -1, -1, 1, -1, -1 };

            Board board = new Board(red, blue, values);

            Assert.AreEqual(false, board.InMill(0));    // egyedulallo kek, nincs malomban
            Assert.AreEqual(false, board.InMill(21));   // 2. kek az oszlopban, nincs malomban
            Assert.AreEqual(true, board.InMill(14));    // 3. kek a sorban, valoban malom

            Assert.AreEqual(false, board.InMill(7));    // egyedulallo piros, nincs malomban
            Assert.AreEqual(false, board.InMill(11));   // 2. piros a sorban, nincs malomban
            Assert.AreEqual(true, board.InMill(4));     // 3. piros az oszlopban, valoban malom
        }

        [TestMethod]
        public void CanRemoveTest()
        {
            Int32[] red_1 = new Int32[] { 5, 3, 1 };
            Int32[] blue_1 = new Int32[] { 6, 3, 0 };
            Int32[] values_1 = new Int32[] { -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, 0, 0, 0, -1, -1, -1, -1, -1, 1, 1, 1, -1, -1, -1 };

            Board board_1 = new Board(red_1, blue_1, values_1);

            Assert.AreEqual(false, board_1.canRemove(0));     // a kek jatekos minden babuja malomban all, ezert a piros egyet sem tud levenni
            Assert.AreEqual(false, board_1.canRemove(1));     // a piros jatekos minden babuja malomban all, ezert a kek egyet sem tud levenni


            Int32[] red_2 = new Int32[] { 6, 2, 1 };
            Int32[] blue_2 = new Int32[] { 7, 2, 0 };
            Int32[] values_2 = new Int32[] { -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, 0, -1, 0, -1, -1, -1, -1, -1, 1, 1, -1, -1, -1, -1 };

            Board board_2 = new Board(red_2, blue_2, values_2);

            Assert.AreEqual(true, board_2.canRemove(0));     // a kek jatekosnak vannak olyan babui, amik nem allnak malomban, ezert a piros jatekos tud babut levenni
            Assert.AreEqual(true, board_2.canRemove(1));     // a piros jatekosnak vannak olyan babui, amik nem allnak malomban, ezert a kek jatekos tud babut levenni
        }

        [TestMethod]
        public void BoardRemoveOpponentTest()
        {
            Int32[] red = new Int32[] { 3, 5, 1 };
            Int32[] blue = new Int32[] { 3, 5, 1 };
            Int32[] values = new Int32[] { 1, -1, 0, 0, 0, -1, -1, 0, -1, -1, -1, 0, 1, 1, 1, -1, -1, -1, -1, -1, -1, 1, -1, -1 };

            Board board = new Board(red, blue, values);

            Assert.AreEqual(false, board.RemoveOpponent(1, 0));   // kek nem veheti le sajat babujat
            Assert.AreEqual(false, board.RemoveOpponent(1, 2));   // kek nem veheti le piros babujat, ami egy malom resze
            Assert.AreEqual(false, board.RemoveOpponent(1, 15));  // kek nem vehet le babut arrol a helyrol, ami ures
            Assert.AreEqual(true, board.RemoveOpponent(1, 11));   // kek leveheti azt a piros babut, ami nem egy malom resze

            Assert.AreEqual(false, board.RemoveOpponent(0, 7));   // piros nem veheti le sajat babujat
            Assert.AreEqual(false, board.RemoveOpponent(0, 14));  // piros nem veheti le kek babujat, ami egy malom resze
            Assert.AreEqual(false, board.RemoveOpponent(0, 18));  // piros nem vehet le babut arrol a helyrol, ami ures
            Assert.AreEqual(true, board.RemoveOpponent(0, 21));   // piros leveheti azt a piros babut, ami nem egy malom resze
        }

        [TestMethod]
        public void BoardGameOverTest()
        {
            Int32[] red_1 = new Int32[] { 7, 2, 0 };
            Int32[] blue_1 = new Int32[] { 8, 1, 0 };
            Int32[] values = new Int32[] { 1, -1, 0, 0, 0, -1, -1, 0, -1, -1, -1, 0, 1, 1, 1, -1, -1, -1, -1, -1, -1, 1, -1, -1 };

            Board board_1 = new Board(red_1, blue_1, values);
            Assert.AreEqual(false, board_1.GameOver());  // a jatekosoknak kevesebb, mint 2 babujuk van a palyan, de epp lepakolasi fazis van

            Int32[] red_2 = new Int32[] { 0, 5, 4 };
            Int32[] blue_2 = new Int32[] { 0, 5, 4 };
            Board board_2 = new Board(red_2, blue_2, values);
            Assert.AreEqual(false, board_2.GameOver()); // a jatekosoknak meg 5-5 mozgathato babujuk van a palyan

            Int32[] red_3 = new Int32[] { 0, 4, 5 };
            Int32[] blue_3 = new Int32[] { 0, 2, 7 };
            Board board_3 = new Board(red_3, blue_3, values);
            Assert.AreEqual(true, board_3.GameOver()); // valoban jatek vege, a kek jatekosnak mar csak 2 mozgathato babuja van

            Int32[] red_4 = new Int32[] { 0, 2, 7 };
            Int32[] blue_4 = new Int32[] { 0, 6, 3 };
            Board board_4 = new Board(red_4, blue_4, values);
            Assert.AreEqual(true, board_4.GameOver()); // valoban jatek vege, a piros jatekosnak mar csak 2 mozgathato babuja van
        }
    }
}