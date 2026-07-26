using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void PlayGame()
    {
        SceneManager.LoadScene("Game");
        Soundtrack.Play(Song.MainMenu);
    }
    public void QuitGame()
    {
        Application.Quit();
    }
}
