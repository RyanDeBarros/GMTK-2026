using System;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.Events;

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

    [SerializeField] private TextAsset timerEffectParameters;

    [SerializeField] private AudioClip tickSFX;
    [SerializeField] private AudioClip goSFX;
    [SerializeField] private float maxTickPitchIncrease = 0.4f;

    private AudioSource tickAudioSource;
    private AudioSource goAudioSource;

    public UnityEvent tick;

    void Awake()
    {
        Assert.IsNull(instance);

        Assert.IsNotNull(timerEffectParameters);
        Assert.IsNotNull(goSFX);

        tickAudioSource = gameObject.AddComponent<AudioSource>();
        tickAudioSource.playOnAwake = false;
        goAudioSource = gameObject.AddComponent<AudioSource>();
        goAudioSource.playOnAwake = false;
    }

    void Start()
    {
        TimerEffectConfigLoader.Load(timerEffectParameters).Configure(timerEffectQueue);
    }

    void Update()
    {
        if (MatchManager.Instance.Phase != MatchPhase.Countdown)
            return;

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
            MatchManager.Instance.SyncCountdownMusic();
        }

        if (countdownValue.Modified())
        {
            timerEffectQueue.OnCountdownChanged();

            if (GetCountdownValue() == CountdownValue.Zero)
            {
                timerEffectQueue.Deactivate();
                MatchManager.Instance.SetPhase(MatchPhase.ChooseAction);
                goAudioSource.clip = goSFX;
                goAudioSource.Play();
            }
            else
                PlayTickSFX();

            tick?.Invoke();
        }

        countdownValue.Consume();
    }

    private void OnEnable()
    {
        Assert.IsNull(instance);
        instance = this;
    }

    private void OnDisable()
    {
        Assert.IsTrue(instance == this);
        instance = null;
    }

    public void Restart()
    {
        timerDebt = 0f;
        countdownValue.Value = CountdownValue.Ten;
        countdownValue.Consume();
        PlayTickSFX();
        NewPass();
    }

    private void PlayTickSFX()
    {
        tickAudioSource.clip = tickSFX;
        tickAudioSource.pitch = 1f + CountdownValueUtil.Alpha(GetCountdownValue()) * maxTickPitchIncrease;
        tickAudioSource.Play();
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
        return (BPMRate)Math.Clamp(settings.bpmIndex, (int)BPMRate.Slow, (int)BPMRate.Fast) switch {
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

    public void DirectSetCountdownValue(CountdownValue value)
    {
        countdownValue.Value = value;
    }
}
