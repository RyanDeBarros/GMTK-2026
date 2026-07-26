using UnityEngine;

public class FractionsTimerEffect : TimerEffect
{
    // TODO instead of always going 1, 1/2, 1/3, 1/4, Go!... randomly select which fraction to actually end with?

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
