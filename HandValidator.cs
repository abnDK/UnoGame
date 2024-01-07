public class HandValidator
{
    public bool ValidateHand(List<Card> hand, Card topCard, bool drawDebt)
    {
        return ValidateFirstCardOfHand(topCard, hand[0], drawDebt) &&
                ValidateHandSequence(hand);
    }

    private bool ValidateFirstCardOfHand(Card topCard, Card newCard, bool drawDebt)
    {
        // if drawDebt and card.modifier not DrawCardmodifier: return false
        if (drawDebt)
        {
            if (newCard is SpecialCard sC && sC.modifier is DrawModifier)
            {
                return true;
            }
            else
            {
                Console.WriteLine("Man kan kun spille 'Træk kort op' specialkort her. Prøv igen!");
                Console.ReadLine();
                return false;
            }
        }

        // validate if new card is a valid next for top card on deck.
        if (topCard is RegCard topRegCard)
        {
            if (newCard is RegCard newRegCard)
            {
                if (!RegToRegValidation(topRegCard, newRegCard))
                    return InvalidFirstCard();
            }
            else if (newCard is SpecialCard newSpecCard)
            {
                if (!RegToSpecValidation(topRegCard, newSpecCard))
                    return InvalidFirstCard();
            }
            return true;
        }
        else if (topCard is SpecialCard topSpecCard)
        {
            if (newCard is RegCard newRegCard)
            {
                if (!SpecToRegValidation(topSpecCard, newRegCard))
                    return InvalidFirstCard();

            }
            else if (newCard is SpecialCard newSpecCard)
            {
                if (!SpecToSpecValidation(topSpecCard, newSpecCard))
                    return InvalidFirstCard();

            }
            return true;
        }
        else
        {
            throw new Exception("Ukendt kort type spillet. Spørg Anders, hvordan man fikser det!");
        }

        bool InvalidFirstCard()
        {
            Console.WriteLine("Det første kort kan ikke spilles her. Prøv igen!");
            Console.ReadLine();
            return false;
        }

    }

    private bool ValidateHandSequence(List<Card> hand)
    {

        // validate n vs n+1 for entire list of cards.
        if (hand.Count == 0)
            throw new Exception("Der er ingen kort i hånden. Hånden kan derfor ikke valideres. Spørg lige Anders, hvad der er op og ned her!");
        else if (hand.Count == 1)
            // if only one card, it is valid pr. default
            return true;
        else if (hand.Count > 1)
        {
            for (int i = 0; i < hand.Count - 1; i++)
            {
                Card current = hand[i];
                Card next = hand[i + 1];

                if (current is RegCard currentRegCard)
                {
                    if (next is RegCard nextRegCard)
                    {
                        if (currentRegCard.number != nextRegCard.number)
                        {
                            Console.WriteLine("Almindelige kort skal have samme nummer for at kunne spille dem. Prøv igen!");
                            Console.ReadLine();
                            return false;
                        }
                    }

                    if (next is SpecialCard)
                    {
                        Console.WriteLine("Man kan ikke spille et specialkort efter et almindeligt kort i samme hånd. Prøv igen!");
                        Console.ReadLine();
                        return false;
                    }
                }
                if (current is SpecialCard currentSpecialCard)
                {
                    if (currentSpecialCard.modifier is not DrawModifier)
                    {
                        Console.WriteLine("Det er kun 'Træk kort op' specialkort, der kan spilles flere af i samme hånd. Prøv igen!");
                        Console.ReadLine();
                        return false;

                    }

                    if (next is RegCard)
                    {
                        Console.WriteLine("Man kan ikke spille et almindelig kort efter et specialkort i samme hånd. Prøv igen!");
                        Console.ReadLine();
                        return false;
                    }

                    if (next is SpecialCard nextSpecialCard)
                    {
                        if (nextSpecialCard.modifier is not DrawModifier)
                        {
                            Console.WriteLine("Det er kun 'Træk kort op' specialkort, der kan spilles flere af i samme hånd. Prøv igen!");
                            Console.ReadLine();
                            return false;
                        }
                    }
                }
            }
        }
        return true;
    }

    private bool RegToRegValidation(RegCard first, RegCard second)
    {
        if (first.Color == second.Color || first.number == second.number)
        {
            return true;
        }
        return false;
    }

    private bool RegToSpecValidation(RegCard first, SpecialCard second)
    {
        if (first.Color == second.Color || second.modifier is ColorModifier) // only skip color validation if a color modifiercard
        {
            return true;
        }
        return false;
    }

    private bool SpecToRegValidation(SpecialCard first, RegCard second)
    {
        if (first.Color == second.Color)
        {
            return true;
        }
        return false;
    }

    private bool SpecToSpecValidation(SpecialCard first, SpecialCard second)
    {
        if (first.Color == second.Color ||
                    first.modifier.GetType() == second.modifier.GetType() ||
                    second.modifier is ColorModifier
                )
        {
            return true;
        }
        return false;
    }











}