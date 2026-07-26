using UnityEngine;
using UnityEngine.Assertions;

public enum MatchPhase
{
    Intro,
    Countdown,
    ChooseAction,
    Slomo,
    End
}

public class MatchManager : MonoBehaviour
{
    private static MatchManager instance;
    public static MatchManager Instance => instance;

    private MatchPhase phase = MatchPhase.Intro;
    public MatchPhase Phase => phase;

    [Header("Game Phase")]
    [SerializeField] private float slomoDuration = 2.5f;
    [SerializeField] private int chooseActionBPM = 140;

    [Header("Aiming")]
    [SerializeField] private float aimAngleMin = -8f;
    [SerializeField] private float aimAngleMax = 10f;
    [SerializeField] private float aimSpeed = 30f;

    private float timer = 0f;
    private Song countdownSong;

    private void Awake()
    {
        Assert.IsNull(instance);
    }

    private void Start()
    {
        CheckCountdownMusicChange();
        Soundtrack.Play(countdownSong, true);

        // TODO intro cutscene first

        SetPhase(MatchPhase.Countdown);
    }

    private void Update()
    {
        if (phase == MatchPhase.ChooseAction)
        {
            timer += Time.deltaTime;
            if (timer >= ChooseActionDuration())
            {
                if (PlayerController.Player1.ChosenAction == PlayerAction.Shoot || PlayerController.Player2.ChosenAction == PlayerAction.Shoot)
                    SetPhase(MatchPhase.Slomo);
                else
                    SetPhase(MatchPhase.Countdown);
            }
        }
        else if (phase == MatchPhase.Slomo)
        {
            timer += Time.deltaTime;
            if (timer >= slomoDuration || (!PlayerController.Player1.IsAiming() && !PlayerController.Player2.IsAiming()))
            {
                Soundtrack.Play(countdownSong);
                SetPhase(MatchPhase.Countdown);
            }
        }
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

    private float ChooseActionDuration()
    {
        return 60f / chooseActionBPM;
    }

    public void SetPhase(MatchPhase phase)
    {
        this.phase = phase;

        if (phase == MatchPhase.Countdown)
        {
            PlayerController.Player1.StartCountdownPhase();
            PlayerController.Player2.StartCountdownPhase();
            BaseCountdownTimer.Instance.Restart();
        }

        if (phase == MatchPhase.ChooseAction)
            timer = 0f;

        if (phase == MatchPhase.Slomo)
        {
            float initialAngle = Mathf.Lerp(aimAngleMin, aimAngleMax, Random.value);
            bool ccw = Random.value < 0.5f;
            PlayerController.Player1.Pistol.SetInitialAngle(initialAngle, ccw, aimSpeed, aimAngleMin, aimAngleMax);
            PlayerController.Player2.Pistol.SetInitialAngle(initialAngle, ccw, aimSpeed, aimAngleMin, aimAngleMax);
            timer = 0f;
        }
    }

    public void CheckCountdownMusicChange()
    {
        int lives1 = PlayerController.Player1.Lives;
        int maxLives1 = PlayerController.Player1.MaxLives;
        int lives2 = PlayerController.Player2.Lives;
        int maxLives2 = PlayerController.Player2.MaxLives;

        if (lives1 == maxLives1 && lives2 == maxLives2)
            countdownSong = Song.CountdownSlow;
        else if (lives1 == 1 || lives2 == 1)
            countdownSong = Song.CountdownFast;
        else
            countdownSong = Song.CountdownNormal;
    }

    public void SyncCountdownMusic()
    {
        if (phase == MatchPhase.Countdown)
            Soundtrack.Play(countdownSong, true);
    }
}
