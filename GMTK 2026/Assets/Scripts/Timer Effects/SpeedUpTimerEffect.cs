using UnityEngine;

public class SpeedUpTimerEffect : TimerEffect
{
    private enum Phase
    {
        PreSpeedUp,
        OnSpeedUp,
        PostSpeedUp
    }

    private Phase phase = Phase.PreSpeedUp;

    public CountdownValue whereToStart;
    public CountdownValue whereToStartFractions;
    public int durationTicks;

    public override void Activate()
    {
        // NOP
    }

    public override void Deactivate()
    {
        EndSpeedUp();
    }

    public override void OnCountdownChanged()
    {
        if (phase == Phase.PreSpeedUp)
        {
            CountdownValue startAt = BaseCountdownTimer.Instance.FractionsEnabled() ? whereToStartFractions : whereToStart;
            if (CountdownValueUtil.Surpassed(BaseCountdownTimer.Instance.GetCountdownValue(), startAt, BaseCountdownTimer.Instance.ReverseEnabled()))
            {
                phase = Phase.OnSpeedUp;
                BaseCountdownTimer.Instance.SpeedUp();
            }
        }
        else if (phase == Phase.OnSpeedUp)
        {
            --durationTicks;
            if (durationTicks < 0)
                EndSpeedUp();
        }
    }

    private void EndSpeedUp()
    {
        if (phase == Phase.OnSpeedUp)
        {
            phase = Phase.PostSpeedUp;
            BaseCountdownTimer.Instance.SlowDown();
        }
    }
}

public class SpeedUpTimerEffectGenerator : TimerEffectGenerator
{
    public float probability = 0.2f;
    public readonly DiscreteDistribution<CountdownValue> whereToStart = new();
    public readonly DiscreteDistribution<CountdownValue> whereToStartFractions = new();
    public readonly DiscreteDistribution<int> durationTicks = new();

    public override TimerEffect Generate()
    {
        if (Random.value < probability)
        {
            return new SpeedUpTimerEffect()
            {
                whereToStart = whereToStart.Poll(),
                whereToStartFractions = whereToStartFractions.Poll(),
                durationTicks = durationTicks.Poll()
            };
        }
        else
            return null;
    }
}
