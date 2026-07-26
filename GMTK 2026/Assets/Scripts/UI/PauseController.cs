using UnityEngine;
using UnityEngine.SceneManagement;

// TODO make sure to fade out current music track, then fade that back in after resuming
public class PauseController : MonoBehaviour
{
    private void Awake()
    {
        gameObject.SetActive(false);
    }

    private void OnEnable()
    {
        Time.timeScale = 0f;
    }

    private void OnDisable()
    {
        Time.timeScale = 1f;
    }

    public void Resume()
    {
        gameObject.SetActive(false);
    }

    public void MainMenu()
    {
        gameObject.SetActive(false);
        SceneManager.LoadScene("MainMenu");
    }
}
