
public class Hand
{
    private List<Card> _cards;

    private List<Card>? potentialHand;

    private List<int>? potentialHandIds;

    private List<Card>? remainingHand;

    private bool hidden;


    public Hand()
    {
        _cards = new List<Card>();

    }

    public void HideHand(bool makeHidden)
    {
        hidden = makeHidden;
    }

    public bool HandHidden()
    {
        return hidden;
    }

    public List<Card> Cards
    {
        get
        {
            if (hidden)
            {

                List<Card> hiddenHand = new List<Card>();

                for (int i = 0; i < _cards.Count; i++)
                {
                    hiddenHand.Add(new HiddenCard("None"));
                }

                return hiddenHand;
            }
            return _cards;
        }
        set
        {
            _cards = value;
        }
    }

    public void Draw(Card card)
    {

        // if hand is hidden, we cannot add cards to the hand
        // so we temporarily show hand and return to previous state
        // afterwards.

        bool tempHidden = hidden;
        if (hidden)
            hidden = false;

        Cards.Add(card);

        hidden = tempHidden;

    }
    public bool ValidIdsOfPotentialHand(List<int> ids)
    {
        /// Validates if ids is in valid range of the players current hand

        // filter hand for cards matching the ids
        // doesnt remove them from the hand
        potentialHand = new List<Card>();
        potentialHandIds = new List<int>();

        // VALIDATION FOR IDS BEING IN RANGE!!
        if (ids.Any((id) =>
        {
            bool cond = id < 1 || id > Cards.Count;
            return cond;
        }))
        {
            return false;
        }

        foreach (int id in ids)
        {
            // ids stem from choices in a menu, that is 1-indexed
            int zeroIndexId = id - 1;
            potentialHand.Add(Cards[zeroIndexId]);
            potentialHandIds.Add(zeroIndexId);

        }



        return true;

    }

    public List<Card> PotentialHand()
    {
        if (potentialHand == null)
            throw new Exception("No potential hand available");

        return potentialHand;
    }

    public List<Card> ConfirmHand()
    {
        // takes the latest potential hand 
        // and returns the cards
        // and filter the remaining cards
        // and stores then in the hand member.

        // this first filters the cards needed (dont remove form hand)

        // then we iterate through the hand
        // and all cards not matching the ids
        // is written to the remaining hand var
        // 
        // this solves the problem of
        // returning the proper cards and preserve order
        // of them
        // 
        // and it filters the remaining from index
        // 0 to last without thinking about the
        // order of the cards in the hand.

        if (potentialHandIds == null)
            throw new Exception("Cannot confirm potential hand when potential id's are null");

        // IMPLEMENTATION
        // filter out played cards
        var confirmedHand = potentialHand ?? throw new Exception("No potential hand");

        // filter out remaining hand

        remainingHand = new List<Card>();

        for (int i = 0; i < Cards.Count; i++)
        {
            if (!potentialHandIds.Contains(i))
            {
                remainingHand.Add(Cards[i]);
            }
        }

        Cards = remainingHand;

        return confirmedHand;
    }

    public int Count()
    {
        return Cards.Count;
    }

    public void Sort()
    {
        Cards.Sort((Card a, Card b) =>
        {
            // implemented custom CompareTo on Card class
            return a.CompareTo(b);

        });
    }
}



