using UnityEngine;

public class ReverseTimerEffect : TimerEffect
{
    public override void Activate(BaseCountdownTimer timer)
    {
        // TODO
    }

    public override void Deactivate(BaseCountdownTimer timer)
    {
        // TODO
    }

    public override void OnCountdownChanged(BaseCountdownTimer timer)
    {
        // TODO
    }
}

public class ReverseTimerEffectGenerator : TimerEffectGenerator
{
    public float probability = 0.2f;

    public override TimerEffect Generate()
    {
        return new ReverseTimerEffect();
    }

    public override bool ShouldGenerate()
    {
        return Random.value < probability;
    }
}
