using UnityEngine;

public class FakeoutTimerEffect : TimerEffect
{
    public override void Activate()
    {
        // TODO
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

public class FakeoutTimerEffectGenerator : TimerEffectGenerator
{
    public float probability = 0.2f;

    public override TimerEffect Generate()
    {
        if (Random.value < probability)
            return new FakeoutTimerEffect();
        else
            return null;
    }
}
