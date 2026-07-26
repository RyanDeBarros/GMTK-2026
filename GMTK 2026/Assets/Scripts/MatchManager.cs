using System.Collections;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

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

    [Header("Game Phase")]
    [SerializeField] private int introCutsceneBPM = 35;
    [SerializeField] private RawImage fadeOverlay;
    [SerializeField] private float slomoDuration = 2.5f;
    [SerializeField] private int chooseActionBPM = 140;
    [SerializeField] private PauseController pauseController;

    [Header("Aiming")]
    [SerializeField] private float aimAngleMin = -8f;
    [SerializeField] private float aimAngleMax = 10f;
    [SerializeField] private float aimSpeed = 30f;
    [Header("Game Over UI")]
    [Header("Game Over UI")]
    [SerializeField] private GameObject gameOverPanel;
    [SerializeField] private Image winnerImage;
    [SerializeField] private Sprite redWinsSprite;
    [SerializeField] private Sprite blueWinsSprite;
    [SerializeField] private Sprite noWinnerSprite;
    [SerializeField] private TMP_Text resultText;
    [SerializeField] private CanvasGroup gameOverCanvasGroup;
    [SerializeField] private float fadeInDuration = 1f;
    [SerializeField] private Image blackFade;
    [SerializeField] private Image backgroundImage;
    [SerializeField] private float blackFadeDuration = 0.5f;
    [SerializeField] private float bgFadeDuration = 0.8f;

    private float timer = 0f;
    private Song countdownSong;

    private bool paused = false;
    public bool Paused => paused;

    private void Awake()
    {
        Assert.IsNull(instance);
        
        Assert.IsNotNull(fadeOverlay);
        Assert.IsNotNull(pauseController);
    }

    private void Start()
    {
        CheckCountdownMusicChange();
        Soundtrack.Play(countdownSong, true);

        StartCoroutine(IntroCutscene());

        IEnumerator Transition()
        {
            yield return new WaitForSecondsRealtime(60f / introCutsceneBPM);
            SetPhase(MatchPhase.Countdown);
        }

        StartCoroutine(Transition());
    }

    private void Update()
    {
        if (phase == MatchPhase.ChooseAction)
        {
            timer += Time.deltaTime;
            if (timer >= ChooseActionDuration())
            {
                if (PlayerController.Player1.ChosenAction == PlayerAction.Shoot || PlayerController.Player2.ChosenAction == PlayerAction.Shoot)
                    SetPhase(MatchPhase.Slomo);
                else
                    SetPhase(MatchPhase.Countdown);
            }
        }
        else if (phase == MatchPhase.Slomo)
        {
            timer += Time.deltaTime;
            if (timer >= slomoDuration || (!PlayerController.Player1.IsAiming() && !PlayerController.Player2.IsAiming()))
            {
                Soundtrack.Play(countdownSong);
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

    private IEnumerator IntroCutscene()
    {
        float duration = 60f / introCutsceneBPM;
        for (float t = 0f; t < duration; t += Time.deltaTime)
        {
            fadeOverlay.color = new(0f, 0f, 0f, 1f - t / duration);
            yield return null;
        }

        fadeOverlay.color = new(0f, 0f, 0f, 0f);
    }

    private float ChooseActionDuration()
    {
        return 60f / chooseActionBPM;
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
            PlayerController.Player1.StartChooseActionPhase();
            PlayerController.Player2.StartChooseActionPhase();
            timer = 0f;
        }

        if (phase == MatchPhase.Slomo)
        {
            float initialAngle = Mathf.Lerp(aimAngleMin, aimAngleMax, Random.value);
            bool ccw = Random.value < 0.5f;
            PlayerController.Player1.Pistol.SetInitialAngle(initialAngle, ccw, aimSpeed, aimAngleMin, aimAngleMax);
            PlayerController.Player2.Pistol.SetInitialAngle(initialAngle, ccw, aimSpeed, aimAngleMin, aimAngleMax);
            timer = 0f;
        }
    }

    public void CheckCountdownMusicChange()
    {
        int lives1 = PlayerController.Player1.Lives;
        int maxLives1 = PlayerController.Player1.MaxLives;
        int lives2 = PlayerController.Player2.Lives;
        int maxLives2 = PlayerController.Player2.MaxLives;

        if (lives1 == maxLives1 && lives2 == maxLives2)
            countdownSong = Song.CountdownSlow;
        else if (lives1 == 1 || lives2 == 1)
            countdownSong = Song.CountdownFast;
        else
            countdownSong = Song.CountdownNormal;
    }

    public void SyncCountdownMusic()
    {
        if (phase == MatchPhase.Countdown)
            Soundtrack.Play(countdownSong, true);
    }

    public void MatchComplete()
    {
        phase = MatchPhase.End;
        Soundtrack.Play(Song.MatchComplete);

        int lives1 = PlayerController.Player1.Lives;
        int lives2 = PlayerController.Player2.Lives;

        string reason = "What for..."; // change this to whatever fits your game

        if (lives1 == lives2)
        {
            winnerImage.sprite = noWinnerSprite;
            resultText.text = $"Nobody won... {reason}";
        }
        else if (lives1 > lives2)
        {
            winnerImage.sprite = redWinsSprite;
            resultText.text = $"Red won, at the cost of a life... {reason}";
        }
        else
        {
            winnerImage.sprite = blueWinsSprite;
            resultText.text = $"Blue won, at the cost of a life... {reason}";
        }

        gameOverPanel.SetActive(true);
        SetAlpha(blackFade, 0f);
        SetAlpha(backgroundImage, 0f);
        StartCoroutine(FadeToBlackThenBackground());
    }

    private void SetAlpha(Image img, float a)
{
    Color c = img.color;
    img.color = new Color(c.r, c.g, c.b, a);
}

    private IEnumerator FadeToBlackThenBackground()
    {
        // Step 1: fade to black
        float t = 0f;
        while (t < blackFadeDuration)
        {
            t += Time.unscaledDeltaTime;
            SetAlpha(blackFade, Mathf.Clamp01(t / blackFadeDuration));
            yield return null;
        }
        SetAlpha(blackFade, 1f);

        // Step 2: fade in background image on top of black
        t = 0f;
        while (t < bgFadeDuration)
        {
            t += Time.unscaledDeltaTime;
            SetAlpha(backgroundImage, Mathf.Clamp01(t / bgFadeDuration));
            yield return null;
        }
        SetAlpha(backgroundImage, 1f);
    }

    public void PlayAgain()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void BackToMainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenu");
    }

    public void Pause()
    {
        pauseController.Pause();
    }
}
