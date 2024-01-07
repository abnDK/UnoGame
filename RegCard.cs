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


}