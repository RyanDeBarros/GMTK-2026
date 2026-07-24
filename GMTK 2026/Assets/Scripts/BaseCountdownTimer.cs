using System;
using UnityEngine;

public class BaseCountdownTimer : MonoBehaviour
{
    public static readonly float normalBPM = 70f;
    public static readonly float fastBPM = 140f;

    private enum BPMRate
    {
        Normal,
        Fast
    }

    private CountdownValue countdownValue = CountdownValue.Zero;
    private float timerDebt = 0f;

    public class Settings
    {
        public bool fractions = false;
        public bool reverse = false;
        public int bpmIndex = 0;
    }

    private readonly Settings settings = new();

    void Start()
    {
        Restart();
    }

    void Update()
    {
        timerDebt += Time.deltaTime * CurrentBPM() / 60f;
        bool changed = false;
        while (timerDebt >= 1f)
        {
            --timerDebt;
            CountdownValue oldCountdownValue = countdownValue;
            countdownValue = CountdownValueUtil.Next(countdownValue, settings.fractions, settings.reverse);
            changed |= oldCountdownValue != countdownValue;
        }

        if (changed)
            OnCountdownValueChanged();
    }

    public void Restart()
    {
        timerDebt = 0f;
        countdownValue = CountdownValue.Ten;
        OnCountdownValueChanged();
    }

    public void SpeedUp()
    {
        ++settings.bpmIndex;
    }

    public void SlowDown()
    {
        --settings.bpmIndex;
    }

    public void ToggleFractions()
    {
        settings.fractions = !settings.fractions;
    }

    public void ToggleReverse()
    {
        settings.reverse = !settings.reverse;
    }

    public float CurrentBPM()
    {
        return ((BPMRate)Math.Clamp(settings.bpmIndex, (int)BPMRate.Normal, (int)BPMRate.Fast)) switch {
            BPMRate.Normal => normalBPM,
            BPMRate.Fast => fastBPM,
            _ => throw new NotImplementedException()
        };
    }

    private void OnCountdownValueChanged()
    {
        CountdownDisplay.Instance.SetCountdownValue(countdownValue);
        // TODO sfx
    }
}
