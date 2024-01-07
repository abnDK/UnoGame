namespace UnoGame
{
    /* 
            TODO:
            - Scoreboard functionality
            - Render Player list and emphasize current player
            - Say "UNO" before playing last card (and what happens if you forget?)
            - Add new specialcards (play from a previous card in stack, draw x card from deck and choose what to keep, play 3 rare winnercards and win everything...)
            - Add cheatcodes - get supercards...
            - Multiple screens/terminals - or at least hiding cards of opponent / ability to activate when opponent looks away
            - Make plantUML diagrams of classes
            
            
            
            // (X) MAKE SKIP TURN WORK
            // (X) MAKE REVERSE TURN WORK
            // (X) MAKE CHANGE COLOR WORK

            // (X) SORT PLAYER CARDS
            // (X) SHOW PLAYER CARDS HORISONTAL
            // --- REMOVE COMMENTS AND CLEAN UP RENDER CODE
            
            // MAKE PLANT UML DIAGRAMS

         */



    class Program
    {

        static void Main(string[] args)
        {

            Game newGame = new Game(
                                    new SpectreRender(),
                                    new Dealer(
                                        new Deck(),
                                        new SequenceOfPlay(),
                                        new HandValidator()));

            newGame.InitGame();


            /* 
                        var dealer = new Dealer(
                            new Deck(),
                            new SequenceOfPlay(),
                            new HandValidator()
                        );

                        dealer.InitPlayersTest(new string[] { "anders", "britt" });


                        dealer.DoWeHaveAWinner();
             */


        }
    }







}






