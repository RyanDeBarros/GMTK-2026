using UnityEngine;

public class SpeedUpTimerEffect : TimerEffect
{
    public override void Activate(BaseCountdownTimer timer)
    {
        // TODO
    }

    public override void Deactivate(BaseCountdownTimer timer)
    {
        // TODO
    }

    public override bool OnCountdownChanged(BaseCountdownTimer timer)
    {
        // TODO
        return false;
    }
}

public class SpeedUpTimerEffectGenerator : TimerEffectGenerator
{
    public float probability = 0.2f;

    public override TimerEffect Generate()
    {
        return new SpeedUpTimerEffect();
    }

    public override bool ShouldGenerate()
    {
        return Random.value < probability;
    }
}
