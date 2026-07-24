using UnityEngine;

public class SpeedUpTimerEffect : TimerEffect
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
