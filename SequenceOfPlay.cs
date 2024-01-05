public class SequenceOfPlay
{
    private int index;
    private bool reverse;
    public List<Player> sequence;

    public Player current;

    public SequenceOfPlay(List<Player> players)
    {
        this.index = 10000;
        this.reverse = false;
        this.sequence = players;
        this.current = Next();
    }

    public Player Next()
    {

        Increment();

        current = sequence[index % sequence.Count];
        return current;

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

