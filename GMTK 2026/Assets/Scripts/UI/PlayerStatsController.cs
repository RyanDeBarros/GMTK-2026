using TMPro;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;

public class PlayerStatsController : MonoBehaviour
{
    [SerializeField] private PlayerController controller;
    [SerializeField] private TextMeshProUGUI ammoText;
    [SerializeField] private GameObject ammoRoot;
    [SerializeField] private TextMeshProUGUI dodgeText;
    [SerializeField] private GameObject dodgeRoot;
    [SerializeField] private ProgressBar healthBar;

    [SerializeField] private Image shootPrompt;
    [SerializeField] private Image reloadPrompt;
    [SerializeField] private Image dodgePrompt;
    [SerializeField] private GameObject slomoPrompt;

    [SerializeField] private Sprite activeBg;
    [SerializeField] private Sprite inactiveBg;

    private void Awake()
    {
        Assert.IsNotNull(controller);
        Assert.IsNotNull(ammoText);
        Assert.IsNotNull(ammoRoot);
        Assert.IsNotNull(dodgeText);
        Assert.IsNotNull(dodgeRoot);
        Assert.IsNotNull(healthBar);

        Assert.IsNotNull(shootPrompt);
        Assert.IsNotNull(reloadPrompt);
        Assert.IsNotNull(dodgePrompt);
        Assert.IsNotNull(slomoPrompt);

        Assert.IsNotNull(activeBg);
        Assert.IsNotNull(inactiveBg);
    }

    private void Update()
    {
        ammoText.text = $"{controller.Ammo} / {controller.MaxAmmo}";
        dodgeText.text = controller.DodgeCooldown > 0 ? $"{controller.DodgeCooldown}" : "";
        healthBar.SetValue(controller.Lives / (float)controller.MaxLives);

        bool showHUD = (MatchManager.Instance.Phase == MatchPhase.Countdown || MatchManager.Instance.Phase == MatchPhase.ChooseAction || MatchManager.Instance.Phase == MatchPhase.Intro)
            && !MatchManager.Instance.Paused;
        ammoRoot.SetActive(showHUD && ammoText.text.Length > 0);
        dodgeRoot.SetActive(showHUD && dodgeText.text.Length > 0);
        healthBar.gameObject.SetActive(showHUD);

        if (controller.CanSelectAction())
        {
            shootPrompt.gameObject.SetActive(true);
            reloadPrompt.gameObject.SetActive(true);
            dodgePrompt.gameObject.SetActive(true);

            shootPrompt.sprite = controller.CanShoot() && !MatchManager.Instance.Paused ? activeBg : inactiveBg;
            reloadPrompt.sprite = controller.CanReload() && !MatchManager.Instance.Paused ? activeBg : inactiveBg;
            dodgePrompt.sprite = controller.CanDodge() && !MatchManager.Instance.Paused ? activeBg : inactiveBg;

            shootPrompt.GetComponentInChildren<TextMeshProUGUI>().color = controller.CanShoot() && !MatchManager.Instance.Paused ? Color.white : Color.black;
            reloadPrompt.GetComponentInChildren<TextMeshProUGUI>().color = controller.CanReload() && !MatchManager.Instance.Paused ? Color.white : Color.black;
            dodgePrompt.GetComponentInChildren<TextMeshProUGUI>().color = controller.CanDodge() && !MatchManager.Instance.Paused ? Color.white : Color.black;
        }
        else
        {
            shootPrompt.gameObject.SetActive(false);
            reloadPrompt.gameObject.SetActive(false);
            dodgePrompt.gameObject.SetActive(false);
        }

        slomoPrompt.SetActive(MatchManager.Instance.Phase == MatchPhase.Slomo && controller.ChosenAction == PlayerAction.Shoot);
    }
}
