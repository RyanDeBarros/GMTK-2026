using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClipInfo
{
    public int bpm = 0;
    public bool loop = true;
    public bool restart = true;
    public float volume = 1f;
}

public class MusicManager : MonoBehaviour
{
    private static MusicManager instance;
    public static MusicManager Instance => instance;

    private AudioSource as1;
    private ClipInfo ci1;
    private AudioSource as2;
    private ClipInfo ci2;
    private bool as1Front = true;

    [SerializeField] private float crossfadeDuration = 0.5f;

    private Coroutine fadeInRoutine;
    private Coroutine fadeOutRoutine;

    private class TrackCacheInfo
    {
        public ClipInfo clipInfo;
        public float time;
    }

    private readonly Dictionary<AudioClip, TrackCacheInfo> trackCache = new();

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;

        DontDestroyOnLoad(instance);

        as1 = gameObject.AddComponent<AudioSource>();
        as1.playOnAwake = false;
        as2 = gameObject.AddComponent<AudioSource>();
        as2.playOnAwake = false;
    }

    private void Update()
    {
        CacheTrack(FrontAudioSource(), FrontClipInfo());
        CacheTrack(BackAudioSource(), BackClipInfo());
    }

    public AudioSource FrontAudioSource()
    {
        return as1Front ? as1 : as2;
    }

    public AudioSource BackAudioSource()
    {
        return as1Front ? as2 : as1;
    }

    public ClipInfo FrontClipInfo()
    {
        return as1Front ? ci1 : ci2;
    }

    public void SetFrontClipInfo(ClipInfo info)
    {
        if (as1Front)
            ci1 = info;
        else
            ci2 = info;
    }

    public ClipInfo BackClipInfo()
    {
        return as1Front ? ci2 : ci1;
    }

    public void SetBackClipInfo(ClipInfo info)
    {
        if (as1Front)
            ci2 = info;
        else
            ci1 = info;
    }

    private void ToggleTracks()
    {
        as1Front = !as1Front;
    }

    public void CrossFadeTrack(AudioClip clip, bool direct, ClipInfo info)
    {
        if (FrontAudioSource().clip == clip)
            return;

        ToggleTracks();

        if (fadeInRoutine != null) StopCoroutine(fadeInRoutine);
        if (fadeOutRoutine != null) StopCoroutine(fadeOutRoutine);

        FrontAudioSource().clip = clip;
        SetFrontClipInfo(info);
        FrontAudioSource().loop = info.loop;

        if (trackCache.TryGetValue(clip, out TrackCacheInfo cache) && cache.clipInfo.loop && !cache.clipInfo.restart)
        {
            if (cache.clipInfo.bpm > 0)
            {
                float beatDuration = 60f / cache.clipInfo.bpm;
                FrontAudioSource().time = Mathf.Floor(cache.time / beatDuration) * beatDuration;
            }
            else
                FrontAudioSource().time = cache.time;
        }

        FrontAudioSource().Play();
        
        if (direct)
            FrontAudioSource().volume = info.volume;
        else
            fadeInRoutine = StartCoroutine(FadeIn(FrontAudioSource(), info.volume));

        fadeOutRoutine = StartCoroutine(FadeOut(BackAudioSource()));
    }

    private IEnumerator FadeIn(AudioSource source, float toVolume = 1f)
    {
        source.volume = 0f;
        for (float t = 0f; t < crossfadeDuration; t += Time.deltaTime)
        {
            yield return null;
            float a = Mathf.Pow(t / crossfadeDuration, 0.5f);
            source.volume = Mathf.Lerp(0f, toVolume, a);
        }

        source.volume = toVolume;
    }

    private IEnumerator FadeOut(AudioSource source)
    {
        float fromVolume = source.volume;
        for (float t = 0f; t < crossfadeDuration; t += Time.deltaTime)
        {
            float a = Mathf.Pow(t / crossfadeDuration, 2f);
            source.volume = Mathf.Lerp(fromVolume, 0f, a);
            yield return null;
        }

        source.volume = 0f;
        source.Stop();
    }

    private void CacheTrack(AudioSource source, ClipInfo clipInfo)
    {
        if (source.clip != null && clipInfo.loop && source.isPlaying)
            trackCache[source.clip] = new TrackCacheInfo() { clipInfo = clipInfo, time = source.time };
    }
}
