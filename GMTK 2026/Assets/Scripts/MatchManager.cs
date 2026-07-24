using UnityEngine;
using UnityEngine.Assertions;

public enum MatchPhase
{
    Intro,
    Countdown,
    Slomo,
    End
}

public class MatchManager : MonoBehaviour
{
    private static MatchManager instance;
    public static MatchManager Instance => instance;

    private MatchPhase phase = MatchPhase.Intro;
    public MatchPhase Phase => phase;

    private void Awake()
    {
        Assert.IsNull(instance);
    }

    private void Start()
    {
        // TODO intro cutscene first
        phase = MatchPhase.Countdown;
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

        CountdownDisplay.Instance.enabled = phase == MatchPhase.Countdown || phase == MatchPhase.Slomo;
    }
}
