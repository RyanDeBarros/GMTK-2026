using NUnit.Framework.Constraints;
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

    [SerializeField] private float chooseActionDuration = 0.5f;
    [SerializeField] private float slomoDuration = 2.5f;

    private float timer = 0f;

    private void Awake()
    {
        Assert.IsNull(instance);
    }

    private void Start()
    {
        // TODO intro cutscene first
        SetPhase(MatchPhase.Countdown);
    }

    private void Update()
    {
        if (phase == MatchPhase.ChooseAction)
        {
            timer += Time.deltaTime;
            if (timer >= chooseActionDuration)
                SetPhase(MatchPhase.Slomo);
        }
        else if (phase == MatchPhase.Slomo)
        {
            timer += Time.deltaTime;
            if (timer >= slomoDuration || (PlayerController.Player1.ChosenAction != PlayerAction.Shoot && PlayerController.Player2.ChosenAction != PlayerAction.Shoot))
            {
                // TODO fade out slomo music track
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
        {
            // TODO fade in slomo music track
            timer = 0f;
        }

        if (phase == MatchPhase.Slomo)
            timer = 0f;
    }
}
