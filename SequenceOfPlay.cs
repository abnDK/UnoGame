public class SequenceOfPlay
{
    private int index;
    private bool reverse;
    public List<Player>? Players { get; set; }

    public Player? current;

    public SequenceOfPlay()
    {
        index = 10000;
        reverse = false;
    }

    public void SetPlayers(List<Player> players)
    {
        Players = players;
        current = Next();


    }

    public Player Next()
    {

        Increment();

        current = Players[index % Players.Count];
        return current;

    }

    public Player PeekNext()
    {
        throw new Exception("NOT IMPLEMENTED YET"); // find a way to get next without incrementing the index

    }

    public void Reverse()
    {
        reverse = !reverse;
    }

    public void Skip()
    {
        Increment();
    }

    private void Increment()
    {
        if (reverse)
        {
            index--;
        }
        else
        {
            index++;
        }
    }
}

