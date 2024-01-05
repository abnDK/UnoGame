using System.Reflection;
using System.Reflection.Metadata.Ecma335;
using Spectre.Console;

public class SpectreRender : IRender
{
    private Layout BaseLayout()
    {
        var baseLayout = new Layout("Base")
            .SplitRows(
                new Layout("Header"),
                new Layout("Content")
            );

        baseLayout["Header"].Update(Align.Center(new FigletText("ESTHER UNO")));
        baseLayout["Header"].Size(8);

        return baseLayout;
    }



    public void Splash()
    {

        AnsiConsole.Write(
            new FigletText("Velkommen")
                .Centered()
                .Color(Color.Red));

        AnsiConsole.Write(
            new FigletText("Til")
                .Centered()
                .Color(Color.Green));

        AnsiConsole.Write(
            new FigletText("ESTHER UNO")
                .Centered()
                .Color(Color.Yellow));

        Console.ReadKey();
    }


    public string Menu()
    {
        var baseLayout = BaseLayout();

        baseLayout["Content"].SplitColumns(
            new Layout("Menu"),
            new Layout(Text.Empty)
        );

        var panelA = new Panel("SPIL UNO!");
        panelA.Header("# 1 #");
        panelA.Border = BoxBorder.Rounded;
        panelA.Width = 20;

        var panelB = new Panel("SE SCOREBOARD");
        panelB.Header("# 2 #");
        panelB.Border = BoxBorder.Rounded;
        panelB.Width = 20;


        var grid = new Grid();

        grid.AddColumn();

        grid.AddRow(panelA);
        grid.AddRow(panelB);


        baseLayout["Menu"].Update(Align.Center(grid, VerticalAlignment.Middle));

        AnsiConsole.Write(baseLayout);

        string? readResult;
        readResult = AnsiConsole.Ask<string>("Hvad skal vi nu?");



        if (readResult == "1")
        {
            return "play";
        }

        if (readResult == "2")
        {
            return "scoreboard";
        }

        return "";

    }
    public string[] Setup()
    {
        bool validInput = false;

        List<string> playerNames = new List<string>();


        while (!validInput)
        {
            string readResult = AnsiConsole.Ask<string>("Hvem skal være med til UNO?");

            string[] playerNamesSplit = readResult.Split(",");

            foreach (string name in playerNamesSplit)
                playerNames.Add(name.Trim());

            if (playerNames.Count > 1)
                validInput = true;

        }

        return playerNames.ToArray();





    }
    public void Turn(Game game)
    {

        Layout baseLayout = BaseLayout();

        baseLayout["Content"].SplitColumns(
            new Layout("Player"),
            new Layout("Stack")
        );

        baseLayout["Player"].SplitRows(
            new Layout("PlayerName"),
            new Layout("Hand")
        );

        // RENDER PLAYER NAME
        Grid playerGrid = new Grid();
        playerGrid.AddColumn();
        foreach (Player player in game.players)
        {
            Panel playerPanel = new Panel(String.Empty);

            if (player.name == game.turn.current.name)
            {
                playerPanel = new Panel(
                    new Text($">>> {player.name}",
                    new Style(Color.Red, null, Decoration.Bold)
                ));
            }
            else
            {
                playerPanel = new Panel(
                    new Text($"    {player.name}",
                    new Style(Color.White, null, null)
                ));
            }

            playerPanel.Border = BoxBorder.None;

            playerGrid.AddRow(playerPanel);

        }



        // playerPanel.Border = BoxBorder.None;

        baseLayout["PlayerName"].Update(playerGrid);

        // RENDER HAND
        List<Panel> cards = new List<Panel>();
        int cardNum = 1;

        foreach (Card card in game.turn.current.Hand.cards)
        {

            cards.Add(RenderSmallCard(card, cardNum));
            cardNum++;
        }

        Grid cardGrid = new Grid();

        int COLUMNS = 3;

        cardGrid.AddColumns(COLUMNS);

        List<Panel> cardsOnRow = new List<Panel>();

        for (int i = 0; i < cards.Count; i++)
        {
            cardsOnRow.Add(cards[i]);

            AnsiConsole.WriteLine(cards[i].Header.Text);
            AnsiConsole.WriteLine(cardsOnRow.Count);

            if ((i + 1) % COLUMNS == 0 || (i + 1) == cards.Count)
            {
                cardGrid.AddRow(cardsOnRow.ToArray());

                cardsOnRow.Clear();
            }
        }

        baseLayout["Hand"].Update(Align.Center(cardGrid, VerticalAlignment.Middle));


        // RENDER STACK

        // TODO: MAKE HELPER FUNCTIONS: SMALL CARD (AS WE RENDER NOW) and LARGE CARD (WITH FIGLET)


        // - should show top card (highlighted - larger or somehow..?)
        // - should show prev 2 or 3 cards played as well..

        Grid stack = new Grid();
        stack.AddColumns(1);
        stack.AddRow(
            Align.Center(
                RenderLargeCardPanel(
                    game._dealer.PeekPlayedTopCard(),
                    Justify.Center
                )
            )
        );

        if (game._dealer.PlayedCardsCount() > 1)
        {
            stack.AddRow(Align.Center(RenderSmallCard(game._dealer.PeekPlayedTopCard(1), null)));
        }
        if (game._dealer.PlayedCardsCount() > 2)
        {
            stack.AddRow(Align.Center(RenderSmallCard(game._dealer.PeekPlayedTopCard(2), null)));
        }
        if (game._dealer.PlayedCardsCount() > 3)
        {
            stack.AddRow(Align.Center(RenderSmallCard(game._dealer.PeekPlayedTopCard(3), null)));
        }
        if (game._dealer.PlayedCardsCount() > 4)
        {
            stack.AddRow(Align.Center(RenderSmallCard(game._dealer.PeekPlayedTopCard(4), null)));
        }



        baseLayout["Stack"].Update(
            new Align(stack.RightAligned(), HorizontalAlignment.Center, VerticalAlignment.Middle)
        );



        AnsiConsole.Write(baseLayout);

    }

