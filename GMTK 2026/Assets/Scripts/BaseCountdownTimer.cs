using System;
using UnityEngine;

public class BaseCountdownTimer : MonoBehaviour
{
    private static BaseCountdownTimer instance;
    public static BaseCountdownTimer Instance => instance;

    public static readonly float slowBPM = 35f;
    public static readonly float normalBPM = 70f;
    public static readonly float fastBPM = 140f;

    private enum BPMRate
    {
        Slow,
        Normal,
        Fast
    }

    private readonly ModifiableValue<CountdownValue> countdownValue = new();
    private float timerDebt = 0f;

    public class Settings
    {
        public bool fractions = false;
        public bool reverse = false;
        public int bpmIndex = (int)BPMRate.Normal;
    }

    private readonly Settings settings = new();

    private readonly TimerEffectQueue timerEffectQueue = new();

    void Start()
    {
        countdownValue.Value = CountdownValue.Zero;
        countdownValue.Consume();

        // TODO setup timer effect generator parameters
        timerEffectQueue.generators.Add(new SpeedUpTimerEffectGenerator());
        timerEffectQueue.generators.Add(new SlowDownTimerEffectGenerator());
        timerEffectQueue.generators.Add(new ReverseTimerEffectGenerator());
        timerEffectQueue.generators.Add(new FractionTimerEffectGenerator());
        timerEffectQueue.generators.Add(new FakeoutTimerEffectGenerator());

        Restart();
    }

    void Update()
    {
        timerDebt += Time.deltaTime * CurrentBPM() / 60f;
        while (timerDebt >= 1f)
        {
            --timerDebt;
            countdownValue.Value = CountdownValueUtil.Next(countdownValue.Value, settings.fractions, settings.reverse);
        }

        if (countdownValue.Modified())
        {
            timerEffectQueue.OnCountdownChanged();

            // TODO sfx

            if (countdownValue.Value == CountdownValue.Zero)
                timerEffectQueue.Deactivate();

            // TODO update game state when value reaches 0
        }

        countdownValue.Consume();

        CountdownDisplay.Instance.SetCountdownValue(countdownValue.Value);
    }

    private void OnEnable()
    {
        instance = this;
    }

    private void OnDisable()
    {
        instance = null;
    }

    public void Restart()
    {
        timerDebt = 0f;
        countdownValue.Value = CountdownValue.Ten;
        countdownValue.Consume();
        timerEffectQueue.Deactivate();
        timerEffectQueue.Activate();
    }

    public void NewPass()
    {
        timerEffectQueue.Deactivate();
        timerEffectQueue.Activate();
    }

    public void SpeedUp()
    {
        ++settings.bpmIndex;
    }

    public void SlowDown()
    {
        --settings.bpmIndex;
    }

    public void SetFractions(bool fractions)
    {
        settings.fractions = fractions;
    }

    public void SetReverse(bool reverse)
    {
        settings.reverse = reverse;
    }

    public float CurrentBPM()
    {
        return ((BPMRate)Math.Clamp(settings.bpmIndex, (int)BPMRate.Slow, (int)BPMRate.Fast)) switch {
            BPMRate.Slow => slowBPM,
            BPMRate.Normal => normalBPM,
            BPMRate.Fast => fastBPM,
            _ => throw new NotImplementedException()
        };
    }
}
