public class Dealer
{
    private Deck _deck;

    private int HANDSIZE = 7;
    private int STANDARDDRAWAMOUNT = 1;
    private int drawDebt = 0;
    private bool drawAllowed = true;

    public Dealer(Deck deck)
    {
        _deck = deck;
        PrepareDeck();
    }

    public void PrepareDeck()
    {
        _deck.SeedNewDeck();

        ShuffleDeck();

        TurnCardOntoStack();

    }

    private void ShuffleDeck()
    {
        // defines 2 indexes in storedCards and switches the cards around

        Card tempCard;
        Random random = new Random();

        for (int i = 0; i < 10000; i++)
        {
            int cardAIndex = i % _deck.storedCardsCount();
            int cardBIndex = random.Next(1, _deck.storedCardsCount());

            tempCard = _deck.AtIndex(cardBIndex);
            _deck.InsertAtIndex(cardBIndex, _deck.AtIndex(cardAIndex));
            _deck.InsertAtIndex(cardAIndex, tempCard);
        }


    }

    private void TurnCardOntoStack()
    {
        RegCard newTopCard = DealNextRegCard();

        _deck.PushToPlayedCardsStack(newTopCard);
    }

    public void DealCards(List<Player> players)
    {
        /// DEAL CARDS TO PLAYERS OF GAME
        /// 
        foreach (Player player in players)
        {
            // deal regular cards
            foreach (Card card in _deck.DrawNext(HANDSIZE))
            {
                player.DrawCard(card);
            }

        }
    }

    public int AmountOfCardsToDrawNext()
    {
        return drawDebt > 0 ? drawDebt : STANDARDDRAWAMOUNT;
    }

    public Card PeekPlayedTopCard(int numFromTop = 0)
    {
        return _deck.PeekPlayedTopCard(numFromTop);
    }

    public int PlayedCardsCount()
    {
        return _deck.playedCardsCount();
    }

    private RegCard DealNextRegCard()
    {
        return _deck.DrawNextReg();
    }

    public void PlayNext(Card card)
    {
        _deck.PushToPlayedCardsStack(card);
    }

    public List<Card> DrawNewCards(int amount)
    {
        return _deck.DrawNext(amount);
    }


    public int GetCurrentDrawDebt()
    {
        return drawDebt;
    }

    public void AddDrawDebt(int amount)
    {
        drawDebt += amount;
    }


    public void ResetDrawDebt()
    {
        drawDebt = 0;
    }

    public bool IsDrawAllowed()
    {
        return drawAllowed;
    }

    public void AllowDrawing(bool allow)
    {
        if (allow)
            drawAllowed = true;
        else
            drawAllowed = false;
    }
}