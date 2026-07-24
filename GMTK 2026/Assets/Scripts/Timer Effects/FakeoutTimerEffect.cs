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
        return new FakeoutTimerEffect();
    }

    public override bool ShouldGenerate()
    {
        return Random.value < probability;
    }
}
