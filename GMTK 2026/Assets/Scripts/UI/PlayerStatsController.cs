using TMPro;
using UnityEngine;
using UnityEngine.Assertions;

public class PlayerStatsController : MonoBehaviour
{
    [SerializeField] private PlayerController controller;
    [SerializeField] private TextMeshProUGUI ammoText;
    [SerializeField] private GameObject ammoRoot;
    [SerializeField] private TextMeshProUGUI dodgeText;
    [SerializeField] private GameObject dodgeRoot;
    [SerializeField] private ProgressBar healthBar;

    [SerializeField] private GameObject shootPrompt;
    [SerializeField] private GameObject reloadPrompt;
    [SerializeField] private GameObject dodgePrompt;
    [SerializeField] private GameObject slomoPrompt;

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

        // TODO use tint overlay instead to disable action UI
        shootPrompt.SetActive(controller.CanShoot() && !MatchManager.Instance.Paused);
        reloadPrompt.SetActive(controller.CanReload() && !MatchManager.Instance.Paused);
        dodgePrompt.SetActive(controller.CanDodge() && !MatchManager.Instance.Paused);

        slomoPrompt.SetActive(MatchManager.Instance.Phase == MatchPhase.Slomo && controller.ChosenAction == PlayerAction.Shoot);
    }
}
