using UnityEngine;

public class SlowDownTimerEffect : TimerEffect
{
    private enum Phase
    {
        PreSlowDown,
        OnSlowDown,
        PostSlowDown
    }

    private Phase phase = Phase.PreSlowDown;

    public CountdownValue whereToStart;
    public CountdownValue whereToStartFractions;
    public int durationTicks;

    public override void Activate()
    {
        // NOP
    }

    public override void Deactivate()
    {
        EndSlowDown();
    }

    public override void OnCountdownChanged()
    {
        if (phase == Phase.PreSlowDown)
        {
            CountdownValue startAt = BaseCountdownTimer.Instance.FractionsEnabled() ? whereToStartFractions : whereToStart;
            if (CountdownValueUtil.Surpassed(BaseCountdownTimer.Instance.GetCountdownValue(), startAt, BaseCountdownTimer.Instance.ReverseEnabled()))
            {
                phase = Phase.OnSlowDown;
                BaseCountdownTimer.Instance.SlowDown();
            }
        }
        else if (phase == Phase.OnSlowDown)
        {
            --durationTicks;
            if (durationTicks < 0)
                EndSlowDown();
        }
    }

    private void EndSlowDown()
    {
        if (phase == Phase.OnSlowDown)
        {
            phase = Phase.PostSlowDown;
            BaseCountdownTimer.Instance.SpeedUp();
        }
    }
}

public class SlowDownTimerEffectGenerator : TimerEffectGenerator
{
    public float probability = 0.2f;
    public readonly DiscreteDistribution<CountdownValue> whereToStart = new();
    public readonly DiscreteDistribution<CountdownValue> whereToStartFractions = new();
    public readonly DiscreteDistribution<int> durationTicks = new();

    public override TimerEffect Generate()
    {
        if (Random.value < probability)
        {
            return new SlowDownTimerEffect()
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
