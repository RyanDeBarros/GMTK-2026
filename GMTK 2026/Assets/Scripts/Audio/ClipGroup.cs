using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class ClipGroup : MonoBehaviour
{
    [SerializeField] private List<AudioClip> clips;

    private void Awake()
    {
        Assert.IsTrue(clips.Count > 0);
    }

    public AudioClip Poll()
    {
        return clips[Random.Range(0, clips.Count - 1)];
    }
}
