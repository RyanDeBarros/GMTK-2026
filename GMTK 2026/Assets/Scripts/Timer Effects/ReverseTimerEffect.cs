using UnityEngine;

public class ReverseTimerEffect : TimerEffect
{
    public CountdownValue whereToStart;
    public CountdownValue whereToStartFractions;
    public CountdownValue whereToEnd;
    public CountdownValue whereToEndFractions;

    private enum Phase
    {
        PreReverse,
        OnReverse,
        PostReverse
    }

    private Phase phase = Phase.PreReverse;

	public override void Activate()
    {
        BaseCountdownTimer.Instance.SetReverse(false);
    }

    public override void Deactivate()
    {
        BaseCountdownTimer.Instance.SetReverse(false);
    }

    public override void OnCountdownChanged()
    {
        if (phase == Phase.PreReverse)
        {
            if (BaseCountdownTimer.Instance.FractionsEnabled() ?
                CountdownValueUtil.AtMost(BaseCountdownTimer.Instance.GetCountdownValue(), whereToStartFractions) :
                CountdownValueUtil.AtMost(BaseCountdownTimer.Instance.GetCountdownValue(), whereToStart))
            {
                phase = Phase.OnReverse;
                BaseCountdownTimer.Instance.SetReverse(true);
            }
        }
        else if (phase == Phase.OnReverse)
        {
            if (BaseCountdownTimer.Instance.FractionsEnabled() ?
                CountdownValueUtil.AtLeast(BaseCountdownTimer.Instance.GetCountdownValue(), whereToEndFractions) :
                CountdownValueUtil.AtLeast(BaseCountdownTimer.Instance.GetCountdownValue(), whereToEnd))
            {
                phase = Phase.PostReverse;
                BaseCountdownTimer.Instance.SetReverse(false);
                BaseCountdownTimer.Instance.NewPass();
            }
        }
	}
}

public class ReverseTimerEffectGenerator : TimerEffectGenerator
{
    public float probability = 0.2f;
    public readonly DiscreteDistribution<CountdownValue> whereToStart = new();
    public readonly DiscreteDistribution<CountdownValue> whereToStartFractions = new();
    public readonly DiscreteDistribution<CountdownValue> whereToEnd = new();
    public readonly DiscreteDistribution<CountdownValue> whereToEndFractions = new();

    public override TimerEffect Generate()
    {
        if (Random.value < probability)
        {
            ReverseTimerEffect effect = new()
            {
                whereToStart = whereToStart.Poll(),
                whereToStartFractions = whereToStartFractions.Poll(),
                whereToEnd = whereToEnd.Poll(),
                whereToEndFractions = whereToEndFractions.Poll()
            };

            if (CountdownValueUtil.LessThan(effect.whereToStart, effect.whereToEnd) &&
                    CountdownValueUtil.LessThan(effect.whereToStartFractions, effect.whereToEndFractions))
                return effect;
        }

        return null;
    }
}
