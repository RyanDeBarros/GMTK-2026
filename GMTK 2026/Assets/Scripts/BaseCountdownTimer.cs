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
    private bool newPass = false;

    [Header("Timer Effect Parameters")]
    [SerializeField] private float speedUpProbability = 0.2f;
    [SerializeField] private float slowDownProbability = 0.2f;
    [SerializeField] private float fakeoutProbability = 0.2f;
    [SerializeField] private float fractionsProbability = 0.2f;
    [SerializeField] private float reverseProbability = 0.2f;

    void Start()
    {
        countdownValue.Value = CountdownValue.Zero;
        countdownValue.Consume();

        FillTimerEffectQueue();

        Restart();
    }

    void Update()
    {
        if (newPass)
        {
            newPass = false;
            timerEffectQueue.Deactivate();
            timerEffectQueue.Activate();
        }

        timerDebt += Time.deltaTime * CurrentBPM() / 60f;
        while (timerDebt >= 1f)
        {
            --timerDebt;
            countdownValue.Value = CountdownValueUtil.Next(GetCountdownValue(), settings.fractions, settings.reverse);
        }

        if (countdownValue.Modified())
        {
            timerEffectQueue.OnCountdownChanged();

            // TODO sfx

            if (GetCountdownValue() == CountdownValue.Zero)
            {
                timerEffectQueue.Deactivate();
                // TODO update game state when value reaches 0
            }
        }

        countdownValue.Consume();

        CountdownDisplay.Instance.SetCountdownValue(GetCountdownValue());
    }

    private void OnEnable()
    {
        instance = this;
    }

    private void OnDisable()
    {
        instance = null;
    }

    private void FillTimerEffectQueue()
    {
        SpeedUpTimerEffectGenerator speedUp = new()
        {
            probability = speedUpProbability
        };
        timerEffectQueue.generators.Add(speedUp);

        SlowDownTimerEffectGenerator slowDown = new() 
        {
            probability = slowDownProbability
        };
        timerEffectQueue.generators.Add(slowDown);

        ReverseTimerEffectGenerator reverse = new()
        {
            probability = reverseProbability
        };
        reverse.whereToStart.
            AddChoice(CountdownValue.One, 5f).
            AddChoice(CountdownValue.Two, 3f).
            AddChoice(CountdownValue.Three, 1f);
        reverse.whereToStartFractions.
            AddChoice(CountdownValue.One, 3f).
            AddChoice(CountdownValue.OneHalf, 1f).
            AddChoice(CountdownValue.OneThird, 1f).
            AddChoice(CountdownValue.OneFourth, 3f);
        reverse.whereToEnd.
            AddChoice(CountdownValue.Four, 5f).
            AddChoice(CountdownValue.Five, 3f).
            AddChoice(CountdownValue.Six, 1f);
        reverse.whereToEndFractions.
            AddChoice(CountdownValue.Two, 1f).
            AddChoice(CountdownValue.Three, 1f).
            AddChoice(CountdownValue.Four, 1f);
        timerEffectQueue.generators.Add(reverse);

        FractionTimerEffectGenerator fractions = new()
        {
            probability = fractionsProbability
        };
        timerEffectQueue.generators.Add(fractions);

        FakeoutTimerEffectGenerator fakeout = new()
        {
            probability = fakeoutProbability
        };
        timerEffectQueue.generators.Add(fakeout);
    }

    public void Restart()
    {
        timerDebt = 0f;
        countdownValue.Value = CountdownValue.Ten;
        countdownValue.Consume();
        NewPass();
    }

    public void NewPass()
    {
        newPass = true;
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

    public bool FractionsEnabled()
    {
        return settings.fractions;
    }

    public void SetReverse(bool reverse)
    {
        settings.reverse = reverse;
    }

    public bool ReverseEnabled()
    {
        return settings.reverse;
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

    public CountdownValue GetCountdownValue()
    {
        return countdownValue.Value;
    }
}
