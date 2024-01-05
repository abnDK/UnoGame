public class Deck
{
    private List<Card> storedCards;
    private Stack<Card> playedCards;

    public Deck()
    {

        this.storedCards = new List<Card>();
        this.playedCards = new Stack<Card>();

    }


    // SEEDING THE DECK
    private List<RegCard> SeedRegCards()
    {
        string[] colors = new string[] { "green", "red", "blue", "yellow" };
        byte[] numbers = new byte[] { 1, 2, 3, 4, 5, 6, 7, 8, 9 };
        List<RegCard> regCards = new List<RegCard>();

        foreach (string color in colors)
        {
            foreach (byte number in numbers)
            {
                regCards.Add(new RegCard(color, number));
            }
        }

        return regCards;
    }

    private List<RegCard> SeedRegCards(int factor)
    {

        List<RegCard> regCards = new List<RegCard>();

        for (int i = 0; i < factor; i++)
        {
            List<RegCard> factorNRegCards = this.SeedRegCards();
            regCards.AddRange(factorNRegCards);
        }

        return regCards;
    }

    private List<SpecialCard> SeedSpecialCards()
    {
        List<SpecialCard> specialCards = [
        new SpecialCard("green", new DrawModifier(3)),
        new SpecialCard("red", new DrawModifier(3)),
        new SpecialCard("green", new DrawModifier(3)),
        new SpecialCard("red", new DrawModifier(3)),
        new SpecialCard("yellow", new SkipSequenceModifier()),
        new SpecialCard("blue", new SkipSequenceModifier()),
        new SpecialCard("red", new SkipSequenceModifier()),
        new SpecialCard("green", new SkipSequenceModifier()),
        new SpecialCard("yellow", new ReverseSequenceModifier()),
        new SpecialCard("blue", new ReverseSequenceModifier()),
        new SpecialCard("red", new ReverseSequenceModifier()),
        new SpecialCard("green", new ReverseSequenceModifier()),
        new SpecialCard(new ColorModifier()),
        new SpecialCard(new ColorModifier()),
        new SpecialCard(new ColorModifier()),
        new SpecialCard(new ColorModifier())
        ];

        return specialCards;
    }

    private List<SpecialCard> SeedSpecialCards(int factor)
    {
        List<SpecialCard> specialCards = new List<SpecialCard>();

        for (int i = 0; i < factor; i++)
        {
            List<SpecialCard> factorNSpecialCards = SeedSpecialCards();
            specialCards.AddRange(factorNSpecialCards);
        }

        return specialCards;
    }

    public void SeedNewDeck(int regFactor = 1, int specFactor = 1)
    {
        List<Card> cards = new List<Card>();

        List<RegCard> regCards = this.SeedRegCards(regFactor);

        List<SpecialCard> specialCards = this.SeedSpecialCards(specFactor);

        foreach (RegCard card in regCards)
            cards.Add(card);

        foreach (SpecialCard card in specialCards)
            cards.Add(card);

        storedCards = cards;
    }



    // SHOWING THE DECK 




    public int playedCardsCount()
    {
        return playedCards.Count;
    }

    public int storedCardsCount()
    {
        return storedCards.Count;
    }



    /* public Card PeekPlayedTopCard()
    {
        return playedCards.Peek();
    } */

    public Card PeekPlayedTopCard(int numFromTop = 0)
    {
        // hvordan får vi kort nr. n fra toppen?
        return playedCards.ToArray()[numFromTop];
    }

    private Card PeekNext()
    {
        // return the next card in the storedCards stack but does not remove it
        return storedCards[storedCards.Count - 1];

    }

    private Card DrawNext()
    {
        // return the next card in the storedCards stack and removes it


        // if storedCards is Empty, seed new cards before drawing the next card
        if (storedCards.Count == 0)
        {
            SeedNewDeck();
        }

        Card nextCard = PeekNext();
        storedCards.RemoveAt(storedCards.Count - 1);

        return nextCard;
    }

    public List<Card> DrawNext(int amount)
    {
        /// Draw N amount of cards from the storedCards list.


        List<Card> drawnCards = new List<Card>();

        for (int i = 0; i < amount; i++)
        {
            Card poppedCard = DrawNext();
            // Card poppedElement = this.storedCards[this.storedCards.Count - 1];
            // this.storedCards.RemoveAt(this.storedCards.Count - 1);
            drawnCards.Add(poppedCard);
        }
        return drawnCards;

    }

    public RegCard DrawNextReg()
    {
        // return the next regular card in the storedCards stack and removes it

        Card drawnCard = DrawNext();


        if (drawnCard is RegCard r)
            return r;
        else
            return DrawNextReg();



    }








    // MANIPULATING THE DECK
    public Card AtIndex(int storedCardIndex)
    {
        return storedCards[storedCardIndex];
    }

    public void InsertAtIndex(int insertIndex, Card cardToInsert)
    {
        storedCards[insertIndex] = cardToInsert;
    }

    public void PushToPlayedCardsStack(Card card)
    {
        this.playedCards.Push(card);
    }





}
