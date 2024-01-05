

using System.Collections;


public class BasicRender : IRender
{

    public void Splash()
    {

        Console.WriteLine("WELCOME TO:");
        Console.WriteLine("");
        Console.WriteLine("");
        Console.WriteLine("EEEEE SSSSS TTTTTT");
        Console.WriteLine("E     S       TT  ");
        Console.WriteLine("EEEEE SSSSS   TT  ");
        Console.WriteLine("E         S   TT  ");
        Console.WriteLine("EEEEE SSSSS   TT  ");
        Console.WriteLine("");
        Console.WriteLine("H   H EEEEE RRRR  ");
        Console.WriteLine("H   H E     R   R ");
        Console.WriteLine("HHHHH EEEEE RRRR  ");
        Console.WriteLine("H   H E     R  R  ");
        Console.WriteLine("H   H EEEEE R   R ");
        Console.WriteLine("");
        Console.WriteLine("U   U N   N  OOO  ");
        Console.WriteLine("U   U NN  N O   O ");
        Console.WriteLine("U   U N N N O   O ");
        Console.WriteLine("U   U N  NN O   O ");
        Console.WriteLine(" UUU  N   N  OOO  ");

        Console.ReadKey();
    }

    public string Menu()
    {
        Console.Clear();
        Console.WriteLine("Press enter to start the game");
        Console.ReadKey();
        return "play";
    }

    public string[] Setup()
    {

        string? readResult;

        Console.WriteLine("Indtast navne på dem, der skal være med:");
        readResult = Console.ReadLine();

        if (readResult != null)
            return readResult.Split(", ");

        return Setup();
    }

    public void Turn(Game game)
    {
        /// renders the current status of the game at each beginning of a new turn.
        /// Could be:
        ///     Who's turn it is
        ///     cards players etc.
        ///     

        Console.Clear();
        Console.WriteLine("PLAYERS: ");
        foreach (Player player in game.turn.sequence)
        {
            if (game.turn.current == player)
            {
                Console.WriteLine($"-> {player.name}");
            }
            else
            {
                Console.WriteLine($"   {player.name}");
            }
        }

        renderStack(game._dealer.PeekPlayedTopCard());

        Console.WriteLine("PLAYER HAND: ");
        // turn.current.ShowHand();
        RenderPlayerHand(game.turn.current.Hand.cards);


    }

    public void Result(string nameOfWinner)
    {
        Console.WriteLine($"{nameOfWinner} VINDER SPILLET!");
    }

    public bool Replay()
    {
        string? readResult;

        Console.WriteLine("Skal vi spille igen? (ja/nej)");
        readResult = Console.ReadLine();


        if (readResult == "ja")
            return true;

        if (readResult == "nej")
            return false;

        return Replay();

    }

    public void Scoreboard()
    {
        Console.WriteLine("NOT YET IMPLEMENTED");
    }


    /* public static void BeforeInputAfter(string before, string input, string after, string color)
    {

        Console.ResetColor();
        Console.Write(before);

        switch (color)
        {
            case "blue":
                Console.BackgroundColor = ConsoleColor.Blue;
                Console.ForegroundColor = ConsoleColor.White;
                break;
            case "yellow":
                Console.BackgroundColor = ConsoleColor.Yellow;
                Console.ForegroundColor = ConsoleColor.Black;
                break;
            case "green":
                Console.BackgroundColor = ConsoleColor.Green;
                Console.ForegroundColor = ConsoleColor.Black;
                break;
            case "red":
                Console.BackgroundColor = ConsoleColor.Red;
                Console.ForegroundColor = ConsoleColor.White;
                break;
            case "none":
                Console.BackgroundColor = ConsoleColor.White;
                Console.ForegroundColor = ConsoleColor.Black;
                break;
            default:
                break;

        }

        System.Console.Write(input);

        Console.ResetColor();

        Console.Write(after);

    }

    */
    private static void SetConsoleColor(string color)
    {
        switch (color)
        {
            case "blue":
                Console.BackgroundColor = ConsoleColor.Blue;
                Console.ForegroundColor = ConsoleColor.White;
                break;
            case "yellow":
                Console.BackgroundColor = ConsoleColor.Yellow;
                Console.ForegroundColor = ConsoleColor.Black;
                break;
            case "green":
                Console.BackgroundColor = ConsoleColor.Green;
                Console.ForegroundColor = ConsoleColor.Black;
                break;
            case "red":
                Console.BackgroundColor = ConsoleColor.Red;
                Console.ForegroundColor = ConsoleColor.White;
                break;
            case "none":
                Console.BackgroundColor = ConsoleColor.White;
                Console.ForegroundColor = ConsoleColor.Black;
                break;
            default:
                break;

        }
    }

