public class Player
{
    private string _name;
    public string Name
    {
        get
        {
            return _name;
        }
        private set
        {
            _name = value;
        }
    }
    private Hand _hand;

    public bool Uno = false;

    public Hand Hand
    {
        get
        {
            _hand.Sort();
            return _hand;
        }
    }

    public Player(string name)
    {
        _name = name;
        this._hand = new Hand();
    }

    public int CardCount()
    {
        return Hand.Count();
    }

    public void HideHand()
    {
        Hand.HideHand(true);
    }

    public void ShowHand()
    {
        Hand.HideHand(false);
    }

    public bool HiddenHand()
    {
        return Hand.HandHidden();
    }

    public void DrawCard(Card card)
    {
        Hand.Draw(card);
    }

    public void DrawCards(List<Card> cards)
    {
        foreach (Card card in cards)
            DrawCard(card);
    }

    public void SaysUno()
    {
        // if Uno is expressed while player has 1 card left, they CHEAT!
        // thus uno cannot be set!
        if (CardCount() == 1)
        {
            return;
        }
        Uno = true;
    }
    public bool ValidIdsOfPotentialHand(List<int> choices)
    {
        if (Hand.ValidIdsOfPotentialHand(choices))
            return true;

        return false;

    }

    public List<Card> PlayPotentialHand()
    {
        return Hand.ConfirmHand();

    }

    public List<Card> PotentialHand()
    {
        return Hand.PotentialHand();
    }

}


