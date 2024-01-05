public class Player
{

    public string name;
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
        this.name = name;
        this._hand = new Hand();
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

    public bool ValidatePotentialHand(List<int> choices)
    {
        if (Hand.ValidatePotentialHand(choices))
            return true;

        return false;

    }

    public List<Card> ConfirmHand()
    {
        return Hand.ConfirmHand();

    }

    public List<Card> PotentialHand()
    {
        return Hand.PotentialHand();
    }

}