    private static string resolveCardMiddle(Card card)
    {
        if (card is RegCard r)
            return $"  {r.number}  ";
        if (card is SpecialCard s)
        {
            if (s.modifier is DrawModifier dm)
                return $" + {dm.amount} ";
            if (s.modifier is ColorModifier)
                return " W C ";
            if (s.modifier is SkipSequenceModifier)
                return " HOP ";
            if (s.modifier is ReverseSequenceModifier)
                return " < > ";
        }
        return " ??? ";
    }

    private static void RenderCardNum(int num, bool split, bool last)
    {
        if (num < 10)
            Console.Write($" ({num}) ");
        else
            Console.Write($"({num}) ");


        if (last)
            Console.Write("\n");

        if (split && !last)
            Console.Write(" ");

        if (split && last)
            Console.Write("\n");
    }

    private static void RenderCardTop(Card card, bool split, bool last)
    {
        Console.ResetColor();
        SetConsoleColor(card.Color);
        Console.Write("     ");
        Console.ResetColor();

        if (last)
            Console.Write("\n");

        if (split && !last)
            Console.Write(" ");

        if (split && last)
            Console.Write("\n");

    }

    private static void RenderCardMiddle(Card card, bool split, bool last)
    {
        Console.ResetColor();
        SetConsoleColor(card.Color);
        Console.Write(resolveCardMiddle(card)); // 5 char's of color
        Console.ResetColor();

        if (last)
            Console.Write("\n");

        if (split && !last)
            Console.Write(" ");

        if (split && last)
            Console.Write("\n");

    }

    private static void RenderCardBottom(Card card, bool split, bool last)
    {
        Console.ResetColor();
        SetConsoleColor(card.Color);
        Console.Write("     "); // 5 char's of color
        Console.ResetColor();

        if (last)
            Console.Write("\n");

        if (split && !last)
            Console.Write(" ");

        if (split && last)
            Console.Write("\n");

    }

