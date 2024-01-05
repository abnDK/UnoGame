public abstract class Modifier
{
    public string name;
    public virtual void InitMod()
    {
        System.Console.WriteLine("This init's a mod");
    }
}

public class DrawModifier : Modifier
{
    public byte amount;
    public DrawModifier(byte amount)
    {
        this.name = $"+ {amount}";
        this.amount = amount;
    }
    public override void InitMod()
    {
        Console.WriteLine($"Add {this.amount} cards to draw hands variable.");
    }
}

public class SkipSequenceModifier : Modifier
{
    public SkipSequenceModifier()
    {
        name = "HOP";
    }
    public override void InitMod()
    {
        Console.WriteLine("Next player will be skipped.");
    }
}

public class ReverseSequenceModifier : Modifier
{
    public ReverseSequenceModifier()
    {
        name = "<->";
    }
    public override void InitMod()
    {
        Console.WriteLine("Sequence has been reversed.");
    }
}

public class ColorModifier : Modifier
{
    private SpecialCard? host;
    public ColorModifier()
    {
        name = "???";
    }

    public void SetHost(SpecialCard s)
    {
        host = s;
    }

    public override void InitMod()
    {
        if (host == null)
            throw new Exception("No host SpecialCard known - cannot set color!");

        Console.WriteLine("Hvilken farve vil du skifte til?");
        Console.WriteLine("1) Gul");
        Console.WriteLine("2) Blå");
        Console.WriteLine("3) Grøn");
        Console.WriteLine("4) Rød");

        bool validColor = false;
        string? readResult;

        while (true)
        {
            readResult = Console.ReadLine();

            if (readResult != null)
            {
                if (readResult == "1" || readResult.Trim().ToLower() == "gul")
                {
                    host.changeColor("yellow");
                    break;
                }

                if (readResult == "2" || readResult.Trim().ToLower() == "blå")
                {
                    host.changeColor("blue");
                    break;
                }
                if (readResult == "3" || readResult.Trim().ToLower() == "grøn")
                {
                    host.changeColor("green");
                    break;
                }
                if (readResult == "4" || readResult.Trim().ToLower() == "rød")
                {
                    host.changeColor("red");
                    break;
                }
            }
        }
    }
}
