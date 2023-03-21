namespace DecisionMachine;

public class DecisionQuery<T> : Decision where T : IEntity
{
    public override void Evaluate(IEntity entity)
    {
        var e = (T)entity;
        if (Test(e))
        {
            Console.WriteLine($"{Title} => positive");
            Positive.Evaluate(e);
        }
        else
        {
            Console.WriteLine($"{Title} => negative");
            Negative.Evaluate(e);
        }
    }

    public string Title { get; set; }
    public Decision Positive { get; set; }
    public Decision Negative { get; set; }
    public Func<T, bool> Test { get; set; }
}