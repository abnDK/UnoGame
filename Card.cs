public class Card
{
    private string _color;

    public string Color
    {
        get => _color;

    }
    public string Name
    {
        get => $"{this.Color.ToUpper().Trim()[0]}";

    }

    public Card(string color)
    {
        this._color = color.ToLower().Trim();
    }



    public virtual Card Play()
    {
        System.Console.WriteLine($"Playing {this.Name}");
        return this;
    }



    public void changeColor(string color)
    {
        switch (color)
        {
            case "red":
                this._color = color;
                break;
            case "yellow":
                this._color = color;
                break;
            case "blue":
                this._color = color;
                break;
            case "green":
                this._color = color;
                break;
            default:
                break;
        }


    }

    public int CompareTo(Card compareCard)
    {
        // if reg: compare on color
        // if reg a, spec b: return a
        // if spec a, reg b: return b
        // if spec a, spec b: compare on modifier type name

        if (compareCard == null)
        {
            return 1;
        }

        else
        {
            if (this is RegCard thisReg)
            {
                if (compareCard is RegCard compareReg)
                {
                    return thisReg.number.CompareTo(compareReg.number);
                }
                else if (compareCard is SpecialCard)
                {
                    // regular card prioritized over specialcards
                    return -1;
                }
            }
            else if (this is SpecialCard thisSpec)
            {
                if (compareCard is RegCard)
                {
                    // regular card prioritized over specialcards
                    return 1;
                }
                else if (compareCard is SpecialCard compareSpec)
                {

                    return thisSpec.modifier.GetType().ToString().CompareTo(compareSpec.modifier.GetType().ToString());
                }
            }
            return 0;

        }


    }
}
