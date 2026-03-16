namespace Web_X_O.Models
{
    public enum CellState
    {
        Empty,
        X,
        O
    }

    public class Game
    {
        public const int MinSize = 3;
        public const int MaxSize = 8;

        public int Size { get; }
        public int WinLength { get; }

        public CellState[,] Board { get; }

        public CellState CurrentPlayer { get; private set; }

        public bool IsFinished { get; private set; }
        public bool IsDraw { get; private set; }

        public Game(int size, int winLength)
        {
            if (size < MinSize || size > MaxSize)
                throw new ArgumentException($"Размер поля должен быть от {MinSize} до {MaxSize}.");

            if (winLength < MinSize || winLength > size)
                throw new ArgumentException($"Длина победной линии должна быть от {MinSize} до размера поля.");

            Size = size;
            WinLength = winLength;

            Board = new CellState[size, size];

            CurrentPlayer = CellState.X;
        }

        public bool MakeMove(int row, int col)
        {
            if (IsFinished)
                return false;

            if (Board[row, col] != CellState.Empty)
                return false;

            Board[row, col] = CurrentPlayer;

            if (CheckWin(row, col))
            {
                IsFinished = true;
                IsDraw = false;
            }
            else if (IsBoardFull())
            {
                IsFinished = true;
                IsDraw = true;
            }
            else
            {
                SwitchPlayer();
            }

            return true;
        }

        void SwitchPlayer()
        {
            CurrentPlayer =
                CurrentPlayer == CellState.X
                ? CellState.O
                : CellState.X;
        }

        bool CheckWin(int row, int col)
        {
            return CheckDirection(row, col, 1, 0) ||
                   CheckDirection(row, col, 0, 1) ||
                   CheckDirection(row, col, 1, 1) ||
                   CheckDirection(row, col, 1, -1);
        }

        bool CheckDirection(int row, int col, int dx, int dy)
        {
            int count = 1;

            count += Count(row, col, dx, dy);
            count += Count(row, col, -dx, -dy);

            return count >= WinLength;
        }

        int Count(int row, int col, int dx, int dy)
        {
            int count = 0;

            int r = row + dx;
            int c = col + dy;

            while (r >= 0 && r < Size &&
                   c >= 0 && c < Size &&
                   Board[r, c] == CurrentPlayer)
            {
                count++;
                r += dx;
                c += dy;
            }

            return count;
        }

        bool IsBoardFull()
        {
            for (var row = 0; row < Size; row++)
            {
                for (var col = 0; col < Size; col++)
                {
                    if (Board[row, col] == CellState.Empty)
                        return false;
                }
            }

            return true;
        }
    }
}
