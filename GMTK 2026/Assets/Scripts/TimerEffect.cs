using System.Collections.Generic;

public abstract class TimerEffect
{
    public abstract void Activate();
    public abstract void Deactivate();
    public abstract void OnCountdownChanged();
}

public abstract class TimerEffectGenerator
{
    public abstract bool ShouldGenerate();
    public abstract TimerEffect Generate();
}

public class TimerEffectQueue
{
    public readonly List<TimerEffectGenerator> generators = new();

    private readonly List<TimerEffect> effects = new();

    public void Activate()
    {
        generators.ForEach(g => {
            if (g.ShouldGenerate())
            {
                TimerEffect e = g.Generate();
                e.Activate();
                effects.Add(e);
            }
        });
    }

    public void Deactivate()
    {
        effects.ForEach(e => e.Deactivate());
        effects.Clear();
    }

    public void OnCountdownChanged()
    {
        effects.ForEach(e => e.OnCountdownChanged());
    }
}
