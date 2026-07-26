using System;
using UnityEngine;

[Serializable]
public class CountdownChoiceJson
{
    public string value;
    public float weight;

    public CountdownValue ToCountdownValue()
    {
        return CountdownValueUtil.FromString(value);
    }
}

[Serializable]
public class IntChoiceJson
{
    public int value;
    public float weight;
}

[Serializable]
public class SpeedUpTimerEffectJson
{
    public float probability;
    public CountdownChoiceJson[] whereToStart;
    public CountdownChoiceJson[] whereToStartFractions;
    public IntChoiceJson[] durationTicks;

    public void AddGenerator(TimerEffectQueue timerEffectQueue)
    {
        SpeedUpTimerEffectGenerator speedUp = new()
        {
            probability = probability
        };

        foreach (var whereToStart in whereToStart)
            speedUp.whereToStart.AddChoice(whereToStart.ToCountdownValue(), whereToStart.weight);

        foreach (var whereToStartFractions in whereToStartFractions)
            speedUp.whereToStartFractions.AddChoice(whereToStartFractions.ToCountdownValue(), whereToStartFractions.weight);

        foreach (var durationTicks in durationTicks)
            speedUp.durationTicks.AddChoice(durationTicks.value, durationTicks.weight);

        timerEffectQueue.generators.Add(speedUp);
    }
}

[Serializable]
public class SlowDownTimerEffectJson
{
    public float probability;
    public CountdownChoiceJson[] whereToStart;
    public CountdownChoiceJson[] whereToStartFractions;
    public IntChoiceJson[] durationTicks;

    public void AddGenerator(TimerEffectQueue timerEffectQueue)
    {
        SlowDownTimerEffectGenerator slowDown = new()
        {
            probability = probability
        };

        foreach (var whereToStart in whereToStart)
            slowDown.whereToStart.AddChoice(whereToStart.ToCountdownValue(), whereToStart.weight);

        foreach (var whereToStartFractions in whereToStartFractions)
            slowDown.whereToStartFractions.AddChoice(whereToStartFractions.ToCountdownValue(), whereToStartFractions.weight);

        foreach (var durationTicks in durationTicks)
            slowDown.durationTicks.AddChoice(durationTicks.value, durationTicks.weight);

        timerEffectQueue.generators.Add(slowDown);
    }
}

[Serializable]
public class ReverseTimerEffectJson
{
    public float probability;
    public CountdownChoiceJson[] whereToStart;
    public CountdownChoiceJson[] whereToStartFractions;
    public CountdownChoiceJson[] whereToEnd;
    public CountdownChoiceJson[] whereToEndFractions;

    public void AddGenerator(TimerEffectQueue timerEffectQueue)
    {
        ReverseTimerEffectGenerator reverse = new()
        {
            probability = probability
        };

        foreach (var whereToStart in whereToStart)
            reverse.whereToStart.AddChoice(whereToStart.ToCountdownValue(), whereToStart.weight);

        foreach (var whereToStartFractions in whereToStartFractions)
            reverse.whereToStartFractions.AddChoice(whereToStartFractions.ToCountdownValue(), whereToStartFractions.weight);

        foreach (var whereToEnd in whereToEnd)
            reverse.whereToEnd.AddChoice(whereToEnd.ToCountdownValue(), whereToEnd.weight);

        foreach (var whereToEndFractions in whereToEndFractions)
            reverse.whereToEndFractions.AddChoice(whereToEndFractions.ToCountdownValue(), whereToEndFractions.weight);

        timerEffectQueue.generators.Add(reverse);
    }
}

[Serializable]
public class FractionsTimerEffectJson
{
    public float probability;

    public void AddGenerator(TimerEffectQueue timerEffectQueue)
    {
        FractionsTimerEffectGenerator fractions = new()
        {
            probability = probability
        };

        timerEffectQueue.generators.Add(fractions);
    }
}

[Serializable]
public class FakeoutTimerEffectJson
{
    public float probability;
    public CountdownChoiceJson[] jumpTo;

    public void AddGenerator(TimerEffectQueue timerEffectQueue)
    {
        FakeoutTimerEffectGenerator fakeout = new()
        {
            probability = probability
        };

        foreach (var jumpTo in jumpTo)
            fakeout.jumpTo.AddChoice(jumpTo.ToCountdownValue(), jumpTo.weight);

        timerEffectQueue.generators.Add(fakeout);
    }
}

[Serializable]
public class TimerEffectsJson
{
    public SpeedUpTimerEffectJson[] speedUp;
    public SlowDownTimerEffectJson[] slowDown;
    public ReverseTimerEffectJson[] reverse;
    public FractionsTimerEffectJson[] fractions;
    public FakeoutTimerEffectJson[] fakeout;

    public void Configure(TimerEffectQueue timerEffectQueue)
    {
        foreach (var timerEffect in speedUp)
            timerEffect.AddGenerator(timerEffectQueue);

        foreach (var timerEffect in slowDown)
            timerEffect.AddGenerator(timerEffectQueue);

        foreach (var timerEffect in reverse)
            timerEffect.AddGenerator(timerEffectQueue);

        foreach (var timerEffect in fractions)
            timerEffect.AddGenerator(timerEffectQueue);

        foreach (var timerEffect in fakeout)
            timerEffect.AddGenerator(timerEffectQueue);
    }
}

public static class TimerEffectConfigLoader
{
    public static TimerEffectsJson Load(TextAsset asset)
    {
        return JsonUtility.FromJson<TimerEffectsJson>(asset.text);
    }
}
