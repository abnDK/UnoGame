public class Game
{


    public int id;

    public Dealer Dealer { get; init; }

    private Player? winner;

    public SequenceOfPlay? turn;

    private IRender _render;

    private ILogger _logger;

    public Game(IRender render, Dealer dealer, ILogger? logger = null)
    {
        if (logger != null)
            _logger = logger;
        else
            _logger = new SilentLogger();

        _render = render;

        id = 1; // for logging

        Dealer = dealer;

    }



    public void InitGame()
    {

        _render.Splash();

        string choice = _render.Menu();

        if (choice == "play")
        {

            string[] playerNames = _render.Setup();


            // seed deck, shuffle it, turn 1st card onto stack and deal cards to players
            Console.WriteLine(Dealer);
            Dealer.AddPlayersAndDealCards(playerNames);

            _logger.InitGame(this);


            while (!Dealer.DoWeHaveAWinner())
            {
                NewTurn();
            }

            _render.Result(Dealer.Winner?.Name ?? "Ukendt vinder");

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
        // this should only be called when it is a new players turn.
        // the !DrawAllowed call is a potential step 2 
        // of the same players turn. Thus we cannot have
        // the logic in this function.
        // We need to makes sure, that PlayOrDraw does not return
        // until the player either played
        // a valid card or decided to pass
        // on the turn to the next player
        // after drawing a card.
        // This should only call:
        //      Dealer.NextTurn()
        //      PlayOrDraw()

        // make current player the next in sequence
        Dealer.NextTurn(); // should be the dealer who calls this.

        PlayOrDraw();

        return;

    }

    private void PlayOrDraw()
    {


        // if player plays a "hop" card and the turn returns to the same player. No cards has to be hidden.
        if (Dealer.CurrentPlayer == Dealer.PreviousPlayer)
            Dealer.CurrentPlayer?.ShowHand();

        RenderTurnStatus();

        // Wait for player to unhide his/her hand
        while (Dealer.CurrentPlayer?.HiddenHand() ?? true)
        {
            Console.WriteLine("Tryk 'Mellemrum' for at se dine kort.");
            ConsoleKeyInfo keyInput = Console.ReadKey();

            if (keyInput.Key == ConsoleKey.Spacebar)
            {
                Dealer.CurrentPlayer?.ShowHand();
                RenderTurnStatus();
                break;
            }
        }

        // int cardsToDraw = _dealer.AmountOfCardsToDrawNext(); // instead, just call a method in the console.writeline underneath this
        // we could then reset the draw debt as an internal proces every time Dealer.DealCard() has been called.

        Console.WriteLine($"(S)pil eller (T)ræk {Dealer.AmountOfCardsToDrawNext()} kort op: ");
        string? readResult;
        List<int> choices = new List<int>();

        readResult = Console.ReadLine();

        if (readResult == "træk" || readResult == "t")
        {
            // draw cards
            for (int i = 0; i < Dealer.AmountOfCardsToDrawNext(); i++)
            {
                Card card = Dealer.DealCard();
                Dealer.CurrentPlayer?.DrawCard(card);
                _logger.PlayerAction(
                    Dealer.CurrentPlayer ?? new Player("Ukendt spiller"),
                    "træk",
                    Dealer.PeekPlayedTopCard(),
                    DateTime.Now,
                    id,
                    card
                );

            }

            // After drawing 1 or more cards, we reset AmountOfCardsToPlay back to the default value
            Dealer.ResetDrawDebt();

            Dealer.AllowDrawing(false); // could be set to false everytime someone Dealer.DealCard() is called. It would then be set to true everytime a new turn is called.

            PlayOrNext();
            return;


        }

        else if (readResult != null && readResult.Split(" ").All((choice) => choice.Trim().ToLower() == "uno" || int.TryParse(choice, out int _)))
        {
            // "split result into numbers"
            foreach (string choice in readResult.Split(" "))
            {
                if (choice.Trim().ToLower() == "uno")
                {
                    Dealer.CurrentPlayer?.SaysUno();

                }
                else
                    choices.Add(Convert.ToInt32(choice));
            }

            // if choices are ids in valid range
            if (Dealer.CurrentPlayer?.ValidIdsOfPotentialHand(choices) ?? throw new Exception("CurrentPlayer not found!"))
            {
                // if hand sequence is valid to play on stack
                if (Dealer.ValidHandToPlay(Dealer.CurrentPlayer.PotentialHand()))
                {
                    // play the valid hand sequence
                    Dealer.PlayHand(Dealer.CurrentPlayer.PlayPotentialHand());

                    Dealer.CurrentPlayer.HideHand();
                    return;
                }

            }


        }
        PlayOrDraw();
        return;



    }

    private void PlayOrNext()
    {


        RenderTurnStatus();


        // int cardsToDraw = _dealer.AmountOfCardsToDrawNext(); // instead, just call a method in the console.writeline underneath this
        // we could then reset the draw debt as an internal proces every time Dealer.DealCard() has been called.

        Console.WriteLine("Spil eller Næste: ");
        string? readResult;
        List<int> choices = new List<int>();

        readResult = Console.ReadLine();

        if (readResult == "næste" || readResult == "n")
        {
            Console.WriteLine($"Turen går videre til {Dealer.WhoIsNext()}");
            Console.ReadLine();
            return;
        }

        else if (readResult != null && readResult.Split(" ").All((choice) => choice.Trim().ToLower() == "uno" || int.TryParse(choice, out int _)))
        {
            // "split result into numbers"
            foreach (string choice in readResult.Split(" "))
            {
                if (choice.Trim().ToLower() == "uno")
                {
                    Dealer.CurrentPlayer?.SaysUno();

                }
                else
                    choices.Add(Convert.ToInt32(choice));
            }



            // if choices are ids in valid range
            if (Dealer.CurrentPlayer?.ValidIdsOfPotentialHand(choices) ?? throw new Exception("CurrentPlayer not found!")) // validation of a hand could be moved to a HandValidator class.
            {
                // if hand sequence is valid to play on stack
                if (Dealer.ValidHandToPlay(Dealer.CurrentPlayer.PotentialHand()))  // Dealer.CurrentPlayer.PotentialHand();
                {
                    // play the valid hand sequence
                    Dealer.PlayHand(Dealer.CurrentPlayer.PlayPotentialHand()); // Dealer.CurrentPlayer.ConfirmHand();
                    return;
                }

            }

        }

        PlayOrNext();
        return;

    }


    private void EndGame()
    {
        /// Ends the game and announces a winner!
        /// 

        if (Dealer.Winner == null)
            throw new Exception("Cannot end game withot a winner");


        _logger.EndGame(Dealer.Winner);

        if (_render.Replay())
            InitGame();

        Console.WriteLine("Tak for spillet!");
        Console.WriteLine("Skal vi spille igen? (ja/nej)");

        string? readResult;

        readResult = Console.ReadLine();

        if (readResult != null && readResult.Trim().ToLower() == "ja")
            InitGame();

    }

    private void RenderTurnStatus()
    {
        _render.Turn(this);

    }






}

