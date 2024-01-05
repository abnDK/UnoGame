public interface IRender
{
    abstract void Splash(); // Show game splash

    abstract string Menu(); // let user choose between start game and go to scoreboard

    abstract string[] Setup(); // let user input names of players

    abstract void Turn(Game game); // main screen, rendering each turn of the game until a winner is found

    abstract void Result(string nameOfWinner); // renders who won the game

    abstract bool Replay(); // lets user choose wether to play again or not

    abstract void Scoreboard(); // show the scoreboard (not implemented yet) 

}