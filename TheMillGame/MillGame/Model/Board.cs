using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MillGame.Model
{
    public class Board
    {
        #region Fields

        // a palya 24 mezobol all      
        private static readonly Int32 _fieldNum = 24;

        // mindegyik mezo vagy ures (-1) vagy egy piros(0) vagy egy kek(1) jatekos all rajta
        private Int32[] _fieldValues = new Int32[_fieldNum];

        // mezok szomszedossagi listas abrazolasa
        private readonly List<List<Int32>> _neighbours = new List<List<Int32>>() {
            new List<Int32>() { 1,7 },
            new List<Int32>() { 0,9,2 },
            new List<Int32>() { 1,3 },
            new List<Int32>() { 2,11,4 },
            new List<Int32>() { 3,5 },
            new List<Int32>() { 6,13,4 },
            new List<Int32>() { 7,5 },
            new List<Int32>() { 0,15,6 },
            new List<Int32>() { 15,9 },
            new List<Int32>() { 8,1,10,17 },
            new List<Int32>() { 9,11 },
            new List<Int32>() { 19,10,3,12 },
            new List<Int32>() { 13,11 },
            new List<Int32>() { 14,21,12,5 },
            new List<Int32>() { 15,13 },
            new List<Int32>() { 7,8,23,14 },
            new List<Int32>() { 23,17 },
            new List<Int32>() { 16,9,18 },
            new List<Int32>() { 17,19 },
            new List<Int32>() { 18,11,20 },
            new List<Int32>() { 21,19 },
            new List<Int32>() { 22,13,20 },
            new List<Int32>() { 23,21 },
            new List<Int32>() { 15,16,22 }
        };

        // lehetseges malmok listaja
        private readonly List<List<Int32>> _mills = new List<List<Int32>>() {
            new List<Int32>() { 0,1,2 },
            new List<Int32>() { 8,9,10 },
            new List<Int32>() { 16,17,18 },
            new List<Int32>() { 7,15,23 },
            new List<Int32>() { 19,11,3 },
            new List<Int32>() { 22,21,20 },
            new List<Int32>() { 14,13,12},
            new List<Int32>() { 6,5,4 },
            new List<Int32>() { 0,7,6 },
            new List<Int32>() { 8,15,14 },
            new List<Int32>() { 16,23,22 },
            new List<Int32>() { 1,9,17 },
            new List<Int32>() { 21,13,5 },
            new List<Int32>() { 18,19,20 },
            new List<Int32>() { 10,11,12 },
            new List<Int32>() { 2,3,4 }
        };

        // inaktiv jatekosok(arr[cp][0]), aktiv jatekosok(arr[cp][1]), kiesett jatekosok(arr[cp][2]) szama, ahol
        // cp=0 jeloli a piros, cp=1 a kek jatekost
        private Int32[][] _menActivity;

        // (x,y) koordinatak megfeleltetese egyes palya mezo indexeknek
        private Int32[][] _coordinatesToPosition = new Int32[][] {
            new Int32[] { 0, -1, -1, 1, -1, -1, 2 },
            new Int32[] { -1, 8, -1, 9, -1, 10, -1 },
            new Int32[] { -1, -1, 16, 17, 18, -1, -1 },
            new Int32[] { 7, 15, 23, -1, 19, 11, 3 },
            new Int32[] { -1, -1, 22, 21, 20, -1, -1 },
            new Int32[] { -1, 14, -1, 13, -1, 12, -1 },
            new Int32[] { 6, -1, -1, 5, -1, -1, 4 }
        };

        // poziciok megfeleltetese (x,y) koordinataknak
        private Tuple<Int32, Int32>[] _positionToCoordinates = new Tuple<Int32, Int32>[] {
            new(0,0), new(3,0), new(6,0),
            new(6,3), new(6,6), new(3,6),
            new(0,6), new(0,3), new(1,1),
            new(3,1), new(5,1), new(5,3),
            new(5,5), new(3,5), new(1,5),
            new(1,3), new(2,2), new(3,2),
            new(4,2), new(4,3), new(4,4),
            new(3,4), new(2,4), new(2,3)
        };

        public Int32 FieldNum { get { return _fieldNum; } }
        public Int32[] FieldValues { get { return _fieldValues; } }
        public Int32[][] MenActivity { get { return _menActivity; } }
        public Int32[][] CoordinatesToPosition { get { return _coordinatesToPosition; } }
        public Tuple<Int32, Int32>[] PositionToCoordinates { get { return _positionToCoordinates; } }

        #endregion

        #region Constuctors

        public Board()
        {
            for (Int32 i = 0; i < _fieldNum; i++)
            {
                _fieldValues[i] = -1;
            }
            _menActivity = new Int32[][] { new Int32[] { 9, 0, 0 }, new Int32[] { 9, 0, 0 } };
        }

        public Board(Int32[] red, Int32[] blue, Int32[] values)
        {
            _fieldValues = values;
            _menActivity = new Int32[][] { red, blue };
        }

        #endregion

        #region Helper method

        private Boolean areNeighbours(Int32 self, Int32 other)
        {
            return _neighbours[self].Contains(other);
        }

        #endregion

        #region Game actions and queries

        public Boolean Put(Int32 cp, Int32 pos)
        {
            // ha eddig ures volt a cella           
            if (_fieldValues[pos] == -1)
            {
                // helyezzuk ra a jatekost
                _fieldValues[pos] = cp;
                // csokkentsuk az inaktiv jatekosok szamat es noveljuk az aktiv jatekosok szamat
                _menActivity[cp][0]--;
                _menActivity[cp][1]++;
                return true;
            }
            else
            {
                return false;
            }
        }

        public Boolean Move(Int32 cp, Int32 home, Int32 dest)
        {
            // ha a cel cella ures es a kiindulo cella szomszedos a cellal
            if (_fieldValues[dest] == -1 && areNeighbours(home, dest))
            {
                // helyezzuk a celra a jatekost
                _fieldValues[dest] = cp;
                // a kiindulo cellat allitsuk uresre
                _fieldValues[home] = -1;
                return true;
            }
            else
            {
                return false;
            }
        }

        public Boolean RemoveOpponent(Int32 cp, Int32 pos)
        {
            Int32 opponent = 1 - cp;
            // ha a cellan az ellenfel jatekosa all es a jatekos leveheto (nincs malomban)
            if (_fieldValues[pos] == opponent && !InMill(pos))
            {
                // a kijelolt cellat allitsuk uresre
                _fieldValues[pos] = -1;
                // csokkentsuk az ellenfel aktiv jatekosainak szamat es noveljuk a kiesett jatekosoket
                _menActivity[opponent][1]--;
                _menActivity[opponent][2]++;
                return true;
            }
            else
            {
                return false;
            }
        }

        public Boolean GameOver()
        {
            // igaz, ha barmelyik jatekosnak 3 ala csokken a babui szama a mozgatasi fazis alatt
            return _menActivity[0][2] > 6 || _menActivity[1][2] > 6;
        }

        public Boolean InMill(Int32 pos)
        {
            Int32 team = _fieldValues[pos];
            foreach (List<Int32> mill in _mills)
            {
                if (mill.Contains(pos) 
                    && mill.TrueForAll(manPos => _fieldValues[manPos] == team))
                {
                    return true;
                }
            }
            return false;
        }

        public Boolean canMove(Int32 cp)
        {
            // ha a jatekosnak van olyan babuja, ami mozgathato
            for (Int32 pos = 0; pos < _fieldNum; pos++)
            {
                if (_fieldValues[pos] == cp && _neighbours[pos].Any(manPos => _fieldValues[manPos] == -1)) return true;
            }
            return false;
        }

        public Boolean canRemove(Int32 cp)
        {
            // ha az ellenfelnek van olyan babuja, ami nem all malomban
            Int32 op = 1 - cp;            
            for (Int32 pos = 0; pos < _fieldNum; pos++)
            {
                if (_fieldValues[pos] == op && !InMill(pos)) return true;
            }
            return false;
            
        }

        // (x,y) koordinatak alapjan megallapitani, hogy a fieldValues adott indexen melyik jatekos all
        public Int32 FieldPlayer(Int32 x, Int32 y)
        {
            return _fieldValues[_coordinatesToPosition[x][y]];
        }

        #endregion
    }
}