    private static void RenderPlayerHand(List<Card> hand)
    {
        // Should render hand in horisontal matter.
        // Should be able to specify how many columns of cards to
        // be shown before a new line of cards are shown, i.e 5 columns pr. row:

        // Cards:
        // x x x x x
        // x x x x x
        // x x x

        // card is rendered as such: (# = color, WC = WildCard = choose color)
        //  (n)   (n)   (n)   (n)   (n) 
        // ##### ##### ##### ##### #####
        // #+#3# #W#C# ##3## #REV# #SKP#
        // ##### ##### ##### ##### #####
        //
        //  (n)   (n)   (n)   (n)   (n)  
        // ##### ##### ##### ##### #####
        // #+#3# #W#C# ##3## #REV# #SKP#
        // ##### ##### ##### ##### #####

        // TOP AND BOTTOM IS SAME. 

        // do we need to know how many rows?
        // if we have loop that renders each card (3 rows)
        // and break out of loop each time we cardIndex % columns = 0
        // and prints new line character...

        // we need functions that returns strings
        // we need to append them together

        // for int i...
        // append text to before, name and after string
        // when we modulos == 0;
        // print appended string and continue proces until no more cards...

        // to be able to color text, we cannot print all in one go
        // we need to use Console.Write with color set and reset for each 
        // before, name, after in the card...

        // could we use a delegate here to store write command with color information?

        int columns = 8;
        // Console.WriteLine($"Render hands with columns: {columns}");

        // List<RenderCardElementCallback> top = new List<RenderCardElementCallback>();
        // List<RenderCardElementCallback> middle;
        // List<RenderCardElementCallback> bottom;
        // ArrayList topTest = new ArrayList();
        // List<Action> topTestHest = new List<Action>();
        // List<Action> middleTestHest = new List<Action>();
        // List<Action> bottomTestHest = new List<Action>();
        // bool split;
        // bool last;

        List<ArrayList> cardsToRender = new List<ArrayList>();

        for (int i = 0; i < hand.Count; i++)
        {
            Card currentCard = hand[i];


            // standard is to add a split
            bool last = false;
            bool split = true;


            // but if last, we skip the split and add a new line
            if ((i + 1) % columns == 0 || (i + 1) == hand.Count)
            {
                last = true;
                split = false;
            }

            // list of 
            // Card, bool, bool
            ArrayList currentCardToRender = new ArrayList();
            currentCardToRender.Add(currentCard);
            currentCardToRender.Add(split);
            currentCardToRender.Add(last);


            cardsToRender.Add(currentCardToRender);


            /* topTestHest.Add([new Action(() =>
            {
                RenderCardTop;
            }), i]);
            middleTestHest.Add(new Action(() =>
            {
                int index = i;
                RenderCardMiddle(hand[index], split, last);
            }));
            bottomTestHest.Add(new Action(() =>
            {
                int index = i;
                RenderCardBottom(hand[index], split, last);
            }));
 */
            // if ((i + 1) % columns == 0)
            // {


            //     /* foreach (Action<int> a in topTestHest)
            //     {
            //         a.Invoke();
            //     }
            //     foreach (Action a in middleTestHest)
            //     {
            //         a.Invoke();
            //     }
            //     foreach (Action a in bottomTestHest)
            //     {
            //         a.Invoke();
            //     } */
            //     // insert space between rows of cards
            //     Console.WriteLine();
            // }


            // add to string arrays
            // RenderCardElementCallback toRenderTop = RenderCardTop;
            // top.Add(toRenderTop(hand[i], split, last));
            // topTest.Add(toRenderTop(hand[i], split, last));



            // if modulos == 0; call all delegates, clear arrays and keep running the loop.
            // + Write a new line to seperate rows.




        }

        List<ArrayList> rowOfCards = new List<ArrayList>();
        int cardNumber = 1;
        for (int i = 0; i < cardsToRender.Count; i++)
        {
            rowOfCards.Add(cardsToRender[i]);

            if ((i + 1) % columns == 0 || (i + 1) == cardsToRender.Count)
            {
                for (int num = 0; num < rowOfCards.Count; num++)
                {
                    RenderCardNum(cardNumber, (bool)rowOfCards[num][1], (bool)rowOfCards[num][2]);
                    cardNumber++;
                }
                foreach (ArrayList al in rowOfCards)
                {
                    RenderCardTop((Card)al[0], (bool)al[1], (bool)al[2]);
                }
                foreach (ArrayList al in rowOfCards)
                {
                    RenderCardMiddle((Card)al[0], (bool)al[1], (bool)al[2]);
                }
                foreach (ArrayList al in rowOfCards)
                {
                    RenderCardBottom((Card)al[0], (bool)al[1], (bool)al[2]);
                }
                Console.WriteLine();
                rowOfCards.Clear();
            }
        }






    }

    private static void renderStack(Card topCard)
    {
        Console.WriteLine("STACK: ");
        RenderCardTop(topCard, false, true);
        RenderCardMiddle(topCard, false, true);
        RenderCardBottom(topCard, false, true);

    }
    /* public static void renderStatus(Card topCard, SequenceOfPlay turn)
    {
        /// renders the current status of the game at each beginning of a new turn.
        /// Could be:
        ///     Who's turn it is
        ///     cards players etc.
        ///     

        Console.Clear();
        Console.WriteLine("PLAYERS: ");
        foreach (Player player in turn.sequence)
        {
            if (turn.current == player)
            {
                Console.WriteLine($"-> {player.name}");
            }
            else
            {
                Console.WriteLine($"   {player.name}");
            }
        }

        renderStack(topCard);

        Console.WriteLine("PLAYER HAND: ");
        // turn.current.ShowHand();
        RenderPlayerHand(turn.current.Hand.cards);


    } */

}

