public class SilentLogger : ILogger
{
    public void InitGame(Game game)
    {
        // Console.WriteLine($"Logging who the players are {game.players}, id of game {game.id} and datetime {DateTime.Now}");
        // Console.ReadKey();

    }

    public void PlayerAction(Player player, string action, Card topcard, DateTime datetime, int gameId, Card? playedCard)
    {
        // Console.WriteLine("Logs the action of a player");
        // Console.ReadKey();

    }

    public void EndGame(Player player)
    {
        // Console.WriteLine($"Logs who won the game {player.name}");
        // Console.ReadKey();
    }
}