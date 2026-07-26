using System;
using UnityEngine;
using UnityEngine.Assertions;

public enum Song
{
    CountdownNormal,
    CountdownFast,
    CountdownSlow,
    Slomo,
    MainMenu,
    MatchComplete
}

public class Soundtrack : MonoBehaviour
{
    private static Soundtrack instance;
    public static Soundtrack Instance => instance;

    [SerializeField] private AudioClip countdownNormal;
    [SerializeField] private AudioClip countdownFast;
    [SerializeField] private AudioClip countdownSlow;
    [SerializeField] private AudioClip slomo;
    [SerializeField] private AudioClip mainMenu;
    [SerializeField] private AudioClip matchComplete;

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;

        Assert.IsNotNull(countdownNormal);
        Assert.IsNotNull(countdownFast);
        Assert.IsNotNull(countdownSlow);
        Assert.IsNotNull(slomo);
        Assert.IsNotNull(mainMenu);
        Assert.IsNotNull(matchComplete);

        DontDestroyOnLoad(instance);
    }

    public static void Play(Song song, bool direct = false)
    {
        switch (song)
        {
            case Song.CountdownNormal:
                MusicManager.Instance.CrossFadeTrack(instance.countdownNormal, direct, new ClipInfo() { restart = false, bpm = 70 });
                break;

            case Song.CountdownFast:
                MusicManager.Instance.CrossFadeTrack(instance.countdownFast, direct, new ClipInfo() { restart = false, bpm = 140 });
                break;

            case Song.CountdownSlow:
                MusicManager.Instance.CrossFadeTrack(instance.countdownSlow, direct, new ClipInfo() { restart = false, bpm = 35 });
                break;

            case Song.Slomo:
                MusicManager.Instance.CrossFadeTrack(instance.slomo, direct, new ClipInfo() { loop = false });
                break;

            case Song.MainMenu:
                MusicManager.Instance.CrossFadeTrack(instance.mainMenu, direct, new ClipInfo());
                break;

            case Song.MatchComplete:
                MusicManager.Instance.CrossFadeTrack(instance.matchComplete, direct, new ClipInfo());
                break;

            default:
                throw new NotImplementedException();
        }
    }
}
