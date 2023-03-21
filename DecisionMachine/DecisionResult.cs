namespace DecisionMachine;

public class DecisionResult : Decision
{
    public bool Result { get; set; }

    public override void Evaluate(IEntity entity)
    {
        // todo to UI
        Console.WriteLine(Result);
    }
}