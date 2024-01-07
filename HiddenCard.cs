// Class used for hiding a players hand of cards. 
public class HiddenCard : Card
{
    public new string Name
    {
        get => "UNO";
    }

    public HiddenCard(string color) : base(color) { }
}