public interface ILogger
{
    abstract void InitGame(Game game);

    abstract void PlayerAction(Player player, string action, Card topcard, DateTime datetime, int gameId, Card? playedCard);
    /*
    Method purpose:
    
    It should be possible to recreate the game from the logfile. For this we need
    the stack, the players hand, the card player/the card drawn/if the player chooses next
    
    - Log when a card is played
    - Log if the player decides to draw a card
    - Log if the player decides to pass to next player

    - A log should contain: playername, player hand, action (card played, card drawn, next), stack, timestamp, game id, card

    */

    abstract void EndGame(Player winner);
}