using UnityEngine;

public class FractionTimerEffect : TimerEffect
{
    public override void Activate(BaseCountdownTimer timer)
    {
        timer.SetFractions(true);
    }

    public override void Deactivate(BaseCountdownTimer timer)
    {
        timer.SetFractions(false);
    }

    public override void OnCountdownChanged(BaseCountdownTimer timer)
    {
        // NOP
    }
}

public class FractionTimerEffectGenerator : TimerEffectGenerator
{
    public float probability = 0.2f;

    public override TimerEffect Generate()
    {
        return new FractionTimerEffect();
    }

    public override bool ShouldGenerate()
    {
        return Random.value < probability;
    }
}
