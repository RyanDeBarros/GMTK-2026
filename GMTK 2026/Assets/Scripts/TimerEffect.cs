using System.Collections.Generic;

public abstract class TimerEffect
{
    public abstract void Activate(BaseCountdownTimer timer);
    public abstract void Deactivate(BaseCountdownTimer timer);
    public abstract void OnCountdownChanged(BaseCountdownTimer timer);
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

    public void Activate(BaseCountdownTimer timer)
    {
        generators.ForEach(g => {
            if (g.ShouldGenerate())
            {
                TimerEffect e = g.Generate();
                e.Activate(timer);
                effects.Add(e);
            }
        });
    }

    public void Deactivate(BaseCountdownTimer timer)
    {
        effects.ForEach(e => e.Deactivate(timer));
        effects.Clear();
    }

    public void OnCountdownChanged(BaseCountdownTimer timer)
    {
        effects.ForEach(e => e.OnCountdownChanged(timer));
    }
}
