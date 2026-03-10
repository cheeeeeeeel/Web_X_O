using Web_X_O.Models;

namespace Web_X_O.Services
{
    public class GameManager
    {
        Dictionary<string, Game> games = new();

        public Game CreateGame(string id, int size, int winLength)
        {
            var game = new Game(size, winLength);

            games[id] = game;

            return game;
        }

        public Game GetGame(string id)
        {
            return games[id];
        }
    }
}