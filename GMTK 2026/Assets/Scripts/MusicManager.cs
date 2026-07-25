using System.Collections;
using UnityEngine;

public class MusicManager : MonoBehaviour
{
    private static MusicManager instance;
    public static MusicManager Instance => instance;

    private AudioSource as1;
    private AudioSource as2;
    private bool as1Front = true;

    [SerializeField] private float crossfadeDuration = 0.5f;

    private Coroutine fadeInRoutine;
    private Coroutine fadeOutRoutine;

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
        as2 = gameObject.AddComponent<AudioSource>();
    }

    private AudioSource FrontAudioSource()
    {
        return as1Front ? as1 : as2;
    }

    private AudioSource BackAudioSource()
    {
        return as1Front ? as2 : as1;
    }

    private void ToggleTracks()
    {
        as1Front = !as1Front;
    }

    public void CrossFadeTrack(AudioClip clip, bool loop = true, float volume = 1f)
    {
        ToggleTracks();

        StopCoroutine(fadeInRoutine);
        StopCoroutine(fadeOutRoutine);

        FrontAudioSource().clip = clip;
        FrontAudioSource().loop = loop;
        fadeInRoutine = StartCoroutine(FadeIn(FrontAudioSource(), volume));
        fadeOutRoutine = StartCoroutine(FadeOut(BackAudioSource()));
    }

    private IEnumerator FadeIn(AudioSource source, float toVolume = 1f)
    {
        source.volume = 0f;
        for (float t = 0f; t < crossfadeDuration; t += Time.deltaTime)
        {
            yield return null;
            source.volume = Mathf.Lerp(0f, toVolume, t / crossfadeDuration);
        }

        source.volume = toVolume;
    }

    private IEnumerator FadeOut(AudioSource source)
    {
        float fromVolume = source.volume;
        for (float t = 0f; t < crossfadeDuration; t += Time.deltaTime)
        {
            source.volume = Mathf.Lerp(fromVolume, 0f, t / crossfadeDuration);
            yield return null;
        }

        source.volume = 0f;
    }
}
