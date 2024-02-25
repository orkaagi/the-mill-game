using MillGame.Model;
using MillGame.Persistence;

namespace TheMillGame.Persistence
{
    public class MillGameFileDataAccess : IMillGameDataAccess
    {
        public async Task<(Int32, Actions, Int32, Board)> LoadAsync(String path)
        {
            try
            {
                using (StreamReader reader = new StreamReader(path))
                {
                    String line = await reader.ReadLineAsync() ?? String.Empty;
                    String[] numbers = line.Split(' ');
                    Int32 currentPlayer = Int32.Parse(numbers[0]);

                    Actions lastAction = Actions.NEXT;
                    switch (numbers[1])
                    {
                        case ("NEXT"):
                            lastAction = Actions.NEXT;
                            break;
                        case ("MOVE"):
                            lastAction = Actions.MOVE;
                            break;
                        case ("REMOVE"):
                            lastAction = Actions.REMOVE;
                            break;
                    }

                    Int32 lastClicked = Int32.Parse(numbers[2]);

                    line = await reader.ReadLineAsync() ?? String.Empty;
                    numbers = line.Split(' ');
                    Int32[] redMenActivity = new Int32[] { Int32.Parse(numbers[0]), Int32.Parse(numbers[1]), Int32.Parse(numbers[2]) };

                    line = await reader.ReadLineAsync() ?? String.Empty;
                    numbers = line.Split(' ');
                    Int32[] blueMenActivity = new Int32[] { Int32.Parse(numbers[0]), Int32.Parse(numbers[1]), Int32.Parse(numbers[2]) };

                    line = await reader.ReadLineAsync() ?? String.Empty;
                    numbers = line.Split(' ');
                    Int32[] fieldValues = new Int32[24];

                    for (Int32 i = 0; i < 24; i++)
                    {
                        fieldValues[i] = Int32.Parse(numbers[i]);
                    }

                    Board board = new Board(redMenActivity, blueMenActivity, fieldValues);

                    return (currentPlayer, lastAction, lastClicked, board);
                }
            }
            catch
            {
                throw new MillGameDataException();
            }
        }

        public async Task SaveAsync(String path, Board board, Int32 currentPlayer, Actions lastAction, Int32 lasClicked)
        {
            try
            {
                using (StreamWriter writer = new StreamWriter(path))
                {
                    await writer.WriteAsync(currentPlayer + " " + lastAction + " " + lasClicked);
                    await writer.WriteLineAsync();

                    await writer.WriteAsync(board.MenActivity[0][0] + " " + board.MenActivity[0][1] + " " + board.MenActivity[0][2]);
                    await writer.WriteLineAsync();

                    await writer.WriteAsync(board.MenActivity[1][0] + " " + board.MenActivity[1][1] + " " + board.MenActivity[1][2]);
                    await writer.WriteLineAsync();

                    for (Int32 i = 0; i < board.FieldNum; i++)
                    {
                        await writer.WriteAsync(board.FieldValues[i] + " ");
                    }
                }
            }
            catch
            {
                throw new MillGameDataException();
            }
        }
    }
}