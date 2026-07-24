using UnityEngine;

public class FakeoutTimerEffect : TimerEffect
{
    public CountdownValue jumpTo;

    public override void Activate()
    {
        // NOP
    }

    public override void Deactivate()
    {
        // NOP
    }

    public override void OnCountdownChanged()
    {
        if (BaseCountdownTimer.Instance.GetCountdownValue() == CountdownValue.Zero)
        {
            BaseCountdownTimer.Instance.DirectSetCountdownValue(jumpTo);
            BaseCountdownTimer.Instance.NewPass();
        }
    }
}

public class FakeoutTimerEffectGenerator : TimerEffectGenerator
{
    public float probability = 0.2f;
    public readonly DiscreteDistribution<CountdownValue> jumpTo = new();

    public override TimerEffect Generate()
    {
        if (Random.value < probability)
        {
            return new FakeoutTimerEffect()
            {
                jumpTo = jumpTo.Poll()
            };
        }
        else
            return null;
    }
}
