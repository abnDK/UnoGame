public class Game
{


    public int id;

    public Dealer _dealer;
    public List<Player>? players;

    private Player? winner;

    public SequenceOfPlay? turn;

    private List<Card> validNextSequence = new List<Card>();


    private IRender _render;

    private ILogger _logger;

    public Game(IRender render, ILogger logger, Dealer dealer, string[]? playerNames)
    {
        _render = render;
        _logger = logger;

        if (playerNames?.Length > 0)
        {
            players = InitPlayers(playerNames);
        }

        id = 1;

        _dealer = dealer;


    }



    public void InitGame()
    {

        _render.Splash();

        string choice = _render.Menu();

        if (choice == "play")
        {
            if (players == null)
            {
                string[] names = _render.Setup();
                players = InitPlayers(names);
            }



            turn = new SequenceOfPlay(players); // should be held by dealer? Through this, dealer nows the players as well

            // seed deck, shuffle it, turn 1st card onto stack and deal cards to players
            _dealer.PrepareDeck(); // rename to InitDeck() ??
            _dealer.DealCards(players);

            _logger.InitGame(this);


            while (!DoWeHaveAWinner())
            {
                NewTurn();
            }

            _render.Result(winner.name);

            EndGame();
        }
        else if (choice == "scoreboard")
        {
            _render.Scoreboard();
        }
        else
        {
            InitGame();
        }



    }


    private void NewTurn()
    {


        // make current player the next in sequence
        turn?.Next(); // should be the dealer who calls this.




        if (_dealer.IsDrawAllowed())
        {
            PlayOrDraw();
        }

        if (!_dealer.IsDrawAllowed())
        {
            // if we choose draw in prev we enter this
            PlayOrNext();
        }

    }

    private void PlayOrDraw()
    {
        // 

        ///
        // turn.current.ShowHand();
        RenderTurnStatus();

        int cardsToDraw = _dealer.AmountOfCardsToDrawNext(); // instead, just call a method in the console.writeline underneath this
        // we could then reset the draw debt as an internal proces every time Dealer.DealCard() has been called.

        Console.WriteLine($"(S)pil eller (T)ræk {cardsToDraw} kort op: ");
        string? readResult;
        List<int> choices = new List<int>();

        readResult = Console.ReadLine();

        if (readResult == "træk" || readResult == "t")
        {
            // draw cards
            Console.WriteLine("Drawing some cards");
            foreach (Card card in _dealer?.DrawNewCards(cardsToDraw))
            {

                /* alternative:
                CurrentPlayer will be datamember on dealer
                we could do a for (int i = 0; i < cardsToDraw; i++) Dealer.CurrentPlayer.DrawCard(Dealer.DealCard());
                and split the above up, so we still can log each card drawn.

                // MAKE Dealer a property Getter so we can Call Dealer instead of _dealer backing field.

                 */

                turn?.current.DrawCard(card); // instead should be _dealer.CurrentPlayer.DrawCard(card);
                _logger.PlayerAction(turn?.current, "træk", _dealer.PeekPlayedTopCard(), DateTime.Now, id, card);
            }

            _dealer.ResetDrawDebt(); // should be private / internal. When Dealer.DealCard() is called, drawdebt is set to it's default

            _dealer.AllowDrawing(false); // could be set to false everytime someone Dealer.DealCard() is called. It would then be set to true everytime a new turn is called.

            return;

        }

        else if (readResult != null && readResult.Split(" ").All((choice) => choice.Trim().ToLower() == "uno" || int.TryParse(choice, out int _)))
        {
            // "split result into numbers"
            foreach (string choice in readResult.Split(" "))
            {
                if (choice.Trim().ToLower() == "uno")
                {
                    turn.current.Uno = true; // should be Dealer.CurrentPlayer.Uno;

                } //Hvis ikke man har sagt uno skal man trække 2 kort op.
                else
                    choices.Add(Convert.ToInt32(choice));
            }



            // if choices are ids in valid range
            if (turn?.current.ValidatePotentialHand(choices) ?? false) // validation of a hand could be moved to a HandValidator class.
            {
                // if hand sequence is valid to play on stack
                if (ValidNext(turn.current.PotentialHand()))  // Dealer.CurrentPlayer.PotentialHand();
                {
                    // play the valid hand sequence
                    validNextSequence = turn.current.ConfirmHand(); // Dealer.CurrentPlayer.ConfirmHand();
                    PlayNext();
                }
                else
                {
                    Console.WriteLine("hand sequence is not valid...");
                    PlayOrDraw();
                }

            }
            else
            {
                Console.WriteLine("choices was not valid - out of range...");
                PlayOrDraw();
            }


        }
        else
        {
            PlayOrDraw();
        }


    }

    private void PlayOrNext()
    {

        RenderTurnStatus();

        Console.WriteLine("Spil eller Næste: ");
        string? readResult;
        List<int> choices = new List<int>();

        readResult = Console.ReadLine();

        if (readResult == "næste" || readResult == "n" || readResult == "")
        {
            // draw cards
            Console.WriteLine("No cards to play - next player!");

        }

        else if (readResult != null && readResult.Split(" ").All((choice) => choice.Trim().ToLower() == "uno" || int.TryParse(choice, out int _)))
        {
            // "spit result into numbers"
            foreach (string choice in readResult.Split(" "))
            {
                if (choice.Trim().ToLower() == "uno")
                {
                    turn.current.Uno = true;

                } //Hvis ikke man har sagt uno skal man trække 2 kort op.
                else
                    choices.Add(Convert.ToInt32(choice));
            }

            // if choices are ids in valid range
            if (turn.current.ValidatePotentialHand(choices))
            {
                // if hand sequence is valid to play on stack
                if (ValidNext(turn.current.PotentialHand()))
                {
                    // play the valid hand sequence
                    validNextSequence = turn.current.ConfirmHand();
                    PlayNext();
                }
                else
                {
                    Console.WriteLine("hand sequence is not valid...");
                    PlayOrNext();
                }

            }
            else
            {
                Console.WriteLine("choices was not valid - out of range...");
                PlayOrNext();
            }


        }
        else
        {
            PlayOrNext();
        }

        // get input. 
        // if draw, we play draw on player
        // if number, we send it to potential hand
        // we then confirm it.

        // ValidNext(turn.current.Play());

        ///


        _dealer.AllowDrawing(true);
    }

    private void EndGame()
    {
        /// Ends the game and announces a winner!
        /// 

        if (winner == null)
            throw new Exception("Cannot end game withot a winner");


        _logger.EndGame(winner);

        if (_render.Replay())
            InitGame();

        Console.WriteLine("Tak for spillet!");
        Console.WriteLine("Skal vi spille igen? (ja/nej)");

        string? readResult;

        readResult = Console.ReadLine();

        if (readResult.Trim().ToLower() == "ja")
            InitGame();

    }

    private bool ValidNext(List<Card> cards)
    {
        /// Validate if sequence of hands is valid sequence to play


        /// Takes in a sequence of cards to play (1 or more...)
        /// Responsible for validating 2 things:
        ///     1) is the first card in the sequence valid to play as a next card on the stack?
        ///     2) if 1 is true, and there is more than 1 card in the sequence, is the rest of the cards valid sequence cards?


        bool validNext = true;

        // is index 0 of sequence valid to play next?

        // if any drawDebt and index 0 is not a draw card, the play is invalid
        if (_dealer.GetCurrentDrawDebt() > 0)
        {
            Console.WriteLine("drawDebt above 0, validating for draw spec card");
            if (cards[0] is SpecialCard sc)
                Console.WriteLine($"SpecialCard modifier: {sc.modifier.GetType()}");

            if (cards[0] is SpecialCard specialCardWithDrawDebt)
            {
                if (specialCardWithDrawDebt.modifier is not DrawModifier)
                {
                    Console.WriteLine("Can only play special drawcard when drawdebt > 0");
                    Console.ReadLine();
                    return false;
                }
            }
            else if (cards[0] is RegCard)
            {
                Console.WriteLine("Cannot play reg card when there is drawDebt");
                Console.ReadLine();
                return false;

            }

        }


        // check if first card of sequence is valid to play on stack topcard
        Card playedTopCard = _dealer.PeekPlayedTopCard();
        if (playedTopCard is RegCard playedTopRegCard)
        {
            if (cards[0] is RegCard r)
            {
                validNext = playedTopRegCard.ValidNext(r);
            }
            else if (cards[0] is SpecialCard s)
            {
                validNext = playedTopRegCard.ValidNext(s);
            }
        }
        else if (playedTopCard is SpecialCard playedTopSpecCard)
        {
            if (cards[0] is RegCard r)
            {
                validNext = playedTopSpecCard.ValidNext(r);

            }
            else if (cards[0] is SpecialCard s)
            {
                validNext = playedTopSpecCard.ValidNext(s);

            }
        }

        if (!validNext)
        {
            Console.WriteLine("Det kort kan man ikke spille her. Prøv igen!");
            Console.ReadLine();
            validNextSequence.Clear();
            return false;
        }


        // is cards valid to play as a sequence?
        // sequence rules: (if more than 1 card in sequence)
        //      if index 0 is a regular, all other must be regular and same number
        //      if index 0 is a special, it, and all others must be a special draw card (other specialcard types can only be played 1 at the time)

        if (cards.Count > 1)
        {
            for (int i = 0; i < cards.Count - 1; i++)
            {
                Card current = cards[i];
                Card next = cards[i + 1];

                if (current is RegCard cR)
                {
                    if (next is RegCard nR)
                    {
                        if (cR.number != nR.number)
                            validNext = false;
                    }
                    if (next is SpecialCard)
                    {
                        validNext = false;
                    }
                }
                if (current is SpecialCard cS)
                {
                    if (cS.modifier is not DrawModifier)
                        validNext = false;

                    if (next is RegCard)
                    {
                        validNext = false;
                    }
                    if (next is SpecialCard nS)
                    {
                        if (nS.modifier is not DrawModifier)
                        {
                            validNext = false;
                        }

                    }
                }

                if (!validNext)
                {
                    Console.WriteLine("Nogle af kortene passer ikke sammen. Prøv igen!");
                    Console.ReadLine();
                    validNextSequence.Clear();
                    return false;
                }

            }
        }

        validNextSequence = cards;
        return true;


    }

    private void PlayNext()
    {
        /// Will play the last validated sequence of cards.
        /// If it was not valid, this will throw Exception
        if (validNextSequence.Count > 0)
        {
            foreach (Card card in validNextSequence)
            {
                _dealer.PlayNext(card);

                // APPLY MODIFIER
                if (card is SpecialCard s)
                    ApplyModifier(s);
            }
        }
        else
        {
            Console.WriteLine("No next valid sequence to play!");
            Console.ReadLine();
        }

    }

    private List<Player> InitPlayers(string[] names)
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

        return players;

    }

    private bool DoWeHaveAWinner()
    {
        // this checks if any of the players have 0 cards left.
        // has to be called after each turn has been finished.

        foreach (Player player in players)
        {
            if (player.Hand.cards.Count == 0)
            {

                if (!player.Uno) // if player forgets to say "UNO", you have to draw 5 new cards!!!
                {

                    player.DrawCards(_dealer.DrawNewCards(5));

                    break;

                }

                winner = player;

                return true;
            }
            if (player.Hand.cards.Count > 1)
                player.Uno = false;

        }

        return false;
    }

    private void ApplyModifier(SpecialCard card)
    {

        if (card.modifier is DrawModifier d)
        {
            _dealer.AddDrawDebt(d.amount);
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




    private void RenderTurnStatus()
    {
        _render.Turn(this);

    }




    private void showPlayers()
    {
        foreach (Player player in this.players)
        {
            if (player == this.turn.current)
                System.Console.WriteLine($"* {player.name}");

            else
                System.Console.WriteLine($"  {player.name}");
        }
    }

    private void playHand()
    {

    }

    private void SkipRound()
    {
        turn.Skip();
    }

    private void ReverseRound()
    {
        turn.Reverse();
    }

}

