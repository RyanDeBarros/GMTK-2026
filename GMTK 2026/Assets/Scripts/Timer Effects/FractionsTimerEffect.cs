using UnityEngine;

public class FractionsTimerEffect : TimerEffect
{
    public override void Activate()
    {
        BaseCountdownTimer.Instance.SetFractions(true);
    }

    public override void Deactivate()
    {
		BaseCountdownTimer.Instance.SetFractions(false);
    }

    public override void OnCountdownChanged()
    {
        // NOP
    }
}

public class FractionsTimerEffectGenerator : TimerEffectGenerator
{
    public float probability = 0.2f;

    public override TimerEffect Generate()
    {
        if (Random.value < probability)
            return new FractionsTimerEffect();
        else
            return null;
    }
}
