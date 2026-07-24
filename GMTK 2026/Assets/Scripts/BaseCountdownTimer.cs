using System;
using UnityEngine;

public class BaseCountdownTimer : MonoBehaviour
{
    public static readonly int maxIndex = 10;

    public static readonly float normalBPM = 70f;
    public static readonly float fastBPM = 140f;

    private float timer = 0;

    private enum BPMRate
    {
        Normal,
        Fast
    }

    private int currentBPMIndex = 0;

    void Start()
    {
        SetTimer(maxIndex);
    }

    void Update()
    {
        SetTimer(timer - Time.deltaTime * CurrentBPM() / 60f);
    }

    private void SetTimer(float tm)
    {
        int oldCountdownValue = CurrentCountdownValue();
        timer = Mathf.Clamp(tm, 0f, maxIndex);
        if (CurrentCountdownValue() != oldCountdownValue)
            OnCountdownValueChanged();
    }

    public void SpeedUp()
    {
        ++currentBPMIndex;
    }

    public void SlowDown()
    {
        --currentBPMIndex;
    }

    public float CurrentBPM()
    {
        return ((BPMRate)Math.Clamp(currentBPMIndex, (int)BPMRate.Normal, (int)BPMRate.Fast)) switch {
            BPMRate.Normal => normalBPM,
            BPMRate.Fast => fastBPM,
            _ => throw new NotImplementedException()
        };
    }

    public int CurrentCountdownValue()
    {
        return Mathf.CeilToInt(timer);
    }

    private void OnCountdownValueChanged()
    {
        CountdownDisplay.Instance.SetCountdownValue(CurrentCountdownValue());
        // TODO sfx
    }
}
