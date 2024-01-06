public class RegCard : Card
{
    public readonly int number;


    public new string Name
    {
        // get => $"{this.Color.ToUpper().Trim()[0]}-{number}";
        get => $"{number}";
    }

    public RegCard(string color, int number) : base(color)
    {
        this.number = number;
    }

    public override RegCard Play()
    {
        return this;
    }
    public override bool ValidNext(RegCard card)
    {
        if (this.Color == card.Color || this.number == card.number)
        {
            return true;
        }
        return false;
    }
    public override bool ValidNext(SpecialCard card)
    {
        if (this.Color == card.Color || card.modifier is ColorModifier) // only skip color validation if a color modifiercard
        {
            return true;
        }
        return false;
    }


}