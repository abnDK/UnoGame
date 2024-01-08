public class Dealer
{
    private Deck _deck;

    private int HANDSIZE = 7;
    private int STANDARDDRAWAMOUNT = 1;
    private int drawDebt = 0;
    private bool drawAllowed = true;

    public Player? CurrentPlayer { get; private set; }

    public Player? PreviousPlayer { get; private set; }

    public Player? Winner { get; set; }

    private SequenceOfPlay SequenceOfPlay { get; init; }

    public List<Player> Players
    {
        get
        {
            return SequenceOfPlay.Players ?? throw new Exception("No players set!");
        }
    }

    private HandValidator Validator { get; init; }

    public Dealer(Deck deck, SequenceOfPlay sequenceOfPlay, HandValidator validator)
    {
        _deck = deck;
        PrepareDeck();

        SequenceOfPlay = sequenceOfPlay;

        Validator = validator;
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

    public void AddPlayersAndDealCards(string[] players)
    {
        InitPlayers(players);

        DealCards();
    }

    private void DealCards()
    {
        /// DEAL CARDS TO PLAYERS OF GAME
        /// 


        foreach (Player player in Players)
        {
            // deal cards
            foreach (Card card in DrawNewCards(HANDSIZE))
            {
                player.DrawCard(card);
            }

            player.HideHand();

        }
    }

    public Card DealCard()
    {
        return _deck.DrawNext(1)[0];
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

    public void PlayHand(List<Card> hand)
    {
        if (hand.Count > 0)
        {
            foreach (Card card in hand)
            {
                PlayNext(card);

                // APPLY MODIFIER
                if (card is SpecialCard s)
                    ApplyModifier(s);
            }
        }
        else
        {
            throw new Exception("Cannot play an empty hand!");
        }
    }

    private void ApplyModifier(SpecialCard card)
    {

        if (card.modifier is DrawModifier d)
        {
            AddDrawDebt(d.amount);
        }
        if (card.modifier is SkipSequenceModifier)
        {
            SkipRound();
        }
        if (card.modifier is ReverseSequenceModifier)
        {
            ReverseRound();
        }
        if (card.modifier is ColorModifier c)
        {
            c.InitMod();
        }
    }

    public void PlayNext(Card card)
    {
        _deck.PushToPlayedCardsStack(card);
    }

    public List<Card> DrawNewCards(int amount)
    {
        List<Card> drawnCards = _deck.DrawNext(amount);

        return drawnCards;
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

    public void InitPlayersTest(string[] names)
    {
        InitPlayers(names);
    }
    private void InitPlayers(string[] names)
    {
        /// Create a new player object for each name in names
        /// 

        if (names.Length < 2)
            throw new Exception("At least 2 players are needed.");

        List<Player> players = new List<Player>();

        foreach (string name in names)
        {
            players.Add(new Player(name));
        }

        SequenceOfPlay.SetPlayers(players);

    }

    public void NextTurn()
    {
        if (CurrentPlayer != null)
            PreviousPlayer = CurrentPlayer;
        CurrentPlayer = SequenceOfPlay.Next();
        AllowDrawing(true);
    }

    public bool ValidHandToPlay(List<Card> hand)
    {
        return Validator.ValidateHand(
            hand,
            PeekPlayedTopCard(),
            GetCurrentDrawDebt() > 0 ? true : false
        );
    }

    private void SkipRound()
    {
        SequenceOfPlay.Skip();
    }

    private void ReverseRound()
    {
        SequenceOfPlay.Reverse();
    }

    public Player WhoIsNext()
    {
        return SequenceOfPlay.PeekNext();
    }

    public bool DoWeHaveAWinner()
    {
        // this checks if any of the players have 0 cards left.
        // has to be called after each turn has been finished.

        foreach (Player player in Players)
        {
            if (player.CardCount() == 0)
            {

                if (!player.Uno) // if player forgets to say "UNO", you have to draw 5 new cards!!!
                {
                    List<Card> newDrawnCards = DrawNewCards(5);

                    player.DrawCards(newDrawnCards);

                    break;

                }

                Winner = player;

                return true;
            }
            if (player.Hand.Cards.Count > 1)
                player.Uno = false;

        }

        return false;
    }
}