    public void Result(string nameOfWinner)
    {

        FigletText winnerText = new FigletText($"{nameOfWinner.ToUpper()} VINDER!!!!").Justify(Justify.Center);

        AnsiConsole.Write(winnerText);

        Console.ReadKey();

    }

    public bool Replay() { return false; }

    public void Scoreboard() { AnsiConsole.Write("SCOREBOARD UPCOMING"); }


    private Color ResolveColor(string color)
    {
        Color colorReturn = Color.White;

        switch (color)
        {

            case "blue":
                colorReturn = Color.Blue;
                break;
            case "yellow":
                colorReturn = Color.Yellow;
                break;
            case "green":
                colorReturn = Color.Green;
                break;
            case "red":
                colorReturn = Color.Red;
                break;
            default:
                break;


        }
        return colorReturn;
    }

    private FigletText RenderLargeCardFiglet(Card card, Justify? justification = null)
    {
        if (card is RegCard Reg)
        {
            return new FigletText(Reg.Name).Color(ResolveColor(Reg.Color)).Justify(justification);
        }
        else if (card is SpecialCard Spec)
        {
            return new FigletText(Spec.Name).Color(ResolveColor(Spec.Color)).Justify(justification);
        }
        else
        {
            throw new Exception("Card was not recognized as either RegularCard or SpecialCard.");
        }

    }

    private Panel RenderLargeCardPanel(Card card, Justify? justification = null)
    {

        FigletText largeNumber;
        Text smallNumber;

        if (card is RegCard Reg)
        {
            largeNumber = new FigletText(Reg.Name).Color(ResolveColor(Reg.Color)).Justify(justification);
            smallNumber = new Text($"\n{Reg.Name}\n", new Style(ResolveColor(Reg.Color)));
        }
        else if (card is SpecialCard Spec)
        {
            largeNumber = new FigletText(Spec.Name[0].ToString()).Color(ResolveColor(Spec.Color)).Justify(justification);
            smallNumber = new Text($"\n{Spec.Name[0].ToString()}\n", new Style(ResolveColor(Spec.Color)));
        }
        else
        {
            throw new Exception("Card was not recognized as either RegularCard or SpecialCard.");
        }

        Grid largeCardGrid = new Grid();
        largeCardGrid.Alignment(Justify.Right);
        largeCardGrid.AddColumns(3).Width(15);

        largeCardGrid.AddRow(new Text[] {
            smallNumber,
            new Text(String.Empty),
            new Text(String.Empty)
        });

        largeCardGrid.AddRow(
            new Text(String.Empty),
            largeNumber.Centered(),
            new Text(String.Empty)
        );

        largeCardGrid.AddRow(new Text[] {
            new Text(String.Empty),
            new Text(String.Empty),
            smallNumber
        });

        Panel gridInPanel = new Panel(largeCardGrid);
        gridInPanel.Border = BoxBorder.Double;
        gridInPanel.BorderColor(ResolveColor(card.Color));
        return gridInPanel;

    }

    private Panel RenderSmallCard(Card card, int? header)
    {

        Panel panelCard;

        if (card is RegCard r)
        {
            panelCard = new Panel(
                new Text(
                    $"\n{r.Name}\n",
                    new Style(ResolveColor(r.Color), null, Decoration.Bold)
                ).Centered()
            );



        }

        else if (card is SpecialCard s)
        {
            panelCard = new Panel(
                            new Text(
                                $"\n{s.Name}\n",
                                new Style(ResolveColor(s.Color), null, Decoration.Bold)
                            ).Centered()
                        );

        }
        else
        {
            throw new Exception("Card was not recognized as either RegularCard or SpecialCard.");
        }

        if (header != null)
            panelCard.Header($"# {header} #");


        panelCard.BorderColor(ResolveColor(card.Color));
        panelCard.Border = BoxBorder.Rounded;
        panelCard.Width = 9;
        // panelCard.Height = 5;
        // panelCard.PadBottom(5);
        // panelCard.PadTop(5);

        // panelCard.Padding(new Padding(2));

        return panelCard;
    }
}