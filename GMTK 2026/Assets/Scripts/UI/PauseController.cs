using System.Collections;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.SceneManagement;

public class PauseController : MonoBehaviour
{
    [SerializeField] private GameObject background;
    [SerializeField] private GameObject foreground;
    [SerializeField] private int resumeDelayBPM = 140;
    private float pausedTime = 0f;
    private bool resuming = false;

    private void Awake()
    {
        Assert.IsNotNull(background);
        Assert.IsNotNull(foreground);

        background.SetActive(false);
        foreground.SetActive(false);
    }

    public void Pause()
    {
        Time.timeScale = 0f;
        pausedTime = Time.realtimeSinceStartup;
        resuming = false;
        background.SetActive(true);
        foreground.SetActive(true);
    }

    public void Resume()
    {
        if (resuming)
            return;

        foreground.SetActive(false);
        resuming = true;
        float timeElapsed = Time.realtimeSinceStartup - pausedTime;
        float beatLength = 60f / resumeDelayBPM;
        float offset = Mathf.Repeat(timeElapsed, beatLength);
        float delay = offset > 0f ? beatLength - offset : 0f;

        IEnumerator Resume()
        {
            yield return new WaitForSecondsRealtime(delay);
            background.SetActive(false);
            Time.timeScale = 1f;
        }

        StartCoroutine(Resume());
    }

    public void MainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenu");
    }
}
