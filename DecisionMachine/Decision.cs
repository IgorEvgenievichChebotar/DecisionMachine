namespace DecisionMachine;

public abstract class Decision
{
    public bool Result;
    public abstract void Evaluate(IEntity entity);
}