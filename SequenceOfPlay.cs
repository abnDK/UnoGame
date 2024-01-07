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

        current = Players?[index % Players.Count] ?? throw new Exception("No players available!");
        return current;

    }

    public Player PeekNext()
    {
        if (reverse)
            return Players?[(index - 1) % Players.Count] ?? throw new Exception("No players available!");
        return Players?[(index + 1) % Players.Count] ?? throw new Exception("No players available!");

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

