using Microsoft.AspNetCore.SignalR;
using Web_X_O.Models;
using Web_X_O.Services;

namespace Web_X_O.Hubs
{
    public class GameHub : Hub
    {
        private readonly GameManager manager;

        public GameHub(GameManager manager)
        {
            this.manager = manager;
        }

        public async Task JoinGame(string gameId)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, gameId);

            var game = manager.GetGame(gameId);
            await Clients.Caller.SendAsync(
                "UpdateBoard",
                ToClientBoard(game),
                game.CurrentPlayer,
                game.IsFinished
            );
        }

        public async Task MakeMove(string gameId, int row, int col)
        {
            var game = manager.GetGame(gameId);

            if (game.MakeMove(row, col))
            {
                await Clients.Group(gameId).SendAsync(
                    "UpdateBoard",
                    ToClientBoard(game),
                    game.CurrentPlayer,
                    game.IsFinished
                );
            }
        }

        private static int[][] ToClientBoard(Game game)
        {
            var board = new int[game.Size][];

            for (var row = 0; row < game.Size; row++)
            {
                board[row] = new int[game.Size];
                for (var col = 0; col < game.Size; col++)
                {
                    board[row][col] = (int)game.Board[row, col];
                }
            }

            return board;
        }
    }
}
