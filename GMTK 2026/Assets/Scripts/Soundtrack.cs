using System;
using UnityEngine;
using UnityEngine.Assertions;

public enum Song
{
    CountdownNormal,
    CountdownFast,
    CountdownSlow,
    Slomo
}

public class Soundtrack : MonoBehaviour
{
    private static Soundtrack instance;
    public static Soundtrack Instance => instance;

    [SerializeField] private AudioClip countdownNormal;
    [SerializeField] private AudioClip countdownFast;
    [SerializeField] private AudioClip countdownSlow;
    [SerializeField] private AudioClip slomo;

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

        DontDestroyOnLoad(instance);
    }

    public static void Play(Song song)
    {
        switch (song)
        {
            case Song.CountdownNormal:
                MusicManager.Instance.CrossFadeTrack(instance.countdownNormal);
                break;
            case Song.CountdownFast:
                MusicManager.Instance.CrossFadeTrack(instance.countdownFast);
                break;
            case Song.CountdownSlow:
                MusicManager.Instance.CrossFadeTrack(instance.countdownSlow);
                break;
            case Song.Slomo:
                MusicManager.Instance.CrossFadeTrack(instance.slomo, false);
                break;
            default:
                throw new NotImplementedException();
        }
    }
}
