public class SpecialCard : Card
{

    public Modifier modifier;
    public new string Name
    {
        // get => $"{this.Color.ToUpper().Trim()[0]}-{this.modifier.name}";
        get => $"{modifier.name}";
    }

    public SpecialCard(ColorModifier modifier) : base("none")
    {

        this.changeColor("none");
        modifier.SetHost(this);

        this.modifier = modifier;
    }

    public SpecialCard(string color, Modifier modifier) : base(color)
    {
        this.modifier = modifier;
    }

    public override SpecialCard Play()
    {
        return this;
    }
    // when this card i played, the modifier should be activated?
    private Modifier InitMod()
    {
        if (this.modifier is ColorModifier m)
        {
            m.InitMod();

        }
        return this.modifier; // this makes no sense for ColorModifier for now....
    }

    public override bool ValidNext(RegCard card)
    {
        if (this.Color == card.Color)
        {
            return true;
        }
        return false;
    }
    public override bool ValidNext(SpecialCard card)
    {
        if (this.Color == card.Color ||
            card.modifier.GetType() == modifier.GetType() ||
            card.modifier is ColorModifier
        )
        {
            return true;
        }
        return false;
    }




}


