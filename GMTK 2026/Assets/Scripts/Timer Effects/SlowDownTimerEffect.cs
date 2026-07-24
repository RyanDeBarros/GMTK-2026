using UnityEngine;

public class SlowDownTimerEffect : TimerEffect
{
    public override void Activate()
    {
        // NOP
    }

    public override void Deactivate()
    {
        // TODO
    }

    public override void OnCountdownChanged()
    {
        // TODO
    }
}

public class SlowDownTimerEffectGenerator : TimerEffectGenerator
{
    public float probability = 0.2f;

    public override TimerEffect Generate()
    {
        if (Random.value < probability)
            return new SlowDownTimerEffect();
        else
            return null;
    }
}
